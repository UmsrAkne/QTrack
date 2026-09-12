using LiteDB;
using QTrack.Utils;

namespace QTrack.Services
{
    public class LiteDbService : ILiteDbService
    {
        private readonly ILiteDatabase db;

        public LiteDbService(ILiteDatabase db)
        {
            AppLogger.Info("LiteDbService created");
            this.db = db ?? throw new ArgumentNullException(nameof(db));
        }

        /// <inheritdoc />
        public T? Get<T>(BsonValue id, string? collectionName = null)
        {
            var collection = string.IsNullOrEmpty(collectionName)
                ? db.GetCollection<T>()
                : db.GetCollection<T>(collectionName);

            return collection.FindById(id);
        }

        /// <inheritdoc />
        public IEnumerable<T> GetAll<T>(string? collectionName = null)
        {
            var collection = string.IsNullOrEmpty(collectionName)
                ? db.GetCollection<T>()
                : db.GetCollection<T>(collectionName);

            return collection.FindAll();
        }

        /// <inheritdoc />
        public bool Upsert<T>(T entity, string? collectionName = null)
        {
            ArgumentNullException.ThrowIfNull(entity);

            var collection = string.IsNullOrEmpty(collectionName)
                ? db.GetCollection<T>()
                : db.GetCollection<T>(collectionName);

            return collection.Upsert(entity);
        }

        /// <inheritdoc />
        public int Upsert<T>(IEnumerable<T> entities, string? collectionName = null)
        {
            ArgumentNullException.ThrowIfNull(entities);

            var collection = string.IsNullOrEmpty(collectionName)
                ? db.GetCollection<T>()
                : db.GetCollection<T>(collectionName);

            return collection.Upsert(entities);
        }
    }
}