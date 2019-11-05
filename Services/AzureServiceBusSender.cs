using Microsoft.Extensions.Configuration;
using Microsoft.Azure.ServiceBus;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Portfolio.AspNetCore.Middleware.Services
{
    public class AzureServiceBusSender : IAzureServiceBusSender
    {
        private readonly string _connectionString;

        public AzureServiceBusSender(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("AzureServiceBus");
        }

        public async Task SendMessageAsync(string content, string queueName)
        {
            var queueClient = new QueueClient(_connectionString, queueName);

            Message message = new Message();
            message.Body = Encoding.UTF8.GetBytes(content);

            await queueClient.SendAsync(message);
        }
    }
}
