using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTDeviceTypeRepository
    {
        /// <summary>
        /// Cast Item to IoTDeviceType
        /// </summary>
        /// <param name="deviceTypeItem"></param>
        /// <returns></returns>
        IoTDeviceType CastToDeviceType(Item deviceTypeItem);

        /// <summary>
        /// Get IoTDevice from Sitecore.ID
        /// </summary>
        /// <param name="deviceTypeId"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDeviceType GetDeviceType(ID deviceTypeId, Database database = null);

        /// <summary>
        /// Get IoTDevice from string reference (Sitecore path or ID)
        /// </summary>
        /// <param name="deviceTypePath"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        IoTDeviceType GetDeviceType(string deviceTypePath, Database database = null);
    }
}
