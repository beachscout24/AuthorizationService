using AuthorizationService.Data;
using AuthorizationService.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SBSender.Services;
using System.Text.Json;

namespace AuthorizationService
{
    public class AuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IConfiguration _config;

        public AuthService(ILogger<AuthService> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [Function(nameof(AuthService))]
        public async Task Run(
            [ServiceBusTrigger("%registerqueue%", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
        {
            try
            {
                User user = JsonSerializer.Deserialize<User>(message.Body)!;
                _logger.LogInformation(user.FirstName);
                _logger.LogInformation("FirstName: {firstName}", user.FirstName);
                _logger.LogInformation("LastName: {lastName}", user.LastName);
                _logger.LogInformation("Address: {address}", user.Address);
                _logger.LogInformation("City: {city}", user.City);
                _logger.LogInformation("State: {state}", user.State);
                _logger.LogInformation("PostalCode: {postalCode}", user.PostalCode);
                _logger.LogInformation("Email: {email}", user.Email);
                _logger.LogInformation("Username: {username}", user.Username);
                _logger.LogInformation("Password: {password}", user.Password);
                _logger.LogInformation("Db: {db}", user.Db);
                // Write message to database
                DataOperations operations = new DataOperations(user.Db, _logger);
                operations.SaveMessage(user);
                // Complete the message
                await messageActions.CompleteMessageAsync(message);
            }
            catch (Exception exc)
            {

                _logger.LogError($"Message could not be consumed. Moving messsge to rejected message endpoint. {exc.Message}");
                IQueueService service = new QueueService(_config);
                await service.SendMessageAsync(message.Body, "rejected.message.endpoint");
            }
        }
    }
}
