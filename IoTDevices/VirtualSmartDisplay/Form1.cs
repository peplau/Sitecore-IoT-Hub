using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IoTDevices.VirtualSmartDisplay.Configuration;
using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using Message = Microsoft.Azure.Devices.Client.Message;

namespace IoTDevices.VirtualSmartDisplay
{
    /// <summary>
    /// IoT Device - Virtual SmartDisplay
    /// - Method: GetState() - Result: {currentObject: "Tablet1"}
    /// </summary>
    public partial class Form1 : Form
    {
        public delegate void WriteLine(string line);

        private static Device _currentDevice;
        private static DeviceClient _sDeviceClient;
        private static string _currentObject = string.Empty;
        private static readonly List<string> Objects = new List<string>
        {
            "Tablet1", "Tablet2",
            "Mobile1", "Mobile2", "Mobile3",
            "Headphones1", "Headphones2", "Headphones3"
        };
        private readonly List<PictureBox> _pictures = new List<PictureBox>();
        private static Form1 _current;
        public Form1()
        {
            _current = this;
            InitializeComponent();
        }

        private static void RealWriteLine(string line)
        {
            _current.outputLog.AppendText(line + Environment.NewLine);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _pictures.Add(picTablet); _pictures.Add(picTablet2);
            _pictures.Add(picMobile1); _pictures.Add(picMobile2); _pictures.Add(picMobile3);
            _pictures.Add(picHeadphones1); _pictures.Add(picHeadphones2); _pictures.Add(picHeadphones3);

            picTablet.BackColor = picTablet2.BackColor =
                picMobile1.BackColor = picMobile2.BackColor = picMobile3.BackColor =
                    picHeadphones1.BackColor = picHeadphones2.BackColor = picHeadphones3.BackColor =
                        picSelected.BackColor = Color.Transparent;
            picTablet.Parent = picTablet2.Parent =
                picMobile1.Parent = picMobile2.Parent = picMobile3.Parent =
                    picHeadphones3.Parent = picHeadphones2.Parent = picHeadphones1.Parent =
                        picSelected.Parent = picBackground;

            RealWriteLine("------------------------------------");
            RealWriteLine("Welcome to the Virtual Smart Display");
            RealWriteLine("------------------------------------");

            // Selected Device
            _currentDevice = GetDeviceLoop.Run("SmartDisplay", RealWriteLine);

            // Start up Hub client
            _sDeviceClient =
                DeviceClient.CreateFromConnectionString(_currentDevice.ConnectionString, TransportType.Mqtt);

            // Output current state to log
            GetStateMessage(out string messageString);
            RealWriteLine(string.Empty);
            RealWriteLine($"Current state: {messageString}");

            SendDeviceToCloud(RealWriteLine);
            SetupIoTDevice(RealWriteLine);
        }

        private static async void SetupIoTDevice(WriteLine writeLine)
        {
            await Task.Run(() =>
            {
                // Create a handler for the direct method calls
                _sDeviceClient.SetMethodHandlerAsync("GetSelectedObject", GetSelectedObject, writeLine).Wait();
            });
        }

        /// <summary>
        /// IoT Method - GetSelectedObject()
        /// </summary>
        /// <param name="methodRequest"></param>
        /// <param name="userContext"></param>
        /// <returns></returns>
        private static Task<MethodResponse> GetSelectedObject(MethodRequest methodRequest, object userContext)
        {
            var writeLine = (WriteLine) userContext;

            var data = Encoding.UTF8.GetString(methodRequest.Data);
            GetStateMessage(out var messageString);
            
            writeLine(Environment.NewLine);
            writeLine(
                $"[Cloud-to-Device] method GetSelectedObject called - Payload: {data} - Result: {messageString}" +
                Environment.NewLine);

            return Task.FromResult(new MethodResponse(Encoding.UTF8.GetBytes(messageString), 200));
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

        private void PickObject(int selectedIndex)
        {
            // Scape if out of bounds
            if (selectedIndex > Objects.Count)
                return;

            // Unselect/Select item
            _currentObject = selectedIndex == -1 ? string.Empty : Objects[selectedIndex];

            // Send to cloud
            SendDeviceToCloud(RealWriteLine);
        }

        private async void SendDeviceToCloud(WriteLine writeLine)
        {
            GetStateMessage(out var messageString);
            var message = new Message(Encoding.ASCII.GetBytes(messageString));

            // Add references to what method this is
            message.Properties.Add("Method", $"{_currentDevice.Hub.Name}.{_currentDevice.Name}.GetState");

            // Send the telemetry message
            await _sDeviceClient.SendEventAsync(message);
            writeLine($"{DateTime.Now} > Sending message: {messageString}");
        }

        private void picBackground_Paint(object sender, PaintEventArgs e)
        {
            picTablet_Paint(sender, e); picTablet2_Paint(sender, e);
            picMobile1_Paint(sender, e); picMobile2_Paint(sender, e); picMobile3_Paint(sender, e);
            picHeadphones1_Paint(sender, e); picHeadphones2_Paint(sender, e); picHeadphones3_Paint(sender, e);
        }

        private void picObject_Click(object sender, EventArgs e)
        {
            // Show all pictures
            picTablet.Visible =
                picTablet2.Visible = picMobile1.Visible = picMobile2.Visible = picMobile3.Visible =
                    picHeadphones1.Visible = picHeadphones2.Visible = picHeadphones3.Visible = true;

            // Hide the one that was clicked
            var pictureClicked = (PictureBox)sender;
            pictureClicked.Visible = false;

            // Make the selected image appear at the monitor
            picSelected.BackgroundImage = pictureClicked.Image;
            picSelected.Visible = true;
            labSelected.Visible = !picSelected.Visible;
            labUnselect.Visible = !labSelected.Visible;

            // Modify Device state accordingly
            PickObject(_pictures.IndexOf(pictureClicked));
        }

        private void picSelected_Click(object sender, EventArgs e)
        {
            // Show all pictures
            picTablet.Visible =
                picTablet2.Visible = picMobile1.Visible = picMobile2.Visible = picMobile3.Visible =
                    picHeadphones1.Visible = picHeadphones2.Visible = picHeadphones3.Visible = true;
            // Hide selected image
            picSelected.Visible = false;
            labSelected.Visible = !picSelected.Visible;
            labUnselect.Visible = !labSelected.Visible;
            // Clean Device state
            PickObject(-1);
        }

        private void picHeadphones1_Paint(object sender, PaintEventArgs e) { }
        private void picHeadphones2_Paint(object sender, PaintEventArgs e) { }
        private void picHeadphones3_Paint(object sender, PaintEventArgs e) { }
        private void picTablet_Paint(object sender, PaintEventArgs e) { }
        private void picTablet2_Paint(object sender, PaintEventArgs e) { }
        private void picMobile1_Paint(object sender, PaintEventArgs e) { }
        private void picMobile2_Paint(object sender, PaintEventArgs e) { }
        private void picMobile3_Paint(object sender, PaintEventArgs e) { }
    }
}
