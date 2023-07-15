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
using System.Web.UI.WebControls;

namespace EMS.Module.AdmitCard
{
    public partial class RptStudentAdmitCard : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.RptStudentAdmitCard);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.RptStudentAdmitCard));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {


                ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHallInfo();
                LoadHeldInInformation();

                if (UserObj.RoleID == 4) // Student
                {
                    txtStudentId.Text = UserObj.LogInID.ToString();
                    btnLoad.Visible = false;
                    btnLoad_Click(null, null);
                }
                else
                {
                    btnLoad.Visible = true;
                }
            }
        }


        #region On Load Methods

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

        private void LoadHallInfo()
        {
            try
            {
                var HallList = ucamContext.HallInformations.Where(x => x.ActiveStatus == 1).ToList();

                ddlHallInfo.Items.Clear();
                ddlHallInfo.AppendDataBoundItems = true;
                ddlHallInfo.Items.Add(new ListItem("Select", "0"));

                if (HallList != null && HallList.Any())
                {
                    ddlHallInfo.DataTextField = "HallName";
                    ddlHallInfo.DataValueField = "Id";
                    ddlHallInfo.DataSource = HallList.OrderBy(x => x.HallName);
                    ddlHallInfo.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion


        #region On Selected Index Changed Methods

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
                LoadHeldInInformation();
                ClearReportView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearReportView();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHallInfo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearReportView();
            }
            catch (Exception ex)
            {
            }

        }

        #endregion

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearReportView();

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int StudentId = 0;
                int HallInfoId = Convert.ToInt32(ddlHallInfo.SelectedValue);

                if (!string.IsNullOrEmpty(txtStudentId.Text))
                {
                    string Roll = txtStudentId.Text.Trim();

                    var StudentObj = ucamContext.Students.Where(x => x.Roll == Roll).FirstOrDefault();

                    if (StudentObj != null)
                    {
                        StudentId = StudentObj.StudentID;
                    }
                }

                if (HeldInRelationId == 0)
                {
                    showAlert("Please Choose Semester & Held In");
                    return;
                }
                if (ProgramId == 0 && StudentId == 0)
                {
                    showAlert("Please Choose a program or enter a student Id");
                    return;
                }
                if (HallInfoId == 0)
                {
                    showAlert("Please Choose a Hall");
                    return;
                }

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });
                parameters1.Add(new SqlParameter { ParameterName = "@HallId", SqlDbType = System.Data.SqlDbType.Int, Value = HallInfoId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetAdmitCardDataByHeldInId", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    string Program = "";

                    #region Held In Information

                    string ExamYear = "", HeldInMonth = "", Session = "", ExamDate = "";
                    string HeldInName = MisscellaneousCommonMethods.GetHeldInName(HeldInRelationId);

                    var HeldInRelationObj = ucamContext.ExamHeldInAndProgramRelations.Find(HeldInRelationId);
                    if (HeldInRelationObj != null)
                    {
                        try
                        {
                            var HeldInObj = ucamContext.ExamHeldInInformations.Find(HeldInRelationObj.ExamHeldInId);
                            if (HeldInObj != null)
                            {
                                ExamYear = HeldInObj.Year;
                                Session = HeldInObj.Session;

                                #region Get Exam Date

                                List<SqlParameter> parameters2 = new List<SqlParameter>();
                                parameters2.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                                DataTable DataTableRoutineList = DataTableManager.GetDataFromQuery("GetMinimumExamDateOfProgramByHeldInId", parameters2);

                                if (DataTableRoutineList != null && DataTableRoutineList.Rows.Count > 0)
                                {
                                    ExamDate = DataTableRoutineList.Rows[0]["ExamDate"].ToString();
                                }

                                #endregion

                                //if (HeldInObj.ExamStartDate != null)
                                //    ExamDate = Convert.ToDateTime(HeldInObj.ExamStartDate).ToString("dd-MM-yyyy");

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

                    //var HeldInRelationObj = ucamContext.ExamHeldInAndProgramRelations.Find(HeldInRelationId);
                    //if (HeldInRelationObj != null)
                    //{
                    //    try
                    //    {
                    //        var HeldInObj = ucamContext.ExamHeldInInformations.Find(HeldInRelationObj.ExamHeldInId);

                    //        if (HeldInRelationObj.YearNo == 1)
                    //            Year = "1st";
                    //        else if (HeldInRelationObj.YearNo == 2)
                    //            Year = "2nd";
                    //        else if (HeldInRelationObj.YearNo == 3)
                    //            Year = "3rd";
                    //        else
                    //            Year = "4th";

                    //        if (HeldInRelationObj.SemesterNo == 1)
                    //            Semester = "1st";
                    //        else
                    //            Semester = "2nd";

                    //        if (HeldInObj != null)
                    //        {
                    //            ExamYear = HeldInObj.Year;

                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //    }
                    //}

                    Program = ucProgram.selectedText.ToString();


                    ReportParameter p1 = new ReportParameter("HeldInName", HeldInName);
                    ReportParameter p2 = new ReportParameter("ExamYear", ExamYear);
                    ReportParameter p3 = new ReportParameter("ExamDate", ExamDate);

                    string RgExpression = "RetakeNo ='" + 9 + "'";
                    DataTable RegularCourse = DataTableMethods.FilterDataTable(dt, RgExpression);

                    string BackExpression = "RetakeNo ='" + 11 + "'";
                    DataTable BackLogCourse = DataTableMethods.FilterDataTable(dt, BackExpression);

                    string SpecialExpression = "RetakeNo ='" + 13 + "'";
                    DataTable SpecialCourse = DataTableMethods.FilterDataTable(dt, SpecialExpression);

                    ReportViewer1.Visible = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/AdmitCard/RptStudentAdmitCard.rdlc");
                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    ReportDataSource rds2 = new ReportDataSource("DataSet2", BackLogCourse);
                    ReportDataSource rds3 = new ReportDataSource("DataSet3", SpecialCourse);


                    ReportViewer1.LocalReport.DisplayName = "AdmitCard";
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    ReportViewer1.LocalReport.DataSources.Add(rds2);
                    ReportViewer1.LocalReport.DataSources.Add(rds3);

                }
                else
                {
                    ClearReportView();
                    showAlert("No Data Found");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
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


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }
    }
}