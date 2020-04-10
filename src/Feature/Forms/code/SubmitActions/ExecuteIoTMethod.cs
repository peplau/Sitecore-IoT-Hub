using System;
using System.Web.Mvc;
using IoTHub.Feature.Forms.Extensions;
using IoTHub.Foundation.Azure.Repositories;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;

namespace IoTHub.Feature.Forms.SubmitActions
{
    /// <summary>
    /// Submit Action: Execute IoT Method
    /// </summary>
    public class ExecuteIoTMethod : SubmitActionBase<ExecuteIoTMethodData>
    {
        private readonly IIoTHubRepository _hubRepository = DependencyResolver.Current.GetService<IIoTHubRepository>();

        public ExecuteIoTMethod(ISubmitActionData submitActionData) : base(submitActionData) { }
        
        protected override bool Execute(ExecuteIoTMethodData data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(data, nameof (data));
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

            // Method not selected
            if (data.MethodId == Guid.Empty)
            {
                Log.Error($"[ExecuteIoTMethod] Form Submit Action failed - IoTMethod is not selected", this);
                return false;
            }

            // Payload
            var payload = string.Empty;
            if (data.PayloadFieldId.HasValue && data.PayloadFieldId.Value != Guid.Empty)
                payload = GetPayloadFromFieldId(formSubmitContext, data.PayloadFieldId.Value);
            else if (!string.IsNullOrEmpty(data.PayloadString))
                payload = GetPayloadFromString(formSubmitContext, data.PayloadString);

            // Call IoTMethod
            try
            {
                var method = _hubRepository.GetMethod(new ID(data.MethodId));
                if (method == null) {
                    Log.Error($"[ExecuteIoTMethod] Form Submit Action failed - IoTMethod '{data.MethodId}' does not exists", this);
                    return false;
                }
                var response = method.Invoke(payload);
                Log.Info(
                    $"[ExecuteIoTMethod] Called IoTMethod '{data.MethodId}' with payload '{payload}' - Return: '{response.RawMessage}'",
                    this);
            }
            catch (Exception e)
            {
                Log.Error($"[ExecuteIoTMethod] Form Submit Action failed - Error calling IoTMethod '{data.MethodId}'", e,
                    this);
                return false;
            }

            return true;
        }

        private string GetPayloadFromFieldId(FormSubmitContext formSubmitContext, Guid dataPayloadFieldId)
        {
            var field = formSubmitContext.Fields.ById(dataPayloadFieldId);
            return field == null ? string.Empty : field.GetValue();
        }

        private string GetPayloadFromString(FormSubmitContext formSubmitContext, string dataPayloadString)
        {
            foreach (var field in formSubmitContext.Fields)
            {
                var fieldName = field.Name;
                var fieldValue = field.GetValue();
                dataPayloadString = dataPayloadString.Replace($"[{fieldName}]", fieldValue);
            }
            return dataPayloadString;
        }
    }
}