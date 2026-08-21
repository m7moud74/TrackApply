using RabbitMQ.Client;

public class RabbitMqConnectionProvider : IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;
    private readonly SemaphoreSlim _semaphore = new(1, 1);

    public RabbitMqConnectionProvider(IConfiguration configuration)
    {
        _factory = new ConnectionFactory
        {
            HostName = configuration.GetValue<string>("RabbitMQ:HostName") ?? "localhost"
        };
    }

    public async ValueTask<IConnection> GetConnectionAsync()
    {
        if (_connection is not null) 
            return _connection;

        await _semaphore.WaitAsync();
        try
        {
            _connection ??= await _factory.CreateConnectionAsync();
            return _connection;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null) 
            await _connection.DisposeAsync();
    }
}