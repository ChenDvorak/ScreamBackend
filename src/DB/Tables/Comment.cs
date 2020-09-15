using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB.Tables
{
    public class Comment: EntityBase
    {
        [Required]
        public int CommenterId { get; set; }
        public User Commenter { get; set; }
        [Required]
        public string Content { get; set; }
        [Required]
        public int AuditorId { get; set; }
        public User Auditor { get; set; }
        [Required]
        public int HiddenCount { get; set; } = 0;
    }
}
