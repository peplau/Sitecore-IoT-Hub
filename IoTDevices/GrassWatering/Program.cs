using System;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.GrassWatering
{
    /// <summary>
    /// IoT Device - Virtual GrassWatering
    /// - Method: GetState() - Result: {enabled: true}
    /// - Method: SetEnabled() - Result: {enabled: true}
    /// </summary>
    class Program
    {
        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static bool _currentEnabledState;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Welcome to the Virtual Grass Watering");
            Console.WriteLine("-------------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run("GrassWatering");

            // Start up Hub client
            _sDeviceClient =
                DeviceClient.CreateFromConnectionString(_currentDevice.ConnectionString, TransportType.Mqtt);

            // Create a handler for the direct method calls
            _sDeviceClient.SetMethodHandlerAsync("GetState", GetState, null).Wait();
            _sDeviceClient.SetMethodHandlerAsync("SetEnabled", SetEnabled, null).Wait();

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
        /// IoT Method - SetEnabled(bool)
        /// </summary>
        /// <param name="methodRequest"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static Task<MethodResponse> SetEnabled(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);            
            if (!bool.TryParse(data, out bool stateToSet))
                stateToSet = false;
            _currentEnabledState = stateToSet;
            GetStateMessage(out var messageString);

            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method SetEnabled called - Payload: {data} - Result: {messageString}");
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

                if (!int.TryParse(Console.ReadLine(), out var intSelected))
                    intSelected = 0;

                switch (intSelected)
                {
                    case 1:
                        Console.Write($"Type a new On State: ");
                        if (!bool.TryParse(Console.ReadLine(), out var stateToSet))
                            stateToSet = false;
                        _currentEnabledState = stateToSet;
                        // Device-To-Cloud call us executed when the state changes
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
                enabled = _currentEnabledState
            };
            messageString = JsonConvert.SerializeObject(stateMessage);

            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            return message;
        }
    }
}