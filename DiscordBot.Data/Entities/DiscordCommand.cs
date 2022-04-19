using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Data.Entities
{
    [Table("DiscordCommands")]
    public class DiscordCommand
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Category { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string Parameters { get; set; }

        [MaxLength(200)]
        public string Example { get; set; }
        public bool Active { get; set; }
    }
}