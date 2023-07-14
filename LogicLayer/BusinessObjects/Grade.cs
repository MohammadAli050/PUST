using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class Grade
    {
        public int GradeId { get; set; }
        public int GradeMasterId { get; set; }
        public string GradeLetter { get; set; }
        public decimal GradePoint { get; set; }
        public decimal MinMarks { get; set; }
        public decimal MaxMarks { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Sequence { get; set; }
    }
}