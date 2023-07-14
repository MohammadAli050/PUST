using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamMarkSubmissionDate
    {
        	public int ExamMarkSubmissionDateId {get; set; }
		public int AcaCalSectionId {get; set; }
		public DateTime? FirstExaminerSubmissionDate{get; set; }
        public DateTime? SecondExaminerSubmissionDate { get; set; }
        public DateTime? ThirdExaminerSubmissionDate { get; set; }
		public int CreatedBy {get; set; }
        public DateTime CreatedDate { get; set; }
		public int? ModifiedBy {get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}

