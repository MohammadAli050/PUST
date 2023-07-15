using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.result.Report
{
    public partial class RptConsolidatedMarksSheet : BasePage
    {
        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

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
                    if (Request.QueryString["d"].ToString() != "")
                    {
                        if (Request.QueryString["p"].ToString() != "")
                        {
                            if (Request.QueryString["h"].ToString() != "")
                            {
                                if (Request.QueryString["a"].ToString() != "")
                                {
                                    int departmentId = Convert.ToInt32(Request.QueryString["d"].ToString());
                                    ucDepartment.SelectedValue(departmentId);
                                    ucProgram.LoadDropdownByDepartmentId(departmentId);
                                    int programId = Convert.ToInt32(Request.QueryString["p"].ToString());
                                    ucProgram.SelectedValue(programId);
                                    LoadHeldInInformation();
                                    string heldInId = Request.QueryString["h"].ToString();
                                    ddlHeldIn.SelectedValue = heldInId;
                                    LoadCourse();
                                    string acaCalSecId = Request.QueryString["a"].ToString();
                                    ddlCourse.SelectedValue = acaCalSecId;
                                    divDD.Visible = false;
                                    btnLoadReport_Click(null, null);
                                }
                            }
                        }
                    }
                    else
                        base.CheckPage_Load();
                }
                catch (Exception ex)
                {
                    base.CheckPage_Load();
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
                ClearReportView();
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
                ClearReportView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadCourse();
            ClearReportView();
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearReportView();
        }

        private void ClearReportView()
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
            }
            catch (Exception ex)
            {
            }
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
                    User user = UserManager.GetByLogInId(userObj.LogInID);
                    Role userRole = RoleManager.GetById(user.RoleID);

                    if (user.Person != null)
                    {
                        Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);

                        if (empObj != null && (userRole.RoleName != "IT Admin" || userRole.RoleName != "ESCL"))
                        {
                            #region Check Logged In Person Is In Exam Committee

                            bool IsCommitteeMember = false;

                            var CommitteeObj = ucamContext.ExamSetupWithExamCommittees.Where(x => x.HeldInProgramRelationId == HeldInRelationId).FirstOrDefault();

                            if (CommitteeObj != null && (CommitteeObj.ExamCommitteeChairmanId == empObj.EmployeeID || CommitteeObj.ExamCommitteeMemberOneId == empObj.EmployeeID || CommitteeObj.ExamCommitteeMemberTwoId == empObj.EmployeeID))
                                IsCommitteeMember = true;

                            #endregion

                            if (!IsCommitteeMember)
                            {
                                // Get All Course By EmployeeId From Section Table(Course Teacher), Examiner Table(Internal Examiner), Tabulator Table(Tabulator 1,2,3)
                                List<SqlParameter> parameters1 = new List<SqlParameter>();
                                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                                parameters1.Add(new SqlParameter { ParameterName = "@EmployeeId", SqlDbType = System.Data.SqlDbType.Int, Value = empObj.EmployeeID });

                                DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetAllCourseListByEmployeeIdAndHeldInId", parameters1);
                                if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                                {
                                    ddlCourse.DataTextField = "CourseInfo";
                                    ddlCourse.DataValueField = "AcaCal_SectionID";
                                    ddlCourse.DataSource = DataTableCourseList;
                                    ddlCourse.DataBind();
                                }
                            }
                            else
                            {
                                if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                                {
                                    acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                                    foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                                    {
                                        ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + " : " + acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.AcaCal_SectionID.ToString()));
                                    }
                                }
                            }

                        }
                    }
                    else
                    {
                        if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                        {
                            acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                            foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                            {
                                ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + " : " + acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.AcaCal_SectionID.ToString()));
                            }
                        }
                    }
                }
                else
                {

                }
            }
            catch { }
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            try
            {
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

                DataTable dt = DataTableManager.GetDataFromQuery("GetExamMarkDetailsBySectionId", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    int CourseType = 0;

                    AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(AcacalSectionId);
                    if (acs != null)
                        CourseType = Convert.ToInt32(acs.Course.TypeDefinitionID);

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


                    List<SqlParameter> parameters2 = new List<SqlParameter>();
                    parameters2.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                    DataTable dtNew = DataTableManager.GetDataFromQuery("GetSectionCommitteeInformationBySectionId", parameters2);

                    ReportParameter p1 = new ReportParameter("Dept", ucDepartment.selectedText.ToString());
                    ReportParameter p2 = new ReportParameter("HeldIn", ddlHeldIn.SelectedItem.ToString());
                    ReportParameter p3 = new ReportParameter("CourseType", CourseType.ToString());
                    ReportParameter p4 = new ReportParameter("HeldInName", HeldInName);
                    ReportParameter p5 = new ReportParameter("ExamYear", ExamYear);
                    ReportParameter p6 = new ReportParameter("HeldInMonth", HeldInMonth);
                    ReportParameter p7 = new ReportParameter("Session", Session);

                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptConsolidatedMarksSheet.rdlc");


                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2,p3,p4,p5,p6,p7 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    ReportDataSource rds1 = new ReportDataSource("DataSet2", dtNew);

                    ReportViewer1.LocalReport.DisplayName = "100PerccentMarkShet";
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.DataSources.Add(rds1);


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
    }
}