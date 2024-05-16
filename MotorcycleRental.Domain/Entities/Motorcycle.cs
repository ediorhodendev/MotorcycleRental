using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MotorcycleRental.Domain.Entities
{
    public class Motorcycle
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("identifier")]
        public string Identifier { get; set; }

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("model")]
        public string Model { get; set; }

        [BsonElement("licenseplate")]
        public string LicensePlate { get; set; }  
    }

}


