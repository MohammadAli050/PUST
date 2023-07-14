using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string Code { get; set; }
        public int DeptID { get; set; }
        public int CreatedBy { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public int SchoolId { get; set; }
        public string Remarks { get; set; }
        public string History { get; set; }
        public int PersonId { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Program { get; set; }
        public DateTime? DOJ { get; set; }
        public int Status { get; set; }
        public string Designation { get; set; }
        public string LibraryCardNo { get; set; }
        public string MIUEmployeeID { get; set; }
        public int EmployeeTypeId { get; set; }
        public int TeacherTypeId { get; set; }
        public int HallInfoId { get; set; }


        #region Custom Property
        public string CodeAndName
        {
            get
            {
                Person person = PersonManager.GetById(PersonId);
                if (person != null)
                    return Code +" - "+ person.FullName;
                else
                    return "";
            }
        }
        public int OfficeId
        {
            get
            {
                //EmployeeOfficeRelation employeeOffice = EmployeeOfficeRelationManager.GetByPersonId(PersonId);
                //if (employeeOffice != null)
                //{
                //    return employeeOffice.OfficeId;
                //}
                //else 
                return 0;
            }
        }
        //public string OfficeAndName
        //{
        //    get
        //    {
        //        try
        //        {
        //            EmployeeOfficeRelation employeeOffice = EmployeeOfficeRelationManager.GetByPersonId(PersonId);
        //            if (employeeOffice != null)
        //            {
        //                Office office = OfficeManager.GetById(employeeOffice.OfficeId);
        //                return office.OfficeName + " - " + EmployeeName;
        //            }
        //            else return EmployeeName;
        //        }
        //        catch { return string.Empty; }
        //    }
        //}
        public string EmployeeName
        {
            get
            {
                Person person = PersonManager.GetById(PersonId);
                if (person != null)
                    return person.FullName;
                else
                    return "";
            }
        }

        public Person BasicInfo
        {
            get
            {
                Person person = new Person();
                person = PersonManager.GetById(PersonId);
                return person;
            }
        }
        public string LoginIdAndName
        {
            get
            {
                string loginIdAndName = "";
                try
                {
                    User user = UserManager.GetByPersonId(PersonId).FirstOrDefault();
                    Person person = PersonManager.GetById(PersonId);
                    if (user != null)
                        loginIdAndName = user.LogInID + " - " + person.FullName;
                    else loginIdAndName = person.FullName;
                }
                catch { }
                return loginIdAndName;
            }
        }
        public string UserLogInId
        {
            get
            {
                User user = UserManager.GetByPersonId(PersonId).FirstOrDefault();
                if (user != null)
                    return user.LogInID;
                else
                    return "";

            }
        }

        public string StatusDetails
        {
            get
            {
                switch (Status)
                {
                    case 1: return "Full Time";
                    case 2: return "Part Time";
                    case 3: return "Half Time";
                    default: return string.Empty;
                }
            }
        }

        //public ContactDetails ContactDetails
        //{
        //    get
        //    {
        //        ContactDetails cd = ContactDetailsManager.GetContactDetailsByPersonID(BasicInfo.PersonID);
        //        return cd;
        //    }
        //}

        public EmployeeType EmployeeTypeInformation
        {
            get
            {
                return EmployeeTypeManager.GetById(EmployeeTypeId);
            }
        }
        public Department DepartmentInformation
        {
            get
            {
                return DepartmentManager.GetById(DeptID);
            }
        }

        #endregion
    }
}
