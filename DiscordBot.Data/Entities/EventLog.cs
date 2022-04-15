using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DiscordBot.Data.Entities
{
    [Table("EventLog")]
    public class EventLog
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }

        [MaxLength(50)]
        public string Severity { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime Timestamp { get; set; }

        [MaxLength(512)]
        public string LogEvent { get; set; }

        [MaxLength(200)]
        public string Template { get; set; }

        [MaxLength(250)]
        public string Message { get; set; }

        [MaxLength(500)]
        public string Exception { get; set; }

        [MaxLength(500)]
        public string Information { get; set; }
    }
}