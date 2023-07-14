using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class PreviousEducation
    {
        public int PreviousEducationId {get; set; }
		public int PersonId {get; set; }
		public int EducationTypeId {get; set; }
		public int EducationCategoryId {get; set; }
		public string ExamRollNo {get; set; }
		public string InstituteName {get; set; }
		public string ConcentratedMajor {get; set; }
		public string Board {get; set; }
		public int PassingYear {get; set; }
		public string Session {get; set; }
		public string Duration {get; set; }
		public string Result {get; set; }
		public string MarksOrGPA {get; set; }
		public string MarksOrGPAWithoutOptional {get; set; }
		public int CreatedBy {get; set; }
		public DateTime CreatedDate{get; set; }
		public Nullable<int> ModifiedBy {get; set; }
		public Nullable<DateTime> ModifiedDate{get; set; }
    }
}

