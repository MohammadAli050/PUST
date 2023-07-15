using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Admin_ClassRoutine : BasePage
{
    List<ShareBatchInSection> ShareBatchList = new List<ShareBatchInSection>();
    string Session_ShareBatchList = "Session_ShareBatchList";

    List<Batch> batchList = new List<Batch>();
    string Session_BatchList = "Session_BatchList";

    public string flagCourseOffer = "NULL";
    BussinessObject.UIUMSUser userObj = null;
    string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

    protected void Page_Load(object sender, EventArgs e)
    {
        base.CheckPage_Load();
        userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
        if (!IsPostBack)
        {

            //if (userObj.Id == 1)
            //{
            //    chkIsConflict.Visible = true;
            //}
            //else
            //{
            //    chkIsConflict.Visible = false;
            //}
            ucDepartment.LoadDropDownList();
            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            ucProgram.LoadDropdownByDepartmentId(departmentId);
            ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
            ucBatch.LoadDropDownListForClassRoutine(Convert.ToInt32(ucProgram.selectedValue));
            LoadDropDown();
            LoadYearNoDDL();
            LoadSemesterNoDDL();
            //pnPopUp.Visible = false;
            // panelContainer.Enabled = true;
        }
    }

    protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            ucProgram.LoadDropdownByDepartmentId(departmentId);
            ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
            ucBatch.LoadDropDownListForClassRoutine(Convert.ToInt32(ucProgram.selectedValue));
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void LoadDropDown()
    {
        FillSectionGenderCombo();
    }

    private void FillSectionGenderCombo()
    {
        List<Value> collection = ValueManager.GetByValueSetId((int)CommonEnum.ValueSet.SectionGender);

        ddlSectionGender.Items.Clear();
        ddlSectionGender.Items.Add(new ListItem("-Select-", "0"));
        ddlSectionGender.AppendDataBoundItems = true;
        ddlSectionGender.DataValueField = "ValueID";
        ddlSectionGender.DataTextField = "ValueName";

        ddlSectionGender.DataSource = collection;
        ddlSectionGender.DataBind();

    }

    private void LoadClassSchedule()
    {
        int acaCalId = Convert.ToInt32(ucSession.selectedValue);
        int programId = Convert.ToInt32(ucProgram.selectedValue);
        int examId = Convert.ToInt32(ddlExam.SelectedValue);


        List<Course> courseList = CourseManager.GetAllByProgram(programId);
        Hashtable hashCourse = new Hashtable();
        foreach (Course course in courseList)
            hashCourse.Add(course.CourseID.ToString() + "_" + course.VersionID.ToString(), course.FormalCode + ":" + course.Title);

        List<RoomInformation> roomInfoList = RoomInformationManager.GetAll();
        Hashtable hashRoomInfo = new Hashtable();
        if (roomInfoList != null)
        {
            foreach (RoomInformation roomInfo in roomInfoList)
                hashRoomInfo.Add(roomInfo.RoomInfoID, roomInfo.RoomNumber + ": " + roomInfo.Capacity);
        }

        List<Value> valueList = ValueManager.GetAll();
        valueList = valueList.Where(x => x.ValueSetID == 1).ToList();
        Hashtable hashDay = new Hashtable();
        foreach (Value value in valueList)
            hashDay.Add(value.ValueID, value.ValueName);

        List<TimeSlotPlanNew> timeSlotPlanList = TimeSlotPlanManager.GetAll();
        Hashtable hashTimeSlotPlan = new Hashtable();
        foreach (TimeSlotPlanNew timeSlotPlan in timeSlotPlanList)
            hashTimeSlotPlan.Add(timeSlotPlan.TimeSlotPlanID, timeSlotPlan.StartHour + ":" + timeSlotPlan.StartMin + " " + (timeSlotPlan.StartAMPM == 1 ? "AM" : "PM") + "-" + timeSlotPlan.EndHour + ":" + timeSlotPlan.EndMin + " " + (timeSlotPlan.EndAMPM == 1 ? "AM" : "PM"));

        List<Employee> employeeList = EmployeeManager.GetAll();
        Hashtable hashEmployee = new Hashtable();
        foreach (Employee employee in employeeList)
            hashEmployee.Add(employee.EmployeeID, employee.CodeAndName);

        List<Program> programList = ProgramManager.GetAll();
        Hashtable hashProgram = new Hashtable();
        foreach (Program program in programList)
            hashProgram.Add(program.ProgramID, program.ShortName);

        //List<ExamTemplateMaster> gradeSheetTempList = ExamTemplateMasterManager.GetAll();
        //Hashtable hashGradeSheetTemp = new Hashtable();
        //if (gradeSheetTempList != null)
        //{
        //    foreach (ExamTemplateMaster gradeSheetTemp in gradeSheetTempList)
        //        hashGradeSheetTemp.Add(gradeSheetTemp.ExamTemplateMasterId, gradeSheetTemp.ExamTemplateMasterName);
        //}

        List<ExamTemplate> examTemplateList = ExamTemplateManager.GetAll();
        Hashtable hashexamTemplateList = new Hashtable();
        if (examTemplateList != null)
        {
            foreach (ExamTemplate gradeSheetTemp in examTemplateList)
                hashexamTemplateList.Add(gradeSheetTemp.ExamTemplateId, gradeSheetTemp.ExamTemplateName);
        }

        //List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetAll();

        //Ignore Year and Semester.Because Exam has a reference of Year and Semester
        List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetByProgramIdYearNoSemesterNoExamId(programId, 0, 0, examId);
        if (acaCalSectionList.Count > 0)
        {
            string yearNo = ddlYearNo.SelectedItem.ToString();
            string semesterNo = ddlSemesterNo.SelectedItem.ToString();
            string ExamName = ddlExam.SelectedItem.ToString();


            //if (acaCalId != 0)
            //    acaCalSectionList = acaCalSectionList.Where(x => x.AcademicCalenderID == acaCalId).ToList();
            //if (programId != 0)
            //    acaCalSectionList = acaCalSectionList.Where(x => x.ProgramID == programId).ToList();

            for (int i = 0; i < acaCalSectionList.Count; i++)
            {
                string courseIndex = acaCalSectionList[i].CourseID.ToString() + "_" + acaCalSectionList[i].VersionID.ToString();
                acaCalSectionList[i].YearNumber = yearNo;
                acaCalSectionList[i].SemesterNumber = semesterNo;
                acaCalSectionList[i].ExamName = ExamName;
                acaCalSectionList[i].CourseInfo = hashCourse[courseIndex] == null ? "" : hashCourse[courseIndex].ToString();

                acaCalSectionList[i].RoomInfoOne = hashRoomInfo[acaCalSectionList[i].RoomInfoOneID] == null ? "" : hashRoomInfo[acaCalSectionList[i].RoomInfoOneID].ToString();
                acaCalSectionList[i].RoomInfoTwo = hashRoomInfo[acaCalSectionList[i].RoomInfoTwoID] == null ? "" : hashRoomInfo[acaCalSectionList[i].RoomInfoTwoID].ToString();
                acaCalSectionList[i].RoomInfoThree = hashRoomInfo[acaCalSectionList[i].RoomInfoThreeID] == null ? "" : hashRoomInfo[acaCalSectionList[i].RoomInfoThreeID].ToString();

                acaCalSectionList[i].DayInfoOne = hashDay[acaCalSectionList[i].DayOne] == null ? "" : hashDay[acaCalSectionList[i].DayOne].ToString();
                acaCalSectionList[i].DayInfoTwo = hashDay[acaCalSectionList[i].DayTwo] == null ? "" : hashDay[acaCalSectionList[i].DayTwo].ToString();
                acaCalSectionList[i].DayInfoThree = hashDay[acaCalSectionList[i].DayThree] == null ? "" : hashDay[acaCalSectionList[i].DayThree].ToString();

                acaCalSectionList[i].ProgramName = hashProgram[acaCalSectionList[i].ProgramID] == null ? "" : hashProgram[acaCalSectionList[i].ProgramID].ToString();

                acaCalSectionList[i].TimeSlotPlanInfoOne = hashTimeSlotPlan[acaCalSectionList[i].TimeSlotPlanOneID] == null ? "" : hashTimeSlotPlan[acaCalSectionList[i].TimeSlotPlanOneID].ToString();
                acaCalSectionList[i].TimeSlotPlanInfoTwo = hashTimeSlotPlan[acaCalSectionList[i].TimeSlotPlanTwoID] == null ? "" : hashTimeSlotPlan[acaCalSectionList[i].TimeSlotPlanTwoID].ToString();
                acaCalSectionList[i].TimeSlotPlanInfoThree = hashTimeSlotPlan[acaCalSectionList[i].TimeSlotPlanThreeID] == null ? "" : hashTimeSlotPlan[acaCalSectionList[i].TimeSlotPlanThreeID].ToString();

                acaCalSectionList[i].TeacherInfoOne = hashEmployee[acaCalSectionList[i].TeacherOneID] == null ? "" : hashEmployee[acaCalSectionList[i].TeacherOneID].ToString();
                acaCalSectionList[i].TeacherInfoTwo = hashEmployee[acaCalSectionList[i].TeacherTwoID] == null ? "" : hashEmployee[acaCalSectionList[i].TeacherTwoID].ToString();
                acaCalSectionList[i].TeacherInfoThree = hashEmployee[acaCalSectionList[i].TeacherThreeID] == null ? "" : hashEmployee[acaCalSectionList[i].TeacherThreeID].ToString();

                acaCalSectionList[i].GradeSheetTemplate = hashexamTemplateList[acaCalSectionList[i].BasicExamTemplateId] == null ? "" : hashexamTemplateList[acaCalSectionList[i].BasicExamTemplateId].ToString();
            }
            acaCalSectionList = acaCalSectionList.OrderBy(x => x.CourseInfo).ThenBy(x => x.SectionName).ToList();
        }

        if (acaCalSectionList != null)
        {
            SessionManager.SaveListToSession<AcademicCalenderSection>(acaCalSectionList, "_acaCalSectionList");
            LoadGrid(acaCalSectionList);
        }

        //gvClassSchedule.DataSource = acaCalSectionList;
        //gvClassSchedule.DataBind();
    }

    private void LoadComboBox(int programId, int BasicExamTemplateId)
    {
        try
        {
            FillSectionGenderCombo();

            List<Course> courseList = CourseManager.GetAllByProgram(programId);
            if (courseList != null)
            {
                courseList = courseList.OrderBy(c => c.FormalCode).ToList();
            }

            Hashtable hashCourse = new Hashtable();
            foreach (Course course in courseList)
                hashCourse.Add(course.CourseID.ToString() + "_" + course.VersionID.ToString(), course.FormalCode + ":" + course.Title + " " + course.Credits.ToString());

            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            ddlCourse.Items.Clear();
            ddlCourse.Items.Add(new ListItem("-Select-", "0"));
            List<OfferedCourse> offeredCourseList = OfferedCourseManager.GetAll().Where(x => x.YearNo == yearNo && x.SemesterNo == semesterNo && x.ExamId == examId && x.ProgramID == programId).ToList();

            if (offeredCourseList.Count > 0 && offeredCourseList != null)
            {
                offeredCourseList = offeredCourseList.Where(c => c.IsActive == true).ToList();
            }

            if (courseList != null && courseList.Count > 0)
            {
                courseList = courseList.OrderBy(c => c.FormalCode).ToList();

                flagCourseOffer = "Offer";

                foreach (Course course in courseList)
                {
                    //string formalCodeTitle = offeredCourse.CourseID + "_" + offeredCourse.VersionID;
                    ddlCourse.Items.Add(new ListItem(course.FormalCode.ToString() + " - " + course.Title.ToString(), course.CourseID + "_" + course.VersionID));
                }
            }
            else
            {
                flagCourseOffer = "NotOffer";
            }

            ddlRoomInfo1.Items.Clear();
            ddlRoomInfo2.Items.Clear();
            ddlRoomInfo3.Items.Clear();
            ddlRoomInfo1.Items.Add(new ListItem("-Select-", "0"));
            ddlRoomInfo2.Items.Add(new ListItem("-Select-", "0"));
            ddlRoomInfo3.Items.Add(new ListItem("-Select-", "0"));

            List<RoomInformation> roomInfoList = RoomInformationManager.GetAll();
            if (roomInfoList.Count > 0)
                foreach (RoomInformation roomInfo in roomInfoList)
                {
                    ddlRoomInfo1.Items.Add(new ListItem(roomInfo.RoomNumber + ": " + roomInfo.Capacity, roomInfo.RoomInfoID.ToString()));
                    ddlRoomInfo2.Items.Add(new ListItem(roomInfo.RoomNumber + ": " + roomInfo.Capacity, roomInfo.RoomInfoID.ToString()));
                    ddlRoomInfo3.Items.Add(new ListItem(roomInfo.RoomNumber + ": " + roomInfo.Capacity, roomInfo.RoomInfoID.ToString()));
                }

            ddlDay1.Items.Clear();
            ddlDay2.Items.Clear();
            ddlDay3.Items.Clear();
            ddlDay1.Items.Add(new ListItem("-Select-", "0"));
            ddlDay2.Items.Add(new ListItem("-Select-", "0"));
            ddlDay3.Items.Add(new ListItem("-Select-", "0"));

            List<Value> valueList = ValueManager.GetAll().Where(x => x.ValueSetID == 1).ToList();
            if (valueList.Count > 0)
                foreach (Value value in valueList)
                {
                    ddlDay1.Items.Add(new ListItem(value.ValueName, value.ValueID.ToString()));
                    ddlDay2.Items.Add(new ListItem(value.ValueName, value.ValueID.ToString()));
                    ddlDay3.Items.Add(new ListItem(value.ValueName, value.ValueID.ToString()));
                }

            ddlTimeSlot1.Items.Clear();
            ddlTimeSlot2.Items.Clear();
            ddlTimeSlot3.Items.Clear();
            ddlTimeSlot1.Items.Add(new ListItem("-Select-", "0"));
            ddlTimeSlot2.Items.Add(new ListItem("-Select-", "0"));
            ddlTimeSlot3.Items.Add(new ListItem("-Select-", "0"));

            List<TimeSlotPlanNew> timeSlotPlanList = TimeSlotPlanManager.GetAll();
            if (timeSlotPlanList.Count > 0)
            {
                foreach (TimeSlotPlanNew timeSlotPlan in timeSlotPlanList)
                {
                    ddlTimeSlot1.Items.Add(new ListItem(timeSlotPlan.StartHourText + ":" + timeSlotPlan.StartMinText + " " + (timeSlotPlan.StartAMPM == 1 ? "AM" : "PM") + "-" + timeSlotPlan.EndHourText + ":" + timeSlotPlan.EndMinText + " " + (timeSlotPlan.EndAMPM == 1 ? "AM" : "PM"), timeSlotPlan.TimeSlotPlanID.ToString()));
                    ddlTimeSlot2.Items.Add(new ListItem(timeSlotPlan.StartHourText + ":" + timeSlotPlan.StartMinText + " " + (timeSlotPlan.StartAMPM == 1 ? "AM" : "PM") + "-" + timeSlotPlan.EndHourText + ":" + timeSlotPlan.EndMinText + " " + (timeSlotPlan.EndAMPM == 1 ? "AM" : "PM"), timeSlotPlan.TimeSlotPlanID.ToString()));
                    ddlTimeSlot3.Items.Add(new ListItem(timeSlotPlan.StartHourText + ":" + timeSlotPlan.StartMinText + " " + (timeSlotPlan.StartAMPM == 1 ? "AM" : "PM") + "-" + timeSlotPlan.EndHourText + ":" + timeSlotPlan.EndMinText + " " + (timeSlotPlan.EndAMPM == 1 ? "AM" : "PM"), timeSlotPlan.TimeSlotPlanID.ToString()));
                }
            }

            ddlFaculty1.Items.Clear();
            ddlFaculty2.Items.Clear();
            ddlFaculty3.Items.Clear();
            ddlFaculty1.Items.Add(new ListItem("-Select-", "0"));
            ddlFaculty2.Items.Add(new ListItem("-Select-", "0"));
            ddlFaculty3.Items.Add(new ListItem("-Select-", "0"));

            List<Employee> employeeList = EmployeeManager.GetAll();
            if (employeeList.Count > 0)
            {
                employeeList = employeeList.OrderBy(o => o.Code).ToList();

                foreach (Employee employee in employeeList)
                {
                    ddlFaculty1.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
                    ddlFaculty2.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
                    ddlFaculty3.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
                }
            }

            List<ExamTemplate> examTemplateList = ExamTemplateManager.GetAll();

            ddlExamTemplate.DataTextField = "ExamTemplateName";
            ddlExamTemplate.DataValueField = "ExamTemplateId";
            ddlExamTemplate.AppendDataBoundItems = true;
            ddlExamTemplate.Items.Clear();
            ddlExamTemplate.Items.Add(new ListItem("-Select Exam Template-", "0"));
            if (examTemplateList != null)
            {
                ddlExamTemplate.DataSource = examTemplateList;
                ddlExamTemplate.DataBind();
            }
        }
        catch (Exception ex)
        {
            lblMsg.Text = "Error: 103(Dropdown Load)";
        }
        finally { }
    }

    protected void lbAdd_Click(object sender, EventArgs e)
    {
        if (ucSession.selectedValue == "0" || ucProgram.selectedValue == "0")
        {
            lblMsg.Text = "Please Select Batch and Program.";
            return;
        }

        else if (flagCourseOffer == "NotOffer")
        {
            lblMsg.Text = "Course is not Offered.";
            return;
        }

        lblMsg.Text = "";

        btnUpdateInsert.Text = "New Add";

        LoadComboBox(Convert.ToInt32(ucProgram.selectedValue), 0);
    }

    protected void lbEdit_Click(object sender, EventArgs e)
    {

        try
        {
            ShowPopUpBoxMessage(" ");
            lblConflictMessage.Text = "";
            chkIsConflict.Checked = false;
            btnUpdateInsert.Text = "Update";

            LinkButton linkButton = new LinkButton();
            linkButton = (LinkButton)sender;
            int id = Convert.ToInt32(linkButton.CommandArgument);
            btnUpdateInsert.CommandArgument = id.ToString();
            lblAcaCalSectionId.Text = id.ToString();
            //ucBatch.SelectedIndex0();
            lblShareBatch.Text = string.Empty;
            DepartmentUserControl1.SelectedValue(0);
            DepartmentUserControl2.SelectedValue(0);
            DepartmentUserControl3.SelectedValue(0);


            AcademicCalenderSection acaCalSection = AcademicCalenderSectionManager.GetById(id);
            LoadComboBox(acaCalSection.ProgramID, acaCalSection.BasicExamTemplateId);
            LoadYearSemesterExamComboBox(acaCalSection.ProgramID, acaCalSection.ExamId);
            int acaCalId = Convert.ToInt32(ucSession.selectedValue);
            int programId = Convert.ToInt32(ucProgram.selectedValue);

            ddlCourse.SelectedValue = ddlCourse.Items.Count > 0 ? acaCalSection.CourseID + "_" + acaCalSection.VersionID : "0";
            ddlCourse.Enabled = false;

            txtSection.Text = acaCalSection.SectionName;
            txtCapacity.Text = acaCalSection.Capacity.ToString();

            ddlRoomInfo1.SelectedValue = acaCalSection.RoomInfoOneID != 0 ? acaCalSection.RoomInfoOneID.ToString() : "0";
            ddlDay1.SelectedValue = acaCalSection.DayOne != 0 ? acaCalSection.DayOne.ToString() : "0";
            ddlTimeSlot1.SelectedValue = acaCalSection.TimeSlotPlanOneID != 0 ? acaCalSection.TimeSlotPlanOneID.ToString() : "0";
            ddlFaculty1.SelectedValue = acaCalSection.TeacherOneID != 0 ? acaCalSection.TeacherOneID.ToString() : "0";

            ddlRoomInfo2.SelectedValue = acaCalSection.RoomInfoTwoID != 0 ? acaCalSection.RoomInfoTwoID.ToString() : "0";
            ddlDay2.SelectedValue = acaCalSection.DayTwo != 0 ? acaCalSection.DayTwo.ToString() : "0";
            ddlTimeSlot2.SelectedValue = acaCalSection.TimeSlotPlanTwoID != 0 ? acaCalSection.TimeSlotPlanTwoID.ToString() : "0";
            ddlFaculty2.SelectedValue = acaCalSection.TeacherTwoID != 0 ? acaCalSection.TeacherTwoID.ToString() : "0";

            ddlRoomInfo3.SelectedValue = acaCalSection.RoomInfoThreeID != 0 ? acaCalSection.RoomInfoThreeID.ToString() : "0";
            ddlDay3.SelectedValue = acaCalSection.DayThree != 0 ? acaCalSection.DayThree.ToString() : "0";
            ddlTimeSlot3.SelectedValue = acaCalSection.TimeSlotPlanThreeID != 0 ? acaCalSection.TimeSlotPlanThreeID.ToString() : "0";
            ddlFaculty3.SelectedValue = acaCalSection.TeacherThreeID != 0 ? acaCalSection.TeacherThreeID.ToString() : "0";

            // ddlGradeSheetTemplate.SelectedValue = "0";// acaCalSection.GradeSheetTemplateID != 0 ? acaCalSection.GradeSheetTemplateID.ToString() : "0";
            txtSectionDefination.Text = acaCalSection.SectionDefination;
            txtRemark.Text = acaCalSection.Remarks;
            ddlExamTemplate.SelectedValue = acaCalSection.BasicExamTemplateId.ToString();
            ddlSectionGender.SelectedValue = acaCalSection.SectionGenderID.ToString();

            #region Department

            Employee emp = EmployeeManager.GetById(Convert.ToInt32(ddlFaculty1.SelectedValue));
            if (emp != null)
                DepartmentUserControl1.SelectedValue(emp.DeptID);

            Employee emp2 = EmployeeManager.GetById(Convert.ToInt32(ddlFaculty2.SelectedValue));
            if (emp2 != null)
                DepartmentUserControl2.SelectedValue(emp2.DeptID);

            Employee emp3 = EmployeeManager.GetById(Convert.ToInt32(ddlFaculty3.SelectedValue));
            if (emp3 != null)
                DepartmentUserControl3.SelectedValue(emp3.DeptID);
            #endregion

            if (acaCalSection.ShareBatch != null && acaCalSection.ShareBatch.Count != 0)
            {
                SessionManager.SaveListToSession<Batch>(acaCalSection.ShareBatch, Session_BatchList);
                ShowBatchList(acaCalSection.ShareBatch);
            }
            else
            {
                SessionManager.DeletFromSession(Session_BatchList);
            }

            ModalPopupExtender1.Show();
        }
        catch { lblMsg.Text = "Error: 104 This course isn't offered yet!"; }
        //(Load Select Row)
        finally { }
    }

    private void LoadYearSemesterExamComboBox(int ProgramId, int ExamId)
    {
        LoadModalYear();
        LoadModalSemester();
        ExamSetupDetail esd = ExamSetupDetailManager.GetById(ExamId);
        if (esd != null)
        {
            ExamSetup es = ExamSetupManager.GetById(Convert.ToInt32(esd.ExamSetupId));
            if (es != null)
            {
                LoadModalExamDropdown(ProgramId, Convert.ToInt32(es.YearNo), Convert.ToInt32(esd.SemesterNo));
                if (ddlModalYear.Items.FindByValue(es.YearNo.ToString()) != null)
                    ddlModalYear.SelectedValue = es.YearNo.ToString();
                if (ddlModalSemester.Items.FindByValue(esd.SemesterNo.ToString()) != null)
                    ddlModalSemester.SelectedValue = esd.SemesterNo.ToString();
                if (ddlModalExam.Items.FindByValue(ExamId.ToString()) != null)
                    ddlModalExam.SelectedValue = ExamId.ToString();
            }
        }
    }

    private void LoadModalYear()
    {
        List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
        yearList = YearManager.GetAllDistinct();
        yearList = yearList.OrderBy(x => x.YearNo).ToList();


        ddlModalYear.Items.Clear();
        ddlModalYear.AppendDataBoundItems = true;
        ddlModalYear.Items.Add(new ListItem("-Select-", "-1"));
        if (yearList != null && yearList.Count > 0)
        {
            ddlModalYear.DataTextField = "YearNoName";
            ddlModalYear.DataValueField = "YearNo";

            ddlModalYear.DataSource = yearList;
            ddlModalYear.DataBind();
        }
    }

    private void LoadModalSemester()
    {
        List<SemesterDistinctDTO> semesterList = new List<SemesterDistinctDTO>();
        semesterList = SemesterManager.GetAllDistinct();
        semesterList = semesterList.OrderBy(x => x.SemesterNo).ToList();


        ddlModalSemester.Items.Clear();
        ddlModalSemester.AppendDataBoundItems = true;
        ddlModalSemester.Items.Add(new ListItem("-Select-", "-1"));
        if (semesterList != null && semesterList.Count > 0)
        {
            ddlModalSemester.DataTextField = "SemesterNoName";
            ddlModalSemester.DataValueField = "SemesterNo";

            ddlModalSemester.DataSource = semesterList;
            ddlModalSemester.DataBind();

        }
    }

    private void LoadModalExamDropdown(int programId, int yearNo, int semesterNo)
    {
        ddlModalExam.Items.Clear();
        ddlModalExam.Items.Add(new ListItem("-Select Exam-", "0"));
        ddlModalExam.AppendDataBoundItems = true;
        List<ExamSetupDetailDTO> exam = ExamSetupManager.ExamSetupDetailGetProgramIdYearNoSemesterNo(programId, yearNo, semesterNo);
        if (exam != null)
        {
            foreach (ExamSetupDetailDTO examlist in exam)
            {
                ddlModalExam.Items.Add(new ListItem(examlist.ExamName, examlist.ExamSetupDetailId.ToString()));
            }
        }
    }

    protected void ddlModalYear_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        LoadModalExamDropdown(Convert.ToInt32(ucProgram.selectedValue), Convert.ToInt32(ddlModalYear.SelectedValue), Convert.ToInt32(ddlSemesterNo.SelectedValue));
    }

    protected void ddlModalSemester_SelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        LoadModalExamDropdown(Convert.ToInt32(ucProgram.selectedValue), Convert.ToInt32(ddlModalYear.SelectedValue), Convert.ToInt32(ddlSemesterNo.SelectedValue));
    }

    protected void lbDelete_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = new LinkButton();
        linkButton = (LinkButton)sender;
        int id = Convert.ToInt32(linkButton.CommandArgument);
        int occupiedCount = AcademicCalenderSectionManager.GetById(id).Occupied;
        AcademicCalenderSection acacalsec = AcademicCalenderSectionManager.GetById(id);
        Course _course = CourseManager.GetByCourseIdVersionId(acacalsec.CourseID, acacalsec.VersionID);

        if (occupiedCount <= 0)
        {
            bool result = AcademicCalenderSectionManager.Delete(id);
            if (result)
            {
                lblMsg.Text = "Section deleted successfully.";
                #region Log Insert
                // Student student = StudentManager.GetById(registrationWorksheet.StudentID);
                LogGeneralManager.Insert(
                                                     DateTime.Now,
                                                     BaseAcaCalCurrent.Code,
                                                     BaseAcaCalCurrent.FullCode,
                                                     BaseCurrentUserObj.LogInID,
                                                     "",
                                                     "",
                                                     "deleted Section",
                                                     BaseCurrentUserObj.LogInID + " deleted Section for : " + _course.FormalCode + " : " + _course.Title + " , section :" + acacalsec.SectionName,
                                                     "normal",
                                                      ((int)CommonEnum.PageName.CourseExplorer).ToString(),
                                                     CommonEnum.PageName.CourseExplorer.ToString(),
                                                     _pageUrl,
                                                     "");
                #endregion
            }
            else
            {
                lblMsg.Text = "Section could not deleted successfully.";
            }
        }
        else
        {
            lblMsg.Text = "Section could not deleted because it is occupied by " + occupiedCount + " student.";
        }
        LoadClassSchedule();
    }

    private bool Update(int id)
    {
        bool resultUpdate = false;

        //string conflictlog = "";
        //string roomConflict = "";

        AcademicCalenderSection acaCalSection = AcademicCalenderSectionManager.GetById(id);
        //if (acaCalSection != null)
        //{
        //    //need to add mark check
        //    // is mark submitted using this template or not
        //    // code would be found in Gono University Class routine solution

        //    User user = UserManager.GetByLogInId(userObj.LogInID);
        //    Role userRole = RoleManager.GetById(user.RoleID);
        //    // 
        //    if (userRole.RoleName != "Admin" && acaCalSection.BasicExamTemplateId != Convert.ToInt32(ddlExamTemplate.SelectedValue))
        //    {
        //        lblMsg.Text = "Grade Template cannot be changed";
        //        return false;
        //    }
        //}

        //List<int> programIds = new List<int>();

        acaCalSection.SectionName = txtSection.Text;
        acaCalSection.Capacity = txtCapacity.Text.Trim() == "" ? 0 : Convert.ToInt32(txtCapacity.Text);

        //acaCalSection.ExamId = Convert.ToInt32(ddlModalExam.SelectedValue);

        acaCalSection.RoomInfoOneID = Convert.ToInt32(ddlRoomInfo1.SelectedValue);
        acaCalSection.DayOne = Convert.ToInt32(ddlDay1.SelectedValue);
        acaCalSection.TimeSlotPlanOneID = Convert.ToInt32(ddlTimeSlot1.SelectedValue);
        acaCalSection.TeacherOneID = Convert.ToInt32(ddlFaculty1.SelectedValue);

        acaCalSection.RoomInfoTwoID = Convert.ToInt32(ddlRoomInfo2.SelectedValue);
        acaCalSection.DayTwo = Convert.ToInt32(ddlDay2.SelectedValue);
        acaCalSection.TimeSlotPlanTwoID = Convert.ToInt32(ddlTimeSlot2.SelectedValue);
        acaCalSection.TeacherTwoID = Convert.ToInt32(ddlFaculty2.SelectedValue);

        acaCalSection.RoomInfoThreeID = Convert.ToInt32(ddlRoomInfo3.SelectedValue);
        acaCalSection.DayThree = Convert.ToInt32(ddlDay3.SelectedValue);
        acaCalSection.TimeSlotPlanThreeID = Convert.ToInt32(ddlTimeSlot3.SelectedValue);
        acaCalSection.TeacherThreeID = Convert.ToInt32(ddlFaculty3.SelectedValue);

        acaCalSection.BasicExamTemplateId = Convert.ToInt32(ddlExamTemplate.SelectedValue); // Convert.ToInt32(ddlGradeSheetTemplate.SelectedValue);
        acaCalSection.CalculativeExamTemplateId = 0; ;
        acaCalSection.SectionDefination = txtSectionDefination.Text;
        acaCalSection.SectionGenderID = Convert.ToInt32(ddlSectionGender.SelectedValue);
        acaCalSection.OnlineGradeSheetTemplateID = 0;
        acaCalSection.Remarks = txtRemark.Text.Trim();

        acaCalSection.ModifiedBy = userObj.Id;
        acaCalSection.ModifiedDate = DateTime.Now;

        bool result = AcademicCalenderSectionManager.Update(acaCalSection);
        #region teacherone assign 1st examiner

        ExaminorSetups obj = ExaminorSetupsManager.GetByAcaCalSecId(acaCalSection.AcaCal_SectionID);

        if (obj != null && obj.FirstExaminer != acaCalSection.TeacherOneID)
        {
            obj.FirstExaminer = acaCalSection.TeacherOneID;
            obj.ModifiedBy = userObj.Id;
            obj.ModifiedDate = DateTime.Now;
            ExaminorSetupsManager.Update(obj);
        }



        #endregion
        if (result)
        {
            //Course _course = CourseManager.GetByCourseIdVersionId(acaCalSection.CourseID, acaCalSection.VersionID);
            //#region Log Insert
            //// Student student = StudentManager.GetById(registrationWorksheet.StudentID);
            //LogGeneralManager.Insert(
            //                                        DateTime.Now,
            //                                        BaseAcaCalCurrent.Code,
            //                                        BaseAcaCalCurrent.FullCode,
            //                                        BaseCurrentUserObj.LogInID,
            //                                        _course.FormalCode,
            //                                        "",
            //                                        "update Section",
            //                                        BaseCurrentUserObj.LogInID + " update Section for : " + _course.FormalCode + " : " + _course.Title + " , section :" + acaCalSection.SectionName + conflictlog + ", GradeTemplate: " + ddlExamTemplate.SelectedItem.Text + " Session: " + ucSession.selectedText,
            //                                        "normal",
            //                                        ((int)CommonEnum.PageName.ClassRoutine).ToString(),
            //                                        CommonEnum.PageName.ClassRoutine.ToString(),
            //                                        _pageUrl,
            //                                        "");
            //#endregion

            resultUpdate = true;
        }
        return resultUpdate;
    }

    private bool Insert()
    {
        bool result = false;
        string roomConflict = "";
        string conflictlog = "";

        string course = ddlCourse.SelectedValue;
        string[] courseVersion = course.Split('_');
        AcademicCalenderSection acaCalSection = new AcademicCalenderSection();
        List<int> programIds = new List<int>();

        Program program = ProgramManager.GetById(Convert.ToInt32(ucProgram.selectedValue));
        acaCalSection.AcademicCalenderID = Convert.ToInt32(ucSession.selectedValue);
        acaCalSection.ProgramID = program.ProgramID;
        acaCalSection.DeptID = program.DeptID;
        acaCalSection.CourseID = Convert.ToInt32(courseVersion[0]);
        acaCalSection.VersionID = Convert.ToInt32(courseVersion[1]);
        acaCalSection.SectionName = txtSection.Text;
        acaCalSection.Capacity = txtCapacity.Text.Trim() == "" ? 0 : Convert.ToInt32(txtCapacity.Text);

        acaCalSection.RoomInfoOneID = Convert.ToInt32(ddlRoomInfo1.SelectedValue);
        acaCalSection.DayOne = Convert.ToInt32(ddlDay1.SelectedValue);
        acaCalSection.TimeSlotPlanOneID = Convert.ToInt32(ddlTimeSlot1.SelectedValue);
        acaCalSection.TeacherOneID = Convert.ToInt32(ddlFaculty1.SelectedValue);

        acaCalSection.RoomInfoTwoID = Convert.ToInt32(ddlRoomInfo2.SelectedValue);
        acaCalSection.DayTwo = Convert.ToInt32(ddlDay2.SelectedValue);
        acaCalSection.TimeSlotPlanTwoID = Convert.ToInt32(ddlTimeSlot2.SelectedValue);
        acaCalSection.TeacherTwoID = Convert.ToInt32(ddlFaculty2.SelectedValue);

        acaCalSection.RoomInfoThreeID = Convert.ToInt32(ddlRoomInfo3.SelectedValue);
        acaCalSection.DayThree = Convert.ToInt32(ddlDay3.SelectedValue);
        acaCalSection.TimeSlotPlanThreeID = Convert.ToInt32(ddlTimeSlot3.SelectedValue);
        acaCalSection.TeacherThreeID = Convert.ToInt32(ddlFaculty3.SelectedValue);

        acaCalSection.SectionDefination = txtSectionDefination.Text;
        acaCalSection.SectionGenderID = Convert.ToInt32(ddlSectionGender.SelectedValue);
        acaCalSection.OnlineGradeSheetTemplateID = 0;
        acaCalSection.BasicExamTemplateId = Convert.ToInt32(ddlExamTemplate.SelectedValue);
        acaCalSection.CalculativeExamTemplateId = 0;
        acaCalSection.Remarks = txtRemark.Text.Trim();

        acaCalSection.CreatedBy = userObj.Id;
        acaCalSection.CreatedDate = DateTime.Now;
        acaCalSection.ModifiedBy = userObj.Id;
        acaCalSection.ModifiedDate = DateTime.Now;

        int id = AcademicCalenderSectionManager.Insert(acaCalSection);
        //AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(id);

        if (id > 0)
        {
            //#region Log Insert
            //Course courseObj = CourseManager.GetByCourseIdVersionId(Convert.ToInt32(courseVersion[0]), Convert.ToInt32(courseVersion[1]));
            //string courseName = ddlCourse.SelectedItem.Text;
            //LogGeneralManager.Insert(
            //                                        DateTime.Now,
            //                                        BaseAcaCalCurrent.Code,
            //                                        BaseAcaCalCurrent.FullCode,
            //                                        BaseCurrentUserObj.LogInID,
            //                                        courseObj.FormalCode,
            //                                        "",
            //                                        "Add Section",
            //                                        BaseCurrentUserObj.LogInID + " add Section for course : " + courseName + " , " + acaCalSection.SectionName + conflictlog + " , GradeTemplate: " + ddlExamTemplate.SelectedItem.Text + " Session: " + ucSession.selectedText,
            //                                        "normal",
            //                                        ((int)CommonEnum.PageName.ClassRoutine).ToString(),
            //                                        CommonEnum.PageName.ClassRoutine.ToString(),
            //                                        _pageUrl,
            //                                        "");
            //#endregion

            LoadClassSchedule();
            result = true;
        }
        return result;
    }

    private string CheckRoomConflict()
    {
        try
        {
            string conflict1 = "";
            string conflict2 = "";
            string conflict3 = "";


            int RoomInfoOneID = Convert.ToInt32(ddlRoomInfo1.SelectedValue);
            int DayOne = Convert.ToInt32(ddlDay1.SelectedValue);
            int TimeSlotPlanOneID = Convert.ToInt32(ddlTimeSlot1.SelectedValue);
            int TeacherOneID = Convert.ToInt32(ddlFaculty1.SelectedValue);

            int RoomInfoTwoID = Convert.ToInt32(ddlRoomInfo2.SelectedValue);
            int DayTwo = Convert.ToInt32(ddlDay2.SelectedValue);
            int TimeSlotPlanTwoID = Convert.ToInt32(ddlTimeSlot2.SelectedValue);
            int TeacherTwoID = Convert.ToInt32(ddlFaculty2.SelectedValue);

            int RoomInfoThreeID = Convert.ToInt32(ddlRoomInfo3.SelectedValue);
            int DayThree = Convert.ToInt32(ddlDay3.SelectedValue);
            int TimeSlotPlanThreeID = Convert.ToInt32(ddlTimeSlot3.SelectedValue);
            int TeacherThreeID = Convert.ToInt32(ddlFaculty3.SelectedValue);


            if (TeacherOneID == TeacherTwoID && DayOne == DayTwo && TimeSlotPlanOneID == TimeSlotPlanTwoID && RoomInfoOneID != RoomInfoTwoID)
            {
                conflict1 = "Same Info with different Room. Please Provide Correct Info with Slot 1 & 2. ";
            }
            if (TeacherOneID == TeacherThreeID && DayOne == DayThree && TimeSlotPlanOneID == TimeSlotPlanThreeID && RoomInfoOneID != RoomInfoThreeID)
            {
                conflict2 = "Same Info with different Room. Please Provide Correct Info with Slot 1 & 3. ";
            }
            if (TeacherTwoID == TeacherThreeID && DayTwo == DayThree && TimeSlotPlanTwoID == TimeSlotPlanThreeID && RoomInfoTwoID != RoomInfoThreeID)
            {
                conflict3 = "Same Info with different Room. Please Provide Correct Info with Slot 2 & 3. ";
            }

            return conflict1 + conflict2 + conflict3;

        }
        catch (Exception)
        {
            return "";
        }
    }

    protected void btnUpdateInsert_Click(object sender, EventArgs e)
    {
        int id = 0;

        ModalPopupExtender1.Hide();
        lblConflictMessage.Text = "";

        if (btnUpdateInsert.CommandArgument != "")
            id = Convert.ToInt32(btnUpdateInsert.CommandArgument);

        if (!string.IsNullOrEmpty(lblAcaCalSectionId.Text))
        {
            int acaCalSecId = Convert.ToInt32(lblAcaCalSectionId.Text);
            id = acaCalSecId;
        }

        if (ddlCourse.SelectedValue == "0")
        {
            lblMsg.Text = "Please select course.";
            lblMsg.Focus();
            return;
        }

        if (string.IsNullOrEmpty(txtSection.Text))
        {
            lblMsg.Text = "Please insert section name.";
            lblMsg.Focus();
            return;
        }

        if (btnUpdateInsert.Text == "Update")
        {
            btnUpdateInsert.CommandArgument = "";
            if (Update(id))
            {
                lblMsg.Text = "Section updated successfully.";
                lblMsg.Focus();
            }
        }
        else if (btnUpdateInsert.Text == "New Add")
            Insert();

        //RefreshPopUpField();
        // pnPopUp.Visible = false;
        // panelContainer.Enabled = true;
        // ddlCourse.Enabled = true;
        LoadClassSchedule();
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        ddlCourse.Enabled = true;
        // pnPopUp.Visible = false;
        //panelContainer.Enabled = true;
        RefreshPopUpField();
    }

    private void RefreshPopUpField()
    {
        ddlCourse.Items.Clear();
        txtSection.Text = "";
        txtSectionDefination.Text = "";
        txtCapacity.Text = "";
        ddlSectionGender.SelectedIndex = 0;

        ddlRoomInfo1.Items.Clear();
        ddlDay1.Items.Clear();
        ddlTimeSlot1.Items.Clear();
        ddlFaculty1.Items.Clear();

        ddlRoomInfo2.Items.Clear();
        ddlDay2.Items.Clear();
        ddlTimeSlot2.Items.Clear();
        ddlFaculty2.Items.Clear();

        ddlRoomInfo3.Items.Clear();
        ddlDay3.Items.Clear();
        ddlTimeSlot3.Items.Clear();
        ddlFaculty3.Items.Clear();

        ddlExamTemplate.Items.Clear();
        ucBatch.SelectedIndex0();
        lblShareBatch.Text = string.Empty;
        lblConflictMessage.Text = "";
    }

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        if (ucProgram.selectedValue == "0" || ddlExam.SelectedValue == "0")
        {
            lblMsg.Text = "Please select Program and ExamName";
            List<AcademicCalenderSection> acaCalSectionList = new List<AcademicCalenderSection>();
            gvClassSchedule.DataSource = acaCalSectionList;
            gvClassSchedule.DataBind();
            return;
        }

        lblMsg.Text = "";
        LoadClassSchedule();
    }

    protected void ddlSearch_Change(object sender, EventArgs e)
    {
        //string courseIDversionID = ddlSearch.SelectedValue;
        //if (courseIDversionID == "0")
        //    btnLoad_Click(null, null);
        //LoadClassSchedule();
    }

    //protected void btnCopyTo_Click(object sender, EventArgs e)
    //{
    //    int counterFlag = 0;
    //    try
    //    {
    //        if (ucSession.selectedValue == "0" || ucProgram.selectedValue == "0" || ddlAcaCalBatchCopyTo.SelectedValue == "0")
    //            lblMsg.Text = "Please Select Semester, Program and Copy To <b><i>Dropdown</i></b>";
    //        else
    //        {
    //            int copyFromSemesterId = Convert.ToInt32(ucSession.selectedValue);
    //            int copyToSemesterId = Convert.ToInt32(ddlAcaCalBatchCopyTo.SelectedValue);
    //            int programId = Convert.ToInt32(ucProgram.selectedValue);

    //            List<AcademicCalenderSection> academicCalenderSectionList = AcademicCalenderSectionManager.GetAllByAcaCalId(copyToSemesterId);
    //            if (academicCalenderSectionList.Count > 0 && academicCalenderSectionList != null)
    //                academicCalenderSectionList = academicCalenderSectionList.Where(x => x.ProgramID == programId).ToList();

    //            if (academicCalenderSectionList.Count > 0 && academicCalenderSectionList != null)
    //            {
    //                lblMsg.Text = "<b>Already Copy A Routine For This Semester.</b>";
    //            }
    //            else
    //            {
    //                List<AcademicCalenderSection> acaCalSecList = AcademicCalenderSectionManager.GetAllByAcaCalId(copyFromSemesterId);
    //                if (acaCalSecList.Count > 0 && acaCalSecList != null)
    //                {
    //                    acaCalSecList = acaCalSecList.Where(x => x.ProgramID == programId).ToList();
    //                    if (acaCalSecList.Count > 0 && acaCalSecList != null)
    //                    {
    //                        foreach (AcademicCalenderSection acaCalSec in acaCalSecList)
    //                        {
    //                            acaCalSec.AcademicCalenderID = copyToSemesterId;
    //                            int resultInsert = AcademicCalenderSectionManager.Insert(acaCalSec);
    //                            if (resultInsert > 0)
    //                                counterFlag++;
    //                        }
    //                    }
    //                    else
    //                        lblMsg.Text = "This Semester and Program has <b>NO</b> routine.";
    //                }
    //                else
    //                    lblMsg.Text = "This Semester has <b>NO</b> routine.";
    //            }
    //        }
    //    }
    //    catch { }
    //    finally
    //    {
    //        if (counterFlag != 0)
    //            lblMsg.Text = "<b>" + counterFlag + " Section Insert For Semester " + ddlAcaCalBatchCopyTo.SelectedItem.Text + "</b>";
    //    }
    //}

    protected void btnSearch_Click(object sender, EventArgs e)
    {
        List<AcademicCalenderSection> list = SessionManager.GetListFromSession<AcademicCalenderSection>("_acaCalSectionList");

        if (list != null)
        {
            LoadGrid(list);
        }
    }

    private void LoadGrid(List<AcademicCalenderSection> list)
    {
        if (!string.IsNullOrEmpty(txtFilterCourse.Text.Trim()))
        {
            list = list.Where(c => c.CourseInfo.ToLower().Contains(txtFilterCourse.Text.ToLower().Trim())).ToList();
        }

        if (!string.IsNullOrEmpty(txtFilterRoom.Text.Trim()))
        {
            list = list.Where(c => c.RoomInfoOne.ToLower().Contains(txtFilterRoom.Text.ToLower().Trim()) ||
                c.RoomInfoTwo.ToLower().Contains(txtFilterRoom.Text.ToLower().Trim())).ToList();
        }

        if (!string.IsNullOrEmpty(txtFilterDay.Text.Trim()))
        {
            list = list.Where(c => c.DayInfoOne.ToLower().Contains(txtFilterDay.Text.ToLower().Trim()) ||
                c.DayInfoTwo.ToLower().Contains(txtFilterDay.Text.ToLower().Trim())).ToList();
        }

        if (!string.IsNullOrEmpty(txtFilteTimeSlotn.Text.Trim()))
        {
            list = list.Where(c => c.TimeSlotPlanInfoOne.ToLower().Contains(txtFilteTimeSlotn.Text.ToLower().Trim()) ||
                c.TimeSlotPlanInfoTwo.ToLower().Contains(txtFilteTimeSlotn.Text.ToLower().Trim())).ToList();
        }

        if (!string.IsNullOrEmpty(txtFilterFaculty.Text.Trim()))
        {
            list = list.Where(c => c.TeacherInfoOne.ToLower().Contains(txtFilterFaculty.Text.ToLower().Trim()) ||
                c.TeacherInfoTwo.ToLower().Contains(txtFilterFaculty.Text.ToLower().Trim())).ToList();
        }

        gvClassSchedule.DataSource = list;
        gvClassSchedule.DataBind();
    }

    protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
    {
        ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
        ucBatch.LoadDropDownListForClassRoutine(Convert.ToInt32(ucProgram.selectedValue));
        //ucCopySession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));

    }

    protected void OnBatchSelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
    }

    protected void lblR1_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = new LinkButton();
        linkButton = (LinkButton)sender;
        int id = Convert.ToInt32(linkButton.CommandArgument);

        #region Check Conflict

        AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(id);
        if (academicCalenderSection.RoomInfoOneID > 0)
            AcademicCalenderSectionManager.CheckConflictByRoom(academicCalenderSection, academicCalenderSection.RoomInfoOneID);

        #endregion

        LoadClassSchedule();
    }

    protected void lblR2_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = new LinkButton();
        linkButton = (LinkButton)sender;
        int id = Convert.ToInt32(linkButton.CommandArgument);

        #region Check Conflict

        AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(id);
        if (academicCalenderSection.RoomInfoTwoID > 0)
            AcademicCalenderSectionManager.CheckConflictByRoom(academicCalenderSection, academicCalenderSection.RoomInfoTwoID);

        #endregion

        LoadClassSchedule();
    }

    protected void lblT1_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = new LinkButton();
        linkButton = (LinkButton)sender;
        int id = Convert.ToInt32(linkButton.CommandArgument);

        #region Check Conflict

        AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(id);
        if (academicCalenderSection.TeacherOneID > 0)
            AcademicCalenderSectionManager.CheckConflictByFaculty(academicCalenderSection, academicCalenderSection.TeacherOneID);

        #endregion

        LoadClassSchedule();
    }

    protected void lblT2_Click(object sender, EventArgs e)
    {
        LinkButton linkButton = new LinkButton();
        linkButton = (LinkButton)sender;
        int id = Convert.ToInt32(linkButton.CommandArgument);

        #region Check Conflict

        AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(id);
        if (academicCalenderSection.TeacherTwoID > 0)
            AcademicCalenderSectionManager.CheckConflictByFaculty(academicCalenderSection, academicCalenderSection.TeacherTwoID);

        #endregion

        LoadClassSchedule();
    }
    //protected void btnCopyTo_Click(object sender, EventArgs e)
    //{
    //    int counterFlag = 0;
    //    try
    //    {
    //        if (ucProgram.selectedValue == "0" || ucProgram.selectedValue == "0" || ucCopySession.selectedValue == "0")
    //            lblMsg.Text = "Please Select Semester, Program and Copy To <b><i>Dropdown</i></b>";
    //        else
    //        {
    //            int copyFromSemesterId = Convert.ToInt32(ucSession.selectedValue);
    //            int copyToSemesterId = Convert.ToInt32(ucCopySession.selectedValue);
    //            int programId = Convert.ToInt32(ucProgram.selectedValue);

    //            List<AcademicCalenderSection> academicCalenderSectionList = AcademicCalenderSectionManager.GetAllByAcaCalId(copyToSemesterId);
    //            if (academicCalenderSectionList != null && academicCalenderSectionList.Count > 0)
    //                academicCalenderSectionList = academicCalenderSectionList.Where(x => x.ProgramID == programId).ToList();

    //            if (academicCalenderSectionList != null && academicCalenderSectionList.Count > 0)
    //            {
    //                lblMsg.Text = "<b>Already Copy A Routine For This Semester.</b>";
    //            }
    //            else
    //            {
    //                List<AcademicCalenderSection> acaCalSecList = AcademicCalenderSectionManager.GetAllByAcaCalId(copyFromSemesterId);
    //                if (acaCalSecList != null && acaCalSecList.Count > 0)
    //                {
    //                    acaCalSecList = acaCalSecList.Where(x => x.ProgramID == programId).ToList();
    //                    if (acaCalSecList != null && acaCalSecList.Count >0)
    //                    {
    //                        foreach (AcademicCalenderSection acaCalSec in acaCalSecList)
    //                        {
    //                            acaCalSec.AcademicCalenderID = copyToSemesterId;
    //                            acaCalSec.Occupied = 0;
    //                            acaCalSec.CreatedBy = userObj.Id;
    //                            acaCalSec.CreatedDate = DateTime.Now;
    //                            int resultInsert = AcademicCalenderSectionManager.Insert(acaCalSec);
    //                            if (resultInsert > 0)
    //                                counterFlag++;
    //                        }

    //                        LogGeneralManager.Insert(
    //                                                         DateTime.Now,
    //                                                         "",
    //                                                         ucSession.selectedText,
    //                                                         userObj.LogInID,
    //                                                         "",
    //                                                         "",
    //                                                         "Copy Class Routine",
    //                                                         userObj.LogInID + " copied Class Routine for semester " + ucSession.selectedText,
    //                                                         userObj.LogInID + " is Copied Class Routine",
    //                                                          ((int)CommonEnum.PageName.Admin_ClassRoutine).ToString(),
    //                                                         CommonEnum.PageName.Admin_ClassRoutine.ToString(),
    //                                                         _pageUrl,
    //                                                         "");
    //                    }
    //                    else
    //                        lblMsg.Text = "This Semester and Program has <b>NO</b> routine.";
    //                }
    //                else
    //                    lblMsg.Text = "This Semester has <b>NO</b> routine.";
    //            }
    //        }
    //    }
    //    catch { }
    //    finally
    //    {
    //        if (counterFlag != 0)
    //            lblMsg.Text = "<b>" + counterFlag + " Section Insert For Semester " + ucCopySession.selectedText + "</b>";
    //    }
    //}
    protected void btnAddNew_Click(object sender, EventArgs e)
    {
        if (ucSession.selectedValue == "0" || ucProgram.selectedValue == "0")
        {
            lblMsg.Text = "Please Select Batch and Program.";
            return;
        }

        else if (flagCourseOffer == "NotOffer")
        {
            lblMsg.Text = "Course is not Offered.";
            return;
        }
        chkIsConflict.Checked = false;
        lblConflictMessage.Text = "";
        lblMsg.Text = "";
        btnUpdateInsert.Text = "New Add";
        //ucBatch.SelectedIndex0();
        lblShareBatch.Text = string.Empty;
        LoadComboBox(Convert.ToInt32(ucProgram.selectedValue), 0);
        SessionManager.DeletFromSession(Session_BatchList);

        ddlCourse.Enabled = true;
        ModalPopupExtender1.Show();
    }

    protected void btnAddAndNext_Click(object sender, EventArgs e)
    {
        int id = 0;
        bool result = false;
        if (btnUpdateInsert.CommandArgument != "")
            id = Convert.ToInt32(btnUpdateInsert.CommandArgument);

        if (ddlCourse.SelectedValue == "0")
        {
            lblMsg.Text = "Please select course.";
            lblMsg.Focus();
            return;
        }
        if (string.IsNullOrEmpty(txtSection.Text))
        {
            lblMsg.Text = "Please insert section name.";
            lblMsg.Focus();
            return;
        }

        if (btnUpdateInsert.Text == "Update")
        {
            btnUpdateInsert.CommandArgument = "";
            result = Update(id);
        }
        else if (btnUpdateInsert.Text == "New Add")
            result = Insert();

        if (result)
        {
            DefaultField();

            ModalPopupExtender1.Show();
        }
    }

    private void DefaultField()
    {
        ddlCourse.SelectedIndex = 0;
        txtSection.Text = "";
        txtSectionDefination.Text = "";
        txtCapacity.Text = "";
        ddlSectionGender.SelectedIndex = 0;

        ddlRoomInfo1.SelectedIndex = 0;
        ddlDay1.SelectedIndex = 0;
        ddlTimeSlot1.SelectedIndex = 0;
        ddlFaculty1.SelectedIndex = 0;

        ddlRoomInfo2.SelectedIndex = 0;
        ddlDay2.SelectedIndex = 0;
        ddlTimeSlot2.SelectedIndex = 0;
        ddlFaculty2.SelectedIndex = 0;

        ddlRoomInfo3.SelectedIndex = 0;
        ddlDay3.SelectedIndex = 0;
        ddlTimeSlot3.SelectedIndex = 0;
        ddlFaculty3.SelectedIndex = 0;

        ddlExamTemplate.SelectedIndex = 0;
        ucBatch.SelectedIndex0();
        lblShareBatch.Text = string.Empty;
    }

    protected void btnBatchAdd_Click(object sender, EventArgs e)
    {
        batchList = SessionManager.GetListFromSession<Batch>(Session_BatchList);

        if (batchList == null)
            batchList = new List<Batch>();

        if (ucBatch.selectedValue == "-1")
        {
            batchList = null;
            batchList = BatchManager.GetAllByProgram(Convert.ToInt32(ucProgram.selectedValue));

            SessionManager.SaveListToSession<Batch>(batchList, Session_BatchList);
        }
        else
        {
            Batch batch = BatchManager.GetById(Convert.ToInt32(ucBatch.selectedValue));
            if (batch != null)
            {
                if (!batchList.Contains(batch))
                {
                    batchList.Add(batch);
                    SessionManager.SaveListToSession<Batch>(batchList, Session_BatchList);
                }
            }
        }

        ShowBatchList(batchList);
        ModalPopupExtender1.Show();
    }

    private void ShowBatchList(List<Batch> batchList)
    {
        lblShareBatch.Text = string.Empty;
        int f = 0;

        foreach (Batch item in batchList)
        {
            if (f == 0)
            {
                lblShareBatch.Text = item.BatchNO.ToString() + "; ";
                f = 1;
            }
            else
                lblShareBatch.Text += item.BatchNO.ToString() + "; ";
        }
    }

    protected void btnBatchRemove_Click(object sender, EventArgs e)
    {
        batchList = SessionManager.GetListFromSession<Batch>(Session_BatchList);
        if (batchList == null)
            batchList = new List<Batch>();


        if (ucBatch.selectedValue == "-1")
        {
            batchList = new List<Batch>();
            SessionManager.DeletFromSession(Session_BatchList);
        }
        else
        {

            Batch batch = BatchManager.GetById(Convert.ToInt32(ucBatch.selectedValue));
            if (batch != null)
            {
                if (!batchList.Contains(batch))
                {
                    batchList.RemoveAll(b => b.BatchId == batch.BatchId);
                    SessionManager.SaveListToSession<Batch>(batchList, Session_BatchList);
                }
            }
        }
        ShowBatchList(batchList);
        ModalPopupExtender1.Show();
    }

    private void ShowPopUpBoxMessage(string msg)
    {
        lblPopUpMessage.Text = msg;
    }

    protected void btnCheckConflict_Click(object sender, EventArgs e)
    {

    }

    #region Year Semester Exam Load and OnChange

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
        try
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                LoadExamDropdown(programId, yearNo, semesterNo);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    protected void ddlSemesterNo_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        try
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                LoadExamDropdown(programId, yearNo, semesterNo);
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void LoadExamDropdown(int programId, int yearNo, int semesterNo)
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

    protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
    {
        lblMsg.Text = string.Empty;
        try
        {

        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Department Wise Faculty Load

    protected void DepartmentUserControl1_DepartmentSelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();

        int DeptId = Convert.ToInt32(DepartmentUserControl1.selectedValue);
        LoadFacultyOneByDeptId(DeptId);
    }

    private void LoadFacultyOneByDeptId(int DeptId)
    {
        ddlFaculty1.Items.Clear();
        ddlFaculty1.Items.Add(new ListItem("-Select-", "0"));

        List<Employee> employeeList = EmployeeManager.GetAll();
        if (DeptId != 0)
            employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
        if (employeeList.Count > 0)
        {
            employeeList = employeeList.OrderBy(o => o.Code).ToList();

            foreach (Employee employee in employeeList)
            {
                ddlFaculty1.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            }
        }
    }

    protected void DepartmentUserControl2_DepartmentSelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();
        int DeptId = Convert.ToInt32(DepartmentUserControl2.selectedValue);
        LoadFacultyTwoByDeptId(DeptId);
    }

    private void LoadFacultyTwoByDeptId(int DeptId)
    {
        ddlFaculty2.Items.Clear();
        ddlFaculty2.Items.Add(new ListItem("-Select-", "0"));

        List<Employee> employeeList = EmployeeManager.GetAll();
        if (DeptId != 0)
            employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
        if (employeeList.Count > 0)
        {
            employeeList = employeeList.OrderBy(o => o.Code).ToList();

            foreach (Employee employee in employeeList)
            {
                ddlFaculty2.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            }
        }
    }

    protected void DepartmentUserControl3_DepartmentSelectedIndexChanged(object sender, EventArgs e)
    {
        ModalPopupExtender1.Show();

        int DeptId = Convert.ToInt32(DepartmentUserControl3.selectedValue);
        LoadFacultyThreeByDeptId(DeptId);
    }

    private void LoadFacultyThreeByDeptId(int DeptId)
    {
        ddlFaculty3.Items.Clear();
        ddlFaculty3.Items.Add(new ListItem("-Select-", "0"));

        List<Employee> employeeList = EmployeeManager.GetAll();
        if (DeptId != 0)
            employeeList = employeeList.Where(x => x.DeptID == DeptId).ToList();
        if (employeeList.Count > 0)
        {
            employeeList = employeeList.OrderBy(o => o.Code).ToList();

            foreach (Employee employee in employeeList)
            {
                ddlFaculty3.Items.Add(new ListItem(employee.Code + "   (" + employee.BasicInfo.FullName + ")", employee.EmployeeID.ToString()));
            }
        }
    }

    #endregion
}