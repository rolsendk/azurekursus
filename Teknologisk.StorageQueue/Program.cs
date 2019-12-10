using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Queue;
using Newtonsoft.Json;

namespace Teknologisk.StorageQueue
{
    public class AzureVirtualMachineModel
    {
        public string Name { get; set; }
        public string RessourceGroup { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var connectionString = "DefaultEndpointsProtocol=https;AccountName=roldtiqueue;AccountKey=AMvysQ3m+Q+vdD7ytPoV3/ceOsdJm/Aza0qiPLSuc/RsjhiM1Kn/xF/2OUZr14UOUZXBxf6ndkIl5v2IMA09Gg==;EndpointSuffix=core.windows.net";
            CloudStorageAccount account = CloudStorageAccount.Parse(connectionString);
            CloudQueueClient client = account.CreateCloudQueueClient();
            CloudQueue queue = client.GetQueueReference("virtualmachines");

            queue.CreateIfNotExists();

            AzureVirtualMachineModel model;
            for (int i = 0; i < 10; i++)
            {
                model = new AzureVirtualMachineModel()
                {
                    Name = $"VM{i}",
                    RessourceGroup = "teknologisk"
                };
                var json = JsonConvert.SerializeObject(model);

                Console.WriteLine($"Pushing virtual machine to queue: {Environment.NewLine}{json}");

                CloudQueueMessage message = new CloudQueueMessage(json);
                queue.AddMessage(message);
            }

            // Consuming queue
            //while((CloudQueueMessage message = queue.GetMessage()) != null) {

            //}

        }

    }
}
