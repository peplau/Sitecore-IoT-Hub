using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore.Data;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of an IoT Hub
    /// </summary>
    public partial class IoTHub
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

        /// <summary>
        /// ID of the Hubs Repository - By default: {62BA65C8-7E6C-4BD5-AA1F-E6EC9E85A26A}
        /// </summary>
        public static readonly ID HubsRepositoryId =
            ID.Parse(Sitecore.Configuration.Settings.GetSetting("IoTHub.HubsRepositoryId",
                "{62BA65C8-7E6C-4BD5-AA1F-E6EC9E85A26A}"));

        /// <summary>
        /// Get all devices at the current Hub
        /// </summary>
        /// <returns></returns>
        public List<IoTDevice> GetDevices()
        {
            return InnerItem.Children.Where(p => p.TemplateID == IoTDevice.TemplateID)
                .Select(p => _ioTHubRepository.CastToDevice(p)).ToList();
        }
    }
}