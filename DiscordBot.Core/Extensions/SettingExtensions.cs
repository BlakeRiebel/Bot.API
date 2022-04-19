using DiscordBot.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiscordBot.Core.Extensions
{
    public static class SettingExtensions
    {
        public static string GetSettingValue(this List<AppSetting> _settings, string _domain, string _setting)
        {
            return _settings.GetSettingValue(null, _domain, _setting);
        }

        public static string GetSettingValue(this List<AppSetting> _settings, string _environment, string _domain, string _setting)
        {
            AppSetting setting = _settings.Where(w => (w.Environment == null || w.Environment == _environment) &&
                                                w.Domain == _domain &&
                                                w.Name == _setting)
                                            .OrderByDescending(o => o.Environment)
                                            .FirstOrDefault();

            return setting?.Value;
        }
    }
}
