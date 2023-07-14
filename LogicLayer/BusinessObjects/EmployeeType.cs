using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class EmployeeType
    {
        public int EmployeeTypeId { get; set; }
        public string EmployeTypeName { get; set; }
        public string EmployeTypeNameBEN { get; set; }
        public int IndexValue { get; set; }
        public bool IsActive { get; set; }
        public int CivilNoCivilTypeID { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
