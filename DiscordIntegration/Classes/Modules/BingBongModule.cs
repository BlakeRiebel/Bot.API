using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class BingBongModule : ModuleWrapper
    {
        public BingBongModule(IInfusedRealityServices appServices) : base(appServices)
        {
        }

        [Command("Bing")]
        [Summary("Ayyyy yoooo")]
        public Task Bing() => ReplyAsync("Bong");
    }
}