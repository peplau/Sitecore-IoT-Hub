using System;
using System.Xml.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Framework.Conditions;
using Sitecore.Rules.RuleMacros;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace IoTHub.Feature.SitecoreRules.Rules.RuleMacros
{
    public class IoTPropertyMacro : IRuleMacro
    {
        public void Execute(XElement element, string name, UrlString parameters, string value)
        {
            // Param validations
            Condition.Requires(element, nameof(element)).IsNotNull();
            Condition.Requires(name, nameof(name)).IsNotNull();
            Condition.Requires(parameters, nameof(parameters)).IsNotNull();
            Condition.Requires(value, nameof(value)).IsNotNull();

            var selectItemOptions = new SelectItemOptions();

            // Rule Condition item
            var path = XElement.Parse(element.ToString()).FirstAttribute.Value;
            if (!string.IsNullOrEmpty(path))
            {
                var ruleConditionItem = Context.ContentDatabase.GetItem(path);
                if (ruleConditionItem != null)
                    selectItemOptions.FilterItem = ruleConditionItem;
            }

            // Filter templates
            selectItemOptions.ShowRoot = false;
            selectItemOptions.IncludeTemplatesForSelection =
                SelectItemOptions.GetTemplateList(IoTMessageProperty.TemplateID.ToString());

            // Get MethodItem
            var methodId = element.Attribute("MethodId")?.Value;
            if (string.IsNullOrEmpty(methodId)) {
                SheerResponse.Alert("Please select a valid IoT Method");
                return;
            }
            var methodItem = Context.ContentDatabase.GetItem(methodId);
            if (methodItem == null) {
                SheerResponse.Alert("Please select a valid IoT Method");
                return;
            }
            var method = new IoTDeviceMethod(methodItem);

            // Get Return Type item            
            var returnTypeField = method.ReturnType;
            if (returnTypeField?.TargetItem == null) {
                SheerResponse.Alert($"The IoT Method item {methodItem.ID} has an invalid value in the 'Return Type' field");
                return;
            }
            var returnType = new IoTMessageType(returnTypeField.TargetItem);

            // Selected Item
            Item propertyItem = null;
            if (!string.IsNullOrEmpty(value))
                propertyItem = Context.ContentDatabase.GetItem(value);

            // Setup component state
            selectItemOptions.Root = returnType.InnerItem;
            selectItemOptions.SelectedItem = propertyItem;
            selectItemOptions.Title = "Select Property";
            selectItemOptions.Text = "Select the property to use in this rule.";
            selectItemOptions.Icon = "/~/icon/office/32x32/password_field.png";

            var parameter = parameters["resulttype"];
            if (!string.IsNullOrEmpty(parameter))
                selectItemOptions.ResultType =
                    (SelectItemOptions.DialogResultType)Enum.Parse(typeof(SelectItemOptions.DialogResultType),
                        parameter);

            SheerResponse.ShowModalDialog(selectItemOptions.ToUrlString().ToString(), 
                "1200px", "700px", string.Empty, true);
        }
    }
}