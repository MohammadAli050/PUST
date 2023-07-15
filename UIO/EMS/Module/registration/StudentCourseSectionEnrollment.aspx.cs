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
    public partial class StudentCourseSectionEnrollment : BasePage
    {
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
                ucDepartment.LoadDropDownList();
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                ucAcademicSession.selectedValue = "0";
                LoadHeldInInformation();
                LoadSyllabusInfo();
                LoadCourseDistributionTrimester(0);
                LoadCourseddl();
                divEnrollButtton.Visible = false;
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
                LoadSyllabusInfo();
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
                LoadSyllabusInfo();
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSyllabusInfo()
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                TreeMaster treeObj = new TreeMaster();
                treeObj = TreeMasterManager.GetAllProgramID(programId).FirstOrDefault();
                ddlCalenderDistribution.Items.Clear();
                ddlCalenderDistribution.Items.Add(new ListItem("-Select Distribution-", "0"));
                if (treeObj != null)
                {

                    List<TreeCalendarMaster> treeCalenderList = TreeCalendarMasterManager.GetAllByTreeMasterID(treeObj.TreeMasterID);

                    ddlCalenderDistribution.AppendDataBoundItems = true;

                    if (treeCalenderList != null && treeCalenderList.Any())
                    {
                        ddlCalenderDistribution.DataSource = treeCalenderList.OrderBy(d => d.TreeCalendarMasterID).ToList();
                        ddlCalenderDistribution.DataValueField = "TreeCalendarMasterID";
                        ddlCalenderDistribution.DataTextField = "Name";
                        ddlCalenderDistribution.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ucAcademicSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHeldInInformation();
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

                LoadStudent();

            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCalenderDistribution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearCourseGrid();
                int treeCalMasId = Convert.ToInt32(ddlCalenderDistribution.SelectedValue);
                LoadCourseDistributionTrimester(treeCalMasId);
                LoadCourseddl();
            }
            catch (Exception ex)
            {
            }
        }

        private bool CheckCourseExistInGrid(List<LogicLayer.BusinessObjects.Course> courseList, int courseId)
        {
            int counter = 0;
            for (int i = 0; i < courseList.Count; i++)
            {
                if (courseList[i].CourseID == courseId)
                {
                    counter = 1;
                }
            }
            if (counter == 1)
            {
                return true;
            }
            else { return false; }
        }

        private void LoadCourseDistributionTrimester(int treeCalMasId)
        {
            try
            {
                ddlAddCourseTrimester.Items.Clear();
                ddlAddCourseTrimester.Items.Add(new ListItem("-Select Semester-", "0"));

                List<TreeCalendarDetail> treeCalDetails = TreeCalendarDetailManager.GetByTreeCalenderMasterId(treeCalMasId);

                ddlAddCourseTrimester.AppendDataBoundItems = true;

                if (treeCalDetails != null)
                {
                    ddlAddCourseTrimester.DataSource = treeCalDetails.OrderBy(d => d.CalendarDetailID).ToList();
                    ddlAddCourseTrimester.DataValueField = "TreeCalendarDetailID";
                    ddlAddCourseTrimester.DataTextField = "CalenderUnitDistributionName";
                    ddlAddCourseTrimester.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadCourseddl()
        {
            try
            {

                int programId = Convert.ToInt32(ucProgram.selectedValue);
                List<TreeMaster> treeMasterList = TreeMasterManager.GetAll();
                TreeMaster treeMasterObj = new TreeMaster();
                treeMasterObj = treeMasterList.Where(t => t.ProgramID == programId).FirstOrDefault();
                int treeMasterId = treeMasterObj.TreeMasterID;
                int treeCalMasterId = Convert.ToInt32(ddlCalenderDistribution.SelectedValue);
                int treeCalDetilId = Convert.ToInt32(ddlAddCourseTrimester.SelectedValue);

                List<Course> courseList = CourseManager.GetAllByProgramIdTreeCalMasterIdTreeCalDetailId(programId, treeMasterId, treeCalMasterId, treeCalDetilId);

                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-All Course-", "0"));

                courseList = courseList.OrderBy(d => d.FormalCode).ToList();
                if (courseList != null && courseList.Any())
                {
                    foreach (LogicLayer.BusinessObjects.Course course in courseList)
                        ddlCourse.Items.Add(new ListItem(course.CourseFullInfo, course.CourseID + "_" + course.VersionID));
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlAddCourseTrimester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearCourseGrid();
            btnAddCourseAll_Click(null, null);
            LoadCourseddl();
        }

        private void ClearCourseGrid()
        {
            gvStudentCourse.DataSource = null;
            gvStudentCourse.DataBind();
        }

        protected void btnAddCourseAll_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                List<LogicLayer.BusinessObjects.TreeMaster> treeMasterList = TreeMasterManager.GetAll();
                LogicLayer.BusinessObjects.TreeMaster treeMasterObj = new LogicLayer.BusinessObjects.TreeMaster();
                treeMasterObj = treeMasterList.Where(t => t.ProgramID == programId).FirstOrDefault();
                int treeMasterId = treeMasterObj.TreeMasterID;
                int treeCalMasterId = Convert.ToInt32(ddlCalenderDistribution.SelectedValue);
                int treeCalDetilId = Convert.ToInt32(ddlAddCourseTrimester.SelectedValue);

                List<Course> courseList = CourseManager.GetAllByProgramIdTreeCalMasterIdTreeCalDetailId(programId, treeMasterId, treeCalMasterId, treeCalDetilId);

                if (courseList != null && courseList.Count > 0)
                {
                    gvStudentCourse.DataSource = courseList;
                    gvStudentCourse.DataBind();
                    Session["CourseList"] = courseList;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        private void LoadStudent()
        {
            ClearGridView();
            try
            {
                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable DataTableStudentList = DataTableManager.GetDataFromQuery("GetAllStudentByHeldInRelationId", parameters2);


                if (DataTableStudentList != null && DataTableStudentList.Rows.Count > 0)
                {
                    gvStudentList.DataSource = DataTableStudentList;
                    gvStudentList.DataBind();

                    divEnrollButtton.Visible = true;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int AcacalId = Convert.ToInt32(ucAcademicSession.selectedValue);

                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                parameters2.Add(new SqlParameter { ParameterName = "@AcacalId", SqlDbType = System.Data.SqlDbType.Int, Value = AcacalId });
                parameters2.Add(new SqlParameter { ParameterName = "@YearNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                parameters2.Add(new SqlParameter { ParameterName = "@SemesterNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });

                DataTable DataTableHeldInList = DataTableManager.GetDataFromQuery("GetAllHeldInInformation", parameters2);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("-select-", "0"));


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

        protected void chkSelectAllCourse_CheckedChanged(object sender, EventArgs e)
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

                foreach (GridViewRow row in gvStudentCourse.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("CheckBox");
                    ckBox.Checked = chk.Checked;
                }

                //CheckBox chkHeader = (CheckBox)gvStudentCourse.HeaderRow.FindControl("chkSelectAllCourse");
                //if (chkHeader.Checked)
                //{
                //    for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
                //    {
                //        GridViewRow row = gvStudentCourse.Rows[i];
                //        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                //        studentCheckd.Checked = true;
                //    }
                //}
                //if (!chkHeader.Checked)
                //{
                //    for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
                //    {
                //        GridViewRow row = gvStudentCourse.Rows[i];
                //        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                //        studentCheckd.Checked = false;
                //    }
                //}
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
                divEnrollButtton.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                List<Course> newCourselist = new List<Course>();
                string CourseNameNew = ddlCourse.SelectedValue;
                if (CourseNameNew != "0")
                {
                    int courseId = Convert.ToInt32(CourseNameNew.Split('_').First());
                    int versionId = Convert.ToInt32(CourseNameNew.Split('_').Last());

                    Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);
                    if (courseObj != null)
                        newCourselist.Add(courseObj);

                    gvStudentCourse.Visible = true;
                    gvStudentCourse.DataSource = newCourselist;
                    gvStudentCourse.DataBind();

                    Session["CourseList"] = newCourselist;
                }
                else
                {
                    btnAddCourseAll_Click(null, null);
                }


            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCourseEnrollment_Click(object sender, EventArgs e)
        {
            try
            {
                int StudentCount = CountGridStudent();
                if (StudentCount == 0)
                {
                    showAlert("Please select minimum one student");
                    return;
                }
                int CourseCount = CountGridCourse();
                if (CourseCount == 0)
                {
                    showAlert("Please select minimum one course");
                    return;
                }
                ModalPopupConfirm.Show();


            }
            catch (Exception ex)
            {
            }
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int InsertCounter = 0;
                if (heldInRelationId > 0)
                {
                    foreach (GridViewRow row in gvStudentList.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                        if (ckBox.Checked)
                        {
                            HiddenField lblStudentId = (HiddenField)row.FindControl("hdnStudentID");
                            int studentId = Convert.ToInt32(lblStudentId.Value);

                            if (studentId > 0)
                            {
                                GenerateWorkSheetEntry(studentId, heldInRelationId);

                               int Counter = InsertWorksheetCourseIntoStudentCourseHistory(studentId, heldInRelationId);

                               InsertCounter = InsertCounter + Counter;
                            }
                        }
                    }

                    if(InsertCounter>0)
                    {
                        showAlert("Course Registered Successfully");
                    }
                    else
                    {
                        showAlert("Course Enrollment Failed");
                    }
                    ddlHeldIn_SelectedIndexChanged(null, null);
                    ddlCourse_SelectedIndexChanged(null, null);
                }
                else
                {
                    showAlert("Please select an Exam Information Name");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
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
                            NewCourseHistoryObj.CourseStatusID =Convert.ToInt32(CommonEnum.CourseStatus.Running);
                            NewCourseHistoryObj.CourseID =Convert.ToInt32(WorksheetItem.CourseID);
                            NewCourseHistoryObj.VersionID =Convert.ToInt32( WorksheetItem.VersionID);
                            NewCourseHistoryObj.CourseCredit =Convert.ToDecimal( WorksheetItem.CourseCredit);
                            NewCourseHistoryObj.CreatedBy = UserObj.Id;
                            NewCourseHistoryObj.CreatedDate = DateTime.Now;

                            ucamContext.StudentCourseHistories.Add(NewCourseHistoryObj);
                            ucamContext.SaveChanges();

                            int InsertedId = NewCourseHistoryObj.ID;

                            if(InsertedId>0)
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


        private void GenerateWorkSheetEntry(int studentId, int heldInRelationId)
        {
            try
            {
                string registrationType = null;

                registrationType = "R"; // R for Regular Course

                LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                if (studentObj != null)
                {
                    for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentCourse.Rows[i];
                        Label lblCourseId = (Label)row.FindControl("lblCourseId");
                        Label lblVersionId = (Label)row.FindControl("lblVersionId");
                        Label lblCourseCredit = (Label)row.FindControl("lblCourseCredit");
                        CheckBox courseCheckd = (CheckBox)row.FindControl("CheckBox");
                        LogicLayer.BusinessObjects.Course courseObj = CourseManager.GetByCourseIdVersionId(Convert.ToInt32(lblCourseId.Text), Convert.ToInt32(lblVersionId.Text));

                        if (courseCheckd.Checked == true)
                        {
                            int AcacalSectionId = 0;
                            try
                            {
                                AcacalSectionId = CreateClassRoutineOrGetExistingRoutine(heldInRelationId, courseObj.CourseID, courseObj.VersionID);
                            }
                            catch (Exception ex)
                            {
                            }

                            var existingObj = ucamContext.RegistrationWorksheets.Where(x => x.StudentID == studentId && x.HeldInRelationId == heldInRelationId
                                && x.CourseID == courseObj.CourseID).FirstOrDefault();

                            if (existingObj == null)
                            {
                                RegistrationWorksheet regWorkSheet = new RegistrationWorksheet();
                                regWorkSheet.CourseID = courseObj.CourseID;
                                regWorkSheet.VersionID = courseObj.VersionID;
                                regWorkSheet.Credits = courseObj.Credits;
                                regWorkSheet.IsAutoOpen = true;
                                regWorkSheet.IsAutoAssign = true;
                                regWorkSheet.BatchID = studentObj.BatchId;
                                regWorkSheet.RetakeNo = Convert.ToInt32(CommonEnum.CourseRegistrationStatus.Regular);
                                regWorkSheet.StudentID = studentObj.StudentID;
                                regWorkSheet.Session = null;
                                regWorkSheet.AcaCal_SectionID = AcacalSectionId;
                                regWorkSheet.HeldInRelationId = heldInRelationId;
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
                    //RegistrationWorksheetManager.UpdateCourseRegType(studentObj.StudentID, acaCalId);
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


        private int CountGridStudent()
        {
            int studentCounter = 0;
            for (int i = 0; i < gvStudentList.Rows.Count; i++)
            {
                GridViewRow row = gvStudentList.Rows[i];
                CheckBox studentCheckd = (CheckBox)row.FindControl("ChkActive");

                if (studentCheckd.Checked == true)
                {
                    studentCounter = studentCounter + 1;
                }
            }
            return studentCounter;
        }

        private int CountGridCourse()
        {
            int courseCounter = 0;
            for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
            {
                GridViewRow row = gvStudentCourse.Rows[i];
                CheckBox courseCheckd = (CheckBox)row.FindControl("CheckBox");

                if (courseCheckd.Checked == true)
                {
                    courseCounter = courseCounter + 1;
                }
            }
            return courseCounter;
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

    }
}