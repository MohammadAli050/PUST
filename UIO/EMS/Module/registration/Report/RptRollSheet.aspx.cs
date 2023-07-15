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

namespace EMS.Module.registration.Report
{
    public partial class RptRollSheet : BasePage
    {

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
            }
        }

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

        private void ClearReportView()
        {
            try
            {
                ReportViewer1.LocalReport.DataSources.Clear();
                //ReportViewer1.Visible = false;

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region OnLoad Methods
        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);

                DataTable DataTableHeldInList = CommonMethodsForGetHeldIn.GetExamHeldInInformation(ProgramId, 0, 0, 0);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "RelationId";
                    ddlHeldIn.DataSource = DataTableHeldInList;
                    ddlHeldIn.DataBind();

                }

                ddlHeldIn.Items.Insert(0, new ListItem("Select", "0"));
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
                ddlHallInfo.Items.Add(new ListItem("All", "0"));

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

        protected void btnRegistrationSheet_Click(object sender, EventArgs e)
        {
            try
            {
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetAllRegisteredStudentListByHeldInId", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ReportParameter p1 = new ReportParameter("Dept", ucDepartment.selectedText.ToString());
                    ReportParameter p2 = new ReportParameter("HeldIn", ddlHeldIn.SelectedItem.ToString());

                    ReportViewer1.Visible = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/registration/Report/RptRegistrationSheet.rdlc");
                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);

                    ReportViewer1.LocalReport.DisplayName = "RegistrationSheet";
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
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

        protected void btnRollSheet_Click(object sender, EventArgs e)
        {
            try
            {
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int HallId = Convert.ToInt32(ddlHallInfo.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });
                parameters1.Add(new SqlParameter { ParameterName = "@HallId", SqlDbType = System.Data.SqlDbType.Int, Value = HallId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetAllRollSheetStudentListByHeldInId", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
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



                    ReportViewer1.Visible = true;
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/registration/Report/RptRollSheet.rdlc");
                    ReportViewer1.LocalReport.EnableExternalImages = true;
                    this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6 });
                    ReportDataSource rds = new ReportDataSource("DataSet1", dt);

                    ReportViewer1.LocalReport.DisplayName = "RollSheet";
                    ReportViewer1.LocalReport.DataSources.Clear();
                    ReportViewer1.LocalReport.DataSources.Add(rds);
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

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }
    }
}