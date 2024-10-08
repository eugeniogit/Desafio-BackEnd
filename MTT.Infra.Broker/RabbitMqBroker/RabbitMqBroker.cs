using MTT.Domain.Shared;
using RabbitMQ.Client;
using System.Text;

namespace MTT.Infra.Broker.RabbitMqBroker
{
    public class RabbitMqBroker : IMessageBroker
    {
        private readonly RabbitMqConfig _config;

        public RabbitMqBroker(RabbitMqConfig config)
        {
			_config = config;
        }
        public void SendMessage(string message)
        {
            var factory = new ConnectionFactory()
            {
                HostName = _config.Hostname,
                UserName = _config.Username,
                Password = _config.Password
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: _config.QueueName,
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "",
                                     routingKey: _config.QueueName,
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
