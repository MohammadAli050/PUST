using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;


namespace EMS.Module.FormFillUp
{
    public partial class StudentFormFillUpApply : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentFormFillUpApply);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentFormFillUpApply));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                btnSubmit.Visible = false;
                btnSubmit2.Visible = false;

                if (UserObj.RoleID == 4) // Student
                {
                    txtStudentId.Text = UserObj.LogInID.ToString();
                    divBtnLoad.Visible = false;
                    txtStudentId.ReadOnly = true;
                    btnLoad_Click(null, null);
                }
                else
                {
                    txtStudentId.ReadOnly = false;
                    divBtnLoad.Visible = true;
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();

                string Roll = txtStudentId.Text.Trim();

                var StudentObj = ucamContext.Students.Where(x => x.Roll == Roll).FirstOrDefault();

                if (StudentObj != null)
                {
                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentObj.StudentID });

                    DataTable dt = DataTableManager.GetDataFromQuery("GetCourseListForFormFillUp", parameters1);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        gvCourseList.DataSource = dt;
                        gvCourseList.DataBind();


                        #region Check Application Deadline

                        int IsDeadLineTrue = 0;
                        int NotStartYet = 0, NotDeclareYet = 0;
                        string DeadLine = "";


                        try
                        {
                            var HeldInObj = ucamContext.StudentYearSemesterHistories.Where(x => x.StudentId == StudentObj.StudentID
                                               && x.IsActive == true).FirstOrDefault();

                            if (HeldInObj != null)
                            {
                                int HeldInId = 0;
                                int TypeId = Convert.ToInt32(CommonEnum.ActivityType.FormFillUpStudentApplication);

                                var HeldInRelationObj = ucamContext.ExamHeldInAndProgramRelations.Find(HeldInObj.HeldInProgramRelationId);
                                if (HeldInRelationObj != null)
                                    HeldInId = Convert.ToInt32(HeldInRelationObj.ExamHeldInId);

                                var DeadLineObj = ucamContext.DateRangeSetupInformations.Where(x => x.DateRangeActivityTypeId == TypeId && x.HeldInId == HeldInId).FirstOrDefault();

                                if (DeadLineObj != null && DeadLineObj.ActiveStatus == 1)
                                {

                                    if (Convert.ToDateTime(DateTime.Now) < Convert.ToDateTime(DeadLineObj.StartDate))
                                    {
                                        NotStartYet = 1;
                                    }

                                    DeadLine = "আবেদেন এর সময়সীমা " + Convert.ToDateTime(DeadLineObj.StartDate).ToString("dd-MM-yyyy") + " " + Convert.ToDateTime(DeadLineObj.StartTime).ToString("hh:mm:ss tt") + " থেকে "
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
                            btnSubmit.Visible = false;
                            btnSubmit2.Visible = false;

                            if (NotDeclareYet == 1)
                                showAlert("Form fill-up এর আবেদন প্রক্রিয়ার সময়সীমা এখনও ঘোষণা হয় নি।");
                            else if (NotStartYet == 1)
                                showAlert("Form fill-up এর আবেদন প্রক্রিয়া টি এখনও শুরু হয় নি। " + DeadLine);
                            else
                                showAlert("Form fill-up এর আবেদন প্রক্রিয়ার সময়সীমা শেষ। " + DeadLine);

                        }
                        else
                        {
                            string expression = "SetupId ='" + "0" + "'";

                            dt = DataTableMethods.FilterDataTable(dt, expression);

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                btnSubmit.Visible = true;
                                btnSubmit2.Visible = true;

                            }
                            else
                            {
                                btnSubmit.Visible = false;
                                btnSubmit2.Visible = false;

                                showAlert("আপনার Form fill-up এর আবেদনটি সম্পন্ন হয়েছে।");
                            }
                        }

                        #region Application Status

                        int HeldInRelationId = 0;
                        var yearSemesterObj = ucamContext.StudentYearSemesterHistories.Where(x => x.StudentId == StudentObj.StudentID && x.IsActive == true).FirstOrDefault();

                        if (yearSemesterObj != null)
                        {
                            HeldInRelationId = Convert.ToInt32(yearSemesterObj.HeldInProgramRelationId);
                        }

                        var pendingList = ucamContext.ApprovalServicesHistoryInformations.Where(x => x.StudentId == StudentObj.StudentID
                            && x.HeldInRelationId == HeldInRelationId && x.ActiveStatus == 1 && x.ApprovalServiceId == 1).ToList();

                        if (pendingList != null && pendingList.Any())
                        {
                            pendingList = pendingList.Where(x => x.ApplicationStatus != 2).ToList();
                            if (pendingList.Any())
                                lblStatus.Text = "আপনার  Form fill-up এর আবেদনটি প্রক্রিয়াধীন রয়েছে।";
                            else
                                lblStatus.Text = "আপনার Form fill-up এর আবেদনটি গৃহীত হয়েছে।";
                        }
                        else
                        {
                            lblStatus.Text = "আপনি এখনও Form fill-up এর জন্য আবেদন করেন নি।";
                        }


                        #endregion

                    }

                }
                else
                {
                    showAlert("Please enter a valid Student ID");
                    return;
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void ClearGridView()
        {
            gvCourseList.DataSource = null;
            gvCourseList.DataBind();
            lblStatus.Text = "";
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

                foreach (GridViewRow row in gvCourseList.Rows)
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {

                ModalPopupConfirmation.Show();
            }
            catch (Exception ex)
            {
            }

        }


        private int CountGridCourse()
        {
            int CourseCounter = 0;
            for (int i = 0; i < gvCourseList.Rows.Count; i++)
            {
                GridViewRow row = gvCourseList.Rows[i];
                CheckBox CourseCheckd = (CheckBox)row.FindControl("ChkSelect");
                if (CourseCheckd.Checked == true)
                {
                    CourseCounter++;
                }



            }


            return CourseCounter;
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

        protected void btnRequestConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int InsertCounter = 0;

                string Roll = txtStudentId.Text.Trim();

                DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

                var StudentObj = ucamContext.Students.Where(x => x.Roll == Roll).FirstOrDefault();

                int StudentId = 0;

                if (StudentObj == null)
                {
                    showAlert("Invalid Student Id");
                    return;
                }

                StudentId = StudentObj.StudentID;

                var ApprovalMatrixObj = ucamContext.ApprovalServicesAuthorizationMatrices.Where(x => x.EventName == "Form Fill Up").FirstOrDefault();

                int EventId = 0;
                EventId = ApprovalMatrixObj == null ? 0 : ApprovalMatrixObj.Id;


                string[] SplittedValue = CommonMethodForStringSplit.SplitString(ApprovalMatrixObj.ApprovalMatrix, '-');
                int RoleId = 0;
                if (SplittedValue.Length > 0)
                    RoleId = Convert.ToInt32(SplittedValue[0]);

                int CourseCount = CountGridCourse();


                if (CourseCount == 0)
                {
                    showAlert("Please Select Minimum One Course");
                    return;
                }

                if (EventId == 0 || RoleId == 0)
                {
                    showAlert("Approval Matrix Not Set Yet");
                    return;
                }


                #region Single Log

                try
                {
                   MisscellaneousCommonMethods.InsertLog(UserObj.LogInID,"Form Fill-Up Application", UserObj.LogInID + " is trying to submit a form fill-up application ", UserObj.LogInID, "",_pageId,_pageName,_pageUrl);
                }
                catch (Exception ex)
                {
                }

                #endregion


                foreach (GridViewRow row in gvCourseList.Rows)
                {
                    try
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");

                        if (ckBox.Checked)
                        {
                            HiddenField hdnCourseHistoryId = (HiddenField)row.FindControl("hdnCourseHistoryId");
                            HiddenField hdnSetupId = (HiddenField)row.FindControl("hdnSetupId");
                            HiddenField hdnHeldInRelationId = (HiddenField)row.FindControl("hdnHeldInRelationId");

                            int HId = Convert.ToInt32(hdnCourseHistoryId.Value);
                            int SetupId = Convert.ToInt32(hdnSetupId.Value);
                            int HeldInRelationId = Convert.ToInt32(hdnHeldInRelationId.Value);

                            if (SetupId == 0 && HId > 0 && HeldInRelationId > 0)
                            {
                                var ObjectExists = ucamContext.ApprovalServicesHistoryInformations.Where(x => x.ApprovalServiceId == EventId && x.StudentId == StudentId
                                            && x.CourseHistoryId == HId).FirstOrDefault();

                                if (ObjectExists == null)// New Entry For Form Fillup Application
                                {
                                    DAL.ApprovalServicesHistoryInformation NewObj = new DAL.ApprovalServicesHistoryInformation();

                                    NewObj.StudentId = StudentId;
                                    NewObj.ApprovalServiceId = EventId;
                                    NewObj.RoleId = RoleId;
                                    NewObj.ApplicationStatus = 1; // Status 1 For Application Apply
                                    NewObj.ActiveStatus = 0;
                                    NewObj.CourseHistoryId = HId;
                                    NewObj.HeldInRelationId = HeldInRelationId;
                                    NewObj.CreatedBy = UserObj.Id;
                                    NewObj.CreatedDate = DateTime.Now;

                                    ucamContext.ApprovalServicesHistoryInformations.Add(NewObj);
                                    ucamContext.SaveChanges();

                                    if (NewObj.Id > 0)
                                    {
                                        InsertCounter++;

                                        DAL.ApprovalServicesHistoryInformation scndLevelObj = new DAL.ApprovalServicesHistoryInformation();

                                        scndLevelObj.StudentId = StudentId;
                                        scndLevelObj.ApprovalServiceId = EventId;
                                        scndLevelObj.RoleId = Convert.ToInt32(SplittedValue[1]);
                                        scndLevelObj.ApplicationStatus = 0; // Status 0 For Pending
                                        scndLevelObj.ActiveStatus = 1;
                                        scndLevelObj.CourseHistoryId = HId;
                                        scndLevelObj.HeldInRelationId = HeldInRelationId;
                                        scndLevelObj.CreatedBy = UserObj.Id;
                                        scndLevelObj.CreatedDate = DateTime.Now;

                                        ucamContext.ApprovalServicesHistoryInformations.Add(scndLevelObj);
                                        ucamContext.SaveChanges();


                                        #region Log Insert

                                        try
                                        {
                                            var CourseHistoryObj = StudentCourseHistoryManager.GetById(HId);
                                            if (CourseHistoryObj != null)
                                            {
                                                MisscellaneousCommonMethods.InsertLog(UserObj.LogInID, "Form Fill-Up Application", UserObj.LogInID + " Submitted a form fill-up application. Course Information " + CourseHistoryObj.Course.FormalCode, UserObj.LogInID, CourseHistoryObj.FormalCode, _pageId, _pageName, _pageUrl);

                                            }

                                        }
                                        catch (Exception ex)
                                        {
                                        }

                                        #endregion


                                    }
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                    }

                }


                if (InsertCounter > 0)
                {
                    showAlert("আপনার Form fill-up সম্পন্ন হলো।");
                    btnLoad_Click(null, null);
                    return;
                }
                else
                {
                    showAlert("Submission Failed");
                    btnLoad_Click(null, null);
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }




    }
}