using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    [Serializable]
    public class RTabulationData
    {
        public string Roll { get; set; }
        public string FullName { get; set; }
        public string FormalCode { get; set; }
        public string RegistrationNo { get; set; }
        public string Code { get; set; }
        public decimal ExamMark { get; set; }
        public decimal Credits { get; set; }
        public int ID { get; set; }
        public int StudentID { get; set; }
        public string ExamGroupName { get; set; }
        public string ExamName { get; set; }
        public int ColumnSequence { get; set; }
        public string Marks { get; set; }
        public string ObtainedGrade { get; set; }
        public decimal ObtainedGPA { get; set; }
        public decimal PointSecured { get; set; }
        public decimal SemesterGPA { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal TotalCredit { get; set; }
        public decimal TotalPoint { get; set; }

    }
}
