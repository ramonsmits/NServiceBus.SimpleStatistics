using System;
using System.Threading.Tasks;
using NServiceBus;

class Program
{
    public static async Task Main()
    {
        Console.Title = "SimpleStatisticsDemo";

        var cfg = new EndpointConfiguration("SimpleStatisticsDemo");
        cfg.EnableInstallers();
        cfg.SendFailedMessagesTo("error");
        cfg.UseTransport<LearningTransport>();
        var instance = await Endpoint.Start(cfg);
        try
        {
            while (Console.ReadKey().Key != ConsoleKey.Escape)
            {
                await instance.SendLocal(new Message());
            }
        }
        finally
        {
            await instance.Stop();
        }
    }
}

class Message : IMessage
{
}

class Handler : IHandleMessages<Message>
{
    public async Task Handle(Message message, IMessageHandlerContext context)
    {
        await Console.Out.WriteLineAsync($"Received {context.MessageId}");
    }
}