using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordIntegration.Classes.Modules
{
    public class UserTextCommandModule : ModuleWrapper
    {
        public UserTextCommandModule(IInfusedRealityServices appServices) : base(appServices){}

        #region Game Library Commands
        /// <summary>
        /// Adds game to the Users Library
        /// </summary>
        /// <param name="GameName">Name of the game</param>
        /// <returns>Message</returns>
        [Command("AddGameToLibrary")]
        [Summary("Adds game to the Users Library")]
        public Task AddGameToLibrary(string GameName)
        {
            var user = GetUser(Context.User.Username);

            if (user == null)
                user = AddUser(Context.User.Username, Context.User.Id.ToString());

            var game = GetGame(GameName);

            if (game == null)
                return ReplyAsync(String.Format("{0} does not currently exist. Use ?Help to learn more about adding games", GameName));

            AddGameToCollection(user, game);

            return ReplyAsync(String.Format("You have successfully added {0} to your Library!", GameName));
        }

        /// <summary>
        /// Returns a list of the users games
        /// </summary>
        /// <returns>Games | Error Message</returns>
        [Command("MyLibrary")]
        [Summary("Returns a list of the users games")]
        public Task MyLibrary()
        {
            var user = GetUser(Context.User.Username);

            if (user == null)
            {
                user = AddUser(Context.User.Username, Context.User.Id.ToString());

                return ReplyAsync("You have no current games added :(");
            }    

            var Games = GetGames(user);

            if(!Games.Any())
                return ReplyAsync("You have no current games added :(");

            StringWriter writer = new StringWriter();

            writer.WriteLine(string.Format("{0}'s Game Library", user.UserName));
            writer.WriteLine("Name                  Catagory         Max Party Size");
            writer.WriteLine("-----------------------------------------------------");

            foreach (var game in Games)
            {
                var name = game.Name.PadRight(22, ' ');
                var catagory = game.Category.PadRight(17, ' ');
                writer.WriteLine(String.Format("{0}{1}{2}", name, catagory, game.PartySize.ToString()));
            }

            return ReplyAsync(writer.ToString());
        }

        /// <summary>
        /// Removes a game from the Users Library
        /// </summary>
        /// <param name="GameName">Name of the game</param>
        /// <returns>Message</returns>
        [Command("RemoveGameFromLibrary")]
        [Summary("Removes a game from the Users Library")]
        public Task RemoveGameFromLibrary(string GameName)
        {
            var user = GetUser(Context.User.Username);

            if (user == null)
                user = AddUser(Context.User.Username, Context.User.Id.ToString());

            var game = GetGame(GameName);

            if (game == null)
                return ReplyAsync(String.Format("{0} does not currently exist. Use ?Help to learn more about adding games", GameName));

            DeleteGameCollectionItem(user.UserId, game.GameId);

            return ReplyAsync(String.Format("You have successfully removed {0} from your Library!", GameName));
        }
        #endregion

        #region Twitch Subscription Commands
        /// <summary>
        /// Subscribes a user for Live notifications
        /// </summary>
        /// <param name="twitchName">twitch user</param>
        /// <returns>Message</returns>
        [Command("Subscribe")]
        [Summary("Unsubscribes a user for Live notifications")]
        public Task Subscribe(string twitchName)
        {
            var user = GetUser(Context.User.Username) ?? AddUser(Context.User.Username, Context.User.Id.ToString());

            var twitchChannel = GetTwitchChannel(twitchName);

            if (twitchChannel == null)
                return ReplyAsync(String.Format("Twitch Channel Not Found!\nPlease check your spelling and then use ?AddTwitchChannel to add the channel."));

            AddTwitchNotification(user, twitchChannel);

            return ReplyAsync(String.Format("You have subscribed to Live notifications for {0}!", twitchName));
        }

        /// <summary>
        /// Unsubscribes a user for Live notifications
        /// </summary>
        /// <param name="twitchName">twitch user</param>
        /// <returns>Message</returns>
        [Command("Unsubscribe")]
        [Summary("Unsubscribes a user for Live notifications")]
        public Task Unsubscribe(string twitchName)
        {
            var user = GetUser(Context.User.Username) ?? AddUser(Context.User.Username, Context.User.Id.ToString());

            var twitchChannel = GetTwitchChannel(twitchName);

            if (twitchChannel == null)
                return ReplyAsync(String.Format("Twitch Channel Not Found!\nPlease check your spelling and then use ?AddTwitchChannel to add the channel."));

            DeleteTwitchNotification(user.UserId, twitchChannel.Id);

            return ReplyAsync(String.Format("You have unsubscribed from Live notifications for {0}!", twitchName));
        }
        #endregion
    }
}