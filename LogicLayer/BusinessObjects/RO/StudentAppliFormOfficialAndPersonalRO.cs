using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.RO
{
    public class StudentAppliFormOfficialAndPersonalRO
    {
        public string Department { get; set; }
        public string Faculty { get; set; }
        public string Hall { get; set; }
        public string StudentType { get; set; }
        public string StudentIDNo { get; set; }
        public string StudentRegNo { get; set; }
        public string StudentAcademicYear { get; set; }
        public string AppliedSession { get; set; }
        public string AppliedSemester { get; set; }
        public string AppliedProgram { get; set; }
        public int AppliedYear { get; set; }
        public string NameBng { get; set; }
        public string NameEng { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string GuardianName { get; set; }
        public string Mobile { get; set; }
        public string NationalityBng { get; set; }
        public string NationalityEng { get; set; }
        public DateTime? DateofBirth { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string PhotoPath { get; set; }
        public string SignaturePath { get; set; }

    }
}
