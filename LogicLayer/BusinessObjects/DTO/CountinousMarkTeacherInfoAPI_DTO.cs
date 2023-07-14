using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class CountinousMarkTeacherInfoAPI_DTO
    {
        public string TeacherName { get; set; }
        public string DepartmentName { get; set; }
        public string SessionName { get; set; }
        public decimal ExamMark { get; set; }
    }
}
