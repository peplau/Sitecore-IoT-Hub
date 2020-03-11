using System;

namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of a Message Deserializer - responsible for deserializing messages from IoT Methods
    /// </summary>
    public partial class IoTMessageDeserializer
    {
        /// <summary>
        /// Get instance of Deserializer class, according to the field DeserializerType
        /// </summary>
        /// <returns></returns>
        public dynamic GetDeserializerObject()
        {
            // Instantiate Deserializer object
            var objectType = Type.GetType(DeserializerType);
            if (objectType == null)
            {
                Sitecore.Diagnostics.Log.Error($"Cannot instantiate object of class {DeserializerType}", this);
                return null;
            }
            var deserializerObject = (dynamic) Activator.CreateInstance(objectType);
            if (deserializerObject != null) 
                return deserializerObject;

            Sitecore.Diagnostics.Log.Error($"Cannot instantiate object of class {DeserializerType}", this);
            return null;
        }

    }
}