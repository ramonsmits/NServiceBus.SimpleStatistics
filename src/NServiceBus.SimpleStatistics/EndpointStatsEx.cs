using System;
using NServiceBus;
using NServiceBus.Configuration.AdvancedExtensibility;

public static class EndpointStatsEx
{
    public static void ConfigureStatistics(this EndpointConfiguration configuration, Action<Options> configure = null)
    {
        var o = new Options();
        configure?.Invoke(o);
        configuration.GetSettings().Set(o);
    }
}