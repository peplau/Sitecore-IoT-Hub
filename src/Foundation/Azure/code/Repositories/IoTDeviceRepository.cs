using System.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTDeviceRepository : IIoTDeviceRepository
    {
        public IoTDevice CastToDevice(Item deviceItem)
        {
            return deviceItem==null || deviceItem.TemplateID != IoTDevice.TemplateID
                ? null
                : new IoTDevice(deviceItem);
        }

        public IoTDevice GetDevice(ID deviceId, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToDevice(database.GetItem(deviceId));
        }

        public IoTDevice GetDevice(string devicePath, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToDevice(database.GetItem(devicePath));
        }

        public IoTDevice GetDeviceByName(Models.Templates.IoTHub hub, string deviceName)
        {
            var device = hub.InnerItem.Children.Where(p => p.TemplateID == IoTDevice.TemplateID)
                .Select(CastToDevice).FirstOrDefault(p => p.DeviceName == deviceName);
            return device;
        }
    }
}