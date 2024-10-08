using MongoDB.Driver;
using MTT.Domain.Rental.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace MTT.MotoWorker
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IMongoDatabase _database;
		private readonly RabbitMqConfig _rabbitMqConfig;
		private IModel _channel;

		public Worker(ILogger<Worker> logger, IMongoDatabase database, RabbitMqConfig rabbitMqConfig)
		{
			_logger = logger;
			_database = database;
			_rabbitMqConfig = rabbitMqConfig;
			InitializeRabbitMQ(rabbitMqConfig);
		}

		private void InitializeRabbitMQ(RabbitMqConfig rabbitMqConfig)
		{
			var factory = new ConnectionFactory() {
				HostName = rabbitMqConfig.Hostname,
                UserName = rabbitMqConfig.Username,
                Password = rabbitMqConfig.Password
            };
			var connection = factory.CreateConnection();
			_channel = connection.CreateModel();
			_channel.QueueDeclare(queue: rabbitMqConfig.QueueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
		}

		protected override Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var consumer = new EventingBasicConsumer(_channel);

			consumer.Received += async (model, ea) =>
			{
				var body = ea.Body.ToArray();
				var message = Encoding.UTF8.GetString(body);
				var motoEvent = JsonSerializer.Deserialize<MotoAddedOrUpdatedWithTag2024IntegrationEvent>(message);
				await HandleMessageAsync(motoEvent);
			};

			_channel.BasicConsume(queue: _rabbitMqConfig.QueueName, autoAck: true, consumer: consumer);

			return Task.CompletedTask;
		}

		private async Task HandleMessageAsync(MotoAddedOrUpdatedWithTag2024IntegrationEvent motoEvent)
		{
			var collection = _database.GetCollection<MotoAddedOrUpdatedWithTag2024IntegrationEvent>("MotoEvents");
			await collection.InsertOneAsync(motoEvent);
			_logger.LogInformation($"Message received and saved: {motoEvent.Tag} at {motoEvent.OccurredOn}");
		}

		public override void Dispose()
		{
			_channel.Close();
			base.Dispose();
		}
	}
}
