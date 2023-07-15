using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.RO;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.student.Report
{
    public partial class RptStudentList : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        BussinessObject.UIUMSUser userObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                if (userObj != null)
                {
                    ucDepartment.LoadDropDownList();
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    ucProgram.LoadDropdownByDepartmentId(departmentId);
                    ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    LoadYear(programId);
                    LoadYearSemester(0);
                }
            }
        }

        private void LoadYear(int programId)
        {
            List<Year> yearList = new List<Year>();
            yearList = YearManager.GetByProgramId(programId);

            ddlYear.Items.Clear();
            ddlYear.AppendDataBoundItems = true;

            if (yearList != null)
            {
                ddlYear.Items.Add(new ListItem("-Select-", "0"));
                ddlYear.DataTextField = "YearName";
                ddlYear.DataValueField = "YearId";
                if (yearList != null)
                {
                    ddlYear.DataSource = yearList.OrderBy(b => b.YearId).ToList();
                    ddlYear.DataBind();
                }
            }
        }

        private void LoadYearSemester(int yearId)
        {
            List<Semester> semesterList = new List<Semester>();
            semesterList = SemesterManager.GetByYearId(yearId);

            ddlSemester.Items.Clear();
            ddlSemester.AppendDataBoundItems = true;

            if (semesterList != null)
            {
                ddlSemester.Items.Add(new ListItem("-Select-", "0"));
                ddlSemester.DataTextField = "SemesterName";
                ddlSemester.DataValueField = "SemesterId";
                if (semesterList != null)
                {
                    ddlSemester.DataSource = semesterList.OrderBy(b => b.SemesterId).ToList();
                    ddlSemester.DataBind();
                }
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Visible = false;
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYear(programId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            
            try
            {
                ReportViewer1.Visible = false;
                ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYear(programId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ReportViewer1.Visible = false;
                int yearId = Convert.ToInt32(ddlYear.SelectedValue);
                LoadYearSemester(yearId);
            }
            catch (Exception)
            { }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int admSessionId = Convert.ToInt32(ucSession.selectedValue);
                int yearId = Convert.ToInt32(ddlYear.SelectedValue);
                int semesterId = Convert.ToInt32(ddlSemester.SelectedValue);

                if (programId != 0 && yearId != 0 && semesterId != 0)
                {
                    List<rStudentList> list = StudentManager.GetStudentListByProgramAndYearOrSemester(programId, admSessionId, yearId, semesterId);

                    if (list != null && list.Count > 0)
                    {
                        ucSession.LoadDropDownList();
                        string currentSession = list.Select(d => d.CurrentSession).FirstOrDefault();
                        int currentSessionId = list.Select(d => d.CurrentSessionId).FirstOrDefault();
                        if (currentSessionId > 0 )
                        {
                            ucSession.SelectedValue(currentSessionId);
                        }

                        ReportParameter p1 = new ReportParameter("Program", ucProgram.selectedText.ToString());
                        ReportParameter p2 = new ReportParameter("Year", ddlYear.SelectedItem.Text.ToString());
                        ReportParameter p3 = new ReportParameter("Semester", ddlSemester.SelectedItem.Text.ToString());
                        ReportParameter p4 = new ReportParameter("Count", list.Count().ToString());
                        ReportParameter p5 = new ReportParameter("AdmissionSession", currentSession.ToString());

                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/student/Report/RptStudentList.rdlc");
                        this.ReportViewer1.LocalReport.EnableExternalImages = true;
                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5 });

                        ReportDataSource rds = new ReportDataSource("DataSet1", list);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);
                        ReportViewer1.Visible = true;
                        ShowMessage("");
                    }
                    else
                    {
                        ShowAlertMessage("No Data Found!");
                        ShowMessage("No Data Found!");
                        ReportViewer1.Visible = false;
                    }
                }
                else
                {
                    ShowAlertMessage("Please select Program, Year and Semester");
                    ShowMessage("Please select Program, Year and Semester");
                    ReportViewer1.Visible = false;
                }



            }
            catch (Exception)
            {
                ShowAlertMessage("Error Occcured!");
                ShowMessage("Error Occcured!");
                ReportViewer1.Visible = false;
            }
        }

        private void ShowMessage(string msg)
        {
            lblMessage.Text = msg;
            lblMessage.ForeColor = Color.Red;
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }
    }
}