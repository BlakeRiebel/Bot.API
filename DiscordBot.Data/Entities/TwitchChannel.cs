using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Data.Entities
{
    [Table("TwitchChannels")]
    public class TwitchChannel
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        public int TwitchId { get; set; }

        [MaxLength(100)]
        public string Url { get; set; }

        [MaxLength(250)]
        public string LiveMessage { get; set; }

        [MaxLength(250)]
        public string OfflineMessage { get; set; }

        public List<TwitchNotification> Users { get; set; }
    }
}