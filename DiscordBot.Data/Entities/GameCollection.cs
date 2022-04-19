namespace DiscordBot.Data.Entities
{
    public class GameCollection
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }
    }
}