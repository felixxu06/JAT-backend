using Xunit;
using Jat.Entities;
using System.Threading.Tasks;
using System.Linq;
using Jat.Repositories;

namespace Jat.Tests
{
    public class JobRepositoryTests
    {
        private JobRepository CreateRepositoryWithContext(out Jat.Repositories.InMemoryDatabase db)
        {
            db = new Jat.Repositories.InMemoryDatabase();
            var userContext = new UserContext { CurrentUser = null };
            return new JobRepository(userContext, db);
        }

        [Fact]
        public async Task AddAsync_AddsJob()
        {
            var repo = CreateRepositoryWithContext(out var db);
            var job = new Job { CompanyName = "TestCo", Position = "Dev" };
            await repo.AddAsync(job);
            Assert.Single(db.Jobs.Values);
            Assert.Equal("TestCo", db.Jobs.Values.First().CompanyName);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsJobs()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Jobs[1] = new Job { Id = 1, CompanyName = "A", Position = "P" };
            var jobs = await repo.GetAllAsync(1, 10);
            Assert.Single(jobs);
        }

        [Fact]
        public async Task GetByIdAsync_ReturnsJob()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Jobs[1] = new Job { Id = 1, CompanyName = "A", Position = "P" };
            var job = await repo.GetByIdAsync(1);
            Assert.NotNull(job);
            Assert.Equal(1, job.Id);
        }

        [Fact]
        public async Task UpdateAsync_UpdatesJob()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Jobs[1] = new Job { Id = 1, CompanyName = "A", Position = "P" };
            var updated = new Job { CompanyName = "B", Position = "Q" };
            await repo.UpdateAsync(1, updated);
            Assert.Equal("B", db.Jobs[1].CompanyName);
        }

        [Fact]
        public async Task DeleteAsync_DeletesJob()
        {
            var repo = CreateRepositoryWithContext(out var db);
            db.Jobs[1] = new Job { Id = 1, CompanyName = "A", Position = "P" };
            await repo.DeleteAsync(1);
            Assert.True(db.Jobs[1].Deleted);
        }
    }
}
