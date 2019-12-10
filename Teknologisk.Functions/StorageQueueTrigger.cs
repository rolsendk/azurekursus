using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Teknologisk.StorageQueue;

namespace Teknologisk.Functions
{
    public static class StorageQueueTrigger
    {
        /// <summary>
        /// Notice! At start the function will receive events for all messages in queue, even messages pushed before function start.
        /// Notice! The function will delete the message automatically.
        /// </summary>am>
        [FunctionName("StorageQueueTrigger")]
        public static void Run([QueueTrigger("virtualmachines", Connection = "AzureWebJobsStorage")]string myQueueItem, ILogger log)
        {
            log.LogInformation($"C# Queue trigger function processed: {myQueueItem}");

            AzureVirtualMachineModel deserialized = JsonConvert.DeserializeObject<AzureVirtualMachineModel>(myQueueItem);

            log.LogInformation($"Virtual machine '{deserialized.Name}' message received");
        }
    }
}
