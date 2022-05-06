# Mattermost Outlook Presence Provider

## Table of Contents

- [Overview](#overview)
- [Installation](#installation)
- [Development](#development)

## Overview

This app acts as an intermediary between Mattermost and Microsoft Outlook by getting users' status updates from Mattermost and using those statuses to display users' presence information in Outlook. It is created by following the official Microsoft documentation which can be found [here](https://docs.microsoft.com/en-us/office/client-developer/shared/integrating-im-applications-with-office).
It is a Windows application which can be installed through a wizard setup.

## Installation

You can download the latest setup from the [releases page](https://github.com/Brightscout/mattermost-outlook-presence-provider/releases). Extract the zip file after the download. Install the application by running the file `setup.msi` and following the instructions in the wizard. After the installation is complete, you have to run the app by going to the install location or by searching for its name in the Windows Start menu. The executable file that you have to run is `MattermostPresenceProvider.exe`. You need to do the below configurations before running the app:

### Configuration

There's a `config.json` file present in the extracted zip folder which contains the configurations for the application. You can either configure it before installation or after installation. If you are configuring it after installation, then you'll have to modify the `config.json` file which is present in the install directory. The default install directory is "C:\Program Files\Mattermost\MattermostPresenceProvider\" but it can be changed during the installation. The changes done in the `config.json` file present in the extracted zip folder will not affect the installed app. The following config settings need to be configured:

- **MattermostServerURL**: The URL of the Mattermost server from which status updates need to be fetched
- **MattermostSecret**: The webhook secret generated on the plugin settings page of the Mattermost plugin "Outlook Presence Provider".
![image](https://user-images.githubusercontent.com/77336594/165111112-7c976991-7f79-4fdb-8479-801827bdcd23.png)
- **MattermostWebsocketReconnectionTimeoutInSeconds**: The websocket client used in this app waits for messages from the server and if there is no message from the server in a certain interval, it disconnects and attempts to reconnect to the server. This config setting is for that certain interval. Its default value is 30 seconds but can be changed according to the needs.

### Running the application

After the configuration, you can successfully execute the app by running the exe file. Go to the installed location of the app and run the `MattermostPresenceProvider.exe`. After running it, check if its running by opening the Task Manager and looking for "MattermostPresenceProvider" or you can also install additional applications to get more details about the processes running in your system like Microsoft's [ProcessExplorer](https://docs.microsoft.com/en-us/sysinternals/downloads/process-explorer).
If you see the app running, run Outlook and see the users' presence information in the dots on their profile pictures or in the popovers which appear after hovering over them.
![ss](https://user-images.githubusercontent.com/77336594/165121046-354cab06-4ad5-4e51-9895-f28b347f12c7.png)

**Note**: This application assumes that Skype for Business or Microsoft Teams is not installed in the user's system. If they are installed in the system, then this application may not work as expected.

## Development

### Setup

- Install [Visual Studio](https://visualstudio.microsoft.com/) with the following workloads:
    - .NET desktop development
    - Universal Windows Platform Development
    - Office/SharePoint development
- Install the extension [Visual Stuido Installer Projects](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2017InstallerProjects) by Microsoft. This will be needed to add and manage setup projects in Visual Studio.
- Install the latest [Windows SDK](https://developer.microsoft.com/en-gb/windows/downloads/sdk-archive/). The reason for installing Windows SDK is that it comes with the latest and original tools that we need. One of them is [OleViewer](https://docs.microsoft.com/en-us/windows/win32/com/ole-com-object-viewer) which is very useful when working with COM. Another one is [COMView](https://www.softpedia.com/get/System/System-Info/COMView.shtml). You can install either of them.

### Basic Knowledge

- [What is COM?](https://docs.microsoft.com/en-us/windows/win32/com/the-component-object-model)
- [How to integrate an application with MS Office](https://docs.microsoft.com/en-us/office/client-developer/shared/integrating-im-applications-with-office)
- [How to run the integration without Skype for Business installed](https://docs.microsoft.com/en-us/answers/questions/701146/how-to-integrate-im-application-with-office-withou.html)

The links above don't contain all the reference required to completely understand this project but they are enough to give the required context of the project.
More useful links:
- https://docs.microsoft.com/en-us/samples/dotnet/samples/out-of-process-com-server/
- https://social.msdn.microsoft.com/Forums/en-US/da42d864-36a9-4bb3-b754-499380ea5988/prerequisites-in-office-for-im-application-integration?forum=outlookdev
- https://social.msdn.microsoft.com/Forums/windows/en-US/9bb0535e-f330-42c2-abc3-f6517e4a7e4e/sample-outlook-presence-provider-app?forum=outlookdev
- https://docs.microsoft.com/en-us/answers/questions/771004/which-member-in-the-contactinformationtype-is-used.html

### Structure

The basic project structure was taken from a sample app provided in the Microsoft Community [here](https://social.msdn.microsoft.com/Forums/windows/en-US/9bb0535e-f330-42c2-abc3-f6517e4a7e4e/sample-outlook-presence-provider-app?forum=outlookdev). This project contains four separate projects embedded in one [solution file](./CSExeCOMServer.sln).

- **OutOfProcessCOMBase**: This project contains the Native code to run a COM server which was officially provided by Microsoft as a sample with the name "CSExeCOMServer" but later it was removed from the official Microsoft Github and was replaced by a [new sample](https://github.com/dotnet/samples/tree/main/core/extensions). We did not change anything in this project. This project compiles to a dll file which is used by the other projects.
- **PresenceProvider**: This is the primary project in which we have created classes that implement the interfaces exposed by the `UCCollaborationLib` namespace. This project contains all the logic to fetch and subscribe to users' status updates from Mattermost.
- **CSExeCOMServer**: This project creates a Windows application (exe file) to run and it contains the other two projects as dependencies. Building this project automatically builds the other two as well. We can say that this is the startup project for out application. It contains the logic to register the [Unified Collaborations API type library](./DLL/UCCollaborationLib.tlb) that is usually registered by Skype for Business/Microsoft Teams.
- **MattermostPresenceProvider**: This is just a setup project which creates a `setup.msi` file by building the **CSExceCOMServer** project.

Building the solution builds all these four projects. You can see how to build the solution in the CI [yml file](./.github/workflows/release.yml).

### Logging

This project implements logging by creating a separate event log in the Windows Event Viewer. You can open the Event Viewer by following these [steps](https://www.isunshare.com/windows-10/6-ways-to-open-event-viewer-in-windows-10.html). When the app is installed through the setup, a new event log named "Mattermost" and an event source "MattermostPresenceProvider" are created. Below is a screenshot of how the logs look in the Event Viewer.
![image](https://user-images.githubusercontent.com/77336594/166907166-ba05171a-a7bd-42ac-ab51-14065eba5f74.png)

As you can see, all the logs are getting logged along with the timestamp and the level(Error and Information). This event log has a maximum size of 5MB and after that space is occupied, it starts overwriting the logs in the order of their timestamps. The oldest log is overwritten first. These settings can be changed after the app is installed by an admin. To do that, see this [link](https://helpcenter.netwrix.com/bundle/Auditor_10.0/page/Content/Configure_IT_Infrastructure/Windows_Server/WS_Event_Log_Settings.htm). Also, these logs will not be visible after the app is uninstalled and the Event Viewer is restarted.

### Continuous Integration (CI)

This project uses Github Actions for its CI/CD needs. You can look at the yml file [here](./.github/workflows/release.yml). The CI script explains the process of building this project in a whole new machine using Windows Powershell. It contains the following steps:

- Setup Visual Studio with the [VS Installer Projects](https://marketplace.visualstudio.com/items?itemName=VisualStudioClient.MicrosoftVisualStudio2017InstallerProjects) extension. The action that is being used in the CI installs this extension by default.
- Restore Nuget packages. You can either do it using [Visual Studio](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore#restore-packages-manually-using-visual-studio) or using [CLI](https://docs.microsoft.com/en-us/nuget/consume-packages/package-restore#restore-using-msbuild).
- Then, we have to enable out of process builds in Visual studio as it does not support building of projects outside Visual Studio processes. You can take a look at it [here](./.github/workflows/release.yml#L22).
- Then, we can build the solution using the `devenv.com` executable provided by Visual Studio using the `Release` configuration. Remember, the setup project `MattermostPresenceProvider` will not be part of the build if you use the `Debug` configuration instead of `Release`.
- Then, the CI script is removing the unwanted files from the build directory and creating a zip of the build with the version number as a suffix followed by creating a Github release.

---
Made with &#9829; by [Brightscout](https://www.brightscout.com)
