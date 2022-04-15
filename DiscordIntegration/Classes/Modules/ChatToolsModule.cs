using Discord;
using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class ChatToolsModule : ModuleBase<SocketCommandContext>
    {
        private Random rnd = new Random();
        private IInfusedRealityServices _appServices;

        public ChatToolsModule(IInfusedRealityServices appServices)
        {
            _appServices = appServices;
        }

        [Command("RandomUser")]
        [Summary("Selects a random user from the passed in users")]
        public Task RandomUser(params string[] users)
        {
            if(users == null || users.Length == 0) return Task.FromResult(0);

            int UserIndex = rnd.Next(users.Length - 1);

            var user = users[UserIndex];

            return ReplyAsync(String.Format("@{0} You have been chosen!", user));
        }

        [Command("RandomNumber")]
        [Summary("Selects a random Number")]
        public Task RandomNumber(int max) => ReplyAsync(rnd.Next(max).ToString());

        [Command("GetUsers")]
        [Summary("Returns first or default user")]
        public Task GetUsers()
        {
            var user = _appServices.GetUsersService().GetAll().FirstOrDefault();

            return ReplyAsync(String.Format("Name: {0} ID: {1}", user.UserName, user.UserId));
        }
    }
}
