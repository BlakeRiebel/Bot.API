using DiscordBot.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DiscordBot.Data
{
    public class DiscordBotDBContext : DbContext
    {
        public DbSet<AppSetting> AppSettings { get; set; }
        public DbSet<DiscordCommand> DiscordCommands { get; set; }
        public DbSet<EventLog> EventLogs { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<GameCollection> GameCollections { get; set; }
        public DbSet<TwitchChannel> TwitchChannels { get; set; }
        public DbSet<TwitchNotification> TwitchNotifications { get; set; }
        public DbSet<User> Users { get; set; }

        protected readonly IConfiguration Configuration;

        public DiscordBotDBContext(DbContextOptions<DiscordBotDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameCollection>()
                .HasOne(u => u.Game)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.GameId);

            modelBuilder.Entity<GameCollection>()
                .HasOne(u => u.User)
                .WithMany(u => u.GamesLibrary)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<TwitchNotification>()
                .HasOne(u => u.User)
                .WithMany(u => u.TwitchSubs)
                .HasForeignKey(u => u.UserId);

            modelBuilder.Entity<TwitchNotification>()
                .HasOne(u => u.Channel)
                .WithMany(u => u.Users)
                .HasForeignKey(u => u.ChannelId);
        }
    }
}