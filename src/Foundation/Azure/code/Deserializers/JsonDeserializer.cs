using Newtonsoft.Json;

namespace IoTHub.Foundation.Azure.Deserializers
{
    /// <summary>
    /// Json Deserializer - parses Json to dynamic DynamicMessage dictionary
    /// </summary>
    public class JsonDeserializer : IDeserializer
    {
        /// <summary>
        /// Parse a serialized Json to DynamicMessage
        /// </summary>
        /// <param name="serialized"></param>
        /// <returns></returns>
        public DynamicMessage Deserialize(string serialized)
        {
            var deserialized = JsonConvert.DeserializeObject<DynamicMessage>(serialized);
            return deserialized;
        }
    }
}