using homework.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Service.Interface
{
    public interface ITicketService
    {
        List<Ticket> GetAllValidFromUser(string userId);
    }
}
