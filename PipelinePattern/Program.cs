using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PipelinePattern.Entity;

namespace PipelinePattern
{
    class Program
    {
        static void Main(string[] args)
        {
            const int queueBoundedCapacity = 4;
            #region
            var fileNames = new List<string>(){"car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg", "car.jpg", "car2.jpg", "car3.jpg" };
            #endregion
            string sourceDir = "C:\\Users\\Kalpens\\source\\repos\\PipelinePattern\\PipelinePattern\\";
            var LoadBalancingDegreeOfConcurrency = 2;
            CancellationTokenSource token = new CancellationTokenSource();

            Console.WriteLine("Press any key to start pipeline");
            Console.ReadLine();

            var mainTask = Task.Factory.StartNew(() => LoadBalancedPipeline.RunPipeline(fileNames, sourceDir, queueBoundedCapacity, token,
                LoadBalancingDegreeOfConcurrency),
                token.Token,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        
            Console.WriteLine("TheEnd???!?!");
            string s = Console.ReadLine();
            while (s != "s")
            {
                Console.WriteLine("TheEnd???!?!");
                s = Console.ReadLine();
            }
            {
                    token.Cancel();

                    Task.Factory.StartNew(() =>
                    {
                        mainTask.Wait();
                    });
            }
            Console.ReadLine();
        }

    }

}
