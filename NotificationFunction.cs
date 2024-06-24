using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace KhumaloCraftPOEFunction
{
    public class NotificationFunction
    {
        [Function("OrderPlacedbyCustomerNotification")]
        public static async Task SendOrderPlacedNotification([Microsoft.Azure.WebJobs.Extensions.DurableTask.ActivityTrigger] OrderDetails orderDetails, ILogger log)
        {
            log.LogInformation($"Sending notification regarding a order placed for {orderDetails.OrderId}.");
            // Logic to send order placed notification via Azure Event Grid
        }

        [Function("OrderProcessedNotification")]
        public static async Task SendOrderProcessedNotification([Microsoft.Azure.WebJobs.Extensions.DurableTask.ActivityTrigger] OrderDetails orderDetails, ILogger log)
        {
            log.LogInformation($"Sending order processed notification for order {orderDetails.OrderId}.");
            // Logic to send order processed notification via Azure Event Grid
        }

        [Function("OrderHasBeenShippedNotification")]
        public static async Task SendOrderShippedNotification([Microsoft.Azure.WebJobs.Extensions.DurableTask.ActivityTrigger] OrderDetails orderDetails, ILogger log)
        {
            log.LogInformation($"Sending notification indicating the order {orderDetails.OrderId} has been shipped.");
            // Logic to send order shipped notification via Azure Event Grid
        }
    }
}
