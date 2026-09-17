using LiteDB;
using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.Tests.Services
{
    [TestFixture]
    public class ProjectServiceTests
    {
        private class TestIssueService : IIssueService
        {
            public IssueSearchCriteria? ReceivedCriteria { get; private set; }

            public List<Issue> IssuesToReturn { get; set; } = new ();

            public bool ShouldThrow { get; set; }

            public Task<List<Issue>> GetIssuesAsync(Project project, int count)
            {
                return Task.FromResult(new List<Issue>());
            }

            public Task<List<Issue>> GetIssuesAsync(IssueSearchCriteria criteria)
            {
                ReceivedCriteria = criteria;
                if (ShouldThrow)
                {
                    throw new HttpRequestException("Network failure");
                }

                return Task.FromResult(IssuesToReturn);
            }

            public Task<List<Issue>> FetchRecentlyUpdatedIssuesAsync()
            {
                if (ShouldThrow)
                {
                    throw new HttpRequestException("Network failure");
                }

                return Task.FromResult(IssuesToReturn);
            }

            public Task<Issue> CreateIssueAsync(Project project, string summary, string description)
            {
                throw new NotImplementedException();
            }

            public Task CompleteIssueAsync(Issue issue)
            {
                throw new NotImplementedException();
            }
        }

        [Test]
        public void PopulateUpdatedAt_WhenFetchFails_DoesNotUpdateLastIssueFetchDateTime()
        {
            var lastFetchTime = new DateTime(2026, 9, 10, 8, 30, 0);
            var testIssueService = new TestIssueService
            {
                ShouldThrow = true,
            };

            var credentials = new ApiCredentials();
            var service = new ProjectService(credentials, null!, testIssueService, new TestDbService());

            var projects = new List<Project>
            {
                new () { ShortName = "PRJ", Name = "Project PRJ", },
            };

            Assert.ThrowsAsync<HttpRequestException>(async () => await service.PopulateUpdatedAt(projects));
        }
    }

    public class TestDbService : ILiteDbService
    {
        public T? Get<T>(BsonValue id, string? collectionName = null)
        {
            return default;
        }

        public IEnumerable<T> GetAll<T>(string? collectionName = null)
        {
            return new List<T>();
        }

        public bool Upsert<T>(T entity, string? collectionName = null)
        {
            return true;
        }

        public int Upsert<T>(IEnumerable<T> entities, string? collectionName = null)
        {
            return 0;
        }
    }
}