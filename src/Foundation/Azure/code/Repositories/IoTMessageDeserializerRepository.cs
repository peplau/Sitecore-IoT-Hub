using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTMessageDeserializerRepository : IIoTMessageDeserializerRepository
    {
        public IoTMessageDeserializer CastToMessageDeserializer(Item deserializerItem)
        {
            return deserializerItem==null || deserializerItem.TemplateID != IoTMessageDeserializer.TemplateID
                ? null
                : new IoTMessageDeserializer(deserializerItem);
        }
    }
}