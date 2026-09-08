using QTrack.Models;
using QTrack.Services;
using System.Diagnostics;

namespace QTrack.Tests.Services
{
    [TestFixture]
    public class MockIssueServiceTests
    {
        [Test]
        public async Task CreateIssueAsync_WaitAndReturnDummyValue()
        {
            // Arrange
            var service = new MockIssueService();
            var project = new Project { Id = "0-0", ShortName = "TEST", };
            var summary = "Test summary";
            var description = "Test description";

            var stopwatch = new Stopwatch();

            // Act
            stopwatch.Start();
            var result = await service.CreateIssueAsync(project, summary, description);
            stopwatch.Stop();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Summary, Is.EqualTo(summary));
            Assert.That(result.Description, Is.EqualTo(description));
            Assert.That(result.IdReadable, Is.EqualTo("TEST-DUMMY"));
            
            // 数秒（3秒）待機することを確認
            Assert.That(stopwatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(2900));
        }
    }
}