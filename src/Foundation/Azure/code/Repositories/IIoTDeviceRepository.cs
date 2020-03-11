using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTDeviceRepository
    {
        /// <summary>
        /// Cast Item to IoTDevice
        /// </summary>
        /// <param name="deviceItem"></param>
        /// <returns></returns>
        IoTDevice CastToDevice(Item deviceItem);

        /// <summary>
        /// Get IoTDevice from Sitecore.ID
        /// </summary>
        /// <param name="deviceId"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDevice GetDevice(ID deviceId, Database database = null);

        /// <summary>
        /// Get IoTDevice from string reference (Sitecore path or ID)
        /// </summary>
        /// <param name="devicePath"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDevice GetDevice(string devicePath, Database database = null);

        /// <summary>
        /// Get IoTDevice with a certain Name
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="deviceName"></param>
        /// <returns></returns>
        IoTDevice GetDeviceByName(Models.Templates.IoTHub hub, string deviceName);

        /// <summary>
        /// Get IoTDevice with a certain Name
        /// </summary>
        /// <param name="hubName"></param>
        /// <param name="deviceName"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDevice GetDeviceByName(string hubName, string deviceName, Database database = null);
    }
}
