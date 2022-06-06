using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;

namespace Nuyken.Vegasco.Backend.Microservices.Shared.Kafka.Extensions;

public static class KafkaExtensions
{
    public static void AddKafka(this IServiceCollection services)
    {
        services
            .AddSingleton<KafkaClientHandle>()
            .AddSingleton<KafkaDependentProducer<Null, string>>();
    }
}