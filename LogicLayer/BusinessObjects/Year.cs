using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class Year
    {
        public int YearId {get; set; }
		public string YearName {get; set; }
		public string Code {get; set; }
        public int YearNo { get; set; }
		public int ProgramId {get; set; }
		public int DepartmentId {get; set; }
		public int CreatedBy {get; set; }
		public DateTime CreatedDate{get; set; }
		public int ModifiedBy {get; set; }
        public DateTime ModifiedDate { get; set; }

        public Program ProgramObj
        {
            get
            {
                return ProgramManager.GetById(ProgramId);
            }
        }
    }
}

