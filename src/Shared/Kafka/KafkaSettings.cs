namespace Nuyken.Vegasco.Backend.Microservices.Shared.Kafka;

public class KafkaSettings
{
    public string UpdateConsumptionsTopic { get; set; } = null!;

    public string DeleteCarTopic { get; set; } = null!;
}