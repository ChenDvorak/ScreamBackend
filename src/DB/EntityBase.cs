using System;
using System.ComponentModel.DataAnnotations;

namespace ScreamBackend.DB
{
    /// <summary>
    /// Base entity of DB context model
    /// </summary>
    public abstract class EntityBase
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int State { get; set; } = 0;
        public DateTime CreateDate { get; set; } = DateTime.Now;
    }
}
