using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExamMarkGridViewDTO
    {
        public int ExamMarkId { get; set; }
        public string Roll { get; set; }
        public string Name { get; set; }
        public decimal Mark { get; set; }
        public string PresentAbsent { get; set; }
    }
}
