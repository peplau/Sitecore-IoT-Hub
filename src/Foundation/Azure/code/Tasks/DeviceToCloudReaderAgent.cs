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
using IoTHub.Foundation.Azure.Cache;

namespace IoTHub.Foundation.Azure.Tasks
{
    /// <summary>
    /// Represents a DeviceToCloudReader Agent
    /// </summary>
    public class DeviceToCloudReaderAgent
    {
        private const string IotHubSasKeyName = "service";
        private const string MethodProperyName = "method";
        private const string PayloadProperyName = "payload";

        private static IMethodCacheManager _methodCacheManager;
        private static IMethodCacheManager MethodCacheManager
        {
            get
            {
                if (_methodCacheManager != null)
                    return _methodCacheManager;
                _methodCacheManager = DependencyResolver.Current.GetService<IMethodCacheManager>();
                return _methodCacheManager;
            }
        }

        private static IIoTHubRepository _hubRepository;
        private static IIoTHubRepository HubRepository
        {
            get
            {
                if (_hubRepository != null)
                    return _hubRepository;
                _hubRepository = DependencyResolver.Current.GetService<IIoTHubRepository>();
                return _hubRepository;
            }
        }

        private static readonly Dictionary<ID, List<CancellationTokenSource>> HubTokens = new Dictionary<ID, List<CancellationTokenSource>>();
        private static Database _masterDb;

        public static async void Run()
        {
            _masterDb = Database.GetDatabase("master");
            var hubs = HubRepository.GetHubs(_masterDb);
            var hubIds = hubs.Select(p => p.ID);
            var hubsToAdd = hubs.Where(p => !HubTokens.ContainsKey(p.ID)).ToList();
            var hubIdsToRemove = HubTokens.Select(p => p.Key).Where(p => !hubIds.Contains(p)).ToList();

            // Remove loops that are not necessary anymore
            foreach (var id in hubIdsToRemove)
            {
                if (!HubTokens.ContainsKey(id))
                    continue;
                foreach (var token in HubTokens[id])
                    token.Cancel();
            }

            // Create hub loops
            foreach (var hub in hubsToAdd)
            {
                // Create an EventHubClient instance to connect to the IoT Hub Event Hubs-compatible endpoint.
                var connectionString = new EventHubsConnectionStringBuilder(new Uri(hub.EventHubscompatibleEndpoint),
                    hub.EventHubscompatiblePath, IotHubSasKeyName, hub.ServicePrimaryKey);
                var eventHubClient = EventHubClient.CreateFromConnectionString(connectionString.ToString());

                var tokens = new List<CancellationTokenSource>();
                HubTokens.Add(hub.ID, tokens);

                var runtimeInfo = await eventHubClient.GetRuntimeInformationAsync();
                var d2CPartitions = runtimeInfo.PartitionIds;

                var tasks = new List<Task>();
                foreach (var partition in d2CPartitions)
                {
                    var cts = new CancellationTokenSource();
                    tokens.Add(cts);
                    tasks.Add(ReceiveMessagesFromDeviceAsync(eventHubClient, partition, cts.Token));
                }

                // Wait for all the PartitionReceivers to finish.
                Task.WaitAll(tasks.ToArray());
            }
        }

        // Asynchronously create a PartitionReceiver for a partition and then start 
        // reading any messages sent from the simulated client.
        private static async Task ReceiveMessagesFromDeviceAsync(EventHubClient eventHub, string partition, CancellationToken ct)
        {
            try
            {
                // Create the receiver using the default consumer group.
                // For the purposes of this sample, read only messages sent since 
                // the time the receiver is created. Typically, you don't want to skip any messages.
                var eventHubReceiver =
                    eventHub.CreateReceiver("$Default", partition, EventPosition.FromEnqueuedTime(DateTime.Now));
                Sitecore.Diagnostics.Log.Info("Create receiver on partition: " + partition, eventHub);
                while (true)
                {
                    if (ct.IsCancellationRequested)
                        break;

                    Sitecore.Diagnostics.Log.Info("Listening for messages on: " + partition, eventHub);
                    // Check for EventData - this methods times out if there is nothing to retrieve.
                    var events = await eventHubReceiver.ReceiveAsync(100);
                    if (events == null)
                        continue;

                    // If there is data in the batch, process it.
                    foreach (var eventData in events)
                    {
                        if (eventData?.Body.Array == null)
                            continue;

                        var data = Encoding.UTF8.GetString(eventData.Body.Array);
                        Sitecore.Diagnostics.Log.Info($"Message received on partition {partition}: {data}", eventHub);

                        // Find the key that brings the method reference - won't handle the message if this is not found
                        var methodKey = eventData.Properties.Keys.FirstOrDefault(p => p.ToLower() == MethodProperyName.ToLower());
                        if (string.IsNullOrEmpty(methodKey))
                            continue;
                        var methodNameOrPath = eventData.Properties[methodKey].ToString();
                        var method = HubRepository.GetMethodByName(methodNameOrPath, _masterDb);
                        if (method == null)
                            continue;

                        // Get payload from device (if any)
                        var payloadKey = eventData.Properties.Keys.FirstOrDefault(p => p.ToLower() == PayloadProperyName.ToLower());
                        var payload = string.IsNullOrEmpty(payloadKey) ? string.Empty : eventData.Properties[payloadKey].ToString();

                        // Store results on cache
                        MethodCacheManager.SaveResponseToCache(method, payload, data);
                    }
                }
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error($"Error processing Device to Cloud events", e, e);
            }
        }
    }
}