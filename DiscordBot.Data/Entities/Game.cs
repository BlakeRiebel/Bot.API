using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Data.Entities
{
    [Table("Games")]
    public class Game
    {
        [Key]
        [Column("Id")]
        public int GameId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        public int PartySize { get; set; }

        public List<GameCollection> Users { get; set; }
    }
}