using System.Text;
using RabbitMQ.Client;

public class Send
{
    public static void Main()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost",
            Port = 5672,
            Password = "rabbitmq",
            UserName = "rabbitmq"
        };

        using var connection = factory.CreateConnection();
        using var channel = connection.CreateModel();

        channel.QueueDeclare(queue: "hello",
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        const string message = "Olá mundo, vou ser recebido pelo Receive";
        var body = Encoding.UTF8.GetBytes(message);

        channel.BasicPublish(exchange: string.Empty,
                    routingKey: "hello",
                    basicProperties: null,
                    body: body);

        Console.WriteLine($"[x] Sent {message}");

        Console.WriteLine("Press [Enter] to exit.");
        Console.ReadLine();
    }
}