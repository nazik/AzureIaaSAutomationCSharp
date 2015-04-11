// TODO: add the namespaces for MAML (step 0)
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Subscriptions;
using Microsoft.WindowsAzure.Subscriptions.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ManagementLibrariesDemo
{
    class Program
    {
        // Prerequisites
        // 1.   Download the following credential file .publishsettings which has a Base64 azure management certificate from this link https://manage.windowsazure.com/publishsettings
        //      You will need Azure Subscription for the above link to work. You can sign up for one from this link http://azure.microsoft.com/en-us/pricing/free-trial/
        //     
        // 2.   Reference Nuget Microsoft Azure Automation Management Library using the Visual Sudio 2013 Package Manger Console (navigate to Tools/Nuget Package Manager/Package Manager Console)
        //      Run this command "Install-Package Microsoft.WindowsAzure.Management.Automation" from the Package Manager Console.

        

        static string credentialsPath = @"[C:\MyFiles\AzureCredentialFile.publishsettings]";    // Reference the file .publishesetting file downloaded from the link above.
        static string subscriptionId = "[Subscription Id]";                  // Copy this from Azure portal under settings area

        static string Base64cer;
        static string subscriptionName = "[Subscription name]";                                                    // Name of the Azure subscription
        static void Main(string[] args)
        {
            RunDemos();

            Console.ReadLine();
        }

        async static void RunDemos()
        {


            Base64cer = getCertFromAzureCredFile(); // Get Base64 encoded certificate from the Azure credential file downloaded from link above
          
            // Create a cloud service
            var cloudServiceName = await CloudServiceDemo.CreateCloudService(getCredentials());
            Console.WriteLine("Cloud Service {0} Created in Subscription {1}", cloudServiceName, subscriptionName);
            
            // Create a storage where VHDs need to be stored
            var storageAccountName = await StorageDemo.CreateStorageAccount(getCredentials());
            Console.WriteLine("Storage Account Created in Subscription {0}", storageAccountName);
            
            // Create a virtual machine inside the cloud service
            var vmAdminUsername = "AzName";
            var vmAdminPassword = "P@ssw0rd!";
            var imageFilter = "Windows Server 2012 R2 Datacenter";

            Console.WriteLine("Begin provisioning virtual machine(s) in subscription {0}:", subscriptionName);

            var vmName = await VirtualMachineDemo.CreateVirtualMachine(getCredentials(),
              cloudServiceName, storageAccountName, vmAdminUsername, vmAdminPassword, imageFilter);
            Console.WriteLine("Virtual Machine {0} created in Subscription {1}", vmName,
          subscriptionName);
            Console.WriteLine("Visit the Azure portal to check if the resources are created. Press any key to terminate this console.");
            Console.ReadLine();
        }
        static SubscriptionCloudCredentials getCredentials()
        {
            return new CertificateCloudCredentials(subscriptionId, new X509Certificate2(Convert.FromBase64String(Base64cer)));
        } 
        static string getCertFromAzureCredFile() 
        {
            XElement x = XElement.Load(credentialsPath);

            var Base64cer = (from c in x.Descendants("Subscription")
                         where c.Attribute("Id").Value == subscriptionId
                         select (string)c.Attribute("ManagementCertificate").Value).FirstOrDefault();

            return Base64cer;
        }
    }
}
