using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTMethodRepository
    {
        /// <summary>
        /// Cast Item to IoTDeviceMethod
        /// </summary>
        /// <param name="methodItem"></param>
        /// <returns></returns>
        IoTDeviceMethod CastToMethod(Item methodItem);

        /// <summary>
        /// Get IoTDeviceMethod from Sitecore.ID
        /// </summary>
        /// <param name="methodId"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDeviceMethod GetMethod(ID methodId, Database database = null);

        /// <summary>
        /// Get IoTDeviceMethod from string reference (Sitecore path or ID)
        /// </summary>
        /// <param name="methodPath"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDeviceMethod GetMethod(string methodPath, Database database = null);

        /// <summary>
        /// Get IoTDeviceMethod with a certain Name
        /// </summary>
        /// <param name="device"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        IoTDeviceMethod GetMethodByName(IoTDevice device, string methodName);

        /// <summary>
        /// Get IoTDeviceMethod with a certain Name
        /// </summary>
        /// <param name="hubName"></param>
        /// <param name="deviceName"></param>
        /// <param name="methodName"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDeviceMethod GetMethodByName(string hubName, string deviceName, string methodName, Database database = null);
    }
}
