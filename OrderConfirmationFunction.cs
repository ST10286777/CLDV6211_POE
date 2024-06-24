using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace KhumaloCraftPOEFunction
{
    public class OrderConfirmationFunction
    {
       
        [Function("ConfirmationOfOrder")]
        public static async Task ConfirmOrder([Microsoft.Azure.WebJobs.Extensions.DurableTask.ActivityTrigger] OrderDetails orderDetails, ILogger log)
        {
            log.LogInformation($"Confirming the order {orderDetails.OrderId}.");
            // Logic to confirm order
        }
    }
}
