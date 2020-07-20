using System.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTMethodRepository : IIoTMethodRepository
    {
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
            var method = device.GetMethods().FirstOrDefault(p => p.MethodName == methodName);
            return method;
        }
    }
}