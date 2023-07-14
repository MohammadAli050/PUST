using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class EmployeeInfo
    {
        public int EmployeeID { get; set; }
        public int EmployeeTypeId { get; set; }
        public int PersonId { get; set; }
        public string LibraryCardNo { get; set; }
        public string Code { get; set; }
        public string FullName { get; set; }
        public string BanglaName { get; set; }
        public int Status { get; set; }
        public string StatusDetails { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string SMSContactSelf { get; set; }
        public string LogInID { get; set; }
        public string Password { get; set; }
        public int DeptID { get; set; }
        public string DeptName { get; set; }
        public DateTime? DOJ { get; set; }
        public string Program { get; set; }
        public string Remarks { get; set; }
        public string Designation { get; set; }

        public int RoleId
        {
            get
            {
                int RId = 0;

                try
                {
                    User usr = UserManager.GetByLogInId(LogInID);
                    if (usr != null)
                        RId = usr.RoleID;
                }
                catch (Exception ex)
                {
                }


                return RId;
            }
        }
    }
}
