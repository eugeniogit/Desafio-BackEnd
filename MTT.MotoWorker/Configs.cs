namespace MTT.MotoWorker
{
	public class MongoDBConfig
	{
		public string ConnectionString { get; set; }
		public string Database { get; set; }
	}

	public class RabbitMqConfig
	{
		public string Hostname { get; set; }
		public string QueueName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    };
}
