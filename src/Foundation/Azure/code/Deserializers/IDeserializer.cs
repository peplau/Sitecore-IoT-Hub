namespace IoTHub.Foundation.Azure.Deserializers
{
    interface IDeserializer
    {
        DynamicMessage Deserialize(string serialized);
    }
}
