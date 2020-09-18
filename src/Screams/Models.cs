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
            [Required, Range(1, int.MaxValue, ErrorMessage = "发布人参数有误")]
            public int ScreamerId { get; set; }
            [Required(ErrorMessage = "内容至少5个字"), MinLength(5)]
            public string Content { get; set; }
        }
    }
}
