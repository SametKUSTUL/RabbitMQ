using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ytuclrwa:dxJuUYpnPJKr9XtR5R6jqy79rGXKU89Y@moose.rmq.cloudamqp.com/ytuclrwa");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.BasicQos(0, 1, false);

var queueName = "direct-queue-Critical";

var consumer=new EventingBasicConsumer(channel);

channel.BasicConsume(queueName, false, consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message=Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Gelen Mesaj: {message}");

    File.AppendAllText("log-critical.txt", message+ "\n");

    channel.BasicAck(e.DeliveryTag, false);
};




Console.ReadLine();