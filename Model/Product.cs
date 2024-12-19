using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE.Model
{
    internal class Product
    {
        public int ProductID { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public decimal SellingPrice { get; set; }
        public int InventoryQuantity { get; set; }
        public decimal ImportPrice { get; set; }
    }
}
