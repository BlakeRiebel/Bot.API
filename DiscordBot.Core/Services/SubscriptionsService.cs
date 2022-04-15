using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using DiscordBot.Data.Interfaces;

namespace DiscordBot.Core.Services
{
    public class SubscriptionsService : GenericService<Subscriptions>, ISubscriptionsService
    {
        public SubscriptionsService(IUnitOfWork<DiscordBotDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}