﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE.Model
{
    internal class ImportDetail
    {
        public int ImportDetailID { get; set; }
        public int ImportID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal ImportPrice { get; set; }
    }
}
