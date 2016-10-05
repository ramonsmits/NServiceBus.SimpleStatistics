using System;
using NServiceBus;
using NServiceBus.Config;
using NServiceBus.Config.ConfigurationSource;

class Program
{
    public static void Main()
    {
        Console.Title = "SimpleStatisticsDemo";

        var cfg = new BusConfiguration();
        cfg.EndpointName("SimpleStatisticsDemo");
        cfg.UsePersistence<InMemoryPersistence>();
        cfg.EnableInstallers();

        using (var bus = Bus.Create(cfg).Start())
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                bus.SendLocal(new Message());
            }
        }
    }
}

class Message : IMessage
{
}


class Handler : IHandleMessages<Message>
{
    public IBus Bus { get; set; }

    public void Handle(Message message)
    {
        Console.WriteLine("Received {0}", Bus.CurrentMessageContext.Id);
    }
}


class ProvideConfiguration :
    IProvideConfiguration<MessageForwardingInCaseOfFaultConfig>
{
    public MessageForwardingInCaseOfFaultConfig GetConfiguration()
    {
        return new MessageForwardingInCaseOfFaultConfig
        {
            ErrorQueue = "error"
        };
    }
}