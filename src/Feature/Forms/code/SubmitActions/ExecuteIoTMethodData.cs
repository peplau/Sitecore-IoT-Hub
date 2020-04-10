using System;

namespace IoTHub.Feature.Forms.SubmitActions
{
    public class ExecuteIoTMethodData
    {
        public Guid MethodId { get; set; }
        public Guid PayloadFieldId { get; set; }
        public string PayloadString { get; set; }
    }
}