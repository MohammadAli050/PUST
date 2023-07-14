using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class CalenderUnitDistribution
    {
        public int CalenderUnitDistributionID {get; set; }
		public int CalenderUnitMasterID {get; set; }
		public string Name {get; set; }
        public int ProgramId { get; set; }
        public int YearId { get; set; }
        public int SemesterId { get; set; }
		public int Sequence {get; set; }
		public int CreatedBy {get; set; }
		public DateTime CreatedDate{get; set; }
		public int ModifiedBy {get; set; }
		public DateTime ModifiedDate{get; set; }
    }
}

