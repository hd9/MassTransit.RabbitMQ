using System;
using MassTransit;
using MassTransit.RabbitMQ;
using MassTransit.RabbitMQ.Contracts;

namespace MassTransit.RabbitMQ.Service
{
    class Program
    {
        private const string appName = "MassTransit.RabbitMQ.Service";
        private const string endpoint = "rabbitmq://localhost";

        static void Main(string[] args)
        {
            OnInit();

            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                var host = sbc.Host(new Uri(endpoint), h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });

                sbc.ReceiveEndpoint(host, "test_queue", ep =>
                {
                    ep.Handler<CreateAccount>(context =>
                    {
                        return Console.Out.WriteLineAsync($"[CREATEACCOUNT {DateTime.Now.ToString("s")}] Name: {context.Message.Name}, Email: {context.Message.Email}");
                    });
                });
            });

            bus.Start();
            OnStart();
            bus.Stop();
           
            OnShutDown();
        }

        private static void OnInit()
        {
            Log("-----------------------------------------------");
            Log($"Initting {appName}...");
            Log("-----------------------------------------------\r\n");
        }

        private static void OnStart()
        {
            Log($"Successfully connected @ {endpoint}...");
            Log("Entering listening mode...");
            Log("Press any key to shut down service...\r\n");
            Console.ReadLine();
        }

        private static void OnShutDown()
        {
            Log("-----------------------------------------------");
            Log($"Shutting down {appName}...");
            Log("-----------------------------------------------");
        }

        private static void Log(string msg){
            Console.WriteLine(msg);
        }

    }
}
