using Discord.Commands;
using DiscordBot.Core.Services.Interfaces;
using DiscordBot.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace $rootnamespace$
{
	public class $safeitemrootname$Module : ModuleBase<SocketCommandContext>
	{
		private IInfusedRealityServices _appServices;

        public $safeitemrootname$Module(IInfusedRealityServices appServices)
        {
            _appServices = appServices;
        }
    }
}