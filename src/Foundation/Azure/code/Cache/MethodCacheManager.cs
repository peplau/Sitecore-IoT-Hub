using System.Text;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;

namespace IoTHub.Foundation.Azure.Cache
{
    public class MethodCacheManager : IMethodCacheManager
    {
        private readonly Database _codeDatabase = Sitecore.Configuration.Factory.GetDatabase("core");
        private Database GetDatabase(Database db = null)
        {
            return db ?? _codeDatabase;
        }

        /// <summary>
        /// Get the last Response saved on DB cache for a given method and payload
        /// </summary>
        /// <param name="device"></param>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public string GetResponseFromCache(IoTDevice device, IoTDeviceMethod method, string payload, Database db = null)
        {
            if (method == null)
                return string.Empty;

            db = GetDatabase(db);
            if (db == null)
                return string.Empty;

            var key = GetMethodKey(device, method, payload);
            if (string.IsNullOrEmpty(key))
                return string.Empty;

            // Retrieve and return saved value
            var savedValue = db.PropertyStore.GetStringValue(key);
            return savedValue;
        }

        /// <summary>
        /// Save response to DB cache for a given method and payload
        /// </summary>
        /// <param name="device"></param>
        /// <param name="method"></param>
        /// <param name="payload"></param>
        /// <param name="response"></param>
        /// <param name="db"></param>
        public void SaveResponseToCache(IoTDevice device, IoTDeviceMethod method, string payload, string response, Database db = null)
        {
            if (method == null)
                return;

            db = GetDatabase(db);
            if (db == null)
                return;

            var key = GetMethodKey(device, method, payload);
            if (string.IsNullOrEmpty(key))
                return;

            // Save value to DB
            db.PropertyStore.SetStringValue(key,response);
        }

        private static string GetMethodKey(IoTDevice device, IoTDeviceMethod method, string payload)
        {
            var sb = new StringBuilder();
            sb.Append(device.ID);
            sb.Append("_");
            sb.Append(method.ID);
            sb.Append("_");
            sb.Append(GetMd5(payload));
            return sb.ToString();
        }

        private static string GetMd5(string input)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // Use input string to calculate MD5 hash
            var inputBytes = Encoding.ASCII.GetBytes(input);
            using (var md5 = System.Security.Cryptography.MD5.Create())
            {
                var hashBytes = md5.ComputeHash(inputBytes);
                // Convert the byte array to hexadecimal string
                var sb = new StringBuilder();                
                foreach (var t in hashBytes)
                    sb.Append(t.ToString("X2"));
                return sb.ToString();
            }
        }
    }
}