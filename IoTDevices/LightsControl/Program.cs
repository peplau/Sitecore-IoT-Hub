using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using IoTDevices.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace IoTDevices.LightsControl
{
    class Program
    {
        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static readonly List<string> CurrentOnList = new List<string>();
        private static readonly List<string> Rooms = new List<string>
        {
            "bedroom_1", "bedroom_2", "bedroom_3",
            "garage", "living_room", "toilet_1",
            "toilet_2", "toilet_3", "dinner_room"
        };

        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Welcome to the Virtual Lights Control");
            Console.WriteLine("-------------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run();

            // Start up Hub client
            _sDeviceClient =
                DeviceClient.CreateFromConnectionString(_currentDevice.ConnectionString, TransportType.Mqtt);

            // Create a handler for the direct method calls
            _sDeviceClient.SetMethodHandlerAsync("GetState", GetState, null).Wait();
            _sDeviceClient.SetMethodHandlerAsync("SetRoomState", SetRoomState, null).Wait();

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

        private static Task<MethodResponse> SetRoomState(MethodRequest methodRequest, object userContext)
        {
            var data = Encoding.UTF8.GetString(methodRequest.Data);
            var deserialized = JsonConvert.DeserializeObject<dynamic>(data);

            var room = deserialized.room;
            if (Rooms.Contains(room))
            {
                var state = (bool)deserialized.state;
                if (state)
                {
                    // Turn lights on
                    if (!CurrentOnList.Contains(room))
                        CurrentOnList.Add(room);
                }
                else
                {
                    // Turn lights off
                    if (CurrentOnList.Contains(room))
                        CurrentOnList.Remove(room);
                }
            }

            GetStateMessage(out var messageString);
            Console.WriteLine(" ");
            Console.WriteLine($"[Cloud-to-Device] method SetRoomState called - Payload: {data} - Result: {messageString}");
            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
        }

        private static void CommandLoop()
        {
            while (true)
            {
                GetStateMessage(out string messageString);

                Console.WriteLine();
                Console.WriteLine($"Current state: {messageString}");
                Console.WriteLine($"Change: [1] Room State");

                if (!int.TryParse(Console.ReadLine(), out var intSelected))
                    intSelected = 0;

                switch (intSelected)
                {
                    case 1:
                        Console.WriteLine($"What room do you want to change state?");
                        var index = 0;
                        foreach (var room in Rooms)
                        {
                            Console.WriteLine($"[{index}] - {room}");
                            index++;
                        }
                        if (!int.TryParse(Console.ReadLine(), out var intParsed) || intParsed > Rooms.Count - 1)
                            continue;
                        Console.WriteLine($"What state you want to set? (true=lights on / false=lights off)");
                        if (!bool.TryParse(Console.ReadLine(), out var setState))
                            continue;
                        var selectedRoom = Rooms[intParsed];

                        if (setState)
                            if (!CurrentOnList.Contains(selectedRoom))
                                CurrentOnList.Add(selectedRoom);
                            else
                            if (CurrentOnList.Contains(selectedRoom))
                                CurrentOnList.Remove(selectedRoom);

                        break;
                }
            }
        }

        private static Message GetStateMessage(out string messageString)
        {
            // Create JSON message
            messageString = "{";
            foreach (var room in Rooms)
            {
                var onState = CurrentOnList.Contains(room) ? "true" : "false";
                messageString += $"\"{room}\":{onState},";
            }
            messageString = messageString.Substring(0, messageString.Length - 1);
            messageString += "}";

            var message = new Message(Encoding.ASCII.GetBytes(messageString));
            return message;
        }
    }
}
