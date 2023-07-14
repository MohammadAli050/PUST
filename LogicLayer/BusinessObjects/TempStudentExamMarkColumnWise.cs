//using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class TempStudentExamMarkColumnWise
    {
        public int StudentCourseHistoryId { get; set; }
        public int StudentId { get; set; }
        public int AcaCalSectionID { get; set; }
        public string Roll { get; set; }
        public int ExamTemplateItemId { get; set; }
        public int ColumnSequence { get; set; }
        public string ColumnName { get; set; }
        public decimal GradePoint { get; set; }
        public string Grade { get; set; }
        public decimal Marks { get; set; }
        public int ExamStatus { get; set; }
    }
}

