using System;
using System.Collections.Generic;

namespace homework.Domain.Dto
{
    public class PurchaseItemViewModel
    {
        public string Movie { get; set; }

        public DateTime ScreaningTime { get; set; }

        public int Quantity { get; set; }

        public int Price { get; set; }

        public int TotalPrice()
        {
            return Quantity * Price;
        }
    }
}
