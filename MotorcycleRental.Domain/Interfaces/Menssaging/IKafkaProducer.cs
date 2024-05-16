namespace MotorcycleRental.Domain.Interfaces.Menssaging
{
    public interface IKafkaProducer
    {
        Task SendMessageAsync(string message);
    }
}


