using System;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;

class SimpleStatisticsBehavior : IBehavior<IncomingContext>
{
    readonly Implementation provider;

    public SimpleStatisticsBehavior(Implementation provider)
    {
        this.provider = provider;
    }

    public void Invoke(IncomingContext context, Action next)
    {
        var start = provider.Timestamp();
        try
        {
            provider.ConcurrencyInc();
            next();
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

    public static readonly string Name = "StatisticsStep";

    public class Step : RegisterStep
    {
        public Step() : base(Name, typeof(SimpleStatisticsBehavior), "Logs and displays statistics.")
        {
            InsertBefore(WellKnownStep.CreateChildContainer);
        }
    }
}
