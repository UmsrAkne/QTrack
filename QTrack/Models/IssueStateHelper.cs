using QTrack.Utils;

namespace QTrack.Models
{
    public class IssueStateHelper
    {
        public static IssueState ToIssueState(string? state)
        {
            return state switch
            {
                "未着手" => IssueState.Created,
                "作業中" => IssueState.InProgress,
                "中断" => IssueState.Pausing,
                "完了" => IssueState.Completed,
                "廃止" => IssueState.Obsolete,
                _ => HandleUnknownState(state),
            };
        }

        public static string ToString(IssueState state)
        {
            return state switch
            {
                IssueState.Created => "未着手",
                IssueState.InProgress => "作業中",
                IssueState.Pausing => "中断",
                IssueState.Completed => "完了",
                IssueState.Obsolete => "廃止",
                _ => throw new ArgumentOutOfRangeException(nameof(state), state, null),
            };
        }

        private static IssueState HandleUnknownState(string? state)
        {
            AppLogger.Warn($"Unknown state: {state}");
            AppLogger.Warn($"return {IssueState.Created} (default)");
            return IssueState.Created;
        }
    }
}