using System.Text;
using System.Text.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class RabbitMqEmailWorker(
    RabbitMqConnectionProvider connectionProvider, 
    ILogger<RabbitMqEmailWorker> logger) : BackgroundService
{
    private IChannel? _channel;
    private const string QueueName = "EmailChangedStatus";

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("RabbitMQ Email Worker is starting...");

        var connection = await connectionProvider.GetConnectionAsync();
        _channel = await connection.CreateChannelAsync(cancellationToken: stoppingToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: stoppingToken);

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (sender, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var @event = JsonSerializer.Deserialize<ApplicationStatusChangedEvent>(json);

                logger.LogInformation("Processing email for {Email}... Status: {Status}", 
                    @event?.UserEmail, @event?.NewStatus);
                await Task.Delay(2000, stoppingToken);

                logger.LogInformation("Email sent successfully to {Email}!", @event?.UserEmail);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false, cancellationToken: stoppingToken);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to process message. Putting it back to the queue.");
                await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true, cancellationToken: stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false, 
            consumer: consumer,
            cancellationToken: stoppingToken);

        try
        {
            await Task.Delay(Timeout.Infinite, stoppingToken);
        }
        catch (TaskCanceledException)
        {
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null)
        {
            await _channel.CloseAsync(cancellationToken: cancellationToken);
            await _channel.DisposeAsync();
        }
        await base.StopAsync(cancellationToken);
    }
}