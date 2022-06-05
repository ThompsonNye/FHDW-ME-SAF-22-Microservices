namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Kafka;

public class KafkaSettings
{
    public string UpdateConsumptionsTopic { get; set; } = null!;

    public string DeleteCarTopic { get; set; } = null!;
}