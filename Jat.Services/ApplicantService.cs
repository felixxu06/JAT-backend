using Jat.DTOs;
using Jat.IServices;
using Jat.IRepositories;
using Jat.Entities;
using DTOApplicantStatus = Jat.DTOs.ApplicantStatus;
using EntityApplicantStatus = Jat.Entities.ApplicantStatus;

namespace Jat.Services
{
    public class ApplicantService : IApplicantService
    {
        private readonly IApplicantRepository _repository;

        public ApplicantService(IApplicantRepository repository)
        {
            _repository = repository;
        }

        public async Task<(IEnumerable<ApplicantDto> Applicants, int TotalCount, int TotalPages)> GetAllApplicantsAsync(int pageNumber, int pageSize)
        {
            var applicants = await _repository.GetAllAsync(pageNumber, pageSize);
            var totalCount = await _repository.GetTotalCountAsync();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
            var applicantDtos = applicants.Select(applicant => new ApplicantDto
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                Address = applicant.Address,
                AppliedAt = applicant.AppliedAt,
                JobId = applicant.JobId ?? 0,
                Status = Enum.TryParse<DTOApplicantStatus>(applicant.Status.ToString(), out var status) ? (DTOApplicantStatus?)status : null,
                Description = applicant.Description,
            });
            return (applicantDtos, totalCount, totalPages);
        }

        public async Task<ApplicantDto?> GetApplicantByIdAsync(long id)
        {
            var applicant = await _repository.GetByIdAsync(id);
            if (applicant == null) return null;
            return new ApplicantDto
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                Address = applicant.Address,
                AppliedAt = applicant.AppliedAt,
                JobId = applicant.JobId ?? 0,
                Status = Enum.TryParse<DTOApplicantStatus>(applicant.Status.ToString(), out var status) ? (DTOApplicantStatus?)status : null,
                Description = applicant.Description,
            };
        }

        private void ValidateApplicantDto(ApplicantDto applicantDto)
        {
            if (applicantDto == null)
                throw new DtoValidationException("ApplicantDto", "DTO cannot be null.");
            if (string.IsNullOrWhiteSpace(applicantDto.FirstName))
                throw new DtoValidationException("FirstName", "FirstName is required.");
            if (applicantDto.FirstName.Length > 50)
                throw new DtoValidationException("FirstName", "FirstName cannot exceed 50 characters.");
            if (string.IsNullOrWhiteSpace(applicantDto.LastName))
                throw new DtoValidationException("LastName", "LastName is required.");
            if (applicantDto.LastName.Length > 50)
                throw new DtoValidationException("LastName", "LastName cannot exceed 50 characters.");
            if (string.IsNullOrWhiteSpace(applicantDto.Email))
                throw new DtoValidationException("Email", "Email is required.");
            if (applicantDto.Email.Length > 100)
                throw new DtoValidationException("Email", "Email cannot exceed 100 characters.");
            if (!string.IsNullOrWhiteSpace(applicantDto.PhoneNumber) && applicantDto.PhoneNumber.Length > 20)
                throw new DtoValidationException("PhoneNumber", "PhoneNumber cannot exceed 20 characters.");
            if (!string.IsNullOrWhiteSpace(applicantDto.Address) && applicantDto.Address.Length > 200)
                throw new DtoValidationException("Address", "Address cannot exceed 200 characters.");
            if (!string.IsNullOrWhiteSpace(applicantDto.Description) && applicantDto.Description.Length > 1000)
                throw new DtoValidationException("Description", "Description cannot exceed 1000 characters.");
        }

        public async Task AddApplicantAsync(ApplicantDto applicantDto)
        {
            ValidateApplicantDto(applicantDto);
            var applicant = new Applicant
            {
                FirstName = applicantDto.FirstName,
                LastName = applicantDto.LastName,
                Email = applicantDto.Email,
                PhoneNumber = applicantDto.PhoneNumber,
                Address = applicantDto.Address,
                AppliedAt = DateTime.UtcNow,
                JobId = applicantDto.JobId,
                Status = EntityApplicantStatus.Applied,
                Description = applicantDto.Description,
            };
            await _repository.AddAsync(applicant);
            applicantDto.Id = applicant.Id;
        }

        public async Task UpdateApplicantAsync(long id, ApplicantDto applicantDto)
        {
            ValidateApplicantDto(applicantDto);
            var existingApplicant = await _repository.GetByIdAsync(id);
            if(existingApplicant == null)
            {
                throw new Exception($"Applicant with ID {id} not found.");
            }
            var applicant = new Applicant
            {
                Id = id,
                FirstName = applicantDto.FirstName,
                LastName = applicantDto.LastName,
                Email = applicantDto.Email,
                PhoneNumber = applicantDto.PhoneNumber,
                Address = applicantDto.Address,
                AppliedAt = existingApplicant.AppliedAt,
                JobId = applicantDto.JobId,
                Status = applicantDto.Status.HasValue ? (EntityApplicantStatus)applicantDto.Status.Value : EntityApplicantStatus.Applied,
                Description = applicantDto.Description,
            };
            await _repository.UpdateAsync(id, applicant);
        }

        public async Task DeleteApplicantAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}