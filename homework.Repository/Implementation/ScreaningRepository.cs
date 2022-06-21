using homework.Domain.Models;
using Microsoft.EntityFrameworkCore;
using homework.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace homework.Repository.Implementation
{
    public class ScreaningRepository : IScreaningRepository
    {
        private readonly ApplicationDbContext _context;
        private DbSet<Screaning> _entities;

        public ScreaningRepository(ApplicationDbContext context)
        {
            _context = context;
            _entities = context.Set<Screaning>();
        }

        public void Delete(Screaning entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Remove(entity);
            _context.SaveChanges();
        }

        public Screaning Get(Guid? id)
        {
            return _entities.Where(s => s.Id.Equals(id)).Include(s => s.Movie).FirstOrDefault();
        }

        public List<Screaning> GetAll()
        {
            return _entities.Include(s => s.Movie).ToList();
        }

        public void Insert(Screaning entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            _entities.Add(entity);
            _context.SaveChanges();
        }

        public void Update(Screaning entity)
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
