using DiscordBot.Core.Classes.Settings;
using DiscordBot.Core.Helpers;
using DiscordBot.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services,
            ConfigurationHelper _helper)
        {
            var connectionString = _helper.config.GetConnectionString("WebApiDatabase");

            services.AddDbContext<DiscordBotDBContext>(options =>
            {
                options.UseMySQL(connectionString);
            });

            return services;
        }

        public static IServiceCollection InjectApplicationServices(this IServiceCollection services,
             ConfigurationHelper _helper)
        {
            //Infrastructure
            services.AddSingleton(_helper.config);
            return services.InjectApplicationServices();
        }

        private static IServiceCollection InjectApplicationServices(this IServiceCollection services)
        {
            MyServiceProvider.ServiceProvider = services.BuildServiceProvider()
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope()
                .ServiceProvider;

            using (DiscordBotDBContext dicordBotcontext = MyServiceProvider.ServiceProvider.GetService<DiscordBotDBContext>())
            {
                services.AddSingleton(new DiscordSettings().PopulateSettings(dicordBotcontext));

                services.AddUnitOfWork<DiscordBotDBContext>();
                services.AddServices<DiscordBotDBContext>();
            }

            return services;
        }
    }
}
