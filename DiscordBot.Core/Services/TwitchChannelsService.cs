using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using DiscordBot.Data.Interfaces;

namespace DiscordBot.Core.Services
{
    public class TwitchChannelsService : GenericService<TwitchChannels>, ITwitchChannelsService
    {
        public TwitchChannelsService(IUnitOfWork<DiscordBotDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}