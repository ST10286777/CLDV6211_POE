using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using KhumaloCraft_Part2.Data;
using KhumaloCraft_Part2.Models;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using KhumaloCraft_Part2.ViewModels;

namespace KhumaloCraft_Part2.Controllers
{
    [Authorize]
    public class DeliveriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public DeliveriesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Deliveries/Create
        public IActionResult Create(int orderHistoryId)
        {
            var model = new DeliveryViewModel
            {
                OrderHistoryId = orderHistoryId
            };
            return View(model);
        }

        // POST: Deliveries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DeliveryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var userCart = await _context.OrderHistory
                    .Include(o => o.Products)
                    .FirstOrDefaultAsync(o => o.UserId == userId && o.IsCart);

                if (userCart == null)
                {
                    return NotFound("Cart not found.");
                }

                var delivery = new Delivery
                {
                    UserId = userId,
                    Country = model.Country,
                    FullName = model.FullName,
                    Address = model.Address,
                    City = model.City,
                    PostalCode = model.PostalCode,
                    OrderHistories = new List<OrderHistory> { userCart }
                };

               _context.Deliveries.Add(delivery);

                // Set the cart as no longer a cart (i.e., it's now an order)
                userCart.IsCart = false;
                await _context.SaveChangesAsync();

                // Store the DeliveryId in TempData
                TempData["DeliveryId"] = delivery.DeliveryId;

                // Redirect to the Transaction action in ProductsController
                return RedirectToAction("Transaction", "Products", new { orderHistoryId = userCart.OrderHistoryId });
            }

            return View(model);
        }
        // GET: Deliveries/OrderConfirmation
        public async Task<IActionResult> OrderConfirmation(int id)
        {
            if (TempData["DeliveryId"] is int deliveryId)
            {
                var delivery = await _context.Deliveries
                    .Include(d => d.OrderHistories)
                    .ThenInclude(o => o.Products)
                    .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);

                if (delivery == null)
                {
                    return NotFound();
                }

                return View(delivery);
            }

            return NotFound();
        }

    }
}