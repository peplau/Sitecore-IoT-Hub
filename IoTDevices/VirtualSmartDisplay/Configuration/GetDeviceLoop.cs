using System;
using System.Collections.Generic;
using System.Linq;

namespace IoTDevices.VirtualSmartDisplay.Configuration
{
    public static class GetDeviceLoop
    {
        public delegate void WriteLine(string line);

        public static Device Run(string selectedName, WriteLine writeLine)
        {
            Device selectedDevice;

            while (true)
            {
                // Chose configuration
                writeLine(string.Empty);
                var configurationManager = new ConfigurationManager();
                var hubs = configurationManager.GetHubs();
                var allDevices = new List<Device>();
                foreach (var hub in hubs)
                {
                    var devices = configurationManager.GetDevices(hub.Name);
                    foreach (var device in devices)
                        allDevices.Add(device);
                }

                // Get selected device
                var selected = string.IsNullOrEmpty(selectedName)
                    ? -1
                    : allDevices.IndexOf(allDevices.FirstOrDefault(p => p.Name == selectedName));

                var intSelected = selected;
                writeLine("Please chose the IoT Device Configuration: " + intSelected);
                if (intSelected < 0 || intSelected > allDevices.Count - 1)
                    continue;
                
                selectedDevice = allDevices[intSelected];
                writeLine($"Selected device: {selectedDevice.Name}");
                break;
            }

            return selectedDevice;
        }
    }
}
