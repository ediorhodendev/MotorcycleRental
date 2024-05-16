using MongoDB.Driver;
using MotorcycleRental.Domain.Entities;

namespace MotorcycleRental.Infrastructure.Data
{
    public class MongoContext
    {
        private readonly IMongoDatabase _database;

        //public MongoContext(IConfiguration configuration)
        //{
        //    // Recupera a string de conexão do arquivo de configuração (appsettings.json)
        //    var connectionString = configuration.GetConnectionString("MongoDB");
        //    var mongoClient = new MongoClient(connectionString);

        //    // Define o nome do banco de dados
        //    var databaseName = configuration.GetValue<string>("MongoDBSettings:DatabaseName");
        //    _database = mongoClient.GetDatabase(databaseName);
        //}

        // Propriedades para acessar as coleções no banco de dados
        public IMongoCollection<Motorcycle> Motos => _database.GetCollection<Motorcycle>("Motos");
        public IMongoCollection<Domain.Entities.Delivery> Entregadores => _database.GetCollection<Domain.Entities.Delivery>("Entregadores");

        // Adicione outras coleções conforme necessário
    }
}
