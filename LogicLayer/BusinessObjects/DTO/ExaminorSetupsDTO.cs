using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    public class ExaminorSetupsDTO
    {
        public int ID { get; set; }
        public Nullable<int> AcaCalSectionId { get; set; }
        public string Title { get; set; }      
        public string FirstExaminer { get; set; }
        public string SecondExaminor { get; set; }
        public string ThirdExaminor { get; set; }
        public string ExamName { get; set; }
       
    }
}
