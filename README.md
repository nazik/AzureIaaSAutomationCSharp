# AzureIaaSAutomationCSharp
An Azure IaaS Automation C# starter solution using Microsoft Azure Management Libraries (MAML)

Troubleshooting: 
  Please do right-click on Visual Studio Solution and select "Clean Solution" if you encounter problems compiling after        restoring the NuGet package as instructed below.

Prerequisites

1. 
Visual Studio 2013 Community Edition or above https://www.visualstudio.com/en-us/products/visual-studio-community-vs

2.   
Download credential file .publishsettings which has the Base64 Azure management certificate 
from this link https://manage.windowsazure.com/publishsettings and save it in a hard drive folder. 
You will need an Azure Subscription to download the file. You can sign up for one from this link http://azure.microsoft.com/en-us/pricing/free-trial/ .

3.   
Reference Nuget "Microsoft Azure Management Libraries" in one of the following ways:

  Right-click on the solution node and select "Enable Nuget Package Retore".

  Or

  Use the Visual Sudio 2013 Package Manger Console (navigate to Tools/Nuget Package Manager/Package Manager Console). 
  Then run this command "Install-Package Microsoft.WindowsAzure.Management.Libraries" from the Package Manager Console.

