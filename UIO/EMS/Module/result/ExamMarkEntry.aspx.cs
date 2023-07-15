using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using Microsoft.Reporting.WebForms.Internal.Soap.ReportingServices2005.Execution;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace EMS.Module.result
{
    public partial class ExamMarkEntry : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        int userId = 0;
        int employeeId = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            #region N/A
            ////this.Page.Form.Enctype = "multipart/form-data";

            //base.CheckPage_Load();
            //userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            //userId = userObj.Id;

            //UserInPerson userInPerson = UserInPersonManager.GetById(userId);
            //if (userInPerson != null)
            //{
            //    Employee emp = EmployeeManager.GetByPersonId(userInPerson.PersonID);
            //    if (emp != null)
            //    {
            //        employeeId = emp.EmployeeID;
            //    }
            //}

            //if (!IsPostBack)
            //{
            //    LoadGridView(employeeId);
            //} 
            #endregion

            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            userId = userObj.Id;

            UserInPerson userInPerson = UserInPersonManager.GetById(userId);
            if (userInPerson != null)
            {
                Employee emp = EmployeeManager.GetByPersonId(userInPerson.PersonID);
                if (emp != null)
                {
                    employeeId = emp.EmployeeID;
                }
            }

            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;

                if (userObj.RoleID != 1 && userObj.RoleID != 2)
                {
                    UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userObj.Id);
                    if (uapObj != null)
                    {
                        ucDepartment.LoadDropdownWithUserAccess(userObj.Id);
                        ucProgram.LoadDropdownWithUserAccess(userObj.Id);
                        ucProgram.SelectedIndex(1);
                        ucDepartment.SelectedIndex(1);
                    }
                }
                else
                {
                    ucDepartment.LoadDropDownList();
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    ucProgram.LoadDropdownByDepartmentId(departmentId);
                }
                LoadYearNoDDL();
                LoadSemesterNoDDL();

                //LoadGridView(employeeId, 0);
            }
        }

        protected void MessageView(string msg, string status)
        {

            if (status == "success")
            {
                lblMsg.Text = string.Empty;
                lblMsg.Text = msg.ToString();
                lblMsg.Attributes.CssStyle.Add("font-weight", "bold");
                lblMsg.Attributes.CssStyle.Add("color", "green");

            }
            else if (status == "fail")
            {
                lblMsg.Text = string.Empty;
                lblMsg.Text = msg.ToString();
                lblMsg.Attributes.CssStyle.Add("font-weight", "bold");
                lblMsg.Attributes.CssStyle.Add("color", "crimson");

            }
            else if (status == "clear")
            {
                lblMsg.Text = string.Empty;
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

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
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

        protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                btnLoad_Click(null, null);
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

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;


                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    LoadGridView(employeeId, examId);
                }
                else
                {
                    lblMsg.Text = "Please select Program, Year, Semester and Exam.";
                }

            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }











        private void LoadGridView(int employeeId, int examSetupDetailId)
        {
            try
            {

                if (employeeId > 0)
                {
                    #region First Examiner
                    List<ExamMarkEntryDTO> firstExaminerList = ExamMarkFirstSecondThirdExaminerManager.GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(employeeId, null, null, examSetupDetailId);

                    if (firstExaminerList != null && firstExaminerList.Count > 0)
                    {
                        gvExamMarkEntryFirstExaminer.DataSource = firstExaminerList;
                        gvExamMarkEntryFirstExaminer.DataBind();
                    }
                    else
                    {
                        gvExamMarkEntryFirstExaminer.DataSource = null;
                        gvExamMarkEntryFirstExaminer.DataBind();
                    }
                    #endregion

                    #region Second Examiner
                    List<ExamMarkEntryDTO> secondExaminerList = ExamMarkFirstSecondThirdExaminerManager.GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(null, employeeId, null, examSetupDetailId);

                    if (secondExaminerList != null && secondExaminerList.Count > 0)
                    {
                        gvExamMarkEntrySecondExaminer.DataSource = secondExaminerList;
                        gvExamMarkEntrySecondExaminer.DataBind();
                    }
                    else
                    {
                        gvExamMarkEntrySecondExaminer.DataSource = null;
                        gvExamMarkEntrySecondExaminer.DataBind();
                    }
                    #endregion

                    #region Third Examiner
                    List<ExamMarkEntryDTO> thirdExaminerFinalList = new List<ExamMarkEntryDTO>();
                    List<ExamMarkEntryDTO> thirdExaminerList = ExamMarkFirstSecondThirdExaminerManager.GetExamTemplateItemCourseSectionExamByFirstSecondThirdExaminerId(null, null, employeeId, examSetupDetailId);

                    if (thirdExaminerList != null && thirdExaminerList.Count > 0)
                    {
                        foreach (var tData in thirdExaminerList)
                        {
                            List<ExamMarkFirstSecondThirdExaminer> listByAcaCalSectionId = ExamMarkFirstSecondThirdExaminerManager.GetAllByAcaCalSectionId(tData.AcaCalSectionID);
                            if (listByAcaCalSectionId != null && listByAcaCalSectionId.Count > 0)
                            {
                                listByAcaCalSectionId = listByAcaCalSectionId.Where(x => x.ThirdExaminerStatus == 1).ToList();

                                if (listByAcaCalSectionId != null && listByAcaCalSectionId.Count > 0)
                                {
                                    thirdExaminerFinalList.Add(tData);
                                }
                            }
                        }

                        if (thirdExaminerFinalList != null && thirdExaminerFinalList.Count > 0)
                        {
                            gvExamMarkEntryThirdExaminer.DataSource = thirdExaminerList;
                            gvExamMarkEntryThirdExaminer.DataBind();
                        }
                        else
                        {
                            gvExamMarkEntryThirdExaminer.DataSource = null;
                            gvExamMarkEntryThirdExaminer.DataBind();
                        }

                    }
                    else
                    {
                        gvExamMarkEntryThirdExaminer.DataSource = null;
                        gvExamMarkEntryThirdExaminer.DataBind();
                    }
                    #endregion
                }


            }
            catch (Exception ex)
            {

            }

        }

        public void DownloadFile(FileInfo file)
        {
            try
            {
                if (file != null && file.Name.Length > 0)
                {
                    Response.Clear();
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + file.Name);
                    //Response.AddHeader("Content-Length", file.Length.ToString());
                    Response.ContentType = "application/octet-stream";
                    Response.WriteFile(file.FullName);
                    //Response.End();
                    Response.Flush();
                }
                else
                {
                    MessageView("Failed to Download Excel !!", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Failed to Download Excel; Error: " + ex.Message.ToString(), "fail");
            }
        }

        private void InsertOrUpdateExamQuestionMark(StudentsCourseMarks scmModel, int ExaminerTypeId)
        {
            try
            {
                if (scmModel != null)
                {
                    string studentRoll = scmModel.Roll;
                    Student studentModel = StudentManager.GetByRoll(studentRoll);
                    int studentId = studentModel != null ? studentModel.StudentID : 0;
                    int courseHistoryId = scmModel.StudentCourseHistoryId;
                    int examTemplateItemId = scmModel.ExamTemplateTypeId;

                    if (scmModel.StudentMarks != null && scmModel.StudentMarks.Count > 0 && studentId > 0 && courseHistoryId > 0 && examTemplateItemId > 0)
                    {
                        foreach (var tData in scmModel.StudentMarks)
                        {
                            ExamMarkQuestion emq = ExamMarkQuestionManager.GetByStudentIdCourseHistoryId(studentId, courseHistoryId, tData.QuestionNumber);

                            if (emq != null)
                            {
                                // Update

                                emq.ExamTemplateItemId = examTemplateItemId;
                                if (ExaminerTypeId == 1)
                                {
                                    emq.FirstExaminerMark = tData.ObtainedMarks;
                                }
                                else if (ExaminerTypeId == 2)
                                {
                                    emq.SecondExaminerMark = tData.ObtainedMarks;
                                }
                                else if (ExaminerTypeId == 3)
                                {
                                    emq.ThirdExaminerMark = tData.ObtainedMarks;
                                }
                                else
                                { }

                                emq.ModifiedBy = userObj.Id;
                                emq.ModifiedDate = DateTime.Now;

                                bool isUpdate = ExamMarkQuestionManager.Update(emq);

                                if (isUpdate == true)
                                {

                                }
                            }
                            else
                            {
                                // Insert

                                ExamMarkQuestion model = new ExamMarkQuestion();

                                model.StudentId = studentId;
                                model.CourseHistoryId = courseHistoryId;
                                model.QuestionNo = tData.QuestionNumber;
                                if (ExaminerTypeId == 1)
                                {
                                    model.FirstExaminerMark = tData.ObtainedMarks;
                                }
                                else if (ExaminerTypeId == 2)
                                {
                                    model.SecondExaminerMark = tData.ObtainedMarks;
                                }
                                else if (ExaminerTypeId == 3)
                                {
                                    model.ThirdExaminerMark = tData.ObtainedMarks;
                                }
                                else
                                { }
                                model.ExamTemplateItemId = examTemplateItemId;
                                model.CreateBy = userObj.Id;
                                model.CreatedDate = DateTime.Now;


                                int examMarkQuestionId = ExamMarkQuestionManager.Insert(model);

                                if (examMarkQuestionId > 0)
                                {
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageView("Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
        }

        private bool CheckStudentHasTwoExamMarkInContinuousAssessmentExam(int CourseHistoryId, int? ColumnTypeId, int? MultipleExaminerId, int? ExamStatusId)
        {
            bool result = false;
            try
            {
                if (CourseHistoryId > 0)
                {
                    List<ExamMarkDetails> list = ExamMarkDetailsManager.GetAllByCourseHistoryIdColumnTypeIdMultipleExaminerIdExamStatusId(CourseHistoryId, ColumnTypeId, MultipleExaminerId, ExamStatusId);

                    if (list != null && list.Count >= 2)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
            }

            return result;
        }

        #region First Examiner
        protected void btnDownloadFirstExaminer_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            int flagExcept = 0;
            string fileName = string.Empty;
            FileInfo newFile = null;

            try
            {
                bool excelLoopLimit = false;

                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                //if (programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)


                Button btn = (Button)sender;

                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                int examSetupId = int.Parse(commandArgs[1]);
                int examTemplateItemId = int.Parse(commandArgs[2]);

                List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();

                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0 &&
                    programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                    Program program = ProgramManager.GetById(academicCalenderSection.ProgramID);
                    AcademicCalender acaCal = AcademicCalenderManager.GetById(academicCalenderSection.AcademicCalenderID);
                    Course course = CourseManager.GetByCourseIdVersionId(academicCalenderSection.CourseID, academicCalenderSection.VersionID);
                    Employee emp1 = EmployeeManager.GetById(academicCalenderSection.TeacherOneID);

                    fileName = course.VersionCode.Replace(' ', '_') + "_" + academicCalenderSection.SectionName.Replace(' ', '_') + "_FE";

                    AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);

                    if (acs != null)
                    {
                        List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);


                        etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                        if (etiList == null || etiList.Count == 0)
                        {
                            MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                            return;
                        }

                        if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                        {
                            excelLoopLimit = true;
                        }

                    }
                    else
                    {
                        MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                        return;
                    }


                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                    List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(examId, acaCalSectionId, employeeId, null, null);

                    if (stdCourseHistoryList != null && stdCourseHistoryList.Count > 0)
                    {
                        List<Student> studentList = new List<Student>();
                        foreach (var tData in stdCourseHistoryList)
                        {
                            // CourseHistoryId
                            // ColumnType = 1 (Nullable)
                            // MultipleExaminer = 0 (Nullable)
                            // ExamStatus = 1 (Nullable)
                            bool checkStudentHasTwoExamMarkInContinuousAssessmentExam = true; // CheckStudentHasTwoExamMarkInContinuousAssessmentExam(tData.ID, 1, 0, 1);

                            if (checkStudentHasTwoExamMarkInContinuousAssessmentExam == true)
                            {
                                Student model = StudentManager.GetById(tData.StudentID);
                                if (model != null)
                                {
                                    studentList.Add(model);

                                    #region Check Is there any Question Mark Data Available
                                    StudentsCourseMarks scm = new StudentsCourseMarks();

                                    for (int i = 1; i <= 10; i++)
                                    {
                                        ExamMarkQuestion emq = ExamMarkQuestionManager.GetByStudentIdCourseHistoryId(model.StudentID, tData.ID, i);
                                        if (emq != null && emq.FirstExaminerMark != null)
                                        {
                                            Marks marks = new Marks();
                                            marks.ObtainedMarks = Convert.ToDecimal(emq.FirstExaminerMark);
                                            marks.QuestionNumber = Convert.ToInt32(emq.QuestionNo);
                                            scm.StudentMarks.Add(marks);
                                        }
                                    }


                                    if (scm.StudentMarks != null && scm.StudentMarks.Count > 0)
                                    {
                                        scm.StudentId = model.StudentID;
                                        scm.Roll = model.Roll;
                                        scm.StudentCourseHistoryId = tData.ID;

                                        studentsCourseMarksesList.Add(scm);
                                    }
                                    #endregion

                                }
                            }
                        }


                        #region N/A
                        //DataTable table = new DataTable();
                        //DataRow newRow;

                        //table.Columns.Add("SL", typeof(string));
                        //table.Columns.Add("Student Roll", typeof(string));
                        //table.Columns.Add("1", typeof(string));
                        //table.Columns.Add("2", typeof(string));
                        //table.Columns.Add("3", typeof(string));
                        //table.Columns.Add("4", typeof(string));
                        //table.Columns.Add("5", typeof(string));
                        //table.Columns.Add("6", typeof(string));
                        //table.Columns.Add("7", typeof(string));
                        //table.Columns.Add("8", typeof(string));
                        //table.Columns.Add("9", typeof(string));
                        //table.Columns.Add("10", typeof(string));
                        //table.Columns.Add("Total", typeof(string));


                        //if (studentList.Count > 0 && studentList != null)
                        //{

                        //    int serialNo = 1;
                        //    foreach (var tData in studentList)
                        //    {
                        //        object[] rowArray = new object[13];
                        //        rowArray[0] = serialNo++;
                        //        rowArray[1] = tData.Roll;
                        //        rowArray[2] = "0";
                        //        rowArray[3] = "0";
                        //        rowArray[4] = "0";
                        //        rowArray[5] = "0";
                        //        rowArray[6] = "0";
                        //        rowArray[7] = "0";
                        //        rowArray[8] = "0";
                        //        rowArray[9] = "0";
                        //        rowArray[10] = "0";
                        //        rowArray[11] = "0";
                        //        rowArray[12] = "";

                        //        newRow = table.NewRow();
                        //        newRow.ItemArray = rowArray;
                        //        table.Rows.Add(newRow);
                        //    }



                        //    CreateExcel(table, fileName);
                        //} 
                        #endregion


                        if (studentList != null && studentList.Count > 0)
                        {
                            string departmentName = string.Empty;
                            string courseCode = string.Empty;
                            string totalMark = string.Empty;
                            string courseTitle = string.Empty;
                            string teacherName = string.Empty;
                            string teacherDesignation = string.Empty;
                            string examName = string.Empty;
                            string session = string.Empty;

                            #region Get Header Info
                            ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                            if (emgvsiDTO != null)
                            {
                                departmentName = emgvsiDTO.DepartmentName;
                                courseCode = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                                totalMark = emgvsiDTO.ExamMark.ToString();
                                courseTitle = emgvsiDTO.CourseTitle;
                                teacherName = emgvsiDTO.TeacherName;
                                teacherDesignation = emgvsiDTO.TeacherDesignation;
                                examName = emgvsiDTO.ExamName;
                                session = emgvsiDTO.Session;
                            }
                            #endregion


                            #region Excel Create
                            FileInfo template = null;
                            if (excelLoopLimit)
                            {
                                template = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/ExcelFiles/SingleMarkSheet.xlsx"));
                            }
                            else
                            {
                                template = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/ExcelFiles/MarkSheetV2.xlsx"));
                            }

                            string path = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/Grade_Sheet_Files/")).DirectoryName;

                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            newFile = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/Grade_Sheet_Files/" + fileName + ".xlsx"));


                            using (ExcelPackage excelPackage = new ExcelPackage(newFile, template))
                            {
                                ExcelWorkbook myWorkbook = excelPackage.Workbook;

                                ExcelWorksheet gradeSheet = myWorkbook.Worksheets["Final"];

                                //var sheet = excelPackage.Workbook.Worksheets.Add("Validation");

                                //Char cName = 'Women's Movement from Global Perspectives'; //courseTitle.Replace("'", "\'");

                                gradeSheet.Cell(5, 3).Value = departmentName;
                                gradeSheet.Cell(6, 7).Value = courseCode;
                                gradeSheet.Cell(6, 3).Value = totalMark;
                                gradeSheet.Cell(6, 12).Value = courseTitle.Replace("'", "");
                                gradeSheet.Cell(7, 14).Value = acaCalSectionId.ToString();
                                gradeSheet.Cell(3, 6).Value = examName.ToString();
                                gradeSheet.Cell(5, 12).Value = session.ToString();


                                int footerCount = 9;
                                if (stdCourseHistoryList.Count > 0 && stdCourseHistoryList != null)
                                {
                                    int index = 0;
                                    foreach (var temp in studentList)
                                    {
                                        index++;

                                        gradeSheet.Cell(8 + index, 1).Value = index.ToString();
                                        gradeSheet.Cell(8 + index, 2).Value = temp.Roll.ToString().ToLower();

                                        #region If Question Mark is Exist, Assign into Fresh Excel
                                        if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                        {
                                            int startColumn = 3;
                                            int endColumn = 12;
                                            int questionNo = 1;
                                            for (int column = startColumn; column <= endColumn; column++)
                                            {
                                                foreach (var tStudentsCourseMarksesList in studentsCourseMarksesList.Where(x => x.StudentId == temp.StudentID).ToList())
                                                {
                                                    foreach (Marks mark in tStudentsCourseMarksesList.StudentMarks.Where(x => x.QuestionNumber == questionNo).ToList())
                                                    {
                                                        if (mark != null)
                                                        {
                                                            gradeSheet.Cell(8 + index, column).Value = mark.ObtainedMarks.ToString();
                                                        }

                                                        questionNo++;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion
                                        if (excelLoopLimit)
                                        {
                                            gradeSheet.Cell(8 + index, 4).Formula = "=IF(SUM(C" + (8 + index) + ":C" + (8 + index) + ")=0,\"\",SUM(C" + (8 + index) + ":C" + (8 + index) + "))"; //"=SUM(A" + (8 + index) + ",B" + (8 + index) + ")";
                                        }
                                        else
                                        {
                                            gradeSheet.Cell(8 + index, 13).Formula = "=IF(SUM(C" + (8 + index) + ":L" + (8 + index) + ")=0,\"\",SUM(C" + (8 + index) + ":L" + (8 + index) + "))"; //"=SUM(A" + (8 + index) + ",B" + (8 + index) + ")";

                                        }



                                        footerCount++;
                                    }

                                    // where loop is end add 3 number to get the position of after 3 cell Row
                                    footerCount = footerCount + 3;

                                    int dateAndExaminerSignatureRow = footerCount;

                                    int examinerNameRow = footerCount + 1;
                                    int designationRow = footerCount + 2;
                                    int departmentRow = footerCount + 3;

                                    gradeSheet.Cell(dateAndExaminerSignatureRow, 1).Value = "Date :";
                                    gradeSheet.Cell(dateAndExaminerSignatureRow, 6).Value = "Examiner Signature :";

                                    gradeSheet.Cell(examinerNameRow, 6).Value = "Examiner Name :";
                                    gradeSheet.Cell(examinerNameRow, 9).Value = teacherName;

                                    gradeSheet.Cell(designationRow, 6).Value = "Designation :";
                                    gradeSheet.Cell(designationRow, 9).Value = teacherDesignation;

                                    gradeSheet.Cell(departmentRow, 6).Value = "Department :";
                                    gradeSheet.Cell(departmentRow, 9).Value = departmentName;


                                    excelPackage.Save();

                                    #region Log Insert
                                    //LogGeneralManager.Insert(
                                    //        DateTime.Now,
                                    //        BaseAcaCalCurrent.Code,
                                    //        BaseAcaCalCurrent.FullCode,
                                    //        BaseCurrentUserObj.LogInID,
                                    //        "",
                                    //        "",
                                    //        "Grade Sheet Download",
                                    //        "Downaloded grade sheet of " + ddlAcaCalBatch.SelectedItem.Text + " " + ddlProgram.SelectedItem.Text + " " + ddlAcaCalSection.SelectedItem.Text,
                                    //        "normal",
                                    //        _pageId,
                                    //        _pageName,
                                    //        _pageUrl,
                                    //        "");

                                    #endregion
                                }
                                else
                                {
                                    flagExcept = 1;
                                    MessageView("No Student Found !!", "fail");
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            flagExcept = 1;
                            MessageView("Every student needs to have at least 2 marks on continuous exam to download Final Exam Student List !!", "fail");
                        }


                    }
                    else
                    {
                        flagExcept = 1;
                        MessageView("No Student Found !!", "fail");
                    }

                }
                else
                {
                    MessageView("Please select Program, Year, Semester and Exam.", "fail");
                }
            }
            catch (Exception ex)
            {
                flagExcept = 1;
                MessageView("Exception: Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
            finally
            {
                if (flagExcept == 0)
                {
                    DownloadFile(newFile);
                }
            }

        }

        protected void btnUploadFirstExaminer_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtenderFirstExaminerExcelUpload.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examTemplateItemId = int.Parse(commandArgs[2]);


            if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0)
            {
                hfFirstExaminerExamMarkUploadAcaCalSectionId.Value = acaCalSectionId.ToString();
                hfFirstExaminerExamMarkUploadExamTemplateItemId.Value = examTemplateItemId.ToString();
            }
            else
            {
                hfFirstExaminerExamMarkUploadAcaCalSectionId.Value = "0";
                hfFirstExaminerExamMarkUploadExamTemplateItemId.Value = "0";
            }
        }

        protected void FirstExaminerExamMarkUpload_Click(object sender, EventArgs e)
        {

            MessageView("", "clear");

            try
            {
                int excelLoopLimit = 11;
                int excelLoopLimitsingle = 12;

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (examId > 0)
                {

                    if (FileUploadExamMarkFirstExaminer.HasFile)
                    {

                        if ((!string.IsNullOrEmpty(hfFirstExaminerExamMarkUploadAcaCalSectionId.Value) && Convert.ToInt32(hfFirstExaminerExamMarkUploadAcaCalSectionId.Value) > 0))
                        {
                            int acaCalSectionId = Convert.ToInt32(hfFirstExaminerExamMarkUploadAcaCalSectionId.Value);

                            string FileName = Path.GetFileName(FileUploadExamMarkFirstExaminer.FileName);

                            if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                            {

                                //string path = Server.MapPath("~/Module/result/Grade_Sheet_Files/");

                                #region N/A -- Delete All Existing File
                                //System.IO.DirectoryInfo di = new DirectoryInfo(path);

                                //foreach (FileInfo file in di.GetFiles())
                                //{
                                //    try
                                //    {

                                //        file.Delete();

                                //    }
                                //    catch (Exception ex)
                                //    {

                                //    }
                                //}
                                #endregion

                                #region Get StudentCourseHistory

                                //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                                List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(examId, acaCalSectionId, employeeId, null, null);


                                if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                                {
                                    MessageView("No student is found agaist this course or section !!", "fail");
                                    return;
                                }
                                #endregion

                                #region Get ExamTemplateItemID and TemplateExamMark
                                int examTemplateItemId = 0;
                                decimal examTemplateItemExamMark = 0.00M;
                                AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);




                                if (acs != null)
                                {
                                    List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);


                                    etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                                    if (etiList == null || etiList.Count == 0)
                                    {
                                        MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                                        return;
                                    }

                                    if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                                    {
                                        excelLoopLimit = 2;
                                        excelLoopLimitsingle = 3;
                                    }

                                    examTemplateItemId = etiList.Where(x => x.MultipleExaminer == 1).FirstOrDefault().ExamTemplateItemId;
                                    examTemplateItemExamMark = etiList.Where(x => x.MultipleExaminer == 1).FirstOrDefault().ExamMark;
                                }
                                else
                                {
                                    MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                                    return;
                                }
                                #endregion

                                #region Get Data from Excel



                                int breakpoint = 0;

                                List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();

                                DataTable DtTable = new DataTable();
                                string path = Path.GetFileName(FileUploadExamMarkFirstExaminer.FileName);
                                path = path.Replace(" ", "");

                                String ExcelPath = Server.MapPath("~/Module/result/Grade_Sheet_Files/") + path;

                                try
                                {
                                    if (File.Exists(ExcelPath))
                                    {
                                        System.IO.File.Delete(ExcelPath);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                try
                                {
                                    FileUploadExamMarkFirstExaminer.SaveAs(Server.MapPath("~/Module/result/Grade_Sheet_Files/") + path);

                                    OleDbConnection mycon = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + ExcelPath + "; Extended Properties=Excel 8.0; Persist Security Info = False");
                                    mycon.Open();
                                    OleDbCommand cmd = new OleDbCommand("select * from [Final$]", mycon);
                                    OleDbDataAdapter da = new OleDbDataAdapter();
                                    da.SelectCommand = cmd;
                                    DataSet ds = new DataSet();
                                    da.Fill(ds);
                                    DtTable = ds.Tables[0];
                                    //GridView1.DataSource = ds.Tables[0];
                                    //GridView1.DataBind();
                                    mycon.Close();

                                    if (DtTable.Rows.Count > 0)
                                    {

                                        DataRow rowT = DtTable.Rows[5];

                                        if (rowT[13].ToString() != "" && Convert.ToInt16(rowT[13].ToString()) == acaCalSectionId)
                                        {

                                            int rowNumber = 0;
                                            #region GetDataFromDataTable
                                            foreach (DataRow row in DtTable.Rows)
                                            {
                                                rowNumber = rowNumber + 1;

                                                if (rowNumber >= 8)
                                                {

                                                    if (row[1].ToString() != "" && row[excelLoopLimitsingle].ToString() != "")
                                                    {
                                                        int questionNumber = 0;
                                                        StudentsCourseMarks astudStudentCourseMarks = new StudentsCourseMarks();
                                                        astudStudentCourseMarks.StudentMarks = new List<Marks>();

                                                        string roll = row[1].ToString() == "" ? "0" : row[1].ToString();

                                                        astudStudentCourseMarks.Roll = roll.Trim();

                                                        var studentCourseHistory = stdCourseHistoryList.FirstOrDefault(x => x.Roll == roll);
                                                        if (studentCourseHistory != null)
                                                        {
                                                            astudStudentCourseMarks.StudentCourseHistoryId = studentCourseHistory.ID;
                                                            astudStudentCourseMarks.ExamTemplateTypeId = examTemplateItemId;
                                                        }
                                                        else
                                                        {
                                                            breakpoint = breakpoint + 1;
                                                            break;
                                                        }

                                                        for (int i = 2; i <= excelLoopLimit; i++)
                                                        {

                                                            Marks marks = new Marks();
                                                            marks.ObtainedMarks = row[i].ToString() == "" ? 0 : Convert.ToDecimal(row[i]);
                                                            astudStudentCourseMarks.TotalObtainedMarks += marks.ObtainedMarks;
                                                            marks.QuestionNumber = ++questionNumber;
                                                            astudStudentCourseMarks.StudentMarks.Add(marks);
                                                        }

                                                        string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                                                        User user = UserManager.GetByLogInId(loginId);
                                                        if (user != null)
                                                        {
                                                            astudStudentCourseMarks.CreateBy = user.User_ID;
                                                            astudStudentCourseMarks.CreatedDate = DateTime.Now;
                                                            astudStudentCourseMarks.ModifiedBy = user.User_ID;
                                                            astudStudentCourseMarks.ModifiedDate = DateTime.Now;
                                                        }


                                                        if (astudStudentCourseMarks.StudentMarks != null && astudStudentCourseMarks.StudentMarks.Count > 0)
                                                        {
                                                            studentsCourseMarksesList.Add(astudStudentCourseMarks);
                                                        }


                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            MessageView("You have Uploaded a Wrong Course Mark File  !!", "fail");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageView("No Data Found in Excel  !!", "fail");
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //System.IO.File.Delete(ExcelPath);

                                    MessageView("Failed to Read Data from Excel; Error: " + ex.Message.ToString(), "fail");
                                    return;
                                }


                                #endregion


                                if (breakpoint > 0)
                                {
                                    MessageView("You have Uploaded a wrong file. Student is not matched against this course or section !!", "fail");
                                    return;
                                }


                                int checkExcelTotalMarkIsGreaterThenExamTemplatemMark = 0;
                                if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                {
                                    foreach (var tData in studentsCourseMarksesList)
                                    {
                                        if (tData.TotalObtainedMarks > examTemplateItemExamMark)
                                        {
                                            checkExcelTotalMarkIsGreaterThenExamTemplatemMark = checkExcelTotalMarkIsGreaterThenExamTemplatemMark + 1;
                                            break;
                                        }
                                    }
                                }

                                if (checkExcelTotalMarkIsGreaterThenExamTemplatemMark > 0)
                                {
                                    MessageView("Excel Total Mark is Greater Than Exam Template Mark !!", "fail");
                                    return;
                                }


                                int isInsertOrUpdateCount = 0;
                                if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                {
                                    foreach (StudentsCourseMarks scmModel in studentsCourseMarksesList)
                                    {
                                        ExamMarkFirstSecondThirdExaminer emfsteModel = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(scmModel.StudentCourseHistoryId, scmModel.ExamTemplateTypeId);

                                        if (emfsteModel != null)
                                        {
                                            // UPDATE

                                            emfsteModel.FirstExaminerMark = scmModel.TotalObtainedMarks;
                                            //emfsteModel.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            emfsteModel.IsAbsent = false;
                                            #region N/A -- FirstExaminerMarkSubmissionStatus
                                            //if (!string.IsNullOrEmpty(emfsteModel.FirstExaminerMarkSubmissionStatus.ToString()))
                                            //{
                                            //    int statusId = Convert.ToInt16(emfsteModel.FirstExaminerMarkSubmissionStatus);
                                            //    statusId = statusId + 1;
                                            //    emfsteModel.FirstExaminerMarkSubmissionStatus = statusId;
                                            //}
                                            //else
                                            //{
                                            //    emfsteModel.FirstExaminerMarkSubmissionStatus = 1;
                                            //}
                                            #endregion
                                            emfsteModel.ModifiedBy = userObj.Id;
                                            emfsteModel.ModifiedDate = DateTime.Now;

                                            bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(emfsteModel);

                                            if (isUpdate == true)
                                            {
                                                isInsertOrUpdateCount++;

                                                InsertOrUpdateExamQuestionMark(scmModel, 1);
                                            }
                                        }
                                        else
                                        {
                                            // INSERT

                                            ExamMarkFirstSecondThirdExaminer model = new ExamMarkFirstSecondThirdExaminer();

                                            model.CourseHistoryId = scmModel.StudentCourseHistoryId;
                                            model.ExamTemplateItemId = scmModel.ExamTemplateTypeId;
                                            model.FirstExaminerMark = scmModel.TotalObtainedMarks;

                                            //model.FirstExaminerMarkSubmissionStatus = 1;
                                            //model.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;

                                            model.IsAbsent = false;

                                            model.CreatedBy = userObj.Id;
                                            model.CreatedDate = DateTime.Now;


                                            int examMarkFirstSecondThirdExaminerId = ExamMarkFirstSecondThirdExaminerManager.Insert(model);

                                            if (examMarkFirstSecondThirdExaminerId > 0)
                                            {
                                                isInsertOrUpdateCount++;

                                                InsertOrUpdateExamQuestionMark(scmModel, 1);
                                            }

                                        }
                                    }


                                    if (isInsertOrUpdateCount > 0)
                                    {
                                        MessageView("Data Updated Successfully !!", "success");
                                    }
                                    else
                                    {
                                        MessageView("No Data Updated !!", "fail");
                                    }

                                }
                                else
                                {
                                    //MessageView("No Mark is provided in Excel !!", "fail");
                                    MessageView("Same Mark is already Exist; Please provide a new mark for Upload  !!", "fail");
                                }

                            }
                            else
                            {
                                MessageView("File formate is not correct. File should be (XLSX, XLS)", "fail");
                            }
                        }
                        else
                        {
                            MessageView("Something went wrong while Uploading First Examiner Exam Mark", "fail");
                        }

                    }
                    else
                    {
                        MessageView("No File is Selected !!", "fail");
                    }
                }
                else
                {
                    MessageView("Please Select Exam !!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnViewFirstExaminer_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtenderFirstExaminer.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examTemplateItemId = int.Parse(commandArgs[2]);

            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            try
            {
                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0 && examId > 0)
                {

                    string param = acaCalSectionId.ToString() + "_" + examSetupId.ToString() + "_" + 1 + "_" + examId.ToString() + "_" + employeeId;
                    string url = "~/Module/result/Report/RPTExamMarkEntry.aspx?val=" + param;
                    Response.Redirect(url, false);

                    #region N/A
                    //#region Get StudentCourseHistory
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                    //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                    //{
                    //    MessageView("No student is found agaist this course or section !!", "fail");
                    //    return;
                    //}
                    //#endregion

                    //#region Get GridView Details
                    //List<ExamMarkGridViewDTO> emgvDTOList = new List<ExamMarkGridViewDTO>();

                    //foreach (var tData in stdCourseHistoryList)
                    //{
                    //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                    //    Student s = StudentManager.GetById(tData.StudentID);
                    //    ExamMarkFirstSecondThirdExaminer em = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(tData.ID, examTemplateItemId);

                    //    if (s != null && em != null)
                    //    {
                    //        model.ExamMarkId = em.ID;
                    //        model.Roll = s.Roll;
                    //        model.Name = s.BasicInfo.FullName;
                    //        try
                    //        {
                    //            if (!string.IsNullOrEmpty(em.FirstExaminerMark.ToString()))
                    //            {
                    //                model.Mark = Convert.ToDecimal(em.FirstExaminerMark);
                    //            }
                    //            else
                    //            {
                    //                model.Mark = 0.00M;
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            model.Mark = 0.00M;
                    //        }

                    //        model.PresentAbsent = Convert.ToBoolean(em.IsAbsent) == true ? "Absent" : "Present";

                    //        emgvDTOList.Add(model);
                    //    }
                    //}

                    //if (emgvDTOList != null && emgvDTOList.Count > 0)
                    //{
                    //    gvExamMarkFirstExaminerView.DataSource = emgvDTOList;
                    //    gvExamMarkFirstExaminerView.DataBind();
                    //}
                    //else
                    //{
                    //    gvExamMarkFirstExaminerView.DataSource = null;
                    //    gvExamMarkFirstExaminerView.DataBind();
                    //}
                    //#endregion

                    //#region Get Shoet Info Details
                    //ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                    //if (emgvsiDTO != null)
                    //{
                    //    lblFirstExaminerModalViewCourse.Text = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                    //    lblFirstExaminerModalViewExam.Text = emgvsiDTO.ExamName;
                    //    lblFirstExaminerModalViewTotalStudent.Text = emgvsiDTO.TotalStudentCount.ToString();
                    //    lblFirstExaminerModalViewAbsentCount.Text = emgvsiDTO.AbsentCount.ToString();
                    //}
                    //#endregion 
                    #endregion
                }
                else
                {
                    MessageView("Something went wrong while Loading First Examiner Exam Mark Data", "fail");
                }
            }
            catch (Exception ex)
            {

                MessageView("Something went wrong while Loading First Examiner Exam Mark Data; Error: " + ex.Message.ToString(), "fail");
            }
        }
        #endregion

        #region Second Examiner
        protected void btnDownloadSecondExaminer_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            int flagExcept = 0;
            string fileName = string.Empty;
            FileInfo newFile = null;

            try
            {
                bool excelLoopLimit = false;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);


                Button btn = (Button)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                int examSetupId = int.Parse(commandArgs[1]);

                if (acaCalSectionId > 0 && programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                    Program program = ProgramManager.GetById(academicCalenderSection.ProgramID);
                    AcademicCalender acaCal = AcademicCalenderManager.GetById(academicCalenderSection.AcademicCalenderID);
                    Course course = CourseManager.GetByCourseIdVersionId(academicCalenderSection.CourseID, academicCalenderSection.VersionID);
                    Employee emp1 = EmployeeManager.GetById(academicCalenderSection.TeacherOneID);

                    fileName = course.VersionCode.Replace(' ', '_') + "_" + academicCalenderSection.SectionName.Replace(' ', '_') + "_SE";
                    AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);

                    if (acs != null)
                    {
                        List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);


                        etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                        if (etiList == null || etiList.Count == 0)
                        {
                            MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                            return;
                        }

                        if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                        {
                            excelLoopLimit = true;
                        }

                    }
                    else
                    {
                        MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                        return;
                    }
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                    List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(examId, acaCalSectionId, null, employeeId, null);

                    List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();

                    if (stdCourseHistoryList != null && stdCourseHistoryList.Count > 0)
                    {
                        List<Student> studentList = new List<Student>();
                        foreach (var tData in stdCourseHistoryList)
                        {
                            // CourseHistoryId
                            // ColumnType = 1 (Nullable)
                            // MultipleExaminer = 0 (Nullable)
                            // ExamStatus = 1 (Nullable)
                            bool checkStudentHasTwoExamMarkInContinuousAssessmentExam = true; //CheckStudentHasTwoExamMarkInContinuousAssessmentExam(tData.ID, 1, 0, 1);

                            if (checkStudentHasTwoExamMarkInContinuousAssessmentExam == true)
                            {
                                Student model = StudentManager.GetById(tData.StudentID);
                                if (model != null)
                                {
                                    studentList.Add(model);


                                    #region Check Is there any Question Mark Data Available
                                    StudentsCourseMarks scm = new StudentsCourseMarks();

                                    for (int i = 1; i <= 10; i++)
                                    {
                                        ExamMarkQuestion emq = ExamMarkQuestionManager.GetByStudentIdCourseHistoryId(model.StudentID, tData.ID, i);
                                        if (emq != null && emq.SecondExaminerMark != null)
                                        {
                                            Marks marks = new Marks();
                                            marks.ObtainedMarks = Convert.ToDecimal(emq.SecondExaminerMark);
                                            marks.QuestionNumber = Convert.ToInt32(emq.QuestionNo);
                                            scm.StudentMarks.Add(marks);
                                        }
                                    }


                                    if (scm.StudentMarks != null && scm.StudentMarks.Count > 0)
                                    {
                                        scm.StudentId = model.StudentID;
                                        scm.Roll = model.Roll;
                                        scm.StudentCourseHistoryId = tData.ID;

                                        studentsCourseMarksesList.Add(scm);
                                    }
                                    #endregion

                                }
                            }
                        }


                        if (studentList != null && studentList.Count > 0)
                        {

                            string departmentName = string.Empty;
                            string courseCode = string.Empty;
                            string totalMark = string.Empty;
                            string courseTitle = string.Empty;
                            string teacherName = string.Empty;
                            string teacherDesignation = string.Empty;
                            string examName = string.Empty;
                            string session = string.Empty;

                            #region Get Header Info
                            ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                            if (emgvsiDTO != null)
                            {
                                departmentName = emgvsiDTO.DepartmentName;
                                courseCode = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                                totalMark = emgvsiDTO.ExamMark.ToString();
                                courseTitle = emgvsiDTO.CourseTitle;
                                teacherName = emgvsiDTO.TeacherName;
                                teacherDesignation = emgvsiDTO.TeacherDesignation;
                                examName = emgvsiDTO.ExamName;
                                session = emgvsiDTO.Session;
                            }
                            #endregion


                            FileInfo template = null;
                            if (excelLoopLimit)
                            {
                                template = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/ExcelFiles/SingleMarkSheet.xlsx"));
                            }
                            else
                            {
                                template = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/ExcelFiles/MarkSheetV2.xlsx"));
                            }
                            //FileInfo template = new FileInfo(HttpContext.Current.Server.MapPath("~/ExcelFiles/Demo.xlsx"));
                            string path = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/Grade_Sheet_Files/")).DirectoryName;
                            //@"~\Result\Grade_Sheet_Files\";
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            newFile = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/Grade_Sheet_Files/" + fileName + ".xlsx"));
                            //new FileInfo(@"~\Result\Grade_Sheet_Files\" + fileName + ".xlsx");

                            using (ExcelPackage excelPackage = new ExcelPackage(newFile, template))
                            {
                                ExcelWorkbook myWorkbook = excelPackage.Workbook;

                                ExcelWorksheet gradeSheet = myWorkbook.Worksheets["Final"];


                                //gradeSheet.Cell(4, 3).Value = departmentName;
                                //gradeSheet.Cell(5, 3).Value = courseCode;
                                //gradeSheet.Cell(6, 3).Value = totalMark;
                                //gradeSheet.Cell(5, 8).Value = courseTitle;
                                //gradeSheet.Cell(6, 14).Value = acaCalSectionId.ToString();
                                ////gradeSheet.Cell(6, 15).Value = examTemplateItemId.ToString();

                                gradeSheet.Cell(5, 3).Value = departmentName;
                                gradeSheet.Cell(6, 7).Value = courseCode;
                                gradeSheet.Cell(6, 3).Value = totalMark;
                                gradeSheet.Cell(6, 12).Value = courseTitle;
                                gradeSheet.Cell(7, 14).Value = acaCalSectionId.ToString();
                                gradeSheet.Cell(3, 6).Value = examName.ToString();
                                gradeSheet.Cell(5, 12).Value = session.ToString();



                                int footerCount = 9;
                                if (stdCourseHistoryList.Count > 0 && stdCourseHistoryList != null)
                                {


                                    int index = 0;
                                    foreach (var temp in studentList)
                                    {
                                        index++;

                                        gradeSheet.Cell(8 + index, 1).Value = index.ToString();
                                        gradeSheet.Cell(8 + index, 2).Value = temp.Roll.ToString().ToLower();


                                        #region If Question Mark is Exist, Assign into Fresh Excel
                                        if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                        {
                                            int startColumn = 3;
                                            int endColumn = 12;
                                            int questionNo = 1;
                                            for (int column = startColumn; column <= endColumn; column++)
                                            {
                                                foreach (var tStudentsCourseMarksesList in studentsCourseMarksesList.Where(x => x.StudentId == temp.StudentID).ToList())
                                                {
                                                    foreach (Marks mark in tStudentsCourseMarksesList.StudentMarks.Where(x => x.QuestionNumber == questionNo).ToList())
                                                    {
                                                        if (mark != null)
                                                        {
                                                            gradeSheet.Cell(8 + index, column).Value = mark.ObtainedMarks.ToString();
                                                        }

                                                        questionNo++;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (excelLoopLimit)
                                        {
                                            gradeSheet.Cell(8 + index, 4).Formula = "=IF(SUM(C" + (8 + index) + ":C" + (8 + index) + ")=0,\"\",SUM(C" + (8 + index) + ":C" + (8 + index) + "))"; //"=SUM(A" + (8 + index) + ",B" + (8 + index) + ")";
                                        }
                                        else
                                        {
                                            gradeSheet.Cell(8 + index, 13).Formula = "=IF(SUM(C" + (8 + index) + ":L" + (8 + index) + ")=0,\"\",SUM(C" + (8 + index) + ":L" + (8 + index) + "))"; //"=SUM(A" + (8 + index) + ",B" + (8 + index) + ")";

                                        }

                                        footerCount++;

                                    }


                                    // where loop is end add 3 number to get the position of after 3 cell Row
                                    footerCount = footerCount + 3;

                                    int dateAndExaminerSignatureRow = footerCount;

                                    int examinerNameRow = footerCount + 1;
                                    int designationRow = footerCount + 2;
                                    int departmentRow = footerCount + 3;

                                    gradeSheet.Cell(dateAndExaminerSignatureRow, 1).Value = "Date :";
                                    gradeSheet.Cell(dateAndExaminerSignatureRow, 6).Value = "Examiner Signature :";

                                    gradeSheet.Cell(examinerNameRow, 6).Value = "Examiner Name :";
                                    gradeSheet.Cell(examinerNameRow, 9).Value = teacherName;

                                    gradeSheet.Cell(designationRow, 6).Value = "Designation :";
                                    gradeSheet.Cell(designationRow, 9).Value = teacherDesignation;

                                    gradeSheet.Cell(departmentRow, 6).Value = "Department :";
                                    gradeSheet.Cell(departmentRow, 9).Value = departmentName;


                                    excelPackage.Save();

                                    #region Log Insert
                                    //LogGeneralManager.Insert(
                                    //        DateTime.Now,
                                    //        BaseAcaCalCurrent.Code,
                                    //        BaseAcaCalCurrent.FullCode,
                                    //        BaseCurrentUserObj.LogInID,
                                    //        "",
                                    //        "",
                                    //        "Grade Sheet Download",
                                    //        "Downaloded grade sheet of " + ddlAcaCalBatch.SelectedItem.Text + " " + ddlProgram.SelectedItem.Text + " " + ddlAcaCalSection.SelectedItem.Text,
                                    //        "normal",
                                    //        _pageId,
                                    //        _pageName,
                                    //        _pageUrl,
                                    //        "");

                                    #endregion
                                }
                                else
                                {
                                    flagExcept = 1;
                                    MessageView("No Student Found !!", "fail");
                                }
                            }
                        }
                        else
                        {
                            flagExcept = 1;
                            MessageView("Every student needs to have at least 2 marks on continuous exam to download Final Exam Student List !!", "fail");
                        }
                    }
                    else
                    {
                        flagExcept = 1;
                        MessageView("No Student Found !!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select Program, Year, Semester and Exam.", "fail");
                }
            }
            catch (Exception ex)
            {
                flagExcept = 1;
                MessageView("Exception: Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
            finally
            {
                //if()
                if (flagExcept == 0)
                {
                    DownloadFile(newFile);
                    //lblMsg.Text = "Check D Drive. File Name- " + fileName;
                }
            }
        }

        protected void btnUploadSecondExaminer_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtenderSecontExaminerExcelUpload.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examTemplateItemId = int.Parse(commandArgs[2]);

            if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0)
            {
                hfSecontExaminerExamMarkUploadAcaCalSectionId.Value = acaCalSectionId.ToString();
                hfSecontExaminerExamMarkUploadExamTemplateItemId.Value = examTemplateItemId.ToString();
            }
            else
            {
                hfSecontExaminerExamMarkUploadAcaCalSectionId.Value = "0";
                hfSecontExaminerExamMarkUploadExamTemplateItemId.Value = "0";
            }
        }

        protected void SecontExaminerExamMarkUpload_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int excelLoopLimit = 11;
                int excelLoopLimitsingle = 12;

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (examId > 0)
                {

                    if (FileUploadExamMarkSecontExaminer.HasFile)
                    {

                        if (!string.IsNullOrEmpty(hfSecontExaminerExamMarkUploadAcaCalSectionId.Value) && Convert.ToInt32(hfSecontExaminerExamMarkUploadAcaCalSectionId.Value) > 0)
                        {
                            int acaCalSectionId = Convert.ToInt32(hfSecontExaminerExamMarkUploadAcaCalSectionId.Value);

                            string FileName = Path.GetFileName(FileUploadExamMarkSecontExaminer.FileName);
                            //Save File to Session
                            //SessionSGD.SaveObjToSession(FileName, "fileName");
                            if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                            {

                                //string path = Server.MapPath("~/Module/result/Grade_Sheet_Files/");
                                #region Delete Existing File
                                #region N/A
                                //string filePath = path + FileUploadExamMarkFirstExaminer.FileName;
                                //FileInfo file = new FileInfo(filePath);
                                //if (file.Exists)//check file exsit or not
                                //{
                                //    file.Delete();
                                //}  
                                #endregion
                                //System.IO.DirectoryInfo di = new DirectoryInfo(path);

                                //foreach (FileInfo file in di.GetFiles())
                                //{
                                //    try
                                //    {

                                //        file.Delete();

                                //    }
                                //    catch (Exception ex)
                                //    {

                                //    }
                                //}
                                #endregion


                                #region Get StudentCourseHistory

                                //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                                List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(examId, acaCalSectionId, null, employeeId, null);


                                if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                                {
                                    MessageView("No student is found agaist this course or section !!", "fail");
                                    return;
                                }
                                #endregion

                                #region Get ExamTemplateItemID and TemplateExamMark
                                int examTemplateItemId = 0;
                                decimal examTemplateItemExamMark = 0.00M;
                                AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                                if (acs != null)
                                {
                                    List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);
                                    etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                                    if (etiList == null || etiList.Count == 0)
                                    {
                                        MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                                        return;
                                    }

                                    if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                                    {
                                        excelLoopLimit = 2;
                                        excelLoopLimitsingle = 3;
                                    }

                                    examTemplateItemId = etiList.Where(x => x.MultipleExaminer == 1).FirstOrDefault().ExamTemplateItemId;
                                    examTemplateItemExamMark = etiList.Where(x => x.MultipleExaminer == 1).FirstOrDefault().ExamMark;
                                }
                                else
                                {
                                    MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                                    return;
                                }
                                #endregion

                                #region Get Data from Excel


                                int breakpoint = 0;

                                List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();

                                DataTable DtTable = new DataTable();
                                string path = Path.GetFileName(FileUploadExamMarkSecontExaminer.FileName);
                                path = path.Replace(" ", "");

                                String ExcelPath = Server.MapPath("~/Module/result/Grade_Sheet_Files/") + path;

                                try
                                {
                                    if (File.Exists(ExcelPath))
                                    {
                                        System.IO.File.Delete(ExcelPath);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                try
                                {
                                    FileUploadExamMarkSecontExaminer.SaveAs(Server.MapPath("~/Module/result/Grade_Sheet_Files/") + path);

                                    OleDbConnection mycon = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + ExcelPath + "; Extended Properties=Excel 8.0; Persist Security Info = False");
                                    mycon.Open();
                                    OleDbCommand cmd = new OleDbCommand("select * from [Final$]", mycon);
                                    OleDbDataAdapter da = new OleDbDataAdapter();
                                    da.SelectCommand = cmd;
                                    DataSet ds = new DataSet();
                                    da.Fill(ds);
                                    DtTable = ds.Tables[0];
                                    //GridView1.DataSource = ds.Tables[0];
                                    //GridView1.DataBind();
                                    mycon.Close();

                                    if (DtTable.Rows.Count > 0)
                                    {
                                        DataRow rowT = DtTable.Rows[5];

                                        if (rowT[13].ToString() != "" && Convert.ToInt16(rowT[13].ToString()) == acaCalSectionId)
                                        {
                                            int rowNumber = 0;
                                            #region GetDataFromDataTable
                                            foreach (DataRow row in DtTable.Rows)
                                            {
                                                rowNumber = rowNumber + 1;

                                                if (rowNumber >= 8)
                                                {

                                                    if (row[1].ToString() != "" && row[excelLoopLimitsingle].ToString() != "")
                                                    {
                                                        int questionNumber = 0;
                                                        StudentsCourseMarks astudStudentCourseMarks = new StudentsCourseMarks();
                                                        astudStudentCourseMarks.StudentMarks = new List<Marks>();

                                                        string roll = row[1].ToString() == "" ? "0" : row[1].ToString();

                                                        astudStudentCourseMarks.Roll = roll.Trim();

                                                        var studentCourseHistory = stdCourseHistoryList.FirstOrDefault(x => x.Roll == roll);
                                                        if (studentCourseHistory != null)
                                                        {
                                                            astudStudentCourseMarks.StudentCourseHistoryId = studentCourseHistory.ID;
                                                            astudStudentCourseMarks.ExamTemplateTypeId = examTemplateItemId;
                                                        }
                                                        else
                                                        {
                                                            breakpoint = breakpoint + 1;
                                                            break;
                                                        }

                                                        for (int i = 2; i <= excelLoopLimit; i++)
                                                        {

                                                            Marks marks = new Marks();
                                                            marks.ObtainedMarks = row[i].ToString() == "" ? 0 : Convert.ToDecimal(row[i]);
                                                            astudStudentCourseMarks.TotalObtainedMarks += marks.ObtainedMarks;
                                                            marks.QuestionNumber = ++questionNumber;
                                                            astudStudentCourseMarks.StudentMarks.Add(marks);
                                                        }

                                                        string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                                                        User user = UserManager.GetByLogInId(loginId);
                                                        if (user != null)
                                                        {
                                                            astudStudentCourseMarks.CreateBy = user.User_ID;
                                                            astudStudentCourseMarks.CreatedDate = DateTime.Now;
                                                            astudStudentCourseMarks.ModifiedBy = user.User_ID;
                                                            astudStudentCourseMarks.ModifiedDate = DateTime.Now;
                                                        }


                                                        if (astudStudentCourseMarks.StudentMarks != null && astudStudentCourseMarks.StudentMarks.Count > 0)
                                                        {
                                                            studentsCourseMarksesList.Add(astudStudentCourseMarks);
                                                        }


                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            MessageView("You have Uploaded a Wrong Course Mark File  !!", "fail");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageView("No Data Found in Excel  !!", "fail");
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //System.IO.File.Delete(ExcelPath);

                                    MessageView("Failed to Read Data from Excel; Error: " + ex.Message.ToString(), "fail");
                                    return;
                                }

                                #region N/A
                                //FileUploadExamMarkSecontExaminer.SaveAs(path + FileUploadExamMarkSecontExaminer.FileName);
                                //string savePath = path + FileUploadExamMarkSecontExaminer.FileName;


                                //Excel.Application xlApp = new Excel.Application();
                                //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(savePath);
                                //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                                //Excel.Range xlRange = xlWorksheet.UsedRange;

                                //int rowCount = xlRange.Rows.Count;
                                ////int colCount = xlRange.Columns.Count;
                                //int colCount = 12;
                                //int rCnt;
                                //int cCnt;
                                //int breakpoint = 0;

                                //List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();
                                //for (rCnt = 9; rCnt <= rowCount; rCnt++)
                                //{
                                //    int questionNumber = 0;
                                //    StudentsCourseMarks astudStudentCourseMarks = new StudentsCourseMarks();
                                //    astudStudentCourseMarks.StudentMarks = new List<Marks>();
                                //    if (((Excel.Range)xlRange.Cells[rCnt, 2]).Value2 != null)
                                //    {
                                //        string roll = ((Excel.Range)xlRange.Cells[rCnt, 2]).Value2.ToString();

                                //        astudStudentCourseMarks.Roll = roll.Trim();

                                //        //int rollDigit = roll.Length;
                                //        //roll = roll.PadLeft(rollDigit + 1, '0');
                                //        var studentCourseHistory = stdCourseHistoryList.FirstOrDefault(x => x.Roll == roll);
                                //        if (studentCourseHistory != null)
                                //        {
                                //            astudStudentCourseMarks.StudentCourseHistoryId = studentCourseHistory.ID;
                                //            astudStudentCourseMarks.ExamTemplateTypeId = examTemplateItemId;
                                //        }
                                //        else
                                //        {
                                //            breakpoint = breakpoint + 1;
                                //            break;
                                //        }

                                //        for (cCnt = 3; cCnt <= colCount; cCnt++)
                                //        {
                                //            if (((Excel.Range)xlRange.Cells[rCnt, cCnt]).Value2 != null)
                                //            {
                                //                //aStudent.StudentMarks.ObtainedMarks = (double) (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                                //                Marks marks = new Marks();
                                //                marks.ObtainedMarks = (decimal)((Excel.Range)xlRange.Cells[rCnt, cCnt]).Value2;
                                //                astudStudentCourseMarks.TotalObtainedMarks += marks.ObtainedMarks;
                                //                marks.QuestionNumber = ++questionNumber;
                                //                astudStudentCourseMarks.StudentMarks.Add(marks);
                                //            }
                                //            else
                                //            {
                                //                ++questionNumber;
                                //            }
                                //        }
                                //        #region N/A
                                //        //if (conversionMarksTextBox.Visible)
                                //        //{
                                //        //    try
                                //        //    {
                                //        //        if (conversionMarksTextBox.Text != "")
                                //        //        {
                                //        //            string course = ddlAcaCalSection.SelectedValue;
                                //        //            int acaCalSection = Convert.ToInt32(course);
                                //        //            decimal totalMarks = Convert.ToDecimal(conversionMarksTextBox.Text);
                                //        //            AcademicCalenderSection acacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSection);
                                //        //            List<ExamTemplateItem> examList = ExamTemplateItemManager.GetByExamTemplateId(acacalSectionObj.BasicExamTemplateId).Where(x => x.ColumnType == 1).ToList();
                                //        //            var examTemplateItem = examList.First(c => c.ExamTemplateItemId == examTypeItemId);
                                //        //            astudStudentCourseMarks.TotalObtainedMarks =
                                //        //                (examTemplateItem.ExamMark * astudStudentCourseMarks.TotalObtainedMarks) / totalMarks;
                                //        //        }
                                //        //        else
                                //        //        {
                                //        //            lblMsg.Text = "Please enter the exam taken total marks.";
                                //        //            return;

                                //        //        }
                                //        //    }
                                //        //    catch (Exception ex)
                                //        //    {
                                //        //        lblMsg.Text = "Please enter the exam taken total marks.";
                                //        //    }
                                //        //} 
                                //        #endregion
                                //        string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                                //        User user = UserManager.GetByLogInId(loginId);
                                //        if (user != null)
                                //        {
                                //            astudStudentCourseMarks.CreateBy = user.User_ID;
                                //            astudStudentCourseMarks.CreatedDate = DateTime.Now;
                                //            astudStudentCourseMarks.ModifiedBy = user.User_ID;
                                //            astudStudentCourseMarks.ModifiedDate = DateTime.Now;
                                //        }

                                //        if (astudStudentCourseMarks.StudentMarks != null && astudStudentCourseMarks.StudentMarks.Count > 0)
                                //        {
                                //            studentsCourseMarksesList.Add(astudStudentCourseMarks);
                                //        }
                                //    }
                                //}
                                //// END: for (rCnt = 10; rCnt <= rowCount; rCnt++)


                                ////cleanup
                                //GC.Collect();
                                //GC.WaitForPendingFinalizers();

                                ////rule of thumb for releasing com objects:
                                ////  never use two dots, all COM objects must be referenced and released individually
                                ////  ex: [somthing].[something].[something] is bad

                                ////release com objects to fully kill excel process from running in the background
                                //Marshal.ReleaseComObject(xlRange);
                                //Marshal.ReleaseComObject(xlWorksheet);

                                ////close and release
                                //xlWorkbook.Close();
                                //Marshal.ReleaseComObject(xlWorkbook);

                                ////quit and release
                                //xlApp.Quit();
                                //Marshal.ReleaseComObject(xlApp); 
                                #endregion
                                #endregion


                                if (breakpoint > 0)
                                {
                                    MessageView("You have Uploaded a wrong file. Student is not matched against this course or section !!", "fail");
                                    return;
                                }


                                int checkExcelTotalMarkIsGreaterThenExamTemplatemMark = 0;
                                if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                {
                                    foreach (var tData in studentsCourseMarksesList)
                                    {
                                        if (tData.TotalObtainedMarks > examTemplateItemExamMark)
                                        {
                                            checkExcelTotalMarkIsGreaterThenExamTemplatemMark = checkExcelTotalMarkIsGreaterThenExamTemplatemMark + 1;
                                            break;
                                        }
                                    }
                                }

                                if (checkExcelTotalMarkIsGreaterThenExamTemplatemMark > 0)
                                {
                                    MessageView("Excel Total Mark is Greater Than Exam Template Mark !!", "fail");
                                    return;
                                }


                                int isInsertOrUpdateCount = 0;
                                if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                {
                                    foreach (StudentsCourseMarks scmModel in studentsCourseMarksesList)
                                    {
                                        ExamMarkFirstSecondThirdExaminer emfsteModel = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(scmModel.StudentCourseHistoryId, scmModel.ExamTemplateTypeId);

                                        if (emfsteModel != null)
                                        {
                                            // UPDATE

                                            emfsteModel.SecondExaminerMark = scmModel.TotalObtainedMarks;
                                            //emfsteModel.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            emfsteModel.IsAbsent = false;
                                            #region N/A -- SecondExaminerMarkSubmissionStatus
                                            //if (!string.IsNullOrEmpty(emfsteModel.SecondExaminerMarkSubmissionStatus.ToString()))
                                            //{
                                            //    int statusId = Convert.ToInt16(emfsteModel.SecondExaminerMarkSubmissionStatus);
                                            //    statusId = statusId + 1;
                                            //    emfsteModel.SecondExaminerMarkSubmissionStatus = statusId;
                                            //}
                                            //else
                                            //{
                                            //    emfsteModel.SecondExaminerMarkSubmissionStatus = 1;
                                            //}
                                            #endregion
                                            emfsteModel.ModifiedBy = userObj.Id;
                                            emfsteModel.ModifiedDate = DateTime.Now;

                                            bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(emfsteModel);

                                            if (isUpdate == true)
                                            {
                                                isInsertOrUpdateCount++;

                                                InsertOrUpdateExamQuestionMark(scmModel, 2);
                                            }
                                        }
                                        else
                                        {
                                            // INSERT

                                            ExamMarkFirstSecondThirdExaminer model = new ExamMarkFirstSecondThirdExaminer();

                                            model.CourseHistoryId = scmModel.StudentCourseHistoryId;
                                            model.ExamTemplateItemId = scmModel.ExamTemplateTypeId;
                                            model.SecondExaminerMark = scmModel.TotalObtainedMarks;

                                            //model.SecondExaminerMarkSubmissionStatus = 1;
                                            //model.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;

                                            model.IsAbsent = false;

                                            model.CreatedBy = userObj.Id;
                                            model.CreatedDate = DateTime.Now;


                                            int examMarkSecondSecondThirdExaminerId = ExamMarkFirstSecondThirdExaminerManager.Insert(model);

                                            if (examMarkSecondSecondThirdExaminerId > 0)
                                            {
                                                isInsertOrUpdateCount++;

                                                InsertOrUpdateExamQuestionMark(scmModel, 2);
                                            }

                                        }
                                    }


                                    if (isInsertOrUpdateCount > 0)
                                    {
                                        MessageView("Data Updated Successfully !!", "success");
                                    }
                                    else
                                    {
                                        MessageView("No Data Updated !!", "fail");
                                    }

                                }
                                else
                                {
                                    //MessageView("No Mark is provided in Excel !!", "fail");
                                    MessageView("Same Mark is already Exist; Please provide a new mark for Upload  !!", "fail");
                                }

                            }
                            else
                            {
                                MessageView("File formate is not correct. File should be (XLSX, XLS)", "fail");
                            }
                        }
                        else
                        {
                            MessageView("Something went wrong while Uploading Second Examiner Exam Mark", "fail");
                        }


                    }
                    else
                    {
                        MessageView("No File is Selected !!", "fail");
                    }
                }
                else
                {
                    MessageView("Please Select Exam !!", "fail");
                }

            }
            catch (Exception ex)
            {
                MessageView("Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnViewSecondExaminer_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtenderSecondExaminer.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examTemplateItemId = int.Parse(commandArgs[2]);

            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            try
            {
                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0 && examId > 0)
                {

                    string param = acaCalSectionId.ToString() + "_" + examSetupId.ToString() + "_" + 2 + "_" + examId.ToString() + "_" + employeeId;
                    string url = "~/Module/result/Report/RPTExamMarkEntry.aspx?val=" + param;
                    Response.Redirect(url, false);


                    #region N/A
                    //#region Get StudentCourseHistory
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                    //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                    //{
                    //    MessageView("No student is found agaist this course or section !!", "fail");
                    //    return;
                    //}
                    //#endregion

                    //#region Get GridView Details
                    //List<ExamMarkGridViewDTO> emgvDTOList = new List<ExamMarkGridViewDTO>();

                    //foreach (var tData in stdCourseHistoryList)
                    //{
                    //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                    //    Student s = StudentManager.GetById(tData.StudentID);
                    //    ExamMarkFirstSecondThirdExaminer em = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(tData.ID, examTemplateItemId);

                    //    if (s != null && em != null)
                    //    {
                    //        model.ExamMarkId = em.ID;
                    //        model.Roll = s.Roll;
                    //        model.Name = s.BasicInfo.FullName;
                    //        try
                    //        {
                    //            if (!string.IsNullOrEmpty(em.SecondExaminerMark.ToString()))
                    //            {
                    //                model.Mark = Convert.ToDecimal(em.SecondExaminerMark);
                    //            }
                    //            else
                    //            {
                    //                model.Mark = 0.00M;
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            model.Mark = 0.00M;
                    //        }

                    //        model.PresentAbsent = Convert.ToBoolean(em.IsAbsent) == true ? "Absent" : "Present";

                    //        emgvDTOList.Add(model);
                    //    }
                    //}

                    //if (emgvDTOList != null && emgvDTOList.Count > 0)
                    //{
                    //    gvExamMarkSecondExaminerView.DataSource = emgvDTOList;
                    //    gvExamMarkSecondExaminerView.DataBind();
                    //}
                    //else
                    //{
                    //    gvExamMarkSecondExaminerView.DataSource = null;
                    //    gvExamMarkSecondExaminerView.DataBind();
                    //}
                    //#endregion


                    //#region Get Shoet Info Details
                    //ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                    //if (emgvsiDTO != null)
                    //{
                    //    lblSecondExaminerModalViewCourse.Text = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                    //    lblSecondExaminerModalViewExam.Text = emgvsiDTO.ExamName;
                    //    lblSecondExaminerModalViewTotalStudent.Text = emgvsiDTO.TotalStudentCount.ToString();
                    //    lblSecondExaminerModalViewAbsentCount.Text = emgvsiDTO.AbsentCount.ToString();
                    //}
                    //#endregion 
                    #endregion

                }
                else
                {
                    MessageView("Something went wrong while Loading Second Examiner Exam Mark Data", "fail");
                }
            }
            catch (Exception ex)
            {

                MessageView("Something went wrong while Loading Second Examiner Exam Mark Data; Error: " + ex.Message.ToString(), "fail");
            }
        }
        #endregion

        #region Third Examiner
        protected void btnDownloadThirdExaminer_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");
            int flagExcept = 0;
            string fileName = string.Empty;
            FileInfo newFile = null;

            try
            {
                bool excelLoopLimit = false;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                Button btn = (Button)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                int examSetupId = int.Parse(commandArgs[1]);
                int examTemplateItemId = int.Parse(commandArgs[2]);

                if (acaCalSectionId > 0 && programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                    Program program = ProgramManager.GetById(academicCalenderSection.ProgramID);
                    AcademicCalender acaCal = AcademicCalenderManager.GetById(academicCalenderSection.AcademicCalenderID);
                    Course course = CourseManager.GetByCourseIdVersionId(academicCalenderSection.CourseID, academicCalenderSection.VersionID);
                    Employee emp1 = EmployeeManager.GetById(academicCalenderSection.TeacherOneID);

                    fileName = course.VersionCode.Replace(' ', '_') + "_" + academicCalenderSection.SectionName.Replace(' ', '_') + "_TE";
                    AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);

                    if (acs != null)
                    {
                        List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);


                        etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                        if (etiList == null || etiList.Count == 0)
                        {
                            MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                            return;
                        }

                        if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                        {
                            excelLoopLimit = true;
                        }

                    }
                    else
                    {
                        MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                        return;
                    }
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                    List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(examId, acaCalSectionId, null, null, employeeId);

                    List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();


                    if (stdCourseHistoryList != null && stdCourseHistoryList.Count > 0)
                    {
                        List<Student> studentList = new List<Student>();
                        foreach (var tData in stdCourseHistoryList)
                        {
                            ExamMarkFirstSecondThirdExaminer emfsteModel = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(tData.ID, examTemplateItemId);

                            if (emfsteModel != null && emfsteModel.ThirdExaminerStatus == 1)
                            {
                                Student model = StudentManager.GetById(tData.StudentID);
                                if (model != null)
                                {
                                    studentList.Add(model);


                                    #region Check Is there any Question Mark Data Available
                                    StudentsCourseMarks scm = new StudentsCourseMarks();

                                    for (int i = 1; i <= 10; i++)
                                    {
                                        ExamMarkQuestion emq = ExamMarkQuestionManager.GetByStudentIdCourseHistoryId(model.StudentID, tData.ID, i);
                                        if (emq != null && emq.ThirdExaminerMark != null)
                                        {
                                            Marks marks = new Marks();
                                            marks.ObtainedMarks = Convert.ToDecimal(emq.ThirdExaminerMark);
                                            marks.QuestionNumber = Convert.ToInt32(emq.QuestionNo);
                                            scm.StudentMarks.Add(marks);
                                        }
                                    }


                                    if (scm.StudentMarks != null && scm.StudentMarks.Count > 0)
                                    {
                                        scm.StudentId = model.StudentID;
                                        scm.Roll = model.Roll;
                                        scm.StudentCourseHistoryId = tData.ID;

                                        studentsCourseMarksesList.Add(scm);
                                    }
                                    #endregion
                                }
                            }
                        }


                        if (studentList != null && studentList.Count > 0)
                        {

                            string departmentName = string.Empty;
                            string courseCode = string.Empty;
                            string totalMark = string.Empty;
                            string courseTitle = string.Empty;
                            string teacherName = string.Empty;
                            string teacherDesignation = string.Empty;
                            string examName = string.Empty;
                            string session = string.Empty;

                            #region Get Header Info
                            ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                            if (emgvsiDTO != null)
                            {
                                departmentName = emgvsiDTO.DepartmentName;
                                courseCode = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                                totalMark = emgvsiDTO.ExamMark.ToString();
                                courseTitle = emgvsiDTO.CourseTitle;
                                teacherName = emgvsiDTO.TeacherName;
                                teacherDesignation = emgvsiDTO.TeacherDesignation;
                                examName = emgvsiDTO.ExamName;
                                session = emgvsiDTO.Session;
                            }
                            #endregion

                            FileInfo template = null;
                            if (excelLoopLimit)
                            {
                                template = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/ExcelFiles/SingleMarkSheet.xlsx"));
                            }
                            else
                            {
                                template = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/ExcelFiles/MarkSheetV2.xlsx"));
                            }
                            //FileInfo template = new FileInfo(HttpContext.Current.Server.MapPath("~/ExcelFiles/Demo.xlsx"));
                            string path = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/Grade_Sheet_Files/")).DirectoryName;
                            //@"~\Result\Grade_Sheet_Files\";
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            newFile = new FileInfo(HttpContext.Current.Server.MapPath("~/Module/result/Grade_Sheet_Files/" + fileName + ".xlsx"));
                            //new FileInfo(@"~\Result\Grade_Sheet_Files\" + fileName + ".xlsx");

                            using (ExcelPackage excelPackage = new ExcelPackage(newFile, template))
                            {
                                ExcelWorkbook myWorkbook = excelPackage.Workbook;

                                ExcelWorksheet gradeSheet = myWorkbook.Worksheets["Final"];

                                //gradeSheet.Cell(4, 3).Value = departmentName;
                                //gradeSheet.Cell(5, 3).Value = courseCode;
                                //gradeSheet.Cell(6, 3).Value = totalMark;
                                //gradeSheet.Cell(5, 8).Value = courseTitle;
                                //gradeSheet.Cell(6, 14).Value = acaCalSectionId.ToString();
                                ////gradeSheet.Cell(6, 15).Value = examTemplateItemId.ToString();

                                gradeSheet.Cell(5, 3).Value = departmentName;
                                gradeSheet.Cell(6, 7).Value = courseCode;
                                gradeSheet.Cell(6, 3).Value = totalMark;
                                gradeSheet.Cell(6, 12).Value = courseTitle;
                                gradeSheet.Cell(7, 14).Value = acaCalSectionId.ToString();
                                gradeSheet.Cell(3, 6).Value = examName.ToString();
                                gradeSheet.Cell(5, 12).Value = session.ToString();


                                int footerCount = 9;
                                if (stdCourseHistoryList.Count > 0 && stdCourseHistoryList != null)
                                {

                                    int index = 0;
                                    foreach (var temp in studentList)
                                    {
                                        index++;

                                        gradeSheet.Cell(8 + index, 1).Value = index.ToString();
                                        gradeSheet.Cell(8 + index, 2).Value = temp.Roll.ToString().ToLower();


                                        #region If Question Mark is Exist, Assign into Fresh Excel
                                        if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                        {
                                            int startColumn = 3;
                                            int endColumn = 12;
                                            int questionNo = 1;
                                            for (int column = startColumn; column <= endColumn; column++)
                                            {
                                                foreach (var tStudentsCourseMarksesList in studentsCourseMarksesList.Where(x => x.StudentId == temp.StudentID).ToList())
                                                {
                                                    foreach (Marks mark in tStudentsCourseMarksesList.StudentMarks.Where(x => x.QuestionNumber == questionNo).ToList())
                                                    {
                                                        if (mark != null)
                                                        {
                                                            gradeSheet.Cell(8 + index, column).Value = mark.ObtainedMarks.ToString();
                                                        }

                                                        questionNo++;
                                                    }
                                                }
                                            }
                                        }
                                        #endregion

                                        if (excelLoopLimit)
                                        {
                                            gradeSheet.Cell(8 + index, 4).Formula = "=IF(SUM(C" + (8 + index) + ":C" + (8 + index) + ")=0,\"\",SUM(C" + (8 + index) + ":C" + (8 + index) + "))"; //"=SUM(A" + (8 + index) + ",B" + (8 + index) + ")";
                                        }
                                        else
                                        {
                                            gradeSheet.Cell(8 + index, 13).Formula = "=IF(SUM(C" + (8 + index) + ":L" + (8 + index) + ")=0,\"\",SUM(C" + (8 + index) + ":L" + (8 + index) + "))"; //"=SUM(A" + (8 + index) + ",B" + (8 + index) + ")";

                                        }
                                        footerCount++;

                                    }


                                    // where loop is end add 3 number to get the position of after 3 cell Row
                                    footerCount = footerCount + 3;

                                    int dateAndExaminerSignatureRow = footerCount;

                                    int examinerNameRow = footerCount + 1;
                                    int designationRow = footerCount + 2;
                                    int departmentRow = footerCount + 3;

                                    gradeSheet.Cell(dateAndExaminerSignatureRow, 1).Value = "Date :";
                                    gradeSheet.Cell(dateAndExaminerSignatureRow, 6).Value = "Examiner Signature :";

                                    gradeSheet.Cell(examinerNameRow, 6).Value = "Examiner Name :";
                                    gradeSheet.Cell(examinerNameRow, 9).Value = teacherName;

                                    gradeSheet.Cell(designationRow, 6).Value = "Designation :";
                                    gradeSheet.Cell(designationRow, 9).Value = teacherDesignation;

                                    gradeSheet.Cell(departmentRow, 6).Value = "Department :";
                                    gradeSheet.Cell(departmentRow, 9).Value = departmentName;

                                    excelPackage.Save();

                                    #region Log Insert
                                    //LogGeneralManager.Insert(
                                    //        DateTime.Now,
                                    //        BaseAcaCalCurrent.Code,
                                    //        BaseAcaCalCurrent.FullCode,
                                    //        BaseCurrentUserObj.LogInID,
                                    //        "",
                                    //        "",
                                    //        "Grade Sheet Download",
                                    //        "Downaloded grade sheet of " + ddlAcaCalBatch.SelectedItem.Text + " " + ddlProgram.SelectedItem.Text + " " + ddlAcaCalSection.SelectedItem.Text,
                                    //        "normal",
                                    //        _pageId,
                                    //        _pageName,
                                    //        _pageUrl,
                                    //        "");

                                    #endregion
                                }
                                else
                                {
                                    flagExcept = 1;
                                    MessageView("No Student Found !!", "fail");
                                }
                            }
                        }
                        else
                        {
                            flagExcept = 1;
                            MessageView("Every student needs to have at least 2 marks on continuous exam to download Final Exam Student List !!", "fail");
                        }

                    }
                    else
                    {
                        flagExcept = 1;
                        MessageView("No Student Found !!", "fail");
                    }
                }
                else
                {
                    MessageView("Please select Program, Year, Semester and Exam.", "fail");
                }
            }
            catch (Exception ex)
            {
                flagExcept = 1;
                MessageView("Exception: Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
            finally
            {
                if (flagExcept == 0)
                {
                    DownloadFile(newFile);
                }
            }
        }

        protected void btnUploadThirdExaminer_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtenderThirdExaminerExcelUpload.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examTemplateItemId = int.Parse(commandArgs[2]);

            if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0)
            {
                hfThirdExaminerExamMarkUploadAcaCalSectionId.Value = acaCalSectionId.ToString();
                hfThirdExaminerExamMarkUploadExamTemplateItemId.Value = examTemplateItemId.ToString();
            }
            else
            {
                hfThirdExaminerExamMarkUploadAcaCalSectionId.Value = "0";
                hfThirdExaminerExamMarkUploadExamTemplateItemId.Value = "0";
            }
        }

        protected void ThirdExaminerExamMarkUpload_Click(object sender, EventArgs e)
        {
            MessageView("", "clear");

            try
            {
                int excelLoopLimit = 11;
                int excelLoopLimitsingle = 12;

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (examId > 0)
                {

                    if (FileUploadExamMarkThirdExaminer.HasFile)
                    {

                        if (!string.IsNullOrEmpty(hfThirdExaminerExamMarkUploadAcaCalSectionId.Value) && Convert.ToInt32(hfThirdExaminerExamMarkUploadAcaCalSectionId.Value) > 0)
                        {
                            int acaCalSectionId = Convert.ToInt32(hfThirdExaminerExamMarkUploadAcaCalSectionId.Value);

                            string FileName = Path.GetFileName(FileUploadExamMarkThirdExaminer.FileName);

                            if (FileName.ToUpper().EndsWith("XLS") || FileName.ToUpper().EndsWith("XLSX"))
                            {

                                //string path = Server.MapPath("~/Module/result/Grade_Sheet_Files/");
                                #region Delete Existing File
                                //System.IO.DirectoryInfo di = new DirectoryInfo(path);

                                //foreach (FileInfo file in di.GetFiles())
                                //{
                                //    try
                                //    {

                                //        file.Delete();

                                //    }
                                //    catch (Exception ex)
                                //    {

                                //    }
                                //}
                                #endregion

                                #region Get StudentCourseHistory

                                //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                                List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByExamSetupDetailIdFirstSecondThirdExaminerId(examId, acaCalSectionId, null, null, employeeId);


                                if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                                {
                                    MessageView("No student is found agaist this course or section !!", "fail");
                                    return;
                                }
                                #endregion

                                #region Get ExamTemplateItemID and TemplateExamMark
                                int examTemplateItemId = 0;
                                decimal examTemplateItemExamMark = 0.00M;
                                AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                                if (acs != null)
                                {
                                    List<ExamTemplateItem> etiList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId);
                                    etiList = etiList.Where(x => x.MultipleExaminer == 1).ToList();
                                    if (etiList == null || etiList.Count == 0)
                                    {
                                        MessageView("No Exam Template Item is found agaist this course or section !!", "fail");
                                        return;
                                    }

                                    if (etiList.Where(x => x.SingleQuestionAnswer == true).Count() > 0)
                                    {
                                        excelLoopLimit = 2;
                                        excelLoopLimitsingle = 3;
                                    }

                                    examTemplateItemId = etiList.Where(x => x.MultipleExaminer == 1).FirstOrDefault().ExamTemplateItemId;
                                    examTemplateItemExamMark = etiList.Where(x => x.MultipleExaminer == 1).FirstOrDefault().ExamMark;
                                }
                                else
                                {
                                    MessageView("No Academic Calender Section is found agaist this course or section !!", "fail");
                                    return;
                                }
                                #endregion

                                #region Get Data from Excel

                                int breakpoint = 0;

                                List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();

                                DataTable DtTable = new DataTable();
                                string path = Path.GetFileName(FileUploadExamMarkThirdExaminer.FileName);
                                path = path.Replace(" ", "");

                                String ExcelPath = Server.MapPath("~/Module/result/Grade_Sheet_Files/") + path;

                                try
                                {
                                    if (File.Exists(ExcelPath))
                                    {
                                        System.IO.File.Delete(ExcelPath);
                                    }
                                }
                                catch (Exception ex)
                                {

                                }

                                try
                                {
                                    FileUploadExamMarkThirdExaminer.SaveAs(Server.MapPath("~/Module/result/Grade_Sheet_Files/") + path);

                                    OleDbConnection mycon = new OleDbConnection("Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + ExcelPath + "; Extended Properties=Excel 8.0; Persist Security Info = False");
                                    mycon.Open();
                                    OleDbCommand cmd = new OleDbCommand("select * from [Final$]", mycon);
                                    OleDbDataAdapter da = new OleDbDataAdapter();
                                    da.SelectCommand = cmd;
                                    DataSet ds = new DataSet();
                                    da.Fill(ds);
                                    DtTable = ds.Tables[0];
                                    //GridView1.DataSource = ds.Tables[0];
                                    //GridView1.DataBind();
                                    mycon.Close();

                                    if (DtTable.Rows.Count > 0)
                                    {
                                        DataRow rowT = DtTable.Rows[5];

                                        if (rowT[13].ToString() != "" && Convert.ToInt16(rowT[13].ToString()) == acaCalSectionId)
                                        {
                                            int rowNumber = 0;
                                            #region GetDataFromDataTable
                                            foreach (DataRow row in DtTable.Rows)
                                            {
                                                rowNumber = rowNumber + 1;

                                                if (rowNumber >= 8)
                                                {

                                                    if (row[1].ToString() != "" && row[excelLoopLimitsingle].ToString() != "")
                                                    {
                                                        int questionNumber = 0;
                                                        StudentsCourseMarks astudStudentCourseMarks = new StudentsCourseMarks();
                                                        astudStudentCourseMarks.StudentMarks = new List<Marks>();

                                                        string roll = row[1].ToString() == "" ? "0" : row[1].ToString();

                                                        astudStudentCourseMarks.Roll = roll.Trim();

                                                        var studentCourseHistory = stdCourseHistoryList.FirstOrDefault(x => x.Roll == roll);
                                                        if (studentCourseHistory != null)
                                                        {
                                                            astudStudentCourseMarks.StudentCourseHistoryId = studentCourseHistory.ID;
                                                            astudStudentCourseMarks.ExamTemplateTypeId = examTemplateItemId;
                                                        }
                                                        else
                                                        {
                                                            breakpoint = breakpoint + 1;
                                                            break;
                                                        }

                                                        for (int i = 2; i <= excelLoopLimit; i++)
                                                        {

                                                            Marks marks = new Marks();
                                                            marks.ObtainedMarks = row[i].ToString() == "" ? 0 : Convert.ToDecimal(row[i]);
                                                            astudStudentCourseMarks.TotalObtainedMarks += marks.ObtainedMarks;
                                                            marks.QuestionNumber = ++questionNumber;
                                                            astudStudentCourseMarks.StudentMarks.Add(marks);
                                                        }

                                                        string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                                                        User user = UserManager.GetByLogInId(loginId);
                                                        if (user != null)
                                                        {
                                                            astudStudentCourseMarks.CreateBy = user.User_ID;
                                                            astudStudentCourseMarks.CreatedDate = DateTime.Now;
                                                            astudStudentCourseMarks.ModifiedBy = user.User_ID;
                                                            astudStudentCourseMarks.ModifiedDate = DateTime.Now;
                                                        }


                                                        if (astudStudentCourseMarks.StudentMarks != null && astudStudentCourseMarks.StudentMarks.Count > 0)
                                                        {
                                                            studentsCourseMarksesList.Add(astudStudentCourseMarks);
                                                        }


                                                    }
                                                }
                                            }

                                            #endregion
                                        }
                                        else
                                        {
                                            MessageView("You have Uploaded a Wrong Course Mark File  !!", "fail");
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageView("No Data Found in Excel  !!", "fail");
                                        return;
                                    }
                                }
                                catch (Exception ex)
                                {
                                    //System.IO.File.Delete(ExcelPath);

                                    MessageView("Failed to Read Data from Excel; Error: " + ex.Message.ToString(), "fail");
                                    return;
                                }


                                #region N/A
                                //FileUploadExamMarkThirdExaminer.SaveAs(path + FileUploadExamMarkThirdExaminer.FileName);
                                //string savePath = path + FileUploadExamMarkThirdExaminer.FileName;

                                //Excel.Application xlApp = new Excel.Application();
                                //Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(savePath);
                                //Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                                //Excel.Range xlRange = xlWorksheet.UsedRange;

                                //int rowCount = xlRange.Rows.Count;
                                ////int colCount = xlRange.Columns.Count;
                                //int colCount = 12;
                                //int rCnt;
                                //int cCnt;
                                //int breakpoint = 0;

                                //List<StudentsCourseMarks> studentsCourseMarksesList = new List<StudentsCourseMarks>();
                                //for (rCnt = 9; rCnt <= rowCount; rCnt++)
                                //{
                                //    int questionNumber = 0;
                                //    StudentsCourseMarks astudStudentCourseMarks = new StudentsCourseMarks();
                                //    astudStudentCourseMarks.StudentMarks = new List<Marks>();
                                //    if (((Excel.Range)xlRange.Cells[rCnt, 2]).Value2 != null)
                                //    {
                                //        string roll = ((Excel.Range)xlRange.Cells[rCnt, 2]).Value2.ToString();

                                //        astudStudentCourseMarks.Roll = roll.Trim();

                                //        //int rollDigit = roll.Length;
                                //        //roll = roll.PadLeft(rollDigit + 1, '0');
                                //        var studentCourseHistory = stdCourseHistoryList.FirstOrDefault(x => x.Roll == roll);
                                //        if (studentCourseHistory != null)
                                //        {
                                //            astudStudentCourseMarks.StudentCourseHistoryId = studentCourseHistory.ID;
                                //            astudStudentCourseMarks.ExamTemplateTypeId = examTemplateItemId;
                                //        }
                                //        else
                                //        {
                                //            breakpoint = breakpoint + 1;
                                //            break;
                                //        }

                                //        for (cCnt = 3; cCnt <= colCount; cCnt++)
                                //        {
                                //            if (((Excel.Range)xlRange.Cells[rCnt, cCnt]).Value2 != null)
                                //            {
                                //                //aStudent.StudentMarks.ObtainedMarks = (double) (range.Cells[rCnt, cCnt] as Excel.Range).Value2;
                                //                Marks marks = new Marks();
                                //                marks.ObtainedMarks = (decimal)((Excel.Range)xlRange.Cells[rCnt, cCnt]).Value2;
                                //                astudStudentCourseMarks.TotalObtainedMarks += marks.ObtainedMarks;
                                //                marks.QuestionNumber = ++questionNumber;
                                //                astudStudentCourseMarks.StudentMarks.Add(marks);
                                //            }
                                //            else
                                //            {
                                //                ++questionNumber;
                                //            }
                                //        }
                                //        #region N/A
                                //        //if (conversionMarksTextBox.Visible)
                                //        //{
                                //        //    try
                                //        //    {
                                //        //        if (conversionMarksTextBox.Text != "")
                                //        //        {
                                //        //            string course = ddlAcaCalSection.SelectedValue;
                                //        //            int acaCalSection = Convert.ToInt32(course);
                                //        //            decimal totalMarks = Convert.ToDecimal(conversionMarksTextBox.Text);
                                //        //            AcademicCalenderSection acacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSection);
                                //        //            List<ExamTemplateItem> examList = ExamTemplateItemManager.GetByExamTemplateId(acacalSectionObj.BasicExamTemplateId).Where(x => x.ColumnType == 1).ToList();
                                //        //            var examTemplateItem = examList.First(c => c.ExamTemplateItemId == examTypeItemId);
                                //        //            astudStudentCourseMarks.TotalObtainedMarks =
                                //        //                (examTemplateItem.ExamMark * astudStudentCourseMarks.TotalObtainedMarks) / totalMarks;
                                //        //        }
                                //        //        else
                                //        //        {
                                //        //            lblMsg.Text = "Please enter the exam taken total marks.";
                                //        //            return;

                                //        //        }
                                //        //    }
                                //        //    catch (Exception ex)
                                //        //    {
                                //        //        lblMsg.Text = "Please enter the exam taken total marks.";
                                //        //    }
                                //        //} 
                                //        #endregion
                                //        string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                                //        User user = UserManager.GetByLogInId(loginId);
                                //        if (user != null)
                                //        {
                                //            astudStudentCourseMarks.CreateBy = user.User_ID;
                                //            astudStudentCourseMarks.CreatedDate = DateTime.Now;
                                //            astudStudentCourseMarks.ModifiedBy = user.User_ID;
                                //            astudStudentCourseMarks.ModifiedDate = DateTime.Now;
                                //        }

                                //        if (astudStudentCourseMarks.StudentMarks != null && astudStudentCourseMarks.StudentMarks.Count > 0)
                                //        {
                                //            studentsCourseMarksesList.Add(astudStudentCourseMarks);
                                //        }
                                //    }
                                //}
                                //// END: for (rCnt = 10; rCnt <= rowCount; rCnt++)


                                ////cleanup
                                //GC.Collect();
                                //GC.WaitForPendingFinalizers();

                                ////rule of thumb for releasing com objects:
                                ////  never use two dots, all COM objects must be referenced and released individually
                                ////  ex: [somthing].[something].[something] is bad

                                ////release com objects to fully kill excel process from running in the background
                                //Marshal.ReleaseComObject(xlRange);
                                //Marshal.ReleaseComObject(xlWorksheet);

                                ////close and release
                                //xlWorkbook.Close();
                                //Marshal.ReleaseComObject(xlWorkbook);

                                ////quit and release
                                //xlApp.Quit();
                                //Marshal.ReleaseComObject(xlApp); 
                                #endregion
                                #endregion


                                if (breakpoint > 0)
                                {
                                    MessageView("You have Uploaded a wrong file. Student is not matched against this course or section !!", "fail");
                                    return;
                                }


                                int checkExcelTotalMarkIsGreaterThenExamTemplatemMark = 0;
                                if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                {
                                    foreach (var tData in studentsCourseMarksesList)
                                    {
                                        if (tData.TotalObtainedMarks > examTemplateItemExamMark)
                                        {
                                            checkExcelTotalMarkIsGreaterThenExamTemplatemMark = checkExcelTotalMarkIsGreaterThenExamTemplatemMark + 1;
                                            break;
                                        }
                                    }
                                }

                                if (checkExcelTotalMarkIsGreaterThenExamTemplatemMark > 0)
                                {
                                    MessageView("Excel Total Mark is Greater Than Exam Template Mark !!", "fail");
                                    return;
                                }


                                int isInsertOrUpdateCount = 0;
                                if (studentsCourseMarksesList != null && studentsCourseMarksesList.Count > 0)
                                {
                                    foreach (StudentsCourseMarks scmModel in studentsCourseMarksesList)
                                    {
                                        ExamMarkFirstSecondThirdExaminer emfsteModel = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(scmModel.StudentCourseHistoryId, scmModel.ExamTemplateTypeId);

                                        if (emfsteModel != null)
                                        {
                                            // UPDATE

                                            emfsteModel.ThirdExaminerMark = scmModel.TotalObtainedMarks;
                                            //emfsteModel.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                                            emfsteModel.IsAbsent = false;
                                            #region N/A -- ThirdExaminerMarkSubmissionStatus
                                            //if (!string.IsNullOrEmpty(emfsteModel.ThirdExaminerMarkSubmissionStatus.ToString()))
                                            //{
                                            //    int statusId = Convert.ToInt16(emfsteModel.ThirdExaminerMarkSubmissionStatus);
                                            //    statusId = statusId + 1;
                                            //    emfsteModel.ThirdExaminerMarkSubmissionStatus = statusId;
                                            //}
                                            //else
                                            //{
                                            //    emfsteModel.ThirdExaminerMarkSubmissionStatus = 1;
                                            //}
                                            #endregion
                                            emfsteModel.ModifiedBy = userObj.Id;
                                            emfsteModel.ModifiedDate = DateTime.Now;

                                            bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(emfsteModel);

                                            if (isUpdate == true)
                                            {
                                                isInsertOrUpdateCount++;

                                                InsertOrUpdateExamQuestionMark(scmModel, 3);
                                            }
                                        }
                                        else
                                        {
                                            // INSERT

                                            ExamMarkFirstSecondThirdExaminer model = new ExamMarkFirstSecondThirdExaminer();

                                            model.CourseHistoryId = scmModel.StudentCourseHistoryId;
                                            model.ExamTemplateItemId = scmModel.ExamTemplateTypeId;
                                            model.ThirdExaminerMark = scmModel.TotalObtainedMarks;

                                            //model.ThirdExaminerMarkSubmissionStatus = 1;
                                            //model.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;

                                            model.IsAbsent = false;

                                            model.CreatedBy = userObj.Id;
                                            model.CreatedDate = DateTime.Now;


                                            int examMarkThirdSecondThirdExaminerId = ExamMarkFirstSecondThirdExaminerManager.Insert(model);

                                            if (examMarkThirdSecondThirdExaminerId > 0)
                                            {
                                                isInsertOrUpdateCount++;

                                                InsertOrUpdateExamQuestionMark(scmModel, 3);
                                            }

                                        }
                                    }


                                    if (isInsertOrUpdateCount > 0)
                                    {
                                        MessageView("Data Updated Successfully !!", "success");
                                    }
                                    else
                                    {
                                        MessageView("No Data Updated !!", "fail");
                                    }

                                }
                                else
                                {
                                    //MessageView("No Mark is provided in Excel !!", "fail");
                                    MessageView("Same Mark is already Exist; Please provide a new mark for Upload  !!", "fail");
                                }


                            }
                            else
                            {
                                MessageView("File formate is not correct. File should be (XLSX, XLS)", "fail");
                            }
                        }
                        else
                        {
                            MessageView("Something went wrong while Uploading Third Examiner Exam Mark", "fail");
                        }


                    }
                    else
                    {
                        MessageView("No File is Selected !!", "fail");
                    }
                }
                else
                {
                    MessageView("Please Select Exam !!", "fail");
                }


            }
            catch (Exception ex)
            {
                MessageView("Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnViewThirdExaminer_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtenderThirdExaminer.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examTemplateItemId = int.Parse(commandArgs[2]);

            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            try
            {
                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0 && examId > 0)
                {

                    string param = acaCalSectionId.ToString() + "_" + examSetupId.ToString() + "_" + 3 + "_" + examId.ToString() + "_" + employeeId;
                    string url = "~/Module/result/Report/RPTExamMarkEntry.aspx?val=" + param;
                    Response.Redirect(url, false);


                    #region N/A
                    //#region Get StudentCourseHistory
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                    //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                    //{
                    //    MessageView("No student is found agaist this course or section !!", "fail");
                    //    return;
                    //}
                    //#endregion

                    //#region Get GridView Details
                    //List<ExamMarkGridViewDTO> emgvDTOList = new List<ExamMarkGridViewDTO>();

                    //foreach (var tData in stdCourseHistoryList)
                    //{
                    //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                    //    Student s = StudentManager.GetById(tData.StudentID);
                    //    ExamMarkFirstSecondThirdExaminer em = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(tData.ID, examTemplateItemId);

                    //    if (s != null && em != null)
                    //    {
                    //        model.ExamMarkId = em.ID;
                    //        model.Roll = s.Roll;
                    //        model.Name = s.BasicInfo.FullName;
                    //        try
                    //        {
                    //            if (!string.IsNullOrEmpty(em.ThirdExaminerMark.ToString()))
                    //            {
                    //                model.Mark = Convert.ToDecimal(em.ThirdExaminerMark);
                    //            }
                    //            else
                    //            {
                    //                model.Mark = 0.00M;
                    //            }
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //            model.Mark = 0.00M;
                    //        }

                    //        model.PresentAbsent = Convert.ToBoolean(em.IsAbsent) == true ? "Absent" : "Present";

                    //        emgvDTOList.Add(model);
                    //    }
                    //}

                    //if (emgvDTOList != null && emgvDTOList.Count > 0)
                    //{
                    //    gvExamMarkThirdExaminerView.DataSource = emgvDTOList;
                    //    gvExamMarkThirdExaminerView.DataBind();
                    //}
                    //else
                    //{
                    //    gvExamMarkThirdExaminerView.DataSource = null;
                    //    gvExamMarkThirdExaminerView.DataBind();
                    //}
                    //#endregion


                    //#region Get Shoet Info Details
                    //ExamMarkGridViewShoetInfoDTO emgvsiDTO = ExamMarkFirstSecondThirdExaminerManager.GetExamMarkModalGridViewShoetInfoByCourseHistoryId(acaCalSectionId, examSetupId);
                    //if (emgvsiDTO != null)
                    //{
                    //    lblThirdExaminerModalViewCourse.Text = emgvsiDTO.CourseName + " (" + emgvsiDTO.SectionName + ")";
                    //    lblThirdExaminerModalViewExam.Text = emgvsiDTO.ExamName;
                    //    lblThirdExaminerModalViewTotalStudent.Text = emgvsiDTO.TotalStudentCount.ToString();
                    //    lblThirdExaminerModalViewAbsentCount.Text = emgvsiDTO.AbsentCount.ToString();
                    //}
                    //#endregion 
                    #endregion

                }
                else
                {
                    MessageView("Something went wrong while Loading Third Examiner Exam Mark Data", "fail");
                }
            }
            catch (Exception ex)
            {

                MessageView("Something went wrong while Loading Third Examiner Exam Mark Data; Error: " + ex.Message.ToString(), "fail");
            }
        }
        #endregion

        private decimal CheckPercentageDiff(decimal firstMark, decimal secondMark)
        {
            decimal total = firstMark + secondMark;
            //decimal avgMark = (firstMark + secondMark) / 2;
            decimal difference = Math.Abs(firstMark - secondMark);
            if (difference == 0)
            {
                return difference;
            }
            else
            {
                return (difference / total) * 100;
            }
        }

        #region First, Second, Third Examiner (Submit To Exam Committee)
        protected void btnSubmitToExamCommitteeFirstExaminer_Click(object sender, EventArgs e)
        {
            try
            {
                int updateCount = 0;

                Button btn = (Button)sender;

                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                int examSetupId = int.Parse(commandArgs[1]);
                int examTemplateItemId = int.Parse(commandArgs[2]);

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0 && examId > 0)
                {
                    List<ExamMarkFirstSecondThirdExaminer> examMarkFirstSecondThirdExaminerList = ExamMarkFirstSecondThirdExaminerManager.GetAllByAcaCalSectionIdExamIdExaminerId(acaCalSectionId, examId, employeeId, null, null);

                    if (examMarkFirstSecondThirdExaminerList != null && examMarkFirstSecondThirdExaminerList.Count > 0)
                    {

                        foreach (var tData in examMarkFirstSecondThirdExaminerList)
                        {
                            if ((!string.IsNullOrEmpty(tData.FirstExaminerMark.ToString()) && tData.FirstExaminerMark > 0) &&
                                (!string.IsNullOrEmpty(tData.SecondExaminerMark.ToString()) && tData.SecondExaminerMark > 0))
                            {
                                #region ThirdExaminerStatus (Active/In-Active)
                                decimal percentageDiff = CheckPercentageDiff(Convert.ToDecimal(tData.FirstExaminerMark), Convert.ToDecimal(tData.SecondExaminerMark));
                                if (percentageDiff <= 19.99M)
                                {
                                    tData.ThirdExaminerStatus = 0;
                                }
                                else
                                {
                                    tData.ThirdExaminerStatus = 1;
                                }
                                #endregion

                            }
                            else
                            {
                                tData.ThirdExaminerStatus = 0;
                            }

                            #region FirstExaminerMarkSubmissionStatus
                            if (!string.IsNullOrEmpty(tData.FirstExaminerMarkSubmissionStatus.ToString()))
                            {
                                int statusId = Convert.ToInt16(tData.FirstExaminerMarkSubmissionStatus);
                                statusId = statusId + 1;
                                tData.FirstExaminerMarkSubmissionStatus = statusId;
                            }
                            else
                            {
                                tData.FirstExaminerMarkSubmissionStatus = 1;
                            }
                            #endregion

                            tData.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;

                            tData.ModifiedBy = userObj.Id;
                            tData.ModifiedDate = DateTime.Now;

                            bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(tData);

                            if (isUpdate == true)
                            {
                                updateCount++;
                            }

                        } // END: foreach ()


                        if (updateCount > 0)
                        {
                            LoadGridView(employeeId, 0);
                            MessageView("Submit to Exam Committee (First Examiner) Successful !!", "success");
                        }
                        else
                        {
                            MessageView("Failed; Submit to Exam Committee (First Examiner)!!", "fail");
                        }
                    }
                    else
                    {
                        MessageView("No Student Found !!", "fail");
                    }

                }
                else
                {
                    MessageView("Something went wrong while Submitting To Exam Committee (First Examiner)", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Something went wrong Submitting To Exam Committee (First Examiner); Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnSubmitToExamCommitteeSecondExaminer_Click(object sender, EventArgs e)
        {
            try
            {
                int updateCount = 0;

                Button btn = (Button)sender;

                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                int examSetupId = int.Parse(commandArgs[1]);
                int examTemplateItemId = int.Parse(commandArgs[2]);

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0 && examId > 0)
                {
                    List<ExamMarkFirstSecondThirdExaminer> examMarkFirstSecondThirdExaminerList = ExamMarkFirstSecondThirdExaminerManager.GetAllByAcaCalSectionIdExamIdExaminerId(acaCalSectionId, examId, null, employeeId, null);

                    if (examMarkFirstSecondThirdExaminerList != null && examMarkFirstSecondThirdExaminerList.Count > 0)
                    {
                        foreach (var tData in examMarkFirstSecondThirdExaminerList)
                        {
                            if ((!string.IsNullOrEmpty(tData.FirstExaminerMark.ToString()) && tData.FirstExaminerMark > 0) &&
                                (!string.IsNullOrEmpty(tData.SecondExaminerMark.ToString()) && tData.SecondExaminerMark > 0))
                            {
                                #region ThirdExaminerStatus (Active/In-Active)
                                decimal percentageDiff = CheckPercentageDiff(Convert.ToDecimal(tData.FirstExaminerMark), Convert.ToDecimal(tData.SecondExaminerMark));
                                if (percentageDiff <= 19.99M)
                                {
                                    tData.ThirdExaminerStatus = 0;
                                }
                                else
                                {
                                    tData.ThirdExaminerStatus = 1;
                                }
                                #endregion
                            }
                            else
                            {
                                tData.ThirdExaminerStatus = 0;
                            }

                            #region SecondExaminerMarkSubmissionStatus
                            if (!string.IsNullOrEmpty(tData.SecondExaminerMarkSubmissionStatus.ToString()))
                            {
                                int statusId = Convert.ToInt16(tData.SecondExaminerMarkSubmissionStatus);
                                statusId = statusId + 1;
                                tData.SecondExaminerMarkSubmissionStatus = statusId;
                            }
                            else
                            {
                                tData.SecondExaminerMarkSubmissionStatus = 1;
                            }
                            #endregion

                            tData.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;

                            tData.ModifiedBy = userObj.Id;
                            tData.ModifiedDate = DateTime.Now;

                            bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(tData);

                            if (isUpdate == true)
                            {
                                updateCount++;
                            }

                        }// END: foreach ()


                        if (updateCount > 0)
                        {
                            LoadGridView(employeeId, 0);
                            MessageView("Submit to Exam Committee (Second Examiner) Successful !!", "success");
                        }
                        else
                        {
                            MessageView("Failed; Submit to Exam Committee (Second Examiner)!!", "fail");
                        }
                    }
                    else
                    {
                        MessageView("No Student Found !!", "fail");
                    }
                }
                else
                {
                    MessageView("Something went wrong while Submitting To Exam Committee (Second Examiner)", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Something went wrong Submitting To Exam Committee (Second Examiner); Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnSubmitToExamCommitteeThirdExaminer_Click(object sender, EventArgs e)
        {
            try
            {
                int updateCount = 0;

                Button btn = (Button)sender;

                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                int examSetupId = int.Parse(commandArgs[1]);
                int examTemplateItemId = int.Parse(commandArgs[2]);

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (acaCalSectionId > 0 && examSetupId > 0 && examTemplateItemId > 0)
                {
                    List<ExamMarkFirstSecondThirdExaminer> examMarkFirstSecondThirdExaminerList = ExamMarkFirstSecondThirdExaminerManager.GetAllByAcaCalSectionIdExamIdExaminerId(acaCalSectionId, examId, null, null, employeeId);

                    if (examMarkFirstSecondThirdExaminerList != null && examMarkFirstSecondThirdExaminerList.Count > 0)
                    {
                        foreach (var tData in examMarkFirstSecondThirdExaminerList)
                        {
                            #region N/A -- ThirdExaminerStatus (Active/In-Active)
                            //decimal percentageDiff = CheckPercentageDiff(Convert.ToDecimal(tData.FirstExaminerMark), Convert.ToDecimal(tData.SecondExaminerMark));
                            //if (percentageDiff <= 19.99M)
                            //{
                            //    tData.ThirdExaminerStatus = 0;
                            //}
                            //else
                            //{
                            //    tData.ThirdExaminerStatus = 1;
                            //}
                            #endregion

                            #region ThirdExaminerMarkSubmissionStatus
                            if (!string.IsNullOrEmpty(tData.ThirdExaminerMarkSubmissionStatus.ToString()))
                            {
                                int statusId = Convert.ToInt16(tData.ThirdExaminerMarkSubmissionStatus);
                                statusId = statusId + 1;
                                tData.ThirdExaminerMarkSubmissionStatus = statusId;
                            }
                            else
                            {
                                tData.ThirdExaminerMarkSubmissionStatus = 1;
                            }
                            #endregion

                            tData.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;

                            tData.ModifiedBy = userObj.Id;
                            tData.ModifiedDate = DateTime.Now;

                            bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(tData);

                            if (isUpdate == true)
                            {
                                updateCount++;
                            }
                        }// END: foreach ()


                        if (updateCount > 0)
                        {
                            LoadGridView(employeeId, 0);
                            MessageView("Submit to Exam Committee (Third Examiner) Successful !!", "success");
                        }
                        else
                        {
                            MessageView("Failed; Submit to Exam Committee (Third Examiner)!!", "fail");
                        }
                    }
                    else
                    {
                        MessageView("No Student Found !!", "fail");
                    }
                }
                else
                {
                    MessageView("Something went wrong while Submitting To Exam Committee (Third Examiner)", "fail");
                }
            }
            catch (Exception ex)
            {
                MessageView("Something went wrong Submitting To Exam Committee (Third Examiner); Error: " + ex.Message.ToString(), "fail");
            }
        }
        #endregion

        #region N/A
        //protected void btnLoad_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (FileUpload1.HasFile == false)
        //        {
        //            lblMsg.Text = "Please select a File and try again!";
        //        }
        //        else
        //        {
        //            GridView1.DataSource = null;
        //            GridView1.DataBind();
        //            GridView2.DataSource = null;
        //            GridView2.DataBind();
        //            lblMsg.Text = "";
        //            string saveFolder = "~/Upload/";
        //            string filename = FileUpload1.FileName;
        //            string filePath = Path.Combine(saveFolder, FileUpload1.FileName);
        //            string excelpath = Server.MapPath(filePath);

        //            if (File.Exists(excelpath))
        //            {
        //                System.IO.File.Delete(excelpath);
        //                FileUpload1.SaveAs(excelpath);
        //            }
        //            else
        //            {
        //                FileUpload1.SaveAs(excelpath);
        //            }

        //            try
        //            {
        //                System.Data.OleDb.OleDbConnection MyConnection;
        //                System.Data.DataTable DtTable;
        //                System.Data.OleDb.OleDbDataAdapter MyCommand;
        //                MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + excelpath + ";Extended Properties=Excel 12.0 xml;");
        //                MyCommand = new System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
        //                MyCommand.TableMappings.Add("Table", "TestTable");
        //                DtTable = new System.Data.DataTable();
        //                MyCommand.Fill(DtTable);
        //                //PopulateData(DtTable, excelpath);
        //                Session["DataTableList"] = DtTable;
        //                //GridView1.DataSource = DtTable;
        //                //GridView1.DataBind();
        //                MyConnection.Close();
        //                if (DtTable.Rows.Count > 0)
        //                {
        //                    //if (DtTable.Rows.Count > 500)
        //                    //{
        //                    // Label1.Visible = false;
        //                    // lblMsg.Text = "Load Failed.You can upload maximum 500 result data at a time.";
        //                    // FileUpload1.Visible = true;
        //                    // btnLoad.Visible = true;
        //                    // btnResultSave.Visible = false;
        //                    // System.IO.File.Delete(excelpath);
        //                    //}
        //                    //else
        //                    //{
        //                    Label2.Visible = true;
        //                    GridView1.DataSource = DtTable;
        //                    GridView1.DataBind();
        //                    Label1.Visible = false;
        //                    lblMsg.Text = "Data Loaded Successfully,Please click Insert Result Button.";
        //                    FileUpload1.Visible = false;
        //                    btnLoad.Visible = false;
        //                    btnResultSave.Visible = true;
        //                    System.IO.File.Delete(excelpath);
        //                    //}

        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                lblMsg.ForeColor = Color.Red;
        //                lblMsg.Text = ex.ToString();
        //                System.IO.File.Delete(excelpath);
        //            }
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        lblMsg.ForeColor = Color.Red;
        //        lblMsg.Text = ex.ToString();
        //    }

        //} 
        #endregion
    }
}