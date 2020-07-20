using System.Linq;
using System.Text;
using IoTHub.Foundation.Azure.Cache;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore.Diagnostics;

namespace IoTHub.Foundation.Azure.Pipelines
{
    public class MessageReceivedProcessor
    {
        private const string MethodProperyName = "method";
        private const string PayloadProperyName = "payload";

        private readonly IMethodCacheManager _methodCacheManager;
        private readonly IIoTHubRepository _hubRepository;

        public MessageReceivedProcessor(IMethodCacheManager methodCacheManager, IIoTHubRepository hubRepository)
        {
            _methodCacheManager = methodCacheManager;
            _hubRepository = hubRepository;
        }

        public void Process(MessageReceivedArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            Assert.ArgumentNotNull(args.EventData, nameof(args.EventData));        
            Assert.ArgumentNotNull(args.Database, nameof(args.Database));

            var eventData = args.EventData;
            if (eventData?.Body.Array == null)
                return;

            var data = Encoding.UTF8.GetString(eventData.Body.Array);
            Log.Info($"Message received on partition {args.Partition}: {data}", args.EventData);

            // Find the key that brings the method reference - won't handle the message if this is not found
            var methodKey = eventData.Properties.Keys.FirstOrDefault(p => p.ToLower() == MethodProperyName.ToLower());
            if (string.IsNullOrEmpty(methodKey))
                return;
            var methodNameOrPath = eventData.Properties[methodKey].ToString();
            var method = _hubRepository.GetMethodByName(methodNameOrPath, args.Database);
            var device = _hubRepository.GetDeviceByName(methodNameOrPath, args.Database);
            if (method == null || device==null)
                return;

            // Get payload from device (if any)
            var payloadKey = eventData.Properties.Keys.FirstOrDefault(p => p.ToLower() == PayloadProperyName.ToLower());
            var payload = string.IsNullOrEmpty(payloadKey) ? string.Empty : eventData.Properties[payloadKey].ToString();

            // Store results on cache
            _methodCacheManager.SaveResponseToCache(device, method, payload, data);
        }
    }
}