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


namespace EMS.Module.student.Result
{
    public partial class ResultSheet : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                //lblMsg.Text = string.Empty;
                if (userObj.RoleID != 1 && userObj.RoleID != 8)
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
        public static string GetResultSheetData(string AcaCalId, string YearId, string SemesterId, string ExamType)
        {

            int acaCalId = Convert.ToInt32(AcaCalId);
            int yearId = Convert.ToInt32(YearId);
            int semesterId = Convert.ToInt32(SemesterId);
            int examTypeId = Convert.ToInt32(ExamType);

            //Year y = YearManager.GetById(yearId);
            //Semester s = SemesterManager.GetById(semesterId);

           // DataTable dataTable = new DataTable();
           //// string connString = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString; 
            //string query = "GetResultSheetData";

            List<SqlParameter> parameterListV3 = new List<SqlParameter>();
            parameterListV3.Add(new SqlParameter { ParameterName = "ExamId", SqlDbType = SqlDbType.Int, Value = acaCalId });

            parameterListV3.Add(new SqlParameter { ParameterName = "YearNo", SqlDbType = SqlDbType.Int, Value = yearId });
            parameterListV3.Add(new SqlParameter { ParameterName = "SemesterNo", SqlDbType = SqlDbType.Int, Value = semesterId });
            parameterListV3.Add(new SqlParameter { ParameterName = "ExamType", SqlDbType = SqlDbType.Int, Value = examTypeId });

            string spNameV3 = "GetResultSheetData";

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

            //------------------------------------------------------------------------------

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
            string json = JsonConvert.SerializeObject(dtV3, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
         

            return json;

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
        //private void LoadYearDDL(int programId)
        //{
        //    List<Year> yearList = new List<Year>();
        //    yearList = YearManager.GetByProgramId(programId);

        //    ddlYear.Items.Clear();
        //    ddlYear.AppendDataBoundItems = true;

        //    if (yearList != null)
        //    {
        //        ddlYear.Items.Add(new ListItem("-Select-", "0"));
        //        ddlYear.DataTextField = "YearName";
        //        ddlYear.DataValueField = "YearId";
        //        if (yearList != null)
        //        {
        //            ddlYear.DataSource = yearList.OrderBy(b => b.YearId).ToList();
        //            ddlYear.DataBind();
        //        }
        //    }
        //}

        //private void LoadYearSemesterDDL(int yearId)
        //{
        //    List<Semester> semesterList = new List<Semester>();
        //    semesterList = SemesterManager.GetByYearId(yearId);

        //    ddlSemester.Items.Clear();
        //    ddlSemester.AppendDataBoundItems = true;

        //    if (semesterList != null)
        //    {
        //        ddlSemester.Items.Add(new ListItem("-Select-", "0"));
        //        ddlSemester.DataTextField = "SemesterName";
        //        ddlSemester.DataValueField = "SemesterId";
        //        if (semesterList != null)
        //        {
        //            ddlSemester.DataSource = semesterList.OrderBy(b => b.SemesterId).ToList();
        //            ddlSemester.DataBind();
        //        }
        //    }
        //}




    }
}