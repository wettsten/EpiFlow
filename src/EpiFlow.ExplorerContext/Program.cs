using System;
using System.Diagnostics;
using System.Linq;
using System.Configuration;
using Raven.Client.Document;
using NServiceBus;
using NServiceBus.Persistence;
using EpiFlow.Messages.Commands;
using NServiceBus.Logging;
using System.IO;

namespace EpiFlow.ExplorerContext
{
    public class Program
    {
        static ISendOnlyBus _bus;
        static ILog logger = LogManager.GetLogger<Program>();

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 2)
            {
                StartManualUI();
                StartBus();
                if (_bus != null)
                {
                    SendMessage(args);
                    _bus.Dispose();
                }
            }
        }

        static void StartManualUI()
        {
            var prcs = ConfigurationManager.AppSettings["manualUIProcess"];
            var process = Process.GetProcesses().FirstOrDefault(i => i.ProcessName.Equals(prcs) || i.ProcessName.Equals(prcs + ".vshost"));
            if (process == null)
            {
                var processStart = new ProcessStartInfo(Path.Combine(ConfigurationManager.AppSettings["manualUIPath"], prcs) + ".exe")
                {
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                Process.Start(processStart);
            }
        }

        static void StartBus()
        {
            try
            {
                var myDocumentStore = new DocumentStore { ConnectionStringName = "EpiFlowDB" };

                var busConfiguration = new BusConfiguration();
                busConfiguration.EndpointName("EpiFlow.Messages");
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
                _bus = Bus.CreateSendOnly(busConfiguration);
            }
            catch (Exception exception)
            {
                OnCriticalError("Failed to start the bus.", exception);
            }
        }

        static void SendMessage(string[] args)
        {
            _bus.Send<IManuallyNameEpisodeCommand>(i =>
            {
                i.Filename = args[1];
                i.Action = args[0];
            });
        }

        static void OnCriticalError(string errorMessage, Exception exception)
        {
            //TODO: Decide if shutting down the process is the best response to a critical error
            //http://docs.particular.net/nservicebus/hosting/critical-errors
            var fatalMessage = string.Format("The following critical error was encountered:\n{0}\nProcess is shutting down.", errorMessage);
            logger.Fatal(fatalMessage, exception);
            Environment.FailFast(fatalMessage, exception);
        }
    }
}
