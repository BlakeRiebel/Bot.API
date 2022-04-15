using DiscordBot.Core.Services;
using DiscordBot.Core.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordBot.Core.Extensions
{
    public static class ServicesServiceCollectionExtensions
    {
        public static IServiceCollection AddServices<TContext>(this IServiceCollection services)
            where TContext : DbContext
        {
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped<IInfusedRealityServices, InfusedRealityServices>();
            return services;
        }
    }
}