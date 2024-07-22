using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace SBSender.Services
{
    public class QueueService : IQueueService
    {
        private readonly IConfiguration _configuration;

        public QueueService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMessageAsync<T>(T serviceBusMessage, string queueName)
        {
            var client = new ServiceBusClient(_configuration.GetConnectionString("AzureServiceBus"));
            var sender = client.CreateSender(queueName);

            try
            {
                var messageBody = JsonSerializer.Serialize(serviceBusMessage);

                var message = new ServiceBusMessage(Encoding.UTF8.GetBytes(messageBody));

                await sender.SendMessageAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                await sender.DisposeAsync();
                await client.DisposeAsync();
            }
        }
    }
}
