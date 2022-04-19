using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using DiscordBot.Data.Interfaces;

namespace DiscordBot.Core.Services
{
    public class GamesService : GenericService<Game>, IGamesService
    {
        public GamesService(IUnitOfWork<DiscordBotDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}