using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class StudentCourseGradeDTO
    {
        public int StudentId { get; set; }
        public string StudentName { get; set; }
        public string Roll { get; set; }
        public string ExamRoll { get; set; }
        public decimal Marks { get; set; }
        public string Grade { get; set; }
    }
}
