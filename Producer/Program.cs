using ModelLibrary;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 1. example
            //var factory = new ConnectionFactory() { HostName = "some-rabbit" };
            //using (var connection = factory.CreateConnection())
            //using (var channel = connection.CreateModel())
            //{
            //    channel.QueueDeclare(queue: "hello",
            //                         durable: false,
            //                         exclusive: false,
            //                         autoDelete: false,
            //                         arguments: null);

            //    string message = "Hello World!";
            //    var body = Encoding.UTF8.GetBytes(message);

            //    channel.BasicPublish(exchange: "",
            //                         routingKey: "hello",
            //                         basicProperties: null,
            //                         body: body);
            //    Console.WriteLine(" [x] Sent {0}", message);
            //}
            #endregion

            #region 2. example
            //var factory = new ConnectionFactory() { HostName = "some-rabbit" };
            //var connection = factory.CreateConnection();
            //var channel = connection.CreateModel();

            ////for persistent message
            //var props = channel.CreateBasicProperties();
            //props.ContentType = "text/plain";
            //props.DeliveryMode = 2;

            //channel.QueueDeclare(queue: "hello",
            //                     durable: true,
            //                     exclusive: false,
            //                     autoDelete: false,
            //                     arguments: null);

            //bool loop = true;
            //while (loop)
            //{
            //    channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: props, body: Encoding.UTF8.GetBytes(DateTime.Now.ToString()));
            //    Thread.Sleep(3000);
            //}

            #endregion

            #region 3. example


            var factory = new ConnectionFactory() { HostName = "some-rabbit" };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            ////for persistent message
            var props = channel.CreateBasicProperties();
            props.ContentType = "text/plain";
            props.DeliveryMode = 2;

            channel.QueueDeclare(queue: "hello",
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            bool loop = true;
            while (loop)
            {
                Model model = new Model()
                {
                    Message = randomMessage(),
                    Time = DateTime.Now
                };

                var message = JsonConvert.SerializeObject(model);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish(exchange: "", routingKey: "hello", basicProperties: props, body: body);
                Thread.Sleep(3000);
            }


            #endregion

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static byte[] ToByteArray<T>(T obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }


        static string randomMessage()
        {
            string[] messages = { "Paradise City", "November Rain", "Do i wanna know" };
            Random rand = new Random();
            return messages[rand.Next(2)];
        }
    }    
}
