using MongoDB.Driver;
using MTT.MotoWorker;

IHost host = Host.CreateDefaultBuilder(args)
	.ConfigureServices((context, services) =>
	{
		services.AddHostedService<Worker>();

		var rabbitMqConfig = context.Configuration.GetSection("RabbitMQ").Get<RabbitMqConfig>();
		var mongoDBConfig = context.Configuration.GetSection("MongoDB").Get<MongoDBConfig>();

		services.AddSingleton(rabbitMqConfig);
		services.AddSingleton(sp => new MongoClient(mongoDBConfig.ConnectionString));
		services.AddSingleton(sp => sp.GetRequiredService<MongoClient>().GetDatabase(mongoDBConfig.Database));

	})
	.Build();

await host.RunAsync();
