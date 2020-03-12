# Documentation

## Installation

1. Download the latest [Installation .zip file](https://github.com/peplau/Sitecore-IoT-Hub/blob/master/sc.package)

2. Install using the Installation Wizard

3. In order to use our custom conditions for personalization, make sure to browse the item

    > /sitecore/system/Settings/Rules/Conditional Renderings/Tags/Default 

    then add the IoT Hub Tag to the Tags field, by moving it to the right part of the field:
    ![Conditional Rendering Tags](images/Conditional%20Rendering%20Tags%20field.jpg?raw=true "Conditional Rendering Tags") 

## Configuration

### 1 - Prepare Azure IoT Hub

* If you don't have yet, create an Azure account and access https://portal.azure.com/

* Create a new Azure IoT Hub by following [this step-by-step](https://www.techrepublic.com/article/how-to-create-an-iot-hub-in-microsoft-azure/)

* Open the Azure Cloud Shell - Select the Cloud Shell button on the menu bar at the upper right in the Azure portal.
    ![Open Azure Cloud Shell](https://docs.microsoft.com/en-us/azure/includes/media/cloud-shell-try-it/hdi-cloud-shell-menu.png "Open Azure Cloud Shell")

* Run the following command to add the Microsoft Azure IoT Extension for Azure CLI to your Cloud Shell instance. 
    ```sh
    az extension add --name azure-iot
    ```

### 2 - Map IoT Hub in Sitecore

* Create a new IoT Hub on Sitecore to map to your Azure IoT Hub under 
    > /sitecore/system/Modules/IoT Hub/Hubs
   
   Make sure the "Hub Name" field has the exact same name as your Azure IoT Hub

* Configure the IoT Hub on Sitecore by filling the 4 fields as seen below:

    ![IoT Hub Configured](images/IoT-Hub-Configuration.jpg?raw=true "IoT Hub Configured") 

    Run the following commands on Azure Cloud Shell to obtain each value:

    ##### Connection String: 
    ```sh
    az iot hub show-connection-string --policy-name service --name {YourIoTHubName} --output table
    ```

    ##### Event Hubs-compatible endpoint: 
    ```sh
    az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {YourIoTHubName}
    ```

    ##### Event Hubs-compatible path: 
    ```sh
    az iot hub show --query properties.eventHubEndpoints.events.path --name {YourIoTHubName}
    ```

    ##### Service Primary Key:
    ```sh
    az iot hub policy show --name service --query primaryKey --hub-name {YourIoTHubName}
    ```

### 3 - Create IoT Devices on Azure

* Run the following command in Azure Cloud Shell to create the device identity. 

  Make sure to replace {YourIoTHubName} and {MyDevice} with your correct information.

    ```sh
    az iot hub device-identity create --hub-name {YourIoTHubName} --device-id {MyDevice}
    ```

### 4 - Map IoT Devices in Sitecore

* Create a new IoT Device on Sitecore to map to your Azure IoT Device under the IoT Hub that you previously created.

  Make sure the "Device Name" field has the exact same name as your Azure IoT Device

* Configure the IoT Device on Sitecore by filling the Connection String:

    ![IoT Device Configured](images/IoT-Device-Config.jpg?raw=true "IoT Device Configured") 

    Run the following commands on Azure Cloud Shell to obtain the connectionstring:

    ```sh
    az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id {MyDevice} --output table
    ```

    Make sure to replace {YourIoTHubName} and {MyDevice} with your correct information.
