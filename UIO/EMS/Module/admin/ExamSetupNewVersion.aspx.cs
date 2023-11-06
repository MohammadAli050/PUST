using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ExamSetupNewVersion : BasePage
    {
        static User user = null;
        static DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        static int RoleId = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.CheckPage_Load();
                string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                user = UserManager.GetByLogInId(loginId);
                if (!IsPostBack && !IsCallback)
                {
                    RoleId = user.RoleID;

                    if (RoleId == 1)
                    {

                    }
                    else if (RoleId == 8) // Controller
                    {
                        lblTitle.Text = "Exam Held In Relation With Program";
                        RoleField.Value = "8";
                    }
                    else if (RoleId == 11) // Chairman of Dept
                    {
                        lblTitle.Text = "Exam Committee Setup Information";
                        RoleField.Value = "11";
                    }

                }
            }
            catch (Exception) { }
        }

        #region Web API Methods

        [WebMethod]
        public static string GetDepartmentList(int abc)
        {
            List<Department> departmentList = new List<Department>();
            List<Department> tempdepartmentList = DepartmentManager.GetAll();
            if (tempdepartmentList != null && tempdepartmentList.Any())
            {
                if (RoleId != 1 && RoleId != 8) // Admin And COE
                {
                    List<Program> programList = new List<Program>();

                    UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(user.User_ID);
                    if (uapObj != null && !string.IsNullOrEmpty(uapObj.AccessPattern))
                    {
                        string[] accessCode = uapObj.AccessPattern.Split('-');
                        foreach (string s in accessCode)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                Program program = ProgramManager.GetById(Convert.ToInt32(s));
                                programList.Add(program);
                            }
                        }
                    }
                    if (programList != null && programList.Any())
                    {
                        foreach (var item in programList)
                        {
                            var Dept = tempdepartmentList.Where(x => x.DeptID == item.DeptID).FirstOrDefault();
                            var AlreadyExists = departmentList.Where(x => x.DeptID == item.DeptID).FirstOrDefault();

                            if (Dept != null && AlreadyExists == null)
                                departmentList.Add(Dept);

                        }
                    }

                }
                else
                    departmentList = tempdepartmentList;
                departmentList = departmentList.OrderBy(x => x.DetailedName).ToList();
                string Result = JsonConvert.SerializeObject(departmentList);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetProgramList(int DepartmentId)
        {
            List<Program> programList = new List<Program>();
            programList = ProgramManager.GetAll();
            programList = programList.OrderBy(x => x.ShortName).ToList();

            if (DepartmentId > 0)
                programList = programList.Where(x => x.DeptID == DepartmentId).ToList();

            if (programList != null && programList.Any())
            {
                string Result = JsonConvert.SerializeObject(programList);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetYearList()
        {
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();
            if (yearList != null && yearList.Any())
            {
                string Result = JsonConvert.SerializeObject(yearList);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetSemesterList()
        {
            List<SemesterDistinctDTO> semesterList = new List<SemesterDistinctDTO>();
            semesterList = SemesterManager.GetAllDistinct();
            semesterList = semesterList.OrderBy(x => x.SemesterNo).ToList();

            if (semesterList != null && semesterList.Any())
            {
                string Result = JsonConvert.SerializeObject(semesterList);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetYearShal()
        {
            List<Shal> shalList = new List<Shal>();

            for (int i = DateTime.Now.Year; i > 1950; i--)
            {
                Shal NewObj = new Shal();
                NewObj.Year = i.ToString();

                shalList.Add(NewObj);
            }
            if (shalList != null && shalList.Any())
            {
                string Result = JsonConvert.SerializeObject(shalList);
                return Result;
            }
            else
                return null;
        }
        public class Shal
        {
            public string Year { get; set; }
        }

        [WebMethod]
        public static string GetSessionList()
        {
            List<AcademicCalender> sessionList = AcademicCalenderManager.GetAll().OrderByDescending(m => m.AcademicCalenderID).ToList();

            if (sessionList != null && sessionList.Any())
            {
                string Result = JsonConvert.SerializeObject(sessionList);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetHeldInList(int AcacalId, string Year)
        {
            var HeldInList = ucamContext.ExamHeldInInformations.AsNoTracking().Where(x => (x.AcacalId == AcacalId || AcacalId == 0)
                && (x.Year == Year || Year == "0")
                && x.IsActive == true).ToList();

            if (HeldInList != null && HeldInList.Any())
            {
                foreach (var item in HeldInList)
                {
                    item.ExamName = item.HeldInStartMonth + "-" + item.HeldInEndMonth + " " + item.HeldInEndYear;
                }

                string Result = JsonConvert.SerializeObject(HeldInList);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string SaveUpdateHeldInAndProgramRelation(string receiveListString)
        {
            string Result = "";
            try
            {
                var DataList = JsonConvert.DeserializeObject<List<DAL.ExamHeldInAndProgramRelation>>(receiveListString);
                DAL.ExamHeldInAndProgramRelation receiveObj = DataList[0];

                if (receiveObj != null)
                {
                    if (receiveObj.Id > 0)//Update
                    {
                        var ExistingObj = ucamContext.ExamHeldInAndProgramRelations.Find(receiveObj.Id);
                        if (ExistingObj != null)
                        {
                            ExistingObj.ExamHeldInId = receiveObj.ExamHeldInId;
                            ExistingObj.YearNo = receiveObj.YearNo;
                            ExistingObj.SemesterNo = receiveObj.SemesterNo;
                            ExistingObj.ProgramId = receiveObj.ProgramId;
                            ExistingObj.IsActive = receiveObj.IsActive;
                            ExistingObj.IsDeleted = receiveObj.IsDeleted;
                            ExistingObj.ModifiedBy = -12;
                            ExistingObj.ModifiedDate = DateTime.Now;


                            ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                            Result = JsonConvert.SerializeObject(ExistingObj);
                            return Result;
                        }
                    }
                    else//Insert
                    {
                        DAL.ExamHeldInAndProgramRelation NewObj = new DAL.ExamHeldInAndProgramRelation();

                        NewObj.ExamHeldInId = receiveObj.ExamHeldInId;
                        NewObj.YearNo = receiveObj.YearNo;
                        NewObj.SemesterNo = receiveObj.SemesterNo;
                        NewObj.ProgramId = receiveObj.ProgramId;
                        NewObj.IsActive = receiveObj.IsActive;
                        NewObj.IsDeleted = false;
                        NewObj.CreatedBy = -12;
                        NewObj.CreatedDate = DateTime.Now;

                        ucamContext.ExamHeldInAndProgramRelations.Add(NewObj);
                        ucamContext.SaveChanges();

                        Result = JsonConvert.SerializeObject(NewObj);
                        return Result;

                    }
                }

            }
            catch (Exception ex)
            {
            }

            return Result;

        }

        [WebMethod]
        public static string SaveUpdateExamCommittees(string receiveListString)
        {
            string Result = "";
            try
            {
                var DataList = JsonConvert.DeserializeObject<List<DAL.ExamSetupWithExamCommittee>>(receiveListString);

                DAL.ExamSetupWithExamCommittee receiveObj = DataList[0];

                if (receiveObj != null)
                {
                    if (receiveObj.ID > 0)//Update
                    {
                        var ExistingObj = ucamContext.ExamSetupWithExamCommittees.Find(receiveObj.ID);
                        if (ExistingObj != null)
                        {

                            ExistingObj.ExamCommitteeChairmanDeptId = receiveObj.ExamCommitteeChairmanDeptId;
                            ExistingObj.ExamCommitteeChairmanId = receiveObj.ExamCommitteeChairmanId;

                            ExistingObj.ExamCommitteeMemberOneDeptId = receiveObj.ExamCommitteeMemberOneDeptId;
                            ExistingObj.ExamCommitteeMemberOneId = receiveObj.ExamCommitteeMemberOneId;

                            ExistingObj.ExamCommitteeMemberTwoDeptId = receiveObj.ExamCommitteeMemberTwoDeptId;
                            ExistingObj.ExamCommitteeMemberTwoId = receiveObj.ExamCommitteeMemberTwoId;

                            ExistingObj.ExamCommitteeExternalMemberDeptId = receiveObj.ExamCommitteeExternalMemberDeptId;
                            ExistingObj.ExamCommitteeExternalMemberId = receiveObj.ExamCommitteeExternalMemberId;

                            ExistingObj.ExamCommitteeMemberThreeDeptId = receiveObj.ExamCommitteeMemberThreeDeptId;
                            ExistingObj.ExamCommitteeMemberThreeId = receiveObj.ExamCommitteeMemberThreeId;

                            ExistingObj.ModifiedBy = -12;
                            ExistingObj.ModifiedDate = DateTime.Now;


                            ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                            ucamContext.SaveChanges();

                            Result = JsonConvert.SerializeObject(ExistingObj);
                            return Result;
                        }
                    }
                    else//Insert
                    {
                        DAL.ExamSetupWithExamCommittee NewObj = new DAL.ExamSetupWithExamCommittee();

                        NewObj.HeldInProgramRelationId = receiveObj.HeldInProgramRelationId;

                        NewObj.ExamCommitteeChairmanDeptId = receiveObj.ExamCommitteeChairmanDeptId;
                        NewObj.ExamCommitteeChairmanId = receiveObj.ExamCommitteeChairmanId;

                        NewObj.ExamCommitteeMemberOneDeptId = receiveObj.ExamCommitteeMemberOneDeptId;
                        NewObj.ExamCommitteeMemberOneId = receiveObj.ExamCommitteeMemberOneId;

                        NewObj.ExamCommitteeMemberTwoDeptId = receiveObj.ExamCommitteeMemberTwoDeptId;
                        NewObj.ExamCommitteeMemberTwoId = receiveObj.ExamCommitteeMemberTwoId;

                        NewObj.ExamCommitteeExternalMemberDeptId = receiveObj.ExamCommitteeExternalMemberDeptId;
                        NewObj.ExamCommitteeExternalMemberId = receiveObj.ExamCommitteeExternalMemberId;

                        NewObj.ExamCommitteeMemberTwoDeptId = receiveObj.ExamCommitteeMemberThreeDeptId;
                        NewObj.ExamCommitteeMemberThreeId = receiveObj.ExamCommitteeMemberThreeId;


                        NewObj.CreatedBy = -12;
                        NewObj.CreatedDate = DateTime.Now;

                        ucamContext.ExamSetupWithExamCommittees.Add(NewObj);
                        ucamContext.SaveChanges();

                        Result = JsonConvert.SerializeObject(NewObj);
                        return Result;

                    }
                }
            }
            catch (Exception ex)
            {
            }

            return "";
        }

        [WebMethod]
        public static string GetExamList(int programId, int yearNo, int semesterNo, int shal, int sessionId)
        {

            List<SqlParameter> parameters1 = new List<SqlParameter>();
            parameters1.Add(new SqlParameter { ParameterName = "@programId", SqlDbType = System.Data.SqlDbType.Int, Value = programId });
            parameters1.Add(new SqlParameter { ParameterName = "@yearNo", SqlDbType = System.Data.SqlDbType.Int, Value = yearNo });
            parameters1.Add(new SqlParameter { ParameterName = "@semesterNo", SqlDbType = System.Data.SqlDbType.Int, Value = semesterNo });
            parameters1.Add(new SqlParameter { ParameterName = "@shal", SqlDbType = System.Data.SqlDbType.Int, Value = shal });
            parameters1.Add(new SqlParameter { ParameterName = "@sessionId", SqlDbType = System.Data.SqlDbType.Int, Value = sessionId });


            DataTable dt = DataTableManager.GetDataFromQuery("GetAllHeldInAndProgramRelationData", parameters1);

            if (dt != null && dt.Rows.Count > 0)
            {
                string Result = DataTableMethods.DataTableToJsonConvert(dt);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetFacultyByDeptId(int DeptId)
        {
            List<Employee> empList = EmployeeManager.GetAllByTypeId(1);

            if (DeptId != 0)
            {
                empList = empList.Where(x => x.DeptID == DeptId).ToList();
            }

            if (empList != null && empList.Any())
            {
                List<EmployeeList> newlist = new List<EmployeeList>();
                foreach (var item in empList)
                {
                    EmployeeList newObj = new EmployeeList();

                    newObj.EmployeeId = item.EmployeeID;
                    newObj.CodeAndName = item.CodeAndName;

                    newlist.Add(newObj);

                }

                string Result = JsonConvert.SerializeObject(newlist);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static string GetExamByRelationId(int RelationId)
        {
            try
            {
                var ExistingObj = ucamContext.ExamHeldInAndProgramRelations.Find(RelationId);
                if (ExistingObj != null)
                {
                    string Result = JsonConvert.SerializeObject(ExistingObj);
                    return Result;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public static string GetExamCommitteeByRelationId(int RelationId)
        {
            try
            {
                var ExistingObj = ucamContext.ExamSetupWithExamCommittees.Where(x => x.HeldInProgramRelationId == RelationId).FirstOrDefault();
                if (ExistingObj != null)
                {
                    string Result = JsonConvert.SerializeObject(ExistingObj);
                    return Result;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [WebMethod]
        public static string GetExternalDept()
        {

            var ExternalDeptList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Select(x => x.Department).Distinct().ToList();

            if (ExternalDeptList != null && ExternalDeptList.Any())
            {
                string Result = JsonConvert.SerializeObject(ExternalDeptList);
                return Result;
            }
            else
                return null;
        }
        [WebMethod]
        public static string GetExternalFacultyByDeptId(string Dept)
        {
            List<DAL.ExternalCommitteeMemberInformation> ExternalEmpList = new List<DAL.ExternalCommitteeMemberInformation>();
            if (Dept == "0")
                ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().ToList();
            else
                ExternalEmpList = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Where(x => x.Department == Dept).ToList();

            if (ExternalEmpList != null && ExternalEmpList.Any())
            {

                string Result = JsonConvert.SerializeObject(ExternalEmpList);
                return Result;
            }
            else
                return null;
        }
        [WebMethod]
        public static string GetExternalFacultyById(int ExternalId)
        {

            var ExistingObj = ucamContext.ExternalCommitteeMemberInformations.Find(ExternalId);
            if (ExistingObj != null)
            {
                string Result = JsonConvert.SerializeObject(ExistingObj);
                return Result;
            }
            else
                return null;
        }

        [WebMethod]
        public static int GetPageUserID()
        {
            int userID = RoleId;
            return userID;
        }

        public class EmployeeList
        {
            public string CodeAndName { get; set; }
            public int EmployeeId { get; set; }

        }

        #endregion
    }
}