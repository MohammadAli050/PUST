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


namespace EMS.Module.result.Report
{
    public partial class RptTabulationSheetV2 : BasePage
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
                                btnLoadReport_Click(null, null);
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


        protected void btnLoadReport_Click(object sender, EventArgs e)
        {
            try
            {
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (HeldInId == 0)
                {
                    showAlert("Please select a Load schedule for");
                    return;
                }

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetTabulationSheetDataByHeldInIdNew", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    #region Held In Information

                    DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

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

                    int APlus = 0, A = 0, AMinus = 0, BPlus = 0, B = 0, BMinus = 0, CPlus = 0, C = 0, D = 0, F = 0;


                    try
                    {
                        #region Get Grade Wise Student Count

                        DataTable dtResult = dt.DefaultView.ToTable(true, "Roll", "GPA");

                        string Expression = "GPA  >='" + "0" + "' AND GPA <'" + "2.00" + "'";

                        DataTable dtF = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtF != null && dtF.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtF.DefaultView.ToTable(true, "Roll");
                            F = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "2.00" + "' AND GPA <'" + "2.25" + "'";
                        DataTable dtD = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtD != null && dtD.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtD.DefaultView.ToTable(true, "Roll");
                            D = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "2.25" + "' AND GPA <'" + "2.50" + "'";
                        DataTable dtC = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtC != null && dtC.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtC.DefaultView.ToTable(true, "Roll");
                            C = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "2.50" + "' AND GPA <'" + "2.75" + "'";
                        DataTable dtCPlus = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtCPlus != null && dtCPlus.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtCPlus.DefaultView.ToTable(true, "Roll");
                            CPlus = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "2.75" + "' AND GPA <'" + "3.00" + "'";
                        DataTable dtBMinus = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtBMinus != null && dtBMinus.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtBMinus.DefaultView.ToTable(true, "Roll");
                            BMinus = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "3.00" + "' AND GPA <'" + "3.25" + "'";
                        DataTable dtB = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtB != null && dtB.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtB.DefaultView.ToTable(true, "Roll");
                            B = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "3.25" + "' AND GPA <'" + "3.50" + "'";
                        DataTable dtBPlus = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtBPlus != null && dtBPlus.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtBPlus.DefaultView.ToTable(true, "Roll");
                            BPlus = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "3.50" + "' AND GPA <'" + "3.75" + "'";
                        DataTable dtAMinus = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtAMinus != null && dtAMinus.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtAMinus.DefaultView.ToTable(true, "Roll");
                            AMinus = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "3.75" + "' AND GPA <'" + "4.00" + "'";
                        DataTable dtA = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtA != null && dtA.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtA.DefaultView.ToTable(true, "Roll");
                            A = distinctStudents.Rows.Count;
                        }

                        Expression = "GPA  >='" + "4.00" + "'";
                        DataTable dtAPlus = DataTableMethods.FilterDataTable(dtResult, Expression);
                        if (dtAPlus != null && dtAPlus.Rows.Count > 0)
                        {
                            DataTable distinctStudents = dtAPlus.DefaultView.ToTable(true, "Roll");
                            APlus = distinctStudents.Rows.Count;
                        }

                        #endregion
                    }
                    catch (Exception ex)
                    {
                    }



                    //int CourseType = 0;

                    //AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(AcacalSectionId);
                    //if (acs != null)
                    //    CourseType = Convert.ToInt32(acs.Course.TypeDefinitionID);


                    //List<SqlParameter> parameters2 = new List<SqlParameter>();
                    //parameters2.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                    //DataTable dtNew = DataTableManager.GetDataFromQuery("GetSectionCommitteeInformationBySectionId", parameters2);

                    ReportParameter p1 = new ReportParameter("Dept", ucDepartment.selectedText.ToString());
                    ReportParameter p2 = new ReportParameter("HeldIn", ddlHeldIn.SelectedItem.ToString());
                    ReportParameter p3 = new ReportParameter("HeldInName", HeldInName);
                    ReportParameter p4 = new ReportParameter("ExamYear", ExamYear);
                    ReportParameter p5 = new ReportParameter("HeldInMonth", HeldInMonth);
                    ReportParameter p6 = new ReportParameter("Session", Session);

                    ReportParameter p7 = new ReportParameter("APlus", APlus.ToString());
                    ReportParameter p8 = new ReportParameter("A", A.ToString());
                    ReportParameter p9 = new ReportParameter("AMinus", AMinus.ToString());
                    ReportParameter p10 = new ReportParameter("BPlus", BPlus.ToString());
                    ReportParameter p11 = new ReportParameter("B", B.ToString());
                    ReportParameter p12 = new ReportParameter("BMinus", BMinus.ToString());
                    ReportParameter p13 = new ReportParameter("CPlus", CPlus.ToString());
                    ReportParameter p14 = new ReportParameter("C", C.ToString());
                    ReportParameter p15 = new ReportParameter("D", D.ToString());
                    ReportParameter p16 = new ReportParameter("F", F.ToString());



                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptTabulationSheetV2.rdlc");


                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
                    //ReportDataSource rds1 = new ReportDataSource("DataSet2", dtNew);

                    ReportViewer1.LocalReport.DisplayName = "TabulationSheetData";
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
                    //ReportViewer1.LocalReport.DataSources.Add(rds1);


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