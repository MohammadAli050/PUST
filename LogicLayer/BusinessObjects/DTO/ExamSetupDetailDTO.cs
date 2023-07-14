using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamSetupDetailDTO
    {
        public int ExamSetupDetailId { get; set; }
        public int YearNo { get; set; }
        public int SemesterNo { get; set; }
        public int ExamYear { get; set; }
        public string ExamName { get; set; }
        public string ExamShortName { get; set; }
        public bool IsActive { get; set; }
    }
}
