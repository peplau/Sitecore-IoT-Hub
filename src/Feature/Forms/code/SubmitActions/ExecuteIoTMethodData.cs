using System;

namespace IoTHub.Feature.Forms.SubmitActions
{
    public class ExecuteIoTMethodData
    {
        public Guid DeviceId { get; set; }
        public Guid MethodId { get; set; }
        public Guid? PayloadFieldId { get; set; }
        public string PayloadString { get; set; }
    }
}