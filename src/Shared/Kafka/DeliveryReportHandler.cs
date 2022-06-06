using Confluent.Kafka;
using Microsoft.Extensions.Logging;

namespace Nuyken.Vegasco.Backend.Microservices.Shared.Kafka;

public static class DeliveryReportHandler
{
    public static void Handle<TKey, TValue>(DeliveryReport<TKey, TValue> deliveryReport, ILogger? logger)
    {
        if (deliveryReport.Status == PersistenceStatus.NotPersisted)
        {
            return;
        }

        if (logger is not null)
        {
            logger.LogWarning("Message delivery failed with reason: {Reason}", deliveryReport.Message.Value);
            return;
        }
        
        Console.Error.WriteLine("Message delivery failed with reason: {0}", deliveryReport.Message.Value);
    }
}