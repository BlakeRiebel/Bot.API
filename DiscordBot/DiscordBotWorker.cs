using System;
using System.Collections.Generic;
using System.Text;
using DiscordBot.Core.Extensions;
using DiscordBot.Core.Helpers;
using Microsoft.Extensions.Hosting;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using DiscordBot.Interfaces;
using System.Threading.Tasks;
using System.Threading;

namespace DiscordBot
{
    public class DiscordBotWorker : BackgroundService
    {
        private const string settingsJsonFile = "AppSettings";

        public DiscordBotWorker()
        {

        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConfigurationHelper _helper = new ConfigurationHelper(settingsJsonFile);
            Log.Logger = new LoggerConfiguration()
                             .ReadFrom.Configuration(_helper.config)
                             .CreateLogger();

            try
            {
                var host = CreateHostBuilder(_helper)
                    .Build();

                Log.Information("Testing");

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
