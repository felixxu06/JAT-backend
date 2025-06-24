using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jat.Entities
{
    public class Job : BaseEntity
    {
        [Required, MaxLength(100)]
        public string CompanyName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string Position { get; set; } = string.Empty;

        [Required]
        public JobStatus Status { get; set; } = JobStatus.Open;

        [Required]
        public DateTime DateApplied { get; set; } = DateTime.UtcNow;

        // Actions like Edit are typically handled in the UI, not as entity properties.
        // If you want to track edits, you can use the UpdatedAt/UpdatedBy from BaseEntity.

        public override string? ToString()
        {
            return $"{CompanyName} - {Position} ({Status}) applied on {DateApplied:yyyy-MM-dd}";
        }
    }
}
