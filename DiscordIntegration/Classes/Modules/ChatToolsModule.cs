using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class ChatToolsModule : ModuleWrapper
    {
        private Random rnd = new Random();

        public ChatToolsModule(IInfusedRealityServices appServices) : base(appServices) {}

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
            var users = GetUsersFromDB();
            StringWriter writer = new StringWriter();
            writer.WriteLine("ID    Name        DiscordID");
            writer.WriteLine("---------------------------");
            users.ForEach(user =>
            {
                var ID = user.UserId.ToString().PadRight(6, ' ');
                var Name = user.UserName.PadRight(12, ' ');
                writer.WriteLine(String.Format("{0}{1}{2}", ID, Name, user.DiscordId));
            });

            return ReplyAsync(writer.ToString());
        }
    }
}