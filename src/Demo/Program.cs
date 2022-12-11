using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus;

class Program
{
    public static async Task Main()
    {
        Console.Title = "SimpleStatisticsDemo";

        var host = Host.CreateDefaultBuilder()
            .UseNServiceBus(hostBuilderContext =>
            {
                var cfg = new EndpointConfiguration("SimpleStatisticsDemo");
                cfg.EnableInstallers();
                cfg.SendFailedMessagesTo("error");
                cfg.UseTransport<LearningTransport>();
                cfg.ConfigureStatistics(o =>
                {
                    o.UpdateInMilliSeconds = 1000;
                    o.OutputLog = false;
                    o.OutputTitle = true;
                });
                return cfg;
            })
            .Build();

        await host.RunAsync();
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
