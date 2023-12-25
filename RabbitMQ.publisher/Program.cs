// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;

Console.WriteLine("Hello, World!");

var factory=new ConnectionFactory();
factory.Uri = new Uri("amqps://ytuclrwa:dxJuUYpnPJKr9XtR5R6jqy79rGXKU89Y@moose.rmq.cloudamqp.com/ytuclrwa");

using var connection = factory.CreateConnection();

var channel=connection.CreateModel();

channel.ExchangeDeclare("logs-fanout", ExchangeType.Fanout, true);

Enumerable.Range(1, 50).ToList().ForEach(x =>
{

    string message = $"log {x}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-fanout", "", null, messageBody);

    Console.WriteLine($"Mesaj kuyruğa gönderildi.: {message}");

});

Console.ReadLine();

