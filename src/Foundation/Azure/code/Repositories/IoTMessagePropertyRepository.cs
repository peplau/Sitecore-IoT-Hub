using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTMessagePropertyRepository : IIoTMessagePropertyRepository
    {
        public Models.Templates.IoTMessageProperty CastToMessageProperty(Item propertyItem)
        {
            return propertyItem==null || propertyItem.TemplateID != Models.Templates.IoTMessageProperty.TemplateID
                ? null
                : new Models.Templates.IoTMessageProperty(propertyItem);
        }
    }
}