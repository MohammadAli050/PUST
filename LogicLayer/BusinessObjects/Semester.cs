using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class Semester
    {
        public int SemesterId {get; set; }
		public string SemesterName {get; set; }
		public int YearId {get; set; }
		public int ProgramId {get; set; }
        public int SemesterNo { get; set; }
		public string Attribute1 {get; set; }
		public string Attribute2 {get; set; }
		public string Attribute3 {get; set; }
		public int CreatedBy {get; set; }
		public DateTime CreatedDate{get; set; }
		public int ModifiedBy {get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}

