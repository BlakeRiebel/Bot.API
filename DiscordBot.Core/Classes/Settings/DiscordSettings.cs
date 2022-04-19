using DiscordBot.Core.Extensions;
using DiscordBot.Data;
using DiscordBot.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace DiscordBot.Core.Classes.Settings
{
    public class DiscordSettings
    {
        private const string Domain = "HoldingHandsBot.Discord";

        public string AnnouncementChannelID { get; set; }
        public string ServerGuildId { get; set; }
        public string Token { get; set; }
        public string TwitchNotificationsChannelID { get; set; }

        public string Environment { get { return EnvironmentSettings.Environment; } }

        public DiscordSettings PopulateSettings(DiscordBotDBContext _context)
        {
            return PopulateSettings(_context, Environment);
        }

        public DiscordSettings PopulateSettings(DiscordBotDBContext _context, string _environment)
        {
            List<AppSetting> appSettings = _context.AppSettings.ToList();

            return PopulateSettings(appSettings, _environment);
        }

        public DiscordSettings PopulateSettings(List<AppSetting> _appSettings, string _environment)
        {
            this.AnnouncementChannelID = _appSettings.GetSettingValue(Environment, Domain, "AnnouncementChannelID");
            this.ServerGuildId = _appSettings.GetSettingValue(Environment, Domain, "ServerGuildId");
            this.Token = _appSettings.GetSettingValue(Environment, Domain, "Token");
            this.TwitchNotificationsChannelID = _appSettings.GetSettingValue(Environment, Domain, "TwitchNotificationsChannelID");

            return this;
        }
    }
}