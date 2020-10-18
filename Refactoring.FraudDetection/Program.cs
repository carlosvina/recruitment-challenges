using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using System;
using System.IO;

namespace Refactoring.FraudDetection
{
    public class Program
    {
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            IConfiguration config = 
                new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .Build();

            serviceCollection.AddSingleton(config);
            serviceCollection.AddSingleton<IAppSettings, AppSettings>();
            serviceCollection.AddLogging(configure => configure.AddConsole())
                .AddTransient<FraudRadar>();
            serviceCollection.AddLogging(configure => configure.AddConsole())
                .AddTransient<FilesOrderRepository>();

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var filesRepository = serviceProvider.GetService<FilesOrderRepository>();
            var fraudRadar = serviceProvider.GetService<FraudRadar>();
        
            var orders = filesRepository.GetAllOrdersAsync();

            var fraudResults = fraudRadar.Check(orders.Result);

            foreach (var fraudResult in fraudResults)
            {
                Console.WriteLine($"Order Id {fraudResult.OrderId} is fraudulent");
            }
        }
    }
}