using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class BingBongModule : ModuleBase<SocketCommandContext>
    {
        [Command("Bing")]
        [Summary("Ayyyy yoooo")]
        public Task Bing() => ReplyAsync("Bong");
    }
}
