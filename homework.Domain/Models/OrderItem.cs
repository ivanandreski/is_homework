using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Domain.Models
{
    public class OrderItem : BaseEntity
    {

        public int Quantity { get; set; }
        
        public Guid TicketId { get; set; }

        public Ticket Ticket { get; set; }

        public Guid ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
