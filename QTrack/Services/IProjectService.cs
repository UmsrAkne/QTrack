using QTrack.Models;

namespace QTrack.Services
{
    public interface IProjectService
    {
        /// <summary>
        /// 取得可能な全てのプロジェクトを取得します。
        /// </summary>
        /// <returns>権限内で取得可能な全てのプロジェクト</returns>
        Task<IEnumerable<Project>> GetAllProjectsAsync();
    }
}