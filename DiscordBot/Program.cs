using DiscordBot.Core.Extensions;
using DiscordBot.Core.Helpers;
using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.Interfaces;
using System.Threading.Tasks;

namespace DiscordBot
{
    class Program
    {
        private const string settingsJsonFile = "AppSettings";

        static void Main(string[] args)
        {
            ConfigurationHelper _helper = new ConfigurationHelper(settingsJsonFile);
            Log.Logger = new LoggerConfiguration()
                             .MinimumLevel.Verbose()
                             .Enrich.FromLogContext()
                             .WriteTo.Console()
                             .CreateLogger();

            try
            {
                var host = CreateHostBuilder(_helper)
                    .Build();

                IBotRunner createInvoice = ActivatorUtilities.CreateInstance<BotRunner>(host.Services);
                Task bot = createInvoice.Run();

                bot.Wait();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "There was a problem starting the bot");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        #region Private Methods
        private static IHostBuilder CreateHostBuilder(ConfigurationHelper _helper)
        {
            return Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddCustomizedDataStore(_helper)
                        .InjectApplicationServices(_helper);

                    services.AddTransient<IBotRunner, BotRunner>();
                })
                .UseSerilog();
        }
        #endregion
    }
}
