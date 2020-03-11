﻿using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Repositories;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of an IoT Device
    /// </summary>
    public partial class IoTDevice
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();
        private readonly IIoTMethodRepository _ioTMethodRepository =
            DependencyResolver.Current.GetService<IIoTMethodRepository>();

        /// <summary>
        /// Get the Hub (parent) for current Device
        /// </summary>
        /// <returns></returns>
        public IoTHub GetHub()
        {
            return _ioTHubRepository.CastToHub(InnerItem.Parent);
        }

        /// <summary>
        /// Get all methods registered for this device
        /// </summary>
        /// <returns></returns>
        public List<IoTDeviceMethod> GetMethods()
        {
            return InnerItem.Children.Where(p => p.TemplateID == IoTDeviceMethod.TemplateID)
                .Select(p => _ioTMethodRepository.CastToMethod(p)).ToList();
        }
    }
}