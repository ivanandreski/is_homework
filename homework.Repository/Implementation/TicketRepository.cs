using homework.Domain.Models;
using homework.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace homework.Repository.Implementation
{
    public class TicketRepository : ITicketRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Ticket> _entities;

        public TicketRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Ticket>();
        }

        public void Delete(Ticket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public Ticket Get(Guid? id)
        {
            return _entities.Where(s => s.Id.Equals(id))
                .Include(s => s.Screaning)
                .Include(s => s.User)
                .Include(s => s.OrderItem)
                .FirstOrDefault();
        }

        public List<Ticket> GetAll()
        {
            return _entities.Include(s => s.Screaning)
                .Include(s => s.User)
                .Include(s => s.OrderItem)
                .ToList();
        }

        public void Insert(Ticket entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Ticket entity)
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
