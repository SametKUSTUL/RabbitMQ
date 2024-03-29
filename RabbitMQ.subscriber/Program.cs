﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!");

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://ytuclrwa:dxJuUYpnPJKr9XtR5R6jqy79rGXKU89Y@moose.rmq.cloudamqp.com/ytuclrwa");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

// channel.QueueDeclare("hello-queue", true, false, false);

channel.BasicQos(0, 1, false);

var consumer=new EventingBasicConsumer(channel);

channel.BasicConsume("hello-queue", false, consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message=Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Gelen Mesaj: {message}");
    channel.BasicAck(e.DeliveryTag, false);
};




Console.ReadLine();