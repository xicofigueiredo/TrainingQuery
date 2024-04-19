using System.Text;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Application.DTO;
using Application.Services;

namespace WebApi.Controllers;

public class RabbitMQConsumerController : IRabbitMQConsumerController
{
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;
    private string nameProject;
    private readonly ProjectService _projectService;
    List<string> _errorMessages = new List<string>();
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public RabbitMQConsumerController(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        nameProject = "project_create";
        var factory = new ConnectionFactory { HostName = "localhost" };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();

        _channel.ExchangeDeclare(exchange: nameProject, type: ExchangeType.Fanout);
        
        // _queueName = _channel.QueueDeclare().QueueName;
        // _channel.QueueBind(queue: _queueName,
        //     exchange: nameProject,
        //     routingKey: string.Empty);
        Console.WriteLine(" [*] Waiting for messages.");
    }
    
    public void ConfigQueue(string queueName)
    {
        _queueName = queueName;

        _channel.QueueDeclare(queue: _queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        _channel.QueueBind(queue: _queueName,
            exchange: nameProject,
            routingKey: string.Empty);
    }
    public void StartConsuming()
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += async (model, ea) =>
        {
            byte[] body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            ProjectDTO deserializedObject = ProjectGatewayDTO.ToDTO(message);
            Console.WriteLine($" [x] Received {deserializedObject}");
            Console.WriteLine($" [x] Start checking if exists.");
            
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var projectService = scope.ServiceProvider.GetRequiredService<ProjectService>();
                
                ProjectDTO projectResultDTO = projectService.AddFromAMQP(deserializedObject, _errorMessages).Result;
            }
        };
        _channel.BasicConsume(queue: _queueName,
            autoAck: true,
            consumer: consumer);
    }
    
    public void StopConsuming()
    {
        
    }
    
    
}