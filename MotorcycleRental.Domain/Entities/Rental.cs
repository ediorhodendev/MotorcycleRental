using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace MotorcycleRental.Domain.Entities
{

    public class Rental
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("motorcycleId")]
        public string MotorcycleId { get; set; }

        [BsonElement("deliveryPersonId")]
        public string DeliveryPersonId { get; set; }

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime EndDate { get; set; }

        [BsonElement("predictedEndDate")]
        public DateTime PredictedEndDate { get; set; }

        [BsonElement("planId")]
        public string PlanId { get; set; }

        [BsonElement("dailyRate")]
        public decimal DailyRate { get; set; }

        [BsonElement("totalCost")]
        public decimal TotalCost { get; set; }
    }

}


