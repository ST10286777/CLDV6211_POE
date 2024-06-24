using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.DurableTask;
using Microsoft.DurableTask.Client;
using Microsoft.Extensions.Logging;

namespace KhumaloCraftPOEFunction
{
    public static class OrderProcessingOrchestrator
    {
        [Function(nameof(OrderProcessingOrchestrator))]
        public static async Task Run(
        [Microsoft.Azure.Functions.Worker.OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            var orderDetails = context.GetInput<OrderDetails>();

            // Update Inventory
            await context.CallActivityAsync("UpdateInventory", orderDetails);

            // Process Payment
            await context.CallActivityAsync("ProcessPayment", orderDetails);

            // Confirm Order
            await context.CallActivityAsync("ConfirmOrder", orderDetails);

            // Send Notifications
            await context.CallSubOrchestratorAsync("NotificationWorkflowOrchestrator", orderDetails);
        }
    }
}
