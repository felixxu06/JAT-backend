using Xunit;
using Jat.Entities;
using System.Threading.Tasks;
using System.Linq;
using Jat.Repositories;

namespace Jat.Tests
{
    public class ApplicantRepositoryTests
    {
        private ApplicantRepository CreateRepositoryWithContext(out Jat.Repositories.InMemoryDatabase db)
        {
            db = new Jat.Repositories.InMemoryDatabase();
            var userContext = new UserContext { CurrentUser = null };
            return new ApplicantRepository(userContext, db);
        }

        [Fact]
        public async Task AddAsync_AddsApplicant()
        {
            var repo = CreateRepositoryWithContext(out var db);
            var applicant = new Applicant { FirstName = "John", LastName = "Doe" };
            await repo.AddAsync(applicant);
            Assert.Single(db.Applicants.Values);
            Assert.Equal("John", db.Applicants.Values.First().FirstName);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsApplicants()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Applicants[1] = new Applicant { Id = 1, FirstName = "A", LastName = "B" };
            var applicants = await repo.GetAllAsync(1, 10);
            Assert.Single(applicants);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsApplicant()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Applicants[1] = new Applicant { Id = 1, FirstName = "A", LastName = "B" };
            var applicant = await repo.GetByIdAsync(1);
            Assert.NotNull(applicant);
            Assert.Equal(1, applicant.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesApplicant()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Applicants[1] = new Applicant { Id = 1, FirstName = "A", LastName = "B" };
            var updated = new Applicant { FirstName = "C", LastName = "D" };
            await repo.UpdateAsync(1, updated);
            Assert.Equal("C", db.Applicants[1].FirstName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesApplicant()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Applicants[1] = new Applicant { Id = 1, FirstName = "A", LastName = "B" };
            await repo.DeleteAsync(1);
            Assert.True(db.Applicants[1].Deleted);
        }

        [Fact]
        public async Task GetAllByJobIdAsync_ReturnsApplicantsForJob()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Applicants[1] = new Applicant { Id = 1, FirstName = "A", LastName = "B", JobId = 2 };
            db.Applicants[2] = new Applicant { Id = 2, FirstName = "C", LastName = "D", JobId = 2 };
            db.Applicants[3] = new Applicant { Id = 3, FirstName = "E", LastName = "F", JobId = 3 };
            var applicants = await repo.GetAllByJobIdAsync(2);
            Assert.Equal(2, applicants.Count());
        }
    }
}
