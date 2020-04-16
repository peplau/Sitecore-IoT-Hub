using System;
using System.Collections.Generic;
using System.Linq;

namespace IoTDevices.Configuration
{
    public static class GetDeviceLoop
    {
        public static Device Run()
        {
            return Run(string.Empty);
        }

        public static Device Run(string selectedName)
        {
            Device selectedDevice;

            while (true)
            {
                // Chose configuration
                Console.WriteLine();
                var configurationManager = new ConfigurationManager();
                var hubs = configurationManager.GetHubs();
                var index = 0;
                var allDevices = new List<Device>();
                foreach (var hub in hubs)
                {
                    var devices = configurationManager.GetDevices(hub.Name);
                    foreach (var device in devices)
                    {
                        allDevices.Add(device);
                        Console.WriteLine($"[{index}] {hub.Name} > {device.Name}");
                        index++;
                    }
                }
                Console.WriteLine();
                Console.Write("Please chose the IoT Device Configuration: ");

                // Get selected device
                int intSelected;
                var selected = string.IsNullOrEmpty(selectedName)
                    ? -1
                    : allDevices.IndexOf(allDevices.FirstOrDefault(p => p.Name == selectedName));

                if (selected ==-1)
                {
                    var typed = Console.ReadLine();
                    if (!int.TryParse(typed, out intSelected))
                        continue;
                }
                else
                {
                    intSelected = selected;
                    Console.WriteLine(intSelected);
                }
                if (intSelected < 0 || intSelected > allDevices.Count - 1)
                    continue;
                
                selectedDevice = allDevices[intSelected];

                Console.WriteLine($"Selected device: {selectedDevice.Name}");

                break;
            }

            return selectedDevice;
        }
    }
}
