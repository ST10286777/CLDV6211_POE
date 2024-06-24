using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.WebJobs;
using Microsoft.Extensions.Logging;

namespace KhumaloCraftPOEFunction
{
    public class UpdateQuantity
    {
       
            [Function("UpdateInventory")]
            public static async Task UpdateInventory([Microsoft.Azure.WebJobs.Extensions.DurableTask.ActivityTrigger] OrderDetails orderDetails, ILogger log)
            {
                log.LogInformation($"Updating quantity for order {orderDetails.OrderId}.");

            }
        
    }
}
