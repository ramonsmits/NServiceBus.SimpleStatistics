using System;
using System.Threading.Tasks;
using NServiceBus.Pipeline;

class SimpleStatisticsBehavior : Behavior<ITransportReceiveContext>
{
    readonly Implementation provider;

    public SimpleStatisticsBehavior(Implementation provider)
    {
        this.provider = provider;
    }

    public override async Task Invoke(ITransportReceiveContext context, Func<Task> next)
    {
        var start = provider.Timestamp();
        try
        {
            provider.ConcurrencyInc();
            await next().ConfigureAwait(false);
            provider.SuccessInc();
        }
        catch
        {
            provider.ErrorInc();
            throw;
        }
        finally
        {
            provider.DurationInc(start, provider.Timestamp());
            provider.ConcurrencyDec();
            provider.Inc();
        }
    }
}