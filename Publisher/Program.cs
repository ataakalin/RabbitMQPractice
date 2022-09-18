using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Publisher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("---Publisher---");
            Console.WriteLine("Please enter your message");


            Publisher publisher = new Publisher();
            string sentMessage = string.Empty;
            while (!sentMessage.Contains("q"))
            {
                sentMessage = Console.ReadLine();
                publisher.SendToRabbitMQ(sentMessage);
            }

        }
    }


    class Publisher
    {
        public void SendToRabbitMQ(string paramMessage)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                ///<summary>
                ///Queue: “hello” bu parametre ile göndermek istediğimiz Queue adını belirtilir.
                ///Durable: Bu parametre ile yeniden başlatma sonrasında verilerin kuyrukta kalıp kalmayacağı belirtilir.
                /// Exclusive: Kuyruğa birden fazla bağlantı açılıp açılamayacağını belirtir.
                /// Auto Delete: Son tüketici(Consumer) silindiğinde, kuyruğun silinip silinmeyeceğini belirtir.
                ///</summary>
                channel.QueueDeclare(queue: "hello",
                    durable: true,      
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);


                var body = Encoding.UTF8.GetBytes(paramMessage);
                channel.BasicPublish(exchange: "",
                    routingKey: "hello",
                    body: body);
                Console.WriteLine("Message [{0}] sent", paramMessage);
            }
        }
    }
}
