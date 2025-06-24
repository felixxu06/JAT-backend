using System.ComponentModel.DataAnnotations;

namespace Jat.Entities
{
    public class Applicant : BaseEntity
    {
        [Required, MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress, MaxLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone, MaxLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [MaxLength(200)]
        public string Address { get; set; } = string.Empty;

        public DateTime AppliedAt { get; set; } = DateTime.UtcNow;

        public ApplicantStatus Status { get; set; } = ApplicantStatus.Interview;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Url, MaxLength(200)]
        public string? LinkedInProfile { get; set; }

        [MaxLength(200)]
        public string? ResumeFilePath { get; set; }

        public Int64? JobId { get; set; }

        public override string? ToString()
        {
            return $"{FirstName} {LastName} ({Email}) - {Status}";
        }
    }
}
