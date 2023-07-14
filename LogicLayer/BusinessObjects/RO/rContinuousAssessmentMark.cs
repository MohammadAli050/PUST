using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    [Serializable]
    public class rContinuousAssessmentMark
    {
        public int StudentCourseHistoryId { get; set; }
        public int StudentId { get; set; }
        public int AcaCalSectionID { get; set; }
        public string Roll { get; set; }
        public int ExamTemplateItemId { get; set; }
        public int ColumnSequence { get; set; }
        public string ColumnName { get; set; }
        public string GradePoint { get; set; }
        public string Grade { get; set; }
        public string TemplateGroupName { get; set; }
        public string Marks { get; set; }
        public int ExamStatus { get; set; }
        public int ColumnCount { get; set; }
        public int Serial { get; set; }




    }
}
