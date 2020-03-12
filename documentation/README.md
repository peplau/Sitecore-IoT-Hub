# Documentation

## Installation

1. Download the latest [Installation .zip file](https://github.com/peplau/Sitecore-IoT-Hub/blob/master/sc.package)

2. Install using the Installation Wizard

3. In order to use our custom conditions for personalization, make sure to browse the item

> /sitecore/system/Settings/Rules/Conditional Renderings/Tags/Default 

then add the IoT Hub Tag to the Tags field, by moving it to the right part of the field:

    ![Conditional Rendering Tags](https://github.com/peplau/Sitecore-IoT-Hub/blob/master/documentation/images/Conditional%20Rendering%20Tags%20field.jpg?raw=true "Conditional Rendering Tags") 


## Configuration

1. If you don't have yet, create an Azure account and access https://portal.azure.com/

2. Create a new Azure IoT Hub by following [this step-by-step](https://www.techrepublic.com/article/how-to-create-an-iot-hub-in-microsoft-azure/) (this step-by-step)

3. Open the Azure Cloud Shell - Select the Cloud Shell button on the menu bar at the upper right in the Azure portal.
    ![Open Azure Cloud Shell](https://docs.microsoft.com/en-us/azure/includes/media/cloud-shell-try-it/hdi-cloud-shell-menu.png "Open Azure Cloud Shell")

4. Run the following command to add the Microsoft Azure IoT Extension for Azure CLI to your Cloud Shell instance. 

```sh
az extension add --name azure-iot
```

5. Create a new IoT Hub on Sitecore to map to your Azure IoT Hub under /sitecore/system/Modules/IoT Hub/Hubs
   Make sure the "Hub Name" field has the exact same name of your Azure IoT Hub

6. Configure the IoT Hub on Sitecore by filling the 4 fields as seen below:

    ![IoT Hub Configured](images/IoT-Hub-Configuration.jpg?raw=true "IoT Hub Configured") 

    Run the following commands on Azure Cloud Shell to obtain each value:

    * Connection String: 
    ```sh
    az iot hub show-connection-string --policy-name service --name {YourIoTHubName} --output table
    ```

    * Event Hubs-compatible endpoint: 
    ```sh
    az iot hub show --query properties.eventHubEndpoints.events.endpoint --name {YourIoTHubName}
    ```

    * Event Hubs-compatible path: 
    ```sh
    az iot hub show --query properties.eventHubEndpoints.events.path --name {YourIoTHubName}
    ```

    * Service Primary Key:
    ```sh
    az iot hub policy show --name service --query primaryKey --hub-name {YourIoTHubName}
    ```

7. Create your device