using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jat.Entities
{
    public abstract class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string CreatedBy { get; set; } = string.Empty;

        [MaxLength(100)]
        public string UpdatedBy { get; set; } = string.Empty;

        [Required]
        public bool Deleted { get; set; } = false;

        public BaseEntity()
        {
            // Initialize any common properties if needed
        }

        public void UpdateTimestamp()
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
