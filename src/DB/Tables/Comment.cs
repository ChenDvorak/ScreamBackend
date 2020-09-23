using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class Comment: EntityBase
    {
        [Required]
        public int AuthorId { get; set; }
        public User Author { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int AuditorId { get; set; }
        [Required]
        public int HiddenCount { get; set; } = 0;
        [Required]
        public bool Hidden { get; set; } = false;
        public int ScreamId { get; set; }
    }
}
