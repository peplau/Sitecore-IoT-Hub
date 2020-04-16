using System;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.Hygrometer
{
    /// <summary>
    /// IoT Device - Virtual Hygrometer
    /// - Method: GetState() - Result: {humidity: 31.15}
    /// </summary>
    class Program
    {
        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static double? _currentHumidity;
        private static double _minHumidity = 60;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("---------------------------------");
            Console.WriteLine("Welcome to the Virtual Hygrometer");
            Console.WriteLine("---------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run("Hygrometer");

            // Start up Hub client
            _sDeviceClient =
                DeviceClient.CreateFromConnectionString(_currentDevice.ConnectionString, TransportType.Mqtt);

            // Create a handler for the direct method calls
            _sDeviceClient.SetMethodHandlerAsync("GetState", GetState, null).Wait();

            CommandLoop();
        }

        /// <summary>
        /// IoT Method - GetState (no parameter)
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

        private static void CommandLoop()
        {
            while (true)
            {
                GetStateMessage(out string messageString);

                Console.WriteLine();
                Console.WriteLine($"Current state: {messageString}");
                Console.WriteLine($"Change: [1] Humidity");

                if (!int.TryParse(Console.ReadLine(), out var intSelected))
                    intSelected = 0;

                switch (intSelected)
                {
                    case 1:
                        Console.Write($"Type a new Humidity: ");
                        if (!double.TryParse(Console.ReadLine(), out var doubleVal))
                            doubleVal = 0;
                        _currentHumidity = doubleVal;

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

        public static double ReadHumidity()
        {
            // Return current value, if any
            if (_currentHumidity.HasValue) 
                return _currentHumidity.Value;

            // Create a random entry if not
            var rand = new Random();
            _currentHumidity = _minHumidity + rand.NextDouble() * 20;
            return _currentHumidity.Value;
        }

        private static Message GetStateMessage(out string messageString)
        {
            var currentHumidity = ReadHumidity();

            // Create JSON message
            var stateMessage = new
            {
                humidity = currentHumidity
            };
            messageString = JsonConvert.SerializeObject(stateMessage);

            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            return message;
        }
    }
}