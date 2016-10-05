using System.Configuration;
using System.Linq;

namespace NServiceBus
{
    using Features;
    using Logging;

    public class SimpleStatisticsFeature : Feature
    {
        static readonly string SettingKey = "NServiceBus/SimpleStatistics";

        internal static readonly string OutputConsoleTitleKey = SettingKey + "/OutputConsoleTitle";
        internal static readonly string OutputLogKey = SettingKey + "/OutputLog";
        internal static readonly string IntervalMilliSecondsKey = SettingKey + "/IntervalMilliSeconds";


        public SimpleStatisticsFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            var settings = ConfigurationManager.AppSettings;
            bool enable;
            if (settings.AllKeys.Contains(SettingKey) && bool.TryParse(settings[SettingKey], out enable) && !enable) return;

            context.Container.ConfigureComponent<Collector>(DependencyLifecycle.SingleInstance);
            RegisterStartupTask<StartupTask>();

            context.Container.ConfigureComponent<SimpleStatisticsBehavior>(DependencyLifecycle.SingleInstance);
            // Register the new step in the pipeline
            context.Pipeline.Register<SimpleStatisticsBehavior.Step>();
        }

        internal class StartupTask : FeatureStartupTask
        {
            public Collector Collector { get; set; }

            protected override void OnStart()
            {
                Collector.Start();
            }

            protected override void OnStop()
            {
                Collector.Stop();
            }
        }
    }
}