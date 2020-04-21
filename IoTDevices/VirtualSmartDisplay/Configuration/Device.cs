namespace IoTDevices.VirtualSmartDisplay.Configuration
{
    public class Device
    {
        public Device(string name, string connectionString, Hub hub)
        {
            Name = name;
            ConnectionString = connectionString;
            Hub = hub;
        }

        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public Hub Hub { get; set; }
    }
}
