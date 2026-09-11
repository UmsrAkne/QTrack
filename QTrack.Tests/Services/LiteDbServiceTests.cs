using LiteDB;
using QTrack.Services;

namespace QTrack.Tests.Services
{
    [TestFixture]
    public class LiteDbServiceTests
    {
        private ILiteDatabase database = null!;
        private LiteDbService service = null!;

        public class TestEntity
        {
            public int Id { get; set; }

            public string Name { get; set; } = string.Empty;
        }

        [SetUp]
        public void SetUp()
        {
            database = new LiteDatabase(new MemoryStream());
            service = new LiteDbService(database);
        }

        [TearDown]
        public void TearDown()
        {
            database.Dispose();
        }

        [Test]
        public void Constructor_NullDatabase_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new LiteDbService(null!));
        }

        [Test]
        public void Get_NonExistentEntity_ReturnsNull()
        {
            var result = service.Get<TestEntity>(999);
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Upsert_SingleEntity_InsertsAndUpdatesCorrectly()
        {
            var entity = new TestEntity { Id = 1, Name = "Item 1", };

            var insertResult = service.Upsert(entity);
            Assert.That(insertResult, Is.True);

            var retrieved = service.Get<TestEntity>(1);
            Assert.That(retrieved, Is.Not.Null);
            Assert.That(retrieved!.Name, Is.EqualTo("Item 1"));

            entity.Name = "Item 1 Updated";
            var updateResult = service.Upsert(entity);
            Assert.That(updateResult, Is.False);

            var retrievedUpdated = service.Get<TestEntity>(1);
            Assert.That(retrievedUpdated, Is.Not.Null);
            Assert.That(retrievedUpdated!.Name, Is.EqualTo("Item 1 Updated"));
        }

        [Test]
        public void GetAll_ReturnsAllEntities()
        {
            var entities = new[]
            {
                new TestEntity { Id = 1, Name = "A", },
                new TestEntity { Id = 2, Name = "B", },
                new TestEntity { Id = 3, Name = "C", },
            };

            service.Upsert<TestEntity>(entities);

            var all = service.GetAll<TestEntity>().ToList();
            Assert.That(all, Has.Count.EqualTo(3));
            Assert.That(all.Select(x => x.Name), Is.EquivalentTo(new[] { "A", "B", "C", }));
        }

        [Test]
        public void Upsert_MultipleEntities_UpsertsCorrectly()
        {
            var entities = new[]
            {
                new TestEntity { Id = 1, Name = "First", },
                new TestEntity { Id = 2, Name = "Second", },
            };

            var count = service.Upsert<TestEntity>(entities);
            Assert.That(count, Is.EqualTo(2));

            var updatedEntities = new[]
            {
                new TestEntity { Id = 1, Name = "First Updated", },
                new TestEntity { Id = 3, Name = "Third", },
            };

            var count2 = service.Upsert<TestEntity>(updatedEntities);
            Assert.That(count2, Is.EqualTo(1));

            var all = service.GetAll<TestEntity>().ToList();
            Assert.That(all, Has.Count.EqualTo(3));
            Assert.That(service.Get<TestEntity>(1)!.Name, Is.EqualTo("First Updated"));
            Assert.That(service.Get<TestEntity>(2)!.Name, Is.EqualTo("Second"));
            Assert.That(service.Get<TestEntity>(3)!.Name, Is.EqualTo("Third"));
        }

        [Test]
        public void GetAndUpsert_WithCustomCollectionName_WorksCorrectly()
        {
            var entity = new TestEntity { Id = 10, Name = "Custom", };
            const string customCollection = "custom_entities";

            service.Upsert(entity, customCollection);

            var defaultColResult = service.Get<TestEntity>(10);
            Assert.That(defaultColResult, Is.Null);

            var customColResult = service.Get<TestEntity>(10, customCollection);
            Assert.That(customColResult, Is.Not.Null);
            Assert.That(customColResult!.Name, Is.EqualTo("Custom"));

            var allCustom = service.GetAll<TestEntity>(customCollection).ToList();
            Assert.That(allCustom, Has.Count.EqualTo(1));
        }

        [Test]
        public void Upsert_NullEntity_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => service.Upsert<TestEntity>((TestEntity)null!));
            Assert.Throws<ArgumentNullException>(() => service.Upsert<TestEntity>((IEnumerable<TestEntity>)null!));
        }
    }
}