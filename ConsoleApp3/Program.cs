using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace ConsoleApp3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World! customer");

            var factory = new ConnectionFactory()       // 创建连接工厂对象
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            IConnection connection = factory.CreateConnection();    // 创建连接对象
            IModel channel = connection.CreateModel();         // 创建连接会话对象

            string queueName = "queue1";
            //声明一个队列
            channel.QueueDeclare(
                queue: queueName,//消息队列名称
                durable: false,//是否持久化,true持久化,队列会保存磁盘,服务器重启时可以保证不丢失相关信息。
                exclusive: false,//是否排他,true排他的,如果一个队列声明为排他队列,该队列仅对首次声明它的连接可见,并在连接断开时自动删除.
                autoDelete: false,//是否自动删除。true是自动删除。自动删除的前提是：致少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开时,才会自动删除.
                arguments: null ////设置队列的一些其它参数
            );
            //RabbitMQ每次只能向消费者发送一条消息，消费者未确认之前，不再向他再发送消息
            channel.BasicQos(0, 1, false);
            // 创建消费者对象
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => {
                Thread.Sleep(5000);
                byte[] message = ea.Body.ToArray();
                Console.WriteLine("接收到的消息为：" + Encoding.UTF8.GetString(message));
                
                channel.BasicAck(ea.DeliveryTag,true);//手动确认时，返回确认消息
            };

            // 消费者开启监听
            channel.BasicConsume(queueName, 
                false, //true:自动确认，false：手动确认
                consumer
                );

            Console.ReadKey();
            channel.Dispose();
            connection.Close();
        }
    }
}
