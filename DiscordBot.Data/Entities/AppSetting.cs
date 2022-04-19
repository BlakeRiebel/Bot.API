using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Data.Entities
{
    [Table("AppSettings")]
    public class AppSetting
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [MaxLength(20)]
        public string Environment { get; set; }
        [MaxLength(50)]
        public string Domain { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(50)]
        public string Type { get; set; }
        [MaxLength(100)]
        public string Value { get; set; }
    }
}