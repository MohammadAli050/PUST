using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class AcademicCalender
    {
        public int AcademicCalenderID { get; set; }
        public int CalenderUnitTypeID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TotalWeek { get; set; }
        public int Year { get; set; }
        public string Code { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsNext { get; set; }
        public DateTime AdmissionStartDate { get; set; }
        public DateTime AdmissionEndDate { get; set; }
        public bool IsActiveAdmission { get; set; }
        public DateTime RegistrationStartDate { get; set; }
        public DateTime RegistrationEndDate { get; set; }
        public bool IsActiveRegistration { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        //#region Custom Property
        public string CalendarUnitType_TypeName
        {
            get
            {
                CalenderUnitType calenderUnitType = CalenderUnitTypeManager.GetById(CalenderUnitTypeID);
                if (calenderUnitType != null)
                    return calenderUnitType.TypeName;
                return String.Empty;
            }
        }

        public string FullCode
        {
            get
            {
                //return "Semester" + " " + Year.ToString();
                return Year.ToString() + ", " + CalendarUnitType_TypeName + " - " + Code;
            }
        }
        public string FinalExam
        {
            get
            {
                //return "Semester" + " " + Year.ToString();
                return  CalendarUnitType_TypeName  + " " + Year;
            }
        }

        //public CalenderUnitType CalenderUnitType 
        //{
        //    get
        //    {
        //        CalenderUnitType calenderUnitType = CalenderUnitTypeManager.GetById(CalenderUnitTypeID);
        //        return calenderUnitType;
        //    }
        //}

        //#endregion
    }
}
