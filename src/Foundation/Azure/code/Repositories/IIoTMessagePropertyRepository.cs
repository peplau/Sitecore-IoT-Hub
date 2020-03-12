using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTMessagePropertyRepository
    {
        /// <summary>
        /// Cast Item to IoTMessageProperty
        /// </summary>
        /// <param name="propertyItem"></param>
        /// <returns></returns>
        Models.Templates.IoTMessageProperty CastToMessageProperty(Item propertyItem);
    }
}
