namespace MotorcycleRental.Domain.Interfaces.Menssaging
{

    public interface IKafkaConsumer_livre
    {
        Task ConsumeAsync();
    }
}
