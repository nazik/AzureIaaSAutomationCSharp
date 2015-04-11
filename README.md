# AzureIaaSAutomationCSharp
Starter Azure IaaS automation done with C# using Microsoft Azure Automation Management Library

Prerequisites

1. 
Visual Studio 2013 Community Edition or above https://www.visualstudio.com/en-us/products/visual-studio-community-vs

2.   
Download the following credential file .publishsettings which has the Base64 azure management certificate 
from this link https://manage.windowsazure.com/publishsettings. You will need an Azure Subscription to download the file. 
You can sign up for one from this link http://azure.microsoft.com/en-us/pricing/free-trial/ .

3.   
Reference Nuget "Microsoft Azure Automation Management Library".

  Right-click on the solution and select "Enable Nuget Package Retore".

  Or

  Use the Visual Sudio 2013 Package Manger Console (navigate to Tools/Nuget Package Manager/Package Manager Console). 
  Then run this command "Install-Package Microsoft.WindowsAzure.Management.Automation" from the Package Manager Console.

