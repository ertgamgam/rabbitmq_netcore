using Microsoft.Extensions.Hosting;
using ModelLibrary;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASPConsumer
{
    public class ConsumerHostedService : IHostedService,IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.StringConsume();
            return Task.CompletedTask;            
        }

        public void Consume()
        {
            var factory = new ConnectionFactory() { HostName = "some-rabbit" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.QueueDeclare(queue: "hello",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = FromByteArray<Model>(body);
                Debug.WriteLine("ASPConsumer: Date = " + message.Time.ToString() + "Message = " + message.Message);
            };

            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);
        }

        public void StringConsume()
        {
            var factory = new ConnectionFactory() { HostName = "some-rabbit" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            
                channel.QueueDeclare(queue: "hello",
                                     durable: true,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var message = Encoding.UTF8.GetString(body);
                    Debug.WriteLine(" [x] ASPConsumer = "+ message);
                };
                channel.BasicConsume(queue: "hello",
                                     autoAck: true,
                                     consumer: consumer);                        
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public static T FromByteArray<T>(byte[] data)
        {
            if (data == null)
                return default(T);
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream(data))
            {
                object obj = bf.Deserialize(ms);
                return (T)obj;
            }
        }
    }
}
