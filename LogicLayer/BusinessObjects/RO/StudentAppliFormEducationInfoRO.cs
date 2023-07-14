using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    public class StudentAppliFormEducationInfoRO
    {
        public string ExamNameBng { get; set; }
        public string ExamNameEng { get; set; }
        public string BoardBng { get; set; }
        public string BoardEng { get; set; }
        public string SchoolCollege { get; set; }
        public string Roll { get; set; }
        public int? Year { get; set; }
        public decimal? Grade { get; set; }
        public string Remarks { get; set; }
    }
}
