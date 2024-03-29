﻿using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;

namespace IoTHub.Foundation.Azure.Cache
{
    public interface IMethodCacheManager
    {
        string GetResponseFromCache(IoTDevice device, IoTDeviceMethod method, string payload, Database db = null);
        void SaveResponseToCache(IoTDevice device, IoTDeviceMethod method, string payload, string response,
            Database db = null);
    }
}