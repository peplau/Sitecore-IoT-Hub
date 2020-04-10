(function (speak) {
    var parentApp = window.parent.Sitecore.Speak.app.findApplication('EditActionSubAppRenderer');
    var FormDesignBorder = window.parent.Sitecore.Speak.app.findComponent('FormDesignBoard');

    var iotDeviceMethodTemplateId = "{B96DDC3F-541D-428A-A68B-B0A0B34B1192}";
    var parameterNames = {
        methodId: "methodId",
        payloadField: "payloadFieldId",
        payload: "payloadString"
    };

    speak.pageCode(["underscore", "/-/speak/v1/formsbuilder/layouts/actions/formfieldsutils.js"],
        function (_, formFieldsUtils) {
            return {
                initialized: function () {
                    this.on({
                        "loaded": this.loadDone
                    }, this);

                    this.ItemTreeView.on("change:SelectedItem", this.changedSelectedItemId, this);

                    this.Fields = formFieldsUtils.getInputFields(FormDesignBorder);
                    this.PayloadField = this.getFormComponentByBindingName(parameterNames.payloadField);
                    this.PayloadString = this.getFormComponentByBindingName(parameterNames.payload);

                    if (parentApp) {
                        parentApp.loadDone(this, this.HeaderTitle.Text, this.HeaderSubtitle.Text);
                    }
                },
				
                getFormComponentByBindingName: function(parameterName) {
                    var componentName = this.Form.bindingConfigObject[parameterName].split(".")[0];
                    return this.Form[componentName];
                },			

                changedSelectedItemId: function () {
                    var isSelectable = this.ItemTreeView.SelectedItem.$templateId === iotDeviceMethodTemplateId;
                    parentApp.setSelectability(this, isSelectable, this.ItemTreeView.SelectedItemId);
                },

                loadDone: function (parameters) {
                    this.Parameters = parameters || {};
                    this.ItemTreeView.SelectedItemId = this.Parameters[parameterNames.methodId];					
                    this.Form.setFormData(this.Parameters);
                    this.setDynamicData(parameterNames.payloadField, this.Fields);
                },
				
                setDynamicData: function (formPropKey, items) {
                    var component = this.getFormComponentByBindingName(formPropKey);
                    var selectedItemId = this.Parameters[formPropKey];
                    items = items.slice(0);
                    items.unshift({ Name: "", Id: "" });
					
                    if (selectedItemId &&
                        !_.find(items, function (item) { return item.Id === selectedItemId })) {
                        items.splice(1, 0, {
                            Id: selectedItemId,
                            Name: selectedItemId +
                                " - " +
                                (this.ValueNotInListText.Text || "value not in the selection list")
                        });

                        component.reset(items);
                        $(component.el).find('option').eq(1).css("font-style", "italic");
                    } else {
                        component.reset(items);
                        component.SelectedValue = selectedItemId;
                    }
                },				

                getData: function () {
                    this.Parameters[parameterNames.methodId] = this.ItemTreeView.SelectedItemId;					
                    this.Parameters[parameterNames.payloadField] = this.PayloadField.SelectedValue;
                    this.Parameters[parameterNames.payload] = this.PayloadString.Value;					
                    return this.Parameters;
                },
				
                getDescription: function () {
                    return this.ItemTreeView.SelectedItem.$displayName;
                }
            };
        });
})(Sitecore.Speak);
