using System.Collections.Generic;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Deserializers;
using IoTHub.Foundation.Azure.Repositories;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of an IoT Device
    /// </summary>
    public partial class IoTDevice
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

        /// <summary>
        /// Get the Hub (parent) for current Device
        /// </summary>
        /// <returns></returns>
        public IoTHub GetHub()
        {
            return _ioTHubRepository.CastToHub(InnerItem.Parent);
        }

        /// <summary>
        /// Get all methods registered for this device
        /// </summary>
        /// <returns></returns>
        public List<IoTDeviceMethod> GetMethods()
        {
            if (DeviceType?.TargetItem == null)
                return new List<IoTDeviceMethod>();

            var deviceType = _ioTHubRepository.CastToDeviceType(DeviceType.TargetItem);
            return deviceType==null ? new List<IoTDeviceMethod>() : deviceType.GetMethods();
        }

        /// <summary>
        /// Invoke a Method at this Device passing optional payload
        /// </summary>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public DynamicMessage Invoke(IoTDeviceMethod method, string payload = "")
        {
            return method.Invoke(this, payload);
        }
    }
}