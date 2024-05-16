using Amazon.S3;
using MediatR;
using MongoDB.Driver;
using MotorcycleRental.Application.DatabaseInitializer;
using MotorcycleRental.Application.Handlers;

using MotorcycleRental.Domain.Interfaces;
using MotorcycleRental.Domain.Interfaces.Menssaging;
using MotorcycleRental.Infrastructure.Messaging;
using MotorcycleRental.Infrastructure.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Configure Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MongoDB Configuration
var mongoConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
var mongoClient = new MongoClient(mongoConnectionString);
var mongoDatabase = mongoClient.GetDatabase(builder.Configuration.GetConnectionString("MongoDBDatabaseName"));
builder.Services.AddSingleton<IMongoDatabase>(mongoDatabase);

// Configuração do serviço S3
var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonS3>();


DatabaseInitializer dbInitializer = new DatabaseInitializer(mongoConnectionString, mongoDatabase.DatabaseNamespace.DatabaseName);
dbInitializer.Initialize();



// Kafka Configuration
builder.Services.AddSingleton<IKafkaProducer>(sp => {
    var kafkaConfig = builder.Configuration.GetSection("Kafka");
    return new KafkaProducer(kafkaConfig["BootstrapServers"], kafkaConfig["Topic"]);
});
builder.Services.AddSingleton<IKafkaConsumer>(sp => {
    var logger = sp.GetRequiredService<ILogger<KafkaConsumer>>();
    return new KafkaConsumer(builder.Configuration["Kafka:BootstrapServers"], builder.Configuration["Kafka:Topic"], mongoDatabase, logger);
});

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(typeof(MotorcycleHandlers).Assembly);

// Repository Registrations

builder.Services.AddScoped<IMotorcycleRepository, MotorcycleRepository>();
builder.Services.AddScoped<IDeliveryRepository, DeliveryRepository>();
builder.Services.AddScoped<IRentalRepository, RentalRepository>();
builder.Services.AddScoped<IPlanRepository, PlanRepository>();


builder.Services.AddMediatR(Assembly.GetExecutingAssembly()); 

var app = builder.Build();



// Configure Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    var kafkaConsumer = app.Services.GetRequiredService<IKafkaConsumer>();
    Task.Run(() => kafkaConsumer.ConsumeAsync());

}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
