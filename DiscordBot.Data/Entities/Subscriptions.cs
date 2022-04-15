using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Data.Entities
{
    [Table("Subscriptions")]
    public class Subscriptions
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        public int TwitchChannelId { get; set; }

        [MaxLength(500)]
        public string SubscriberIDs { get; set; }
    }
}
