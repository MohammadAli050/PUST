using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ExamMarkDateRangeSetup : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.ExamMarkDateRangeSetup);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.ExamMarkDateRangeSetup));

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
                LoadHeldInInformation();
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

        #endregion

        #region On Selected Index Changed Methods

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
                ClearGridView();
                LoadData();
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
                ClearGridView();
                LoadData();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                LoadData();
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private void LoadData()
        {
            try
            {
                ClearGridView();

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable DataTableRoutineList = DataTableManager.GetDataFromQuery("GetAllClassRoutineInformationWithMarkEntryDateTime", parameters1);

                if (DataTableRoutineList != null && DataTableRoutineList.Rows.Count > 0)
                {

                    gvScheduleList.DataSource = DataTableRoutineList;
                    gvScheduleList.DataBind();
                    //Session["ScheduleList"] = DataTableRoutineList;
                    //ViewState["sortdrSchedule"] = "Asc";
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void ClearGridView()
        {
            gvScheduleList.DataSource = null;
            gvScheduleList.DataBind();
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton linkButton = new LinkButton();
                linkButton = (LinkButton)sender;
                int AcaCal_SectionID = Convert.ToInt32(linkButton.CommandArgument);

                GridViewRow gvrow = (GridViewRow)(((LinkButton)sender)).NamingContainer;


                if (AcaCal_SectionID > 0)
                {


                    TextBox txtExamDate = (TextBox)gvrow.FindControl("txtExamDate");
                    TextBox CAStart = (TextBox)gvrow.FindControl("txtCAStartDate");
                    TextBox CAEnd = (TextBox)gvrow.FindControl("txtCAEndDate");
                    TextBox FinalStart = (TextBox)gvrow.FindControl("txtFinalStartDate");
                    TextBox FinalEnd = (TextBox)gvrow.FindControl("txtFinalEndDate");



                    string ReturnValue = InsertUpdateDateInformation(AcaCal_SectionID, txtExamDate, CAStart, CAEnd, FinalStart, FinalEnd);

                    string[] splittedValue = ReturnValue.Split('-');

                    if (splittedValue.Length > 0 && Convert.ToInt32(splittedValue[1]) > 0)
                    {
                        showAlert("100% marks submission done. No change will possible");
                        return;

                    }
                    else
                    {
                        showAlert("Date Updated Successfully");
                        LoadData();
                        return;
                    }


                    //var StatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == AcaCal_SectionID).FirstOrDefault();
                    //if (StatusObj == null || StatusObj.StatusId != 5)
                    //{

                    //}
                    //else
                    //{

                    //    txtExamDate.Text = "";
                    //    CAStart.Text = "";
                    //    CAEnd.Text = "";
                    //    FinalStart.Text = "";
                    //    FinalEnd.Text = "";

                    //    showAlert("100% marks submission done. No change will possible");
                    //    return;
                    //}


                }
            }
            catch (Exception ex)
            {
            }
        }

        private string InsertUpdateDateInformation(int AcaCal_SectionID, TextBox txtExamDate, TextBox CAStart, TextBox CAEnd, TextBox FinalStart, TextBox FinalEnd)
        {
            string ReturnValue = "0-0";
            try
            {
                int UpdateCount = 0, SubmissionCount = 0;

                var StatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == AcaCal_SectionID).FirstOrDefault();

                if (StatusObj == null || StatusObj.StatusId != 5)
                {
                    var ExistingObj = ucamContext.ExamMarkDateRangeSetups.Where(x => x.AcacalSectionId == AcaCal_SectionID).FirstOrDefault();

                    #region Update Process
                    if (ExistingObj != null) // Update Existing Entry 
                    {
                        if (txtExamDate.Text != "")
                            ExistingObj.ExamDate = DateTime.ParseExact(txtExamDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        else
                            ExistingObj.ExamDate = null;

                        if (CAStart.Text != "")
                        {
                            ExistingObj.CAStartDate = DateTime.ParseExact(CAStart.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(08, 45, 0);
                            ExistingObj.CAStartTime = Convert.ToDateTime(ExistingObj.CAStartDate).Date + ts;
                        }
                        else
                        {
                            ExistingObj.CAStartDate = null;
                            ExistingObj.CAStartTime = null;
                        }

                        if (CAEnd.Text != "")
                        {
                            ExistingObj.CAEndDate = DateTime.ParseExact(CAEnd.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(09, 29, 0);
                            ExistingObj.CAEndTime = Convert.ToDateTime(ExistingObj.CAEndDate).Date + ts;
                        }
                        else
                        {
                            ExistingObj.CAEndDate = null;
                            ExistingObj.CAEndTime = null;
                        }

                        if (FinalStart.Text != "")
                        {
                            ExistingObj.FinalStartDate = DateTime.ParseExact(FinalStart.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(00, 01, 0);
                            ExistingObj.FinalStartTime = Convert.ToDateTime(ExistingObj.FinalStartDate).Date + ts;
                        }
                        else
                        {
                            ExistingObj.FinalStartDate = null;
                            ExistingObj.FinalStartTime = null;
                        }

                        if (FinalEnd.Text != "")
                        {
                            ExistingObj.FinalEndDate = DateTime.ParseExact(FinalEnd.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(23, 59, 0);
                            ExistingObj.FinalEndTime = Convert.ToDateTime(ExistingObj.FinalEndDate).Date + ts;
                        }
                        else
                        {
                            ExistingObj.FinalEndDate = null;
                            ExistingObj.FinalEndTime = null;
                        }

                        ExistingObj.ModifiedBy = UserObj.Id;
                        ExistingObj.ModifiedDate = DateTime.Now;

                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        UpdateCount++;
                    }
                    #endregion

                    #region Insert Process
                    else // Insert a new Entry
                    {
                        DAL.ExamMarkDateRangeSetup NewObj = new DAL.ExamMarkDateRangeSetup();

                        NewObj.AcacalSectionId = AcaCal_SectionID;

                        if (txtExamDate.Text != "")
                            NewObj.ExamDate = DateTime.ParseExact(txtExamDate.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        else
                            NewObj.ExamDate = null;

                        if (CAStart.Text != "")
                        {
                            NewObj.CAStartDate = DateTime.ParseExact(CAStart.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(08, 45, 0);
                            NewObj.CAStartTime = Convert.ToDateTime(NewObj.CAStartDate).Date + ts;
                        }
                        else
                        {
                            NewObj.CAStartDate = null;
                            NewObj.CAStartTime = null;
                        }

                        if (CAEnd.Text != "")
                        {
                            NewObj.CAEndDate = DateTime.ParseExact(CAEnd.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(09, 29, 0);
                            NewObj.CAEndTime = Convert.ToDateTime(NewObj.CAEndDate).Date + ts;
                        }
                        else
                        {
                            NewObj.CAEndDate = null;
                            NewObj.CAEndTime = null;
                        }

                        if (FinalStart.Text != "")
                        {
                            NewObj.FinalStartDate = DateTime.ParseExact(FinalStart.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(00, 01, 0);
                            NewObj.FinalStartTime = Convert.ToDateTime(NewObj.FinalStartDate).Date + ts;
                        }
                        else
                        {
                            NewObj.FinalStartDate = null;
                            NewObj.FinalStartTime = null;
                        }

                        if (FinalEnd.Text != "")
                        {
                            NewObj.FinalEndDate = DateTime.ParseExact(FinalEnd.Text.Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            TimeSpan ts = new TimeSpan(23, 59, 0);
                            NewObj.FinalEndTime = Convert.ToDateTime(NewObj.FinalEndDate).Date + ts;
                        }
                        else
                        {
                            NewObj.FinalEndDate = null;
                            NewObj.FinalEndTime = null;
                        }

                        NewObj.CreatedBy = UserObj.Id;
                        NewObj.CreatedDate = DateTime.Now;

                        ucamContext.ExamMarkDateRangeSetups.Add(NewObj);
                        ucamContext.SaveChanges();

                        if (NewObj.Id > 0)
                        {
                            UpdateCount++;
                        }

                    }
                    #endregion
                }
                else
                {
                    SubmissionCount++;
                }

                ReturnValue = UpdateCount + "-" + SubmissionCount;
            }
            catch (Exception ex)
            {

            }

            return ReturnValue;
        }

        protected void btnSaveAll_Click(object sender, EventArgs e)
        {
            string message = "Date update failed";

            try
            {
                int UpdateCount = 0, SubmissionCount = 0;

                for (int i = 0; i < gvScheduleList.Rows.Count; i++)
                {
                    try
                    {
                        GridViewRow gvrow = gvScheduleList.Rows[i];

                        TextBox txtExamDate = (TextBox)gvrow.FindControl("txtExamDate");
                        TextBox CAStart = (TextBox)gvrow.FindControl("txtCAStartDate");
                        TextBox CAEnd = (TextBox)gvrow.FindControl("txtCAEndDate");
                        TextBox FinalStart = (TextBox)gvrow.FindControl("txtFinalStartDate");
                        TextBox FinalEnd = (TextBox)gvrow.FindControl("txtFinalEndDate");

                        Label lblSection = (Label)gvrow.FindControl("lblSection");

                        int SectionId = Convert.ToInt32(lblSection.Text);
                        if (SectionId > 0)
                        {
                            string ReturnValue = InsertUpdateDateInformation(SectionId, txtExamDate, CAStart, CAEnd, FinalStart, FinalEnd);
                            string[] splittedValue = ReturnValue.Split('-');
                            if (splittedValue.Length > 0 && Convert.ToInt32(splittedValue[1]) > 0)
                                SubmissionCount++;
                            else
                                UpdateCount++;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                }

                if (UpdateCount > 0)
                    message = "Date Updated Successfully";
                if (UpdateCount > 0 && SubmissionCount > 0)
                    message = message + " and some date not update because of 100% marks submission done";
                else if (SubmissionCount > 0)
                    message = "100% marks submission done. No change will possible";

            }
            catch (Exception ex)
            {
            }

            showAlert(message);
            LoadData();
            return;
        }
    }
}