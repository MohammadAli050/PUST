using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class StudentACUDetailDTO
    {
        public string YearName { get; set; }
        public string SemesterName { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal EarnedCredit { get; set; }
        public decimal GPA { get; set; }
        public decimal CGPA { get; set; }
        public string ExamStatus { get; set; }
    }
}
