using System.Net;
using QTrack.Models;
using QTrack.Services;
using QTrack.Utils;

namespace QTrack.Tests.Services
{
    [TestFixture]
    public class IssueServiceTests
    {
        private class FakeHttpMessageHandler : HttpMessageHandler
        {
            public HttpRequestMessage? LastRequest { get; private set; }

            public HttpResponseMessage ResponseToReturn { get; set; } = new (HttpStatusCode.OK)
            {
                Content = new StringContent("[]"),
            };

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                LastRequest = request;
                return Task.FromResult(ResponseToReturn);
            }
        }

        [Test]
        public async Task GetIssuesAsync_WithCriteria_SendsExpectedQueryAndTop()
        {
            var handler = new FakeHttpMessageHandler();
            var httpClient = new HttpClient(handler);
            var credentials = new ApiCredentials
            {
                YoutrackApiKey = "test-key",
                YoutrackIssuesEndpoint = "https://example.youtrack.cloud/api/issues",
                YoutrackProjectsEndpoint = "https://example.youtrack.cloud/api/projects",
            };
            var service = new IssueService(credentials, new AppSettings(), httpClient);

            var criteria = new IssueSearchCriteria
            {
                FromDate = new DateTime(2026, 9, 15),
                Top = 10,
                SortByUpdatedDesc = false,
            };

            await service.GetIssuesAsync(criteria);

            Assert.That(handler.LastRequest, Is.Not.Null);
            var requestUri = handler.LastRequest!.RequestUri!.ToString();

            Assert.That(requestUri, Does.Contain("query=updated:%202026-09-15%20..%20*").Or.Contain("query=updated: 2026-09-15 .. *"));
            Assert.That(requestUri, Does.Contain("$top=10"));
        }
    }
}