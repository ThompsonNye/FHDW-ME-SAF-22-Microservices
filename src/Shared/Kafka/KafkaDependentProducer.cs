using Confluent.Kafka;

namespace Nuyken.Vegasco.Backend.Microservices.Shared.Kafka;

public class KafkaDependentProducer<TKey, TValue>
{
    private readonly IProducer<TKey, TValue> _kafkaHandle;

    public KafkaDependentProducer(KafkaClientHandle handle)
    {
        _kafkaHandle = new DependentProducerBuilder<TKey, TValue>(handle.Handle).Build();
    }

    public Task<DeliveryResult<TKey, TValue>> ProduceAsync(string topic, Message<TKey, TValue> message)
    {
        return _kafkaHandle.ProduceAsync(topic, message);
    }

    public void Produce(string topic, Message<TKey, TValue> message,
        Action<DeliveryReport<TKey, TValue>>? deliveryHandler = null)
    {
        _kafkaHandle.Produce(topic, message, deliveryHandler);
    }

    public void Flush(TimeSpan timeout)
    {
        _kafkaHandle.Flush(timeout);
    }
}