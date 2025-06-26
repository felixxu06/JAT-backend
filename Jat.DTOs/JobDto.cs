namespace Jat.DTOs
{
    public class JobDto
    {
        public long Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public JobStatus? Status { get; set; } = null;
        public DateTime? DateApplied { get; set; }
    }
}
