namespace DiscordBot.Core.Services.Interfaces
{
    public interface IInfusedRealityServices
    {
        IAppSettingService GetAppSettingService();
        IDiscordCommandService GetDiscordCommandService();
        IEventLogService GetEventLogService();
        IGameCollectionService GetGameCollectionService();
        IGamesService GetGamesService();
        ITwitchChannelsService GetTwitchChannelsService();
        ITwitchNotificationService GetTwitchNotificationService();
        IUsersService GetUsersService();
    }
}