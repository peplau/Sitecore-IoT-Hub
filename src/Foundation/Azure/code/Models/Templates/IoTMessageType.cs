using System.Web.Mvc;
using IoTHub.Foundation.Azure.Repositories;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of a Message Type (Return Type)
    /// </summary>
    public partial class IoTMessageType
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

        /// <summary>
        /// Get Deserializer model for this message type
        /// </summary>
        /// <returns></returns>
        public IoTMessageDeserializer GetDeserializer()
        {
            var deserializerItem = Deserializer?.TargetItem;
            return deserializerItem == null
                ? null
                : _ioTHubRepository.CastToMessageDeserializer(deserializerItem);
        }
    }
}