using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

public class RabbitMqMessageProducer(IConfiguration configuration) : IMessageProducer
{
    public async Task PublishMessage<T>(T message, string queueName)
    {
        var hostName = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost";
        var factory = new ConnectionFactory { HostName = hostName };
        
        await using var connection = await factory.CreateConnectionAsync();
          await using var channel =  await connection.CreateChannelAsync(); // الـ Channel هنا غير بتاعت .NET، دي بتاعت RabbitMQ

        // 2. إعلان الطابور (عشان نتأكد إنه موجود قبل ما نبعت)
        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,      // true يعني الرسايل متضيعش لو سيرفر RabbitMQ عمل ريستارت
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(
            exchange: "", 
            routingKey: queueName, 
            body: body);
    }
}