using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class Scream : EntityBase
    {
        [Required]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        [Required]
        public string Content { get; set; } = "";
        [Required, Range(0, int.MaxValue)]
        public int ContentLength { get; set; } = 0;
        [Required]
        public int HiddenCount { get; set; } = 0;
        [Required]
        public bool Hidden { get; set; } = false;
        public int? AuditorId { get; set; }
    }
}
