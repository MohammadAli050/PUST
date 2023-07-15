using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.FormFillUp
{
    public partial class FormFillUpApplicationManageByAcademicSection : BasePage
    {


        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.FormFillUpApplicationManageByAcademicSection);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.FormFillUpApplicationManageByAcademicSection));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                divbtnPanel.Visible = false;
                divbtnPanel2.Visible = false;
                divSearch.Visible = false;
                divDD.Visible = false;
                bool IsExists = CommonMethodForChekingRoleExistsInApprovalMatrix.IsRoleExists(UserObj.RoleID, "Form Fill Up");

                if (IsExists)
                {
                    divDD.Visible = true;
                    ucDepartment.LoadDropdownWithUserAccess(UserObj.Id);
                    ucDepartment_DepartmentSelectedIndexChanged(null, null);
                    LoadHeldInInformation();
                }
                else
                {
                    showAlert("You are not eligible to access this page");
                    return;
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
                LoadCourse();
                LoadData();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
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


        protected void LoadCourse()
        {
            try
            {
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-All Course-", "0"));
                ddlCourse.AppendDataBoundItems = true;


                int HeldInRelationId = 0;
                HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (HeldInRelationId > 0)
                {
                    BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                    List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetAllByHeldInRelationId(HeldInRelationId);

                    if (acaCalSectionList != null && acaCalSectionList.Any())
                    {
                        acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                        foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                        {
                            ddlCourse.Items.Add(new ListItem(acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Title + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.AcaCal_SectionID.ToString()));
                        }
                    }

                }
                else
                {

                }
            }
            catch (Exception ex) { }
        }

        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
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
                int RoleId = UserObj.RoleID;
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int StatusId = Convert.ToInt32(ddlStatus.SelectedValue);
                int AcacalSectionId = Convert.ToInt32(ddlCourse.SelectedValue);



                #region Check Application Deadline

                int IsDeadLineTrue = 0;
                int NotStartYet = 0, NotDeclareYet = 0;
                string DeadLine = "";


                try
                {
                    var HeldInRelationObj = ucamContext.ExamHeldInAndProgramRelations.Find(HeldInRelationId);

                    if (HeldInRelationObj != null)
                    {
                        int HeldInId = 0;
                        int TypeId = Convert.ToInt32(CommonEnum.ActivityType.FormFillUpAcademicSection);

                        HeldInId = Convert.ToInt32(HeldInRelationObj.ExamHeldInId);

                        var DeadLineObj = ucamContext.DateRangeSetupInformations.Where(x => x.DateRangeActivityTypeId == TypeId && x.HeldInId == HeldInId).FirstOrDefault();

                        if (DeadLineObj != null && DeadLineObj.ActiveStatus == 1)
                        {

                            if (Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.StartDate))
                            {
                                NotStartYet = 1;
                            }

                            DeadLine = "সময়সীমা " + Convert.ToDateTime(DeadLineObj.StartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.StartTime).ToString("hh:mm:ss tt") + " থেকে "
                                + Convert.ToDateTime(DeadLineObj.EndDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.EndTime).ToString("hh:mm:ss tt") + " পর্যন্ত";

                            bool IsValid = MisscellaneousCommonMethods.TimeCheck(Convert.ToDateTime(DeadLineObj.StartDate), Convert.ToDateTime(DeadLineObj.StartTime),
                                Convert.ToDateTime(DeadLineObj.EndDate), Convert.ToDateTime(DeadLineObj.EndTime));

                            if (IsValid)
                                IsDeadLineTrue = 1;
                            else
                                IsDeadLineTrue = 2;

                        }
                        else
                            NotDeclareYet = 1;
                    }
                }
                catch (Exception ex)
                {
                }



                #endregion



                if (IsDeadLineTrue == 0 || IsDeadLineTrue == 2)
                {
                    lblDeadLine.ForeColor = System.Drawing.Color.Red;
                    if (NotDeclareYet == 1)
                        lblDeadLine.Text = "Academic Section কর্তৃক Form fill-up এর প্রক্রিয়ার সময়সীমা এখনও ঘোষণা হয় নি।";
                    else if (NotStartYet == 1)
                        lblDeadLine.Text = "Academic Section কর্তৃক Form fill-up এর প্রক্রিয়া টি এখনও শুরু হয় নি। " + DeadLine;
                    else
                        lblDeadLine.Text = "Academic Section কর্তৃক Form fill-up এর প্রক্রিয়ার সময়সীমা শেষ। " + DeadLine;



                    #region Load All Data After Deadline Exceed

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                    parameters1.Add(new SqlParameter { ParameterName = "@RoleId", SqlDbType = System.Data.SqlDbType.Int, Value = RoleId });
                    parameters1.Add(new SqlParameter { ParameterName = "@StatusId", SqlDbType = System.Data.SqlDbType.Int, Value = StatusId });
                    parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                    parameters1.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                    DataTable DataTableApplicationList = DataTableManager.GetDataFromQuery("GetAllPendingFormFillUpApplication", parameters1);

                    if (DataTableApplicationList != null && DataTableApplicationList.Rows.Count > 0)
                    {
                        gvApplicationList.DataSource = DataTableApplicationList;
                        gvApplicationList.DataBind();

                        divbtnPanel.Visible = false;
                        divbtnPanel2.Visible = false;
                        divSearch.Visible = true;
                        if (StatusId == 0)
                        {
                            this.gvApplicationList.Columns[7].Visible = true;
                        }
                        else
                        {
                            this.gvApplicationList.Columns[7].Visible = false;
                        }
                        
                    }

                    #endregion

                }
                else
                {
                    lblDeadLine.Text = "Academic Section কর্তৃক Form fill-up এর প্রক্রিয়ার " + DeadLine;
                    lblDeadLine.ForeColor = System.Drawing.Color.Blue;

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                    parameters1.Add(new SqlParameter { ParameterName = "@RoleId", SqlDbType = System.Data.SqlDbType.Int, Value = RoleId });
                    parameters1.Add(new SqlParameter { ParameterName = "@StatusId", SqlDbType = System.Data.SqlDbType.Int, Value = StatusId });
                    parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                    parameters1.Add(new SqlParameter { ParameterName = "@AcacalSectionId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalSectionId });

                    DataTable DataTableApplicationList = DataTableManager.GetDataFromQuery("GetAllPendingFormFillUpApplication", parameters1);

                    if (DataTableApplicationList != null && DataTableApplicationList.Rows.Count > 0)
                    {
                        gvApplicationList.DataSource = DataTableApplicationList;
                        gvApplicationList.DataBind();

                        if (StatusId == 0)
                        {
                            divbtnPanel.Visible = true;
                            divbtnPanel2.Visible = true;
                            divSearch.Visible = true;
                            this.gvApplicationList.Columns[7].Visible = true;
                        }
                        else
                        {
                            this.gvApplicationList.Columns[7].Visible = false;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearGridView()
        {
            try
            {
                gvApplicationList.DataSource = null;
                gvApplicationList.DataBind();
                divSearch.Visible = false;
                divbtnPanel.Visible = false;
                divbtnPanel2.Visible = false;
                lblDeadLine.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.Checked)
                {
                    chk.Text = "Unselect All";
                }
                else
                {
                    chk.Text = "Select All";
                }

                foreach (GridViewRow row in gvApplicationList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");
                    ckBox.Checked = chk.Checked;
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

        protected void lnkStatus_Click(object sender, EventArgs e)
        {
            try
            {
                //lblCourseInfo.Text = string.Empty;

                LinkButton btn = (LinkButton)(sender);

                int SetupId = Convert.ToInt32(btn.CommandArgument);


                var SetupObj = ucamContext.ApprovalServicesHistoryInformations.Find(SetupId);
                if (SetupObj != null)
                {
                    LogicLayer.BusinessObjects.Course crs = CourseManager.GetByCourseIdVersionId(Convert.ToInt32(SetupObj.CourseId), Convert.ToInt32(SetupObj.VersionId));
                    //if (crs != null)
                    //lblCourseInfo.Text = crs.FormalCode + "_" + crs.VersionCode + "_" + crs.Title + " (" + crs.Credits + ")";
                }

                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(new SqlParameter { ParameterName = "@SetupId", SqlDbType = System.Data.SqlDbType.Int, Value = SetupId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetApplicationHistoryBySetupId", parameters2);

                if (dt != null && dt.Rows.Count > 0)
                {
                    ModalPopUpHistory.Show();
                    gvStatusHistory.DataSource = dt;
                    gvStatusHistory.DataBind();
                }

            }
            catch (Exception ex)
            {

            }
        }


        private int CountGridSelected()
        {
            int SelectCounter = 0;
            for (int i = 0; i < gvApplicationList.Rows.Count; i++)
            {
                GridViewRow row = gvApplicationList.Rows[i];
                CheckBox CourseCheckd = (CheckBox)row.FindControl("ChkSelect");
                if (CourseCheckd.Checked == true)
                {
                    SelectCounter++;

                }



            }


            return SelectCounter;
        }

        protected void lnkApprove_Click(object sender, EventArgs e)
        {
            try
            {
                ModalPopupConfirmation.Show();


            }
            catch (Exception ex)
            {
            }
        }

        protected void lnkReject_Click(object sender, EventArgs e)
        {
            int SelectCount = CountGridSelected();

            if (SelectCount == 0)
            {
                showAlert("Please select minimum one student");
                return;
            }
            ModalPopupRemarks.Show();
        }

        protected void btnRejectConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                string Reason = "";
                if (string.IsNullOrEmpty(txtReason.Text))
                {
                    showAlert("Please enter a reason");
                    ModalPopupRemarks.Show();
                    return;
                }

                Reason = txtReason.Text.Trim();

                int UpdateCounter = 0;

                foreach (GridViewRow row in gvApplicationList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");

                    if (ckBox.Checked)
                    {
                        Label lblSetupId = (Label)row.FindControl("lblSetupId");

                        int SetupId = Convert.ToInt32(lblSetupId.Text);

                        if (SetupId > 0)
                        {
                            var ExistingObj = ucamContext.ApprovalServicesHistoryInformations.Find(SetupId);

                            if (ExistingObj != null)
                            {
                                try
                                {

                                    //Update existing entry

                                    ExistingObj.ApplicationStatus = 3;
                                    ExistingObj.Reason = Reason;
                                    ExistingObj.ModifiedBy = UserObj.Id;
                                    ExistingObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    UpdateCounter++;




                                    #region Log Insert

                                    try
                                    {
                                        //LogicLayer.BusinessObjects.Course crs = LogicLayer.BusinessLogic.CourseManager.GetByCourseIdVersionId((int)ExistingObj.CourseId, (int)ExistingObj.VersionId);

                                        //InsertLog("Extra Course Add Reject", userObj.LogInID + " Rejected a extra course add Application" + " with reason " + Reason + ". Course Information " + crs.CourseFullInfo, stdObj.Roll);
                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    #endregion
                                }
                                catch (Exception ex)
                                {
                                }
                            }
                        }
                    }
                }

                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);

                if (UpdateCounter > 0)
                {
                    showAlert("Application Rejected Successfully");
                    LoadData();
                    return;
                }
                else
                {
                    showAlert("Application Reject Failed");
                    return;
                }

            }
            catch (Exception ex)
            {
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);
            }
        }

        protected void btnRequestConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int InsertCounter = 0;

                int RoleId = 0, LastRoleId = 0, NextRoleId = 0;

                RoleId = UserObj.RoleID;

                string[] AllRole = CommonMethodForChekingRoleExistsInApprovalMatrix.SplittedValue("Form Fill Up");
                if (AllRole.Length > 1)
                {
                    for (int i = 0; i < AllRole.Length; i++)
                    {
                        try
                        {
                            if (Convert.ToInt32(AllRole[i]) == UserObj.RoleID && i != (AllRole.Length - 1))
                            {
                                RoleId = Convert.ToInt32(AllRole[i + 1]);
                                try
                                {
                                    NextRoleId = Convert.ToInt32(AllRole[i + 2]);
                                }
                                catch (Exception ex)
                                {
                                    NextRoleId = RoleId;
                                }
                                break;
                            }
                            else
                                RoleId = UserObj.RoleID;

                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    LastRoleId = Convert.ToInt32(AllRole[AllRole.Length - 1]);

                }

                int SelectCount = CountGridSelected();

                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                if (SelectCount == 0)
                {
                    cookie.Value = "Flag";
                    cookie.Expires = DateTime.Now.AddDays(1);
                    Response.AppendCookie(cookie);
                    showAlert("No student found for forward");
                    return;
                }


                #region Single Log

                try
                {
                    MisscellaneousCommonMethods.InsertLog(UserObj.LogInID, "Form Fill-Up Application", UserObj.LogInID + " is trying to forward form fill-up application to Controller Office ", "", "", _pageId, _pageName, _pageUrl);
                }
                catch (Exception ex)
                {
                }

                #endregion


                foreach (GridViewRow row in gvApplicationList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");
                    decimal Percentage = 59;

                    if (ckBox.Checked)
                    {
                        Label lblSetupId = (Label)row.FindControl("lblSetupId");

                        int SetupId = Convert.ToInt32(lblSetupId.Text);

                        if (SetupId > 0)
                        {
                            var ExistingObj = ucamContext.ApprovalServicesHistoryInformations.Find(SetupId);

                            if (ExistingObj != null)
                            {
                                var stdObj = ucamContext.Students.Find(ExistingObj.StudentId);

                                try
                                {

                                    //Update existing entry
                                    if (UserObj.RoleID != LastRoleId) // Last Level
                                        ExistingObj.ActiveStatus = 0;

                                    ExistingObj.ApplicationStatus = 2;
                                    ExistingObj.ModifiedBy = UserObj.Id;
                                    ExistingObj.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                    ucamContext.SaveChanges();

                                    InsertCounter++;

                                    if (UserObj.RoleID != LastRoleId) // Last Level
                                    {


                                        // Insert a new Entry

                                        DAL.ApprovalServicesHistoryInformation trdLevelObj = new DAL.ApprovalServicesHistoryInformation();

                                        trdLevelObj.StudentId = ExistingObj.StudentId;
                                        trdLevelObj.ApprovalServiceId = ExistingObj.ApprovalServiceId;
                                        trdLevelObj.RoleId = RoleId;

                                        trdLevelObj.ApplicationStatus = 0; // Status 0 For Pending
                                        trdLevelObj.ActiveStatus = 1;
                                        trdLevelObj.Reason = ExistingObj.Reason;

                                        trdLevelObj.HeldInRelationId = ExistingObj.HeldInRelationId;
                                        trdLevelObj.CourseHistoryId = ExistingObj.CourseHistoryId;
                                        trdLevelObj.AttendancePercentage = ExistingObj.AttendancePercentage;

                                        trdLevelObj.CreatedBy = UserObj.Id;
                                        trdLevelObj.CreatedDate = DateTime.Now;

                                        ucamContext.ApprovalServicesHistoryInformations.Add(trdLevelObj);
                                        ucamContext.SaveChanges();

                                        if (trdLevelObj.Id > 0)
                                            InsertCounter++;

                                    }


                                    #region Log Insert

                                    try
                                    {
                                        var CourseHistoryObj = StudentCourseHistoryManager.GetById(Convert.ToInt32(ExistingObj.CourseHistoryId));
                                        if (CourseHistoryObj != null)
                                        {
                                            MisscellaneousCommonMethods.InsertLog(UserObj.LogInID, "Form Fill-Up Application", UserObj.LogInID + " forward a fill-up application to Controller Office. Student Roll : " + CourseHistoryObj.Roll + " and Course Information " + CourseHistoryObj.Course.FormalCode, CourseHistoryObj.Roll, CourseHistoryObj.FormalCode, _pageId, _pageName, _pageUrl);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    #endregion

                                }
                                catch (Exception ex)
                                {
                                }



                            }
                        }
                    }
                }

                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);

                if (InsertCounter > 0)
                {
                    showAlert("Application Forwarded Successfully");
                    LoadData();
                    return;
                }
                else
                {
                    showAlert("Application Forward Failed");
                    return;
                }
            }
            catch (Exception ex)
            {
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);
            }
        }


    }
}