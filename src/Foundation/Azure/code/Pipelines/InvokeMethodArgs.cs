using IoTHub.Foundation.Azure.Deserializers;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Pipelines;

namespace IoTHub.Foundation.Azure.Pipelines
{
    public class InvokeMethodArgs : PipelineArgs
    {
        public IoTDevice Device { get; set; }
        public IoTDeviceMethod Method { get; set; }
        public string Payload { get; set; }
        public DynamicMessage Response { get; set; }
    }
}