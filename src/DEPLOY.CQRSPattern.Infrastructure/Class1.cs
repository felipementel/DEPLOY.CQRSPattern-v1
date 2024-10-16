using System.Text.Json;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using DEPLOY.CQRSPattern.Infrastructure.MessageBroker.Configuration;
using Microsoft.Extensions.Options;

namespace DEPLOY.CQRSPattern.Infrastructure.MessageBroker.Configuration
{
    public class AzureServiceBusSettings
    {
        public string ConnectionString { get; set; }
        public string QueueName { get; set; }
    }
}


namespace DEPLOY.CQRSPattern.Infrastructure.MessageBroker.Services
{
    public class AzureServiceBusService
    {
        private readonly ServiceBusClient _client;
        private readonly ServiceBusSender _sender;
        private readonly ServiceBusProcessor _processor;

        public AzureServiceBusService(IOptions<AzureServiceBusSettings> options)
        {
            var settings = options.Value;
            _client = new ServiceBusClient(settings.ConnectionString);
            _sender = _client.CreateSender(settings.QueueName);
            _processor = _client.CreateProcessor(settings.QueueName, new ServiceBusProcessorOptions());
            _processor.ProcessMessageAsync += MessageHandler;
            _processor.ProcessErrorAsync += ErrorHandler;
        }

        public async Task SendMessageAsync<T>(T message)
        {
            var messageBody = JsonSerializer.Serialize(message);
            var serviceBusMessage = new ServiceBusMessage(messageBody);
            await _sender.SendMessageAsync(serviceBusMessage);
        }

        public async Task StartProcessingAsync() => await _processor.StartProcessingAsync();
        public async Task StopProcessingAsync() => await _processor.StopProcessingAsync();

        private async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var body = args.Message.Body.ToString();
            // Handle message here
            await args.CompleteMessageAsync(args.Message);
        }

        private Task ErrorHandler(ProcessErrorEventArgs args)
        {
            // Handle error here
            return Task.CompletedTask;
        }
    }
}