using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;

namespace IoTHub.Feature.SitecoreRules.Rules.Conditions
{
    public class CallDeviceMethodConditionDouble<T> : OperatorCondition<T> where T : RuleContext
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
            var parsedValue = (double)property.GetValue(parsedResults);

            // Compare values
            var comparisonResult = false;
            switch (this.GetOperator())
            {
                case ConditionOperator.Equal:
                    comparisonResult = parsedValue == Value;
                    break;
                case ConditionOperator.GreaterThanOrEqual:
                    comparisonResult = parsedValue >= Value;
                    break;
                case ConditionOperator.GreaterThan:
                    comparisonResult = parsedValue > Value;
                    break;
                case ConditionOperator.LessThanOrEqual:
                    comparisonResult = parsedValue <= Value;
                    break;
                case ConditionOperator.LessThan:
                    comparisonResult = parsedValue < Value;
                    break;
                case ConditionOperator.NotEqual:
                    comparisonResult = parsedValue != Value;
                    break;
                default:
                    comparisonResult = false;
                    break;
            }

            return comparisonResult;
        }

        public override bool CanEvaluate(T ruleContext)
        {
            return true;
        }

        public ID MethodId { get; set; }
        public string PropertyId { get; set; }
        public int Value { get; set; }
        public string Payload { get; set; }
    }
}