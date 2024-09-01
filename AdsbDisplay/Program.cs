using AdsbDisplay;
using AdsbDisplay.ADSB;
using AdsbDisplay.ADSB.Feeders;
using AdsbDisplay.ATIS;
using AdsbDisplay.Display;
using AdsbDisplay.Display.Interfaces;

//Tried to find a way to find the local feeder automatically, and the pixoo. But haven't found a good way to pass it into the dependency injection.
//Config config = await Startup.Start();
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddSingleton(typeof(CustomLogger<>));
        services.AddTransient<IMain, Main>();
        services.AddTransient<IADSB, ADSB>();
        services.AddSingleton<ILocalFeeder, LocalFeeder>();
        services.AddTransient<IAPIFeeder, APIFeeder>();
        services.AddTransient<IDisplay, Display>();
        services.AddSingleton<IPixoo, Pixoo>();
        services.AddSingleton<IPixooTest, PixooTest>();
        services.AddSingleton<IAudio, Audio>();
        services.AddSingleton<IAtis, ATIS>();
        services.AddHttpClient();
    })
    .Build();

host.Run();
