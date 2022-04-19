using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using DiscordBot.Data.Interfaces;

namespace $rootnamespace$
{
	public class $safeitemrootname$Service : GenericService<$safeitemrootname$>, I$safeitemrootname$Service
	{
		public $safeitemrootname$Service(IUnitOfWork<DiscordBotDBContext> unitOfWork) : base(unitOfWork)
		{
		}
	}
}