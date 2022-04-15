using DiscordBot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Data
{
    public class DiscordBotDBContext : DbContext
    {
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Subscriptions> Subscriptions { get; set; }
        public DbSet<TwitchChannels> TwitchChannels { get; set; }
        public DbSet<User> Users { get; set; }

        protected readonly IConfiguration Configuration;

        public DiscordBotDBContext(DbContextOptions<DiscordBotDBContext> options) : base(options)
        {

        }
    }
}
