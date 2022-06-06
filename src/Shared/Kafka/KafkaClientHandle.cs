using Confluent.Kafka;
using Microsoft.Extensions.Configuration;

namespace Nuyken.Vegasco.Backend.Microservices.Shared.Kafka;

public class KafkaClientHandle : IDisposable
{
    private readonly IProducer<byte[], byte[]> _producer;

    public KafkaClientHandle(IConfiguration config)
    {
        var conf = new ProducerConfig();
        config.GetSection("Kafka:ProducerSettings").Bind(conf);
        _producer = new ProducerBuilder<byte[], byte[]>(conf).Build();
    }

    public Handle Handle => _producer.Handle;

    public void Dispose()
    {
        _producer.Flush();
        _producer.Dispose();
        GC.SuppressFinalize(this);
    }
}