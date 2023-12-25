// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System.Text;


public enum LogNames
{
    Critical=1,Error=2,Warning=3,Information=4,
}
class Program
{
    static void Main()
    {
        Console.WriteLine("Hello, World!");

        var factory = new ConnectionFactory();
        factory.Uri = new Uri("amqps://ytuclrwa:dxJuUYpnPJKr9XtR5R6jqy79rGXKU89Y@moose.rmq.cloudamqp.com/ytuclrwa");

        using var connection = factory.CreateConnection();

        var channel = connection.CreateModel();

        channel.ExchangeDeclare("logs-direct", ExchangeType.Direct, true);


        Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
        {
            var routeKey = $"route-{x}";
            var queueName = $"direct-queue-{x}";
            channel.QueueDeclare(queueName,true,false,false);
            channel.QueueBind(queueName, "logs-direct", routeKey);
        });


        Enumerable.Range(1, 50).ToList().ForEach(x =>
        {
            LogNames log=(LogNames)new Random().Next(1,5);

            string message = $"log-type: {log}";

            var messageBody = Encoding.UTF8.GetBytes(message);

            var routeKey = $"route-{log}";

            channel.BasicPublish("logs-direct", routeKey,null,messageBody);

            Console.WriteLine($"Log gönderildi.: {message}");

        });

        Console.ReadLine();
    }

}



