using System.Threading.Tasks;

namespace Portfolio.AspNetCore.Middleware.Services
{
    public interface IAzureServiceBusSender
    {
        Task SendMessageAsync(string content, string queueName);
    }
}
