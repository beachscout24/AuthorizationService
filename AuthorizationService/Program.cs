using AuthorizationService.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Configuration;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddDbContext<DataContext>((serviceProvider, dbContextBuilder) =>
        {
            var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            var connectionStringName = httpContextAccessor.HttpContext.Request.Headers["db"].First();
            var connectionString = Environment.GetEnvironmentVariable(connectionStringName);
            dbContextBuilder.UseSqlServer(connectionString);
        });
    })
    .Build();
host.Run();