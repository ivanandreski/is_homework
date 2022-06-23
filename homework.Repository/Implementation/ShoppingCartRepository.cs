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

        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<ShoppingCart>();
        }

        public bool ActiveCartExists(string userName)
        {
            return _entities.Where(s => s.User.UserName.Equals(userName)).Any();
        }

        public List<ShoppingCart> FindAllFromUser(User user)
        {
            return _entities.Where(s => s.UserId == user.Id && s.Active == false).Include(s => s.OrderItems).ToList();
        }

        public ShoppingCart FindById(Guid id)
        {
            return _entities.Include(s => s.OrderItems).FirstOrDefault(s => s.Id.Equals(id));
        }

        public ShoppingCart FindLatestFromUser(User user)
        {
            return _entities.Where(s => s.UserId == user.Id)
                .Include(s => s.OrderItems)
                .FirstOrDefault(s => s.Active);
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
