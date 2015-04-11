using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementLibrariesDemo
{
    /// <summary>
    /// The virtual machine portion of the demo
    /// </summary>
    public class VirtualMachineDemo
    {
        /// <summary>
        /// Creates a virtual machine running one of the Windows operating systems 
        /// (minor tweaks would need to be made to this template to provide for Linux 
        /// VM images). 
        /// </summary>
        /// <param name="credentials">The credentials for the 
        /// authenticated client.</param>
        /// <param name="cloudServiceName">The name of the Cloud
        /// Service in which the VM will be hosted.</param>
        /// <param name="storageAccountName">The name of the 
        /// storage account in which the VM's VHD will be stored.</param>
        /// <param name="username">The administrative username 
        /// of this Virtual Machine</param>
        /// <param name="password">The administrative password
        /// of this Virtual Machine</param>
        /// <param name="imageFilter">A string that will filter the list
        /// of Virtual Machine images. If you provide "Visual Studio" here, 
        /// for instance, you'll create a Virtual Machine running 
        /// the *first* found image with a label  
        /// containing the string "Visual Studio."</param>
        /// <returns>The name of the virtual machine.</returns>
        public async static Task<string> CreateVirtualMachine(
            SubscriptionCloudCredentials credentials,
            string cloudServiceName,
            string storageAccountName,
            string username,
            string password,
            string imageFilter)
        {
            using (var computeClient = new ComputeManagementClient(credentials))
            {
                // get the list of images from the api
                var operatingSystemImageListResult =
                    await computeClient.VirtualMachineOSImages.ListAsync();

                // find the one i want e.g anything that starts with "Windows Server 2012 R2 Datacenter"
                var imageName =
                    operatingSystemImageListResult
                        .Images
                            .FirstOrDefault(x =>
                                x.Label.Contains(imageFilter)).Name;

                var virtualMachineName = cloudServiceName + "vm";

                // set up the configuration set for the windows image
                var windowsConfigSet = new ConfigurationSet
                {
                    ConfigurationSetType = ConfigurationSetTypes.WindowsProvisioningConfiguration,
                    AdminPassword = password,
                    AdminUserName = username,
                    ComputerName = virtualMachineName,
                    HostName = string.Format("{0}.cloudapp.net", cloudServiceName)
                };

                // make sure i enable powershell & rdp access
                var endpoints = new ConfigurationSet
                {
                    ConfigurationSetType = "NetworkConfiguration",
                    InputEndpoints = new List<InputEndpoint>
                    {
                        new InputEndpoint
                        {
                            Name = "PowerShell", LocalPort = 5986, Protocol = "tcp", Port = 5986,
                        },
                        new InputEndpoint
                        {
                            Name = "Remote Desktop", LocalPort = 3389, Protocol = "tcp", Port = 3389,
                        }
                    }
                };

                // set up the hard disk with the os
                var vhd = new OSVirtualHardDisk
                {
                    SourceImageName = imageName,
                    HostCaching = VirtualHardDiskHostCaching.ReadWrite,
                    MediaLink = new Uri(string.Format(CultureInfo.InvariantCulture,
                        "https://{0}.blob.core.windows.net/vhds/{1}.vhd", storageAccountName, imageName),
                            UriKind.Absolute)
                };

                // create the role for the vm in the cloud service
                var role = new Role
                {
                    RoleName = virtualMachineName,
                    RoleSize = VirtualMachineRoleSize.Small,
                    RoleType = VirtualMachineRoleType.PersistentVMRole.ToString(),
                    OSVirtualHardDisk = vhd,
                    ConfigurationSets = new List<ConfigurationSet>
                    {
                        windowsConfigSet,
                        endpoints
                    },
                    ProvisionGuestAgent = true
                };

                // create the deployment parameters
                var createDeploymentParameters = new VirtualMachineCreateDeploymentParameters
                {
                    Name = cloudServiceName,
                    Label = cloudServiceName,
                    DeploymentSlot = DeploymentSlot.Production,
                    Roles = new List<Role> { role }
                };

                // deploy the virtual machine
                var deploymentResult = await computeClient.VirtualMachines.CreateDeploymentAsync(
                    cloudServiceName,
                    createDeploymentParameters);

                // return the name of the virtual machine
                return virtualMachineName;
            }
        }
    }
}
