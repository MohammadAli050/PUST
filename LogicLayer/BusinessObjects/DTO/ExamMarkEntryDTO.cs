using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamMarkEntryDTO
    {
        public int ExamTemplateItemId { get; set; }
        public string CourseSection { get; set; }
        public string Exam { get; set; }
        public int AcaCalSectionID { get; set; }
        public int ExamSetupID { get; set; }
        public string ExamName { get; set; }
        public decimal ExamMark { get; set; }
        public bool IsButtonVisible { get; set; }
        
    }
}
