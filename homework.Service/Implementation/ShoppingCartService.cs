using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace homework.Service.Implementation
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITicketRepository _ticketRepository;
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IScreaningRepository _screaningRepository;

        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, IUserRepository userRepository, ITicketRepository ticketRepository, IOrderItemRepository orderItemRepository, IScreaningRepository screaningRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _userRepository = userRepository;
            _ticketRepository = ticketRepository;
            _orderItemRepository = orderItemRepository;
            _screaningRepository = screaningRepository;
        }

        public void AddScreaning(Guid screaningId, string userName)
        {
            var cart = this.FindLatestFromUser(userName);
            var user = _userRepository.Get(userName);
            var screaning = _screaningRepository.Get(screaningId);

            bool flag = !_ticketRepository.GetAll()
                .Exists(t => t.OrderItem.ShoppingCartId.Equals(cart.Id) && t.ScreaningId.Equals(screaningId));
            if (flag)
            {
                var orderItem = new OrderItem();
                orderItem.Id = Guid.NewGuid();
                orderItem.ShoppingCartId = cart.Id;
                orderItem.ShoppingCart = cart;
                orderItem.ScreaningId = screaning.Id;
                orderItem.Screaning = screaning;
                orderItem.MovieName = screaning.Movie.Name;
                _orderItemRepository.Insert(orderItem);

                var ticket = new Ticket();
                ticket.Id = Guid.NewGuid();
                ticket.UserId = user.Id;
                ticket.User = user;
                ticket.ScreaningId = screaningId;
                ticket.OrderItemId = orderItem.Id;
                ticket.OrderItem = orderItem;

                //orderItem.Tickets.Add(ticket);

                _ticketRepository.Insert(ticket);

                cart.OrderItems.Add(orderItem);
                _shoppingCartRepository.Update(cart);
            }

        }

        public void AddTicketsToCart(List<Ticket> tickets, string userId)
        {
            throw new NotImplementedException();
        }

        public void ChangeNumOfTickets(Guid screaningId, Guid orderItemId, string userId)
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

            this.Create(userId);
        }

        public void Create(string userId)
        {
            var user = _userRepository.Get(userId);
            ShoppingCart cart = new ShoppingCart();
            cart.User = user;
            cart.UserId = user.Id;
            cart.Id = Guid.NewGuid();
            cart.Active = true;
            _shoppingCartRepository.Save(cart);
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

            if(!_shoppingCartRepository.ActiveCartExists(userId))
            {
                this.Create(userId);
            }

            var cart =  _shoppingCartRepository.FindLatestFromUser(user);
            var items = _orderItemRepository.FindAll().Where(i => i.ShoppingCartId.Equals(cart.Id)).ToList();
            cart.OrderItems = items;

            return cart;
        }
    }
}
