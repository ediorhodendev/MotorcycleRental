
using Confluent.Kafka;
using Newtonsoft.Json;
using System;
using MotorcycleRental.Domain.Entities;
using MongoDB.Driver;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using MotorcycleRental.Domain.Interfaces.Menssaging;
using MongoDB.Bson;

namespace MotorcycleRental.Infrastructure.Messaging
{
    public class KafkaConsumer : IKafkaConsumer
    {
        private readonly IMongoDatabase _database;
        private readonly string _bootstrapServers;
        private readonly string _topic;
        private readonly IMongoCollection<MotorcycleEvent> _eventsCollection;
        private readonly ILogger<KafkaConsumer> _logger;

        public KafkaConsumer(string bootstrapServers, string topic, IMongoDatabase database, ILogger<KafkaConsumer> logger)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
            _eventsCollection = database.GetCollection<MotorcycleEvent>("MotorcycleEvents");
            _logger = logger;
            _database = database;
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
                
                while (true)
                {
                    var consumeResult = consumer.Consume();
                    if (consumeResult.Message != null && consumeResult.Message.Value.Contains("2024"))
                    {
                        await StoreEventAsync(consumeResult.Message.Value);
                    }
                }

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
        private async Task StoreEventAsync(string message)
        {
            var collection = _database.GetCollection<BsonDocument>("MotorcycleEvents");
            var document = new BsonDocument { ["Message"] = message, ["ReceivedAt"] = DateTime.UtcNow };
            await collection.InsertOneAsync(document);
            _logger.LogInformation($"Stored new motorcycle event: {message}");
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
