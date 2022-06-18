using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace homework.Domain.Models
{
    public class ShoppingCart : BaseEntity
    {

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime TimeOfPurchase { get; set; }

        public virtual List<OrderItem> OrderItems { get; set; }

        public bool Active { get; set; }
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
