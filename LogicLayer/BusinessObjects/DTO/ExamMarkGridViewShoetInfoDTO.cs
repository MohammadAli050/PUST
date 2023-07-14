using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamMarkGridViewShoetInfoDTO
    {
        public string CourseName { get; set; }
        public string SectionName { get; set; }
        public string ExamName { get; set; }
        public int TotalStudentCount { get; set; }
        public int AbsentCount { get; set; }

        public string DepartmentName { get; set; }
        public string ProgramName { get; set; }
        public string CourseTitle { get; set; }
        public decimal ExamMark { get; set; }
        public string TeacherName { get; set; }
        public string TeacherDesignation { get; set; }
        public string Session { get; set; }
    }
}
