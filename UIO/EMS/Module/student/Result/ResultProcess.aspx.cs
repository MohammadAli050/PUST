using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace EMS.Module.student.Result
{
    public partial class ResultProcess : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                //lblMsg.Text = string.Empty;
                if (userObj.RoleID != 1 && userObj.RoleID != 2)
                {
                    UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userObj.Id);
                    if (uapObj != null)
                    {
                        ucDepartment.LoadDropdownWithUserAccess(userObj.Id);
                        ucProgram.LoadDropdownWithUserAccess(userObj.Id);
                        ucProgram.SelectedIndex(1);
                        ucDepartment.SelectedIndex(1);
                    }
                }
                else
                {
                    ucDepartment.LoadDropDownList();
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    ucProgram.LoadDropdownByDepartmentId(departmentId);
                }
                ucAdmissionSession.LoadDropDownList();
                LoadYearNoDDL();
                LoadSemesterNoDDL();
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
           // lblMsg.Text = string.Empty;
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
            //lblMsg.Text = string.Empty;
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
                ddlYearNo.DataTextField = "YearNo";
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
                ddlSemesterNo.DataTextField = "SemesterNo";
                ddlSemesterNo.DataValueField = "SemesterNo";

                ddlSemesterNo.DataSource = semesterList;
                ddlSemesterNo.DataBind();

            }
        }

        protected void ddlYearNo_SelectedIndexChanged(object sender, EventArgs e)
        {
           // lblMsg.Text = string.Empty;
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
            //lblMsg.Text = string.Empty;
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
          //  lblMsg.Text = string.Empty;
            //try
            //{
            //    btnLoad_Click(null, null);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }


        [System.Web.Services.WebMethod]
        public static string ExecuteResultProcess(string ProgramId, string ExamId)
        {

            int programId = Convert.ToInt32(ProgramId);
            int examId = Convert.ToInt32(ExamId);
          
            // DataTable dataTable = new DataTable();
            //// string connString = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString; 
            //string query = "GetResultSheetData";

            List<SqlParameter> parameterListV3 = new List<SqlParameter>();
            parameterListV3.Add(new SqlParameter { ParameterName = "ProgramId", SqlDbType = SqlDbType.Int, Value = programId });
            parameterListV3.Add(new SqlParameter { ParameterName = "ExamDetailId", SqlDbType = SqlDbType.Int, Value = examId });

         
            string spNameV3 = "StudentResultProcessV2";

            string connStrV3 = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString;

            DataTable dtV3 = new DataTable();
            using (SqlConnection connection = new SqlConnection(connStrV3))
            {
                SqlCommand command = new SqlCommand(spNameV3, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                foreach (var item in parameterListV3)
                {
                    command.Parameters.Add(item);
                }

                try
                {
                    connection.Open();
                    SqlDataReader reader;
                    reader = command.ExecuteReader();
                    dtV3.Load(reader);
                }
                finally
                {
                    connection.Close();
                }
            }

           
            string json = JsonConvert.SerializeObject(dtV3, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;

        }

    }
}