<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="IoTHub.Project.IoTHub.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            IoT Hub:
            <asp:DropDownList runat="server" ID="ddlHubs" AutoPostBack="True" OnSelectedIndexChanged="ddlHubs_SelectedIndexChanged" />

            IoT Device:
            <asp:DropDownList runat="server" ID="ddlDevices" AutoPostBack="True" OnSelectedIndexChanged="ddlDevices_SelectedIndexChanged" />

            Method:
            <asp:DropDownList runat="server" ID="ddlMethods" />

            Payload:
            <asp:TextBox runat="server" ID="txtPayload" Width="100"></asp:TextBox>

            <asp:Button runat="server" ID="btnTrigger" Text="Read Sensors" OnClick="btnTrigger_Click" />
            <hr />

            <asp:Panel runat="server" ID="panResults" Visible="False">
                Raw Message:
                <pre style="background: lightblue;"><asp:Literal runat="server" ID="litRawMessage"></asp:Literal></pre>                
            </asp:Panel>

            <asp:Panel runat="server" ID="panError" Visible="False">
                <div style="background: red; color: white;">
                    <asp:Literal runat="server" ID="litError"></asp:Literal>
                </div>
            </asp:Panel>

        </div>
    </form>
</body>
</html>
