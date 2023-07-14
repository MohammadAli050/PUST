using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamMarkFirstSecondThirdDTO
    {
        public int StudentID { get; set; }
        public string Roll { get; set; }
        public string StudentName { get; set; }
        public int AcaCalSectionId { get; set; }
        public decimal ExamMarkSUM { get; set; }
        public decimal QuestionMark1 { get; set; }
        public decimal QuestionMark2 { get; set; }
        public decimal QuestionMark3 { get; set; }
        public decimal QuestionMark4 { get; set; }
        public decimal QuestionMark5 { get; set; }
        public decimal QuestionMark6 { get; set; }
        public decimal QuestionMark7 { get; set; }
        public decimal QuestionMark8 { get; set; }
        public decimal QuestionMark9 { get; set; }
        public decimal QuestionMark10 { get; set; }
    }
}
