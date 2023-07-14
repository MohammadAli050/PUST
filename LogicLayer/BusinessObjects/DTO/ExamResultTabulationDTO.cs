using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamResultTabulationDTO
    {
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string RegistrationNo { get; set; }
        public string CurrentSession { get; set; }
        public string CourseCode { get; set; }
        public decimal CourseCredit { get; set; }
        public decimal RegisteredCredit { get; set; }

        public string Roll { get; set; }
        public string ExamRoll { get; set; }
        public string ExamName { get; set; }
        public int ColumnSequence { get; set; }
        public string MarksOrGrade { get; set; }
    }
}
