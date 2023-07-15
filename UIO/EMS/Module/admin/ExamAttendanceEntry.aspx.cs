using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ExamAttendanceEntry : BasePage
    {

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {

            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownListFromExamCommitteeWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();
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
                ClearGridView();
                ClearReport();
            }
            catch (Exception)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadCourse();
                LoadHeldInInformation();
                ClearGridView();
                ClearReport();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourse();
            ClearGridView();
            ClearReport();
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            ClearReport();
        }

        private void ClearGridView()
        {
            try
            {
                gvStudentList.DataSource = null;
                gvStudentList.DataBind();
            }
            catch (Exception ex)
            {
            }
        }
        private void ClearReport()
        {
            ReportViewer1.LocalReport.DataSources.Clear();
        }


        protected void LoadCourse()
        {
            try
            {
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0"));
                ddlCourse.AppendDataBoundItems = true;


                int HeldInRelationId = 0;
                HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (HeldInRelationId > 0)
                {
                    BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                    List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetAllByHeldInRelationId(HeldInRelationId);
                    //User user = UserManager.GetByLogInId(userObj.LogInID);
                    //Role userRole = RoleManager.GetById(user.RoleID);

                    //if (user.Person != null)
                    //{
                    //    Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);

                    //    if (empObj != null && (userRole.RoleName != "IT Admin" || userRole.RoleName != "ESCL"))
                    //    {
                    //        acaCalSectionList = acaCalSectionList.Where(x => x.TeacherOneID == empObj.EmployeeID || x.TeacherThreeID == empObj.EmployeeID || x.TeacherTwoID == empObj.EmployeeID).ToList();
                    //    }
                    //}

                    if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                    {
                        acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                        foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                        {
                            ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + " : " + acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.AcaCal_SectionID.ToString()));
                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex) { }
        }


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                ClearReport();
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int AcacalSectionId = Convert.ToInt32(ddlCourse.SelectedValue);

                if (HeldInId == 0)
                {
                    showAlert("Please select a Load schedule for");
                    return;
                }
                if (AcacalSectionId == 0)
                {
                    showAlert("Please select a course");
                    return;
                }

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetStudentListBySectionIdWithExamAttendance", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvStudentList.DataSource = dt;
                    gvStudentList.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void lnkAttendanceSave_Click(object sender, EventArgs e)
        {
            try
            {
                int UpdateCounter = 0;

                foreach (GridViewRow row in gvStudentList.Rows)
                {

                    Label lblSetupId = (Label)row.FindControl("lblSetupId");
                    Label lblHistoryId = (Label)row.FindControl("lblHistoryId");

                    CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");

                    try
                    {
                        int SetupId = Convert.ToInt32(lblSetupId.Text);

                        int HistoryId = Convert.ToInt32(lblHistoryId.Text);

                        int Status = chkStatus.Checked ? 1 : 0;

                        if (SetupId > 0) // Update existing entry
                        {
                            var ExistingObj = ucamContext.ExamAttendanceInformations.Find(SetupId);
                            if (SetupId > 0)
                            {
                                ExistingObj.PresentAbsentStatus = Status;
                                ExistingObj.ModifiedBy = UserObj.Id;
                                ExistingObj.ModifedDate = DateTime.Now;

                                ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                UpdateCounter++;
                            }
                        }
                        else // insert a new entry
                        {
                            DAL.ExamAttendanceInformation NewObj = new DAL.ExamAttendanceInformation();

                            NewObj.CourseHistoryId = HistoryId;
                            NewObj.PresentAbsentStatus = Status;
                            NewObj.CreatedBy = UserObj.Id;
                            NewObj.CreatedDate = DateTime.Now;

                            ucamContext.ExamAttendanceInformations.Add(NewObj);
                            ucamContext.SaveChanges();

                            if (NewObj.Id > 0)
                                UpdateCounter++;
                        }

                    }
                    catch (Exception ex)
                    {
                    }

                }

                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);

                if (UpdateCounter > 0)
                {
                    showAlert("Attendance Update Successfully");
                    btnLoad_Click(null, null);
                    return;
                }
                else
                {
                    showAlert("Attendance Update Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void btnTopSheet_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                ClearReport();
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int AcacalSectionId = Convert.ToInt32(ddlCourse.SelectedValue);

                if (HeldInId == 0)
                {
                    showAlert("Please select a Load schedule for");
                    return;
                }
                if (AcacalSectionId == 0)
                {
                    showAlert("Please select a course");
                    return;
                }



                #region Exam Attendance Checking

                List<SqlParameter> parametersN = new List<SqlParameter>();
                parametersN.Add(new SqlParameter { ParameterName = "@AcaCalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                DataTable dtAttendanceObj = DataTableManager.GetDataFromQuery("GetExamAttendanceCount", parametersN);

                if (dtAttendanceObj != null)
                {
                    int Result = 0;

                    Result = Convert.ToInt32(dtAttendanceObj.Rows[0]["Result"]);

                    // 2 = Attendance Not Given, 3 = Partially Given

                    if (Result == 2)
                    {
                        showAlert("এই course এর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক Exam Attendance Save করুন");
                        return;
                    }
                    else if (Result == 3)
                    {
                        showAlert("এই course এর কিছু শিক্ষার্থীর Exam Attendance এখনও দেয়া হয় নি। অনুগ্রহপূর্বক Exam Attendance Save করুন");
                        return;
                    }
                }

                #endregion


                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetTopSheetStudentListBySectionId", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string FormalCode = "";

                    AcademicCalenderSection acsObj = AcademicCalenderSectionManager.GetById(AcacalSectionId);

                    if (acsObj != null)
                    {
                        FormalCode = acsObj.Course.FormalCode + "_" + acsObj.Course.Title;
                    }

                    #region Held In Information

                    string ExamYear = "", HeldInMonth = "", Session = "";
                    string HeldInName = MisscellaneousCommonMethods.GetHeldInName(HeldInId);

                    var HeldInRelationObj = ucamContext.ExamHeldInAndProgramRelations.Find(HeldInId);
                    if (HeldInRelationObj != null)
                    {
                        try
                        {
                            var HeldInObj = ucamContext.ExamHeldInInformations.Find(HeldInRelationObj.ExamHeldInId);
                            if (HeldInObj != null)
                            {
                                ExamYear = HeldInObj.Year;
                                Session = HeldInObj.Session;
                                if (HeldInObj.HeldInStartYear != HeldInObj.HeldInEndYear)
                                    HeldInMonth = HeldInObj.HeldInStartMonth + " " + HeldInObj.HeldInStartYear + " - " + HeldInObj.HeldInEndMonth + " " + HeldInObj.HeldInEndYear;
                                else
                                    HeldInMonth = HeldInObj.HeldInStartMonth + " - " + HeldInObj.HeldInEndMonth + " " + HeldInObj.HeldInEndYear;
                            }
                        }
                        catch (Exception ex)
                        {
                        }

                    }

                    #endregion


                    ReportParameter p1 = new ReportParameter("Dept", ucDepartment.selectedText.ToString());
                    ReportParameter p2 = new ReportParameter("HeldIn", ddlHeldIn.SelectedItem.ToString());
                    ReportParameter p3 = new ReportParameter("HeldInName", HeldInName);
                    ReportParameter p4 = new ReportParameter("ExamYear", ExamYear);
                    ReportParameter p5 = new ReportParameter("HeldInMonth", HeldInMonth);
                    ReportParameter p6 = new ReportParameter("Session", Session);
                    ReportParameter p7 = new ReportParameter("FormalCode", FormalCode);



                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/admin/RptExamAttendanceTopSheet.rdlc");


                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);

                    ReportViewer1.LocalReport.DisplayName = "TopSheet_" + FormalCode;
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);

                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}