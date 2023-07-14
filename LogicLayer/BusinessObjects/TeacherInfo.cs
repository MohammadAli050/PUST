using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class TeacherInfo
    {
        public int EmployeeID { get; set; }
        public int ExamTypeId { get; set; }
        public int AcaCalSectionFacultyType { get; set; }
        public int ExaminerScutinizerTypeId { get; set; }
        public string FullName { get; set; }
        public string Designation { get; set; }
        public string Code { get; set; }

        public string NameAndCode
        {
            get
            {
                return FullName + "[" + Code + "]";
            }
        }

    }
}
