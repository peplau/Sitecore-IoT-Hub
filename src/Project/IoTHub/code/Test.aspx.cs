using System;
using System.Linq;
using System.Web.Mvc;
using IoTHub.Foundation.Azure.Repositories;

namespace IoTHub.Project.IoTHub
{
    public partial class Test : System.Web.UI.Page
    {
        private readonly IIoTHubRepository _ioTHubRepository = DependencyResolver.Current.GetService<IIoTHubRepository>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;

            FillHubs();
        }

        private void FillHubs()
        {
            var hubs = _ioTHubRepository.GetHubs().ToDictionary(k => k.ID.ToString(), v => v.HubName);
            ddlHubs.Items.Clear();
            ddlHubs.DataValueField = "Key";
            ddlHubs.DataTextField = "Value";
            ddlHubs.DataSource = hubs;
            ddlHubs.DataBind();
            ddlHubs.Items.Insert(0, "");
        }

        private void FillDevices()
        {
            var selectedHub = ddlHubs.SelectedValue;
            var hubModel = _ioTHubRepository.GetHub(selectedHub);          
            var devices = hubModel.GetDevices().ToDictionary(k => k.ID.ToString(), v => v.DeviceName);

            ddlDevices.Items.Clear();
            ddlDevices.DataValueField = "Key";
            ddlDevices.DataTextField = "Value";
            ddlDevices.DataSource = devices;
            ddlDevices.DataBind();
            ddlDevices.Items.Insert(0, "");
        }

        private void FillMethods()
        {
            var selectedDevice = ddlDevices.SelectedValue;
            var deviceModel = _ioTHubRepository.GetDevice(selectedDevice);          
            var methods = deviceModel.GetMethods().ToDictionary(k => k.ID.ToString(), v => v.MethodName);

            ddlMethods.Items.Clear();
            ddlMethods.DataValueField = "Key";
            ddlMethods.DataTextField = "Value";
            ddlMethods.DataSource = methods;
            ddlMethods.DataBind();
            ddlMethods.Items.Insert(0, "");
        }

        protected void btnTrigger_Click(object sender, EventArgs e)
        {
            var method = _ioTHubRepository.GetMethodByName(
                ddlHubs.SelectedItem.Text,
                ddlDevices.SelectedItem.Text,
                ddlMethods.SelectedItem.Text);

            if (method == null)
            {
                panResults.Visible = false;
                panError.Visible = true;
                litError.Text = "Method cannot be found";
                return;
            }

            dynamic result;
            try
            {
                result = method.Invoke(txtPayload.Text);
            }
            catch (Exception exception)
            {
                panResults.Visible = false;
                panError.Visible = true;
                litError.Text = $"Error invoking method: {exception.Message}";
                return;
            }
            if (result == null)
            {
                panResults.Visible = false;
                panError.Visible = true;
                litError.Text = "Error invoking method";
                return;
            }

            litRawMessage.Text = result.RawMessage;

            try
            {
                panError.Visible = false;
                panResults.Visible = true;
            }
            catch (Exception exception)
            {
                panError.Visible = true;
                litError.Text = exception.Message;
            }
        }

        protected void ddlHubs_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillDevices();
        }

        protected void ddlDevices_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillMethods();
        }
    }
}