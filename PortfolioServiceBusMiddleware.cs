using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Portfolio.AspNetCore.Middleware.Services;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Portfolio.AspNetCore.Middleware
{
    public class PortfolioServiceBusMiddleware
    {
        private readonly RequestDelegate _next;

        public PortfolioServiceBusMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IAzureServiceBusSender sender, IOptions<Configuration.PortfolioQueueNames> portfolioConfig)
        {
            string targetQueueName = null;

            if (httpContext.Request.Method == System.Net.Http.HttpMethod.Post.Method)
            {
                targetQueueName = portfolioConfig.Value.PortfolioCreated;
            }
            else if (httpContext.Request.Method == System.Net.Http.HttpMethod.Put.Method)
            {
                targetQueueName = portfolioConfig.Value.PortfolioUpdated;
            }
            else if (httpContext.Request.Method == System.Net.Http.HttpMethod.Delete.Method)
            {
                targetQueueName = portfolioConfig.Value.PortfolioDeleted;
            }

            if (!String.IsNullOrWhiteSpace(targetQueueName))
            {
                httpContext.Request.EnableBuffering();                
                var originalPosition = httpContext.Request.Body.Position;

                using (var reader = new StreamReader(
                    httpContext.Request.Body,
                    encoding: Encoding.UTF8,
                    detectEncodingFromByteOrderMarks: false,
                    bufferSize: 1024,
                    leaveOpen: true))
                {
                    var body = await reader.ReadToEndAsync();
                    await sender.SendMessageAsync(body, targetQueueName);
                    httpContext.Request.Body.Position = originalPosition;
                }
            }

            await _next(httpContext);
        }
    }

}
