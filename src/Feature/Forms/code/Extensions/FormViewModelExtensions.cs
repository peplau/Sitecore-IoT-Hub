using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace IoTHub.Feature.Forms.Extensions
{
    public static class FormViewModelExtensions
    {
        public static string GetValue(this IViewModel helper)
        {
            if (helper == null)
                return string.Empty;

            if (helper is InputViewModel<bool> boolValue)
                return boolValue.Value ? "1" : "0";

            if (helper is ListViewModel listView)
                return helper.GetListViewValue();

            if (helper is InputViewModel<string> stringValue)
                return stringValue.Value;

            if (helper is InputViewModel<DateTime> dateTimeValue)
                return dateTimeValue.Value.ToString(CultureInfo.InvariantCulture);

            if (helper is InputViewModel<double?> doubleValue)
                return doubleValue.Value?.ToString();

            return string.Empty;
        }

        public static string GetListViewValue(this IViewModel helper)
        {
            if (!(helper is ListViewModel listField)) return string.Empty;
            var selectedValue = listField.Items.SingleOrDefault(s => s.Selected);
            return selectedValue?.Value;
        }

        public static string GetListViewText(this IViewModel helper)
        {
            if (!(helper is ListViewModel listField)) return string.Empty;
            var selectedValue = listField.Items.SingleOrDefault(s => s.Selected);
            return selectedValue?.Text;
        }

        public static IViewModel ById(this IEnumerable<IViewModel> fields, Guid? id)
        {
            return id == null ? null : fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == id.Value);
        }
    }
}