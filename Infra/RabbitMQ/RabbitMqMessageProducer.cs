using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

public class RabbitMqMessageProducer(RabbitMqConnectionProvider provider) : IMessageProducer
{
    public async Task PublishMessage<T>(T message, string queueName,CancellationToken cancellationToken)
    {
        var connection = await provider.GetConnectionAsync();
        await using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);
        
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,      
            exclusive: false,
            autoDelete: false,
            arguments: null,cancellationToken :cancellationToken);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            body: body,
            cancellationToken:cancellationToken);
    }
}