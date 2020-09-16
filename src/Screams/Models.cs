using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Screams
{
    /// <summary>
    /// model of scream
    /// </summary>
    public class Models
    {
        /// <summary>
        /// this model will be argument to create a new scream
        /// </summary>
        public struct NewScreamtion
        {
            [Required]
            public int ScreamerId { get; set; }
            [Required, MinLength(5), DisplayName("内容至少5个字")]
            public string Content { get; set; }
        }
    }
}
