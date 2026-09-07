using System.Text.Json;
using QTrack.Services.DTOs;

namespace QTrack.Tests.Services
{
    [TestFixture]
    public class IssueDtoTests
    {
            private const string SampleJson = """
    [
        {
            "customFields": [
                {
                    "value": {
                        "name": "Normal",
                        "$type": "EnumBundleElement"
                    },
                    "name": "Priority",
                    "$type": "SingleEnumIssueCustomField"
                },
                {
                    "value": {
                        "name": "Feature",
                        "$type": "EnumBundleElement"
                    },
                    "name": "Type",
                    "$type": "SingleEnumIssueCustomField"
                },
                {
                    "value": {
                        "name": "未完了",
                        "$type": "StateBundleElement"
                    },
                    "name": "State",
                    "$type": "StateIssueCustomField"
                },
                {
                    "value": null,
                    "name": "Assignee",
                    "$type": "SingleUserIssueCustomField"
                },
                {
                    "value": null,
                    "name": "Subsystem",
                    "$type": "SingleOwnedIssueCustomField"
                },
                {
                    "value": null,
                    "name": "Due Date",
                    "$type": "DateIssueCustomField"
                },
                {
                    "value": null,
                    "name": "予測",
                    "$type": "PeriodIssueCustomField"
                },
                {
                    "value": null,
                    "name": "経過時間",
                    "$type": "PeriodIssueCustomField"
                },
                {
                    "value": null,
                    "name": "EntryNo",
                    "$type": "SimpleIssueCustomField"
                },
                {
                    "value": null,
                    "name": "Rate",
                    "$type": "SimpleIssueCustomField"
                }
            ],
            "updated": 1788688535945,
            "summary": "デバッグ用プロジェクトの課題_1",
            "links": [
                {
                    "direction": "BOTH",
                    "linkType": {
                        "name": "Relates",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                },
                {
                    "direction": "OUTWARD",
                    "linkType": {
                        "name": "Depend",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                },
                {
                    "direction": "INWARD",
                    "linkType": {
                        "name": "Depend",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                },
                {
                    "direction": "OUTWARD",
                    "linkType": {
                        "name": "Duplicate",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                },
                {
                    "direction": "INWARD",
                    "linkType": {
                        "name": "Duplicate",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                },
                {
                    "direction": "OUTWARD",
                    "linkType": {
                        "name": "Subtask",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                },
                {
                    "direction": "INWARD",
                    "linkType": {
                        "name": "Subtask",
                        "$type": "IssueLinkType"
                    },
                    "issues": [],
                    "$type": "IssueLink"
                }
            ],
            "idReadable": "DEB-1",
            "description": "デバッグプロジェクトの課題の説明です(1)",
            "id": "2-7382",
            "$type": "Issue"
        }
    ]
    """;

        [Test]
        public void Deserialize_ValidJson_ReturnsIssueList()
        {
            var issues = JsonSerializer.Deserialize<List<IssueDto>>(SampleJson);

            Assert.That(issues, Is.Not.Null);
            Assert.That(issues.Count, Is.EqualTo(1));

            var issue = issues[0];
            Assert.That(issue.Id, Is.EqualTo("2-7382"));
            Assert.That(issue.IdReadable, Is.EqualTo("DEB-1"));
            Assert.That(issue.Summary, Is.EqualTo("デバッグ用プロジェクトの課題_1"));
            Assert.That(issue.Updated, Is.EqualTo(1788688535945));

            var priorityField = issue.CustomFields?.FirstOrDefault(f => f.Name == "Priority");
            Assert.That(priorityField?.Value?.GetProperty("name").GetString(), Is.EqualTo("Normal"));
        }

        [Test]
        public void ToModel()
        {
            var issues = JsonSerializer.Deserialize<List<IssueDto>>(SampleJson)
                .Select(dto => dto.ToModel())
                .ToList();

            Assert.That(issues, Is.Not.Null);
            Assert.That(issues.Count, Is.EqualTo(1));

            var issue = issues[0];
            Assert.That(issue.IdReadable, Is.EqualTo("DEB-1"));
            Assert.That(issue.Summary, Is.EqualTo("デバッグ用プロジェクトの課題_1"));
            Assert.That(issue.Priority, Is.EqualTo("Normal"));
            Assert.That(issue.UpdatedAt, Is.Not.EqualTo(new DateTime()));
        }
    }
}