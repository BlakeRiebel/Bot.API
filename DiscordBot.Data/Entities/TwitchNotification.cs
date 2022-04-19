namespace DiscordBot.Data.Entities
{
    public class TwitchNotification
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; }

        public int ChannelId { get; set; }
        public TwitchChannel Channel { get; set; }
    }
}