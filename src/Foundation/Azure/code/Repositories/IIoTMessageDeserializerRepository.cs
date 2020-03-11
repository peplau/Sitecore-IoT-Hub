using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTMessageDeserializerRepository
    {
        /// <summary>
        /// Cast Item to IoTMessageDeserializer
        /// </summary>
        /// <param name="deserializerItem"></param>
        /// <returns></returns>
        IoTMessageDeserializer CastToMessageDeserializer(Item deserializerItem);
    }
}
