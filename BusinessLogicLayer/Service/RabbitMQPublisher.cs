using BusinessLogicLayer.Interface;
using Microsoft.Extensions.Configuration;
using ModelLayer.DTOs.Email;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer.Service
{
    public class RabbitMQPublisher : IMessagePublisher
    {
        private readonly IConfiguration _config;

        public RabbitMQPublisher(IConfiguration config)
        {
            _config = config;
        }

        public void PublishEmail(EmailMessageDTO message)
        {
            var factory = new ConnectionFactory
            {
                HostName = _config["RabbitMQ:HostName"],
                UserName = _config["RabbitMQ:UserName"],
                Password = _config["RabbitMQ:Password"],
                Port = int.Parse(_config["RabbitMQ:Port"] ?? "5672")
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: _config["RabbitMQ:QueueName"],
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var body = Encoding.UTF8.GetBytes(
                JsonSerializer.Serialize(message)
            );

            channel.BasicPublish(
                exchange: "",
                routingKey: _config["RabbitMQ:QueueName"],
                basicProperties: null,
                body: body
            );
        }
    }
}
