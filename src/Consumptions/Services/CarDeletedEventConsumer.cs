using System.Text.Json;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Entities;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Events;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Services;

public class CarDeletedEventConsumer : BackgroundService
{
    private const string TopicKey = "Kafka:DeleteCarsTopic";
    private const string ConsumerSettingsKey = "Kafka:ConsumerSettings";
    
    private readonly string _topic;
    private readonly IApplicationDbContext _dbContext;
    private readonly ILogger<CarDeletedEventConsumer> _logger;
    private readonly IConsumer<Null, string> _consumer;

    public CarDeletedEventConsumer(IConfiguration configuration, IApplicationDbContext dbContext, ILogger<CarDeletedEventConsumer> logger)
    {
        _topic = configuration.GetValue<string>(TopicKey);

        var consumerConfig = new ConsumerConfig();
        configuration.GetSection(ConsumerSettingsKey).Bind(consumerConfig);
        _consumer = new ConsumerBuilder<Null, string>(consumerConfig).Build();
        
        _dbContext = dbContext;
        _logger = logger;
    }
    
    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (string.IsNullOrEmpty(_topic))
        {
            _logger.LogWarning("Could not find the delete car topic name in the configuration, cannot start consumer");
            return Task.CompletedTask;
        }

#pragma warning disable CS4014
        new Thread(() => StartConsumerLoop(stoppingToken)).Start();
#pragma warning restore CS4014
        return Task.CompletedTask;
    }

    private async Task StartConsumerLoop(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                _logger.LogDebug("Message received: {Value}", consumeResult.Message.Value);
                try
                {
                    var deletedCar = JsonSerializer.Deserialize<CarDeletedEvent>(consumeResult.Message.Value)!;
                    var carId = new CarId(deletedCar.Id);
                    var consumptionsForDeletedCar = await _dbContext.Consumptions
                        .Where(x => x.CarId == carId)
                        .ToListAsync(cancellationToken);

                    if (!consumptionsForDeletedCar.Any())
                    {
                        _logger.LogDebug("No consumptions found for car with id {CarId}", carId);
                        continue;
                    }
                    
                    _dbContext.Consumptions.RemoveRange(consumptionsForDeletedCar);
                    await _dbContext.SaveChangesAsync(cancellationToken);
                    
                    _logger.LogInformation("Deleted consumptions for car with id {CarId}", carId);
                }
                catch (OperationCanceledException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    _logger.LogWarning(e, "Could not process deleted car event with value: {Value}", consumeResult.Message.Value);
                }
            }
            catch (OperationCanceledException)
            {
                break;
            }
            catch (ConsumeException e)
            {
                if (e.Error.IsFatal)
                {
                    _logger.LogError("Consume error: {Reason}", e.Error.Reason);
                    break;
                }
                
                _logger.LogInformation("Consume error: {Reason}", e.Error.Reason);
            }
            catch (Exception e)
            {
                _logger.LogInformation("Unexpected error: {Reason}", e);
                break;
            }
        }
    }
}