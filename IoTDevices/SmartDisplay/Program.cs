﻿using System;
using System.Collections.Generic;
using System.Configuration.Internal;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.SmartDisplay
{
    class Program
    {
        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static string _currentObject = string.Empty;
        private static readonly List<string> Objects = new List<string>()
        {
            "Object 1", "Object 2", "Object 3",
            "Object 4", "Object 5", "Object 6",
            "Object 7", "Object 8", "Object 9"
        };

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("------------------------------------");
            Console.WriteLine("Welcome to the Virtual Smart Display");
            Console.WriteLine("------------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run();

            // Start up Hub client
            _sDeviceClient =
                DeviceClient.CreateFromConnectionString(_currentDevice.ConnectionString, TransportType.Mqtt);

            // Create a handler for the direct method calls
            _sDeviceClient.SetMethodHandlerAsync("GetSelectedObject", GetSelectedObject, null).Wait();

            CommandLoop();
        }

        private static Task<MethodResponse> GetSelectedObject(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method GetSelectedObject called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        private static void CommandLoop()
        {
            while (true)
            {
                GetStateMessage(out string messageString);

                Console.WriteLine();
                Console.WriteLine($"Current state: {messageString}");
                Console.WriteLine($"What do you want to do?");
                Console.WriteLine($"[0] - Release object");
                var index = 1;
                foreach (var objName in Objects)
                {
                    Console.WriteLine($"[{index}] - Pick '{objName}'");
                    index++;
                }
                if (!int.TryParse(Console.ReadLine(), out var intSelected) || intSelected>Objects.Count)
                    continue;

                _currentObject = intSelected == 0 ? string.Empty : Objects[intSelected - 1];
            }
        }

        private static Message GetStateMessage(out string messageString)
        {
            // Create JSON message
            var stateMessage = new
            {
                currentObject = _currentObject
            };
            messageString = JsonConvert.SerializeObject(stateMessage);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            return message;
        }
    }
}
