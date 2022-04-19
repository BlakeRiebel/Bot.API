using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class TwitchChannelModule : ModuleWrapper
    {
        public TwitchChannelModule(IInfusedRealityServices appServices) : base(appServices) {}

        [Command("AddTwitchChannel")]
        [Summary("Adds a new twitch to the bots system")]
        public Task AddTwitchChannel(string username, int twitchID)
        {
            try
            {
                AddTwitchChannel(username, twitchID);
            }
            catch (Exception ex)
            {
                return ReplyAsync(String.Format("FAILED TO ADD! ERROR: {0}", ex.Message));
            }

            return ReplyAsync(String.Format("{0} has been added!", username));
        }

        [Command("RemoveTwitchChannel")]
        [Summary("Adds a new twitch to the bots system")]
        public Task RemoveTwitchChannel(string username)
        {
            try
            {
                DeleteTwitchChannel(username);
            }
            catch (Exception ex)
            {
                return ReplyAsync(String.Format("FAILED TO ADD! ERROR: {0}", ex.Message));
            }

            return ReplyAsync(String.Format("{0} has been Removed!", username));
        }
    }
}