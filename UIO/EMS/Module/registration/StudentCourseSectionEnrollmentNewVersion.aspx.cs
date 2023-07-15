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

namespace EMS.Module.registration
{
    public partial class StudentCourseSectionEnrollmentNewVersion : BasePage
    {
        decimal TotalCredits = 0;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentCourseSectionEnrollment);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentCourseSectionEnrollment));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                btnCourseEnrollment.Visible = false;
                ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();
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
                ClearGridView();
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
                LoadStudentInformation();
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
        #endregion


        private void LoadStudentInformation()
        {
            try
            {
                ClearGridView();
                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (HeldInRelationId > 0)
                {
                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                    DataTable DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentByHeldInRelationId", parameters1);

                    if (DataTableStudentList != null && DataTableStudentList.Rows.Count > 0)
                    {
                        gvStudentList.DataSource = DataTableStudentList;
                        gvStudentList.DataBind();

                        btnCourseEnrollment.Visible = true;

                    }

                }
                else
                {
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
                gvStudentList.DataSource = null;
                gvStudentList.DataBind();
                btnCourseEnrollment.Visible = false;
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

                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }


        #region View Regular Course List

        protected void lnkViewRegularCourse_Click(object sender, EventArgs e)
        {
            try
            {

                gvModalCourseList.DataSource = null;
                gvModalCourseList.DataBind();
                lblId.Text = string.Empty;
                lblName.Text = string.Empty;

                ModalPopupCourse.Hide();

                LinkButton btn = (LinkButton)(sender);

                string Value = btn.CommandArgument.ToString();

                string[] SplittedValue = Value.Split('_');

                int StudentId = 0, YearId = 0, SemesterId = 0;

                if (SplittedValue.Length > 0)
                {
                    StudentId = Convert.ToInt32(SplittedValue[0]);
                    YearId = Convert.ToInt32(SplittedValue[1]);
                    SemesterId = Convert.ToInt32(SplittedValue[2]);
                }

                Student stdObj = StudentManager.GetById(StudentId);
                if (stdObj != null)
                {
                    lblId.Text = "Student ID : " + stdObj.Roll;
                    lblName.Text = "Student Name : " + stdObj.Name;
                }



                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });
                parameters1.Add(new SqlParameter { ParameterName = "@YearId", SqlDbType = System.Data.SqlDbType.Int, Value = YearId });
                parameters1.Add(new SqlParameter { ParameterName = "@SemesterId", SqlDbType = System.Data.SqlDbType.Int, Value = SemesterId });

                DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetSyllabusCourseListByStudentYearSemesterId", parameters1);

                if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                {
                    gvModalCourseList.DataSource = DataTableCourseList;
                    gvModalCourseList.DataBind();
                    ModalPopupCourse.Show();
                }
                else
                {
                    showAlert("No Course Found");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void gvModalCourseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Credits"));
                try
                {
                    TotalCredits = TotalCredits + Convert.ToDecimal(str);
                }
                catch (Exception ex)
                {

                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = "Total : " + TotalCredits.ToString();
            }
        }



        #endregion

        #region View Special Course List

        protected void lnkViewSpecialCourse_Click(object sender, EventArgs e)
        {
            try
            {

                gvModalCourseList.DataSource = null;
                gvModalCourseList.DataBind();
                lblId.Text = string.Empty;
                lblName.Text = string.Empty;

                ModalPopupSpecialCourse.Hide();

                LinkButton btn = (LinkButton)(sender);

                string Value = btn.CommandArgument.ToString();

                string[] SplittedValue = Value.Split('_');

                int StudentId = 0, YearId = 0, SemesterId = 0;

                if (SplittedValue.Length > 0)
                {
                    StudentId = Convert.ToInt32(SplittedValue[0]);
                    YearId = Convert.ToInt32(SplittedValue[1]);
                    SemesterId = Convert.ToInt32(SplittedValue[2]);
                }

                Student stdObj = StudentManager.GetById(StudentId);
                if (stdObj != null)
                {
                    lblId.Text = "Student ID : " + stdObj.Roll;
                    lblName.Text = "Student Name : " + stdObj.Name;
                }



                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });
                parameters1.Add(new SqlParameter { ParameterName = "@YearId", SqlDbType = System.Data.SqlDbType.Int, Value = YearId });
                parameters1.Add(new SqlParameter { ParameterName = "@SemesterId", SqlDbType = System.Data.SqlDbType.Int, Value = SemesterId });

                DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetSyllabusCourseListByStudentYearSemesterId", parameters1);

                if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                {
                    gvModalSpecialCourseList.DataSource = DataTableCourseList;
                    gvModalSpecialCourseList.DataBind();
                    ModalPopupSpecialCourse.Show();
                }
                else
                {
                    showAlert("No Course Found");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void gvModalSpecialCourseList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string str = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Credits"));
                try
                {
                    TotalCredits = TotalCredits + Convert.ToDecimal(str);
                }
                catch (Exception ex)
                {

                }
            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[3].Text = "Total : " + TotalCredits.ToString();
            }
        }



        #endregion

        #region Regular Course Enrollment

        protected void btnCourseEnrollment_Click(object sender, EventArgs e)
        {
            try
            {
                int SelectedCount = CountGridSelected();

                if (SelectedCount == 0)
                {
                    showAlert("Please select minimum one student");
                    return;
                }
                else
                {
                    try
                    {
                        int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                        int InsertCounter = 0;
                        if (heldInRelationId > 0)
                        {
                            foreach (GridViewRow row in gvStudentList.Rows)
                            {
                                CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                                if (ckBox.Checked)
                                {
                                    HiddenField hdnStudentId = (HiddenField)row.FindControl("hdnStudentID");
                                    HiddenField hdnYearId = (HiddenField)row.FindControl("hdnYearId");
                                    HiddenField hdnSemesterId = (HiddenField)row.FindControl("hdnSemesterId");

                                    int studentId = Convert.ToInt32(hdnStudentId.Value);
                                    int YearId = Convert.ToInt32(hdnYearId.Value);
                                    int SemesterId = Convert.ToInt32(hdnSemesterId.Value);


                                    if (studentId > 0 && YearId > 0 && SemesterId > 0)
                                    {
                                        GenerateWorkSheetEntry(studentId, heldInRelationId, YearId, SemesterId);

                                        int Counter = InsertWorksheetCourseIntoStudentCourseHistory(studentId, heldInRelationId);

                                        InsertCounter = InsertCounter + Counter;
                                    }
                                }
                            }

                            if (InsertCounter > 0)
                            {
                                showAlert("Course Registered Successfully");
                            }
                            else
                            {
                                showAlert("Course Enrollment Failed");
                            }
                            ddlHeldIn_SelectedIndexChanged(null, null);
                        }
                        else
                        {
                            showAlert("Please select a Held In");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void GenerateWorkSheetEntry(int studentId, int heldInRelationId, int YearId, int SemesterId)
        {
            try
            {
                string registrationType = null;

                registrationType = "R"; // R for Regular Course

                LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                if (studentObj != null)
                {
                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = studentId });
                    parameters1.Add(new SqlParameter { ParameterName = "@YearId", SqlDbType = System.Data.SqlDbType.Int, Value = YearId });
                    parameters1.Add(new SqlParameter { ParameterName = "@SemesterId", SqlDbType = System.Data.SqlDbType.Int, Value = SemesterId });

                    DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetRemainingRegularCourseListByStudentYearSemesterId", parameters1);

                    if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                    {
                        foreach (DataRow dr in DataTableCourseList.Rows)
                        {
                            int CourseId = Convert.ToInt32(dr["CourseID"]);
                            int VersionId = Convert.ToInt32(dr["VersionID"]);

                            LogicLayer.BusinessObjects.Course courseObj = CourseManager.GetByCourseIdVersionId(CourseId, VersionId);

                            if (courseObj != null)
                            {
                                SectionCreateAndEntryInWorksheetTable(studentObj, courseObj, heldInRelationId, YearId, SemesterId, registrationType);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SectionCreateAndEntryInWorksheetTable(Student studentObj, Course courseObj, int heldInRelationId, int YearId, int SemesterId, string registrationType)
        {
            try
            {
                int AcacalSectionId = 0;
                try
                {
                    AcacalSectionId = CreateClassRoutineOrGetExistingRoutine(heldInRelationId, courseObj.CourseID, courseObj.VersionID);
                }
                catch (Exception ex)
                {
                }

                var existingObj = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == studentObj.StudentID && x.HeldInRelationId == heldInRelationId
                    && x.CourseID == courseObj.CourseID).FirstOrDefault();

                if (existingObj == null)
                {
                    decimal TotalCredit = 0;

                    #region Total Credit Checking (Maximum 30 credit Including backlog courses)

                    var AutoAssignCourseList = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == studentObj.StudentID
                        && x.HeldInRelationId == heldInRelationId && x.IsAutoAssign == true).ToList();

                    if (AutoAssignCourseList != null && AutoAssignCourseList.Any())
                        TotalCredit = Convert.ToDecimal(AutoAssignCourseList.Sum(x => x.CourseCredit)) + courseObj.Credits;

                    #endregion

                    if (TotalCredit <= 30)
                    {
                        RegistrationWorksheet regWorkSheet = new RegistrationWorksheet();
                        regWorkSheet.CourseID = courseObj.CourseID;
                        regWorkSheet.VersionID = courseObj.VersionID;
                        regWorkSheet.CourseCredit = courseObj.Credits;
                        regWorkSheet.IsAutoOpen = true;
                        regWorkSheet.IsAutoAssign = true;
                        regWorkSheet.BatchID = studentObj.BatchId;
                        if (registrationType == "R")
                            regWorkSheet.RetakeNo = Convert.ToInt32(CommonEnum.CourseRegistrationStatus.Regular);
                        else if (registrationType == "B")
                            regWorkSheet.RetakeNo = Convert.ToInt32(CommonEnum.CourseRegistrationStatus.Backlog);
                        regWorkSheet.StudentID = studentObj.StudentID;
                        regWorkSheet.Session = null;
                        regWorkSheet.AcaCal_SectionID = AcacalSectionId;
                        regWorkSheet.HeldInRelationId = heldInRelationId;
                        regWorkSheet.YearId = YearId;
                        regWorkSheet.SemesterId = SemesterId;
                        regWorkSheet.CourseTitle = courseObj.Title;
                        regWorkSheet.FormalCode = courseObj.FormalCode;
                        regWorkSheet.VersionCode = courseObj.VersionCode;
                        regWorkSheet.AcaCalTypeName = registrationType;
                        regWorkSheet.ProgramID = studentObj.ProgramID;
                        regWorkSheet.CreatedBy = BaseCurrentUserObj.Id;
                        regWorkSheet.CreatedDate = DateTime.Now;
                        regWorkSheet.ModifiedBy = BaseCurrentUserObj.Id;
                        regWorkSheet.ModifiedDate = DateTime.Now;
                        int result = RegistrationWorksheetManager.Insert(regWorkSheet);
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private int CreateClassRoutineOrGetExistingRoutine(int heldInRelationId, int CourseID, int VersionId)
        {
            int AcacalsectionId = 0;
            try
            {
                var ExistingObj = ucamContext.AcademicCalenderSections.Where(x => x.HeldInRelationId == heldInRelationId
                    && x.CourseID == CourseID).FirstOrDefault();

                if (ExistingObj == null)// create a new one
                {
                    DAL.AcademicCalenderSection NewObj = new DAL.AcademicCalenderSection();

                    NewObj.AcademicCalenderID = 0;
                    NewObj.HeldInRelationId = heldInRelationId;
                    NewObj.CourseID = CourseID;
                    NewObj.VersionID = VersionId;
                    NewObj.SectionName = "A";
                    NewObj.Capacity = 500;
                    NewObj.RoomInfoOneID = 0;
                    NewObj.DayOne = 0;
                    NewObj.TimeSlotPlanOneID = 0;
                    NewObj.TeacherOneID = 0;
                    NewObj.BasicExamTemplateId = 0;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.AcademicCalenderSections.Add(NewObj);
                    ucamContext.SaveChanges();

                    AcacalsectionId = NewObj.AcaCal_SectionID;

                }
                else
                    AcacalsectionId = ExistingObj.AcaCal_SectionID;

            }
            catch (Exception ex)
            {
            }
            return AcacalsectionId;
        }

        private int InsertWorksheetCourseIntoStudentCourseHistory(int studentId, int heldInRelationId)
        {
            int InsertCounter = 0;
            try
            {
                var ExistingList = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == studentId
                    && x.HeldInRelationId == heldInRelationId && x.IsAutoAssign == true && (x.IsRegistered == null || x.IsRegistered == false)).ToList();

                if (ExistingList != null && ExistingList.Any())
                {
                    foreach (var WorksheetItem in ExistingList)
                    {
                        var ExistingCourseHistoryObj = ucamContext.StudentCourseHistories.Where(x => x.StudentID == studentId && x.HeldInRelationId == heldInRelationId
                            && x.CourseID == WorksheetItem.CourseID && x.VersionID == WorksheetItem.VersionID).FirstOrDefault();

                        if (ExistingCourseHistoryObj == null)
                        {
                            DAL.StudentCourseHistory NewCourseHistoryObj = new DAL.StudentCourseHistory();

                            NewCourseHistoryObj.StudentID = studentId;
                            NewCourseHistoryObj.HeldInRelationId = heldInRelationId;
                            NewCourseHistoryObj.AcaCalSectionID = WorksheetItem.AcaCal_SectionID;
                            NewCourseHistoryObj.RetakeNo = WorksheetItem.RetakeNo;
                            NewCourseHistoryObj.CourseStatusID = Convert.ToInt32(CommonEnum.CourseStatus.Running);
                            NewCourseHistoryObj.CourseID = Convert.ToInt32(WorksheetItem.CourseID);
                            NewCourseHistoryObj.VersionID = Convert.ToInt32(WorksheetItem.VersionID);
                            NewCourseHistoryObj.CourseCredit = Convert.ToDecimal(WorksheetItem.CourseCredit);
                            NewCourseHistoryObj.YearId = WorksheetItem.YearId;
                            NewCourseHistoryObj.SemesterId = WorksheetItem.SemesterId;
                            NewCourseHistoryObj.CreatedBy = UserObj.Id;
                            NewCourseHistoryObj.CreatedDate = DateTime.Now;

                            ucamContext.StudentCourseHistories.Add(NewCourseHistoryObj);
                            ucamContext.SaveChanges();

                            int InsertedId = NewCourseHistoryObj.ID;

                            if (InsertedId > 0)
                            {
                                WorksheetItem.IsRegistered = true;
                                WorksheetItem.ModifiedBy = UserObj.Id;
                                WorksheetItem.ModifiedDate = DateTime.Now;

                                ucamContext.Entry(WorksheetItem).State = EntityState.Modified;
                                ucamContext.SaveChanges();

                                InsertCounter++;

                            }

                        }

                    }
                }

            }
            catch (Exception ex)
            {
            }

            return InsertCounter;

        }


        private int CountGridSelected()
        {
            int Counter = 0;

            try
            {
                for (int i = 0; i < gvStudentList.Rows.Count; i++)
                {
                    GridViewRow row = gvStudentList.Rows[i];
                    CheckBox studentCheckd = (CheckBox)row.FindControl("ChkActive");

                    if (studentCheckd.Checked == true)
                    {
                        Counter++;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Counter;
        }

        #endregion

        #region View Enrolled Course

        protected void lnkViewEnrolledCourse_Click(object sender, EventArgs e)
        {
            try
            {
                gvModalEnrolledCourseList.DataSource = null;
                gvModalEnrolledCourseList.DataBind();
                Label2.Text = string.Empty;
                Label3.Text = string.Empty;

                LinkButton btn = (LinkButton)(sender);

                int StudentId = Convert.ToInt32(btn.CommandArgument);

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (StudentId > 0 && HeldInRelationId > 0)
                {
                    Student stdObj = StudentManager.GetById(StudentId);
                    if (stdObj != null)
                    {
                        Label2.Text = "Student ID : " + stdObj.Roll;
                        Label3.Text = "Student Name : " + stdObj.Name;
                    }

                    List<SqlParameter> parameters1 = new List<SqlParameter>();
                    parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });
                    parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                    DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetEnrolledCourseListByStudentHeldInRelationId", parameters1);

                    if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                    {
                        gvModalEnrolledCourseList.DataSource = DataTableCourseList;
                        gvModalEnrolledCourseList.DataBind();
                        ModalPopupEnrolledCourse.Show();
                    }
                    else
                    {
                        showAlert("No Course Registered Yet");
                        return;
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region View And Enrolled Backlog Courses

        protected void lnkViewBacklogCourse_Click(object sender, EventArgs e)
        {
            try
            {

                hdnbacklogStudentID.Value = "0";

                gvBacklogCourseList.DataSource = null;
                gvBacklogCourseList.DataBind();
                Label4.Text = string.Empty;
                Label5.Text = string.Empty;

                ModalPopupBacklogCourse.Hide();

                LinkButton btn = (LinkButton)(sender);

                string Value = btn.CommandArgument.ToString();

                string[] SplittedValue = Value.Split('_');

                int StudentId = 0, YearId = 0, SemesterId = 0;

                if (SplittedValue.Length > 0)
                {
                    StudentId = Convert.ToInt32(SplittedValue[0]);
                    YearId = Convert.ToInt32(SplittedValue[1]);
                    SemesterId = Convert.ToInt32(SplittedValue[2]);

                    hdnbacklogStudentID.Value = StudentId.ToString();
                }

                Student stdObj = StudentManager.GetById(StudentId);
                if (stdObj != null)
                {
                    Label4.Text = "Student ID : " + stdObj.Roll;
                    Label5.Text = "Student Name : " + stdObj.Name;
                }



                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.Int, Value = StudentId });
                parameters1.Add(new SqlParameter { ParameterName = "@YearId", SqlDbType = System.Data.SqlDbType.Int, Value = YearId });
                parameters1.Add(new SqlParameter { ParameterName = "@SemesterId", SqlDbType = System.Data.SqlDbType.Int, Value = SemesterId });
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = Convert.ToInt32(ddlHeldIn.SelectedValue) });

                DataTable DataTableCourseList = DataTableManager.GetDataFromQuery("GetBacklogCourseListByStudentYearSemesterId", parameters1);

                if (DataTableCourseList != null && DataTableCourseList.Rows.Count > 0)
                {
                    gvBacklogCourseList.DataSource = DataTableCourseList;
                    gvBacklogCourseList.DataBind();
                    ModalPopupBacklogCourse.Show();
                }
                else
                {
                    showAlert("No Course Found");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void chkSelectAllbacklog_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopupBacklogCourse.Show();

                CheckBox chk = (CheckBox)sender;

                if (chk.Checked)
                {
                    chk.Text = "Unselect All";
                }
                else
                {
                    chk.Text = "Select All";
                }

                foreach (GridViewRow row in gvBacklogCourseList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActivebacklog");
                    if (ckBox.Enabled)
                    {
                        ckBox.Checked = chk.Checked;
                    }
                }

                if (chk.Checked)
                {
                    int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                    if (heldInRelationId > 0)
                    {
                        int StudentId = Convert.ToInt32(hdnbacklogStudentID.Value);

                        decimal CheckedCredit = 0, TotalCredit = 0;

                        foreach (GridViewRow row in gvBacklogCourseList.Rows)
                        {
                            CheckBox ckBox = (CheckBox)row.FindControl("ChkActivebacklog");
                            if (ckBox.Checked && chk.Enabled)
                            {
                                Label lblCredits = (Label)row.FindControl("lblCourseCredit");

                                try
                                {
                                    CheckedCredit = CheckedCredit + Convert.ToDecimal(lblCredits.Text);
                                }
                                catch (Exception ex)
                                {
                                }

                            }
                        }

                        #region Total Credit Checking (Maximum 30 credit Including backlog courses)

                        var AutoAssignCourseList = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == StudentId
                            && x.HeldInRelationId == heldInRelationId && x.IsAutoAssign == true).ToList();

                        if (AutoAssignCourseList != null && AutoAssignCourseList.Any())
                            TotalCredit = Convert.ToDecimal(AutoAssignCourseList.Sum(x => x.CourseCredit));

                        #endregion

                        TotalCredit = TotalCredit + CheckedCredit;

                        if (TotalCredit > 30)
                        {
                            showAlert("You can not assign more than 30 credits. Please remove some course");

                            foreach (GridViewRow row in gvBacklogCourseList.Rows)
                            {
                                CheckBox ckBox = (CheckBox)row.FindControl("ChkActivebacklog");
                                if (ckBox.Enabled)
                                {
                                    ckBox.Checked = false;
                                }
                            }

                            return;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void ChkActivebacklog_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                ModalPopupBacklogCourse.Show();

                GridViewRow gvrow = (GridViewRow)(((CheckBox)sender)).NamingContainer;

                CheckBox ChkCheck = (CheckBox)gvrow.FindControl("ChkActivebacklog");
                if (ChkCheck.Checked)
                {
                    int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                    if (heldInRelationId > 0)
                    {
                        int StudentId = Convert.ToInt32(hdnbacklogStudentID.Value);

                        decimal CheckedCredit = 0, TotalCredit = 0;

                        foreach (GridViewRow row in gvBacklogCourseList.Rows)
                        {
                            CheckBox ckBox = (CheckBox)row.FindControl("ChkActivebacklog");
                            if (ckBox.Checked && ckBox.Enabled)
                            {
                                Label lblCredits = (Label)row.FindControl("lblCourseCredit");

                                try
                                {
                                    CheckedCredit = CheckedCredit + Convert.ToDecimal(lblCredits.Text);
                                }
                                catch (Exception ex)
                                {
                                }

                            }
                        }

                        #region Total Credit Checking (Maximum 30 credit Including backlog courses)

                        var AutoAssignCourseList = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == StudentId
                            && x.HeldInRelationId == heldInRelationId && x.IsAutoAssign == true).ToList();

                        if (AutoAssignCourseList != null && AutoAssignCourseList.Any())
                            TotalCredit = Convert.ToDecimal(AutoAssignCourseList.Sum(x => x.CourseCredit));

                        #endregion

                        TotalCredit = TotalCredit + CheckedCredit;

                        if (TotalCredit > 30)
                        {
                            showAlert("You can not assign more than 30 credits");
                            ChkCheck.Checked = false;
                            return;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAssignBacklog_Click(object sender, EventArgs e)
        {
            try
            {
                int CourseCount = CountGridSelectedBacklog();
                if (CourseCount == 0)
                {
                    ModalPopupBacklogCourse.Show();
                    showAlert("Please select minimum one course");
                    return;
                }
                int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int InsertCounter = 0;
                if (heldInRelationId > 0)
                {

                    int StudentId = Convert.ToInt32(hdnbacklogStudentID.Value);


                    foreach (GridViewRow row in gvBacklogCourseList.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkActivebacklog");
                        if (ckBox.Checked && ckBox.Enabled)
                        {

                            HiddenField hdnbacklogCourseId = (HiddenField)row.FindControl("hdnbacklogCourseId");
                            HiddenField hdnYearId = (HiddenField)row.FindControl("hdnbacklogYearId");
                            HiddenField hdnSemesterId = (HiddenField)row.FindControl("hdnbacklogSemesterId");

                            int CourseId = Convert.ToInt32(hdnbacklogCourseId.Value);
                            int YearId = Convert.ToInt32(hdnYearId.Value);
                            int SemesterId = Convert.ToInt32(hdnSemesterId.Value);


                            if (StudentId > 0 && YearId > 0 && SemesterId > 0 && CourseId > 0)
                            {
                                LogicLayer.BusinessObjects.Student stdObj = StudentManager.GetById(StudentId);
                                LogicLayer.BusinessObjects.Course courseObj = CourseManager.GetByCourseIdVersionId(CourseId, 1);

                                SectionCreateAndEntryInWorksheetTable(stdObj, courseObj, heldInRelationId, YearId, SemesterId, "B");

                                int Counter = InsertWorksheetCourseIntoStudentCourseHistory(StudentId, heldInRelationId);

                                InsertCounter = InsertCounter + Counter;
                            }
                        }
                    }

                    if (InsertCounter > 0)
                    {
                        showAlert("Backlog Course Registered Successfully");
                    }
                    else
                    {
                        showAlert("Course Registration Failed");
                    }
                    ddlHeldIn_SelectedIndexChanged(null, null);
                }
                else
                {
                    showAlert("Please select a Held In");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private int CountGridSelectedBacklog()
        {
            int Counter = 0;

            try
            {
                for (int i = 0; i < gvBacklogCourseList.Rows.Count; i++)
                {
                    GridViewRow row = gvBacklogCourseList.Rows[i];
                    CheckBox courseCheckd = (CheckBox)row.FindControl("ChkActivebacklog");

                    if (courseCheckd.Checked == true)
                    {
                        Counter++;
                    }
                }
            }
            catch (Exception ex)
            {
            }

            return Counter;
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)(sender);

                int CourseHistoryId = Convert.ToInt32(btn.CommandArgument);

                if (CourseHistoryId > 0)
                {
                    var ExistingObj = ucamContext.StudentCourseHistories.Find(CourseHistoryId);

                    if (ExistingObj != null)
                    {
                        var MarkExists = ucamContext.ExamMarkDetails.Where(x => x.CourseHistoryId == ExistingObj.ID).FirstOrDefault();
                        if (MarkExists != null)
                        {
                            showAlert("You can not remove this course.Mark already entered");
                            ModalPopupEnrolledCourse.Show();
                            return;
                        }

                        var ExistingWorksheetObj = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == ExistingObj.StudentID && x.HeldInRelationId == ExistingObj.HeldInRelationId
                            && x.CourseID == ExistingObj.CourseID && x.VersionID == ExistingObj.VersionID).FirstOrDefault();

                        if (ExistingWorksheetObj != null)
                        {
                            ucamContext.RegistrationWorksheets.Remove(ExistingWorksheetObj);
                            ucamContext.SaveChanges();
                        }

                        if (ExistingObj != null)
                        {
                            ucamContext.StudentCourseHistories.Remove(ExistingObj);
                            ucamContext.SaveChanges();
                        }

                        var RemovedObj = ucamContext.StudentCourseHistories.Find(CourseHistoryId);

                        if (RemovedObj == null)
                        {
                            showAlert("Course Remove Successfully");
                            ddlHeldIn_SelectedIndexChanged(null, null);
                        }
                        else
                        {
                            ModalPopupEnrolledCourse.Show();
                            showAlert("Course Remove Failed");
                            return;
                        }

                    }
                }

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




    }
}