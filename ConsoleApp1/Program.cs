using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var queue = new QueueSemaphoreSlim<string>(0))
            {
                queue.QueueCallBack += Queue_QueueCallBack;
                queue.StartCallBack += Queue_StartCallBack;
                queue.StopCallBack += Queue_StopCallBack;
                queue.ErrorCallBack += Queue_ErrorCallBack;
                queue.RunQueue();
                Console.WriteLine("------------------任务队列启动------------------");
                bool read;
                do
                {
                    Console.WriteLine("继续加入队列（Y退出）");
                    var readMessage = Console.ReadLine();
                    read = readMessage.ToLower().Equals("y");
                    queue.AddTQuequ(readMessage);
                } while (!read);
                #region 示例
                //var task1 = Task<bool>.Factory.StartNew(() =>
                //{
                //    for (int i = 0; i < 10; i++)
                //    {
                //        queue.AddTQuequ(i + "第一个队列");
                //    }
                //    return true;
                //});
                //var task2 = Task<bool>.Factory.StartNew(() =>
                //{
                //    for (int i = 0; i < 10; i++)
                //    {
                //        queue.AddTQuequ(i + "第二个队列");
                //    }
                //    return true;
                //});
                //var task3 = Task<bool>.Factory.StartNew(() =>
                //{
                //    for (int i = 0; i < 10; i++)
                //    {
                //        queue.AddTQuequ(i + "第三个队列");
                //    }
                //    return true;
                //});
                //var result = Task.WhenAll(task1, task2, task3).Result;
                #endregion
                queue.StopQueue();
            }
            Console.WriteLine("------------------任务队列完成------------------");
            //Console.WriteLine("最后：{0}", queue.QueueCount);
            //Console.WriteLine("最后：{0}", queue.QueueCount);
            Console.ReadLine();
        }

        private static void Queue_ErrorCallBack(object sender, EventArgs e)
        {
            Console.WriteLine(sender.ToString());
        }

        private static void Queue_StopCallBack(object sender, EventArgs e)
        {
            Console.WriteLine("停止队列");
        }

        private static void Queue_StartCallBack(object sender, EventArgs e)
        {
            Console.WriteLine("开始队列");
        }

        private static void Queue_QueueCallBack(object sender, EventArgs e)
        {
            Console.WriteLine("取出信息" + sender.ToString());
            Thread.Sleep(1000);
        }
    }
}
