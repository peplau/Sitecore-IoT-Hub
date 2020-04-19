using System;
using System.Threading.Tasks;
using IoTHub.Foundation.Azure.Cache;
using IoTHub.Foundation.Azure.Deserializers;
using IoTHub.Foundation.Azure.Models.Templates;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;
using Sitecore.Diagnostics;

namespace IoTHub.Foundation.Azure.Pipelines
{
    public class InvokeMethodProcessor
    {
        private readonly IMethodCacheManager _methodCacheManager;
        public InvokeMethodProcessor(IMethodCacheManager methodCacheManager)
        {
            _methodCacheManager = methodCacheManager;
        }

        public void Process(InvokeMethodArgs args)
        {
            Assert.ArgumentNotNull(args, nameof(args));
            Assert.ArgumentNotNull(args.Method, nameof(args.Method));

            // No two way - call the method directly
            if (!args.Method.TwoWay)
            {
                args.Response = InvokeMethod(args.Method, args.Payload);
                return;
            }

            // Two way - gets from cache 
            var resultFromCache = _methodCacheManager.GetResponseFromCache(args.Method, args.Payload);
            if (!string.IsNullOrEmpty(resultFromCache))
            {
                args.Response = DeserializeAndParse(args.Method, resultFromCache);
                return;
            }

            // Or the method itself
            args.Response = InvokeMethod(args.Method, args.Payload);
            // If taken from Itself then needs to save back to cache if TwoWay
            _methodCacheManager.SaveResponseToCache(args.Method, args.Payload, args.Response.RawMessage);
        }

        /// <summary>
        /// Invoke a given Method passing optional payload
        /// </summary>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static DynamicMessage InvokeMethod(IoTDeviceMethod method, string payload = "")
        {
            var hub = method.GetHub();
            var connectionStringsServer = hub.ConnectionString;
            var parsedDictionary =
                InvokeMethodInternal(method, connectionStringsServer, payload).GetAwaiter().GetResult();
            return parsedDictionary;
        }

        /// <summary>
        /// Internal Invoke method call
        /// </summary>
        /// <param name="method"></param>
        /// <param name="connectionStringsServer"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        private static async Task<DynamicMessage> InvokeMethodInternal(IoTDeviceMethod method,
            string connectionStringsServer, string payload = "")
        {
            var methodInvocation = new CloudToDeviceMethod(method.MethodName) { ResponseTimeout = TimeSpan.FromSeconds(30) };

            // Payload to send            
            if (!string.IsNullOrEmpty(payload))
                methodInvocation.SetPayloadJson(JsonConvert.SerializeObject(payload));

            // Invoke the direct method asynchronously and get the response from the device.
            var device = method.GetDevice();
            var serviceClient = ServiceClient.CreateFromConnectionString(connectionStringsServer);
            var response = serviceClient.InvokeDeviceMethodAsync(device.DeviceName, methodInvocation).GetAwaiter().GetResult();

            // Resulting Payload
            var receivedPayload = response.GetPayloadAsJson();

            // Log results
            Log.Info($"Response status: {response.Status}, payload:", typeof(IoTDeviceMethod));
            Log.Info(receivedPayload, typeof(IoTDeviceMethod));

            // Deserialize
            var result = DeserializeAndParse(method, receivedPayload);
            return result;
        }

        /// <summary>
        /// Deserialize and Parse a string using Deserializer from the giving method
        /// </summary>
        /// <param name="method"></param>
        /// <param name="response"></param>
        /// <returns></returns>
        public static DynamicMessage DeserializeAndParse(IoTDeviceMethod method, string response)
        {
            // Get returnType and deserializer
            var returnType = method.GetMessageType();
            if (returnType == null)
            {
                Log.Error($"Method {method.ID} has an invalid Return Type", typeof(IoTDeviceMethod));
                return null;
            }
            var deserializer = returnType.GetDeserializer();
            if (deserializer == null)
            {
                Log.Error($"Message Type {returnType.ID} has an invalid Deserializer",
                    typeof(IoTDeviceMethod));
                return null;
            }

            // Instantiate Deserializer object
            var deserializerObject = deserializer.GetDeserializerObject();
            if (deserializerObject == null)
                return null;

            // Parse results
            var parsedDictionary = (DynamicMessage)deserializerObject.Deserialize(response);
            parsedDictionary.RawMessage = response;
            return parsedDictionary;
        }
    }
}