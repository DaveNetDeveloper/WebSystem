using Application.DependencyInjection;
using Infrastructure.DependencyInjection;
using WorkerService.Configuration;
using WorkerService.Jobs;
using Domain.Entities;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        IConfiguration configuration = context.Configuration;
        services.Configure<JobsConfiguration>(configuration.GetSection("JobsConfiguration"));
        services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration"));

        services.AddApplication();
        services.AddInfrastructure(string.Empty);
        services.AddHostedService<ReminderForUnsubscribers>();
        services.AddHostedService<UpdateUserSegments>();
        services.AddHostedService<ExecuteCampaigns>();

    }).Build();

await host.RunAsync();