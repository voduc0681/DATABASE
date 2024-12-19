using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATABASE.Model
{
    internal class Employee
    {
        public int EmployeeID { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Position { get; set; }
        public string Authority { get; set; }
        public int UserID { get; set; }
    }
}
