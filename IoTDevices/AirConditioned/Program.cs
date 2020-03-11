using System;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.AirConditioned
{
    class Program
    {
        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static bool _currentOnState = false;
        private static int _currentTemperature = 24;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("--------------------------------------");
            Console.WriteLine("Welcome to the Virtual Air-conditioned");
            Console.WriteLine("--------------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run();

            // Start up Hub client
            _sDeviceClient =
                DeviceClient.CreateFromConnectionString(_currentDevice.ConnectionString, TransportType.Mqtt);

            // Create a handler for the direct method calls
            _sDeviceClient.SetMethodHandlerAsync("GetState", GetState, null).Wait();
            _sDeviceClient.SetMethodHandlerAsync("SetTemperature", SetTemperature, null).Wait();
            _sDeviceClient.SetMethodHandlerAsync("TurnOn", TurnOn, null).Wait();
            _sDeviceClient.SetMethodHandlerAsync("TurnOff", TurnOff, null).Wait();

            CommandLoop();
        }

        private static Task<MethodResponse> GetState(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method GetState called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        private static Task<MethodResponse> SetTemperature(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);            
            if (!int.TryParse(data, out var newTemp))
                newTemp = _currentTemperature;
            _currentTemperature = newTemp;
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method SetTemperature called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        private static Task<MethodResponse> TurnOn(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);            
            _currentOnState = true;
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method TurnOn called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        private static Task<MethodResponse> TurnOff(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);            
            _currentOnState = false;
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method TurnOff called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        private static void CommandLoop()
        {
            while (true)
            {
                GetStateMessage(out string messageString);

                Console.WriteLine();
                Console.WriteLine($"Current state: {messageString}");
                Console.WriteLine($"Change: [1] On State");
                Console.WriteLine($"Change: [2] Temperature");

                if (!int.TryParse(Console.ReadLine(), out var intSelected))
                    intSelected = 0;

                switch (intSelected)
                {
                    case 1:
                        Console.Write($"Type a new On State: ");
                        if (!bool.TryParse(Console.ReadLine(), out var boolVal))
                            boolVal = false;
                        _currentOnState = boolVal;
                        break;
                    case 2:
                        Console.Write($"Type a new Temperature: ");
                        if (!int.TryParse(Console.ReadLine(), out var intVal))
                            intVal = 0;
                        _currentTemperature = intVal;
                        break;
                }
            }
        }

        private static Message GetStateMessage(out string messageString)
        {
            // Create JSON message
            var stateMessage = new
            {
                enabled = _currentOnState,
                temperature = _currentTemperature
            };
            messageString = JsonConvert.SerializeObject(stateMessage);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            return message;
        }
    }
}