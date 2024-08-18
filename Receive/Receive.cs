using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


public class Receive
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

        Console.WriteLine("[x] Aguardando mensagem");

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            Console.WriteLine($"[x] Received {message}");
        };

        channel.BasicConsume(queue: "hello",
        autoAck: true,
        consumer: consumer);

        Console.WriteLine("Press [Enter] to exit.");
        Console.ReadLine();
    }
}