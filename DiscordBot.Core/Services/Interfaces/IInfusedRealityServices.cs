namespace DiscordBot.Core.Services.Interfaces
{
    public interface IInfusedRealityServices
    {
        IEventLogService GetEventLogService();
        ISubscriptionsService GetSubscriptionsService();
        ITwitchChannelsService GetTwitchChannelsService();
        IUsersService GetUsersService();
    }
}