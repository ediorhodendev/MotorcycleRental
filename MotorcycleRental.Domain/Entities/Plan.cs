using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace MotorcycleRental.Domain.Entities
{

    namespace MotorcycleRental.Domain.Entities
    {
        public class Plan
        {
            [BsonId]
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            [BsonElement("days")]
            public int Days { get; set; }

            [BsonElement("costPerDay")]
            public decimal CostPerDay { get; set; }

            [BsonElement("penaltyPercentage")]
            public double PenaltyPercentage { get; set; }
        }
    }
}