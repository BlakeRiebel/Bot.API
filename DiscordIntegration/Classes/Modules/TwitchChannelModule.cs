using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class TwitchChannelModule : ModuleBase<SocketCommandContext>
    {
        private IInfusedRealityServices _appServices;

        private const string TwitchLiveMessageTemplate = "{0}'s Stream has started!\n https://www.twitch.tv/{0}";
        private const string TwitchOfflineMessageTemplate = "{0}'s Stream has ended! \n https://www.twitch.tv/{0}";
        private const string TwitchURLTemplate = "https://www.twitch.tv/{0}";

        public TwitchChannelModule(IInfusedRealityServices appServices)
        {
            _appServices = appServices;
        }

        [Command("AddTwitchChannel")]
        [Summary("Adds a new twitch to the bots system")]
        public Task AddTwitchChannel(string username, int twitchID)
        {
            try
            {
                var newChannel = new TwitchChannels()
                {
                    Name = username,
                    Url = String.Format(TwitchURLTemplate, username),
                    TwitchId = twitchID,
                    LiveMessage = String.Format(TwitchLiveMessageTemplate, username),
                    OfflineMessage = String.Format(TwitchOfflineMessageTemplate, username),
                };

                _appServices.GetTwitchChannelsService().Insert(newChannel);
            }
            catch (Exception ex)
            {
                return ReplyAsync(String.Format("FAILED TO ADD! ERROR: {0}", ex.Message));
            }

            return ReplyAsync(String.Format("{0} has been added!", username));
        }
    }
}