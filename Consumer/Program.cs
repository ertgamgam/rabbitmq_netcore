using ModelLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Consumer
{
    class Program
    {
        static void Main(string[] args)
        {

            #region example 1-2
            //var factory = new ConnectionFactory() { HostName = "some-rabbit" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "hello",
            //                         durable: true,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //    var consumer = new EventingBasicConsumer(channel);
            //    consumer.Received += (model, ea) =>
            //    {
            //        var body = ea.Body;
            //        var message = Encoding.UTF8.GetString(body);
            //        Console.WriteLine(" [x] ConsoleConsumer = {0}", message);
            //    };
            //    channel.BasicConsume(queue: "hello",
            //                         autoAck: true,
            //                         consumer: consumer);

            //    Console.WriteLine(" Press [enter] to exit.");
            //    Console.ReadLine();
            //}
            #endregion

            #region example3

            Example3();
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();

            #endregion

        }

        public static void Example3()
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
                var messagestring = Encoding.UTF8.GetString(body);
                var message = JsonConvert.DeserializeObject<Model>(messagestring);
                Console.WriteLine("ConsoleConsumer: Date = " + message.Time.ToString() + "Message = " + message.Message);
            };

            channel.BasicConsume(queue: "hello",
                                 autoAck: true,
                                 consumer: consumer);


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
