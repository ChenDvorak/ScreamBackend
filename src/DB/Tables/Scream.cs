using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class Scream: EntityBase
    {
        [Required]
        public int ScreamerId { get; set; }
        public User Screamer { get; set; }
        [Required]
        public string Content { get; set; } = "";
        [Required]
        public int HiddenCount { get; set; } = 0;
        [Required]
        public bool Hidden { get; set; } = false;
        [Required]
        public int AuditorId { get; set; }
        public User Auditor { get; set; }
    }
}
