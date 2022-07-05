using homework.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System;
using System.Linq;

namespace homework.web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IScreaningService screaningService;

        public ShoppingCartController(IShoppingCartService shoppingCartService, IScreaningService screaningService)
        {
            _shoppingCartService = shoppingCartService;
            this.screaningService = screaningService;
        }

        [HttpGet]
        public IActionResult Index(string error)
        {
            if(!string.IsNullOrEmpty(error))
                ViewData["Error"] = error;

            var userId = User.Identity.Name;
            var cart = _shoppingCartService.FindLatestFromUser(userId);
            var maxTickets = cart.OrderItems.Select(i => screaningService.FindAvailableTicketsForScreaning(i.ScreaningId)).ToList();

            ViewData["ShoppingCart"] = cart;
            ViewData["AvailableTickets"] = maxTickets;
            double totalPrice = 0;

            foreach (var item in cart.OrderItems)
            {
                var screaning = screaningService.FindById(item.ScreaningId);

                totalPrice += screaning.Price * item.Quantity;
            }
            ViewData["TotalPrice"] = totalPrice;

            return View();
        }

        [HttpPost]
        public IActionResult AddToCart(Guid? id)
        {
            if (id == null)
                return NotFound();

            _shoppingCartService.AddScreaning(id.GetValueOrDefault(), User.Identity.Name);

            return Redirect("/ShoppingCart");
        }

        [HttpPost]
        public IActionResult ChangeQuantity(Guid orderItemId, int quantity)
        {
            if (orderItemId == null) return NotFound();

            _shoppingCartService.ChangeNumOfTickets(orderItemId, quantity);

            return Redirect("/ShoppingCart");

        }

        [HttpPost]
        public IActionResult RemoveOrderItem(Guid orderItemId)
        {
            if (orderItemId == null)
            {
                return NotFound();
            }

            _shoppingCartService.RemoveOrderItem(orderItemId);

            return Redirect("/ShoppingCart");
        }

        public IActionResult PayOrder(string stripeEmail, string stripeToken)
        {
            var customerService = new CustomerService();
            var chargeService = new ChargeService();
            string userName = User.Identity.Name;
            var cart = _shoppingCartService.FindLatestFromUser(userName);
            double totalPrice = 0;

            foreach(var item in cart.OrderItems)
            {
                var screaning = screaningService.FindById(item.ScreaningId);

                totalPrice += screaning.Price * item.Quantity;
            }

            var customer = customerService.Create(new CustomerCreateOptions
            {
                Email = stripeEmail,
                Source = stripeToken
            });

            var charge = chargeService.Create(new ChargeCreateOptions
            {
                Amount = Convert.ToInt32(totalPrice) * 100,
                Description = "EShop Application Payment",
                Currency = "usd",
                Customer = customer.Id
            });

            if (charge.Status == "succeeded")
            {
                _shoppingCartService.CloseCart(cart.Id);

                return Redirect("/ShoppingCart");
            }
            else
            {
                return Redirect("/ShoppingCart?error=PaymentError");
            }
        }

        //[HttpPost]
        //public IActionResult Purchase(Guid cartId)
        //{
        //    if(cartId == null)
        //    {
        //        return NotFound();
        //    }

        //    _shoppingCartService.CloseCart(cartId);

        //    return Redirect("/ShoppingCart");
        //}

        [HttpPost]
        public IActionResult ClearCart(Guid cartId)
        {
            if (cartId == null)
            {
                return NotFound();
            }

            _shoppingCartService.ClearCart(cartId);

            return Redirect("/ShoppingCart");
        }
    }
}
