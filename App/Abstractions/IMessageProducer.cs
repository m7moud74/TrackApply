public interface IMessageProducer
{
    Task PublishMessage<T>(T message, string queueName);
}