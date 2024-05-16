using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using MotorcycleRental.Domain.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MotorcycleRental.Domain.Interfaces.Menssaging;

namespace MotorcycleRental.Infrastructure.Messaging
{
    public class KafkaConsumer_Livre : IKafkaConsumer_livre
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly IMongoCollection<MotorcycleEvent> _eventsCollection;
        private readonly ILogger<KafkaConsumer_Livre> _logger;

        public KafkaConsumer_Livre(string bootstrapServers, string topic, IMongoDatabase database, ILogger<KafkaConsumer_Livre> logger)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
            _eventsCollection = database.GetCollection<MotorcycleEvent>("MotorcycleEvents");
            _logger = logger;
        }

        public async Task ConsumeAsync()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _bootstrapServers,
                GroupId = "motorcycle-group",
                AutoOffsetReset = AutoOffsetReset.Earliest,
                EnableAutoCommit = false
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            try
            {
                var consumeResult = consumer.Consume();
                await ProcessMessage(consumeResult.Message.Value);
                consumer.Commit();
            }
            catch (ConsumeException ex)
            {
                _logger.LogError($"Error consuming message: {ex.Error.Reason}");
            }
            finally
            {
                consumer.Close();
            }
        }

        private async Task ProcessMessage(string message)
        {
            try
            {
                var motorcycle = JsonConvert.DeserializeObject<Motorcycle>(message);
                if (motorcycle != null && motorcycle.Year == 2024)
                {
                    var eventRecord = new MotorcycleEvent { MotorcycleId = motorcycle.Id, EventDetails = message };
                    await _eventsCollection.InsertOneAsync(eventRecord);
                }
            }
            catch (JsonException jsonEx)
            {
                _logger.LogError($"JSON Error: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Unexpected error: {ex.Message}");
            }
        }
    }
}
