using homework.Domain.Dto;
using homework.Repository.Interface;
using homework.Service.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace homework.web.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderItemRepository _orderItemRepository;

        public UserController(IShoppingCartService shoppingCartService, IOrderItemRepository orderItemRepository)
        {
            _shoppingCartService = shoppingCartService;
            _orderItemRepository = orderItemRepository;
        }

        [HttpGet]
        public IActionResult Purchases()
        {
            var purchases = _shoppingCartService.FindAllFromUser(User.Identity.Name);
            ViewData["Purchases"] = purchases;
            List<int> prices = new List<int>();
            foreach (var purchase in purchases)
            {
                int price = 0;
                foreach(var orderItem in _shoppingCartService.FindAllFromPurchase(purchase.Id))
                {
                    price += orderItem.Quantity * (int)orderItem.Screaning.Price;
                }

                prices.Add(price);
            }
            ViewData["Prices"] = prices;

            return View();
        }

        [HttpGet]
        public IActionResult Purchase(Guid cartId, int price)
        {
            var cart = _shoppingCartService.FindById(cartId);
            List<PurchaseItemViewModel> items = new List<PurchaseItemViewModel>();
            cart.OrderItems.ForEach(i => items.Add(_shoppingCartService.GetPurchaseItemViewModel(i)));
            ViewData["items"] = items;
            ViewData["Price"] = price.ToString();

            return View(cart);
        }
    }
}
