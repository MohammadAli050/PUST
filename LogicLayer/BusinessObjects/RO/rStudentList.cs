using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    public class rStudentList
    {
        public int StudentId { get; set; }
        public int Roll { get; set; }
        public string Name { get; set; }

        public string Phone { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string RegistrationNo { get; set; }
        public string AdmissionSession { get; set; }
        public string CurrentSession { get; set; }
        public int CurrentSessionId { get; set; }
    }
}
