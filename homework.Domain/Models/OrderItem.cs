using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Domain.Models
{
    public class OrderItem : BaseEntity
    {

        public int Quantity { get; set; }
        
        public string TicketId { get; set; }

        public Ticket Ticket { get; set; }

        public string ShoppingCartId { get; set; }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
