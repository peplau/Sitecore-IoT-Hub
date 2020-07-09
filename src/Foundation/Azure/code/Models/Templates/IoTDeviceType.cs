using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Repositories;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    public partial class IoTDeviceType
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

        public List<IoTDeviceMethod> GetMethods()
        {
            return InnerItem.Children.Where(p => p.TemplateID == IoTDeviceMethod.TemplateID)
                .Select(p => _ioTHubRepository.CastToMethod(p)).ToList();
        }
    }
}