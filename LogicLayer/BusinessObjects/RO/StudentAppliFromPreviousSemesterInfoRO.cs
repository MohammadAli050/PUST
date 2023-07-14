using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    public class StudentAppliFromPreviousSemesterInfoRO
    {
        public string ExamName { get; set; }
        public int? Year { get; set; }
        public decimal? Result { get; set; }
        public string CourseCode { get; set; }
        public string CourseGP { get; set; }

    }
}
