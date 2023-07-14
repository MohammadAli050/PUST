using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExaminerSetupStudentWise
    {
        public int ExaminerSetupStudentWiseId {get; set; }
		public int AcaCalSectionId {get; set; }
		public int StudentCourseHistoryId {get; set; }
		public int ExamSetupDetailId {get; set; }
		public int FirstExaminer {get; set; }
		public int SecondExaminer {get; set; }
		public int ThirdExaminer {get; set; }
		public string Attribute1 {get; set; }
		public string Attribute2 {get; set; }
		public string Attribute3 {get; set; }
		public bool IsDeleted{get; set; }
		public int CreatedBy {get; set; }
		public DateTime CreatedDate{get; set; }
		public int? ModifiedBy {get; set; }
        public DateTime? ModifiedDate { get; set; }

        public Employee ExaminerName1
        {
            get
            {
                if (FirstExaminer > 0)
                {
                    return EmployeeManager.GetById(FirstExaminer);
                }
                 return null;
            }
            
        }

        public Employee ExaminerName2
        {
            get
            {
                if (SecondExaminer > 0)
                {
                    return EmployeeManager.GetById(SecondExaminer);
                }
                return null;
            }

        }

        public Employee ExaminerName3
        {
            get
            {
                if (ThirdExaminer > 0)
                {
                    return EmployeeManager.GetById(ThirdExaminer);
                }
                return null;
            }

        }

        public int ExaminerId { get; set; }
        public string ExaminerName { get; set; }


        //public Person Person
        //{
        //    get
        //    {
        //        if (Employee != null)
        //        {
        //            return PersonManager.GetById(Employee.PersonId);
        //        }
        //        return null;
        //    }

        //}
        
    }
}

