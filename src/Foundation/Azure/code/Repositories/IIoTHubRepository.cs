using System.Collections.Generic;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public interface IIoTHubRepository
    {
        /// <summary>
        /// Cast Item to IoTHub
        /// </summary>
        /// <param name="hubItem"></param>
        /// <returns></returns>
        Models.Templates.IoTHub CastToHub(Item hubItem);

        /// <summary>
        /// Get IoTHub from Sitecore.ID
        /// </summary>
        /// <param name="hubId"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        Models.Templates.IoTHub GetHub(ID hubId, Database database = null);

        /// <summary>
        /// Get IoTHub from string reference (Sitecore path or ID)
        /// </summary>
        /// <param name="hubPath"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        Models.Templates.IoTHub GetHub(string hubPath, Database database = null);

        /// <summary>
        /// Get IoTHub with a certain Name
        /// </summary>
        /// <param name="hubName"></param>
        /// <param name="database"></param>
        /// <returns></returns>
        Models.Templates.IoTHub GetHubByName(string hubName, Database database = null);

        /// <summary>
        /// Get a list of all available Hubs
        /// </summary>
        /// <param name="database"></param>
        /// <returns></returns>
        List<Models.Templates.IoTHub> GetHubs(Database database = null);
    }
}
