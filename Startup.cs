using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Portfolio.AspNetCore.Middleware.Services;
using System.Threading.Tasks;

namespace Portfolio.AspNetCore.Middleware
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.Configure<Configuration.PortfolioQueueNames>(Configuration.GetSection("PortfolioQueueNames"));
            services.AddTransient<IAzureServiceBusSender, AzureServiceBusSender>();

            //TODO: replace once corefx JSON bugs are fixed ~3.1
            services.AddMvc().AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                context.Response.OnStarting(() =>
                {
                    context.Response.Headers.Add("x-webapp-environment", System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));
                    context.Response.Headers.Add("x-webapp-version", System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());
                    return Task.FromResult(0);
                });

                await next();
            });

            app.UseWhen(context => context.Request.Path.StartsWithSegments("/api/portfolio"), appBuilder =>
            {
                appBuilder.UseMiddleware<PortfolioServiceBusMiddleware>();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
