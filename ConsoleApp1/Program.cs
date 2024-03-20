using RabbitMQ.Client;
using System.Text;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Hello, World! productor");

            var factory = new ConnectionFactory()       // 创建连接工厂对象
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };
            var connection = factory.CreateConnection();    // 创建连接对象
            var channel = connection.CreateModel();         // 创建连接会话对象

            string queueName = "queue1";

            // 声明一个队列
            channel.QueueDeclare(
                queue: queueName,   // 队列名称
                durable: false,     // 是否持久化，true持久化，队列会保存磁盘，服务器重启时可以保证不丢失相关信息
                exclusive: false,   // 是否排他，如果一个队列声明为排他队列，该队列仅对时候次声明它的连接可见，并在连接断开时自动删除
                autoDelete: false,  // 是否自动删除，自动删除的前提是：至少有一个消费者连接到这个队列，之后所有与这个队列连接的消费者都断开时，才会自动删除
                arguments: null     // 设置队列的其他参数
            );

            string str = string.Empty;

            do
            {
                Console.WriteLine("发送内容：");
                str = Console.ReadLine()!;

                // 消息内容
                byte[] body = Encoding.UTF8.GetBytes(str);

                // 发送消息
                channel.BasicPublish("", queueName, null, body);

                // Console.WriteLine("成功发送消息：" + str);
            } while (str.Trim().ToLower() != "exit");

            channel.Close();
            connection.Close();

        }
    }
}
