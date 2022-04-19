using Discord;
using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class GameLibraryModule : ModuleWrapper
    {
        public GameLibraryModule(IInfusedRealityServices appServices) : base(appServices)
        {
        }

        #region CREATE
        [Command("AddGameToBot")]
        [Summary("Adds game to the DB")]
        public Task AddGame(params string[] GameDetials)
        {
            if (GameDetials.Length != 3)
                return ReplyAsync("Arguments not formatted correctly. ?AddGame \"GameTitle\" \"Catagory\" <MaxPartySize>");

            var Name = GameDetials[0];
            var Catagory = GameDetials[1];
            var MaxParty = int.Parse(GameDetials[2]);

            if((Name ?? string.Empty) == string.Empty || (Catagory ?? string.Empty) == string.Empty || MaxParty <= 0)
                return ReplyAsync("Arguments not formatted correctly. ?AddGame \"GameTitle\" \"Catagory\" <MaxPartySize>");

            var game = AddGame(Name, Catagory, MaxParty);

            StringWriter writer = new StringWriter();
            var name = game.Name.PadRight(22, ' ');
            var catagory = game.Category.PadRight(17, ' ');

            writer.WriteLine("                   New Game Added!                   ");
            writer.WriteLine("Name                  Catagory         Max Party Size");
            writer.WriteLine("-----------------------------------------------------");
            writer.WriteLine(String.Format("{0}{1}{2}", name, catagory, game.PartySize.ToString()));
            return ReplyAsync(writer.ToString());
        }

        [Command("AddGameToUsersLibrary")]
        [Summary("Adds game to the Users Library")]
        public Task AddGameToUsersLibrary(IUser user, string GameName)
        {
            var discordId = user?.Id;

            if (discordId == null || (GameName ?? String.Empty) == String.Empty)
                return ReplyAsync("Arguments not formatted correctly. ?AddGameToUsersLibrary @User GameName");

            var UserDB = GetUser((ulong)discordId);
            var game = GetGame(GameName);

            AddGameToCollection(UserDB, game);

            return ReplyAsync("Game Added Successfully");
        }
        #endregion
        #region READ
        [Command("AllGames")]
        [Summary("Returns all games")]
        public Task AllGames()
        {
            var games = GetGames();

            StringWriter writer = new StringWriter();

            writer.WriteLine("                 Server Game Library                 ");
            writer.WriteLine("Name                  Catagory         Max Party Size");
            writer.WriteLine("-----------------------------------------------------");

            foreach (var game in games)
            {
                var name = game.Name.PadRight(22, ' ');
                var catagory = game.Category.PadRight(17, ' ');
                writer.WriteLine(String.Format("{0}{1}{2}", name, catagory, game.PartySize.ToString()));
            }

            return ReplyAsync(writer.ToString());
        }

        [Command("GameInfo")]
        [Summary("Adds game to the Users Library")]
        public Task GameInfo(string GameName)
        {
            var game = GetGame(GameName);

            if (game == null)
                return ReplyAsync("Game not found");

            StringWriter writer = new StringWriter();
            writer.WriteLine("Name                  Catagory         Max Party Size");
            writer.WriteLine("-----------------------------------------------------");
            var name = game.Name.PadRight(22, ' ');
            var catagory = game.Category.PadRight(17, ' ');
            writer.WriteLine(String.Format("{0}{1}{2}", name, catagory, game.PartySize.ToString()));
            return ReplyAsync(writer.ToString());
        }

        [Command("GameUsers")]
        [Summary("Adds game to the Users Library")]
        public Task GameUsers(string GameName)
        {
            var game = GetGame(GameName);
            var users = GetUsers(game);

            if (game == null)
                return ReplyAsync("Game not found");

            if (users == null)
                return ReplyAsync("users not found");

            StringWriter writer = new StringWriter();
            writer.WriteLine("UserId    Name      ");
            writer.WriteLine("--------------------");
            foreach (var user in users)
            {
                var ID = user.UserId.ToString().PadRight(10, ' ');
                var Name = user.UserName.PadRight(10, ' ');
                writer.WriteLine("{0}{1}", ID, Name);
            }

            return ReplyAsync(writer.ToString());
        }

        [Command("UsersGames")]
        [Summary("Adds game to the Users Library")]
        public Task UsersGames(IUser discordUser)
        {
            var user = GetUser(discordUser.Id.ToString());
            var games = GetGames(user);

            if (user == null)
                return ReplyAsync("User not found");

            if (games == null)
                return ReplyAsync("Games not found");

            StringWriter writer = new StringWriter();

            writer.WriteLine("                    Game Library                 ");
            writer.WriteLine("Name                  Catagory         Max Party Size");
            writer.WriteLine("-----------------------------------------------------");

            foreach (var game in games)
            {
                var name = game.Name.PadRight(22, ' ');
                var catagory = game.Category.PadRight(17, ' ');
                writer.WriteLine(String.Format("{0}{1}{2}", name, catagory, game.PartySize.ToString()));
            }

            return ReplyAsync(writer.ToString());
        }
        #endregion
        #region UPDATE
        [Command("UpdateCategory")]
        [Summary("Adds game to the Users Library")]
        public Task UpdateCatagory(params string[] GameName_Catagory)
        {
            var game = GetGame(GameName_Catagory[0]);

            if (game == null)
                return ReplyAsync("Game not found");

            game.Category = GameName_Catagory[1];
            UpdateGame(game);
            return ReplyAsync("Game Updated");
        }

        [Command("UpdateName")]
        [Summary("Adds game to the Users Library")]
        public Task UpdateName(params string[] GameName_OLD_NEW)
        {
            var game = GetGame(GameName_OLD_NEW[0]);

            if (game == null)
                return ReplyAsync("Game not found");

            game.Name = GameName_OLD_NEW[1];
            UpdateGame(game);
            return ReplyAsync("Game Updated");
        }
        #endregion
        #region DELETE
        [Command("RemoveGame")]
        [Summary("Adds game to the Users Library")]
        public Task RemoveGame(string GameName)
        {
            DeleteGame(GameName);
            return ReplyAsync("Game Removed");
        }
        #endregion
    }
}