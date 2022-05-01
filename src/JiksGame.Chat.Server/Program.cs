using JiksGame.Chat.Server.Factories;

Console.WriteLine("Hello, World!");
var hostBuilder = AppServiceProviderFactory.CreateHostBuilder(args);
var host = hostBuilder.Build();
await host.StartAsync();

Console.WriteLine("Please Enter, will be stop program");
Console.ReadLine();
await host.StopAsync();

//while (true)
//{
//    // Check all services stop.    
//}

Console.WriteLine("Applcation stoped.");

