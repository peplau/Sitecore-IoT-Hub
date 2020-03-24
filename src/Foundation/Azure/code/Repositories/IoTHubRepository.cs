using System.Collections.Generic;
using System.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace IoTHub.Foundation.Azure.Repositories
{
    public class IoTHubRepository : IIoTHubRepository
    {
        private readonly IIoTDeviceRepository _deviceRepository;
        private readonly IIoTMessageDeserializerRepository _deserializerRepository;
        private readonly IIoTMessagePropertyRepository _messagePropertyRepository;
        private readonly IIoTMessageTypeRepository _messageTypeRepository;
        private readonly IIoTMethodRepository _methodRepository;

        public IoTHubRepository(IIoTDeviceRepository deviceRepository,
            IIoTMessageDeserializerRepository deserializerRepository,
            IIoTMessagePropertyRepository messagePropertyRepository,
            IIoTMessageTypeRepository messageTypeRepository,
            IIoTMethodRepository methodRepository)
        {
            _deviceRepository = deviceRepository;
            _deserializerRepository = deserializerRepository;
            _messagePropertyRepository = messagePropertyRepository;
            _messageTypeRepository = messageTypeRepository;
            _methodRepository = methodRepository;
        }

        #region IIoTHubRepository

        public Models.Templates.IoTHub CastToHub(Item hubItem)
        {
            return hubItem == null || hubItem.TemplateID != Models.Templates.IoTHub.TemplateID
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

        public List<Models.Templates.IoTHub> GetHubs(Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;
            var hubsRoot = database.GetItem(Models.Templates.IoTHub.HubsRepositoryId);

            var hubs = hubsRoot.Children.Where(p => p.TemplateID == Models.Templates.IoTHub.TemplateID)
                .Select(CastToHub).ToList();
            return hubs;
        }

        public IoTDevice GetDeviceByName(string hubName, string deviceName, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;

            var hub = GetHubByName(hubName, database);
            return hub == null ? null : GetDeviceByName(hub, deviceName);
        }

        public IoTDeviceMethod GetMethodByName(string hubName, string deviceName, string methodName, Database database = null)
        {
            if (database == null)
                database = Sitecore.Context.Database;

            var device = GetDeviceByName(hubName, deviceName, database);
            return device == null ? null : GetMethodByName(device, methodName);
        }
        #endregion

        #region IIoTDeviceRepository

        public IoTDevice CastToDevice(Item deviceItem)
        {
            return _deviceRepository.CastToDevice(deviceItem);
        }

        public IoTDevice GetDevice(ID deviceId, Database database = null)
        {
            return _deviceRepository.GetDevice(deviceId, database);
        }

        public IoTDevice GetDevice(string devicePath, Database database = null)
        {
            return _deviceRepository.GetDevice(devicePath, database);
        }

        public IoTDevice GetDeviceByName(Models.Templates.IoTHub hub, string deviceName)
        {
            return _deviceRepository.GetDeviceByName(hub, deviceName);
        }
        #endregion

        #region IIoTMessageTypeRepository

        public IoTDeviceMethod CastToMethod(Item methodItem)
        {
            return _methodRepository.CastToMethod(methodItem);
        }

        public IoTDeviceMethod GetMethod(ID methodId, Database database = null)
        {
            return _methodRepository.GetMethod(methodId, database);
        }

        public IoTDeviceMethod GetMethod(string methodPath, Database database = null)
        {
            return _methodRepository.GetMethod(methodPath, database);
        }

        public IoTDeviceMethod GetMethodByName(IoTDevice device, string methodName)
        {
            return _methodRepository.GetMethodByName(device, methodName);
        }
        #endregion

        #region IIoTMessageDeserializerRepository

        public IoTMessageDeserializer CastToMessageDeserializer(Item deserializerItem)
        {
            return _deserializerRepository.CastToMessageDeserializer(deserializerItem);
        }

        #endregion

        #region IIoTMessagePropertyRepository

        public IoTMessageProperty CastToMessageProperty(Item propertyItem)
        {
            return _messagePropertyRepository.CastToMessageProperty(propertyItem);
        }

        #endregion

        #region IIoTMessageTypeRepository

        public IoTMessageType CastToMessageType(Item messageTypeItem)
        {
            return _messageTypeRepository.CastToMessageType(messageTypeItem);
        }

        #endregion
    }
}