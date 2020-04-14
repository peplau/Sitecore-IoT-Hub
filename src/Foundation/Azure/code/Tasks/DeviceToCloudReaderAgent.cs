using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IoTHub.Foundation.Azure.Repositories;
using Microsoft.Azure.EventHubs;
using Sitecore.Data;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IoTHub.Foundation.Azure.Tasks
{
    /// <summary>
    /// Represents a DeviceToCloudReader Agent
    /// </summary>
    public class DeviceToCloudReaderAgent
    {
        private const string IotHubSasKeyName = "service";
        private IIoTHubRepository _hubRepository;
        private IIoTHubRepository HubRepository
        {
            get
            {
                if (_hubRepository != null)
                    return _hubRepository;
                _hubRepository = DependencyResolver.Current.GetService<IIoTHubRepository>();
                return _hubRepository;
            }
        }

        private static readonly Dictionary<ID, EventHubClient> HubClients = new Dictionary<ID, EventHubClient>();

        public void Run()
        {
            var masterDb = Database.GetDatabase("master");

            var hubs = HubRepository.GetHubs(masterDb);
            var unhandledHubs = hubs.Where(p => !HubClients.ContainsKey(p.ID)).ToList();

            foreach (var hub in unhandledHubs)
            {
                // Create an EventHubClient instance to connect to the IoT Hub Event Hubs-compatible endpoint.
                var connectionString = new EventHubsConnectionStringBuilder(new Uri(hub.EventHubscompatibleEndpoint),
                    hub.EventHubscompatiblePath, IotHubSasKeyName, hub.ServicePrimaryKey);
                var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());
                HubClients.Add(hub.ID, eventHubClient);

                var ignored = CreatePartitionReceiver(eventHubClient);
            }
        }

        /// <summary>
        /// Create a PartitionReceiver for each partition on the hub.
        /// </summary>
        /// <param name="eventHub"></param>
        /// <returns></returns>
        private static async Task CreatePartitionReceiver(EventHubClient eventHub)
        {
            var runtimeInfo = await eventHub.GetRuntimeInformationAsync();
            var d2cPartitions = runtimeInfo.PartitionIds;

            var tasks = new List<Task>();
            foreach (var partition in d2cPartitions)
            {
                var cts = new CancellationTokenSource();
                tasks.Add(ReceiveMessagesFromDeviceAsync(eventHub, partition, cts.Token));
            }

            // Wait for all the PartitionReceivers to finish.
            Task.WaitAll(tasks.ToArray());
        }

        // Asynchronously create a PartitionReceiver for a partition and then start 
        // reading any messages sent from the simulated client.
        private static async Task ReceiveMessagesFromDeviceAsync(EventHubClient eventHub, string partition, CancellationToken ct)
        {
            // Create the receiver using the default consumer group.
            // For the purposes of this sample, read only messages sent since 
            // the time the receiver is created. Typically, you don't want to skip any messages.
            var eventHubReceiver =
                eventHub.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
            Console.WriteLine("Create receiver on partition: " + partition);
            while (true)
            {
                if (ct.IsCancellationRequested)
                    break;

                Sitecore.Diagnostics.Log.Info("Listening for messages on: " + partition, eventHub);

                // Check for EventData - this methods times out if there is nothing to retrieve.
                var events = await eventHubReceiver.ReceiveAsync(100);

                // If there is data in the batch, process it.
                if (events == null) 
                    continue;

                foreach (var eventData in events)
                {
                    if (eventData?.Body.Array == null)
                        continue;

                    var data = Encoding.UTF8.GetString(eventData.Body.Array);
                    Sitecore.Diagnostics.Log.Info($"Message received on partition {partition}: {data}", eventHub);

                    var sb = new StringBuilder();
                    sb.AppendLine("Application properties (set by device):");
                    foreach (var prop in eventData.Properties)
                        sb.AppendLine($"  {prop.Key}: {prop.Value}");
                    Sitecore.Diagnostics.Log.Info(sb.ToString(), eventHub);

                    sb = new StringBuilder();
                    sb.AppendLine("System properties (set by IoT Hub):");
                    foreach (var prop in eventData.SystemProperties)
                        sb.AppendLine($"  {prop.Key}: {prop.Value}");
                    Sitecore.Diagnostics.Log.Info(sb.ToString(), eventHub);
                }
            }
        }
    }
}