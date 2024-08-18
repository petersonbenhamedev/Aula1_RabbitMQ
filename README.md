## Introdução

Este projeto demonstra como usar o RabbitMQ, uma plataforma de mensagens, para enviar e receber mensagens entre sistemas distribuídos. Usando a biblioteca RabbitMQ.Client, o exemplo a seguir implementa duas classes principais: `Send` e `Receive`. A classe `Send` é responsável por enviar mensagens para uma fila, enquanto a classe `Receive` recebe essas mensagens da fila.

### Dependências

Antes de começar, certifique-se de que você tenha o RabbitMQ instalado e em execução no seu ambiente. Além disso, adicione a biblioteca `RabbitMQ.Client` ao seu projeto .NET.

### Classe `Send`

A classe `Send` é responsável por enviar uma mensagem para a fila do RabbitMQ. Aqui está uma explicação do que o código faz:

1. **Conexão com o RabbitMQ:** Cria uma instância de `ConnectionFactory` e configura as credenciais de conexão, incluindo o hostname, porta, username, e password.

2. **Criação da Fila:** Usa o método `QueueDeclare` para garantir que a fila chamada "hello" exista. Se a fila não existir, ela será criada com as propriedades especificadas.

3. **Envio de Mensagem:** Converte a string da mensagem em um array de bytes e publica essa mensagem na fila "hello" usando o método `BasicPublish`.

4. **Finalização:** Exibe no console uma mensagem confirmando o envio e aguarda a entrada do usuário para finalizar.

```csharp
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
```

### Classe `Receive`

A classe `Receive` é responsável por consumir mensagens da fila "hello". A seguir está uma explicação do que o código faz:

1. **Conexão com o RabbitMQ:** Similar à classe `Send`, cria uma instância de `ConnectionFactory` e estabelece uma conexão com o RabbitMQ.

2. **Criação da Fila:** Verifica se a fila "hello" existe, criando-a se necessário, com as mesmas propriedades especificadas na classe `Send`.

3. **Consumo de Mensagem:** Cria um consumidor que escuta a fila "hello". Quando uma mensagem é recebida, ela é decodificada de bytes para string e exibida no console.

4. **Execução contínua:** O programa continua em execução aguardando novas mensagens até que o usuário pressione Enter para sair.

```csharp
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
```

### Como Executar

1. **Enviar Mensagem:**
   - Compile e execute a classe `Send` para enviar uma mensagem para a fila.

2. **Receber Mensagem:**
   - Compile e execute a classe `Receive` para consumir a mensagem da fila e exibi-la no console.

Esses exemplos simples demonstram como é fácil configurar uma comunicação básica entre diferentes componentes de um sistema distribuído usando RabbitMQ e .NET.