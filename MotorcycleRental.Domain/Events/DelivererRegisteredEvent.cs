using System;

namespace MotorcycleRental.Domain.Events
{
    public class DeliveryRegisteredEvent
    {
        public string DeliveryId { get; }
        public string Name { get; }
        public string CNPJ { get; }
        public string CNHNumber { get; }
        public string CNHType { get; }
        public string CNHPhotoUrl { get; }
        public DateTime RegistrationDate { get; }

        public DeliveryRegisteredEvent(string DeliveryId, string name, string cnpj, string cnhNumber, string cnhType, string cnhPhotoUrl, DateTime registrationDate)
        {
            DeliveryId = DeliveryId;
            Name = name;
            CNPJ = cnpj;
            CNHNumber = cnhNumber;
            CNHType = cnhType;
            CNHPhotoUrl = cnhPhotoUrl;
            RegistrationDate = registrationDate;
        }
    }
}
