using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Domain.Models
{
    public class OrderItem : BaseEntity
    {
        public string MovieName { get; set; }

        public int Quantity { get; set; }

        public Guid ScreaningId { get; set; }

        public virtual Screaning Screaning { get; set; }

        public virtual List<Ticket> Tickets { get; set; }

        public Guid ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
