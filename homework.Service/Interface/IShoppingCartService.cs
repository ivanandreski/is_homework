using homework.Domain.Dto;
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

        List<OrderItem> FindAllFromPurchase(Guid cartId);

        void Create(string userId);

        void AddTicketsToCart(List<Ticket> tickets, string userId);

        void AddScreaning(Guid screaningId, string userName);

        bool ChangeNumOfTickets(Guid orderItemId, int quantity);

        void CloseCart(Guid cartId);

        void ClearCart(Guid cartId);

        void RemoveOrderItem(Guid orderItemId);

        PurchaseItemViewModel GetPurchaseItemViewModel(OrderItem item);

        PurchaseViewModel GetPurchaseViewModel(Guid purchaseId, List<Guid> purchaseItemIds);
    }
}
