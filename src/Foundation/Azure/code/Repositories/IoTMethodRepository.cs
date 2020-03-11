using System.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTMethodRepository : IIoTMethodRepository
    {
        private readonly IoTDeviceRepository _deviceRepository = new IoTDeviceRepository();

        public IoTDeviceMethod CastToMethod(Item methodItem)
        {
            return methodItem==null || methodItem.TemplateID != IoTDeviceMethod.TemplateID
                ? null
                : new IoTDeviceMethod(methodItem);
        }

        public IoTDeviceMethod GetMethod(ID methodId, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToMethod(database.GetItem(methodId));
        }

        public IoTDeviceMethod GetMethod(string methodPath, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToMethod(database.GetItem(methodPath));
        }

        public IoTDeviceMethod GetMethodByName(IoTDevice device, string methodName)
        {
            var method = device.InnerItem.Children.Where(p => p.TemplateID == IoTDeviceMethod.TemplateID)
                .Select(CastToMethod).FirstOrDefault(p => p.MethodName == methodName);
            return method;
        }

        public IoTDeviceMethod GetMethodByName(string hubName, string deviceName, string methodName, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;

            var device = _deviceRepository.GetDeviceByName(hubName, deviceName, database);
            return device == null ? null : GetMethodByName(device, methodName);
        }
    }
}