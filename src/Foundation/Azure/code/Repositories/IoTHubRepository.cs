using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTHubRepository : IIoTHubRepository
    {
        public Models.Templates.IoTHub CastToHub(Item hubItem)
        {
            return hubItem==null || hubItem.TemplateID != Models.Templates.IoTHub.TemplateID
                ? null
                : new Models.Templates.IoTHub(hubItem);
        }

        public Models.Templates.IoTHub GetHub(ID hubId, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToHub(database.GetItem(hubId));
        }

        public Models.Templates.IoTHub GetHub(string hubPath, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            return CastToHub(database.GetItem(hubPath));
        }

        public Models.Templates.IoTHub GetHubByName(string hubName, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;

            var hubsRepositoryItem = database.GetItem(Models.Templates.IoTHub.HubsRepositoryId);
            if (hubsRepositoryItem == null)
            {
                Sitecore.Diagnostics.Log.Error(
                    $"Can't find the IoT Hubs Repository item - ID: {Models.Templates.IoTHub.HubsRepositoryId}", this);
                return null;
            }

            var hubItem = hubsRepositoryItem.Children.Where(p => p.TemplateID == Models.Templates.IoTHub.TemplateID)
                .Select(CastToHub).FirstOrDefault(p => p.HubName == hubName);
            return hubItem;
        }
    }
}