using System.Web.Mvc;
using IoTHub.Feature.Demo.Models;
using IoTHub.Foundation.Azure.Models.Templates;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace IoTHub.Feature.Demo.Controllers
{
    public class SmartDisplayController : SitecoreController
    {
        private readonly IoTDeviceMethod _method;
        private const string MethodPath = "SitecoreIoTHub.SmartDisplay.GetSelectedObject";

        public SmartDisplayController(IIoTHubRepository ioTHubRepository)
        {
            _method = ioTHubRepository.GetMethodByName(MethodPath);
        }

        public override ActionResult Index()
        {
            var dataSourceId = RenderingContext.Current.Rendering.DataSource;
            var dataSourceItem = !string.IsNullOrEmpty(dataSourceId)
                ? Context.Database.GetItem(dataSourceId)
                : Context.Item;
            return dataSourceItem == null ? View() : View(new __ProductEntry(dataSourceItem));
        }

        [HttpPost]
        public JsonResult HasChanges(string currentState)
        {
            if (_method == null)
                return Json(false);

            dynamic response = _method.Invoke();
            string selectedObject = response.currentObject;
            if (string.IsNullOrEmpty(selectedObject))
                selectedObject = "Empty";

            var hasChanges = selectedObject != currentState;

            return Json(hasChanges);
        }
    }
}