using System.Collections.Generic;
using System.Dynamic;

namespace IoTHub.Foundation.Azure.Deserializers
{
    /// <summary>
    /// Represents a Dynamic dictionary to store the parsed message
    /// </summary>
    public class DynamicMessage : DynamicObject
    {
        // The inner dictionary.
        private readonly Dictionary<string, object> _dictionary = new Dictionary<string, object>();

        public string RawMessage { get; set; }

        /// <summary>
        /// Get value from string key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            object ret = null;
            if (_dictionary.ContainsKey(key))
                ret = _dictionary[key];
            return ret;
        }

        // This property returns the number of elements
        // in the inner dictionary.
        public int Count => _dictionary.Count;

        // If you try to get a value of a property 
        // not defined in the class, this method is called.
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            // If the property name is found in a dictionary,
            // set the result parameter to the property value and return true.
            // Otherwise, return false.
            return _dictionary.TryGetValue(binder.Name, out result);
        }

        // If you try to set a value of a property that is
        // not defined in the class, this method is called.
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            _dictionary[binder.Name] = value;

            // You can always add a value to a dictionary,
            // so this method always returns true.
            return true;
        }
    }
}