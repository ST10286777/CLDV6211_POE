using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace KhumaloCraftPOEFunction
{
    public class PaymentProcessFunction
    {
       
            [Function("ProcessPayment")]
            public static async Task ProcessPayment([Microsoft.Azure.WebJobs.Extensions.DurableTask.ActivityTrigger] OrderDetails orderDetails, ILogger log)
            {
                log.LogInformation($"The payment for order {orderDetails.OrderId} has been processed.");
                // Logic to process payment
            }
        
    }
}
