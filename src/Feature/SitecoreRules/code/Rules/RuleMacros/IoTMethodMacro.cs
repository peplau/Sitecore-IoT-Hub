using System;
using System.Web.Mvc;
using System.Xml.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Framework.Conditions;
using Sitecore.Rules.RuleMacros;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace IoTHub.Feature.SitecoreRules.Rules.RuleMacros
{
    public class IoTMethodMacro : IRuleMacro
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

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
                SelectItemOptions.GetTemplateList(IoTDeviceMethod.TemplateID.ToString());

            // Get Device Item
            var deviceId = element.Attribute("DeviceId")?.Value;
            if (string.IsNullOrEmpty(deviceId)) {
                SheerResponse.Alert("Please select a valid IoT Device");
                return;
            }
            var deviceItem = Context.ContentDatabase.GetItem(deviceId);
            if (deviceItem == null) {
                SheerResponse.Alert("Please select a valid IoT Device");
                return;
            }
            var device = _ioTHubRepository.CastToDevice(deviceItem);

            // Get Device Type item
            var deviceTypeField = device.DeviceType;
            if (deviceTypeField?.TargetItem == null) {
                SheerResponse.Alert($"The IoT Device item {deviceItem.ID} has an invalid value in the 'Device Type' field");
                return;
            }
            var deviceType = _ioTHubRepository.CastToDeviceType(deviceTypeField.TargetItem);

            // Selected Item
            Item methodItem = null;
            if (!string.IsNullOrEmpty(value))
                methodItem = Context.ContentDatabase.GetItem(value);

            // Setup component state
            selectItemOptions.Root = deviceType.InnerItem;
            selectItemOptions.SelectedItem = methodItem;
            selectItemOptions.Title = "Select IoT Method";
            selectItemOptions.Text = "Select the IoT Method to use in this rule.";
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