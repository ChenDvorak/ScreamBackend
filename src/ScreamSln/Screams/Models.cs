using ScreamBackend.DB.Tables;
using System.ComponentModel.DataAnnotations;

namespace Screams
{
    /// <summary>
    /// scream model 
    /// </summary>
    public class Models
    {
        /// <summary>
        /// this model will be argument to create a new scream
        /// </summary>
        public struct NewScreamtion
        {
            public int AuthorId { get; set; }
            [Required(ErrorMessage = "内容至少5个字"), MinLength(5)]
            public string Content { get; set; }
        }

        public struct NewComment
        {
            public User Author { get; set; }
            [Required]
            [MinLength(Comments.AbstractCommentsManager.COMMENT_MIN_LENGTH)]
            [MaxLength(Comments.AbstractCommentsManager.COMMENT_MAX_LENGTH)]
            public string Content { get; set; }
        }
    }
}
