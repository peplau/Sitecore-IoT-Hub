namespace IoTDevices.Configuration
{
    public class Device
    {
        public Device(string name, string connectionString)
        {
            Name = name;
            ConnectionString = connectionString;
        }

        public string Name { get; set; }
        public string ConnectionString { get; set; }
    }
}
