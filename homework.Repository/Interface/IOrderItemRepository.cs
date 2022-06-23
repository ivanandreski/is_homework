using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Repository.Interface
{
    public interface IOrderItemRepository
    {
        List<OrderItem> FindAll();

        OrderItem Get(Guid? id);
        void Insert(OrderItem entity);
        void Update(OrderItem entity);
        void Delete(OrderItem entity);
    }
}
