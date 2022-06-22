using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Service.Interface
{
    public interface IShoppingCartService
    {
        List<ShoppingCart> FindAllFromUser(string userId);

        ShoppingCart FindById(Guid id);

        ShoppingCart FindLatestFromUser(string userId);

        void Create(string userId);

        void AddTicketsToCart(List<Ticket> tickets, string userId);

        void CloseCart(string userId);
    }
}
