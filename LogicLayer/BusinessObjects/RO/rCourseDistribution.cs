using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable] 
    public class rCourseDistribution
    {
        public string SemesterName { get; set; }
        public string FormalCode { get; set; }
        public string Title { get; set; }
        public decimal Credits { get; set; }
        public int Sequence { get; set; }
        public int Priority { get; set; }
        public decimal Marks { get; set; }
        public int NodeId { get; set; }
        public string NodeName { get; set; }
    }
}
