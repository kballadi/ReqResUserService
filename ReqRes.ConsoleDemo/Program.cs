using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using ReqRes.Core.Interfaces;
using ReqRes.Core.Services;

var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddMemoryCache();
        services.AddHttpClient<IExternalUserService, ExternalUserService>(client =>
        {
            client.BaseAddress = new Uri("https://reqres.in/api/");
            client.Timeout = TimeSpan.FromSeconds(10);
        })
        .AddPolicyHandler(HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(2)));

        services.AddLogging(cfg => cfg.AddConsole());
    })
    .Build();

var service = host.Services.GetRequiredService<IExternalUserService>();

Console.WriteLine("Fetching user with ID 2...");
var user = await service.GetUserByIdAsync(2);
Console.WriteLine(user is null ? "User not found." : $"{user.FirstName} {user.LastName}");

Console.WriteLine("Fetching all users...");
var users = await service.GetAllUsersAsync();
foreach (var u in users)
{
    Console.WriteLine($"{u.Id}: {u.FirstName} {u.LastName}");
}
