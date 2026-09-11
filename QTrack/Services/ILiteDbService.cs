using LiteDB;

namespace QTrack.Services
{
    public interface ILiteDbService
    {
        /// <summary>
        /// 指定されたIDに対応するエンティティを取得します。
        /// </summary>
        /// <typeparam name="T">エンティティの型</typeparam>
        /// <param name="id">取得対象のID</param>
        /// <param name="collectionName">コレクション名（省略時は型名が使用されます）</param>
        /// <returns>取得したエンティティ。存在しない場合はnull。</returns>
        T? Get<T>(BsonValue id, string? collectionName = null);

        /// <summary>
        /// 全てのエンティティを取得します。
        /// </summary>
        /// <typeparam name="T">エンティティの型</typeparam>
        /// <param name="collectionName">コレクション名（省略時は型名が使用されます）</param>
        /// <returns>エンティティのコレクション</returns>
        IEnumerable<T> GetAll<T>(string? collectionName = null);

        /// <summary>
        /// エンティティを挿入または更新（アップサート）します。
        /// </summary>
        /// <typeparam name="T">エンティティの型</typeparam>
        /// <param name="entity">対象エンティティ</param>
        /// <param name="collectionName">コレクション名（省略時は型名が使用されます）</param>
        /// <returns>新規挿入された場合は true、更新された場合は false</returns>
        bool Upsert<T>(T entity, string? collectionName = null);

        /// <summary>
        /// 複数のエンティティを一括で挿入または更新（アップサート）します。
        /// </summary>
        /// <typeparam name="T">エンティティの型</typeparam>
        /// <param name="entities">対象エンティティのコレクション</param>
        /// <param name="collectionName">コレクション名（省略時は型名が使用されます）</param>
        /// <returns>新規挿入されたエンティティの件数</returns>
        int Upsert<T>(IEnumerable<T> entities, string? collectionName = null);
    }
}