using System;
using System.Diagnostics;
using System.ServiceProcess;
using NServiceBus;
using NServiceBus.Logging;
using Raven.Client.Document;
using NServiceBus.Persistence;
using StructureMap;

namespace EpiFlow.FileWatcher
{
    public class ProgramService : ServiceBase
    {
        IBus bus;
        Container container;
        static ILog logger = LogManager.GetLogger<ProgramService>();

        static void Main()
        {
            using (var service = new ProgramService())
            {
                // so we can run interactive from Visual Studio or as a windows service
                if (Environment.UserInteractive)
                {
                    Console.CancelKeyPress += (sender, e) =>
                    {
                        service.OnStop();
                    };
                    service.OnStart(null);
                    Console.WriteLine("\r\nPress enter key to stop program\r\n");
                    Console.Read();
                    service.OnStop();
                    return;
                }
                Run(service);
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                container = new Container(x => x.AddRegistry<DependencyRegistry>());
                var myDocumentStore = new DocumentStore { ConnectionStringName = "EpiFlowDB" };

                var busConfiguration = new BusConfiguration();
                busConfiguration.EndpointName("EpiFlow.Messages");
                busConfiguration.UseContainer<StructureMapBuilder>(c => c.ExistingContainer(container));
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
                var startableBus = Bus.Create(busConfiguration);
                bus = startableBus.Start();
            }
            catch (Exception exception)
            {
                OnCriticalError("Failed to start the bus.", exception);
            }
        }

        void OnCriticalError(string errorMessage, Exception exception)
        {
            //TODO: Decide if shutting down the process is the best response to a critical error
            //http://docs.particular.net/nservicebus/hosting/critical-errors
            var fatalMessage = string.Format("The following critical error was encountered:\n{0}\nProcess is shutting down.", errorMessage);
            logger.Fatal(fatalMessage, exception);
            Environment.FailFast(fatalMessage, exception);
        }

        protected override void OnStop()
        {
            bus?.Dispose();
            container?.Dispose();
        }

        private void InitializeComponent()
        {
            // 
            // ProgramService
            // 
            this.ServiceName = "EpiFlow.MessageProcessor";
        }
    }
}