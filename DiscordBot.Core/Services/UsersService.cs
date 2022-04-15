using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using DiscordBot.Data.Interfaces;

namespace DiscordBot.Core.Services
{
    public class UsersService : GenericService<User>, IUsersService
    {
        public UsersService(IUnitOfWork<DiscordBotDBContext> unitOfWork) : base(unitOfWork)
        {
        }
    }
}