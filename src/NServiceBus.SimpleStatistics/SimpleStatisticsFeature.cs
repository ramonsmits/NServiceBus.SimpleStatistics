
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
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

            var task = new StartupTask
            {
                Collector = new Collector()
            };
            context.Container.RegisterSingleton<Implementation>(task.Collector);
            context.RegisterStartupTask(task);

            context.Container.ConfigureComponent<SimpleStatisticsBehavior>(DependencyLifecycle.SingleInstance);
            // Register the new step in the pipeline
            context.Pipeline.Register(nameof(SimpleStatisticsBehavior), typeof(SimpleStatisticsBehavior), "Logs and displays statistics.");
        }

        internal class StartupTask : FeatureStartupTask
        {
            public Collector Collector { get; set; }

            protected override Task OnStart(IMessageSession session)
            {
                Collector.Start();
                return Task.FromResult(0);
            }

            protected override Task OnStop(IMessageSession session)
            {
                Collector.Stop();
                return Task.FromResult(0);
            }
        }
    }
}