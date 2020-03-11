namespace IoTHub.Foundation.Azure.Models.Templates
{
    /// <summary>
    /// Sitecore representation of a Message Property
    /// </summary>
    public partial class IoTMessageProperty
    {
        /// <summary>
        /// Get a value from a given dictionary
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public dynamic GetValue(dynamic dictionary)
        {
            // Parse result
            var rawObject = dictionary.GetValue(PropertyName);
            return rawObject;
        }
    }
}