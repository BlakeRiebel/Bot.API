using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DiscordBot.Data.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [Column("UserId")]
        public int UserId { get; set; }

        [MaxLength(255)]
        public string UserName { get; set; }

        [MaxLength(200)]
        public string DiscordId { get; set; }

        public List<GameCollection> GamesLibrary { get; set; }
        public List<TwitchNotification> TwitchSubs { get; set; }
    }
}
