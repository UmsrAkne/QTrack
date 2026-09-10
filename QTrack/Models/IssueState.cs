namespace QTrack.Models
{
    public enum IssueState
    {
        /// <summary>
        /// 作成されてからまだ一度も作業をしていない状態。
        /// </summary>
        Created,

        /// <summary>
        /// 作業中の状態。
        /// </summary>
        InProgress,

        /// <summary>
        /// 一度作業開始したが、中断した状態。
        /// </summary>
        Pausing,

        /// <summary>
        /// 作業が完了した状態。
        /// </summary>
        Completed,

        /// <summary>
        /// 作業再開の見込みがない、または不要となった課題につけられる状態。
        /// </summary>
        Obsolete,
    }
}