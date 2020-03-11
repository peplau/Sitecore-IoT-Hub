using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace IoTDevices.Configuration
{
    public class ConfigurationManager
    {
        private readonly IEnumerable<XElement> _hubNodes;

        public ConfigurationManager()
        {
            _hubNodes = XElement.Load(GetConfigXmlPath()).Elements("hub");
        }

        private static string GetConfigXmlPath()
        {
            var currentDir = System.AppContext.BaseDirectory;
            while (true)
            {
                var currentFilePath = $"{currentDir}\\configuration.xml";
                if (File.Exists(currentFilePath))
                    return currentFilePath;
                var dir = new DirectoryInfo(currentDir).Parent;
                if (dir==null)
                    return string.Empty;
                currentDir = dir.FullName;
            }            
        }

        public List<Hub> GetHubs()
        {
            var ret = _hubNodes.Where(p => p.Attribute("name") != null).Select(p => new Hub(p.Attribute("name")?.Value))
                .ToList();
            return ret;
        }

        public XElement GetHubXmlNode(string hubName)
        {
            var node = _hubNodes.FirstOrDefault(
                p => p.Attribute("name") != null && p.Attribute("name")?.Value == hubName);
            return node;
        }

        public List<Device> GetDevices(string hubName)
        {
            var hubXmlNode = GetHubXmlNode(hubName);
            if (hubXmlNode == null)
                return new List<Device>();

            var devices = hubXmlNode.Elements("device");
            var ret = devices.Select(p => new Device(p.Attribute("name")?.Value, p.Attribute("connectionString")?.Value))
                .ToList();
            return ret;
        }
    }
}
