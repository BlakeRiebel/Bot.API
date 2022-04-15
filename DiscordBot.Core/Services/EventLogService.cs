using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using DiscordBot.Data.Interfaces;

namespace DiscordBot.Core.Services
{
    public class EventLogService : GenericService<EventLog>, IEventLogService
    {
        public EventLogService(IUnitOfWork<DiscordBotDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}