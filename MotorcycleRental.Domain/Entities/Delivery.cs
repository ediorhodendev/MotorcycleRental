using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MotorcycleRental.Domain.Entities
{
    public class Delivery
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("cnpj")]
        public string Cnpj { get; set; }  // CNPJ �nico

        [BsonElement("birthDate")]
        public DateTime BirthDate { get; set; }

        [BsonElement("licenseNumber")]
        public string LicenseNumber { get; set; }  // N�mero da CNH �nico

        [BsonElement("licenseType")]
        public string LicenseType { get; set; }  // 'A', 'B', 'A+B'

        [BsonElement("LicenseImagePath")] // Ignora pois a imagem n�o ser� armazenada no banco
        public string LicenseImagePath { get; set; }  // Caminho para a imagem da CNH

      
    }
}




