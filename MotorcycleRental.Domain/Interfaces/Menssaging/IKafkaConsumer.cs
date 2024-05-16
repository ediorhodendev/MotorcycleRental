namespace MotorcycleRental.Domain.Interfaces.Menssaging
{
    public interface IKafkaConsumer
    {
        Task ConsumeAsync();
    }
}


