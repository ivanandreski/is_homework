using homework.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace homework.web.Controllers
{
    [Authorize]
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;

        public ShoppingCartController(IShoppingCartService shoppingCartService)
        {
            _shoppingCartService = shoppingCartService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var userId = User.Identity.Name;
            var cart = _shoppingCartService.FindLatestFromUser(userId);

            ViewData["ShoppingCart"] = cart;

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

        [HttpPost]
        public IActionResult Purchase(Guid cartId)
        {
            if(cartId == null)
            {
                return NotFound();
            }

            _shoppingCartService.CloseCart(cartId);

            return Redirect("/ShoppingCart");
        }

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
