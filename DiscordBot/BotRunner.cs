using DiscordBot.Core.Classes.Settings;
using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Interfaces;
using DiscordIntegration.Classes;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TwitchIntegration.Classes;

namespace DiscordBot
{
    public class BotRunner : IBotRunner
    {
        private readonly ILogger<BotRunner> _logger;
        private readonly DiscordBotController _botController;
        private CancellationToken _discordCancellation;
        private Task _discordTask;

        private readonly TwitchPubSubController _twitchPubSub;
        private CancellationToken _twitchCancellation;
        private Task _twitchTask;

        private IInfusedRealityServices _appServices;

        public BotRunner(ILogger<BotRunner> logger, IServiceProvider services, IInfusedRealityServices appServices, DiscordSettings settings)
        {
            _logger = logger;
            _botController = new DiscordBotController(services, appServices, settings);
            _discordCancellation = new CancellationToken();

            _twitchPubSub = new TwitchPubSubController(appServices);
            _twitchCancellation = new CancellationToken();
            _appServices = appServices;
        }

        public async Task Run()
        {
            try
            {
                InitalizeDiscordBot();
                InitalizeTwitchIntegration();

                while (!_discordTask.IsCompleted && !_twitchTask.IsCompleted)
                {
                    
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while running! {ex}", ex);
            }
        }

        #region Private Methods
        private async Task InitalizeDiscordBot()
        {
            await _botController.Login();
            _discordTask = Task.Run(_botController.Run, _discordCancellation);
            await _discordTask;
        }

        private async Task InitalizeTwitchIntegration()
        {
            _twitchTask = Task.Run(_twitchPubSub.Run, _twitchCancellation);
            await _twitchTask;
        }
        #endregion
    }
}
