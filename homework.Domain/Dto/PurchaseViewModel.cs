using System;
using System.Collections.Generic;
using System.Text;

namespace homework.Domain.Dto
{
    public class PurchaseViewModel
    {

        public DateTime TimeOfPurchase { get; set; }

        public int Price { get; set; }

        public List<PurchaseItemViewModel> Items { get; set; }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
