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
    public partial class FormFillUpApplicationManageByHallProvost : BasePage
    {

        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.FormFillUpApplicationManageByHallProvost);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.FormFillUpApplicationManageByHallProvost));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                divSearch.Visible = false;
                divDD.Visible = false;
                bool IsExists = CommonMethodForChekingRoleExistsInApprovalMatrix.IsRoleExists(UserObj.RoleID, "Form Fill Up");

                if (IsExists)
                {
                    divDD.Visible = true;
                    divbtnPanel.Visible = false;
                    divbtnPanel2.Visible = false;
                    LoadExamYear();
                    LoadHeldInInformation();
                    //ucDepartment.LoadDropdownWithUserAccess(UserObj.Id);
                    //ucDepartment_DepartmentSelectedIndexChanged(null, null);
                    //LoadHeldInInformation();
                }
                else
                {
                    showAlert("You are not eligible to access this page");
                    return;
                }

            }
        }

        private void LoadExamYear()
        {
            try
            {
                ddlExamYear.Items.Clear();

                int startYear = 2003;

                for (int MaxYear = DateTime.Now.Year + 1; MaxYear >= startYear; MaxYear--)
                {
                    ddlExamYear.Items.Add(new ListItem(Convert.ToString(MaxYear)));
                }
                ddlExamYear.SelectedValue = ((DateTime.Now.Year) - 1).ToString();
                ddlExamYear_SelectedIndexChanged(null, null);
            }
            catch (Exception ex)
            {
            }
        }

        #region On Load Methods

        private void LoadHeldInInformation()
        {
            try
            {
                int ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);

                var HeldInList = ucamContext.ExamHeldInInformations.Where(x => (x.Year == ExamYear.ToString() || ExamYear == 0) && x.IsActive == true).ToList();

                if (HeldInList != null && HeldInList.Any())
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "Id";
                    ddlHeldIn.DataSource = HeldInList.OrderBy(x => x.Id);
                    ddlHeldIn.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadProgram()
        {
            try
            {
                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<Program> ProgramList = new List<Program>();

                var HeldInRelationList = ucamContext.ExamHeldInAndProgramRelations.Where(x => x.ExamHeldInId == HeldInId && x.IsActive == true).ToList();

                if (HeldInRelationList != null && HeldInRelationList.Any())
                {
                    foreach (var item in HeldInRelationList)
                    {
                        Program prgObj = LogicLayer.BusinessLogic.ProgramManager.GetById(item.ProgramId);

                        if (prgObj != null)
                        {
                            ProgramList.Add(prgObj);
                        }
                    }
                }

                ddlProgram.Items.Clear();
                ddlProgram.AppendDataBoundItems = true;
                ddlProgram.Items.Add(new ListItem("All", "0"));

                if (ProgramList != null && ProgramList.Any())
                {
                    ddlProgram.DataTextField = "ShortName";
                    ddlProgram.DataValueField = "ProgramID";

                    ddlProgram.DataSource = ProgramList.OrderBy(x => x.ShortName);
                    ddlProgram.DataBind();

                }

            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region On Selected Index Changed Methods


        protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHeldInInformation();
            LoadProgram();
            LoadData();
        }

        //protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
        //        ucProgram.LoadDropdownByDepartmentId(departmentId);
        //        LoadHeldInInformation();
        //        ClearGridView();
        //        LoadData();
        //    }
        //    catch (Exception)
        //    {
        //    }
        //}

        //protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LoadHeldInInformation();
        //        ClearGridView();
        //        LoadData();
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                LoadProgram();
                LoadData();
            }
            catch (Exception ex)
            {
            }
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

        protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
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

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        private void LoadData()
        {
            try
            {
                ClearGridView();

                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int RoleId = UserObj.RoleID;
                int ProgramId = Convert.ToInt32(ddlProgram.SelectedValue);
                int StatusId = Convert.ToInt32(ddlStatus.SelectedValue);



                #region Check Application Deadline

                int IsDeadLineTrue = 0;
                int NotStartYet = 0, NotDeclareYet = 0;
                string DeadLine = "";


                try
                {
                    int TypeId = Convert.ToInt32(CommonEnum.ActivityType.FormFillUpHallProvost);


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
                catch (Exception ex)
                {
                }



                #endregion

                if (IsDeadLineTrue == 0 || IsDeadLineTrue == 2)
                {
                    lblDeadLine.ForeColor = System.Drawing.Color.Red;
                    if (NotDeclareYet == 1)
                        lblDeadLine.Text = "Hall Provost কর্তৃক Form fill-up এর প্রক্রিয়ার সময়সীমা এখনও ঘোষণা হয় নি।";
                    else if (NotStartYet == 1)
                        lblDeadLine.Text = "Hall Provost কর্তৃক Form fill-up এর প্রক্রিয়া টি এখনও শুরু হয় নি। " + DeadLine;
                    else
                        lblDeadLine.Text = "Hall Provost কর্তৃক Form fill-up এর প্রক্রিয়ার সময়সীমা শেষ। " + DeadLine;


                    #region Load All Data After Deadline Exceed

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });
                    parameters1.Add(new SqlParameter { ParameterName = "@RoleId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.RoleID });
                    parameters1.Add(new SqlParameter { ParameterName = "@StatusId", SqlDbType = System.Data.SqlDbType.Int, Value = StatusId });
                    parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                    parameters1.Add(new SqlParameter { ParameterName = "@LoginId", SqlDbType = System.Data.SqlDbType.NVarChar, Value = UserObj.LogInID });

                    DataTable DataTableApplicationList = DataTableManager.GetDataFromQuery("GetAllFormFillUpApplicationByHallProvost", parameters1);

                    DataTable distinctStudents = new DataTable();
                    if (DataTableApplicationList != null && DataTableApplicationList.Rows.Count > 0)
                    {
                        distinctStudents = DataTableApplicationList.DefaultView.ToTable(true, "ShortName", "Session", "StudentID", "Roll", "FullName", "RoleName", "ApplicationStatus", "HeldInRelationId");

                        gvApplicationList.DataSource = distinctStudents;
                        gvApplicationList.DataBind();

                        divbtnPanel.Visible = false;
                        divbtnPanel2.Visible = false;
                        divSearch.Visible = true;
                        if (StatusId == 0)
                        {
                            this.gvApplicationList.Columns[4].Visible = true;
                        }
                        else
                        {
                            this.gvApplicationList.Columns[4].Visible = false;
                        }
                    }

                    #endregion

                }
                else
                {
                    lblDeadLine.Text = "Hall Provost কর্তৃক Form fill-up এর প্রক্রিয়ার " + DeadLine;
                    lblDeadLine.ForeColor = System.Drawing.Color.Blue;

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInId });
                    parameters1.Add(new SqlParameter { ParameterName = "@RoleId", SqlDbType = System.Data.SqlDbType.Int, Value = UserObj.RoleID });
                    parameters1.Add(new SqlParameter { ParameterName = "@StatusId", SqlDbType = System.Data.SqlDbType.Int, Value = StatusId });
                    parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                    parameters1.Add(new SqlParameter { ParameterName = "@LoginId", SqlDbType = System.Data.SqlDbType.NVarChar, Value = UserObj.LogInID });

                    DataTable DataTableApplicationList = DataTableManager.GetDataFromQuery("GetAllFormFillUpApplicationByHallProvost", parameters1);

                    DataTable distinctStudents = new DataTable();
                    if (DataTableApplicationList != null && DataTableApplicationList.Rows.Count > 0)
                    {
                        distinctStudents = DataTableApplicationList.DefaultView.ToTable(true, "ShortName", "Session", "StudentID", "Roll", "FullName", "RoleName", "ApplicationStatus", "HeldInRelationId");

                        gvApplicationList.DataSource = distinctStudents;
                        gvApplicationList.DataBind();

                        if (StatusId == 0)
                        {
                            divbtnPanel.Visible = true;
                            divbtnPanel2.Visible = true;
                            divSearch.Visible = true;
                            this.gvApplicationList.Columns[4].Visible = true;
                        }
                        else
                        {
                            this.gvApplicationList.Columns[4].Visible = false;
                        }

                    }
                }
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

        protected void btnRequestConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int InsertCount = 0;

                int RoleId = 0, LastRoleId = 0;

                RoleId = UserObj.RoleID;

                #region Get Next Role

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

                #endregion


                #region Single Log

                try
                {
                    MisscellaneousCommonMethods.InsertLog(UserObj.LogInID, "Form Fill-Up Application", UserObj.LogInID + " is trying to forward form fill-up application to Academic Section ", "", "", _pageId, _pageName, _pageUrl);
                }
                catch (Exception ex)
                {
                }

                #endregion


                foreach (GridViewRow row in gvApplicationList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");

                    if (ckBox.Checked)
                    {

                        Label lblStudentId = (Label)row.FindControl("lblStudentId");
                        Label lblHeldInRelationId = (Label)row.FindControl("lblHeldInRelationId");

                        int HeldInId = Convert.ToInt32(lblHeldInRelationId.Text);
                        int StudentId = Convert.ToInt32(lblStudentId.Text);

                        if (StudentId > 0)
                        {
                            #region Process


                            var ItemList = ucamContext.ApprovalServicesHistoryInformations.AsNoTracking().Where(x => x.HeldInRelationId == HeldInId
                                && x.ApprovalServiceId == 1 && x.StudentId == StudentId && x.RoleId == UserObj.RoleID && x.ApplicationStatus == 0 && x.ActiveStatus == 1).ToList();

                            if (ItemList != null && ItemList.Any())
                            {
                                foreach (var item in ItemList)
                                {
                                    // Update Existing Object

                                    item.ActiveStatus = 0;
                                    item.ApplicationStatus = 2;
                                    item.ModifiedBy = UserObj.Id;
                                    item.ModifiedDate = DateTime.Now;

                                    ucamContext.Entry(item).State = EntityState.Modified;
                                    ucamContext.SaveChanges();


                                    // Enter a New Object

                                    DAL.ApprovalServicesHistoryInformation trdLevelObj = new DAL.ApprovalServicesHistoryInformation();

                                    trdLevelObj.StudentId = item.StudentId;
                                    trdLevelObj.ApprovalServiceId = item.ApprovalServiceId;
                                    trdLevelObj.RoleId = RoleId;
                                    trdLevelObj.ApplicationStatus = 0; // Status 0 For Pending
                                    trdLevelObj.ActiveStatus = 1;
                                    trdLevelObj.Reason = item.Reason;

                                    trdLevelObj.HeldInRelationId = item.HeldInRelationId;
                                    trdLevelObj.CourseHistoryId = item.CourseHistoryId;
                                    trdLevelObj.AttendancePercentage = item.AttendancePercentage;

                                    trdLevelObj.CreatedBy = UserObj.Id;
                                    trdLevelObj.CreatedDate = DateTime.Now;

                                    ucamContext.ApprovalServicesHistoryInformations.Add(trdLevelObj);
                                    ucamContext.SaveChanges();

                                    if (trdLevelObj.Id > 0)
                                        InsertCount++;



                                    #region Log Insert

                                    try
                                    {
                                        var CourseHistoryObj = StudentCourseHistoryManager.GetById(Convert.ToInt32(item.CourseHistoryId));
                                        if (CourseHistoryObj != null)
                                        {
                                            MisscellaneousCommonMethods.InsertLog(UserObj.LogInID, "Form Fill-Up Application", UserObj.LogInID + " forward a fill-up application to Academic Section. Student Roll : " + CourseHistoryObj.Roll + " and Course Information " + CourseHistoryObj.Course.FormalCode, CourseHistoryObj.Roll, CourseHistoryObj.FormalCode, _pageId, _pageName, _pageUrl);
                                        }

                                    }
                                    catch (Exception ex)
                                    {
                                    }

                                    #endregion
                                }
                            }
                            #endregion

                        }
                    }

                }
                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);

                if (InsertCount > 0)
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