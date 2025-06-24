
namespace Jat.DTOs
{
    public class ApplicantDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public DateTime AppliedAt { get; set; }
        public ApplicantStatus? Status { get; set; } = null;
        public string Description { get; set; } = string.Empty;
        public string? LinkedInProfile { get; set; }
        public string? ResumeFilePath { get; set; }
    }
}
