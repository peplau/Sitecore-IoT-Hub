using System;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.AirConditioned
{
    /// <summary>
    /// IoT Device - Virtual AirConditioned
    /// - Method: GetState() - Result: {enabled: true, temperature: 28}
    /// - Method: SetTemperature() - Result: {enabled: true, temperature: 28}
    /// - Method: TurnOn() - Result: {enabled: true, temperature: 28}
    /// - Method: TurnOff() - Result: {enabled: true, temperature: 28}
    /// </summary>
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
            _currentDevice = GetDeviceLoop.Run("AirConditioned");

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

        /// <summary>
        /// IoT Method - GetState()
        /// </summary>
        /// <param name="methodRequest"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static Task<MethodResponse> GetState(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method GetState called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        /// <summary>
        /// IoT Method - SetTemperature(int)
        /// </summary>
        /// <param name="methodRequest"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static Task<MethodResponse> SetTemperature(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            try { _currentTemperature = JsonConvert.DeserializeObject<int>(data); } catch(Exception){}
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method SetTemperature called - Payload: {data} - Result: {messageString}");
            // Device-To-Cloud call is executed when the state changes
            SendDeviceToCloud();
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        /// <summary>
        /// IoT Method - TurnOn()
        /// </summary>
        /// <param name="methodRequest"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static Task<MethodResponse> TurnOn(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            _currentOnState = true;
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method TurnOn called - Payload: {data} - Result: {messageString}");
            // Device-To-Cloud call is executed when the state changes
            SendDeviceToCloud();
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        /// <summary>
        /// IoT Method - TurnOff()
        /// </summary>
        /// <param name="methodRequest"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static Task<MethodResponse> TurnOff(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);            
            _currentOnState = false;
            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method TurnOff called - Payload: {data} - Result: {messageString}");
            // Device-To-Cloud call is executed when the state changes
            SendDeviceToCloud();
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
                        // Device-To-Cloud call is executed when the state changes
                        SendDeviceToCloud();
                        break;
                    case 2:
                        Console.Write($"Type a new Temperature: ");
                        if (!int.TryParse(Console.ReadLine(), out var intVal))
                            intVal = 0;
                        _currentTemperature = intVal;
                        // Device-To-Cloud call is executed when the state changes
                        SendDeviceToCloud();
                        break;
                }
            }
        }

        private static async void SendDeviceToCloud()
        {
            GetStateMessage(out var messageString);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            // Add references to what method this is
            message.Properties.Add("Method", $"{_currentDevice.Hub.Name}.{_currentDevice.Name}.GetState");

            // Send the telemetry message
            await _sDeviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
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