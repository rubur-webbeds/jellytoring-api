using jellytoring_api.Infrastructure.Inference;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jellytoring_api.Service.Inference
{
    public class InferenceEngine
    {
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly string replyQueueName;
        private readonly EventingBasicConsumer consumer;
        private readonly BlockingCollection<string> respQueue = new();
        private readonly IBasicProperties props;
        private readonly InferenceRepository inferenceRepository;

        public InferenceEngine(InferenceRepository inferenceRepository)
        {
            this.inferenceRepository = inferenceRepository ?? throw new ArgumentNullException(nameof(inferenceRepository));

            try
            {
                var factory = new ConnectionFactory() { HostName = "localhost" };

                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                replyQueueName = channel.QueueDeclare().QueueName;
                consumer = new EventingBasicConsumer(channel);

                props = channel.CreateBasicProperties();
                var correlationId = Guid.NewGuid().ToString();
                props.CorrelationId = correlationId;
                props.ReplyTo = replyQueueName;

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        respQueue.Add(response);
                    }
                };

                channel.BasicConsume(
                    consumer: consumer,
                    queue: replyQueueName,
                    autoAck: true);
            }
            catch (BrokerUnreachableException ex)
            {

            }
            catch (Exception ex)
            {

            }
        }

        public void Send(string message)
        {
            var messageBytes = Encoding.UTF8.GetBytes(message);
            Console.WriteLine($"Sending message: {message}");
            channel.BasicPublish(
                exchange: "",
                routingKey: "rpc_queue",
                basicProperties: props,
                body: messageBytes);

            Console.WriteLine("Waiting response...");
            var response = respQueue.Take();
            Console.WriteLine($"Response: {response}"); // testdir/3/28042022/results/output/xxx_o.jpg#xxx.jpg

            if (response == "ko")
                throw new Exception("Inference failed");

            var tokens = response.Split("#");
            var outputPath = tokens[0];
            var originalImageName = tokens[1];

            var result = inferenceRepository.MarkAsCompletedAsync(originalImageName, outputPath).GetAwaiter().GetResult();

            if (result == "error")
            {
                throw new Exception($"Failed while marking inference as completed. Inference: {originalImageName}");
            }
        }
    }
}
