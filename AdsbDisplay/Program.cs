using AdsbDisplay;


IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton(typeof(CustomLogger<>));
        services.AddTransient<IMain, Main>();
    })
    .Build();

host.Run();
