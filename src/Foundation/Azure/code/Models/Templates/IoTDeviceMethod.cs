using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Deserializers;
using IoTHub.Foundation.Azure.Repositories;
using Microsoft.Azure.Devices;
using Newtonsoft.Json;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of an IoT Device Method
    /// </summary>
    public partial class IoTDeviceMethod
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

        /// <summary>
        /// Get Hub for this method (Parent.Parent)
        /// </summary>
        /// <returns></returns>
        public IoTHub GetHub()
        {
            return _ioTHubRepository.CastToHub(InnerItem.Parent.Parent);
        }

        /// <summary>
        /// Get Device for this method (Parent)
        /// </summary>
        /// <returns></returns>
        public IoTDevice GetDevice()
        {
            return _ioTHubRepository.CastToDevice(InnerItem.Parent);
        }

        /// <summary>
        /// Get MessageType/ReturnType for this method
        /// </summary>
        /// <returns></returns>
        public IoTMessageType GetMessageType()
        {
            var typeItem = ReturnType?.TargetItem;
            return typeItem == null ? null : _ioTHubRepository.CastToMessageType(typeItem);
        }

        /// <summary>
        /// Invoke this Method passing optional payload
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public dynamic Invoke(string payload = "")
        {
            return InvokeMethod(this, payload);
        }

        /// <summary>
        /// Invoke a given Method passing optional payload
        /// </summary>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static dynamic InvokeMethod(IoTDeviceMethod method, string payload="")
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
            var methodInvocation = new CloudToDeviceMethod(method.MethodName) {ResponseTimeout = TimeSpan.FromSeconds(30)};

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
            Sitecore.Diagnostics.Log.Info($"Response status: {response.Status}, payload:", typeof(IoTDeviceMethod));
            Sitecore.Diagnostics.Log.Info(receivedPayload, typeof(IoTDeviceMethod));

            // Get returnType and deserializer
            var returnType = method.GetMessageType();
            if (returnType == null) {
                Sitecore.Diagnostics.Log.Error($"Method {method.ID} has an invalid Return Type", typeof(IoTDeviceMethod));
                return null;
            }
            var deserializer = returnType.GetDeserializer();
            if (deserializer == null) {
                Sitecore.Diagnostics.Log.Error($"Message Type {returnType.ID} has an invalid Deserializer",
                    typeof(IoTDeviceMethod));
                return null;
            }

            // Instantiate Deserializer object
            var deserializerObject = deserializer.GetDeserializerObject();
            if (deserializerObject == null)
                return null;

            // Parse results
            var parsedDictionary = (DynamicMessage)deserializerObject.Deserialize(receivedPayload);
            parsedDictionary.RawMessage = receivedPayload;
            return parsedDictionary;
        }
    }
}