using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.IO;
using LogicLayer.BusinessLogic;
using System.Data;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace EMS.Module.result
{
    public partial class ResultHistoryUpload : BasePage
    {
        FileConversion aFileConverterObj = new FileConversion();
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYearDDL(programId);
                LoadYearNoDDL();
                LoadSemesterNoDDL();

                ddlExam.Items.Clear();
                ddlExam.Items.Add(new ListItem("-Select Exam-", "0"));
                ddlExam.AppendDataBoundItems = true;

                lblMsg.Text = null;
            }
        }

        protected void ucDepartment_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = null;
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                //ucBatch.LoadDropDownList(programId);
                LoadYearDDL(programId);
                LoadYearSemesterDDL(0);
                LoadCourseDDL(programId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = null;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadCourseDDL(programId);
                LoadYearDDL(programId);
                if (programId == -1)
                {
                    return;
                }
                //LoadAdmissionSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadYearDDL(int programId)
        {
            List<Year> yearList = new List<Year>();
            yearList = YearManager.GetByProgramId(programId);

            ddlYear.Items.Clear();
            ddlYear.AppendDataBoundItems = true;

            if (yearList != null)
            {
                ddlYear.Items.Add(new ListItem("-Select-", "0"));
                ddlYear.DataTextField = "YearName";
                ddlYear.DataValueField = "YearId";
                if (yearList != null)
                {
                    ddlYear.DataSource = yearList.OrderBy(b => b.YearId).ToList();
                    ddlYear.DataBind();
                }
            }
        }

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
            //else 
            //{
            //    lblMsg.Text = "Please select program, year no and semester no.";
            //}
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

        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            int yearId = Convert.ToInt32(ddlYear.SelectedValue);
            LoadYearSemesterDDL(yearId);
        }

        private void LoadYearSemesterDDL(int yearId)
        {
            List<Semester> semesterList = new List<Semester>();
            semesterList = SemesterManager.GetByYearId(yearId);

            ddlSemester.Items.Clear();
            ddlSemester.AppendDataBoundItems = true;

            if (semesterList != null)
            {
                ddlSemester.Items.Add(new ListItem("-Select-", "0"));
                ddlSemester.DataTextField = "SemesterName";
                ddlSemester.DataValueField = "SemesterId";
                if (semesterList != null)
                {
                    ddlSemester.DataSource = semesterList.OrderBy(b => b.SemesterId).ToList();
                    ddlSemester.DataBind();
                }
            }
        }

        private void LoadCourseDDL(int programId)
        {
            List<Course> yearList = new List<Course>();
            yearList = CourseManager.GetAllByProgram(programId);

            ddlCourse.Items.Clear();
            ddlCourse.AppendDataBoundItems = true;

            if (yearList != null)
            {
                ddlCourse.Items.Add(new ListItem("-Select-", "0"));
                ddlCourse.DataTextField = "CourseFullInfo";
                ddlCourse.DataValueField = "CoureIdVersionId";
                if (yearList != null)
                {
                    ddlCourse.DataSource = yearList.OrderBy(b => b.FormalCode).ToList();
                    ddlCourse.DataBind();
                }
            }
        }

        protected void LoadSheet_Click(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            try
            {
                if (UploadPanel.HasFile)
                {
                    try
                    {
                        string FileName = Path.GetFileName(UploadPanel.FileName);

                        if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                        {
                            string UploadFileLocation = string.Empty;
                            UploadFileLocation = Server.MapPath("~/Upload/");
                            try
                            {
                                string customFileName = GetCustomFileName();
                                string savePath = UploadFileLocation + customFileName;
                                UploadPanel.SaveAs(savePath);
                                FileNameHiddenField.Value = customFileName;
                                FilePathHiddenField.Value = savePath;
                                LoadSheetNames();
                            }
                            catch (Exception ex)
                            {

                            }
                        }
                        else
                        {

                        }

                        if (!string.IsNullOrEmpty(FileName))
                        {
                            lblFileName.Text = Convert.ToString(FileName);
                        }
                        //UploadPanel.Enabled = FileName;
                    }
                    catch (Exception ex)
                    {

                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GetCustomFileName()
        {
            string examName = "StudentResultInfo";
            string date = DateTime.Now.Day.ToString() + "_" +
                          DateTime.Now.Year.ToString() + "_" +
                          DateTime.Now.Month.ToString();
            return examName + "_" + date + ".xlsx";
        }

        private void LoadSheetNames()
        {
            try
            {
                string newPath = Convert.ToString(FilePathHiddenField.Value);
                string fileName = Convert.ToString(FileNameHiddenField.Value);
                //List<SheetName> sheetList = aFileConverterObj.ListSheetInExcel(newPath);
                List<LogicLayer.BusinessLogic.SheetName> sheetList = aFileConverterObj.PassFileName(newPath);
                ddlSheetName.DataSource = sheetList;
                ddlSheetName.DataTextField = "Name";
                ddlSheetName.DataValueField = "Id";
                ddlSheetName.DataBind();
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlSheetName_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            try
            {
                string newPath = Convert.ToString(FilePathHiddenField.Value);
                string fileName = Convert.ToString(FileNameHiddenField.Value);
                string sheetId = Convert.ToString(ddlSheetName.SelectedValue);

                decimal grandTotalAmount = 0;
                DataTable dt = aFileConverterObj.ReadExcelFileDOM(newPath, fileName, sheetId);
                DataRow row = dt.Rows[0];

                var arrayNames = (from DataColumn x in dt.Columns
                                  select x.ColumnName).ToArray();
                int coulmnCounter = arrayNames.Count();

                for (int i = 1; i <= coulmnCounter; i++)
                {
                    dt.Columns[i - 1].ColumnName = Convert.ToString(dt.Rows[0][i - 1]);
                }

                dt.Rows.Remove(row);
                gvStudentResultInfo.DataSource = dt;
                gvStudentResultInfo.DataBind();
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnSaveToServer_Click(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int acacalId = Convert.ToInt32(ucSession.selectedValue);
                int batchId = 0; // Convert.ToInt32(ucBatch.selectedValue);
                int yearId = Convert.ToInt32(ddlYear.SelectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                int semesterId = Convert.ToInt32(ddlSemester.SelectedValue);
                string courseIdVersionId = Convert.ToString(ddlCourse.SelectedValue);
                string[] courseVersion = courseIdVersionId.Split('_');
                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);

                List<GradeDetails> gradeList = GradeDetailsManager.GetAll();
                int selectedRow = 0;
                int insertedRow = 0;
                int updatedRow = 0;
                if (programId > 0 && yearId > 0 && semesterId > 0 && yearNo > 0 && semesterNo > 0 && courseId > 0 && examId > 0)
                {
                    for (int i = 0; i < gvStudentResultInfo.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentResultInfo.Rows[i];
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                        if (ckBox.Checked)
                        {
                            selectedRow = selectedRow + 1;
                            string studentRoll = Convert.ToString(row.Cells[1].Text.Trim());
                            string courseCode = Convert.ToString(row.Cells[2].Text.Trim());
                            string courseTitle = Convert.ToString(row.Cells[3].Text.Trim());
                            string grade = Convert.ToString(row.Cells[4].Text.Trim());
                            string gradePoint = Convert.ToString(row.Cells[5].Text.Trim());

                            LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetByRoll(studentRoll.Trim());
                            if (studentObj != null)
                            {
                                GradeDetails gradeObj = gradeList.Where(x => x.Grade.ToLower() == grade.ToLower()).FirstOrDefault();
                                bool IsCourseExist = IsStudentCourseHistoryExist(studentObj.StudentID, acacalId, yearNo, semesterNo, examId, courseId, versionId);
                                if (IsCourseExist == false)
                                {
                                    int result = studentCourseHistoryInsert(studentObj, gradeObj, acacalId, courseId, versionId, yearNo, yearId, semesterNo, semesterId, examId, grade, gradePoint);
                                    if (result > 0)
                                    {
                                        insertedRow = insertedRow + 1;
                                    }
                                }
                                else 
                                {
                                    //bool updateResult = studentCourseHistoryUpdate(studentObj, gradeObj, acacalId, courseId, versionId, yearNo, yearId, semesterNo, semesterId, examId, grade, gradePoint);
                                    //if (updateResult)
                                    //{
                                    //    updatedRow = updatedRow + 1;
                                    //}
                                }
                            }
                            else 
                            {
                                //int personId = personInsert(null, null, studentRoll);

                                //if (personId > 0)
                                //{
                                //    int studentId = studentInsert(studentRoll, personId, programId, batchId, acacalId, 0);

                                //    if (studentId > 0)
                                //    {
                                //        int studentAdditionalInfoId = studentAdditionalInfoInsert(studentId, Convert.ToInt32(ddlYear.SelectedValue), Convert.ToInt32(ddlSemester.SelectedValue), hallName, regNo);
                                //    }
                                //}

                                lblMsg.Text = "Student not found in system Student Id : " + studentRoll + ".";
                                //return;
                            }
                        }
                    }

                    if (insertedRow > 0)
                    {
                        lblMsg.Text = "Course inserted for student : " + selectedRow + " and inserted : " + insertedRow;
                    }
                    else { lblMsg.Text = "Course could not insert for selected student : " + selectedRow + " and inserted : " + insertedRow; }
                }
                else
                {
                    lblMsg.Text = "Please select program, course, year no, semester no, exam, year, semester, session properly, and try again.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private bool studentCourseHistoryUpdate(Student studentObj, GradeDetails gradeObj, int acacalId, int courseId, int versionId, int yearNo, int yearId, int semesterNo, int semesterId, int examId, string grade, string gradePoint)
        {
            bool result = false;
            try
            {
                StudentCourseHistory studentCourseHistoryObj = StudentCourseHistoryManager.GetAllByStudentIdYearNoSemesterNoExamId(studentObj.StudentID, yearNo, semesterNo, examId).Where(x => x.CourseID == courseId && x.VersionID == versionId).FirstOrDefault();
                if (studentCourseHistoryObj == null)
                {
                    studentCourseHistoryObj.StudentID = studentObj.StudentID;
                    studentCourseHistoryObj.AcaCalID = 0;
                    studentCourseHistoryObj.CourseID = courseId;
                    studentCourseHistoryObj.VersionID = versionId;
                    studentCourseHistoryObj.YearId = yearId;
                    studentCourseHistoryObj.SemesterId = semesterId;
                    studentCourseHistoryObj.YearNo = yearNo;
                    studentCourseHistoryObj.SemesterNo = semesterNo;
                    studentCourseHistoryObj.ExamId = examId;
                    studentCourseHistoryObj.ObtainedGrade = grade;
                    if (gradeObj != null)
                    {
                        studentCourseHistoryObj.GradeId = gradeObj.GradeId;
                        if (gradeObj.GradeId < 10)
                        {
                            studentCourseHistoryObj.CourseStatusID = 5;
                        }
                        //fail
                        else if (gradeObj.GradeId == 10)
                        {
                            studentCourseHistoryObj.CourseStatusID = 7;
                        }
                        //Incomplete
                        else if (gradeObj.GradeId == 11)
                        {
                            studentCourseHistoryObj.CourseStatusID = 3;
                        }
                        //withdraw
                        else if (gradeObj.GradeId == 12)
                        {
                            studentCourseHistoryObj.CourseStatusID = 2;
                        }
                        //Absent
                        else if (gradeObj.GradeId == 13)
                        {
                            studentCourseHistoryObj.CourseStatusID = 10;
                        }
                    }
                    if (!string.IsNullOrEmpty(gradePoint))
                    {
                        studentCourseHistoryObj.ObtainedGPA = Convert.ToDecimal(gradePoint);
                    }
                    studentCourseHistoryObj.CreatedBy = userObj.Id;
                    studentCourseHistoryObj.CreatedDate = DateTime.Now;
                    studentCourseHistoryObj.ModifiedBy = userObj.Id;
                    studentCourseHistoryObj.ModifiedDate = DateTime.Now;


                    bool isUpdate = StudentCourseHistoryManager.Update(studentCourseHistoryObj);
                    return result;
                }
                else 
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                return result;
            }
        }

        private bool IsStudentCourseHistoryExist(int studentId, int acacalId, int yearNo, int semesterNo, int examId, int courseId, int versionId)
        {
            bool result = false;
            try 
            {
                StudentCourseHistory studentCourseHistoryObj = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(studentId, acacalId).Where(x => x.YearNo == yearNo && x.SemesterNo == semesterNo && x.ExamId == examId && x.CourseID == courseId && x.VersionID == versionId).FirstOrDefault();
                if(studentCourseHistoryObj== null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                return result;
            }
        }

        protected void btnCheckStudentAvilability_Click(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            try
            {
                string studentNotFound = null;
                for (int i = 0; i < gvStudentResultInfo.Rows.Count; i++)
                {
                    GridViewRow row = gvStudentResultInfo.Rows[i];

                    string studentRoll = Convert.ToString(row.Cells[1].Text.Trim());
                    if (studentRoll != null)
                    {
                        LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetByRoll(studentRoll.Trim());
                        if (studentObj != null)
                        {
                            //Course courseObj = CourseManager.
                        }
                        else
                        {
                            studentNotFound = studentNotFound + ", " + studentRoll;
                        }
                    }
                }
                if (studentNotFound != null)
                {
                    lblMsg.Text = "Student not found Student Id : " + studentNotFound + ".";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnCheckCourseAvailability_Click(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int acacalId = Convert.ToInt32(ucSession.selectedValue);
                string courseIdVersionId = Convert.ToString(ddlCourse.SelectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                string courseFound = null;
                if (acacalId > 0 && courseIdVersionId != Convert.ToString(0))
                {
                    string[] courseVersion = courseIdVersionId.Split('_');
                    int courseId = Convert.ToInt32(courseVersion[0]);
                    int versionId = Convert.ToInt32(courseVersion[1]);

                    for (int i = 0; i < gvStudentResultInfo.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentResultInfo.Rows[i];

                        string studentRoll = Convert.ToString(row.Cells[1].Text.Trim());
                        if (studentRoll != null)
                        {
                            LogicLayer.BusinessObjects.Student studentObj = StudentManager.GetByRoll(studentRoll.Trim());
                            if (studentObj != null)
                            {
                                bool IsCourseExist = IsStudentCourseHistoryExist(studentObj.StudentID, acacalId, yearNo, semesterNo, examId, courseId, versionId);
                                if (IsCourseExist)
                                {
                                    courseFound = courseFound + ", " + studentRoll;
                                }
                                else { }
                            }
                        }
                    }

                    if (courseFound != null)
                    {
                        lblMsg.Text = "Student course found for Student Id : " + courseFound + ".";
                    }
                    else
                    {
                        lblMsg.Text = "Selected course not found for any student on selected semester.";
                    }
                }
                else
                {
                    lblMsg.Text = "Please select a session and course to check course exist or not.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private int personInsert(string gender, string mobileno, string name)
        {
            LogicLayer.BusinessObjects.Person personObj = new LogicLayer.BusinessObjects.Person();
            if (gender == "Male")
            {
                personObj.Gender = Convert.ToString((int)CommonUtility.CommonEnum.Gender.Male);
            }
            else if (gender == "Female")
            {
                personObj.Gender = Convert.ToString((int)CommonUtility.CommonEnum.Gender.Female);
            }
            personObj.Phone = mobileno;
            personObj.FullName = Convert.ToString(name);
            personObj.CreatedBy = userObj.Id;
            personObj.CreatedDate = DateTime.Now;
            personObj.ModifiedBy = userObj.Id;
            personObj.ModifiedDate = DateTime.Now;

            int personId = PersonManager.Insert(personObj);

            return personId;
        }

        private int studentInsert(string studentRoll, int personId, int programId, int batchId, int acacalId, int activeAcacalId)
        {
            LogicLayer.BusinessObjects.Student studentObj = new LogicLayer.BusinessObjects.Student();

            studentObj.Roll = Convert.ToString(studentRoll);
            studentObj.PersonID = personId;
            studentObj.ProgramID = programId;
            studentObj.BatchId = batchId;
            studentObj.StudentAdmissionAcaCalId = acacalId;
            studentObj.ActiveSession = activeAcacalId;
            studentObj.CreatedBy = userObj.Id;
            studentObj.CreatedDate = DateTime.Now;
            studentObj.ModifiedBy = userObj.Id;
            studentObj.ModifiedDate = DateTime.Now;

            int studentId = StudentManager.Insert(studentObj);

            return studentId;
        }

        private int studentAdditionalInfoInsert(int studentId, int yearId, int semesterId, string hallName, string regNo)
        {
            LogicLayer.BusinessObjects.StudentAdditionalInfo studentAddiotionalInfoObj = new LogicLayer.BusinessObjects.StudentAdditionalInfo();

            studentAddiotionalInfoObj.StudentId = studentId;
            studentAddiotionalInfoObj.YearId = yearId;
            studentAddiotionalInfoObj.SemesterId = semesterId;
            studentAddiotionalInfoObj.RegistrationNo = regNo;
            studentAddiotionalInfoObj.Attribute1 = hallName;
            studentAddiotionalInfoObj.CreatedBy = userObj.Id;
            studentAddiotionalInfoObj.CreatedDate = DateTime.Now;
            studentAddiotionalInfoObj.ModifiedBy = userObj.Id;
            studentAddiotionalInfoObj.ModifiedDate = DateTime.Now;

            int studentAdditionalInfoId = StudentAdditionalInfoManager.Insert(studentAddiotionalInfoObj);

            return studentAdditionalInfoId;
        }

        private int studentCourseHistoryInsert(Student studentObj, GradeDetails gradeObj, int acacalId, int courseId, int versionId, int yearNo, int yearId, int semesterNo, int semesterId, int examId, string grade, string gradePoint) 
        {
            int result = 0;
            try
            {
                StudentCourseHistory studentCourseHistoryObj = new StudentCourseHistory();
                studentCourseHistoryObj.StudentID = studentObj.StudentID;
                studentCourseHistoryObj.AcaCalID = 0;
                studentCourseHistoryObj.CourseID = courseId;
                studentCourseHistoryObj.VersionID = versionId;
                studentCourseHistoryObj.YearId = yearId;
                studentCourseHistoryObj.SemesterId = semesterId;
                studentCourseHistoryObj.YearNo = yearNo;
                studentCourseHistoryObj.SemesterNo = semesterNo;
                studentCourseHistoryObj.ExamId = examId;
                studentCourseHistoryObj.ObtainedGrade = grade;
                if (gradeObj != null)
                {
                    studentCourseHistoryObj.GradeId = gradeObj.GradeId;
                    if (gradeObj.GradeId < 10)
                    {
                        studentCourseHistoryObj.CourseStatusID = 5;
                    }
                        //fail
                    else if (gradeObj.GradeId == 10)
                    {
                        studentCourseHistoryObj.CourseStatusID = 7;
                    }
                        //Incomplete
                    else if (gradeObj.GradeId == 11)
                    {
                        studentCourseHistoryObj.CourseStatusID = 3;
                    }
                        //withdraw
                    else if (gradeObj.GradeId == 12)
                    {
                        studentCourseHistoryObj.CourseStatusID = 2;
                    }
                        //Absent
                    else if (gradeObj.GradeId == 13)
                    {
                        studentCourseHistoryObj.CourseStatusID = 10;
                    }
                }
                if (!string.IsNullOrEmpty(gradePoint))
                {
                    studentCourseHistoryObj.ObtainedGPA = Convert.ToDecimal(gradePoint);
                }
                studentCourseHistoryObj.CreatedBy = userObj.Id;
                studentCourseHistoryObj.CreatedDate = DateTime.Now;
                studentCourseHistoryObj.ModifiedBy = userObj.Id;
                studentCourseHistoryObj.ModifiedDate = DateTime.Now;


                result = StudentCourseHistoryManager.Insert(studentCourseHistoryObj);
                return result;
                
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
                return result;
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            lblMsg.Text = null;
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

                foreach (GridViewRow row in gvStudentResultInfo.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void btnDeleteData_Click(object sender, EventArgs e)
        {
            lblMsg.Text = null;
            try 
            { 
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int acacalId = Convert.ToInt32(ucSession.selectedValue);
                int yearId = Convert.ToInt32(ddlYear.SelectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                int semesterId = Convert.ToInt32(ddlSemester.SelectedValue);

                string courseIdVersionId = Convert.ToString(ddlCourse.SelectedValue);
                string[] courseVersion = courseIdVersionId.Split('_');
                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);

                if (programId > 0 && yearId > 0 && semesterId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    bool result = StudentCourseHistoryManager.DeleteCourseByProgramYearSemesterExamId(programId, yearId, semesterId, yearNo, semesterNo, examId, courseId, versionId);
                    if (result)
                    {
                        lblMsg.Text = "Result deleted successfully.";
                    }
                    else 
                    {
                        lblMsg.Text = "Result could not deleted successfully.";
                    }
                }
                else 
                {
                    lblMsg.Text = "Please select program, course, year no, semester no, exam, year, semester, session properly, and try again.";
                }
            }
            catch(Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

 
    }
}