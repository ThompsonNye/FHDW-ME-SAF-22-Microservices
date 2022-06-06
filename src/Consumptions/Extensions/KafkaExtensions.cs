using Confluent.Kafka;
using Nuyken.Vegasco.Backend.Microservices.Shared.Kafka;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Extensions;

public static class KafkaExtensions
{
    public static void AddKafka(this IServiceCollection services)
    {
        services
            .AddSingleton<KafkaClientHandle>()
            .AddSingleton<KafkaDependentProducer<Null, string>>();
    }
}