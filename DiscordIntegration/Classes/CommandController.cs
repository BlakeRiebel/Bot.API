using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Core.Classes.Settings;
using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data.Entities;
using DiscordIntegration.Classes.Modules;
using Serilog;
using Serilog.Events;
using System;
using System.Linq;
using System.Threading.Tasks;
using TwitchIntegration.Classes;

namespace DiscordIntegration.Classes
{
    public class CommandController
    {
        public readonly DiscordSocketClient Client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly IInfusedRealityServices _appServices;

        private readonly ulong GUILD_ID;
        private readonly ulong LIVE_NOTIFICATION_CHANNEL_ID;

        public CommandController(IServiceProvider services, IInfusedRealityServices appServices, DiscordSettings settings)
        {
            _commands = new CommandService(new CommandServiceConfig()
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            Client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
            });

            _appServices = appServices;

            GUILD_ID = ulong.Parse(settings.ServerGuildId);
            LIVE_NOTIFICATION_CHANNEL_ID = ulong.Parse(settings.TwitchNotificationsChannelID);

            TwitchPubSubController.StreamStarted += StreamStarted;
            TwitchPubSubController.StreamEnded += StreamEnded;

            _services = services;
            _commands.Log += LogAsync;
            Client.Log += LogAsync;
            AddModules(services);
            Client.MessageReceived += HandleCommandAsync;
        }

        #region Private Methods
        private void AddModules(IServiceProvider services)
        {
            _commands.AddModuleAsync<BingBongModule>(services);
            _commands.AddModuleAsync<ChatToolsModule>(services);
            _commands.AddModuleAsync<UserTextCommandModule>(services);
            _commands.AddModuleAsync<TwitchChannelModule>(services);
            _commands.AddModuleAsync<GameLibraryModule>(services);
            _commands.AddModuleAsync<HelpModule>(services);
        }

        private async Task HandleCommandAsync(SocketMessage arg)
        {
            var msg = arg as SocketUserMessage;

            if (msg == null)
                return;

            if (msg.Author.Id == Client.CurrentUser.Id || msg.Author.IsBot)
                return;

            int pos = 0;

            if(msg.HasCharPrefix('?', ref pos))
            {
                var context = new SocketCommandContext(Client, msg);

                var result = await _commands.ExecuteAsync(context, pos, _services);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

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

            Log.Write(severity, message.Exception, "[{Source}] {Message}", message.Source, message.Message);

            await Task.CompletedTask;
        }

        private async void StreamStarted(object sender, TwitchChannel channel)
        {
            var textchannel = Client.GetGuild(GUILD_ID)
                                .TextChannels.Where(c => c.Id == LIVE_NOTIFICATION_CHANNEL_ID).FirstOrDefault();

            var subscriptions = _appServices.GetTwitchNotificationService().GetAll().Where(w => w.ChannelId == channel.Id).ToList();



            subscriptions.ForEach(subscription =>
            {
                var user = _appServices.GetUsersService().GetAll().Where(w => w.UserId == subscription.UserId).FirstOrDefault();

                var discordUser = Client.GetGuild(GUILD_ID).GetUser(ulong.Parse(user.DiscordId));

                if (discordUser != null)
                    discordUser.CreateDMChannelAsync().Result.SendMessageAsync(string.Format("Hey {0}! {1}", discordUser.DisplayName, channel.LiveMessage));
            });


            textchannel.SendMessageAsync(channel.LiveMessage);
        }

        private async void StreamEnded(object sender, TwitchChannel channel)
        {
            var textchannel = Client.GetGuild(GUILD_ID)
                                .TextChannels.Where(c => c.Id == LIVE_NOTIFICATION_CHANNEL_ID).FirstOrDefault();

            textchannel.SendMessageAsync(channel.OfflineMessage);
        }
        #endregion
    }
}
