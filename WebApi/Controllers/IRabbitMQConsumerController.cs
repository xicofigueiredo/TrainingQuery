using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

public interface IRabbitMQConsumerController
{
    void StartConsuming();
    void ConfigQueue(string queueName);
    void StopConsuming();
}