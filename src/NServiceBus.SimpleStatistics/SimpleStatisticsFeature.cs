using System.Threading.Tasks;

namespace NServiceBus
{
    using Features;
    using System.Configuration;

    public class SimpleStatisticsFeature : Feature
    {
        public SimpleStatisticsFeature()
        {
            EnableByDefault();
        }

        protected override void Setup(FeatureConfigurationContext context)
        {
            context.Container.ConfigureComponent<Options>(DependencyLifecycle.SingleInstance);
            context.Container.ConfigureComponent<Collector>(DependencyLifecycle.SingleInstance);
            context.Container.ConfigureComponent<Implementation>(x => x.Build<Collector>(), DependencyLifecycle.SingleInstance);
            context.Container.ConfigureComponent<StartupTask>(DependencyLifecycle.SingleInstance);

            context.RegisterStartupTask<StartupTask>(b => b.Build<StartupTask>());

            context.Container.ConfigureComponent<SimpleStatisticsBehavior>(DependencyLifecycle.SingleInstance);
            // Register the new step in the pipeline
            context.Pipeline.Register(nameof(SimpleStatisticsBehavior), typeof(SimpleStatisticsBehavior), "Logs and displays statistics.");
        }

        internal class StartupTask : FeatureStartupTask
        {
            readonly Collector Collector;

            public StartupTask(Collector collector)
            {
                Collector = collector;
            }

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