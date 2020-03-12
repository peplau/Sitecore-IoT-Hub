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

To use this module you will need a valid Azure Account, with the IoT Hub service installed and some adjustments in the Azure Cloud Shell

1. If you don't have yet, create an Azure account and access https://portal.azure.com/

2. Create a new Azure IoT Hub by following [this step-by-step](https://www.techrepublic.com/article/how-to-create-an-iot-hub-in-microsoft-azure/)

3. Open the Azure Cloud Shell - Select the Cloud Shell button on the menu bar at the upper right in the Azure portal.
    ![Open Azure Cloud Shell](https://docs.microsoft.com/en-us/azure/includes/media/cloud-shell-try-it/hdi-cloud-shell-menu.png "Open Azure Cloud Shell")

4. Run the following command to add the Microsoft Azure IoT Extension for Azure CLI to your Cloud Shell instance. 
    ```sh
    az extension add --name azure-iot
    ```


### 2 - Map IoT Hub in Sitecore

You have created your IoT Hub in Azure, now you need to map this Hub in Sitecore.

1. Create a new IoT Hub on Sitecore to map to your Azure IoT Hub under 
    > /sitecore/system/Modules/IoT Hub/Hubs
   
   Make sure the "Hub Name" field has the exact same name as your Azure IoT Hub

2. Configure the IoT Hub on Sitecore by filling the 4 fields as seen below:

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


### 3 - Register IoT Devices on Azure

Your IoT Hub can handle multiple Devices. Here you will register a new device in your IoT Hub.

* Run the following command in Azure Cloud Shell to create the device identity. 

  Make sure to replace {YourIoTHubName} and {MyDevice} with your correct information.

    ```sh
    az iot hub device-identity create --hub-name {YourIoTHubName} --device-id {MyDevice}
    ```


### 4 - Map IoT Devices in Sitecore

You device is now registered in Azure, and now you will map this device in Sitecore.

1. Create a new IoT Device on Sitecore to map to your Azure IoT Device under the IoT Hub that you previously created.

  Make sure the "Device Name" field has the exact same name as your Azure IoT Device

2. Configure the IoT Device on Sitecore by filling the Connection String:

    ![IoT Device Configured](images/IoT-Device-Config.jpg?raw=true "IoT Device Configured") 

    Run the following commands on Azure Cloud Shell to obtain the connectionstring:

    ```sh
    az iot hub device-identity show-connection-string --hub-name {YourIoTHubName} --device-id {MyDevice} --output table
    ```

    Make sure to replace {YourIoTHubName} and {MyDevice} with your correct information.


### 5 - Map a Method in your IoT Device 

Your devices can have methods exposed to be called by Sitecore (C2D - Cloud to Device direction). 

Here you will map in Sitecore the methods to be called in your device.

Take for instance our [virtual Thermometer device](/IoTDevices/Thermometer), it has the following Method:

    GetState()
    Parameters: none
    Return: {temperature: 31.15}

Below steps shows how to map this method in Sitecore.

1. Create a new Message Type to handle the return type under the path

    > /sitecore/system/Modules/IoT Hub/Message Types

    Because the result format is JSON, don't forget to select "Json Deserializer" as your Deserializer
    ![Thermometer GetState](images/Message-Type-GetState.jpg?raw=true "Thermometer GetState") 

2. Under the Message Type you just created, add propertie(s) to map to your method return type

    Eg: Thermometer only has the property "temperature"
    ![Property temperature](images/Message-Property.jpg?raw=true "Property temperature") 

3. Create a new IoT Device in Sitecore to map to your Azure IoT Device under the IoT Hub that you previously created.

    ![Method GetState in Sitecore](images/Create-Method.jpg?raw=true "Method GetState in Sitecore")

    ** Make sure the "Method Name" field has exactly the same name as your Device Method
    ** Make sure your Return Type points to the Message Type recently created


The following image shows how the Device Method "GetState()" is mapped in Sitecore:

    ![Sitecore-Method Mapping](images/Sitecore-Method-Mapping.jpg?raw=true "Sitecore-Method Mapping")
