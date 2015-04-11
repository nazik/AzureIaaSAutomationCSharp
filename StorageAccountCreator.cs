using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Management.Models;
using Microsoft.WindowsAzure.Management.Storage;
using Microsoft.WindowsAzure.Management.Storage.Models;
using System.IO;
using System.Threading.Tasks;

namespace ManagementLibrariesDemo
{
    /// <summary>
    /// The storage component of the demo.
    /// </summary>
    public class StorageDemo
    {
        /// <summary>
        /// Create a storage account.
        /// </summary>
        /// <param name="credentials">The credentials for the 
        /// authenticated client.</param>
        /// <returns>The name of the storage account</returns>
        public async static Task<string>
            CreateStorageAccount(SubscriptionCloudCredentials credentials)
        {
            var accountName = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());

            using (var storageClient = new StorageManagementClient(credentials))
            {
                var result = await storageClient.StorageAccounts.CreateAsync(
                    new StorageAccountCreateParameters
                    {
                        AccountType = StorageAccountTypes.StandardLRS,
                        Label = "Sample Storage Account",
                        Location = LocationNames.WestUS,
                        Name = accountName
                    });
            }

            return accountName;
        }
    }
}
