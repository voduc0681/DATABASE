using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE.Model
{
    internal class Import
    {
        public int ImportID { get; set; }
        public string ImportCode { get; set; }
        public int EmployeeID { get; set; }
        public DateTime ImportDate { get; set; }
        public decimal TotalCost { get; set; }
    }
}
