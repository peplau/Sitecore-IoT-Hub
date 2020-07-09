using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTDeviceTypeRepository : IIoTDeviceTypeRepository
    {
        public IoTDeviceType CastToDeviceType(Item deviceTypeItem)
        {
            return deviceTypeItem==null || deviceTypeItem.TemplateID != IoTDeviceType.TemplateID
                ? null
                : new IoTDeviceType(deviceTypeItem);
        }

        public IoTDeviceType GetDeviceType(ID deviceTypeId, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToDeviceType(database.GetItem(deviceTypeId));            
        }

        public IoTDeviceType GetDeviceType(string deviceTypePath, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToDeviceType(database.GetItem(deviceTypePath));
        }
    }
}