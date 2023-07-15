using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.registration
{
    public partial class StudentCourseSectionAssign : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //LoadYearDDL(programId);
                //ucSession.LoadDropDownList(programId);

                TreeMaster treeObj = new TreeMaster();
                treeObj = TreeMasterManager.GetAllProgramID(programId).FirstOrDefault();
                if (treeObj != null)
                {
                    LoadTreeCalender(treeObj.TreeMasterID);
                }

                //ddlYear.Items.Clear();
                //ddlYear.AppendDataBoundItems = true;
                //ddlYear.Items.Add(new ListItem("-Select-", "0"));

                //ddlSemester.Items.Clear();
                //ddlSemester.AppendDataBoundItems = true;
                //ddlSemester.Items.Add(new ListItem("-Select-", "0"));

                pnlEnrollment.Visible = false;

                LoadYearNoDDL();
                LoadSemesterNoDDL();

                ddlExam.Items.Clear();
                ddlExam.Items.Add(new ListItem("-Select Exam-", "0"));
                ddlExam.AppendDataBoundItems = true;
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e) 
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //LoadYearDDL(programId);
                
                TreeMaster treeObj = new TreeMaster();
                treeObj = TreeMasterManager.GetAllProgramID(programId).FirstOrDefault();
                if (treeObj != null)
                {
                    LoadTreeCalender(treeObj.TreeMasterID);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            TreeMaster treeObj = new TreeMaster();
            treeObj = TreeMasterManager.GetAllProgramID(programId).FirstOrDefault();
            if (treeObj != null)
            {
                LoadTreeCalender(treeObj.TreeMasterID);
            }
        }

        protected void ucSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadStudent();
        }

        //private void LoadYearDDL(int programId)
        //{
        //    List<Year> yearList = new List<Year>();
        //    yearList = YearManager.GetByProgramId(programId);

        //    ddlYear.Items.Clear();
        //    ddlYear.AppendDataBoundItems = true;

        //    if (yearList != null)
        //    {
        //        ddlYear.Items.Add(new ListItem("-Select-", "0"));
        //        ddlYear.DataTextField = "YearName";
        //        ddlYear.DataValueField = "YearId";
        //        if (yearList != null)
        //        {
        //            ddlYear.DataSource = yearList.OrderBy(b => b.YearId).ToList();
        //            ddlYear.DataBind();
        //        }
        //    }
        //}

        //protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int yearId = Convert.ToInt32(ddlYear.SelectedValue);
        //    LoadYearSemesterDDL(yearId);
        //    LoadStudent();
        //}

        //private void LoadYearSemesterDDL(int yearId)
        //{
        //    List<Semester> semesterList = new List<Semester>();
        //    semesterList = SemesterManager.GetByYearId(yearId);

        //    ddlSemester.Items.Clear();
        //    ddlSemester.AppendDataBoundItems = true;

        //    if (semesterList != null)
        //    {
        //        ddlSemester.Items.Add(new ListItem("-Select-", "0"));
        //        ddlSemester.DataTextField = "SemesterName";
        //        ddlSemester.DataValueField = "SemesterId";
        //        if (semesterList != null)
        //        {
        //            ddlSemester.DataSource = semesterList.OrderBy(b => b.SemesterId).ToList();
        //            ddlSemester.DataBind();
        //        }
        //    }
        //}

        //protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    LoadStudent();
        //}

        private void LoadYearNoDDL()
        {
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();


            ddlYearNo.Items.Clear();
            ddlYearNo.AppendDataBoundItems = true;
            ddlYearNo.Items.Add(new ListItem("-Select-", "-1"));
            if (yearList != null && yearList.Count > 0)
            {
                ddlYearNo.DataTextField = "YearNoName";
                ddlYearNo.DataValueField = "YearNo";

                ddlYearNo.DataSource = yearList;
                ddlYearNo.DataBind();
            }
        }

        private void LoadSemesterNoDDL()
        {
            List<SemesterDistinctDTO> semesterList = new List<SemesterDistinctDTO>();
            semesterList = SemesterManager.GetAllDistinct();
            semesterList = semesterList.OrderBy(x => x.SemesterNo).ToList();


            ddlSemesterNo.Items.Clear();
            ddlSemesterNo.AppendDataBoundItems = true;
            ddlSemesterNo.Items.Add(new ListItem("-Select-", "-1"));
            if (semesterList != null && semesterList.Count > 0)
            {
                ddlSemesterNo.DataTextField = "SemesterNoName";
                ddlSemesterNo.DataValueField = "SemesterNo";

                ddlSemesterNo.DataSource = semesterList;
                ddlSemesterNo.DataBind();

            }
        }

        protected void ddlYearNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                loadExamDropdown(programId, yearNo, semesterNo);
            }
            //LoadStudent();
        }

        protected void ddlSemesterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;

            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                loadExamDropdown(programId, yearNo, semesterNo);
            }
            
            LoadStudent();
        }

        private void loadExamDropdown(int programId, int yearNo, int semesterNo)
        {
            ddlExam.Items.Clear();
            ddlExam.Items.Add(new ListItem("-Select Exam-", "0"));
            ddlExam.AppendDataBoundItems = true;
            List<ExamSetupDetailDTO> exam = ExamSetupManager.ExamSetupDetailGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
            if (exam != null)
            {
                foreach (ExamSetupDetailDTO examlist in exam)
                {
                    ddlExam.Items.Add(new ListItem(examlist.ExamName, examlist.ExamSetupDetailId.ToString()));

                }
            }
        }

        protected void txtStudentId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetByRoll(Convert.ToString(txtStudentId.Text.Trim()));
                if (studentObj != null)
                {
                    gvStudentList.DataSource = null;
                    gvStudentList.DataBind();

                    List<LogicLayer.BusinessObjects.Student> studentObjList = new List<LogicLayer.BusinessObjects.Student>();
                    studentObjList.Add(studentObj);
                    if (studentObjList != null)
                    {
                        gvStudentList.DataSource = studentObjList.OrderBy(d=> d.Roll);
                        gvStudentList.DataBind();
                    }

                    if (gvStudentCourse.Rows.Count > 0)
                    {
                        //btnWorkSheet.Visible = true;
                        //ddlCourseStatus.Visible = true;
                        pnlEnrollment.Visible = true;
                    }
                    else
                    {
                        pnlEnrollment.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string CourseNameNew = ddlCourse.SelectedValue;
                if (CourseNameNew != null)
                {
                    int courseId = Convert.ToInt32(CourseNameNew.Split('_').First());
                    int versionId = Convert.ToInt32(CourseNameNew.Split('_').Last());

                    LogicLayer.BusinessObjects.Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);

                    List<LogicLayer.BusinessObjects.Course> courselist = new List<LogicLayer.BusinessObjects.Course>();
                    var courseSessionList = Session["CourseList"];
                    courselist = courseSessionList as List<LogicLayer.BusinessObjects.Course>;
                    if (courselist != null)
                    {
                        for (int i = 0; i < courselist.Count; i++)
                        {
                            if (CheckCourseExistInGrid(courselist, courseObj.CourseID))
                            {
                                lblMsg.Text = "";
                            }
                            else
                            {
                                courselist.Add(courseObj);
                                lblMsg.Text = "";
                            }
                        }
                    }
                    else
                    {
                        List<LogicLayer.BusinessObjects.Course> newCourselist = new List<LogicLayer.BusinessObjects.Course>();
                        newCourselist.Add(courseObj);
                        courselist = newCourselist;
                    }
                    gvStudentCourse.Visible = true;
                    gvStudentCourse.DataSource = courselist;
                    gvStudentCourse.DataBind();
                    //btnRegistration.Visible = false;
                    //btnRegistrationAndBilling.Visible = true;
                    if (gvStudentCourse.Rows.Count > 0)
                    {
                        //btnWorkSheet.Visible = true;
                        //ddlCourseStatus.Visible = true;
                        pnlEnrollment.Visible = true;
                    }
                    else
                    {
                        //btnWorkSheet.Visible = false;
                        //ddlCourseStatus.Visible = false;
                        pnlEnrollment.Visible = false;
                    }
                    Session["CourseList"] = courselist;
                }
                else
                {
                    lblMsg.Text = "Please select a course.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private bool CheckCourseExistInGrid(List<LogicLayer.BusinessObjects.Course> courseList, int courseId)
        {
            //LogicLayer.BusinessObjects.Course courseObj = courseList.Where(d=> d.CourseID == courseId).FirstOrDefault();
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

                List<LogicLayer.BusinessObjects.Course> courseList = CourseManager.GetAllByProgramIdTreeCalMasterIdTreeCalDetailId(programId, treeMasterId, treeCalMasterId, treeCalDetilId);

                if (courseList != null && courseList.Count > 0)
                {
                    gvStudentCourse.Visible = true;
                    gvStudentCourse.DataSource = courseList;
                    gvStudentCourse.DataBind();
                    //btnWorkSheet.Visible = true;
                    //ddlCourseStatus.Visible = true;
                    pnlEnrollment.Visible = true;
                    Session["CourseList"] = courseList;
                }
                else
                {
                    lblMsg.Text = "Please select a course.";
                }
            }
            catch (Exception ex)
            {

            }
        }

        //private void LoadTreeDDL(int programId)
        //{
        //    List<TreeMaster> treeList = new List<TreeMaster>();
        //    treeList = TreeMasterManager.GetAllProgramID(programId);

        //    ddlTree.Items.Clear();
        //    ddlTree.AppendDataBoundItems = true;

        //    if (treeList != null)
        //    {
        //        ddlTree.Items.Add(new ListItem("-Select-", "0"));
        //        ddlTree.DataTextField = "Node_Name";
        //        ddlTree.DataValueField = "TreeMasterID";
        //        if (treeList != null)
        //        {
        //            ddlTree.DataSource = treeList.OrderBy(b => b.TreeMasterID).ToList();
        //            ddlTree.DataBind();
        //        }
        //    }
        //}

        //protected void ddlTree_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    int treeId = Convert.ToInt32(ddlTree.SelectedValue);
        //    LoadTreeCalender(treeId);
        //}

        private void LoadTreeCalender(int treeId)
        {
            try
            {
                ddlCalenderDistribution.Items.Clear();
                ddlCalenderDistribution.Items.Add(new ListItem("-Select Distribution-", "0"));
                List<LogicLayer.BusinessObjects.TreeCalendarMaster> treeCalenderList = TreeCalendarMasterManager.GetAllByTreeMasterID(treeId);

                ddlCalenderDistribution.AppendDataBoundItems = true;

                if (treeCalenderList != null)
                {
                    ddlCalenderDistribution.DataSource = treeCalenderList.OrderBy(d => d.TreeCalendarMasterID).ToList();
                    ddlCalenderDistribution.DataValueField = "TreeCalendarMasterID";
                    ddlCalenderDistribution.DataTextField = "Name";
                    ddlCalenderDistribution.DataBind();
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
            finally { }
        }

        protected void ddlCalenderDistribution_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int treeCalMasId = Convert.ToInt32(ddlCalenderDistribution.SelectedValue);
                LoadCourseDistributionTrimester(treeCalMasId);
                LoadCourseddl();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void LoadCourseDistributionTrimester(int treeCalMasId)
        {
            try
            {
                ddlAddCourseTrimester.Items.Clear();
                ddlAddCourseTrimester.Items.Add(new ListItem("-Select Semester-", "0"));

                List<LogicLayer.BusinessObjects.TreeCalendarDetail> treeCalDetails = TreeCalendarDetailManager.GetByTreeCalenderMasterId(treeCalMasId);

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
                lblMsg.Text = ex.Message;
            }
        }

        private void LoadCourseddl()
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

                List<LogicLayer.BusinessObjects.Course> courseList = CourseManager.GetAllByProgramIdTreeCalMasterIdTreeCalDetailId(programId, treeMasterId, treeCalMasterId, treeCalDetilId);

                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0"));

                courseList = courseList.OrderBy(d => d.FormalCode).ToList();
                if (courseList.Count > 0 && courseList != null)
                {
                    foreach (LogicLayer.BusinessObjects.Course course in courseList)
                        ddlCourse.Items.Add(new ListItem(course.CourseFullInfo, course.CourseID + "_" + course.VersionID));
                }
            }
            catch (Exception ex)
            {
                //lblMsg.Text = ex.Message;
            }
        }

        protected void ddlAddCourseTrimester_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnAddCourseAll_Click(null, null);
            LoadCourseddl();
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {

        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkHeader = (CheckBox)gvStudentCourse.HeaderRow.FindControl("chkSelectAll");
                if (chkHeader.Checked)
                {
                    for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentCourse.Rows[i];
                        //Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = true;
                    }
                }
                if (!chkHeader.Checked)
                {
                    for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentCourse.Rows[i];
                        //Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }    

        private void LoadStudent()
        {
            try
            {
                //Session["CourseList"] = null;
                gvStudentList.DataSource = null;
                gvStudentList.DataBind();

                int currentSessionId = Convert.ToInt32(ucCurrentSession.selectedValue);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                List<Student> studentList = new List<Student>();

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    studentList = StudentManager.GetAllByProgramIdYearNoSemsterNoCurrentSessionIdForRegistration(programId, yearNo, semesterNo, currentSessionId);

                    gvStudentList.DataSource = studentList;
                    gvStudentList.DataBind();
                }
                else 
                {
                    lblMsg.Text = "Please select program, current session, year and semester";
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void chkSelectAllStudent_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkHeader = (CheckBox)gvStudentList.HeaderRow.FindControl("chkSelectAllStudent");
                if (chkHeader.Checked)
                {
                    for (int i = 0; i < gvStudentList.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentList.Rows[i];
                        //Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = true;
                    }
                }
                if (!chkHeader.Checked)
                {
                    for (int i = 0; i < gvStudentList.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentList.Rows[i];
                        //Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnWorkSheet_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(ddlCourseStatus.SelectedValue) == 0)
                {
                    lblMsg.Text = "Please Select RegType. Then try again.";
                    return;
                }

                //string currentStudentSession = Convert.ToString(ucCurrentSession.selectedText);
                //string currentVersitySession = Convert.ToString(ucVersitySession.selectedText);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int yearId = 0;
                int semesterId = 0;
                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                if (programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    Year yearObj = YearManager.GetByProgramId(programId).Where(d => d.YearNo == yearNo).FirstOrDefault();
                    if (yearObj != null)
                    {
                        yearId = yearObj.YearId;
                        Semester semesterObj = SemesterManager.GetByYearId(yearId).Where(d => d.SemesterNo == semesterNo).FirstOrDefault();
                        if (semesterObj != null)
                        {
                            semesterId = semesterObj.SemesterId;
                        }
                    }

                   

                    foreach (GridViewRow row in gvStudentList.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("CheckBox");
                        if (ckBox.Checked)
                        {
                            Label lblStudentId = (Label)row.FindControl("lblStudentId");
                            int studentId = Convert.ToInt32(lblStudentId.Text);

                            List<LogicLayer.BusinessObjects.Course> courselist = new List<LogicLayer.BusinessObjects.Course>();
                            var courseSessionList = Session["CourseList"];
                            courselist = courseSessionList as List<LogicLayer.BusinessObjects.Course>;
                            if (studentId > 0 && courselist.Count > 0)
                            {
                                GenerateWorkSheetEntry(studentId, yearId, semesterId, yearNo, semesterNo, examId);

                                //btnWorkSheet.Visible = true;
                                //ddlCourseStatus.Visible = true;
                                pnlEnrollment.Visible = true;
                            }
                            else
                            {
                                lblMsg.Text = "Please provide student roll, rgistartion session, and course information";
                            }
                        }
                    }
                    string returnMessage = SectionCreateCourseOfferAssignSection(programId, yearId, semesterId, yearNo, semesterNo, examId);
                    //Session["CourseList"] = null;
                    lblMsg.Text = returnMessage;
                }
                else 
                {
                    lblMsg.Text = "Please provide year, session and exam to open courses for enrollment.";
                }

                if (!string.IsNullOrEmpty(txtStudentId.Text.Trim()))
                {
                    txtStudentId_TextChanged(null, null);
                }
                else
                {
                    LoadStudent();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private string SectionCreateCourseOfferAssignSection(int programId, int yearId, int semesterId, int yearNo, int semesterNo, int examId) 
        {
            string returnMessage = null;
            try
            {
                int checkedCounter = 0;
                int insertCounter = 0;
                bool isInserted = false;

                ///OfferedCourseManager.GenerateOfferedCourse(programId, yearId, semesterId, acaCalId);
                AcademicCalenderSectionManager.GenerateSection(programId, yearId, semesterId, yearNo, semesterNo, examId);
                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("CheckBox");
                    if (ckBox.Checked)
                    {
                        checkedCounter = checkedCounter + 1;
                        Label lblStudentId = (Label)row.FindControl("lblStudentId");
                        int studentId = Convert.ToInt32(lblStudentId.Text);
                        if (studentId > 0 && yearNo > 0 && semesterNo > 0)
                        {
                            isInserted = UpdateWorkSheetWithSection(programId, studentId, yearNo, semesterNo, examId);
                            if(isInserted)
                            { insertCounter = insertCounter +1;}
                            pnlEnrollment.Visible = true;
                        }
                        else
                        {
                            returnMessage = "Please provide student roll, rgistartion session, and course information";
                        }
                    }
                }

                returnMessage = "Student checked for registration : " + checkedCounter + " and successfully registration done for : " + insertCounter + " students";
                return returnMessage;
            }
            catch (Exception ex) { return returnMessage; }
        }

        private bool UpdateWorkSheetWithSection(int programId, int studentId, int yearNo, int semesterNo, int examId)
        {
            try
            {
                bool isresultInserted = false;
                int workSheetInsertCount = 0;
                int courseHistoryInsertCount = 0;
                LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                List<RegistrationWorksheet> registrationWorksheetList = RegistrationWorksheetManager.GetByStudentIdYearNoSemesterNoExamId(studentObj.StudentID, yearNo, semesterNo, examId);
                if (studentObj != null && registrationWorksheetList!= null)
                {
                    for (int i = 0; i < registrationWorksheetList.Count; i++)
                    {
                        RegistrationWorksheet registrationWorksheetObj = registrationWorksheetList[i];
                        AcademicCalenderSection acacalSectioObj = AcademicCalenderSectionManager.GetByProgramIdExamIdYearNoSemesterNoCourseIdVersionId(programId, examId, yearNo, semesterNo, registrationWorksheetObj.CourseID, registrationWorksheetObj.VersionID).FirstOrDefault();
                        if (registrationWorksheetObj != null && acacalSectioObj != null)
                        {
                            registrationWorksheetObj.AcaCal_SectionID = acacalSectioObj.AcaCal_SectionID;
                            registrationWorksheetObj.RetakeNo = 2;
                            registrationWorksheetObj.IsRegistered = true;
                            registrationWorksheetObj.ModifiedBy = Convert.ToInt32(BaseCurrentUserObj.Id);
                            registrationWorksheetObj.ModifiedDate = DateTime.Now;

                            bool result = RegistrationWorksheetManager.Update(registrationWorksheetObj);
                            if (result)
                            {
                                workSheetInsertCount = workSheetInsertCount + 1;
                                bool resultInsert = InsertCourseHistory(registrationWorksheetObj);
                                if (resultInsert)
                                {
                                    courseHistoryInsertCount = courseHistoryInsertCount + 1;
                                }
                            }
                        }
                    }
                }

                if (workSheetInsertCount == courseHistoryInsertCount)
                {
                    isresultInserted = true;
                }
                return isresultInserted;
            }
            catch (Exception ex) { return false; }
        }

        private void GenerateWorkSheetEntry(int studentId, int yearId, int semesterId, int yearNo, int semesterNo, int examId)
        {
            try
            {
                //int acaCalId = Convert.ToInt32(ucSession.selectedValue);
                string registrationType = null;
                int registrationTypeId = Convert.ToInt32(ddlCourseStatus.SelectedValue);
                if (registrationTypeId == 9)
                {
                    registrationType = "R";
                }
                if (registrationTypeId == 12)
                {
                    registrationType = "IM";

                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    ExamSetupDetailDTO examObj = ExamSetupManager.ExamSetupDetailGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo).Where(d => d.ExamSetupDetailId == examId).FirstOrDefault();

                    Year yearObj = YearManager.GetByProgramId(programId).Where(d => d.YearNo == examObj.YearNo).FirstOrDefault();
                    if (yearObj != null)
                    {
                        yearId = yearObj.YearId;
                        Semester semesterObj = SemesterManager.GetByYearId(yearId).Where(d => d.SemesterNo == examObj.SemesterNo).FirstOrDefault();
                        if (semesterObj != null)
                        {
                            semesterId = semesterObj.SemesterId;
                        }
                    }
                }


                LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetById(studentId);
                //List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(studentObj.StudentID, Convert.ToInt32(ucSession.selectedValue));
                if (studentObj != null)
                {
                    for (int i = 0; i < gvStudentCourse.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentCourse.Rows[i];
                        Label lblCourseId = (Label)row.FindControl("lblCourseId");
                        Label lblVersionId = (Label)row.FindControl("lblVersionId");
                        Label lblCourseCredit = (Label)row.FindControl("lblCourseCredit");
                        CheckBox courseCheckd = (CheckBox)row.FindControl("CheckBox");
                        //DropDownList ddlCourseStatus = (DropDownList)row.FindControl("ddlCourseStatus");
                        if (courseCheckd.Checked == true && Convert.ToInt32(ddlCourseStatus.SelectedValue) != 0)
                        {
                            LogicLayer.BusinessObjects.Course courseObj = CourseManager.GetByCourseIdVersionId(Convert.ToInt32(lblCourseId.Text), Convert.ToInt32(lblVersionId.Text));
                            RegistrationWorksheet regWorkSheet = new RegistrationWorksheet();
                            regWorkSheet.CourseID = courseObj.CourseID;
                            regWorkSheet.VersionID = courseObj.VersionID;
                            regWorkSheet.Credits = courseObj.Credits;
                            regWorkSheet.IsAutoOpen = true;
                            regWorkSheet.IsAutoAssign = true;
                            regWorkSheet.BatchID = studentObj.BatchId;
                            regWorkSheet.RetakeNo = 1;
                            //regWorkSheet.Node_CourseID = nodeCourseId;
                            regWorkSheet.StudentID = studentObj.StudentID;
                            regWorkSheet.Session = null;
                            regWorkSheet.YearId = yearId;
                            regWorkSheet.SemesterId = semesterId;

                            //regWorkSheet.OriginalCalID = Convert.ToInt32(ucSession.selectedValue);
                            regWorkSheet.YearNo = yearNo;
                            regWorkSheet.SemesterNo = semesterNo;
                            regWorkSheet.ExamId = examId;
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
                            if (result > 0)
                            {
                                lblMsg.Text = "Course inserted successfully.";
                            }
                            else
                            {
                                lblMsg.Text = "Course could not inserted successfully.";
                            }

                        }
                    }
                    //RegistrationWorksheetManager.UpdateCourseRegType(studentObj.StudentID, acaCalId);
                }
            }
            catch (Exception ex) { lblMsg.Text = ex.Message; }
        }

        protected void btnLoadStudent_Click(object sender, EventArgs e)
        {
            LoadStudent();
        }

        private bool InsertCourseHistory(RegistrationWorksheet registrationWorksheetObj)
        {
            StudentCourseHistory studentCourseHistory = new StudentCourseHistory();
            List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdYearNoSemesterNoExamId(registrationWorksheetObj.StudentID, registrationWorksheetObj.YearNo, registrationWorksheetObj.SemesterNo, registrationWorksheetObj.ExamId);
            studentCourseHistory = studentCourseHistoryList.Find(o => o.CourseStatusID == (int)CommonEnum.CourseStatus.Regular &&
                                                                        o.CourseID == registrationWorksheetObj.CourseID &&
                                                                        o.VersionID == registrationWorksheetObj.VersionID);
            bool isInserted = false;
            if (studentCourseHistory == null)
            {
                StudentCourseHistory studentCourseHistoryInserObj = new StudentCourseHistory();
                studentCourseHistoryInserObj.StudentID = Convert.ToInt32(registrationWorksheetObj.StudentID);
                if (registrationWorksheetObj.AcaCalTypeName == "IM")
                {
                    studentCourseHistoryInserObj.RetakeNo = 12;
                    studentCourseHistoryInserObj.CourseStatusID = (int)CommonEnum.CourseStatus.Improvement;
                }
                else if (registrationWorksheetObj.AcaCalTypeName == "R")
                {
                    studentCourseHistoryInserObj.RetakeNo = 9;
                    studentCourseHistoryInserObj.CourseStatusID = (int)CommonEnum.CourseStatus.Regular;
                }
                
                studentCourseHistoryInserObj.AcaCalID = registrationWorksheetObj.OriginalCalID;
                studentCourseHistoryInserObj.CourseID = registrationWorksheetObj.CourseID;
                studentCourseHistoryInserObj.VersionID = registrationWorksheetObj.VersionID;
                studentCourseHistoryInserObj.CourseCredit = registrationWorksheetObj.Credits;
                studentCourseHistoryInserObj.AcaCalSectionID = registrationWorksheetObj.AcaCal_SectionID;
                studentCourseHistoryInserObj.YearNo = registrationWorksheetObj.YearNo;
                studentCourseHistoryInserObj.SemesterNo = registrationWorksheetObj.SemesterNo;
                studentCourseHistoryInserObj.ExamId = registrationWorksheetObj.ExamId;
                studentCourseHistoryInserObj.YearId = registrationWorksheetObj.YearId;
                studentCourseHistoryInserObj.SemesterId = registrationWorksheetObj.SemesterId;
                studentCourseHistoryInserObj.CourseStatusDate = DateTime.Now;
                studentCourseHistoryInserObj.CreatedBy = Convert.ToInt32(BaseCurrentUserObj.Id);
                studentCourseHistoryInserObj.CreatedDate = DateTime.Now;
                studentCourseHistoryInserObj.ModifiedBy = Convert.ToInt32(BaseCurrentUserObj.Id);
                studentCourseHistoryInserObj.ModifiedDate = DateTime.Now;
                int result = StudentCourseHistoryManager.Insert(studentCourseHistoryInserObj);
                if(result > 0)
                {
                    isInserted = true;
                }
                else
                {
                    isInserted = false;
                }
            }
            return isInserted;
        }
    }
}