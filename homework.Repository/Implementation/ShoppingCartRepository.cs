using homework.Domain.Models;
using homework.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace homework.Repository.Implementation
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<ShoppingCart> _entities;

        public List<ShoppingCart> FindAllFromUser(User user)
        {
            return _entities.Where(s => s.UserId == user.Id && s.Active == false).ToList();
        }

        public ShoppingCart FindById(Guid id)
        {
            return _entities.FirstOrDefault(s => s.Id.Equals(id));
        }

        public ShoppingCart FindLatestFromUser(User user)
        {
            return _entities.FirstOrDefault(s => s.Active);
        }

        public void Save(ShoppingCart cart)
        {
            if (cart == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(cart);
            _context.SaveChanges();
        }

        public void Update(ShoppingCart entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Update(entity);
            _context.SaveChanges();
        }
    }
}
