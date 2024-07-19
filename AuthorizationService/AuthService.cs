using AuthorizationService.Models;
using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AuthorizationService
{
    public class AuthService
    {
        private readonly ILogger<AuthService> _logger;

        public AuthService(ILogger<AuthService> logger)
        {
            _logger = logger;
        }

        [Function(nameof(AuthService))]
        public async Task Run(
            [ServiceBusTrigger("%registerqueue%", Connection = "ServiceBusConnection")]
            ServiceBusReceivedMessage message,
            ServiceBusMessageActions messageActions)
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

            // Complete the message
            await messageActions.CompleteMessageAsync(message);
        }
    }
}
