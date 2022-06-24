using homework.Domain.Models;
using homework.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace homework.Repository.Implementation
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<OrderItem> _entities;

        public OrderItemRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<OrderItem>();
        }

        public List<OrderItem> FindAll()
        {
            return _entities.Include(o => o.ShoppingCart)
                .Include(o => o.Tickets)
                .Include(o => o.Screaning)
                .ToList();
        }

        public OrderItem Get(Guid? id)
        {
            return _entities.Include(o => o.Tickets).SingleOrDefault(s => s.Id == id);
        }
        public void Insert(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Update(entity);
            _context.SaveChanges();
        }

        public void Delete(OrderItem entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            _context.SaveChanges();
        }
    }
}
