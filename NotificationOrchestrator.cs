using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace KhumaloCraftPOEFunction
{
    public static class NotificationOrchestrator
    {
        [Function("NotificationOrchestrator")]
        public static async Task Run(
           [Microsoft.Azure.WebJobs.Extensions.DurableTask.OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var orderDetails = context.GetInput<OrderDetails>();

            // Order Placed Notification
            await context.CallActivityAsync("OrderPlacedbyCustomerNotification", orderDetails);

            // Order Processed Notification
            await context.CallActivityAsync("OrderProcessedNotification", orderDetails);

            // Order Shipped Notification
            await context.CallActivityAsync("dOrderShippedNotification", orderDetails);
        }
    }
    }
}
