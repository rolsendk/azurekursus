using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Cosmos.Table;
using Teknologisk.WebApp.Models;


namespace Teknologisk.WebApp.Controllers
{
    public class StorageTableController : Controller
    {
        public IActionResult Index()
        {
            var connectionstring = "DefaultEndpointsProtocol=https;AccountName=storageaccountkursu9c74;AccountKey=0UKdJ8IwdyLWE30jU43KGqnRu0n73sHORUHfVAE3XgVLHnZwbcez0MKpSOHs0IBNH6jUV9uBgA4gUSzq7LOfGQ==;EndpointSuffix=core.windows.net";
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionstring);
            CloudTableClient cloudTableClient = cloudStorageAccount.CreateCloudTableClient();

            CloudTable cloudTable = cloudTableClient.GetTableReference("customers");

            cloudTable.CreateIfNotExists();



            Customer customer = new Customer("Skovby", Guid.NewGuid().ToString())
            {
                Email = "farmer@farmer.dk",
                FirstName = "Rune",
                LastName = "Klan"
            };

            TableOperation operation = TableOperation.Insert(customer);
            cloudTable.Execute(operation);

            List<Customer> customers = new List<Customer>();
            var hasMore = true;
            while (hasMore)
            {
                TableContinuationToken token = null;
                var queryResult = cloudTable.ExecuteQuerySegmented(new TableQuery<Customer>(), token);
                customers.AddRange(queryResult.Results);
                token = queryResult.ContinuationToken;
                hasMore = token != null;
            }

            return View(customers);
        }
    }
}