using System;

namespace MassTransit.RabbitMQ.Contracts
{
    public class CreateAccount
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
