namespace MTT.Infra.Broker.RabbitMqBroker
{
	public class RabbitMqConfig
	{
		public string Hostname { get; set; }
		public string QueueName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    };
}
