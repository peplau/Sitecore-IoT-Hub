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
        public DynamicMessage Invoke(string payload = "")
        {
            var args = new InvokeMethodArgs {Method = this, Payload = payload};
            CorePipeline.Run("iotHub.InvokeMethod", args);
            return args.Response;
        }
    }
}