using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Repository.Interface
{
    public interface IShoppingCartRepository
    {
        bool ActiveCartExists(string userName);

        ShoppingCart FindById(Guid id);

        List<ShoppingCart> FindAllFromUser(User user);

        ShoppingCart FindLatestFromUser(User user);

        void Save(ShoppingCart cart);

        void Update(ShoppingCart entity);
    }
}
