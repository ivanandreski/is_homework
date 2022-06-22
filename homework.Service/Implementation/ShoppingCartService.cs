using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserRepository _userRepository;

        public void AddTicketsToCart(List<Ticket> tickets, string userId)
        {
            throw new NotImplementedException();
        }

        public void CloseCart(string userId)
        {
            var user = _userRepository.Get(userId);
            var cart = _shoppingCartRepository.FindLatestFromUser(user);
            cart.TimeOfPurchase = DateTime.Now;
            cart.Active = false;
            _shoppingCartRepository.Update(cart);
        }

        public void Create(string userId)
        {
            var user = _userRepository.Get(userId);
            ShoppingCart cart = new ShoppingCart();
            cart.User = user;
            cart.UserId = user.Id;
            cart.Id = Guid.NewGuid();
            cart.Active = true;
        }

        public List<ShoppingCart> FindAllFromUser(string userId)
        {
            var user = _userRepository.Get(userId);

            return _shoppingCartRepository.FindAllFromUser(user);
        }

        public ShoppingCart FindById(Guid id)
        {
            return _shoppingCartRepository.FindById(id);
        }

        public ShoppingCart FindLatestFromUser(string userId)
        {
            var user = _userRepository.Get(userId);

            return _shoppingCartRepository.FindLatestFromUser(user);
        }
    }
}
