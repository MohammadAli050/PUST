using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class StudentAdditionalInfo
    {
        public int InfoId { get; set; }
        public int StudentId { get; set; }
        public int YearId { get; set; }
        public int SemesterId { get; set; }
        public int YearNo { get; set; }
        public int SemesterNo { get; set; }
        public int RunningSession { get; set; }
        public int YearSectionId { get; set; }
        public string RegistrationNo { get; set; }
        public int QuataId { get; set; }
        public int ParentsJobId { get; set; }
        public string FatherPhoneNumber { get; set; }
        public string MotherPhoneNumber { get; set; }
        public int CampusId { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public YearSection StudentYearSection
        {
            get
            {
                return YearSectionManager.GetById(YearSectionId);
            }
        }

        public Year StudentYear
        {
            get
            {
                return YearManager.GetById(YearId);
            }
        }

        public Semester StudentYearSemester
        {
            get
            {
                return SemesterManager.GetById(SemesterId);
            }
        }
    }
}

