using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System.Drawing;
using LogicLayer.BusinessObjects.DTO;
using Microsoft.Reporting.WebForms;
using System.IO;
using LogicLayer.BusinessObjects.RO;

namespace EMS.Module.registration
{
    public partial class SingleStudentRegistration : BasePage
    {
        private string AddRegistrationWorksheetId = "AddRegistrationWorksheet";
        private string SessionStudentId = "Registration_StudentId";
        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            
            if (!IsPostBack)
            {
                //btnDownload.Visible = false;
                LoadCurrentRegSessions();

                User user = UserManager.GetById(userObj.Id);
                if (user.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
                {
                    Student student = StudentManager.GetBypersonID(user.Person.PersonID);
                    if (student != null)
                    {
                        txtStudent.Text = student.Roll.ToString();
                        btnLoad.Visible = false;
                        btnLoad_Click(null, null);
                    }
                }
                else if (Session["StudentId"] != null)
                {
                    int id = Convert.ToInt32(Session["StudentId"]);
                    Student obj = StudentManager.GetById(id);
                    txtStudent.Text = obj.Roll.ToString();
                    btnLoad_Click(null, null);
                }
                else
                {
                    btnLoad.Visible = true;
                    CleareGrid();
                }
            }
        }

        #region Load Student
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            
            if (string.IsNullOrEmpty(txtStudent.Text))
            {
                ShowAlertMessage(" Please provide a Student Id.");
                return;
            }           

            if (userObj.RoleID == Convert.ToInt32(CommonEnum.Role.Student) && userObj.LogInID != txtStudent.Text.Trim())
            {
                ShowAlertMessage("You are unable to view other Student.");
                return;
            }
            Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
            if (student != null)
            {
                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Advisor) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Faculty))
                {
                    User user = UserManager.GetById(userObj.Id);
                    StudentAdvisor studentAdvisor = StudentAdvisorManager.GetByStudentIdAcaCalId(student.StudentID, acaCalId);
                    if (studentAdvisor != null)
                    {
                        if (studentAdvisor.TeacherID == user.Person.Employee.EmployeeID)
                        {
                            LoadStudent(student);
                            
                        }
                        else
                        {
                            CleareGrid();
                            ShowAlertMessage("You are unable to view this Student. Please provide Student Id to whom you set as advisor.");
                            return;
                        }
                    }
                    else
                    {
                        CleareGrid();
                        ShowAlertMessage("No advisor is set for this student.");
                        return;
                    }
                }
                else if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Head) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ExecutiveG2))
                {
                    bool hasAccess = false;
                    UserAccessProgram programAccess = UserAccessProgramManager.GetByUserId(userObj.Id);
                    string[] accessCode = programAccess.AccessPattern.Split('-');
                    foreach (string s in accessCode)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            if (student.ProgramID == Convert.ToInt32(s))
                                hasAccess = true;
                        }
                    }
                    if (hasAccess)
                    {
                        LoadStudent(student);
                    }
                    else
                    {
                        CleareGrid();
                        ShowAlertMessage("You are unable to view this Student. Please provide Student Id from your program.");
                        return;
                    }
                }
                else
                {
                    LoadStudent(student);
                    GridReBind();
                }
            }
            else 
            {
                ShowAlertMessage("Student Not Found.");
                return;
            }
        }

        private void LoadStudent(Student studentObj)
        {
            FillStudentInfo(studentObj);
            LoadStudentCourse(studentObj.StudentID);
        }

        private void FillStudentInfo(Student student)
        {
            lblProgram.Text = student.Program.ShortName;
            lblBatch.Text = student.Batch.BatchNO.ToString();
            lblName.Text = student.BasicInfo.FullName;
        }

        private void LoadStudentCourse(int studentId)
        {
            List<RegistrationWorksheet> collection = null;
           
            Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
            int sectionCount = 0;
            decimal creditCount = 0;
            decimal AssignCredit = 0;

            if (student.CampusID != 2)
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                StudentEarnedCredit studentEarnedCreditObj = StudentManager.GetByStudentIdAcaCalId(student.StudentID, acaCalId);
                if (student.ProgramID != 13)
                {
                    if (studentEarnedCreditObj.LevelNo == 1)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("0 < Total Earned Credit <= 36");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 2)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("36 <Total Earned Credit <=72");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 3)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("72 < Total Earned Credit <=108");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 4)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("Total Earned Credit > 108");
                    }
                }
                else
                {
                    if (studentEarnedCreditObj.LevelNo == 1)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("0 <Total Earned Credit <=34");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 2)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("34 <Total Earned Credit <=72");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 3)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("72 < Total Earned Credit <=110");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 4)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("110 < Total Earned Credit <=147");
                    }
                    else if (studentEarnedCreditObj.LevelNo == 5)
                    {
                        lblLevel.Text = HttpUtility.HtmlEncode("Total Earned Credit > 147");
                    }
                }
            }
            else 
            { 
                lblLevel.Text = string.Empty;
            }

            if (student == null)
            {
                ShowAlertMessage(" Student not found.");
                return;
            }
            // AcademicCalender acaCal = AcademicCalenderManager.GetActiveRegistrationCalender();
            int AcademicCalenderID = Convert.ToInt32(ddlSession.SelectedValue);
            //List<AcademicCalender> activeRegistrationSession = AcademicCalenderManager.GetActiveRegistrationCalenders();

            collection = RegistrationWorksheetManager.GetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID(studentId, AcademicCalenderID);

            if (collection != null && collection.Count > 0)
            {
                string courses = "";

                foreach (var item in collection)
                {
                    if (item.IsAdd == true)
                    {
                        courses += item.FormalCode + ", ";
                    }
                }
                //lblForwardedCourse.Text = courses;
            }

            if (collection != null)
            {
                sectionCount = collection.Count(c => c.IsRegistered == true);
                creditCount = collection.Where(c => c.IsRegistered == true).Sum(c => c.Credits);
                collection = collection.Where(c => c.CourseStatusId != -1).ToList();
                AssignCredit = collection.Where(x => x.IsAutoAssign == true && x.AcaCal_SectionID > 0).Sum(x => x.Credits);

                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Advisor) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Faculty) ||
                    userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ExecutiveG2) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ITAdmin)
                    || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Admin))
                {
                    collection = collection.Where(x => x.OriginalCalID == AcademicCalenderID).ToList();
                    //collection = collection.Where(x => x.IsAutoAssign == true && x.OriginalCalID == AcademicCalenderID).ToList();
                }
                else { collection = collection.Where(x => x.IsAutoAssign == true && x.OriginalCalID == AcademicCalenderID).ToList(); }
                
                gvCourseRegistration.DataSource = collection.ToList().OrderBy(c => c.FormalCode);
                gvCourseRegistration.DataBind();
                
                if (userObj.RoleID == 8)
                {
                    if (gvCourseRegistration.Columns.Count > 0)
                    {
                        gvCourseRegistration.Columns[6].Visible = false;
                        gvCourseRegistration.Columns[10].Visible = false;
                        gvCourseRegistration.Columns[11].Visible = false;
                        gvCourseRegistration.Columns[17].Visible = false;
                        //gvCourseRegistration.Columns[8].Visible = false;
                    }
                }

                ButtonEnableDisableBasedOnRoleAndCurrentRegStatus(collection);
                GridReBind();
                //ButtonEnableDisableBasedOnRoleAndCurrentRegStatus();
            }

            lblSectionCount.Text = "Course Registered: " + sectionCount;
            lblCreditCount.Text = "Total Credit: " + creditCount;
            lblAssignCredit.Text = "Section Assigned Credit: " + AssignCredit;
        }

        #endregion Load Student

        #region Inside Grid Event
        //need to modify
        private void GridReBind()
        {
            for (int i = 0; i < gvCourseRegistration.Rows.Count; i++)
            {
                GridViewRow row = gvCourseRegistration.Rows[i];
                Button btnAddSection = (Button)row.FindControl("btnSectionAdd");
                CheckBox chkActive= (CheckBox)row.FindControl("ChkActive");
                Button btnRemoveCourse = (Button)row.FindControl("btnRemoveCourse");
                DropDownList ddlCourseStatus = (DropDownList)row.FindControl("ddlCourseStatus");
                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
                {
                    btnAddSection.Visible = false;
                    btnAddSection.Enabled = false;
                    chkActive.Visible = false;
                    chkActive.Enabled = false;
                    btnRemoveCourse.Visible = false;
                    btnRemoveCourse.Enabled = false;
                    ddlCourseStatus.Visible = false;
                    ddlCourseStatus.Enabled = false;
                }
                else
                {
                    if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Admin) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ITAdmin))
                    {
                        btnAddSection.Visible = true;
                        btnAddSection.Enabled = true;
                        btnRemoveCourse.Visible = true;
                        btnRemoveCourse.Enabled = true;
                        chkActive.Visible = true;
                        chkActive.Enabled = true;
                        ddlCourseStatus.Visible = true;
                        ddlCourseStatus.Enabled = true;
                    }
                    else 
                    {
                        btnAddSection.Visible = true;
                        btnAddSection.Enabled = true;
                        btnRemoveCourse.Visible = false;
                        btnRemoveCourse.Enabled = false;
                        chkActive.Visible = true;
                        chkActive.Enabled = true;
                        ddlCourseStatus.Visible = false;
                        ddlCourseStatus.Enabled = false;
                    }
                }
            }
        }

        protected void btnSectionAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (userObj.RoleID != Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
                {
                    ShowLabelMessage("", false);
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                    Button btn = (Button)sender;
                    int id = int.Parse(btn.CommandArgument.ToString());
                    Session[AddRegistrationWorksheetId] = id;

                    ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);

                    if (userObj.RoleID == 9)
                    {
                        ShowAlertMessage("Department Head can not add section.");
                        return;
                    }

                    RegistrationWorksheet registrationWorksheet = RegistrationWorksheetManager.GetById(id);
                    if (registrationWorksheet.IsRegistered == true)
                    {
                        ShowAlertMessage("Course already registered.");


                        return;
                    }
                    else if (!string.IsNullOrEmpty(registrationWorksheet.SectionName))
                    {

                        ShowAlertMessage("First remove the section then add again.");
                        ShowAlertMessage(" .");
                        return;
                    }
                    else if (IsAlreadyTakeThisCourse(registrationWorksheet.StudentID, registrationWorksheet.CourseID, registrationWorksheet.VersionID, acaCalId))
                    {
                        ShowAlertMessage("Course has been taken already. Please select others.");
                        ShowAlertMessage(" .");
                        return;
                    }

                    else
                    {
                        // string acacal = AcademicCalenderManager.GetActiveRegistrationCalender().AcademicCalenderID.ToString();// registrationWorksheet.AcademicCalenderID.ToString();
                        //AcademicCalender session = AcademicCalenderManager.GetActiveRegistrationCalender();
                        AcademicCalender session = AcademicCalenderManager.GetById(Convert.ToInt32(ddlSession.SelectedValue));
                        string prog = registrationWorksheet.ProgramID.ToString();
                        string dept = registrationWorksheet.DeptID.ToString();
                        string crs = registrationWorksheet.CourseID.ToString();
                        string vrs = registrationWorksheet.VersionID.ToString();

                        List<SectionDTO> entitiesDTO = SectionDTOManager.GetAllBy(session.AcademicCalenderID,
                                                                                   registrationWorksheet.ProgramID,
                                                                                   registrationWorksheet.CourseID,
                                                                                   registrationWorksheet.VersionID);

                        Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                        Session[SessionStudentId] = student.StudentID;
                        if (student != null && entitiesDTO != null && entitiesDTO.Count > 0)
                        {
                            if (student.BasicInfo.Gender == CommonEnum.SectionGender.Male.ToString())
                                entitiesDTO = entitiesDTO.Where(s => s.SectionGender != (int)CommonEnum.SectionGender.Female).ToList();
                            else if (student.BasicInfo.Gender == CommonEnum.SectionGender.Female.ToString())
                                entitiesDTO = entitiesDTO.Where(s => s.SectionGender != (int)CommonEnum.SectionGender.Male).ToList();

                            if (entitiesDTO.Count == 0)
                            {

                                ShowAlertMessage("Section not found.");

                                return;
                            }
                            else
                            {
                                GridViewSection.DataSource = null;
                                GridViewSection.DataSource = entitiesDTO;
                                GridViewSection.DataBind();

                                ModalPopupExtender1.Show();
                            }
                        }
                        else
                        {

                            ShowAlertMessage("Section not found.");
                            return;
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        protected void btnRemoveSection_Click(object sender, EventArgs e)
        {

        }

        protected void btnAssignCourse_Click(object sender, EventArgs e)
        {
            Student student = new Student();

            try
            {
                CheckBox chk = sender as CheckBox;
                GridViewRow row1 = chk.NamingContainer as GridViewRow;
                HiddenField hdnID = row1.FindControl("hdnId") as HiddenField;
                int id = Convert.ToInt32(hdnID.Value);

                RegistrationWorksheet registrationWorksheet = RegistrationWorksheetManager.GetById(id);

                student = StudentManager.GetById(registrationWorksheet.StudentID);

                List<RegistrationWorksheet> rw = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                //get no of course submitted to advisor
                int courseSubmitedToAdvisorCounter = rw.Count(x => x.OriginalCalID == Convert.ToInt32(ddlSession.SelectedValue) && x.RetakeNo == 1);
                //check user role is student or not
                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
                {

                    if (registrationWorksheet.IsRegistered == true)
                    {
                        ShowAlertMessage("Course already registered.");
                        return;
                    }
                    
                    if (registrationWorksheet.RetakeNo == 0)
                    {
                        if (!chk.Checked)
                        {
                            registrationWorksheet.AcaCal_SectionID = 0;
                            registrationWorksheet.SectionName = null;
                            registrationWorksheet.IsAutoAssign = false;
                            registrationWorksheet.RetakeNo = 0;
                        }
                        else 
                        {
                            registrationWorksheet.IsAutoAssign = true;
                            registrationWorksheet.RetakeNo = 0;
                        }
                    }
                    else
                    {
                        ShowAlertMessage("This Course already Forwarded.");
                        return;
                    }

                    RegistrationWorksheetManager.Update(registrationWorksheet);
                    btnLoad_Click(null, null);

                    //}
                    //else
                    //{
                    //    ShowAlertMessage("Can not change after submitting to advisor.");
                    //    return;
                    //}
                }
                else
                {
                    if (registrationWorksheet.IsRegistered == true)
                    {
                        ShowAlertMessage("Course already registered.");
                        return;
                    }
                    else
                    {
                        if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Advisor) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Faculty) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ExecutiveG2))
                        {
                            if (registrationWorksheet.RetakeNo < 2)
                            {
                                if (!chk.Checked)
                                {
                                    registrationWorksheet.AcaCal_SectionID = 0;
                                    registrationWorksheet.SectionName = null;
                                    registrationWorksheet.IsAutoAssign = false;
                                    registrationWorksheet.RetakeNo = 0;
                                }
                                else
                                {
                                    registrationWorksheet.IsAutoAssign = true;
                                    registrationWorksheet.RetakeNo = 0;
                                }
                            }
                            else 
                            {
                                ShowAlertMessage("This course already forwarded to HOD.");
                                return;
                            }
                        }
                        else if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Head) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Admin) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ITAdmin))
                        {
                            if (registrationWorksheet.RetakeNo < 3)
                            {
                                if (!chk.Checked)
                                {
                                    registrationWorksheet.AcaCal_SectionID = 0;
                                    registrationWorksheet.SectionName = null;
                                    registrationWorksheet.IsAutoAssign = false;
                                    registrationWorksheet.RetakeNo = 0;
                                }
                                else
                                {
                                    registrationWorksheet.IsAutoAssign = true;
                                    registrationWorksheet.RetakeNo = 0;
                                }
                            }
                            else
                            {
                                ShowAlertMessage("This course already forwarded to Admission Office. You cannot remove section of this course.");
                                return;
                            }
                        }
                        else 
                        {
                            return;
                        }

                        //if (registrationWorksheet.IsAutoAssign == true)
                        //{
                        //    if (string.IsNullOrEmpty(registrationWorksheet.SectionName) || registrationWorksheet.AcaCal_SectionID == 0)
                        //    {
                        //        registrationWorksheet.IsAutoAssign = false;
                        //        registrationWorksheet.RetakeNo = 0;
                        //    }
                        //    else
                        //    {
                        //        ShowAlertMessage("This Course has Section.");
                        //        return;
                        //    }
                        //}
                        //else
                        //{
                        //    registrationWorksheet.IsAutoOpen = true;
                        //    registrationWorksheet.IsAutoAssign = true;
                        //}

                    }

                    RegistrationWorksheetManager.Update(registrationWorksheet);
                    btnLoad_Click(null, null);
                }

            }
            catch (Exception)
            {
            }
            finally
            {
                btnLoad_Click(null, null);
            }
        }

        protected void btnRequisition_Click(object sender, EventArgs e)
        {

        }

        private bool IsAlreadyTakeThisCourse(int studentId, int courseId, int versionId, int acaCalId)
        {
            bool result = false;

            List<RegistrationWorksheet> collection = new List<RegistrationWorksheet>();
            collection = RegistrationWorksheetManager.GetAllOpenCourseByStudentID(studentId);

            int count = collection.Where(c => c.CourseID == courseId && c.VersionID == versionId && !string.IsNullOrEmpty(c.SectionName) && c.OriginalCalID == acaCalId).ToList().Count();
            if (count > 0)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        //method for section grid
        //need to modify
        protected void GridViewSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                bool isConflict = false;

                LogicLayer.BusinessObjects.AcademicCalenderSection academicCalenderSection = null;

                GridViewRow row = GridViewSection.SelectedRow;
                DataKey dtkey = GridViewSection.SelectedDataKey;
                string sectionId = dtkey.Value.ToString();
                academicCalenderSection = AcademicCalenderSectionManager.GetById(Convert.ToInt32(sectionId));
                if (academicCalenderSection.Capacity > academicCalenderSection.Occupied)
                {
                    Label lblSection = (Label)row.FindControl("lblSectionName");
                    string sectionName = lblSection.Text.Trim();

                    Label lblTimeSlot1 = (Label)row.FindControl("lblTimeSlot1");
                    string timeSlot1 = lblTimeSlot1.Text.Trim();

                    Label lblTimeSlot2 = (Label)row.FindControl("lblTimeSlot2");
                    string timeSlot2 = lblTimeSlot2.Text.Trim();

                    Label lblDayOne = (Label)row.FindControl("lblDayOne");
                    string dayOne = lblDayOne.Text.Trim();

                    Label lblDayTwo = (Label)row.FindControl("lblDayTwo");
                    string dayTwo = lblDayTwo.Text.Trim();

                    RegistrationWorksheet registrationWorksheet = RegistrationWorksheetManager.GetById(Convert.ToInt32(Session[AddRegistrationWorksheetId]));

                    #region Section Batch Filter;
                    if (academicCalenderSection.ShareBatch.Count > 0)
                    {
                        Batch batch = BatchManager.GetById(registrationWorksheet.BatchID);
                        if (batch != null)
                        {
                            if (!academicCalenderSection.ShareBatch.Exists(b => b.BatchId == batch.BatchId))
                            {
                                ShowAlertMessage(" Your Batch is not permited in this section. Please try another section.");
                                return;
                            }
                        }
                    }
                    #endregion


                    #region Check conflict

                    Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());

                    //AcademicCalender acaCal = AcademicCalenderManager.GetActiveRegistrationCalender();
                    AcademicCalender acaCal = AcademicCalenderManager.GetById(Convert.ToInt32(ddlSession.SelectedValue));

                    List<RegistrationWorksheet> courseList = RegistrationWorksheetManager.GetAllOpenCourseByStudentID(registrationWorksheet.StudentID)
                                                                                         .Where(r => !string.IsNullOrEmpty(r.SectionName) && r.OriginalCalID == acaCal.AcademicCalenderID).ToList();
                    if (courseList.Count > 0)
                    {

                        foreach (RegistrationWorksheet item in courseList)
                        {
                            LogicLayer.BusinessObjects.AcademicCalenderSection sectionTemp = AcademicCalenderSectionManager.GetById(item.AcaCal_SectionID);

                            bool conflict = false;// AcademicCalenderSectionManager.IsSectionConflict(academicCalenderSection.DayOne,                                                                                             academicCalenderSection.TimeSlotPlanOneID,                                                                                             academicCalenderSection.DayTwo,                                                                                             academicCalenderSection.TimeSlotPlanTwoID,                                                                                             sectionTemp.DayOne,                                                                                             sectionTemp.TimeSlotPlanOneID,                                                                                             sectionTemp.DayTwo,                                                                                             sectionTemp.TimeSlotPlanTwoID);

                            if (conflict)
                            {
                                ShowAlertMessage("Section Conflict. " + item.FormalCode + " with the new section of " + registrationWorksheet.FormalCode + ", Please choose other section.");
                                //isConflict = true;
                                return;
                            }
                        }
                    }
                    if (!isConflict)
                    {
                        string changeSectionName = "";
                        changeSectionName = registrationWorksheet.SectionName;

                        registrationWorksheet.SectionName = sectionName + " ( " + dayOne + " " + timeSlot1 + "; " + dayTwo + " " + timeSlot2 + " )";
                        registrationWorksheet.AcaCal_SectionID = Convert.ToInt32(sectionId);
                        registrationWorksheet.ModifiedDate = DateTime.Now;
                        bool result = RegistrationWorksheetManager.UpdateForSectionTake(registrationWorksheet);
                        string Roll = registrationWorksheet.Roll;

                        if (result)
                        {
                            //#region Log Insert

                            //LogGeneralManager.Insert(
                            //            DateTime.Now,
                            //            "",
                            //            "",
                            //            userObj.LogInID,
                            //            registrationWorksheet.FormalCode,
                            //            "",
                            //            " Change Section",
                            //            userObj.LogInID + " Change Section for -- " + registrationWorksheet.FormalCode + " : " + registrationWorksheet.CourseTitle + " -- course from section " + changeSectionName + " to " + registrationWorksheet.SectionName,
                            //            userObj.LogInID + " Change Section",
                            //            ((int)CommonEnum.PageName.Registration).ToString(),
                            //            CommonEnum.PageName.Registration.ToString(),
                            //            _pageUrl,
                            //            Roll);
                            //#endregion

                            academicCalenderSection.Occupied = (academicCalenderSection.Occupied + 1);
                            academicCalenderSection.ModifiedDate = DateTime.Now;
                            academicCalenderSection.ModifiedBy = userObj.Id;
                            AcademicCalenderSectionManager.Update(academicCalenderSection);
                        }
                    }
                }
                else
                {

                    ShowAlertMessage(" Section overflow. Please try another section.");
                }
                    #endregion

                ModalPopupExtender1.Hide();

                CleareGrid();
                LoadStudentCourse(SessionManager.GetObjFromSession<int>(SessionStudentId));
            }
            catch (Exception ex)
            {
                ModalPopupExtender1.Hide();
                ShowLabelMessage(ex.Message, true);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

        protected void btnRemoveCourse_Click(object sender, EventArgs e)
        {
            try
            {
                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Admin) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ITAdmin))
                {
                    ShowLabelMessage("", false);
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                    Button btn = (Button)sender;
                    int id = int.Parse(btn.CommandArgument.ToString());
                    Session[AddRegistrationWorksheetId] = id;

                    RegistrationWorksheet registrationWorksheet = RegistrationWorksheetManager.GetById(id);
                    if (registrationWorksheet.IsRegistered == true)
                    {
                        ShowAlertMessage("Course already registered.");
                        return;
                    } 

                    if (registrationWorksheet != null)
                    {
                        List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(registrationWorksheet.StudentID, acaCalId);
                        StudentCourseHistory studentCourseHistoryObj = studentCourseHistoryList.Where(d => d.CourseID == registrationWorksheet.CourseID && d.VersionID == registrationWorksheet.VersionID).FirstOrDefault();
                        bool isWorkSheetDeleted = RegistrationWorksheetManager.Delete(registrationWorksheet.ID);
                        if(studentCourseHistoryObj!= null)
                        {
                            BillHistory billHistoryObj = BillHistoryManager.GetByStudentCourseHistoryId(studentCourseHistoryObj.ID);
                            bool isCourseDeleted = StudentCourseHistoryManager.Delete(studentCourseHistoryObj.ID);
                            
                            if (billHistoryObj != null)
                            {
                                CollectionHistory collectionHistoryObj = CollectionHistoryManager.GetByBillHistoryMasterId(billHistoryObj.BillHistoryMasterId).Where(d => d.BillHistoryId == billHistoryObj.BillHistoryId).FirstOrDefault();
                                bool isBillDeleted = BillHistoryManager.Delete(billHistoryObj.BillHistoryId);
                                
                                if (collectionHistoryObj != null)
                                {
                                    bool isCollectionDeleted = CollectionHistoryManager.Delete(collectionHistoryObj.CollectionHistoryId);
                                }
                            }
                        }
                    }
                    btnLoad_Click(null, null);
                }
            }
            catch(Exception ex){}
        }

        protected void ddlCourseStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList ddlCourseStatus = (DropDownList)gvr.FindControl("ddlCourseStatus");
            Label lblWorkSheetId = (Label)gvr.FindControl("lblWorkSheetId");

            int courseStatusId = Convert.ToInt32(ddlCourseStatus.SelectedItem.Value);
            string courseStatus = Convert.ToString(ddlCourseStatus.SelectedItem.Text);
            int workSheetId = Convert.ToInt32(lblWorkSheetId.Text);
            if (courseStatusId > 0 && workSheetId > 0)
            {
                RegistrationWorksheet registrationWorkSheetObj = RegistrationWorksheetManager.GetById(workSheetId);
                if (registrationWorkSheetObj != null)
                {
                    registrationWorkSheetObj.AcaCalTypeName = courseStatus;
                    bool result = RegistrationWorksheetManager.Update(registrationWorkSheetObj);
                    if (result)
                    {
                        btnLoad_Click(null, null);
                        ShowAlertMessage(" Course Reg Type Changed Successfully. ");
                        return;
                    }
                }
            }
        }
        #endregion Inside Grid Event

        #region Common Method
        private void LoadCurrentRegSessions()
        {
            ddlSession.Items.Clear();
            List<AcademicCalender> activeRegistrationSession = AcademicCalenderManager.GetActiveRegistrationCalenders().OrderByDescending(d => d.AcademicCalenderID).ToList();
            if (activeRegistrationSession.Count > 0)
            {
                ddlSession.DataTextField = "FullCode";
                ddlSession.DataValueField = "AcademicCalenderID";
                ddlSession.DataSource = activeRegistrationSession;
                ddlSession.DataBind();
            }
        }

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad_Click(null, null);
        }

        private void ButtonEnableDisableBasedOnRoleAndCurrentRegStatus(List<RegistrationWorksheet> registartionList)
        {
            if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
            {
                txtStudent.Text = userObj.LogInID.ToString();
                //registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = true;
                    btnDownload.Visible = true;
                    lBtnRefresh.Visible = true;

                    pnlStudentReg.Visible = true;
                    btnSubmitToAdvisor.Visible = true;

                    pnlAdvisor.Visible = false;
                    btnSubmitToHOD.Visible = false;
                    btnAddCourse.Visible = false;
                    btnAddAnyCourse.Visible = false;

                    pnlHOD.Visible = false;
                    btnRejectHOD.Visible = false;
                    btnApproveHOD.Visible = false;

                    pnlAdmissionOffice.Visible = false;
                    btnAproveAdmissionOffice.Visible = false;

                    pnlRegisterOffice.Visible = false;
                    btnApproveRegisterOffice.Visible = false;
                }
            }
            else if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Advisor) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Faculty) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ExecutiveG2))
            {
                registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = true;
                    btnDownload.Visible = true;
                    lBtnRefresh.Visible = true;

                    pnlStudentReg.Visible = false;
                    btnSubmitToAdvisor.Visible = false;

                    pnlAdvisor.Visible = true;
                    btnSubmitToHOD.Visible = true;
                    btnAddCourse.Visible = true;
                    btnAddAnyCourse.Visible = true;

                    pnlHOD.Visible = false;
                    btnRejectHOD.Visible = false;
                    btnApproveHOD.Visible = false;

                    pnlAdmissionOffice.Visible = false;
                    btnAproveAdmissionOffice.Visible = false;

                    pnlRegisterOffice.Visible = false;
                    btnApproveRegisterOffice.Visible = false;

                    btnAddAnyCourse.Visible = true;
                }
            }
            else if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Head))
            {
                registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = true;
                    btnDownload.Visible = true;
                    lBtnRefresh.Visible = true;

                    pnlStudentReg.Visible = false;
                    btnSubmitToAdvisor.Visible = false;

                    pnlAdvisor.Visible = false;
                    btnSubmitToHOD.Visible = false;
                    btnAddCourse.Visible = false;
                    btnAddAnyCourse.Visible = false;

                    pnlHOD.Visible = true;
                    btnRejectHOD.Visible = true;
                    btnApproveHOD.Visible = true;

                    pnlAdmissionOffice.Visible = false;
                    btnAproveAdmissionOffice.Visible = false;

                    pnlRegisterOffice.Visible = false;
                    btnApproveRegisterOffice.Visible = false;
                }

            }
            else if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Admission))
            {
                registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = true;
                    btnDownload.Visible = true;
                    lBtnRefresh.Visible = true;

                    pnlStudentReg.Visible = false;
                    btnSubmitToAdvisor.Visible = false;

                    pnlAdvisor.Visible = false;
                    btnSubmitToHOD.Visible = false;
                    btnAddCourse.Visible = false;
                    btnAddAnyCourse.Visible = false;

                    pnlHOD.Visible = false;
                    btnRejectHOD.Visible = false;
                    btnApproveHOD.Visible = false;

                    pnlAdmissionOffice.Visible = true;
                    btnAproveAdmissionOffice.Visible = true;

                    pnlRegisterOffice.Visible = false;
                    btnApproveRegisterOffice.Visible = false;
                }
            }
            else if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Registarar))
            {
                registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = true;
                    btnDownload.Visible = true;
                    lBtnRefresh.Visible = true;

                    pnlStudentReg.Visible = false;
                    btnSubmitToAdvisor.Visible = false;

                    pnlAdvisor.Visible = false;
                    btnSubmitToHOD.Visible = false;
                    btnAddCourse.Visible = false;
                    btnAddAnyCourse.Visible = false;

                    pnlHOD.Visible = false;
                    btnRejectHOD.Visible = false;
                    btnApproveHOD.Visible = false;

                    pnlAdmissionOffice.Visible = false;
                    btnAproveAdmissionOffice.Visible = false;

                    pnlRegisterOffice.Visible = true;
                    btnApproveRegisterOffice.Visible = true;
                }
            }
            else if (userObj.RoleID == 1 || userObj.RoleID == 3)
            {
                btnAddAnyCourse.Visible = true;
                //registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = true;
                    btnDownload.Visible = true;
                    lBtnRefresh.Visible = true;

                    List<RegistrationWorksheet> registartionOpenForReg = new List<RegistrationWorksheet>();
                    registartionOpenForReg = registartionList.Where(d => d.RetakeNo == 0).ToList();
                    if (registartionOpenForReg != null && registartionOpenForReg.Count > 0)
                    {
                        pnlStudentReg.Visible = true;
                        btnSubmitToAdvisor.Visible = true;
                    }

                    List<RegistrationWorksheet> registartionSubmittedToAdvisor = new List<RegistrationWorksheet>();
                    registartionSubmittedToAdvisor = registartionList.Where(d => d.RetakeNo == 1).ToList();
                    if (registartionSubmittedToAdvisor != null && registartionSubmittedToAdvisor.Count > 0)
                    {
                        pnlAdvisor.Visible = true;
                        btnSubmitToHOD.Visible = true;
                        btnAddCourse.Visible = true;
                        btnAddAnyCourse.Visible = true;
                    }

                    List<RegistrationWorksheet> registartionForwarToHOD = new List<RegistrationWorksheet>();
                    registartionForwarToHOD = registartionList.Where(d => d.RetakeNo == 2).ToList();
                    if (registartionForwarToHOD != null && registartionForwarToHOD.Count > 0)
                    {
                        pnlHOD.Visible = true;
                        btnRejectHOD.Visible = true;
                        btnApproveHOD.Visible = true;
                    }

                    List<RegistrationWorksheet> registartionHODApproved = new List<RegistrationWorksheet>();
                    registartionHODApproved = registartionList.Where(d => d.RetakeNo == 3).ToList();
                    if (registartionHODApproved != null && registartionHODApproved.Count > 0)
                    {
                        pnlAdmissionOffice.Visible = true;
                        btnAproveAdmissionOffice.Visible = true;
                    }

                    List<RegistrationWorksheet> registartionAdmissionOfcForward = new List<RegistrationWorksheet>();
                    registartionAdmissionOfcForward = registartionList.Where(d => d.RetakeNo == 4).ToList();
                    if (registartionAdmissionOfcForward != null && registartionAdmissionOfcForward.Count > 0)
                    {
                        pnlRegisterOffice.Visible = true;
                        btnApproveRegisterOffice.Visible = true;
                    }
                }
            }
            else
            {
                btnAddAnyCourse.Visible = false;
                registartionList = registartionList.Where(d=> d.IsAutoAssign).ToList();
                if (registartionList != null && registartionList.Count > 0)
                {
                    pnlRefreshDownload.Visible = false;
                    btnDownload.Visible = false;
                    lBtnRefresh.Visible = false;

                    pnlStudentReg.Visible = false;
                    btnSubmitToAdvisor.Visible = false;

                    pnlAdvisor.Visible = true;
                    btnSubmitToHOD.Visible = true;
                    btnAddCourse.Visible = false;
                    btnAddAnyCourse.Visible = false;

                    pnlHOD.Visible = false;
                    btnRejectHOD.Visible = false;
                    btnApproveHOD.Visible = false;

                    pnlAdmissionOffice.Visible = false;
                    btnAproveAdmissionOffice.Visible = false;

                    pnlRegisterOffice.Visible = false;
                    btnApproveRegisterOffice.Visible = false;
                }
            }
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        private void ShowLabelMessage(string msg, bool isVisible)
        {
            pnlMessage.Visible = isVisible;
            lblMessage.Text = msg;
        }

        private bool UpdateRegistrationWorkSheet(RegistrationWorksheet registrationWorksheetObj, int regStage)
        {
            registrationWorksheetObj.RetakeNo = regStage;
            registrationWorksheetObj.ModifiedBy = Convert.ToInt32(userObj.Id);
            registrationWorksheetObj.ModifiedDate = DateTime.Now;
            bool result = RegistrationWorksheetManager.Update(registrationWorksheetObj);
            return result;
        }

        private void InsertCourseHistory(RegistrationWorksheet registrationWorksheetObj)
        {
            StudentCourseHistory studentCourseHistory = new StudentCourseHistory();
            List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(registrationWorksheetObj.StudentID, registrationWorksheetObj.OriginalCalID);
            studentCourseHistory = studentCourseHistoryList.Find(o => o.CourseStatusID == (int)CommonEnum.CourseStatus.Rn &&
                                                                        o.CourseID == registrationWorksheetObj.CourseID &&
                                                                        o.VersionID == registrationWorksheetObj.VersionID);
            if (studentCourseHistory == null)
            {
                StudentCourseHistory studentCourseHistoryInserObj = new StudentCourseHistory();
                studentCourseHistoryInserObj.StudentID = Convert.ToInt32(registrationWorksheetObj.StudentID);
                studentCourseHistoryInserObj.CourseStatusID = (int)CommonEnum.CourseStatus.Rn;
                
                if (registrationWorksheetObj.AcaCalTypeName == "RT") 
                {
                    studentCourseHistoryInserObj.RetakeNo = 10;
                }
                else if (registrationWorksheetObj.AcaCalTypeName == "IM") 
                {
                    studentCourseHistoryInserObj.RetakeNo = 12;
                }
                else if (registrationWorksheetObj.AcaCalTypeName == "SS") 
                {
                    studentCourseHistoryInserObj.RetakeNo = 11;
                }
                else if (registrationWorksheetObj.AcaCalTypeName == "R")
                {
                    studentCourseHistoryInserObj.RetakeNo = 9;
                }
                studentCourseHistoryInserObj.AcaCalID = registrationWorksheetObj.OriginalCalID;
                studentCourseHistoryInserObj.CourseID = registrationWorksheetObj.CourseID;
                studentCourseHistoryInserObj.VersionID = registrationWorksheetObj.VersionID;
                studentCourseHistoryInserObj.CourseCredit = registrationWorksheetObj.Credits;
                studentCourseHistoryInserObj.AcaCalSectionID = registrationWorksheetObj.AcaCal_SectionID;
                studentCourseHistoryInserObj.CourseStatusDate = DateTime.Now;
                studentCourseHistoryInserObj.CreatedBy = userObj.Id;
                studentCourseHistoryInserObj.CreatedDate = DateTime.Now;
                studentCourseHistoryInserObj.ModifiedBy = userObj.Id;
                studentCourseHistoryInserObj.ModifiedDate = DateTime.Now;
                int i = StudentCourseHistoryManager.Insert(studentCourseHistoryInserObj);
            }
        }

        private void CleareGrid()
        {
            lblForwardedCourse.Text = "";
            lblGroupName.Text = "";
            lblName.Text = "";
            lblProgram.Text = "";
            lblRegistrationSession.Text = "";
            lblBatch.Text = "";
            gvCourseRegistration.DataSource = null;
            gvCourseRegistration.DataBind();
        }

        private void InsertLog(string messageString, string courseName, string courseCode, string roll)
        {
            #region Log Insert
            LogGeneralManager.Insert(
                        DateTime.Now,
                        "",
                        "",
                        userObj.LogInID,
                        "",
                        "",
                        messageString,
                        userObj.LogInID + messageString + courseName + ", " + courseCode + ", " + roll,
                        userObj.LogInID + messageString,
                        ((int)CommonEnum.PageName.ForceRegistration).ToString(),
                        CommonEnum.PageName.ForceRegistration.ToString(),
                        _pageUrl,
                        roll);
            #endregion
        }
        #endregion Common Method

        #region Refresh and Download PDF
        protected void lBtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                ShowLabelMessage("", false);
                btnLoad_Click(null, null);
                //LoadStudentCourse(SessionManager.GetObjFromSession<Student>(ConstantValue.Session_ForRegistrationPage_Student).StudentID);
            }
            catch (Exception)
            {
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                if (student == null)
                {
                    ShowAlertMessage(" Student not found.");
                    return;
                }
                AcademicCalender acaCal = AcademicCalenderManager.GetById(Convert.ToInt32(ddlSession.SelectedValue));
                //AcademicCalender acaCal = AcademicCalenderManager.GetActiveRegistrationCalender();
                ReportViewer reportViewer = new ReportViewer();


                if (student.Roll != "" && acaCal.AcademicCalenderID != 0)
                {

                    List<rStudentClassExamSumNew> list = ReportManager.GetStudentRegSummaryNew(student.Roll, acaCal.AcademicCalenderID);

                    List<rStudentResultSum> resultlist = ReportManager.GetStudentResultSummary(student.Roll, acaCal.AcademicCalenderID);

                    //List<RegistrationBill> billHistory = BillHistoryManager.GetRegistrationBillHistoryByStudentIdAcaCalId(student.StudentID, acaCal.AcademicCalenderID);

                    rStudentGeneralInfo regInfo = StudentManager.GetStudentGeneralInfoById(student.StudentID);


                    if (list != null && list.Count != 0)
                    {

                        this.ReportViewer.LocalReport.ReportPath = Server.MapPath("~/Module/registration/report/RptStudentRegSummaryNew.rdlc");

                        List<ReportParameter> param1 = new List<ReportParameter>();

                        param1.Add(new ReportParameter("Roll", regInfo.Roll));
                        param1.Add(new ReportParameter("FullName", regInfo.FullName));
                        param1.Add(new ReportParameter("Phone", regInfo.Phone));
                        param1.Add(new ReportParameter("Degree", regInfo.DegreeName));
                        param1.Add(new ReportParameter("Program", regInfo.ShortName));
                        param1.Add(new ReportParameter("Shift", regInfo.Shift));
                        param1.Add(new ReportParameter("Semester", acaCal.FullCode));
                        param1.Add(new ReportParameter("UserName", userObj.LogInID));



                        ReportDataSource rds = new ReportDataSource("RegistrationCourseHistory", list);
                        ReportDataSource rds3 = new ReportDataSource("ResultSummary", resultlist);
                        //ReportDataSource rds2 = new ReportDataSource("RegistrationBill", billHistory);

                        this.ReportViewer.LocalReport.SetParameters(param1);


                        ReportViewer.LocalReport.DataSources.Clear();
                        ReportViewer.LocalReport.DataSources.Add(rds);
                        ReportViewer.LocalReport.DataSources.Add(rds3);
                        //ReportViewer.LocalReport.DataSources.Add(rds2);

                        Warning[] warnings;
                        string[] streamids;
                        string mimeType;
                        string encoding;
                        string filenameExtension;

                        byte[] bytes = this.ReportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                        using (FileStream fs = new FileStream(Server.MapPath("~/Upload/ReportPDF/" + "ReportRegistration" + student.Roll + ".pdf"), FileMode.Create))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }

                        //string url = string.Format("RegistrationPDF.aspx?FN={0}", "ReportRegistration" + student.Roll + ".pdf");
                        //string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
                        //this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);

                        string fileName = "ReportRegistration" + student.Roll + ".pdf";

                        string FilePath = Server.MapPath("~/Upload/ReportPDF/");


                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                        Response.TransmitFile(Server.MapPath("~/Upload/ReportPDF/" + fileName));
                        //Response.Redirect("~/Module/registration/SingleStudentRegistrationV2.aspx?mmi=40545d1c42555c5b4e63", false);
                        Response.End();

                        //string filename = "filename from Database";
                        //Response.ContentType = "application/octet-stream";
                        //Response.AppendHeader("Content-Disposition", "attachment;filename=" + filename);
                        //string aaa = Server.MapPath("~/SavedFolder/" + filename);
                        //Response.TransmitFile(Server.MapPath("~/SavedFolder/" + filename));
                        //Response.End();
                    }

                    else
                    {
                        reportViewer.LocalReport.DataSources.Clear();
                        ShowAlertMessage(" Registered Data not Found.");
                        return;
                    }
                }
                else
                {
                    reportViewer.LocalReport.DataSources.Clear();

                    ShowAlertMessage(" Please Enter Student ID.");
                    return;
                }
            }
            catch (Exception ex)
            {
                PrintUtility pu = new PrintUtility();

                string txtMsg = ex.StackTrace + Environment.NewLine + Environment.NewLine
                    + " ex.Message//" + ex.Message + Environment.NewLine + Environment.NewLine
                    + " ex.Source//" + ex.Source + Environment.NewLine + Environment.NewLine
                    + " ex.TargetSite//" + ex.TargetSite;

                ShowLabelMessage(txtMsg, true);
            }
        }
        #endregion Refresh and Download PDF

        #region Student Submit To advisor
        protected void btnSubmitToAdvisor_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (student != null)
                {
                    StudentEarnedCredit studentEarnedCreditObj = StudentManager.GetByStudentIdAcaCalId(student.StudentID, acaCalId);
                   
                    if(studentEarnedCreditObj != null)
                    {
                        bool result = RegistrationWorksheetManager.checkStudentRegistrationCredit(student.StudentID, student.CampusID, student.ProgramID, studentEarnedCreditObj.LevelNo, acaCalId);
                        decimal courserCreditSubmittedToAdvisor = RegistrationWorksheetManager.calculateCreditSubmitToAdvisor(student.StudentID, acaCalId);
                        decimal courserCreditForwardToHOD = RegistrationWorksheetManager.calculateCreditForwardToHead(student.StudentID, acaCalId);
                        decimal courserCreditApprovedByHOD = RegistrationWorksheetManager.calculateCreditFrowardToAdmission(student.StudentID, acaCalId);
                        decimal courserCreditForwardToRegOffice = RegistrationWorksheetManager.calculateCreditFrowardToRegOffice(student.StudentID, acaCalId);
                        //admin can do registration
                        if (userObj.RoleID == 1 || userObj.RoleID == 3)
                        {
                            result = true;
                            //courserCreditSubmittedToAdvisor = 0;
                        }

                        //if ( courserCreditForwardToHOD == 0 && courserCreditApprovedByHOD == 0 && courserCreditForwardToRegOffice == 0)
                        //{
                            if (result)
                            {
                                List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                                //need to add this logic && x.AcaCal_SectionID > 0
                                rwList = RegistrationWorksheetManager.getEligibleCourseSubmitToAdvisor(rwList, acaCalId);
                                if (rwList.Count > 0)
                                {
                                    if ((userObj.RoleID == 1 || userObj.RoleID == 3) || RegistrationWorksheetManager.checkNoOfAllowedRTorIMCourse(rwList, acaCalId, studentEarnedCreditObj.LevelNo, student.CampusID))
                                    {
                                        foreach (var registrationWorksheetObj in rwList)
                                        {
                                            int regStage = 1;
                                            registrationWorksheetObj.IsAdd = true;
                                            bool isUpdated = UpdateRegistrationWorkSheet(registrationWorksheetObj, regStage);
                                            if (isUpdated)
                                            {
                                                string messege = "Course submitted to advisor ";
                                                string courseName = registrationWorksheetObj.CourseTitle;
                                                string courseCode = registrationWorksheetObj.FormalCode;
                                                string roll = registrationWorksheetObj.Roll;
                                                InsertLog(messege, courseName, courseCode, roll);
                                                
                                            }
                                        }
                                    }
                                    else
                                    {
                                        ShowAlertMessage(" You are not allowed to take Retake or Improvemnt course more than department allow.");
                                        return;
                                    }
                                }
                                else
                                {
                                    ShowAlertMessage(" No course found to forward / No course without section will be forward to your Advisor.");
                                    return;
                                }
                            }
                            else
                            {
                                ShowAlertMessage(" Total credit for registration must be 15 to 24.");
                                return;
                            }
                        //}
                        //else
                        //{
                        //    ShowAlertMessage(" Already submitted for registration. Please contact with advisor.");
                        //    return;
                        //}
                    }
                }
                lBtnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
            }
        }


        #endregion Student Submit To advisor

        #region Advisor Forwarded to HOD
        protected void btnSubmitToHOD_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (student != null)
                {
                    StudentEarnedCredit studentEarnedCreditObj = StudentManager.GetByStudentIdAcaCalId(student.StudentID, acaCalId);
                    if (studentEarnedCreditObj != null)
                    {
                        bool result = RegistrationWorksheetManager.checkStudentRegistrationCredit(student.StudentID, student.CampusID, student.ProgramID, studentEarnedCreditObj.LevelNo, acaCalId);
                        if (userObj.RoleID == 1 || userObj.RoleID == 3)
                        {
                            result = true;
                        }
                            
                        if (result)
                        {
                            List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                            rwList = RegistrationWorksheetManager.getEligibleCourseSubmitAdvisorToHead(rwList, acaCalId);
                            if (rwList.Count > 0)
                            {
                                if ((userObj.RoleID == 1 || userObj.RoleID == 3) || RegistrationWorksheetManager.checkNoOfAllowedRTorIMCourse(rwList, acaCalId, studentEarnedCreditObj.LevelNo, student.CampusID))
                                {
                                    foreach (var registrationWorksheetObj in rwList)
                                    {
                                        int regStage = 2;
                                        registrationWorksheetObj.IsAdd = true;
                                        bool isUpdated = UpdateRegistrationWorkSheet(registrationWorksheetObj, regStage);
                                        if (isUpdated)
                                        {
                                            string messege = "Course forwarded to HOD ";
                                            string courseName = registrationWorksheetObj.CourseTitle;
                                            string courseCode = registrationWorksheetObj.FormalCode;
                                            string roll = registrationWorksheetObj.Roll;
                                            InsertLog(messege, courseName, courseCode, roll);

                                        }
                                    }
                                }
                                else
                                {
                                    ShowAlertMessage(" Student is not allowed to take Retake or Improvemnt course more than department allow.");
                                    return;
                                }
                            }
                            else
                            {
                                ShowAlertMessage(" No course found to forward / No course without section will be forward to HOD.");
                                return;
                            }
                        }
                        else
                        {
                            ShowAlertMessage(" Total credit for registration must be 15 to 24.");
                            return;
                        }
                    }
                }
                lBtnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddCourse_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

            if (string.IsNullOrEmpty(txtStudent.Text))
            {
                ShowAlertMessage(" Please provide a Student Id.");
                return;
            }
            Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
            List<Course> courseList = new List<Course>();
            List<Course> retakeCourseList = RegistrationWorksheetManager.getRetakeCourseForRegistration(student.StudentID, acaCalId);
            
            courseList.AddRange(retakeCourseList);
            if (courseList!= null)
            {
                gvAddCourse.DataSource = courseList;
                gvAddCourse.DataBind();
            }
            lBtnRefresh_Click(null, null);
            //RegistrationWorksheetManager.UpdateCourseRegType(studentObj.StudentID, acaCalId);
        }

        protected void btnAddAnyCourse_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtStudent.Text))
            {
                ShowAlertMessage(" Please provide a Student Id.");
                return;
            }
            else
            {
                Student studentObj = StudentManager.GetByRoll(Convert.ToString(txtStudent.Text));
                if (studentObj != null)
                {
                    string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                    var id = _pageUrl.Substring(_pageUrl.LastIndexOf('=') + 1);
                    int StudentId = Convert.ToInt32(studentObj.StudentID);

                    Session["StudentId"] = Convert.ToString(StudentId);
                    Response.Redirect("StudentBulkForceRegistration.aspx?mmi=" + id);
                }
            }
        }
        #endregion Advisor Forwarded to HOD

        #region HOD Approve (To Admission Office) or Reject
        protected void btnApproveHOD_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (student != null)
                {
                    StudentEarnedCredit studentEarnedCreditObj = StudentManager.GetByStudentIdAcaCalId(student.StudentID, acaCalId);
                    if (studentEarnedCreditObj != null)
                    {
                        bool result = RegistrationWorksheetManager.checkStudentRegistrationCredit(student.StudentID, student.CampusID, student.ProgramID, studentEarnedCreditObj.LevelNo, acaCalId);
                        if (userObj.RoleID == 1 || userObj.RoleID == 3)
                        {
                            result = true;
                        }

                        if (result)
                        {
                            List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                            rwList = RegistrationWorksheetManager.getEligibleCourseSubmitHeadToAdmissionOffice(rwList, acaCalId);
                            if (rwList.Count > 0)
                            {
                                if ((userObj.RoleID == 1 || userObj.RoleID == 3) || RegistrationWorksheetManager.checkNoOfAllowedRTorIMCourse(rwList, acaCalId, studentEarnedCreditObj.LevelNo, student.CampusID))
                                {
                                    foreach (var registrationWorksheetObj in rwList)
                                    {
                                        int regStage = 3;
                                        bool isUpdated = UpdateRegistrationWorkSheet(registrationWorksheetObj, regStage);
                                        if (isUpdated)
                                        {
                                            string messege = "Course approved by HOD ";
                                            string courseName = registrationWorksheetObj.CourseTitle;
                                            string courseCode = registrationWorksheetObj.FormalCode;
                                            string roll = registrationWorksheetObj.Roll;
                                            InsertLog(messege, courseName, courseCode, roll);

                                            InsertCourseHistory(registrationWorksheetObj);
                                            
                                        }
                                    }

                                    #region Sending SMS
                                    //if (totalfees>0)
                                    //{
                                    //    PersonBlockDTO person = PersonBlockManager.GetByRoll(student.Roll);
                                    //    string msg = "ID-" + student.Roll + ",your registration for " + AcademicCalenderManager.GetById(studentBillDeatilList[0].AcaCalId).FullCode + " is recorded,dues is TK. "
                                    //        + person.Dues.ToString() + ".Pay fees by due date to avoid registration cancellation.";
                                    //    SendSMS(student.BasicInfo.SMSContactSelf, student.Roll, msg);
                                    //}
                                    #endregion

                                    #region Bill Insert
                                    try
                                    {
                                        AcademicCalender academicCalenderObj = AcademicCalenderManager.GetById(acaCalId);
                                        User user_obj = UserManager.GetById(userObj.Id);
                                        if (academicCalenderObj != null)
                                        {
                                            //int calenderUnitTypeId = academicCalenderObj.CalenderUnitTypeID;

                                            BillHistoryMasterManager.GenerateBill(student.StudentID, acaCalId, user_obj);
                                            //if (calenderUnitTypeId == 3 || calenderUnitTypeId == 4)
                                            //{
                                            //    if (user_obj != null)
                                            //    {
                                            //        BillHistoryMasterManager.GenerateBill(student.StudentID, acaCalId, user_obj);
                                            //    }
                                            //}
                                            //else if (calenderUnitTypeId == 5 || calenderUnitTypeId == 6)
                                            //{
                                            //    RegistrationWorksheetManager.confirmRegistrationAfterPaymentPosting(student.StudentID, acaCalId, user_obj.User_ID);
                                            //}

                                        }
                                    }
                                    catch
                                    {
                                    }
                                    #endregion
                                }
                                else
                                {
                                    ShowAlertMessage(" Student is not allowed to take Retake or Improvemnt course more than department allow.");
                                    return;
                                }
                            }
                            else
                            {
                                ShowAlertMessage(" No course found to forward / No course without section will be forward to Admission Office.");
                                return;
                            }
                        }
                        else
                        {
                            ShowAlertMessage(" Total credit for registration must be 15 to 24.");
                            return;
                        }
                    }
                }
                lBtnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
            }
        }       

        protected void btnRejectHOD_Click(object sender, EventArgs e)
        {
            try
            {
                Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                if (student == null)
                {
                    ShowAlertMessage(" Student not found.");
                    return;
                }
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                rwList = rwList.Where(x => x.IsAutoAssign == true && x.OriginalCalID == acaCalId && x.RetakeNo == 2).ToList();
                //int isregistered = rwList.Count(x => x.IsRegistered == true);
                if (rwList.Count > 0)
                {
                    foreach (var registrationWorksheetObj in rwList)
                    {
                        //RegistrationWorksheet obj = item;
                        registrationWorksheetObj.RetakeNo = 1;
                        registrationWorksheetObj.ModifiedBy = Convert.ToInt32(userObj.Id);
                        registrationWorksheetObj.ModifiedDate = DateTime.Now;
                        bool isUpdated = RegistrationWorksheetManager.Update(registrationWorksheetObj);

                        if (isUpdated)
                        {
                            string messege = "Course rejected by HOD ";
                            string courseName = registrationWorksheetObj.CourseTitle;
                            string courseCode = registrationWorksheetObj.FormalCode;
                            string roll = registrationWorksheetObj.Roll;
                            InsertLog(messege, courseName, courseCode, roll);
                        }
                    }
                    LoadStudentCourse(student.StudentID);
                }
                else
                {
                    ShowAlertMessage("No course found to Reject!");
                    return;
                }
                lBtnRefresh_Click(null, null);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                btnLoad_Click(null, null);
            }
        }
        #endregion HOD Approve (To Admission Office) or Reject

        #region Admission Office To Reg Office
        protected void btnAproveAdmissionOffice_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            try
            {
                if (true)//Student Block
                {
                    ShowLabelMessage("", false);
                    student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                    if (student == null)
                    {
                        ShowAlertMessage("No Student Found");
                        return;
                    }
                    // AcademicCalender acaCal = AcademicCalenderManager.GetActiveRegistrationCalender();
                    if (Convert.ToInt32(ddlSession.SelectedValue) == 0)
                    {
                        lblMessage.Text = "Select session.";
                        return;
                    }
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                    List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                    rwList = RegistrationWorksheetManager.getEligibleCourseSubmitAdmissionOfficeToRegisterOffice(rwList, acaCalId);
                    if (rwList.Count > 0)
                    {
                        //StudentCourseHistory studentCourseHistory = new StudentCourseHistory();
                        //List<StudentCourseHistory> newStudentBillableCourseList = new List<StudentCourseHistory>();
                        foreach (RegistrationWorksheet registrationWorksheetObj in rwList)
                        {
                            int regStage = 4;
                            bool isUpdated = UpdateRegistrationWorkSheet(registrationWorksheetObj, regStage);
                            if (isUpdated)
                            {
                                string messege = "Course forwarded to Register Office ";
                                string courseName = registrationWorksheetObj.CourseTitle;
                                string courseCode = registrationWorksheetObj.FormalCode;
                                string roll = registrationWorksheetObj.Roll;
                                InsertLog(messege, courseName, courseCode, roll);
                            }
                        }
                    }
                    else
                    {
                        ShowAlertMessage("No course found to forward / No course without section will be forward to Register Office.");
                        return;
                    }
                }
                else
                {
                    //if (student.IsBlock == true)
                    //{
                    //    ShowAlertMessage("Currently you are blocked (not able to do registration), please contact with your Department or Accounts Department.");
                    //    return;
                    //}
                }
                lBtnRefresh_Click(null, null);
            }
            catch (Exception)
            {
                // lblMessage.Text = "Registration not complete.";
                ShowAlertMessage("Registration not complete.");

            }
            finally
            {
                if (student != null)
                    LoadStudentCourse(student.StudentID);
            }
        }
        #endregion Admission Office To Reg Office

        #region Register Office Final Confirmation
        protected void btnApproveRegisterOffice_Click(object sender, EventArgs e)
        {
            Student student = new Student();
            try
            {
                if (true)//Student Block
                {
                    ShowLabelMessage("", false);
                    student = StudentManager.GetByRoll(txtStudent.Text.Trim());
                    if (student == null)
                    {
                        ShowAlertMessage("No Student Found");
                        return;
                    }
                    // AcademicCalender acaCal = AcademicCalenderManager.GetActiveRegistrationCalender();
                    if (Convert.ToInt32(ddlSession.SelectedValue) == 0)
                    {
                        lblMessage.Text = "Select session.";
                        return;
                    }
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                    List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentID(student.StudentID);
                    rwList = RegistrationWorksheetManager.getEligibleCourseForFinalRegistration(rwList, acaCalId);
                    if (rwList.Count > 0)
                    {
                        StudentCourseHistory studentCourseHistory = new StudentCourseHistory();
                        List<StudentCourseHistory> newStudentBillableCourseList = new List<StudentCourseHistory>();
                        //int RCount = 0;
                        //int billCount = 0;
                        foreach (RegistrationWorksheet registrationWorksheetObj in rwList)
                        {
                            int regStage = 5;
                            registrationWorksheetObj.IsRegistered = true;
                            bool isUpdated = UpdateRegistrationWorkSheet(registrationWorksheetObj, regStage);
                            if (isUpdated)
                            {
                                string messege = "Course registration confirmed by Register Office ";
                                string courseName = registrationWorksheetObj.CourseTitle;
                                string courseCode = registrationWorksheetObj.FormalCode;
                                string roll = registrationWorksheetObj.Roll;
                                InsertLog(messege, courseName, courseCode, roll);
                            }
                        }
                        #region Sending SMS

                        List<RegistrationWorksheet> registrationCourseList = RegistrationWorksheetManager.GetByStudentID(student.StudentID).Where(d=> d.OriginalCalID == acaCalId &&  d.IsRegistered == true ).ToList();
                        if (rwList.Count == registrationCourseList.Count)
                        {
                            RegistrationWorksheetManager.SendSMS(student.StudentID, acaCalId, userObj.Id);
                        }
                        //if (totalfees > 0)
                        //{
                        //    PersonBlockDTO person = PersonBlockManager.GetByRoll(student.Roll);
                        //    string msg = "ID-" + student.Roll + ",your registration for " + AcademicCalenderManager.GetById(studentBillDeatilList[0].AcaCalId).FullCode + " is recorded,dues is TK. "
                        //        + person.Dues.ToString() + ".Pay fees by due date to avoid registration cancellation.";
                        //    SendSMS(student.BasicInfo.SMSContactSelf, student.Roll, msg);
                        //}
                        #endregion
                    }
                    else
                    {
                        ShowAlertMessage(" No course found to confirm as Registered / No course without section will confirm as Registered.");
                        return;
                    }
                }


                else
                {

                }
                lBtnRefresh_Click(null, null);
            }
            catch (Exception)
            {
                // lblMessage.Text = "Registration not complete.";
                ShowAlertMessage("Registration not complete.");

            }
            finally
            {
                if (student != null)
                    LoadStudentCourse(student.StudentID);
            }
        }
        #endregion Register Office Final Confirmation

        #region Add Course by Advisor
        protected void btnAddCourseCancel_Click(object sender, EventArgs e)
        {

        }

        protected void gvAddCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            Student studentObj = StudentManager.GetByRoll(txtStudent.Text.Trim());

            if (studentObj != null)
            {
                GridViewRow row = gvAddCourse.SelectedRow;
                Label lblCourseID = (Label)row.FindControl("lblCourseID");
                Label lblVersionID = (Label)row.FindControl("lblVersionID");
                Label lblCourseFailCounter = (Label)row.FindControl("lblCourseFailCounter");

                if (!string.IsNullOrEmpty(lblCourseID.Text) && !string.IsNullOrEmpty(lblVersionID.Text))
                {
                    int courseId = Convert.ToInt32(lblCourseID.Text);
                    int versionId = Convert.ToInt32(lblVersionID.Text);
                    int failCounter = Convert.ToInt32(lblCourseFailCounter.Text);

                    if (courseId > 0 && versionId > 0)
                    {
                        if (RegistrationWorksheetManager.checkCourse(failCounter, userObj.RoleID, acaCalId))
                        {
                            InsertWorkSheet(studentObj, courseId, versionId, acaCalId);
                        }
                        else
                        {
                            ShowAlertMessage("Student is not eligible to take this course.");
                            return;
                        }
                    }
                }
            }
            ModalPopupExtender2.Show();
        }

        private void InsertWorkSheet(Student studentObj, int courseId, int versionId, int acaCalId)
        {
            try
            {
                Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);
                RegistrationWorksheet regWorkSheet = new RegistrationWorksheet();
                regWorkSheet.CourseID = courseObj.CourseID;
                regWorkSheet.VersionID = courseObj.VersionID;
                regWorkSheet.Credits = courseObj.Credits;
                regWorkSheet.IsAutoOpen = true;
                regWorkSheet.IsAutoAssign = true;
                regWorkSheet.IsAdd = true;
                regWorkSheet.BatchID = studentObj.BatchId;
                regWorkSheet.RetakeNo = 1;
                //regWorkSheet.Node_CourseID = nodeCourseId;
                regWorkSheet.StudentID = studentObj.StudentID;
                regWorkSheet.OriginalCalID = acaCalId;
                regWorkSheet.CourseTitle = courseObj.Title;
                regWorkSheet.FormalCode = courseObj.FormalCode;
                regWorkSheet.VersionCode = courseObj.VersionCode;
                regWorkSheet.ProgramID = studentObj.ProgramID;
                regWorkSheet.CreatedBy = userObj.Id;
                regWorkSheet.CreatedDate = DateTime.Now;
                regWorkSheet.ModifiedBy = userObj.Id;
                regWorkSheet.ModifiedDate = DateTime.Now;
                int result = RegistrationWorksheetManager.Insert(regWorkSheet);

                if (result > 0)
                {
                    string messege = "Course added by advisor ";
                    string courseName = regWorkSheet.CourseTitle;
                    string courseCode = regWorkSheet.FormalCode;
                    string roll = regWorkSheet.Roll;
                    InsertLog(messege, courseName, courseCode, roll);

                    ShowAlertMessage("Course added successfully.");
                    RegistrationWorksheetManager.UpdateCourseRegType(studentObj.StudentID, acaCalId);
                }
                else
                {
                    ShowAlertMessage("Course could not added successfully.");
                }
            }
            catch(Exception ex)
            {
            }
        }
        #endregion Add Course by Advisor
    }
}