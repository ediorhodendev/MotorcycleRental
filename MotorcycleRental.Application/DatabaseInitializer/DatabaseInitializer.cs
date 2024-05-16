using MongoDB.Driver;
using MotorcycleRental.Domain.Entities;
using MotorcycleRental.Domain.Entities.MotorcycleRental.Domain.Entities;


namespace MotorcycleRental.Application.DatabaseInitializer
{

    public class DatabaseInitializer
    {
        private readonly IMongoDatabase _database;

        public DatabaseInitializer(string connectionString, string databaseName)
        {
            var client = new MongoClient(connectionString);
            _database = client.GetDatabase(databaseName);
        }

        public void Initialize()
        {
            CreateCollectionIfNotExists("Deliverys");
            CreateCollectionIfNotExists("Motorcycles");
            CreateCollectionIfNotExists("Rentals");
            CreateCollectionIfNotExists("Plans");
            PopulateDeliverys();
            PopulateMotorcycles();
            PopulatePlans();
            PopulateRentals();
        }

        private void CreateCollectionIfNotExists(string collectionName)
        {
            var collectionList = _database.ListCollectionNames().ToList();
            if (!collectionList.Contains(collectionName))
            {
                _database.CreateCollection(collectionName);
            }
        }

        private void PopulatePlans()
        {
            var plansCollection = _database.GetCollection<Plan>("Plans");
            if (plansCollection.EstimatedDocumentCount() == 0)
            {
                var plans = new List<Plan>
            {
                new Plan { Days = 7, CostPerDay = 30m, PenaltyPercentage = 20.0 },
                new Plan { Days = 15, CostPerDay = 28m, PenaltyPercentage = 40.0 },
                new Plan { Days = 30, CostPerDay = 22m, PenaltyPercentage = 0.0 },
                new Plan { Days = 45, CostPerDay = 20m, PenaltyPercentage = 0.0 },
                new Plan { Days = 50, CostPerDay = 18m, PenaltyPercentage = 0.0 }
            };

                plansCollection.InsertMany(plans);
            }
        }

        private void PopulateDeliverys()
        {
            var deliveriesCollection = _database.GetCollection<Delivery>("Deliverys");
            if (deliveriesCollection.EstimatedDocumentCount() == 0)
            {
                var newDeliveries = Enumerable.Range(1, 10).Select(i => new Delivery
                {
                    Name = $"Deliverer {i}",
                    Cnpj = $"1234567890123{i}",
                    BirthDate = DateTime.Now.AddYears(-25 - i),
                    LicenseNumber = $"AB12345{i}",
                    LicenseType = i % 3 == 0 ? "A+B" : (i % 2 == 0 ? "B" : "A"),
                    LicenseImagePath = $"path/to/image{i}.png"
                }).ToList();

                deliveriesCollection.InsertMany(newDeliveries);
            }
        }

        private void PopulateMotorcycles()
        {
            var motorcyclesCollection = _database.GetCollection<Motorcycle>("Motorcycles");
            if (motorcyclesCollection.EstimatedDocumentCount() == 0)
            {
                var newMotorcycles = Enumerable.Range(1, 10).Select(i => new Motorcycle
                {
                    Identifier = $"Moto00{i}",
                    Year = 2020 - i,
                    Model = $"Model {i}",
                    LicensePlate = $"XYZ12{i:00}"
                }).ToList();

                motorcyclesCollection.InsertMany(newMotorcycles);
            }
        }

        private void PopulateRentals()
        {
            var rentalsCollection = _database.GetCollection<Rental>("Rentals");
            if (rentalsCollection.EstimatedDocumentCount() == 0)
            {
                var plans = _database.GetCollection<Plan>("Plans").Find(_ => true).ToList();
                var motorcycles = _database.GetCollection<Motorcycle>("Motorcycles").Find(_ => true).ToList();
                var deliverys = _database.GetCollection<Delivery>("Deliverys").Find(_ => true).ToList();

                var newRentals = Enumerable.Range(1, 10).Select(i => new Rental
                {
                    MotorcycleId = motorcycles[i % motorcycles.Count].Id,
                    DeliveryPersonId = deliverys[i % deliverys.Count].Id,
                    StartDate = DateTime.Now.AddDays(-i),
                    EndDate = DateTime.Now.AddDays(i),
                    PredictedEndDate = DateTime.Now.AddDays(i),
                    PlanId = plans[i % plans.Count].Id, // Ensuring each rental gets a plan
                    DailyRate = plans[i % plans.Count].CostPerDay,
                    TotalCost = plans[i % plans.Count].CostPerDay * i
                }).ToList();

                rentalsCollection.InsertMany(newRentals);
            }
        }
    }
}
