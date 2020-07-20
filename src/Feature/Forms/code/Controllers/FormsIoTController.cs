using System.Collections.Generic;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Repositories;

namespace IoTHub.Feature.Forms.Controllers
{
    public class FormsIoTController : Controller
    {
        private readonly IIoTHubRepository _hubRepository;
        public FormsIoTController(IIoTHubRepository hubRepository)
        {
            _hubRepository = hubRepository;
        }

        // GET api/<controller>
        public ActionResult GetMethods(string deviceId)
        {
            var device = _hubRepository.GetDevice(deviceId);
            var methods = device.GetMethods();
            var result = new List<object>();
            foreach (var method in methods)
                result.Add(new
                {
                    Key = method.ID.ToString(),
                    Value = method.MethodName
                });
            var json = Json(result);
            json.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return json;
        }
    }
}