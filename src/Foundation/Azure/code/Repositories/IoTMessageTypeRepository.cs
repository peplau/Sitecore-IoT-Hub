using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTMessageTypeRepository : IIoTMessageTypeRepository
    {
        public IoTMessageType CastToMessageType(Item messageTypeItem)
        {
            return messageTypeItem==null || messageTypeItem.TemplateID != IoTMessageType.TemplateID
                ? null
                : new IoTMessageType(messageTypeItem);
        }
    }
}