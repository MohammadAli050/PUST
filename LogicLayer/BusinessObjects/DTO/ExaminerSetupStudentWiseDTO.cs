using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExaminerSetupStudentWiseDTO
    {
        public int ExaminerSetupStudentWiseId { get; set; }
        public int AcaCalSectionId { get; set; }
        public int StudentCourseHistoryId { get; set; }
        public int ExamSetupDetailId { get; set; }
        public string StudentName { get; set; }
        public string Roll { get; set; }
        public string FirstExaminer { get; set; }
        public string SecondExaminer { get; set; }
        public string ThirdExaminer { get; set; }
        public string ExamName { get; set; }
    }
}
