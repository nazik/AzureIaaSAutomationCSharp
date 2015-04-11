using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Compute;
using Microsoft.WindowsAzure.Management.Compute.Models;
using Microsoft.WindowsAzure.Management.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementLibrariesDemo
{
    public static class CloudServiceDemo
    {
        public async static Task<string> CreateCloudService(
            this SubscriptionCloudCredentials credentials
            )
        {
            var randomName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            using (var computeManagementClient = new ComputeManagementClient(credentials))
            {
                var createHostedServiceResult = await computeManagementClient.HostedServices.CreateAsync(
                    new HostedServiceCreateParameters
                    {
                        Label = "NewCloudService",
                        Location = LocationNames.WestUS,
                        ServiceName = randomName
                    });
            }

            return randomName;
        }
    }
}
