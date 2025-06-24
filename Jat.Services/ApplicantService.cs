using System;
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

        public async Task<IEnumerable<ApplicantDto>> GetAllApplicantsAsync(int pageNumber, int pageSize)
        {
            var applicants = await _repository.GetAllAsync(pageNumber, pageSize);
            return applicants.Select(applicant => new ApplicantDto
            {
                Id = applicant.Id,
                FirstName = applicant.FirstName,
                LastName = applicant.LastName,
                Email = applicant.Email,
                PhoneNumber = applicant.PhoneNumber,
                Address = applicant.Address,
                AppliedAt = applicant.AppliedAt,
                Status = Enum.TryParse<DTOApplicantStatus>(applicant.Status.ToString(), out var status) ? (DTOApplicantStatus?)status : null,
                Description = applicant.Description,
                LinkedInProfile = applicant.LinkedInProfile,
                ResumeFilePath = applicant.ResumeFilePath
            });
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
                Status = Enum.TryParse<DTOApplicantStatus>(applicant.Status.ToString(), out var status) ? (DTOApplicantStatus?)status : null,
                Description = applicant.Description,
                LinkedInProfile = applicant.LinkedInProfile,
                ResumeFilePath = applicant.ResumeFilePath
            };
        }

        public async Task AddApplicantAsync(ApplicantDto applicantDto)
        {
            var applicant = new Applicant
            {
                FirstName = applicantDto.FirstName,
                LastName = applicantDto.LastName,
                Email = applicantDto.Email,
                PhoneNumber = applicantDto.PhoneNumber,
                Address = applicantDto.Address,
                AppliedAt = applicantDto.AppliedAt,
                Status = applicantDto.Status.HasValue ? (EntityApplicantStatus)applicantDto.Status.Value : EntityApplicantStatus.Applied,
                Description = applicantDto.Description,
                LinkedInProfile = applicantDto.LinkedInProfile,
                ResumeFilePath = applicantDto.ResumeFilePath
            };
            await _repository.AddAsync(applicant);
        }

        public async Task UpdateApplicantAsync(long id, ApplicantDto applicantDto)
        {
            var applicant = new Applicant
            {
                Id = id,
                FirstName = applicantDto.FirstName,
                LastName = applicantDto.LastName,
                Email = applicantDto.Email,
                PhoneNumber = applicantDto.PhoneNumber,
                Address = applicantDto.Address,
                AppliedAt = applicantDto.AppliedAt,
                Status = applicantDto.Status.HasValue ? (EntityApplicantStatus)applicantDto.Status.Value : EntityApplicantStatus.Applied,
                Description = applicantDto.Description,
                LinkedInProfile = applicantDto.LinkedInProfile,
                ResumeFilePath = applicantDto.ResumeFilePath
            };
            await _repository.UpdateAsync(id, applicant);
        }

        public async Task DeleteApplicantAsync(long id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}