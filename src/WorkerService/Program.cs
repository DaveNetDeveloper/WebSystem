using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using WorkerService.Configuration;
using WorkerService.Jobs; 

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        IConfiguration configuration = context.Configuration;
        services.Configure<JobsConfiguration>(configuration.GetSection("JobsConfiguration"));
        services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration")); 
        //services.Configure<SegmentsConfiguration>(configuration.GetSection("SegmentsConfiguration")); 

        services.AddApplication();
        services.AddInfrastructure();
        services.AddHostedService<CheckUsers>();
        services.AddHostedService<UpdateUserSegments>();

    }).Build();

await host.RunAsync();