using Confluent.Kafka;

namespace Nuyken.Vegasco.Backend.Microservices.Consumptions.Kafka;

public static class DeliveryReportHandler
{
    public static void Handle<TKey, TValue>(DeliveryReport<TKey, TValue> deliveryReport, ILogger logger)
    {
        if (deliveryReport.Status == PersistenceStatus.NotPersisted)
        {
            return;
        }
        
        logger.LogWarning("Message delivery failed with reason: {Reason}", deliveryReport.Message.Value);
    }
}