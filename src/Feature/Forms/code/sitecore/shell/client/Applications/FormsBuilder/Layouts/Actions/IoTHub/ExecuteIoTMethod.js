(function (speak) {

    var parentApp;
    var formDesignBorder;
    if (window.parent.Sitecore.Speak.app) {
        parentApp = window.parent.Sitecore.Speak.app.findApplication('EditActionSubAppRenderer');
        formDesignBorder = window.parent.Sitecore.Speak.app.findComponent('FormDesignBoard');
    }

    var iotDeviceTemplateId = "{534E5B10-8659-4576-9C69-AA47FEAA533C}";
    var methodAutoLoad = "";

    var parameterNames = {
        deviceId: "deviceId",
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

                    this.ItemTreeView.on("change:SelectedItem", this.changedSelectedDeviceId, this);
                    this.MethodList.on("change:SelectedItem", this.changedSelectedMethodId, this);

                    if (formDesignBorder) {
                        this.Fields = formFieldsUtils.getInputFields(formDesignBorder);
                    }
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

                setSelectable: function() {
                    var isSelectable = this.ItemTreeView.SelectedItem.$templateId === iotDeviceTemplateId;
                    isSelectable = isSelectable && this.MethodList.SelectedValue !== undefined && this.MethodList.SelectedValue !== "";
                    if (parentApp) {
                        parentApp.setSelectability(this, isSelectable, this.ItemTreeView.SelectedItemId);
                    }
                },

                changedSelectedDeviceId: function () {
                    this.setSelectable();
                    var selectedDevice = this.ItemTreeView.SelectedItem;
                    var self = this;
                    self.MethodsDataSource.Items = [];
                    if (this.ItemTreeView.SelectedItem.$templateId === iotDeviceTemplateId)
                        $.get("/api/sitecore/FormsIoT/GetMethods?deviceId=" + selectedDevice.$itemId, function (result) {						
						    // Load dropdown items
                            result.unshift({Key:'',Value:''});
                            self.MethodsDataSource.Items = result;
                            // Auto-select
                            if (methodAutoLoad !== "") {
                                self.MethodList.SelectedValue = methodAutoLoad;
                                methodAutoLoad = "";
                            }

                        }, "json");
                },

                changedSelectedMethodId: function () {
                    this.setSelectable();
                },

                loadDone: function (parameters) {
                    this.Parameters = parameters || {};
                    this.ItemTreeView.SelectedItemId = this.Parameters[parameterNames.deviceId];
                    methodAutoLoad = this.Parameters[parameterNames.methodId];
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
                    this.Parameters[parameterNames.deviceId] = this.ItemTreeView.SelectedItemId;
                    this.Parameters[parameterNames.methodId] = this.MethodList.SelectedValue;
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
