using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class Screamtion: EntityBase
    {
        [Required]
        public int ScreamerId { get; set; }
        public User Screamer { get; set; }
        [Required]
        public string Context { get; set; } = "";
        [Required]
        public int HiddenCount { get; set; } = 0;
        [Required]
        public int AuditorId { get; set; }
        public User Auditor { get; set; }
    }
}
