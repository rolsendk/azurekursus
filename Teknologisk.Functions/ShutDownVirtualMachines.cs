using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.Compute.Fluent;


namespace Teknologisk.Functions
{
    public static class ShutDownVirtualMachines
    {
        [FunctionName("ShutDownVirtualMachines")]
        public static void Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");


            var clientId = "60053bbe-4287-4690-bc3b-966df253ce62";
            var tenantId = "09440d4b-ee23-40e2-823c-ac93a877e714";
            var secret = "_NzrD]W38VSF-lfMxNfDLKHBDX:9D3@5";
            var credentials = SdkContext.AzureCredentialsFactory.FromServicePrincipal(clientId, secret, tenantId, AzureEnvironment.AzureGlobalCloud);

            var azure = Azure.Configure().Authenticate(credentials).WithDefaultSubscription();

            var vms = azure.VirtualMachines;

            foreach(var vm in vms.List())
            {
                var state = vm.PowerState;
                log.LogInformation($"{vm.Name} status: {state}");
                if (vm.PowerState == PowerState.Running)
                {
                    log.LogInformation($"Shutting down vm: {vm.Name}");
                    vm.PowerOff();
                    log.LogInformation($"{vm.Name} is now shut down");
                }

            }

        }
    }
}
