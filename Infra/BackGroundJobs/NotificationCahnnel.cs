using System.Threading.Channels;

public class NotificationCahnnel
{
    private readonly Channel<ApplicationStatusChangedEvent> _channel =
    Channel.CreateBounded<ApplicationStatusChangedEvent>(100);

    public async Task PublishAsync(ApplicationStatusChangedEvent @event, CancellationToken ct = default)
    {
        await _channel.Writer.WriteAsync(@event, ct);
    }

    public IAsyncEnumerable<ApplicationStatusChangedEvent> ReadAllAsync(CancellationToken ct = default)
    {
        return _channel.Reader.ReadAllAsync(ct);
    }
}