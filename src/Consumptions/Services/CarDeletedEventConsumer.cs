using Confluent.Kafka;
using Nuyken.Vegasco.Backend.Microservices.Consumptions.Models.Abstractions;

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

        new Thread(() => StartConsumerLoop(stoppingToken)).Start();
        return Task.CompletedTask;
    }

    private void StartConsumerLoop(CancellationToken cancellationToken)
    {
        _consumer.Subscribe(_topic);
        
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var consumeResult = _consumer.Consume(cancellationToken);

                // Handle message...
                _logger.LogInformation("Message received: {Value}", consumeResult.Message.Value);
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