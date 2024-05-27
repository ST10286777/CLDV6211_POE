using KhumaloCraft_Part2.Data;
using KhumaloCraft_Part2.Models;
using KhumaloCraft_Part2.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace KhumaloCraft_Part2.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductsController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        // GET: Products
        public async Task<IActionResult> MyWork()
        {
            return View(await _context.Product.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,Name,Price,Category,Quantity,ImageUrl,Description")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyWork));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,Price,Category,Quantity,ImageUrl,Description")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(MyWork));
            }
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MyWork));
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id);
        }

        [HttpPost]
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var userCart = await _context.OrderHistory
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.IsCart);
            if (userCart == null)
            {
                userCart = new OrderHistory
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    IsCart = true,
                    Products = new List<Product>()
                };

                
                _context.OrderHistory.Add(userCart);
                await _context.SaveChangesAsync();
            }
            var product = await _context.Products.FindAsync(productId);
            if (product != null)
            {
                product.Quantity = product.Quantity -1;
                userCart.Products.Add(product);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Cart");
        }



        public async Task<IActionResult> CartDetails(OrderHistoryView model)
        {
            if (!string.IsNullOrEmpty(model.FilterUserEmail))
            {
                var user = await _userManager.FindByEmailAsync(model.FilterUserEmail);
                if (user != null)
                {
                    model.FilterUserId = user.Id;
                }
            }

            var query = _context.OrderHistory.AsQueryable();

            if (!string.IsNullOrEmpty(model.FilterUserId))
            {
                query = query.Where(e => e.UserId == model.FilterUserId);
            }

            model.OrderHistories = await query.OrderByDescending(e => e.OrderDate).ToListAsync();
            model.Deliveries = new List<Delivery>();
            model.Transactions = new List<Transaction>(); // Initialize the transactions list

            foreach (var orderHistory in model.OrderHistories)
            {
                orderHistory.Products = await _context.Product
                    .Where(p => p.OrderHistories.Any(o => o.OrderHistoryId == orderHistory.OrderHistoryId))
                    .ToListAsync();

                var delivery = await _context.Deliveries
                    .Include(d => d.OrderHistories)
                    .FirstOrDefaultAsync(d => d.OrderHistories.Any(o => o.OrderHistoryId == orderHistory.OrderHistoryId));
                var transaction = await _context.Transactions
                    .Include(d => d.OrderHistories)
                    .FirstOrDefaultAsync(d => d.OrderHistories.Any(o => o.OrderHistoryId == orderHistory.OrderHistoryId));

                if (delivery != null)
                {
                    model.Deliveries.Add(delivery);
                }
                if (transaction != null)
                {
                    model.Transactions.Add(transaction);
                }
            }

            return View(model);
        }




        [HttpPost]
        public async Task<IActionResult> UpdateOrderProcessed(int orderHistoryId, bool isProcessed)
        {
            var orderHistory = await _context.OrderHistory.FindAsync(orderHistoryId);
            if (orderHistory != null)
            {
                orderHistory.IsProcessed = isProcessed;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("CartDetails");
        }

        [Authorize]
        public async Task<IActionResult> Cart()
        {
            var userId = _userManager.GetUserId(User);
            var userCart = await _context.OrderHistory
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.IsCart);

            var userDeliveries = await _context.Deliveries
                .Include(d => d.OrderHistories)
                .ThenInclude(oh => oh.Products)
                .Where(d => d.UserId == userId)
                .ToListAsync();

            if (userCart == null)
            {
                userCart = new OrderHistory { Products = new List<Product>() };
            }

            var model = new CartViewModel
            {
                CurrentCart = userCart,
                PreviousDeliveries = userDeliveries
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveCartItem(int productId)
        {
            var userId = _userManager.GetUserId(User);
            var userCart = await _context.OrderHistory
                .Include(o => o.Products)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.IsCart);

            if (userCart == null)
            {
                return NotFound();
            }

            var product = userCart.Products.FirstOrDefault(p => p.ProductId == productId);
            if (product == null)
            {
                return NotFound();
            }

            userCart.Products.Remove(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Cart));
        }
        public IActionResult RemoveOrderDetails()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveOrderDetails(int orderHistoryId)
        {
           
            var orderHistory = await _context.OrderHistory.FindAsync(orderHistoryId);
            if (orderHistory == null)
            {
                return NotFound(); 
            }
            _context.OrderHistory.Remove(orderHistory);
            await _context.SaveChangesAsync();

            return RedirectToAction("CartDetails"); 
        }

        // GET: Products/Transaction
        public async Task<IActionResult> Transaction(int orderHistoryId)
        {
            var orderHistory = await _context.OrderHistory.FindAsync(orderHistoryId);
            if (orderHistory == null || orderHistory.UserId != _userManager.GetUserId(User))
            {
                return NotFound();
            }

            var model = new TransactionViewModel
            {
                OrderHistoryId = orderHistoryId
            };

            return View(model);
        }

        // POST: Products/Transaction
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Transaction(TransactionViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var transaction = new Transaction
                {
                    UserId = userId,
                    NameonCard = model.NameonCard,
                    CardNumber = model.CardNumber,
                    ExpiryDate = model.ExpiryDate,
                    SecurityCode = model.SecurityCode,
                    OrderHistoryId = model.OrderHistoryId
                };

               _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();

                return RedirectToAction("OrderConfirmation", "Deliveries");
            }

            return View(model);
        }

    }
}


