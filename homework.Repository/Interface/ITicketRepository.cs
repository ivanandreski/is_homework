using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Repository.Interface
{
    public interface ITicketRepository
    {
        List<Ticket> GetAll();
        Ticket Get(Guid? id);
        void Insert(Ticket entity);
        void Update(Ticket entity);
        void Delete(Ticket entity);
    }
}
