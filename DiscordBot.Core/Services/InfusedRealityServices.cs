using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Interfaces;
using System.Collections.Generic;

namespace DiscordBot.Core.Services
{
    public class InfusedRealityServices : IInfusedRealityServices
    {
        private readonly IUnitOfWork<DiscordBotDBContext> _unitOfWork;
        private Dictionary<string, object> _services = new Dictionary<string, object>();

        public InfusedRealityServices(IUnitOfWork<DiscordBotDBContext> unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IAppSettingService GetAppSettingService()
        {
            return (IAppSettingService)GetService("AppSettingService", new AppSettingService(_unitOfWork));
        }

        public IDiscordCommandService GetDiscordCommandService()
        {
            return (IDiscordCommandService)GetService("DiscordCommandService", new DiscordCommandService(_unitOfWork));
        }

        public IEventLogService GetEventLogService()
        {
            return (IEventLogService)GetService("EventLogService", new EventLogService(_unitOfWork));
        }

        public IGamesService GetGamesService()
        {
            return (IGamesService)GetService("GamesService", new GamesService(_unitOfWork));
        }

        public ITwitchNotificationService GetTwitchNotificationService()
        {
            return (ITwitchNotificationService)GetService("TwitchNotificationService", new TwitchNotificationService(_unitOfWork));
        }

        public ITwitchChannelsService GetTwitchChannelsService()
        {
            return (ITwitchChannelsService)GetService("TwitchChannelsService", new TwitchChannelsService(_unitOfWork));
        }

        public IGameCollectionService GetGameCollectionService()
        {
            return (IGameCollectionService)GetService("GameCollectionService", new GameCollectionService(_unitOfWork));
        }

        public IUsersService GetUsersService()
        {
            return (IUsersService)GetService("UsersService", new UsersService(_unitOfWork));
        }

        private object GetService(string key, object value)
        {
            if (!_services.ContainsKey(key))
            {
                _services[key] = value;
            }

            return _services[key];
        }
    }
}