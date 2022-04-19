using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiscordIntegration.Classes.Modules
{
    public class ModuleWrapper : ModuleBase<SocketCommandContext>
    {
        protected readonly IInfusedRealityServices _appServices;

        private const string LiveTemplate = "{0}'s Stream has started! \nhttps://www.twitch.tv/{0}";
        private const string OfflineTemplate = "{0}'s Stream has ended! \nhttps://www.twitch.tv/{0}";
        private const string URLTemplate = "https://www.twitch.tv/{0}";

        public ModuleWrapper(IInfusedRealityServices appServices)
        {
            _appServices = appServices;
        }

        #region Helper Methods
        #region CREATE
        #region Game Library
        /// <summary>
        /// Add a new game to the DB
        /// </summary>
        /// <param name="Name">Name of Game</param>
        /// <param name="Catagory">Catagory of Game</param>
        /// <param name="MaxPartySize">Max Party Size of Game</param>
        /// <returns>The newly added game</returns>
        protected Game AddGame(string Name, string Catagory = "", int MaxPartySize = 0)
        {
            var game = new Game() { Name = Name, Category = Catagory, PartySize = MaxPartySize };
            return _appServices.GetGamesService().Insert(game);
        }
        #endregion
        #region Twitch Notifications
        /// <summary>
        /// Adds a twitch channel to the DB
        /// </summary>
        /// <param name="Name">Name of the Twitch Channel</param>
        /// <param name="TwitchId">Twitch Channel ID</param>
        /// <param name="URL">Channel URL</param>
        /// <param name="LiveMessage">Live Notification Message</param>
        /// <param name="OfflineMessage">Offline Notification Message</param>
        /// <returns>The newly added twitch channel</returns>
        protected TwitchChannel AddTwitchChannel(string Name, int TwitchId = 0, string URL = "", 
                                                 string LiveMessage = "", string OfflineMessage = "")
        {
            var channel = new TwitchChannel() 
            { 
                Name = Name, 
                TwitchId = TwitchId, 
                Url = (URL == "") ? string.Format(URLTemplate, Name) : URL, 
                LiveMessage = (LiveMessage == "") ? string.Format(LiveTemplate, Name) : LiveMessage,
                OfflineMessage = (OfflineMessage == "") ? string.Format(OfflineTemplate, Name) : OfflineMessage
            };

            return _appServices.GetTwitchChannelsService().Insert(channel);
        }
        #endregion
        #region User
        /// <summary>
        /// Adds a User to the DB
        /// </summary>
        /// <param name="Name">User Name</param>
        /// <param name="DiscordId">Discord ID</param>
        /// <returns>The newly added user</returns>
        protected User AddUser(string Name, string DiscordId)
        {
            var user = new User() { UserName = Name, DiscordId = DiscordId };
            return _appServices.GetUsersService().Insert(user);
        }
        #endregion
        #region Many To Many Links
        /// <summary>
        /// Adds game to a users collection
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="game">Game</param>
        /// <returns>Game Collection Item</returns>
        protected GameCollection AddGameToCollection(User user, Game game)
        {
            var collection = new GameCollection() { UserId = user.UserId, GameId = game.GameId };
            return _appServices.GetGameCollectionService().Insert(collection);
        }

        /// <summary>
        /// Adds Twitch Notification for the user
        /// </summary>
        /// <param name="user">User</param>
        /// <param name="channel">Twitch Channel</param>
        /// <returns>Twitch Notification Item</returns>
        protected TwitchNotification AddTwitchNotification(User user, TwitchChannel channel)
        {
            var twitchNotification = new TwitchNotification() { UserId = user.UserId, ChannelId = channel.Id };
            return _appServices.GetTwitchNotificationService().Insert(twitchNotification);
        }
        #endregion
        #endregion
        #region READ
        #region Game Library
        /// <summary>
        /// Gets a game from the DB
        /// </summary>
        /// <param name="id">Primary Key ID</param>
        /// <returns>Game</returns>
        protected Game GetGame(int id)
        {
            return _appServices.GetGamesService().SingleOrDefault(w => w.GameId == id) ?? null;
        }

        /// <summary>
        /// Gets a List of Games
        /// </summary>
        /// <returns>List of Games</returns>
        protected List<Game> GetGames()
        {
            return _appServices.GetGamesService().GetAll().ToList();
        }

        /// <summary>
        /// Gets a game from the DB
        /// </summary>
        /// <param name="Name">Name of game</param>
        /// <returns>Game</returns>
        protected Game GetGame(string Name)
        {
            return _appServices.GetGamesService().SingleOrDefault(w => w.Name == Name) ?? null;
        }
        #endregion
        #region Twitch Notifications
        /// <summary>
        /// Gets Twitch Channel
        /// </summary>
        /// <param name="id">Primary Key Id</param>
        /// <returns>The Twitch Channel</returns>
        protected TwitchChannel GetTwitchChannel(int id)
        {
            return _appServices.GetTwitchChannelsService().SingleOrDefault(w => w.Id == id) ?? null;
        }

        /// <summary>
        /// Gets Twitch Channel
        /// </summary>
        /// <param name="name">Name of the Channel</param>
        /// <returns>The Twitch Channel</returns>
        protected TwitchChannel GetTwitchChannel(string name)
        {
            return _appServices.GetTwitchChannelsService().SingleOrDefault(w => w.Name == name) ?? null;
        }
        #endregion
        #region User
        /// <summary>
        /// Gets a User
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User</returns>
        protected User GetUser(int id)
        {
            return _appServices.GetUsersService().SingleOrDefault(w => w.UserId == id) ?? null;
        }

        /// <summary>
        /// Gets a User
        /// </summary>
        /// <param name="Name">Name of the User</param>
        /// <returns>User</returns>
        protected User GetUser(string Name)
        {
            return _appServices.GetUsersService().SingleOrDefault(w => w.UserName == Name) ?? null;
        }

        /// <summary>
        /// Gets a User
        /// </summary>
        /// <param name="discordID">Discord ID of the User</param>
        /// <returns>User</returns>
        protected User GetUser(ulong discordID)
        {
            return _appServices.GetUsersService().SingleOrDefault(w => w.DiscordId == discordID.ToString()) ?? null;
        }

        /// <summary>
        /// Gets a List of Users
        /// </summary>
        /// <returns>Users</returns>
        protected List<User> GetUsersFromDB()
        {
            return _appServices.GetUsersService().GetAll().ToList();
        }
        #endregion
        #region Many To Many Links
        /// <summary>
        /// Gets a List of Users
        /// </summary>
        /// <param name="game">Game</param>
        /// <returns>List of Users</returns>
        protected List<User> GetUsers(Game game)
        {
            List<User> users = new List<User>();
            
            foreach (var gameCollection in GetGameCollectionByGame(game.GameId))
            {
                users.Add(GetUser(gameCollection.UserId));
            }

            return users;
        }

        /// <summary>
        /// Gets a List of Games
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>List of Games</returns>
        protected List<Game> GetGames(User user)
        {
            List<Game> games = new List<Game>();

            foreach (var gameCollection in GetGameCollectionByUser(user.UserId))
            {
                games.Add(GetGame(gameCollection.GameId));
            }

            return games;
        }

        /// <summary>
        /// Gets a Game Collection Item
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="GameId">Game Id</param>
        /// <returns>Game Collection Item</returns>
        protected GameCollection GetGameCollectionItem(int UserId, int GameId)
        {
            return _appServices.GetGameCollectionService().SingleOrDefault(w => w.UserId == UserId && w.GameId == GameId) ?? new GameCollection();
        }

        /// <summary>
        /// Gets a Game Collection
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <returns>Game Collection</returns>
        protected List<GameCollection> GetGameCollectionByUser(int UserId)
        {
            return _appServices.GetGameCollectionService().GetAll(w => w.UserId == UserId).ToList() ?? new List<GameCollection>();
        }

        /// <summary>
        /// Gets a Game Collection
        /// </summary>
        /// <param name="GameId">Game Id</param>
        /// <returns>Game Collection</returns>
        protected List<GameCollection> GetGameCollectionByGame(int GameId)
        {
            return _appServices.GetGameCollectionService().GetAll(w => w.GameId == GameId).ToList() ?? new List<GameCollection>();
        }

        /// <summary>
        /// Gets Twitch Channels
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>List of Twitch Channels</returns>
        protected List<TwitchChannel> GetTwitchChannels(User user)
        {
            List<TwitchChannel> channels = new List<TwitchChannel>();

            foreach (var twitchNotification in GetTwitchNotificationsByUser(user.UserId))
            {
                channels.Add(GetTwitchChannel(twitchNotification.ChannelId));
            }

            return channels;
        }

        /// <summary>
        /// Gets Users
        /// </summary>
        /// <param name="channel">Twitch Channel</param>
        /// <returns>List of users</returns>
        protected List<User> GetUsers(TwitchChannel channel)
        {
            List<User> users = new List<User>();

            foreach (var twitchNotification in GetTwitchNotificationsByTwitchChannel(channel.Id))
            {
                users.Add(GetUser(twitchNotification.UserId));
            }

            return users;
        }

        /// <summary>
        /// Gets the Twitch Notification Link
        /// </summary>
        /// <param name="userId">Users Id</param>
        /// <param name="twitchId">Twitch Channel Id</param>
        /// <returns>Twitch Notification Link</returns>
        protected TwitchNotification GetTwitchNotification(int userId, int twitchId)
        {
            return _appServices.GetTwitchNotificationService()
                        .SingleOrDefault(w => w.UserId == userId && w.ChannelId == twitchId) ?? new TwitchNotification();
        }

        /// <summary>
        /// Gets a list of Twitch Notification Links for the user
        /// </summary>
        /// <param name="userId">Users Id</param>
        /// <returns>List of Twitch Notification Links</returns>
        protected List<TwitchNotification> GetTwitchNotificationsByUser(int userId)
        {
            return _appServices.GetTwitchNotificationService().GetAll(w => w.UserId == userId).ToList() ?? new List<TwitchNotification>();
        }

        /// <summary>
        /// Gets a list of Twitch Notification Links for the twitch channel
        /// </summary>
        /// <param name="twitchId">ID for the twitch channel</param>
        /// <returns>List of Twitch Notification Links</returns>
        protected List<TwitchNotification> GetTwitchNotificationsByTwitchChannel(int twitchId)
        {
            return _appServices.GetTwitchNotificationService().GetAll(w => w.ChannelId == twitchId).ToList() ?? new List<TwitchNotification>();
        }
        #endregion
        #endregion
        #region UPDATE
        #region Game Library
        /// <summary>
        /// Update a game in the DB
        /// </summary>
        /// <param name="game">Updated Game</param>
        /// <returns>Updated Game</returns>
        protected Game UpdateGame(Game game)
        {
            _appServices.GetGamesService().Update(game);
            return game;
        }

        /// <summary>
        /// Update a game in the DB
        /// </summary>
        /// <param name="Id">Primary Key ID</param>
        /// <param name="Name">Updated Name</param>
        /// <param name="Category">Updated Category</param>
        /// <param name="MaxPartySize">Updated MaxPartySize</param>
        /// <returns>Updated Game</returns>
        protected Game UpdateGame(int Id, string Name = null, string Category = null, int MaxPartySize = 0)
        {
            var game = GetGame(Id);

            if (game.Name != null)
                game.Name = Name;

            if (game.Category != null)
                game.Category = Category;

            if (game.PartySize != 0)
                game.PartySize = MaxPartySize;

            return UpdateGame(game);
        }
        #endregion
        #region Twitch Notifications
        /// <summary>
        /// Updates a Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel</param>
        /// <returns>Updated Twitch Channel</returns>
        protected TwitchChannel UpdateTwitchChannel(TwitchChannel channel)
        {
            _appServices.GetTwitchChannelsService().Update(channel);
            return channel;
        }

        /// <summary>
        /// Updates a Twitch Channel
        /// </summary>
        /// <param name="Id">Primary Key Id</param>
        /// <param name="Name">Updated Name</param>
        /// <param name="TwitchId">Updated TwitchId</param>
        /// <param name="URL">Updated URL</param>
        /// <param name="LiveMessage">Updated LiveMessage</param>
        /// <param name="OfflineMessage">Updated OfflineMessage</param>
        /// <returns>Updated Twitch Channel</returns>
        protected TwitchChannel UpdateTwitchChannel(int Id, string Name = null, int TwitchId = 0, string URL = null, string LiveMessage = null, string OfflineMessage = null)
        {
            var channel = GetTwitchChannel(Id);

            if (channel.Name != null)
                channel.Name = Name;

            if (channel.TwitchId != 0)
                channel.TwitchId = TwitchId;

            if (channel.Url != null)
                channel.Url = URL;

            if (channel.LiveMessage != null)
                channel.LiveMessage = LiveMessage;

            if (channel.OfflineMessage != null)
                channel.OfflineMessage = OfflineMessage;

            return UpdateTwitchChannel(channel);
        }
        #endregion
        #region User
        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Updated User</returns>
        protected User UpdateUser(User user)
        {
            _appServices.GetUsersService().Update(user);
            return user;
        }

        /// <summary>
        /// Updates a User
        /// </summary>
        /// <param name="Id">Primary Key Id</param>
        /// <param name="Name">Updated Name</param>
        /// <param name="DiscordId">Updated DiscordId</param>
        /// <returns>Updated User</returns>
        protected User UpdateUser(int Id, string Name = null, string DiscordId = null)
        {
            var user = GetUser(Id);

            if (user.UserName != null)
                user.UserName = Name;

            if (user.DiscordId != null)
                user.DiscordId = DiscordId;

            return UpdateUser(user);
        }
        #endregion
        #endregion
        #region DELETE
        #region Game Library
        /// <summary>
        /// Deletes a Game from the DB
        /// </summary>
        /// <param name="game">Game</param>
        /// <returns>Deleted Game</returns>
        protected Game DeleteGame(Game game)
        {
            DeleteGameCollectionForGame(game.GameId);
            _appServices.GetGamesService().Delete(game);
            return game;
        }

        /// <summary>
        /// Deletes a Game from the DB
        /// </summary>
        /// <param name="Id">Primary Key Id</param>
        /// <returns>Deleted Game</returns>
        protected Game DeleteGame(int Id)
        {
            return DeleteGame(GetGame(Id));
        }

        /// <summary>
        /// Deletes a Game from the DB
        /// </summary>
        /// <param name="Name">Game Name</param>
        /// <returns>Deleted Game</returns>
        protected Game DeleteGame(string Name)
        {
            return DeleteGame(GetGame(Name));
        }

        /// <summary>
        /// Deletes a List of Games from the DB
        /// </summary>
        /// <param name="games">List of Games</param>
        /// <returns>Deleted Games</returns>
        protected List<Game> DeleteGames(List<Game> games)
        {
            games.ForEach(game => DeleteGame(game));
            return games;
        }

        /// <summary>
        /// Deletes a List of Games from the DB
        /// </summary>
        /// <param name="names">List of Game Names</param>
        /// <returns>Deleted Games</returns>
        protected List<Game> DeleteGames(List<string> names)
        {
            List<Game> games = new List<Game>();
            names.ForEach(name =>
            {
                games.Add(DeleteGame(name));
            });
            return games;
        }
        #endregion
        #region Twitch Notifications
        /// <summary>
        /// Deletes a Twitch Channel
        /// </summary>
        /// <param name="channel">Twitch Channel</param>
        /// <returns>Deleted Twitch Channel</returns>
        protected TwitchChannel DeleteTwitchChannel(TwitchChannel channel)
        {
            _appServices.GetTwitchChannelsService().Delete(channel);
            return channel;
        }

        /// <summary>
        /// Deletes a Twitch Channel
        /// </summary>
        /// <param name="Id">Channel Id</param>
        /// <returns>Deleted Twitch Channel</returns>
        protected TwitchChannel DeleteTwitchChannel(int Id)
        {
            return DeleteTwitchChannel(GetTwitchChannel(Id));
        }

        /// <summary>
        /// Deletes a Twitch Channel
        /// </summary>
        /// <param name="name">Channel Name</param>
        /// <returns>Deleted Twitch Channel</returns>
        protected TwitchChannel DeleteTwitchChannel(string name)
        {
            return DeleteTwitchChannel(GetTwitchChannel(name));
        }

        /// <summary>
        /// Deletes a List of Twitch Channels
        /// </summary>
        /// <param name="channels">Twitch Channels</param>
        /// <returns>Deleted Twitch Channels</returns>
        protected List<TwitchChannel> DeleteTwitchChannels(List<TwitchChannel> channels)
        {
            channels.ForEach(channel => DeleteTwitchChannel(channel));
            return channels;
        }

        /// <summary>
        /// Deletes a List of Twitch Channels
        /// </summary>
        /// <param name="IDs">Channel Ids</param>
        /// <returns>Deleted Twitch Channels</returns>
        protected List<TwitchChannel> DeleteTwitchChannels(List<int> IDs)
        {
            List<TwitchChannel> channels = IDs.Select(id => GetTwitchChannel(id)).ToList();
            return DeleteTwitchChannels(channels);
        }

        /// <summary>
        /// Deletes a List of Twitch Channels
        /// </summary>
        /// <param name="names">Channel Names</param>
        /// <returns>Deleted Twitch Channels</returns>
        protected List<TwitchChannel> DeleteTwitchChannels(List<string> names)
        {
            List<TwitchChannel> channels = names.Select(name => GetTwitchChannel(name)).ToList();
            return DeleteTwitchChannels(channels);
        }
        #endregion
        #region User
        /// <summary>
        /// Deletes a user from the DB
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>Deleted User</returns>
        protected User DeleteUser(User user)
        {
            _appServices.GetUsersService().Delete(user);
            return user;
        }

        /// <summary>
        /// Deletes a user from the DB
        /// </summary>
        /// <param name="Id">User Id</param>
        /// <returns>Deleted User</returns>
        protected User DeleteUser(int Id) 
        { 
            return DeleteUser(GetUser(Id));
        }

        /// <summary>
        /// Deletes a user from the DB
        /// </summary>
        /// <param name="Name">User Name</param>
        /// <returns>Deleted User</returns>
        protected User DeleteUser(string Name) 
        {
            return DeleteUser(GetUser(Name));
        }

        /// <summary>
        /// Deletes a user from the DB
        /// </summary>
        /// <param name="DiscordID">User DiscordID</param>
        /// <returns>Deleted User</returns>
        protected User DeleteUser(ulong DiscordID)
        {
            return DeleteUser(GetUser(DiscordID));
        }

        /// <summary>
        /// Deletes users from the DB
        /// </summary>
        /// <param name="users">Users</param>
        /// <returns>Deleted Users</returns>
        protected List<User> DeleteUsers(List<User> users) 
        {
            users.ForEach(user => DeleteUser(user));
            return users;
        }

        /// <summary>
        /// Deletes users from the DB
        /// </summary>
        /// <param name="Ids">User Ids</param>
        /// <returns>Deleted Users</returns>
        protected List<User> DeleteUsers(List<int> Ids) 
        {
            List<User> Users = Ids.Select(Id => GetUser(Id)).ToList();
            return DeleteUsers(Users);
        }

        /// <summary>
        /// Deletes users from the DB
        /// </summary>
        /// <param name="Names">User Names</param>
        /// <returns>Deleted Users</returns>
        protected List<User> DeleteUsers(List<string> Names) 
        {
            List<User> Users = Names.Select(Name => GetUser(Name)).ToList();
            return DeleteUsers(Users);
        }

        /// <summary>
        /// Deletes users from the DB
        /// </summary>
        /// <param name="DiscordIDs">User DiscordIDs</param>
        /// <returns>Deleted Users</returns>
        protected List<User> DeleteUsers(List<ulong> DiscordIDs)
        {
            List<User> Users = DiscordIDs.Select(DiscordID => GetUser(DiscordID)).ToList();
            return DeleteUsers(Users);
        }
        #endregion
        #region Many To Many Links
        /// <summary>
        /// Deletes a Game Collection Item
        /// </summary>
        /// <param name="collectionItem">Game Collection Item</param>
        /// <returns>Deleted Game Collection Item</returns>
        protected GameCollection DeleteGameCollectionItem(GameCollection collectionItem) 
        {
            _appServices.GetGameCollectionService().Delete(collectionItem);
            return collectionItem;
        }

        /// <summary>
        /// Deletes a Game Collection Item
        /// </summary>
        /// <param name="UserId">Used ID</param>
        /// <param name="GameId">Game ID</param>
        /// <returns>Deleted Game Collection Item</returns>
        protected GameCollection DeleteGameCollectionItem(int UserId, int GameId) 
        {
            return DeleteGameCollectionItem(GetGameCollectionItem(UserId, GameId));
        }

        /// <summary>
        /// Deletes a Game Collection
        /// </summary>
        /// <param name="collection">Game Collection</param>
        /// <returns>Deleted Game Collection</returns>
        protected List<GameCollection> DeleteGameCollection(List<GameCollection> collection) 
        {
            collection.ForEach(collectionItem => DeleteGameCollectionItem(collectionItem));
            return collection;
        }

        /// <summary>
        /// Deletes a Game Collection
        /// </summary>
        /// <param name="UserId">User ID</param>
        /// <returns>Deleted Game Collection</returns>
        protected List<GameCollection> DeleteGameCollectionForUser(int UserId) 
        {
            var collection = GetGameCollectionByUser(UserId);
            return DeleteGameCollection(collection);
        }

        /// <summary>
        /// Deletes a Game Collection
        /// </summary>
        /// <param name="GameId">Game ID</param>
        /// <returns>Deleted Game Collection</returns>
        protected List<GameCollection> DeleteGameCollectionForGame(int GameId) 
        {
            var collection = GetGameCollectionByGame(GameId);
            return DeleteGameCollection(collection);
        }

        /// <summary>
        /// Deletes a Twitch Notification
        /// </summary>
        /// <param name="notification">Twitch Notification</param>
        /// <returns>Deleted Twitch Notification</returns>
        protected TwitchNotification DeleteTwitchNotification(TwitchNotification notification) 
        {
            _appServices.GetTwitchNotificationService().Delete(notification);
            return notification;
        }

        /// <summary>
        /// Deletes a Twitch Notification
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <param name="ChannelId">Channel Id</param>
        /// <returns>Deleted Twitch Notification</returns>
        protected TwitchNotification DeleteTwitchNotification(int UserId, int ChannelId) 
        {
            return DeleteTwitchNotification(GetTwitchNotification(UserId, ChannelId));
        }

        /// <summary>
        /// Deletes a List of Twitch Notifications
        /// </summary>
        /// <param name="notifications">Twitch  Notifications</param>
        /// <returns>Deleted Twitch Notifications</returns>
        protected List<TwitchNotification> DeleteTwitchNotifications(List<TwitchNotification> notifications) 
        {
            notifications.ForEach(notification => DeleteTwitchNotification(notification));
            return notifications;
        }

        /// <summary>
        /// Deletes a List of Twitch Notifications
        /// </summary>
        /// <param name="UserId">User Id</param>
        /// <returns>Deleted Twitch Notifications</returns>
        protected List<TwitchNotification> DeleteTwitchNotificationsForUser(int UserId) 
        {
            var notifications = GetTwitchNotificationsByUser(UserId);
            return DeleteTwitchNotifications(notifications);
        }

        /// <summary>
        /// Deletes a List of Twitch Notifications
        /// </summary>
        /// <param name="ChannelId">Channel Id</param>
        /// <returns>Deleted Twitch Notifications</returns>
        protected List<TwitchNotification> DeleteTwitchNotificationsForChannel(int ChannelId) 
        {
            var notifications = GetTwitchNotificationsByTwitchChannel(ChannelId);
            return DeleteTwitchNotifications(notifications);
        }

        /// <summary>
        /// Deletes a List of Twitch Notifications
        /// </summary>
        /// <param name="ChannelName">Channel Name</param>
        /// <returns>Deleted Notifications</returns>
        protected List<TwitchNotification> DeleteTwitchNotificationsForChannel(string ChannelName)
        {
            var notifications = GetTwitchNotificationsByTwitchChannel(GetTwitchChannel(ChannelName).Id);
            return DeleteTwitchNotifications(notifications);
        }
        #endregion
        #endregion
        #endregion
    }
}