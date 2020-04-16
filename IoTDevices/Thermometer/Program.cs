using System;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.Thermometer
{
    /// <summary>
    /// IoT Device - Virtual Thermometer
    /// - Method: GetState() - Result: {temperature: 31.15}
    /// </summary>
    class Program
    {
        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static double? _currentTemperature;
        private static double _minTemperature = 20;

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("----------------------------------");
            Console.WriteLine("Welcome to the Virtual Thermometer");
            Console.WriteLine("----------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run("Thermometer");

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
                Console.WriteLine($"Change: [1] Temperature");

                if (!int.TryParse(Console.ReadLine(), out var intSelected))
                    intSelected = 0;

                switch (intSelected)
                {
                    case 1:
                        Console.Write($"Type a new Temperature: ");
                        if (!double.TryParse(Console.ReadLine(), out var doubleTemp))
                            doubleTemp = 0;
                        _currentTemperature = doubleTemp;
                        // Device-To-Cloud call us executed when the state changes
                        SendDeviceToMethod();
                        break;
                }
            }
        }

        private static async void SendDeviceToMethod()
        {
            GetStateMessage(out var messageString);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            // Add references to what method this is
            message.Properties.Add("Method", $"{_currentDevice.Hub.Name}.{_currentDevice.Name}.GetState");

            // Send the telemetry message
            await _sDeviceClient.SendEventAsync(message);
            Console.WriteLine("{0} > Sending message: {1}", DateTime.Now, messageString);
        }

        public static double ReadTemperature()
        {
            // Return current value, if any
            if (_currentTemperature.HasValue) 
                return _currentTemperature.Value;

            // Create a random entry if not
            var rand = new Random();
            _currentTemperature = _minTemperature + rand.NextDouble() * 15;

            // When it first loads it will also send Device-to-Cloud message
            SendDeviceToMethod();

            return _currentTemperature.Value;
        }

        private static Message GetStateMessage(out string messageString)
        {
            var currentTemperature = ReadTemperature();

            // Create JSON message
            var stateMessage = new
            {
                temperature = currentTemperature
            };
            messageString = JsonConvert.SerializeObject(stateMessage);

            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            return message;
        }
    }
}
