using NServiceBus;
using NServiceBus.Logging;
using NServiceBus.Persistence;
using Raven.Client.Document;
using System;
using System.Diagnostics;
using System.Windows;
using System.Linq;
using StructureMap;

namespace EpiFlow.ManualUI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IStartableBus _bus;
        private readonly IContainer _container;
        private readonly ILog _logger = LogManager.GetLogger<App>();

        public App()
        {
            _container = new Container(x => x.AddRegistry<DependencyRegistry>());
            //StartBus();
            var main = new MainWindow(_container);
            main.ShowDialog();
        }
        private void StartBus()
        {
            try
            {
                var myDocumentStore = new DocumentStore { ConnectionStringName = "EpiFlowDB" };

                var busConfiguration = new BusConfiguration();
                busConfiguration.EndpointName("EpiFlow.Messages");
                busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(_container));
                busConfiguration.UseSerialization<JsonSerializer>();
                busConfiguration.UsePersistence<RavenDBPersistence>()
                    .UseDocumentStoreForSubscriptions(myDocumentStore)
                    .UseDocumentStoreForSagas(myDocumentStore)
                    .UseDocumentStoreForTimeouts(myDocumentStore);
                busConfiguration.UseTransport<RabbitMQTransport>();
                busConfiguration.DefineCriticalErrorAction(OnCriticalError);
                busConfiguration.Transactions().DisableDistributedTransactions();

                if (Environment.UserInteractive && Debugger.IsAttached)
                {
                    busConfiguration.EnableInstallers();
                }
                _bus = Bus.Create(busConfiguration);
                _bus.Start();
            }
            catch (Exception exception)
            {
                OnCriticalError("Failed to start the bus.", exception);
            }
        }

        private void OnCriticalError(string errorMessage, Exception exception)
        {
            //TODO: Decide if shutting down the process is the best response to a critical error
            //http://docs.particular.net/nservicebus/hosting/critical-errors
            var fatalMessage = string.Format("The following critical error was encountered:\n{0}\nProcess is shutting down.", errorMessage);
            _logger.Fatal(fatalMessage, exception);
            //Environment.FailFast(fatalMessage, exception);
        }
    }
    
}
