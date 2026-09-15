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
        public async Task PopulateUpdatedAt_WhenLastIssueFetchDateTimeIsNull_UsesTop200AndSetsLastFetchDateTime()
        {
            var testIssueService = new TestIssueService
            {
                IssuesToReturn = new List<Issue>
                {
                    new ()
                    {
                        IdReadable = "PRJ-1",
                        UpdatedAt = new DateTime(2026, 9, 15, 10, 0, 0),
                    },
                },
            };

            var appSettings = new AppSettings
            {
                LastIssueFetchDateTime = null,
            };

            var credentials = new ApiCredentials();
            var service = new ProjectService(credentials, null, testIssueService, null, appSettings);

            var projects = new List<Project>
            {
                new () { ShortName = "PRJ", Name = "Project PRJ", },
            };

            await service.PopulateUpdatedAt(projects);

            Assert.That(testIssueService.ReceivedCriteria, Is.Not.Null);
            Assert.That(testIssueService.ReceivedCriteria!.Top, Is.EqualTo(200));
            Assert.That(testIssueService.ReceivedCriteria.FromDate, Is.Null);
            Assert.That(testIssueService.ReceivedCriteria.SortByUpdatedDesc, Is.True);

            Assert.That(projects[0].UpdatedAt, Is.EqualTo(new DateTime(2026, 9, 15, 10, 0, 0)));
            Assert.That(appSettings.LastIssueFetchDateTime, Is.Not.Null);
            Assert.That(appSettings.LastIssueFetchDateTime!.Value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(5)));
        }

        [Test]
        public async Task PopulateUpdatedAt_WhenLastIssueFetchDateTimeIsSet_UsesFromDate()
        {
            var lastFetchTime = new DateTime(2026, 9, 10, 8, 30, 0);
            var testIssueService = new TestIssueService
            {
                IssuesToReturn = new List<Issue>
                {
                    new ()
                    {
                        IdReadable = "PRJ-2",
                        UpdatedAt = new DateTime(2026, 9, 15, 12, 0, 0),
                    },
                },
            };

            var appSettings = new AppSettings
            {
                LastIssueFetchDateTime = lastFetchTime,
            };

            var credentials = new ApiCredentials();
            var service = new ProjectService(credentials, null, testIssueService, null, appSettings);

            var projects = new List<Project>
            {
                new () { ShortName = "PRJ", Name = "Project PRJ", UpdatedAt = new DateTime(2026, 9, 9) },
            };

            await service.PopulateUpdatedAt(projects);

            Assert.That(testIssueService.ReceivedCriteria, Is.Not.Null);
            Assert.That(testIssueService.ReceivedCriteria!.FromDate, Is.EqualTo(lastFetchTime));
            Assert.That(testIssueService.ReceivedCriteria.SortByUpdatedDesc, Is.True);

            Assert.That(projects[0].UpdatedAt, Is.EqualTo(new DateTime(2026, 9, 15, 12, 0, 0)));
            Assert.That(appSettings.LastIssueFetchDateTime, Is.Not.EqualTo(lastFetchTime));
            Assert.That(appSettings.LastIssueFetchDateTime!.Value, Is.EqualTo(DateTime.Now).Within(TimeSpan.FromSeconds(5)));
        }

        [Test]
        public void PopulateUpdatedAt_WhenFetchFails_DoesNotUpdateLastIssueFetchDateTime()
        {
            var lastFetchTime = new DateTime(2026, 9, 10, 8, 30, 0);
            var testIssueService = new TestIssueService
            {
                ShouldThrow = true,
            };

            var appSettings = new AppSettings
            {
                LastIssueFetchDateTime = lastFetchTime,
            };

            var credentials = new ApiCredentials();
            var service = new ProjectService(credentials, null, testIssueService, null, appSettings);

            var projects = new List<Project>
            {
                new () { ShortName = "PRJ", Name = "Project PRJ", },
            };

            Assert.ThrowsAsync<HttpRequestException>(async () => await service.PopulateUpdatedAt(projects));
            Assert.That(appSettings.LastIssueFetchDateTime, Is.EqualTo(lastFetchTime));
        }
    }
}