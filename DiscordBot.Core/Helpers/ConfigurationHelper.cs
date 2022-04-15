using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace DiscordBot.Core.Helpers
{
    public class ConfigurationHelper
    {
        public IConfiguration config { get; set; }

        /// <summary>
        /// Set Configuraiton Builder
        /// </summary>
        public ConfigurationHelper()
        {
            SetConfigurationBuilder("");
        }

        /// <summary>
        /// Set Configuraiton Builder
        /// </summary>
        /// <param name="jsonFile">the JSON Filename</param>
        public ConfigurationHelper(string jsonFile)
        {
            SetConfigurationBuilder(jsonFile);
        }

        /// <summary>
        /// Set Configuraiton Builder
        /// </summary>
        /// <param name="jsonFile">the JSON Filename</param>
        private void SetConfigurationBuilder(string jsonFile)
        {
            string environmentName = System.Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");

            config = new ConfigurationBuilder()
               .SetBasePath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, AppDomain.CurrentDomain.RelativeSearchPath ?? ""))
               .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true)
               .AddJsonFile($"AppSettings.{environmentName}.json", optional: true)
               .AddEnvironmentVariables()
               .Build();
        }

        public T GetAppSettings<T>(string key) where T : class, new()
        {
            return config.GetSection(key).Get<T>();
        }

        public T GetAppSettings<T>() where T : class, new()
        {
            return GetAppSettings<T>(typeof(T).Name);
        }
    }
}
