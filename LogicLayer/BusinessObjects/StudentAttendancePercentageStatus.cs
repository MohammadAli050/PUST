using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class StudentAttendancePercentageStatus
    {
        public int StudentPercentageId {get; set; }
		public int StudentCourseHistoryId {get; set; }
		public decimal Percentage { get; set; }
		public int Status {get; set; }
		public string Attribute1 {get; set; }
		public string Attribute2 {get; set; }
		public int CreatedBy {get; set; }
        public Nullable<DateTime>  CreatedDate{get; set; }
		public int ModifiedBy {get; set; }
        public Nullable<DateTime> ModifiedDate {get; set; }
    }
}

