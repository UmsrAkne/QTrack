namespace QTrack.Models
{
    public class IssueSearchCriteria
    {
        public string? Project { get; set; }

        public string? State { get; set; }

        public string? Keyword { get; set; }

        public bool SortByUpdatedDesc { get; set; } = true;

        /// <summary>
        /// YouTrack API 用のクエリ文字列 を生成します。
        /// </summary>
        /// <returns>生成されたクエリ文字列</returns>
        public string ToQueryString()
        {
            var parts = new List<string>();

            if (!string.IsNullOrWhiteSpace(Project))
            {
                parts.Add($"project: {Project}");
            }

            if (!string.IsNullOrWhiteSpace(State))
            {
                parts.Add($"state: {State}");
            }

            if (!string.IsNullOrWhiteSpace(Keyword))
            {
                parts.Add(Keyword);
            }

            if (SortByUpdatedDesc)
            {
                parts.Add("sort by: updated desc");
            }

            return string.Join(" ", parts);
        }
    }
}