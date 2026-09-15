using QTrack.Models;

namespace QTrack.Tests.Models
{
    [TestFixture]
    public class IssueSearchCriteriaTests
    {
        [Test]
        public void ToQueryString_WithFromDate_ReturnsUpdatedQuery()
        {
            var criteria = new IssueSearchCriteria
            {
                FromDate = new DateTime(2026, 9, 15),
                SortByUpdatedDesc = false,
            };

            var query = criteria.ToQueryString();

            Assert.That(query, Is.EqualTo("updated: 2026-09-15 .. *"));
        }

        [Test]
        public void ToQueryString_WithFromDateAndSortByUpdatedDesc_ReturnsUpdatedAndSortQuery()
        {
            var criteria = new IssueSearchCriteria
            {
                FromDate = new DateTime(2026, 9, 15),
                SortByUpdatedDesc = true,
            };

            var query = criteria.ToQueryString();

            Assert.That(query, Is.EqualTo("updated: 2026-09-15 .. * sort by: updated desc"));
        }

        [Test]
        public void ToQueryString_WithAllParameters_CombinesCorrectly()
        {
            var criteria = new IssueSearchCriteria
            {
                Project = "TEST",
                State = "Open",
                Keyword = "bug",
                FromDate = new DateTime(2026, 9, 15),
                SortByUpdatedDesc = true,
            };

            var query = criteria.ToQueryString();

            Assert.That(query, Is.EqualTo("project: TEST state: Open bug updated: 2026-09-15 .. * sort by: updated desc"));
        }

        [Test]
        public void ToQueryString_WithoutFromDate_DoesNotIncludeUpdatedQuery()
        {
            var criteria = new IssueSearchCriteria
            {
                Project = "TEST",
                SortByUpdatedDesc = true,
            };

            var query = criteria.ToQueryString();

            Assert.That(query, Is.EqualTo("project: TEST sort by: updated desc"));
        }
    }
}
