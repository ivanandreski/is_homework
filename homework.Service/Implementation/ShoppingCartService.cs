using homework.Domain.Models;
using homework.Repository.Interface;
using homework.Service.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using homework.Domain.Dto;

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

            bool flag = !_orderItemRepository.FindAll()
                .Exists(item => item.Screaning.Id.Equals(screaningId) &&
                    item.ShoppingCartId.Equals(cart.Id));
            if (flag)
            {
                var orderItem = new OrderItem();
                orderItem.Id = Guid.NewGuid();
                orderItem.ShoppingCartId = cart.Id;
                orderItem.ShoppingCart = cart;
                orderItem.ScreaningId = screaning.Id;
                orderItem.Screaning = screaning;
                orderItem.MovieName = screaning.Movie.Name;
                orderItem.Quantity = 1;
                _orderItemRepository.Insert(orderItem);

                //var ticket = new Ticket();
                //ticket.Id = Guid.NewGuid();
                //ticket.UserId = user.Id;
                //ticket.User = user;
                //ticket.ScreaningId = screaningId;
                //ticket.OrderItemId = orderItem.Id;
                //ticket.OrderItem = orderItem;

                //orderItem.Tickets.Add(ticket);

                //_ticketRepository.Insert(ticket);

                // Tickets should be added on purchase not on add to cart

                cart.OrderItems.Add(orderItem);
                _shoppingCartRepository.Update(cart);
            }

        }

        public void AddTicketsToCart(List<Ticket> tickets, string userId)
        {
            throw new NotImplementedException();
        }

        public void ChangeNumOfTickets(Guid orderItemId, int quantity)
        {
            var orderItem =  _orderItemRepository.Get(orderItemId);
            orderItem.Quantity = quantity;
            _orderItemRepository.Update(orderItem);
        }

        public void ClearCart(Guid cartId)
        {
            var cart = _shoppingCartRepository.FindById(cartId);
            //cart.OrderItems.ForEach(o => _orderItemRepository.Delete(o));
            cart.OrderItems.Clear();
            _shoppingCartRepository.Update(cart);
        }

        public void CloseCart(Guid cartId)
        {
            var cart = _shoppingCartRepository.FindById(cartId);
            var user = _userRepository.Get(cart.User.UserName);
            cart.TimeOfPurchase = DateTime.Now;
            cart.Active = false;
            foreach(var item in cart.OrderItems)
            {
                List<Ticket> tickets = new List<Ticket>();

                for(int i=0; i<item.Quantity; i++)
                {
                    var ticket = new Ticket();
                    ticket.Id = Guid.NewGuid();
                    ticket.UserId = user.UserName;
                    ticket.User = user;
                    ticket.OrderItemId = item.Id;
                    ticket.OrderItem = item;
                    var screaning = _screaningRepository.Get(item.ScreaningId);
                    ticket.ScreaningId = screaning.Id;
                    ticket.Screaning = screaning;
                    tickets.Add(ticket);

                    _ticketRepository.Insert(ticket);
                }

                item.Tickets = tickets;
                _orderItemRepository.Update(item);
            }

            _shoppingCartRepository.Update(cart);

            this.Create(cart.User.UserName);
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

        public List<OrderItem> FindAllFromPurchase(Guid cartId)
        {
            return _orderItemRepository.FindAll()
                .Where(o => o.ShoppingCartId.Equals(cartId))
                .ToList();
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

        public PurchaseItemViewModel GetPurchaseItemViewModel(OrderItem item)
        {
            var screaning = _screaningRepository.Get(item.ScreaningId);

            PurchaseItemViewModel model = new PurchaseItemViewModel();
            model.Quantity = _orderItemRepository.Get(item.Id).Tickets.Count();
            model.Movie = item.MovieName;
            model.ScreaningTime = screaning.Date;
            model.Price = (int) screaning.Price;

            return model;
        }

        public void RemoveOrderItem(Guid orderItemId)
        {
            var orderItem = _orderItemRepository.Get(orderItemId);
            _orderItemRepository.Delete(orderItem);
        }
    }
}
