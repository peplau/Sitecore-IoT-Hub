using System.Web.Mvc;
using IoTHub.Foundation.Azure.Deserializers;
using IoTHub.Foundation.Azure.Pipelines;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore.Pipelines;

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
        /// Get MessageType/ReturnType for this method
        /// </summary>
        /// <returns></returns>
        public IoTMessageType GetMessageType()
        {
            var typeItem = ReturnType?.TargetItem;
            return typeItem == null ? null : _ioTHubRepository.CastToMessageType(typeItem);
        }

        /// <summary>
        /// Invoke this Method in a given Device passing optional payload
        /// </summary>
        /// <param name="device"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public DynamicMessage Invoke(IoTDevice device, string payload = "")
        {
            var args = new InvokeMethodArgs {Device = device, Method = this, Payload = payload};
            CorePipeline.Run("iotHub.InvokeMethod", args);
            return args.Response;
        }
    }
}