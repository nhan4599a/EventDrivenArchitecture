using Global.Shared.Extensions;
using Global.Shared.Settings;
using Infrastructure.Configuration;
using Infrastructure.Constants;
using Infrastructure.Conventions;
using Infrastructure.DataAccess.Abstraction;
using Infrastructure.DataAccess.Attributes;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Reflection;

namespace Infrastructure.DataAccess.DbContexts
{
    public class NonRelationalDbContext : INonRelationalDbContext
    {
        private const string COLLECTION_METHOD_NAME = "Collection";

        private readonly IMongoDatabase _database;

        protected virtual Assembly MigrationAssembly { get; }

        protected virtual Assembly? ConventionAssembly { get; }

        public IMongoClient Client { get; }

        public IClientSessionHandle Session { get; }

        public IMongoDatabase Database => _database;

        public NonRelationalDbContext(NonRelationalDatabaseSetting databaseSetting)
        {
            Client = new MongoClient(databaseSetting.ConnectionString);

            _database = Client.GetDatabase(databaseSetting.DatabaseName);

            MigrationAssembly ??= Assembly.GetExecutingAssembly();

            ConfigureConventions();

            InitializeCollections();

            Session = Client.StartSession();
        }

        public IMongoCollection<TEntity> Collection<TEntity>()
        {
            var typeOfTEntity = typeof(TEntity);
            var tableNameAttribute = typeOfTEntity.GetCustomAttribute<TableNameAttribute>();
            var collectionName = tableNameAttribute == null ? typeOfTEntity.Name : tableNameAttribute.TableName;
            return _database.GetCollection<TEntity>(collectionName.ToLower().ToPluralForm());
        }

        private void InitializeCollections()
        {
            var genericCollectionType = typeof(IMongoCollection<>);
            var genericConfigurationType = typeof(INonRelationalDbEntityConfiguration<>);

            var typeOfThisClass = GetType();

            var collections = typeOfThisClass
                                .GetRuntimeProperties()
                                .Where(e => e.SetMethod != null
                                            && e.SetMethod.IsPublic
                                            && e.PropertyType.GetGenericTypeDefinition() == genericCollectionType);

            var typesInExecutingAssembly = MigrationAssembly.GetTypes();

            foreach (var collectionType in collections)
            {
                var entityType = collectionType.PropertyType.GenericTypeArguments[0];

                var underlyingCollection = typeOfThisClass.InvokeGeneric(COLLECTION_METHOD_NAME, this, entityType);

                collectionType.SetValue(this, underlyingCollection);

                var configurationType = typesInExecutingAssembly
                                            .FirstOrDefault(e =>
                                            {
                                                var interfaces = e.GetInterfaces();
                                                return interfaces.Length == 1
                                                        && interfaces[0].IsGenericType
                                                        && interfaces[0].GetGenericTypeDefinition() == genericConfigurationType
                                                        && interfaces[0].GetGenericArguments()[0] == entityType;
                                            });

                if (configurationType != null)
                {
                    var configurationObject = configurationType.CreateInstance();

                    configurationType.Invoke(
                        InfrastructureConstants.Migration.CONFIGURE_METHOD_NAME,
                        configurationObject,
                        null,
                        underlyingCollection);
                }
            }
        }

        private void ConfigureConventions()
        {
            var conventionPack = new ConventionPack
            {
                new FieldNamingConvention()
            };
            if (ConventionAssembly != null)
            {
                var conventions = ConventionAssembly
                                    .GetTypes()
                                    .Where(e => e is IConvention);
                foreach (var convention in conventions)
                {
                    conventionPack.Add(convention as IConvention);
                }
            }
            ConventionRegistry.Register(
                InfrastructureConstants.Convention.DEFAULT_CONVENTIONS_PACK_NAME,
                conventionPack,
                e => true);
        }

        public void Dispose()
        {
            Session.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
