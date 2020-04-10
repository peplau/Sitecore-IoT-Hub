﻿using System.Linq;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Models.Templates;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore.ExperienceForms.ValueProviders;

namespace IoTHub.Feature.Forms.ValueProviders
{
    /// <summary>
    /// Value provider to pre-load fields with values taken from IoT Objects
    /// </summary>
    public class IoTHubValueProvider: IFieldValueProvider
    {
        private readonly IIoTHubRepository _hubRepository = DependencyResolver.Current.GetService<IIoTHubRepository>();

        /// <summary>
        /// Parameters accepts string separated by ','
        /// Position 0 = Sitecore ID, Sitecore Path or dot separated string Eg: "HubName.DeviceName.MethodName"
        /// Position 1 = PropertyName
        /// Position 2 = Payload
        /// </summary>
        /// <param name="strParameters"></param>
        /// <returns></returns>
        public object GetValue(string strParameters)
        {
            if (string.IsNullOrEmpty(strParameters) || !strParameters.Contains(","))
                return string.Empty;

            var parameters = strParameters.Split(',');
            if (parameters.Length<2)
                return string.Empty;

            var methodPart = parameters[0].Trim();
            var propertyPart = parameters[1].Trim();
            var payloadPart = parameters.Length>2 ? parameters[2].Trim() : string.Empty;
            if (string.IsNullOrEmpty(methodPart) || string.IsNullOrEmpty(propertyPart))
                return string.Empty;

            // Get method
            IoTDeviceMethod method;
            if (methodPart.Contains(".")) {
                var names = methodPart.Split('.').Select(p=>p.Trim()).ToArray();
                if (names.Length!=3)
                    return string.Empty;
                method = _hubRepository.GetMethodByName(names[0], names[1], names[2]);
            }
            else
                method = _hubRepository.GetMethod(methodPart);
            if (method==null)
                return string.Empty;

            // Invoke method and get result
            var result = method.Invoke(payloadPart);
            var propertyValue = result.GetValue(propertyPart);
            return propertyValue.ToString();
        }

        public FieldValueProviderContext ValueProviderContext { get; set; }
    }
}