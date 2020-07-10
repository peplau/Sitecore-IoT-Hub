using System;
using System.Web.Mvc;
using System.Xml.Linq;
using IoTHub.Foundation.Azure.Models.Templates;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Framework.Conditions;
using Sitecore.Rules.RuleMacros;
using Sitecore.Shell.Applications.Dialogs.ItemLister;
using Sitecore.Text;
using Sitecore.Web.UI.Sheer;

namespace IoTHub.Feature.SitecoreRules.Rules.RuleMacros
{
    public class IoTDeviceMacro : IRuleMacro
    {
        private readonly IIoTHubRepository _ioTHubRepository =
            DependencyResolver.Current.GetService<IIoTHubRepository>();

        private const string DeviceRootId = "{62BA65C8-7E6C-4BD5-AA1F-E6EC9E85A26A}";

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
                SelectItemOptions.GetTemplateList(IoTDevice.TemplateID.ToString());

            // Selected Item
            Item deviceItem = null;
            if (!string.IsNullOrEmpty(value))
                deviceItem = Context.ContentDatabase.GetItem(value);

            // Setup component state
            selectItemOptions.Root = Context.ContentDatabase.GetItem(DeviceRootId);
            selectItemOptions.SelectedItem = deviceItem;
            selectItemOptions.Title = "Select IoT Device";
            selectItemOptions.Text = "Select the IoT Device to use in this rule.";
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