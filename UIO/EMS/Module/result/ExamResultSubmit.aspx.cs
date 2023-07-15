using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.result
{
    public partial class ExamResultSubmit : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;
                lblExamMark.Text = null;

                ucProgram.LoadDropdownWithUserAccess(userObj.Id);
                LoadCourse();
                LoadContinousExam(0);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
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
                ddlYearNo.DataTextField = "YearNo";
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
                ddlSemesterNo.DataTextField = "SemesterNo";
                ddlSemesterNo.DataValueField = "SemesterNo";

                ddlSemesterNo.DataSource = semesterList;
                ddlSemesterNo.DataBind();

            }
        }

        protected void ddlYearNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //lblMsg.Text = string.Empty;
            //LoadCourse();

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
            //LoadCourse();

            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                loadExamDropdown(programId, yearNo, semesterNo);
            }
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

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            string course = ddlCourse.SelectedValue;
            string[] courseVersion = course.Split('_');

            int courseId = Convert.ToInt32(courseVersion[0]);
            int versionId = Convert.ToInt32(courseVersion[1]);
            int acaCalSection = Convert.ToInt32(courseVersion[2]);

            LoadContinousExam(acaCalSection);
            //LoadAcaCalSection(courseId, versionId, acaCalId);
        }

        protected void ddlExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                LoadCourse();
            }
            
        }

        protected void LoadContinousExam(int acaCalSection)
        {
            try
            {
                lblMsg.Text = string.Empty;
                ddlContinousExam.Items.Clear();
                ddlContinousExam.Items.Add(new ListItem("-Select Exam-", "0"));
                ddlContinousExam.AppendDataBoundItems = true;
                AcademicCalenderSection acacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSection);
                if (acacalSectionObj != null)
                {
                    List<ExamTemplateItem> examList = ExamTemplateItemManager.GetByExamTemplateId(acacalSectionObj.BasicExamTemplateId).Where(x => x.ColumnType == 1 && x.MultipleExaminer != 1).ToList();
                    ddlContinousExam.DataSource = examList;
                    ddlContinousExam.DataValueField = "ExamTemplateItemId";
                    ddlContinousExam.DataTextField = "ExamName";
                    ddlContinousExam.DataBind();
                }
            }
            catch { }
            finally { }
        }

        protected void LoadCourse()
        {
            try
            {
                lblMsg.Text = string.Empty;
                ddlCourse.Items.Clear();
                ddlCourse.Items.Add(new ListItem("-Select Course-", "0"));
                ddlCourse.AppendDataBoundItems = true;

                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                    List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetByProgramIdYearNoSemesterNoExamId(programId, yearNo, semesterNo, examId);
                    User user = UserManager.GetByLogInId(userObj.LogInID);
                    Role userRole = RoleManager.GetById(user.RoleID);
                    if (user.Person != null)
                    {
                        Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);

                        if (empObj != null && userRole.RoleName != "Admin")
                        {
                            acaCalSectionList = acaCalSectionList.Where(x => x.TeacherOneID == empObj.EmployeeID || x.TeacherThreeID == empObj.EmployeeID || x.TeacherTwoID == empObj.EmployeeID).ToList();
                        }
                    }

                    if (acaCalSectionList.Count > 0 && acaCalSectionList != null)
                    {
                        acaCalSectionList = acaCalSectionList.OrderBy(x => x.Course.Title).ToList();
                        foreach (AcademicCalenderSection acaCalSec in acaCalSectionList)
                        {
                            ddlCourse.Items.Add(new ListItem(acaCalSec.Course.Title + " : " + acaCalSec.Course.FormalCode + " : " + acaCalSec.Course.Credits + " (" + acaCalSec.SectionName + ")", acaCalSec.Course.CourseID + "_" + acaCalSec.Course.VersionID + "_" + acaCalSec.AcaCal_SectionID));
                        }
                    }
                }
                else 
                {
                    lblMsg.Text = "Please select program, yaer and semester.";
                }
            }
            catch { }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersion[2]);
                int examTemplateItemId = Convert.ToInt32(ddlContinousExam.SelectedValue);

                ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                if (examTemplateItemObj != null)
                {
                    lblExamMark.Text = Convert.ToString(examTemplateItemObj.ExamMark);
                    lblExamTemplateBasicItemId.Text = Convert.ToString(examTemplateItemObj.ExamTemplateItemId);
                    List<ExamMarkDTO> studentList = ExamMarkManager.GetByExamMarkDtoByParameter(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
                    ResultEntryGrid.DataSource = studentList;
                    ResultEntryGrid.DataBind();
                    ResultEntryGrid.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void ResultEntryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ResultSubmit")
                {
                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    TextBox mark = (TextBox)row.FindControl("txtMark");
                    CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");

                    int examTemplateItemId = Convert.ToInt32(lblExamTemplateBasicItemId.Text);
                    ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
                    if (examTemplateItemId > 0)
                    {
                        examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                    }

                    
                    int courseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                    ExamMark studentResult = new ExamMark();
                    studentResult.CourseHistoryId = courseHistoryId;
                    studentResult.Mark = Convert.ToDecimal(mark.Text);
                    if (chkStatus.Checked)
                    {
                        studentResult.Attribute1 = "Absent";
                    }
                    else
                    {
                        studentResult.Attribute1 = "Present";
                    }
                    studentResult.ExamTemplateItemId = examTemplateItemId;
                    studentResult.CreateBy = userObj.Id;
                    studentResult.CreatedDate = DateTime.Now;
                    studentResult.ModifiedBy = userObj.Id;
                    studentResult.ModifiedDate = DateTime.Now;

                    if (CheckMark(Convert.ToDecimal(studentResult.Mark), Convert.ToDecimal(examTemplateItemObj.ExamMark)))
                    {
                        ExamMark newStudentResult = ExamMarkManager.GetStudentMarkByExamId(studentResult.CourseHistoryId, studentResult.ExamTemplateItemId);
                        if (Convert.ToString(studentResult.Mark) != string.Empty)
                        {
                            if (newStudentResult != null)
                            {
                                //if (studentResult.Id == newStudentResult.Id)
                                //{
                                newStudentResult.Mark = Convert.ToDecimal(mark.Text);
                                if (chkStatus.Checked)
                                {
                                    newStudentResult.Attribute1 = "Absent";
                                }
                                else
                                {
                                    newStudentResult.Attribute1 = "Present";
                                }
                                ExamMarkManager.Update(newStudentResult);
                                lblMsg.Text = "Student marks updated.";
                                //}
                            }
                            else
                            {
                                ExamMarkManager.Insert(studentResult);
                                lblMsg.Text = "Student marks inserted.";
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Student marks could not be empty.";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Student marks could not be more then exam mark.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private decimal GetStudentMark(int lblCourseHistoryId)
        {
            decimal studentMark = 0;
            for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
            {
                GridViewRow row = ResultEntryGrid.Rows[i];
                Label courseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                TextBox mark = (TextBox)row.FindControl("txtMark");
                if (Convert.ToString(mark) != string.Empty)
                {
                    if (Convert.ToInt32(courseHistoryId.Text) == lblCourseHistoryId)
                    {
                        studentMark = Convert.ToDecimal(mark.Text);
                        break;
                    }
                }

            }
            return studentMark;
        }

        protected void SubmitAllMarkButton_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                int examTemplateItemId = Convert.ToInt32(lblExamTemplateBasicItemId.Text);
                ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
                if (examTemplateItemId > 0)
                {
                    examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                }

                if (examTemplateItemObj != null)
                {
                    if (CheckAllMark(examTemplateItemObj.ExamMark))
                    {
                        for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                        {
                            GridViewRow row = ResultEntryGrid.Rows[i];
                            Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                            TextBox mark = (TextBox)row.FindControl("txtMark");
                            CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");


                            if (Convert.ToString(mark.Text) != string.Empty)
                            {
                                ExamMark studentResult = new ExamMark();

                                studentResult.CourseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                                studentResult.Mark = Convert.ToDecimal(GetStudentMark(Convert.ToInt32(lblCourseHistoryId.Text)));
                                if (chkStatus.Checked)
                                {
                                    studentResult.Attribute1 = "Absent";
                                }
                                else
                                {
                                    studentResult.Attribute1 = "Present";
                                }
                                studentResult.ExamTemplateItemId = examTemplateItemId;
                                studentResult.CreateBy = userObj.Id;
                                studentResult.CreatedDate = DateTime.Now;
                                studentResult.ModifiedBy = userObj.Id;
                                studentResult.ModifiedDate = DateTime.Now;
                                ExamMark newStudentResult = ExamMarkManager.GetStudentMarkByExamId(studentResult.CourseHistoryId, studentResult.ExamTemplateItemId);

                                if (Convert.ToString(studentResult.Mark) != string.Empty)
                                {
                                    //if (CheckMark(Convert.ToDecimal(studentResult.Mark), Convert.ToDecimal(examTemplateItemObj.ExamMark)))
                                    //{
                                    if (newStudentResult != null)
                                    {
                                        newStudentResult.Mark = Convert.ToDecimal(GetStudentMark(Convert.ToInt32(Convert.ToInt32(lblCourseHistoryId.Text))));
                                        if (chkStatus.Checked)
                                        {
                                            newStudentResult.Attribute1 = "Absent";
                                        }
                                        else
                                        {
                                            newStudentResult.Attribute1 = "Present";
                                        }
                                        ExamMarkManager.Update(newStudentResult);
                                        lblMsg.Text = "Student marks updated successfully.";
                                    }
                                    else
                                    {
                                        ExamMarkManager.Insert(studentResult);
                                        lblMsg.Text = "Student marks submitted successfully.";
                                    }
                                }
                                else
                                {
                                    lblMsg.Text = "Student marks could not be empty.";
                                }
                            }
                            else
                            {
                                //    lblMsg.Text = "Student marks could not be empty.";
                            }
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Student marks could not be more then exam mark.";
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private bool CheckMark(decimal studentMark, decimal examMark)
        {
            double stuMark = Convert.ToDouble(studentMark);
            double exmMark = Convert.ToDouble(examMark);

            if (stuMark > exmMark)
            {
                return false;
            }
            if (stuMark >= 0 && stuMark <= exmMark)
            {
                return true;
            }
            else { return false; }
        }

        private bool CheckAllMark(decimal examMark) 
        {
            try
            {
                bool result = true;
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label courseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    TextBox mark = (TextBox)row.FindControl("txtMark");
                    decimal studentMark = GetStudentMark(Convert.ToInt32(courseHistoryId.Text));
                    
                    bool chkResult = CheckMark(studentMark, examMark);
                    if (chkResult == false)
                    {
                        result = false;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                int index = row.RowIndex;
                TextBox txtMark = (TextBox)ResultEntryGrid.Rows[index].FindControl("txtMark");
                CheckBox cb1 = (CheckBox)ResultEntryGrid.Rows[index].FindControl("chkStatus");

                if (cb1.Checked)
                {
                    txtMark.Text = "0.00";
                    txtMark.Enabled = false;
                }
                else
                {
                    txtMark.Enabled = true;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ddlContinousExam_SelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                lblMsg.Text = string.Empty;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersion[2]);
                int examTemplateItemId = Convert.ToInt32(ddlContinousExam.SelectedValue);

                ExamTemplateItem examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                if (examTemplateItemObj != null)
                {
                    lblExamMark.Text = Convert.ToString(examTemplateItemObj.ExamMark);
                    lblExamTemplateBasicItemId.Text = Convert.ToString(examTemplateItemObj.ExamTemplateItemId);
                    List<ExamMarkDTO> studentList = ExamMarkManager.GetByExamMarkDtoByParameter(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
                    ResultEntryGrid.DataSource = studentList;
                    ResultEntryGrid.DataBind();
                    ResultEntryGrid.Visible = true;
                }
                else
                {
                    lblExamMark.Text = string.Empty;
                    ResultEntryGrid.DataSource = null;
                    ResultEntryGrid.DataBind();
                    ResultEntryGrid.EmptyDataText = "No data is found.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }

        }

        protected void ResultEntryGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //GridViewRow row = (GridViewRow)e.NamingContainer;
                    //int index = row.RowIndex;
                    Label statusLabel = (Label)e.Row.FindControl("statusLabel");
                    if (statusLabel != null)
                    {
                        if (statusLabel.Text.ToString() == "Absent")
                        {
                            CheckBox cb1 = (CheckBox)e.Row.FindControl("chkStatus");
                            cb1.Checked = true;
                        }

                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       

 

        
    }
}