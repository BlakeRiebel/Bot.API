using Discord;
using Discord.WebSocket;
using DiscordBot.Core.Services.Interfaces;
using Serilog;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes
{
    public class DiscordBotController
    {
        private CommandController _commandController;

        public DiscordBotController(IServiceProvider services, IInfusedRealityServices appServices)
        {
            _commandController = new CommandController(services, appServices);
        }

        public async Task Login()
        {
            await _commandController.Client.LoginAsync(TokenType.Bot, "<NOPE>");
        }

        public async Task Run()
        {
            await _commandController.Client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        #region Private Methods
        private static async Task LogAsync(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogEventLevel.Fatal,
                LogSeverity.Error => LogEventLevel.Error,
                LogSeverity.Warning => LogEventLevel.Warning,
                LogSeverity.Info => LogEventLevel.Information,
                LogSeverity.Verbose => LogEventLevel.Debug,
                LogSeverity.Debug => LogEventLevel.Verbose,
                _ => LogEventLevel.Information,
            };

            Log.Write(severity, message.Exception, "", message.Source, message.Message);

            await Task.CompletedTask;
        }
        #endregion
    }
}