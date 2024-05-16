namespace MotorcycleRental.Domain.Interfaces
{
    public interface IDeliveryRepository
    {
        Task<Entities.Delivery> GetDeliveryByIdAsync(string DeliveryId);
        Task<IEnumerable<Entities.Delivery>> GetAllDeliverysAsync();
        Task AddDeliveryAsync(Entities.Delivery Delivery);
        Task UpdateDeliveryAsync(Entities.Delivery Delivery);
        Task DeleteDeliveryAsync(string DeliveryId);
        Task<bool> CheckCNPJExistsAsync(string cnpj);
        Task<bool> CheckLicenseNumberExistsAsync(string cnhNumber);

    }
}
