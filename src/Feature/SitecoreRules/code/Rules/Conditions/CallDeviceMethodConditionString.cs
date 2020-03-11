using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace IoTHub.Feature.SitecoreRules.Rules.Conditions
{
    public class CallDeviceMethodConditionString<T> : StringOperatorCondition<T> where T : RuleContext
    {
        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull(ruleContext, nameof (ruleContext));

            // Get passed method and property
            var methodItem = Sitecore.Context.Database.GetItem(MethodId);
            var propertyItem = Sitecore.Context.Database.GetItem(PropertyId);
            Assert.ArgumentNotNull(methodItem, $"methodItem.ID='{MethodId}'");
            Assert.ArgumentNotNull(propertyItem, $"propertyItem.ID='{PropertyId}'");
            var method = new IoTDeviceMethod(methodItem);
            var property = new IoTMessageProperty(propertyItem);
            
            // Call method and receive results
            var parsedResults = method.Invoke(Payload);
            var parsedValue = property.GetValue(parsedResults).ToString();

            // Compare values
            var comparisonResult = Compare(parsedValue, Value);
            return comparisonResult;
        }

        public override bool CanEvaluate(T ruleContext)
        {
            return true;
        }

        protected virtual string GetValue(T ruleContext)
        {
            return Value ?? string.Empty;
        }

        public ID MethodId { get; set; }
        public ID PropertyId { get; set; }
        public string Value { get; set; }
        public string Payload { get; set; }
    }
}