using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using TPLibrary.Logger;

namespace Wardship.Infrastructure
{
    public class PresentationLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            //container.Register(Component.For<ITelemetryLogger>().ImplementedBy<TelemetryLogger>().LifestyleSingleton());
            //container.Register(Component.For<SQLRepository>());
            container.Register(Component.For<ISQLRepository>().ImplementedBy<SQLRepository>().LifestyleSingleton());
            container.Register(Component.For<ICloudWatchLogger>().ImplementedBy<CloudWatchLogger>().LifestyleSingleton());

        }
    }
}