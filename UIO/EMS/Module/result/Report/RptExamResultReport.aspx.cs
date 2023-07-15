using LogicLayer.BusinessObjects;
using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using LogicLayer.BusinessObjects.DTO;


namespace EMS.Module.result.Report
{
    public partial class RptExamResultReport : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;
                ucDepartment.LoadDropdownWithUserAccess(userObj.Id);
                ucProgram.LoadDropdownWithUserAccess(userObj.Id);
                LoadCourse();
                LoadYearNoDDL();
                LoadSemesterNoDDL();
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
        }

        private void LoadYearNoDDL()
        {
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();


            ddlYearNo.Items.Clear();
            ddlYearNo.AppendDataBoundItems = true;
            ddlYearNo.Items.Add(new ListItem("-Select-", "-1"));
            if (yearList != null && yearList.Count > 0)
            {
                ddlYearNo.DataTextField = "YearNoName";
                ddlYearNo.DataValueField = "YearNo";

                ddlYearNo.DataSource = yearList;
                ddlYearNo.DataBind();
            }
        }

        private void LoadSemesterNoDDL()
        {
            List<SemesterDistinctDTO> semesterList = new List<SemesterDistinctDTO>();
            semesterList = SemesterManager.GetAllDistinct();
            semesterList = semesterList.OrderBy(x => x.SemesterNo).ToList();


            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.AppendDataBoundItems = true;
            ddlSemesterNo.Items.Add(new ListItem("-Select-", "-1"));
            if (semesterList != null && semesterList.Count > 0)
            {
                ddlSemesterNo.DataTextField = "SemesterNoName";
                ddlSemesterNo.DataValueField = "SemesterNo";

                ddlSemesterNo.DataSource = semesterList;
                ddlSemesterNo.DataBind();

            }
        }

        protected void ddlYearNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadExamDropdown(programId, yearNo, semesterNo);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ddlSemesterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadExamDropdown(programId, yearNo, semesterNo);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadExamDropdown(int programId, int yearNo, int semesterNo)
        {
            ddlExam.Items.Clear();
            ddlExam.Items.Add(new ListItem("-Select Exam-", "0"));
            ddlExam.AppendDataBoundItems = true;
            List<ExamSetupDetailDTO> exam = ExamSetupManager.ExamSetupDetailGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
            if (exam != null)
            {
                foreach (ExamSetupDetailDTO examlist in exam)
                {
                    ddlExam.Items.Add(new ListItem(examlist.ExamName, examlist.ExamSetupDetailId.ToString()));

                }
            }
        }

        protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadCourse();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void LoadCourse()
        {
            try
            {
                lblMsg.Text = string.Empty;
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0"));
                ddlCourse.AppendDataBoundItems = true;

                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                    List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetByProgramIdYearNoSemesterNoExamId(programId, yearNo, semesterNo, examId);
                    User user = UserManager.GetByLogInId(userObj.LogInID);
                    Role userRole = RoleManager.GetById(user.RoleID);
                    //if (user.Person != null)
                    //{
                    //    Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);

                    //    if (empObj != null && userRole.RoleName != "Admin")
                    //    {
                    //        acaCalSectionList = acaCalSectionList.Where(x => x.TeacherOneID == empObj.EmployeeID || x.TeacherThreeID == empObj.EmployeeID || x.TeacherTwoID == empObj.EmployeeID).ToList();
                    //    }
                    //}

                    if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                    {
                        acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                        foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                        {
                            ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + " : " + acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.Course.CourseID + "_" + acaCalSec.Course.VersionID + "_" + acaCalSec.AcaCal_SectionID));
                        }
                    }
                }
                else
                {
                    lblMsg.Text = "Please select program, yaer and semester.";
                }
            }
            catch { }
        }
     

        [System.Web.Services.WebMethod]
        public static string GetExamMarks(string courseSection)
        {
            string[] courseVersionSection = courseSection.Split('_');
            int courseId = Convert.ToInt32(courseVersionSection[0]);
            int versionId = Convert.ToInt32(courseVersionSection[1]);
            int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);

            List<ExamResultDTO> examResultDTOList = ExamTemplateManager.GetExamResultDTO(courseId, versionId, acaCalSectionId).ToList();
            List<ExamResultDTO> newList = new List<ExamResultDTO>();

            CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoObj = ExamMarkMasterManager.GetTeacherInfoAPIBySectionId(acaCalSectionId);

            var jsonData = new
            {
                list1 = examResultDTOList,
                list2 = countinousMarkTeacherInfoObj

            };

            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;
            

            // DataTable dataTable = new DataTable();
            //// string connString = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString; 
            //string query = "GetResultSheetData";

            //List<SqlParameter> parameterListV3 = new List<SqlParameter>();
            //parameterListV3.Add(new SqlParameter { ParameterName = "AcaCalId", SqlDbType = SqlDbType.Int, Value = AcaCalId });

            //parameterListV3.Add(new SqlParameter { ParameterName = "YearId", SqlDbType = SqlDbType.Int, Value = YearId });
            //parameterListV3.Add(new SqlParameter { ParameterName = "SemesterId", SqlDbType = SqlDbType.Int, Value = SemesterId });

            //string spNameV3 = "GetResultSheetData";

            //string connStrV3 = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString;

            //DataTable dtV3 = new DataTable();
            //using (SqlConnection connection = new SqlConnection(connStrV3))
            //{
            //    SqlCommand command = new SqlCommand(spNameV3, connection);
            //    command.CommandType = System.Data.CommandType.StoredProcedure;
            //    foreach (var item in parameterListV3)
            //    {
            //        command.Parameters.Add(item);
            //    }

            //    try
            //    {
            //        connection.Open();
            //        SqlDataReader reader;
            //        reader = command.ExecuteReader();
            //        dtV3.Load(reader);
            //    }
            //    finally
            //    {
            //        connection.Close();
            //    }
            //}

            ////------------------------------------------------------------------------------

            //SqlConnection conn = new SqlConnection(connString);
            //SqlCommand cmd = new SqlCommand(query, conn);
            //conn.Open();

            //List<SqlParameter> prm = new List<SqlParameter>()
            // {
            //     new SqlParameter("@AcaCalId", SqlDbType.Int) {Value = acaCalId},
            //     new SqlParameter("@YearId", SqlDbType.Int) {Value = yearId},
            //     new SqlParameter("@SemesterId", SqlDbType.Int) {Value = semesterId},
            // };
            //cmd.Parameters.AddRange(prm.ToArray());


            //// create data adapter
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //// this will query your database and return the result to your datatable
            //da.Fill(dataTable);
            //conn.Close();
            //da.Dispose();

            //List<DataRow> list = dataTable.AsEnumerable().ToList();
            //string json = JsonConvert.SerializeObject(dtV3, Formatting.Indented, new JsonSerializerSettings
            //{
            //    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            //});


            return "Hello";

        }

        //protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int programId = Convert.ToInt32(ucDepartment.selectedValue);
        //    LoadYearDDL(programId);
        //    //LoadTreeDDL(programId);
        //    TreeMaster treeObj = new TreeMaster();
        //    treeObj = TreeMasterManager.GetAllProgramID(programId).FirstOrDefault();
        //    if (treeObj != null)
        //    {
        //        LoadTreeCalender(treeObj.TreeMasterID);
        //    }
        //}

        //[System.Web.Services.WebMethod]
        //public static string GetTeacherInfoAPIBySectionId(string courseSection)
        //{ 
        //    string[] courseVersionSection = courseSection.Split('_');
        //    int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);

        //    CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoObj = ExamMarkMasterManager.GetTeacherInfoAPIBySectionId(acaCalSectionId);

        //    string json = JsonConvert.SerializeObject(countinousMarkTeacherInfoObj, Formatting.Indented, new JsonSerializerSettings
        //    {
        //        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        //    });
        //    return json;
        //}

        public class ExamName
        {
            public string ExamNameString { set; get; }
        }

    }
}