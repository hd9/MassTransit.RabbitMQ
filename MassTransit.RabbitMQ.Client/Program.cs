using System;
using MassTransit.RabbitMQ.Contracts;

namespace MassTransit.RabbitMQ.Client
{
    class Program
    {
        private const string options = "Options: [h]elp | [s]end name mail | [q]uit\r\n";
        private const string appName = "MassTransit.RabbitMQ.Client";

        static void Main(string[] args)
        {
            OnInit();

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri("rabbitmq://localhost"), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
            });

            bus.Start();
            
            var run = true;
            while(run){
                Console.Write("> ");
                var cmd = Console.ReadLine();
                var slices = (cmd ?? "").Split(' ');

                switch(slices[0]){
                    case "q":
                        run = false;
                        break;

                    case "s":
                        if (slices.Length == 3) bus.Publish(new CreateAccount{ Name = slices[1], Email = slices[2] });
                        else Log(options);
                        break;

                    default:
                        Log(options);
                        break;
                }
            }

            bus.Stop();

            OnShutDown();
        }
        
        private static void OnInit()
        {
            Log("-----------------------------------------------");
            Log($"Initting {appName}...");
            Log("-----------------------------------------------\r\n");
        }

        private static void OnShutDown()
        {
            Log("\r\n-----------------------------------------------");
            Log($"Shutting down {appName}...");
            Log("-----------------------------------------------");
        }

        private static void Log(string msg){
            Console.WriteLine(msg);
        }

    }
}
