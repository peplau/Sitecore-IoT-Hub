using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTMessageTypeRepository
    {
        /// <summary>
        /// Cast Item to IoTMessageType
        /// </summary>
        /// <param name="messageTypeItem"></param>
        /// <returns></returns>
        IoTMessageType CastToMessageType(Item messageTypeItem);
    }
}
