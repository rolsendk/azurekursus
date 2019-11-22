using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Teknologisk.WebApp.Models;

namespace Teknologisk.WebApp.Controllers
{
    public class StorageBlobController : Controller
    {
        public IActionResult Index()
        {

            AzureServiceTokenProvider tokenProvider = new AzureServiceTokenProvider();
            KeyVaultClient keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback((tokenProvider.KeyVaultTokenCallback)));

            // asp.net classic app settings => environ variables in .net core
            // Define the keys locally in Properties/launchSettings.json
            // Define manually in Azure web app config keys, or via VS Publish menu "Edit Azure App Service settings"
            var azureKeyVaultUrl = Environment.GetEnvironmentVariable("AzureKeyVaultUrl"); // https://aladdinscave.vault.azure.net/

            var connectionStringViaKeyVault = keyVaultClient.GetSecretAsync(azureKeyVaultUrl, "storageAccountKey").GetAwaiter().GetResult().Value;

            //var connectionstring = "DefaultEndpointsProtocol=https;AccountName=storageaccountkursu9c74;AccountKey=0UKdJ8IwdyLWE30jU43KGqnRu0n73sHORUHfVAE3XgVLHnZwbcez0MKpSOHs0IBNH6jUV9uBgA4gUSzq7LOfGQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionStringViaKeyVault);
            CloudBlobClient client = new CloudBlobClient(account.BlobStorageUri, account.Credentials);

            CloudBlobContainer container = client.GetContainerReference("logs");
            container.CreateIfNotExists();

            ICollection<BlobModel> models = new Collection<BlobModel>();

            foreach (IListBlobItem blob in container.ListBlobs())
            {
                if (!(blob is CloudBlockBlob)) continue;

                CloudBlockBlob cBlob = (CloudBlockBlob)blob;

                models.Add(new BlobModel
                {
                    Name = cBlob.Name,
                    Url = cBlob.Uri.ToString()
                });
            }

            return View(models);

        }
    }
}