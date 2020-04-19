using Microsoft.Azure.EventHubs;
using Sitecore.Data;
using Sitecore.Pipelines;

namespace IoTHub.Foundation.Azure.Pipelines
{
    public class MessageReceivedArgs : PipelineArgs
    {
        public EventData EventData { get; set; }
        public string Partition { get; set; }
        public EventHubClient EventHubClient { get; set; }
        public Database Database { get; set; }
    }
}