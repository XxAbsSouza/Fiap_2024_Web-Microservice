using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Reflection;
using System.Text;

var factory = new ConnectionFactory()
{
    HostName = "localhost"
};
using var Connection = factory.CreateConnection();
using var channel = Connection.CreateModel();
const string fila = "fila_teste";

channel.QueueDeclare(queue: fila,
    durable: false,
    exclusive: false,
    autoDelete: false,
    arguments: null);

var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, args) =>
{
   var body = args.Body.ToArray();
   var mensagem = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Mensagem recebida {mensagem}...");
};

channel.BasicConsume(queue: fila,
    autoAck: true, //esse atributo determina se vai tirar da fila ou n
    consumer: consumer);

Console.WriteLine("Aguardando mensagem");
Console.ReadLine();