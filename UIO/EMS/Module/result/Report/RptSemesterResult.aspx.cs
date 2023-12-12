using CommonUtility;
using LogicLayer.BusinessLogic;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.ClientServices;
using System.Web.Services;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace EMS.Module.result.Report
{
    public partial class RptSemesterResult : BasePage
    {
        BussinessObject.UIUMSUser UserObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();
                divDD.Visible = true;
                try
                {
                    if (Request.QueryString.AllKeys.Contains("d") == true)
                    {
                        if (Request.QueryString.AllKeys.Contains("p") == true)
                        {
                            if (Request.QueryString.AllKeys.Contains("h") == true)
                            {
                                int departmentId = Convert.ToInt32(Request.QueryString["d"].ToString());
                                ucDepartment.SelectedValue(departmentId);
                                ucProgram.LoadDropdownByDepartmentId(departmentId);
                                int programId = Convert.ToInt32(Request.QueryString["p"].ToString());
                                ucProgram.SelectedValue(programId);
                                LoadHeldInInformation();
                                string heldInId = Request.QueryString["h"].ToString();
                                ddlHeldIn.SelectedValue = heldInId;
                                divDD.Visible = false;
                            }
                        }
                    }
                    else
                        base.CheckPage_Load();

                }
                catch (Exception ex)
                {
                }
            }
        }

        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);

                DataTable DataTableHeldInList = CommonMethodsForGetHeldIn.GetExamHeldInInformation(ProgramId, 0, 0, 0);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("Select", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "RelationId";
                    ddlHeldIn.DataSource = DataTableHeldInList;
                    ddlHeldIn.DataBind();

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
            }
            catch (Exception)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHeldInInformation();
            }
            catch (Exception ex)
            {
            }
        }

        public class SemesterResultList
        {
            public int ID { get; set; }
            public int StudentID { get; set; }
            public string Roll { get; set; }
            public string Reg { get; set; }
            public string FullName { get; set; }
            public string FatherName { get; set; }
            public string Hall { get; set; }
            public string FormalCode { get; set; }
            public string VersionCode { get; set; }
            public string Title { get; set; }
            public decimal CourseCredit { get; set; }
            public decimal ObtainedTotalMarks { get; set; }
            public string ObatainedGrade { get; set; }
            public decimal ObtainedGPA {get;set;}
            public decimal PointSecured { get;set;}
            public string SessionWithReAdd {get;set;}
            public int ReAddStatus {get;set;}
            public decimal TotalPointSecured {get;set;}
            public decimal CreaditTaken {get;set;}
            public decimal CreaditEarned {get;set; }
            public decimal GPA {get;set; }
            public decimal ContinuosAssesment {get;set; }
            public decimal FinalMark {get;set; }
        }

        [WebMethod]
        public static string LoadSemesterResultReport(string heldInID)
        {
            try {

                int HeldInId = Convert.ToInt32(heldInID);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetSemesterResultReportByHeldIn", parameters1);
                var list = DataTableMethods.DataTableToJsonConvert(dt);


                return list;
                //return null;
            }
            catch(Exception ex)
            {
                return null;
            }
            
            
        }        
    }
}