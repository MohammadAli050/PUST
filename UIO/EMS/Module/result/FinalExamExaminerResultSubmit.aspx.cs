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
    public partial class FinalExamExaminerResultSubmit : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        int employeeId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            UserInPerson userInPerson = UserInPersonManager.GetById(userObj.Id);
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
                lblExamMark.Text = null;

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
                LoadCourse();
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
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadCourse();
                }
            }
            catch (Exception)
            {
                throw;
            }
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
                int examinerTypeId = Convert.ToInt32(ddlExaminerType.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    BussinessObject.UIUMSUser userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                    List<AcademicCalenderSection> acaCalSectionList = AcademicCalenderSectionManager.GetByProgramIdYearNoSemesterNoExamId(programId, yearNo, semesterNo, examId);
                    
                    User user = UserManager.GetByLogInId(userObj.LogInID);
                    Role userRole = RoleManager.GetById(user.RoleID);

                    if (user.Person != null)
                    {
                        Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);

                        if (empObj != null && (userRole.RoleName != "BRUR Admin" || userRole.RoleName != "ESCL"))
                        {
                            acaCalSectionList = AcademicCalenderSectionManager.GetByExaminerIdYearNoSemesterNoExamId(empObj.EmployeeID, yearNo, semesterNo, examId, examinerTypeId);
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

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            ddlQuestionNo.SelectedValue = Convert.ToString("1");

            try
            {
                btnLoad_Click(null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ddlExaminerType_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadCourse();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersion[2]);
                //int examTemplateItemId = Convert.ToInt32(ddlContinousExam.SelectedValue);
                int examinerTypeId = Convert.ToInt32(ddlExaminerType.SelectedValue);

                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (acaCalSectionId > 0 && examinerTypeId > 0 && examId > 0)
                {
                    Course courseObj = CourseManager.GetByCourseIdVersionId(courseId, versionId);
                    if ((courseObj.TypeDefinitionID != 3) || (courseObj.TypeDefinitionID == 3 && examinerTypeId == 1))
                    {
                        AcademicCalenderSection acacalSectionObj = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                        ExamTemplateItem examTemplateItem = ExamTemplateItemManager.GetByExamTemplateId(acacalSectionObj.BasicExamTemplateId).Where(x => x.ColumnType == 1 && x.MultipleExaminer == 1).FirstOrDefault();
                        if (examTemplateItem != null)
                        {
                            lblExamMark.Text = Convert.ToString(examTemplateItem.ExamMark);
                            lblExamTemplateItemId.Text = Convert.ToString(examTemplateItem.ExamTemplateItemId);
                            lblExaminerType.Text = Convert.ToString(examinerTypeId);
                            LoadResultGrid(acaCalSectionId, examinerTypeId, examId);

                            bool isSingleQuestion = Convert.ToBoolean(examTemplateItem.SingleQuestionAnswer);

                            GridRebind(isSingleQuestion);
                        }
                    }
                    else 
                    {
                        lblMsg.Text = "For Viva course Second and Thrid examiner mark entry not possible.";
                        lblExamMark.Text = string.Empty;
                        ResultEntryGrid.DataSource = null;
                        ResultEntryGrid.DataBind();
                        ResultEntryGrid.EmptyDataText = "No data is found.";
                        pnlTotalMark.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void GridRebind(bool isSingleQuestion)
        {
            if (isSingleQuestion == false)
            {
                #region Show All 10 Question
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label lblQuestionAnsweredCount = (Label)row.FindControl("lblQuestionAnsweredCount");

                    TextBox txtQustion1Mark = (TextBox)row.FindControl("txtQustion1Mark");
                    TextBox txtQustion2Mark = (TextBox)row.FindControl("txtQustion2Mark");
                    TextBox txtQustion3Mark = (TextBox)row.FindControl("txtQustion3Mark");
                    TextBox txtQustion4Mark = (TextBox)row.FindControl("txtQustion4Mark");
                    TextBox txtQustion5Mark = (TextBox)row.FindControl("txtQustion5Mark");
                    TextBox txtQustion6Mark = (TextBox)row.FindControl("txtQustion6Mark");
                    TextBox txtQustion7Mark = (TextBox)row.FindControl("txtQustion7Mark");
                    TextBox txtQustion8Mark = (TextBox)row.FindControl("txtQustion8Mark");
                    TextBox txtQustion9Mark = (TextBox)row.FindControl("txtQustion9Mark");
                    TextBox txtQustion10Mark = (TextBox)row.FindControl("txtQustion10Mark");

                    int answeredCounter = 0;

                    decimal? qustion1Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion1Mark.Text))
                    {
                        qustion1Mark = Convert.ToDecimal(txtQustion1Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion2Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion2Mark.Text))
                    {
                        qustion2Mark = Convert.ToDecimal(txtQustion2Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion3Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion3Mark.Text))
                    {
                        qustion3Mark = Convert.ToDecimal(txtQustion3Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion4Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion4Mark.Text))
                    {
                        qustion4Mark = Convert.ToDecimal(txtQustion4Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion5Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion5Mark.Text))
                    {
                        qustion5Mark = Convert.ToDecimal(txtQustion5Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion6Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion6Mark.Text))
                    {
                        qustion6Mark = Convert.ToDecimal(txtQustion6Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion7Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion7Mark.Text))
                    {
                        qustion7Mark = Convert.ToDecimal(txtQustion7Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion8Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion8Mark.Text))
                    {
                        qustion8Mark = Convert.ToDecimal(txtQustion8Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion9Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion9Mark.Text))
                    {
                        qustion9Mark = Convert.ToDecimal(txtQustion9Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }
                    decimal? qustion10Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion10Mark.Text))
                    {
                        qustion10Mark = Convert.ToDecimal(txtQustion10Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }

                    lblQuestionAnsweredCount.Text = Convert.ToString(answeredCounter);
                } 
                #endregion
            }
            else
            {
                #region Show Only 1 Question
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label lblQuestionAnsweredCount = (Label)row.FindControl("lblQuestionAnsweredCount");

                    TextBox txtQustion1Mark = (TextBox)row.FindControl("txtQustion1Mark");
                    TextBox txtQustion2Mark = (TextBox)row.FindControl("txtQustion2Mark");
                    TextBox txtQustion3Mark = (TextBox)row.FindControl("txtQustion3Mark");
                    TextBox txtQustion4Mark = (TextBox)row.FindControl("txtQustion4Mark");
                    TextBox txtQustion5Mark = (TextBox)row.FindControl("txtQustion5Mark");
                    TextBox txtQustion6Mark = (TextBox)row.FindControl("txtQustion6Mark");
                    TextBox txtQustion7Mark = (TextBox)row.FindControl("txtQustion7Mark");
                    TextBox txtQustion8Mark = (TextBox)row.FindControl("txtQustion8Mark");
                    TextBox txtQustion9Mark = (TextBox)row.FindControl("txtQustion9Mark");
                    TextBox txtQustion10Mark = (TextBox)row.FindControl("txtQustion10Mark");

                    int answeredCounter = 0;

                    decimal? qustion1Mark = null;
                    if (!string.IsNullOrEmpty(txtQustion1Mark.Text))
                    {
                        qustion1Mark = Convert.ToDecimal(txtQustion1Mark.Text);
                        answeredCounter = answeredCounter + 1;
                    }

                    txtQustion2Mark.Enabled = false;
                    txtQustion3Mark.Enabled = false;
                    txtQustion4Mark.Enabled = false;
                    txtQustion5Mark.Enabled = false;
                    txtQustion6Mark.Enabled = false;
                    txtQustion7Mark.Enabled = false;
                    txtQustion8Mark.Enabled = false;
                    txtQustion9Mark.Enabled = false;
                    txtQustion10Mark.Enabled = false;

                    #region N/A
                    //decimal? qustion2Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion2Mark.Text))
                    //{
                    //    qustion2Mark = Convert.ToDecimal(txtQustion2Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion3Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion3Mark.Text))
                    //{
                    //    qustion3Mark = Convert.ToDecimal(txtQustion3Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion4Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion4Mark.Text))
                    //{
                    //    qustion4Mark = Convert.ToDecimal(txtQustion4Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion5Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion5Mark.Text))
                    //{
                    //    qustion5Mark = Convert.ToDecimal(txtQustion5Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion6Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion6Mark.Text))
                    //{
                    //    qustion6Mark = Convert.ToDecimal(txtQustion6Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion7Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion7Mark.Text))
                    //{
                    //    qustion7Mark = Convert.ToDecimal(txtQustion7Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion8Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion8Mark.Text))
                    //{
                    //    qustion8Mark = Convert.ToDecimal(txtQustion8Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion9Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion9Mark.Text))
                    //{
                    //    qustion9Mark = Convert.ToDecimal(txtQustion9Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //}
                    //decimal? qustion10Mark = null;
                    //if (!string.IsNullOrEmpty(txtQustion10Mark.Text))
                    //{
                    //    qustion10Mark = Convert.ToDecimal(txtQustion10Mark.Text);
                    //    answeredCounter = answeredCounter + 1;
                    //} 
                    #endregion

                    lblQuestionAnsweredCount.Text = Convert.ToString(answeredCounter);
                }
                #endregion
            }
        }

        private void LoadResultGrid(int acaCalSectionId, int examinerTypeId, int examId)
        {
            List<ExaminerQuestionWiseMarkDTO> studentList = ExamMarkFirstSecondThirdExaminerManager.GetByFinalExamQuestionwiseMarkBySectionIdExaminerType(acaCalSectionId, examinerTypeId, examId, employeeId);
            if (studentList != null && studentList.Count > 0)
            {
                ResultEntryGrid.DataSource = studentList;
                ResultEntryGrid.DataBind();
                ResultEntryGrid.Visible = true;
                pnlTotalMark.Visible = true;
            }
            else
            {
                lblExamMark.Text = string.Empty;
                ResultEntryGrid.DataSource = null;
                ResultEntryGrid.DataBind();
                ResultEntryGrid.EmptyDataText = "No data is found.";
                pnlTotalMark.Visible = false;
            }
            //GridRebind();
        }

        protected void btnSubmitFinalExamMark_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int examinerType = Convert.ToInt32(lblExaminerType.Text);
                int needToBeAnswered = Convert.ToInt32(ddlQuestionNo.SelectedValue);
                int asnwerMoreCounter = 0;
                int examMarkMoreCounter = 0;
                int markSavedStudentCounter = 0;
                string asnwerMoreCounterErrorMessage = null;
                string examMarkMoreCounterErrorMessage = null;
                string markSavedMessage = null;
                decimal examMark = Convert.ToDecimal(lblExamMark.Text);

                if (CheckAllExamMark())
                {

                    for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                    {
                        GridViewRow row = ResultEntryGrid.Rows[i];
                        Label lblStudentId = (Label)row.FindControl("lblStudentId");
                        Label lblStudentRoll = (Label)row.FindControl("lblStudentRoll");
                        Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                        Label lblExamMarkFirstSecondThirdExaminerId = (Label)row.FindControl("lblExamMarkFirstSecondThirdExaminerId");
                        TextBox txtQustion1Mark = (TextBox)row.FindControl("txtQustion1Mark");
                        TextBox txtQustion2Mark = (TextBox)row.FindControl("txtQustion2Mark");
                        TextBox txtQustion3Mark = (TextBox)row.FindControl("txtQustion3Mark");
                        TextBox txtQustion4Mark = (TextBox)row.FindControl("txtQustion4Mark");
                        TextBox txtQustion5Mark = (TextBox)row.FindControl("txtQustion5Mark");
                        TextBox txtQustion6Mark = (TextBox)row.FindControl("txtQustion6Mark");
                        TextBox txtQustion7Mark = (TextBox)row.FindControl("txtQustion7Mark");
                        TextBox txtQustion8Mark = (TextBox)row.FindControl("txtQustion8Mark");
                        TextBox txtQustion9Mark = (TextBox)row.FindControl("txtQustion9Mark");
                        TextBox txtQustion10Mark = (TextBox)row.FindControl("txtQustion10Mark");
                    
                        int answeredCounter = 0;

                        decimal? qustion1Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion1Mark.Text))
                        {
                            qustion1Mark = Convert.ToDecimal(txtQustion1Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion2Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion2Mark.Text))
                        {
                            qustion2Mark = Convert.ToDecimal(txtQustion2Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion3Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion3Mark.Text))
                        {
                            qustion3Mark = Convert.ToDecimal(txtQustion3Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion4Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion4Mark.Text))
                        {
                            qustion4Mark = Convert.ToDecimal(txtQustion4Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion5Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion5Mark.Text))
                        {
                            qustion5Mark = Convert.ToDecimal(txtQustion5Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion6Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion6Mark.Text))
                        {
                            qustion6Mark = Convert.ToDecimal(txtQustion6Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion7Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion7Mark.Text))
                        {
                            qustion7Mark = Convert.ToDecimal(txtQustion7Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion8Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion8Mark.Text))
                        {
                            qustion8Mark = Convert.ToDecimal(txtQustion8Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion9Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion9Mark.Text))
                        {
                            qustion9Mark = Convert.ToDecimal(txtQustion9Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }
                        decimal? qustion10Mark = null;
                        if (!string.IsNullOrEmpty(txtQustion10Mark.Text))
                        {
                            qustion10Mark = Convert.ToDecimal(txtQustion10Mark.Text);
                            answeredCounter = answeredCounter + 1;
                        }

                        if (answeredCounter <= needToBeAnswered)
                        {
                            decimal totalMark = Convert.ToDecimal(Convert.ToDecimal(qustion1Mark) + Convert.ToDecimal(qustion2Mark) + Convert.ToDecimal(qustion3Mark) + Convert.ToDecimal(qustion4Mark) + Convert.ToDecimal(qustion5Mark) + Convert.ToDecimal(qustion6Mark) + Convert.ToDecimal(qustion7Mark) + Convert.ToDecimal(qustion8Mark) + Convert.ToDecimal(qustion9Mark) + Convert.ToDecimal(qustion10Mark));

                            if (totalMark <= examMark)
                            {
                                int courseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                                int examTemplateItemId = Convert.ToInt32(lblExamTemplateItemId.Text);
                                int studentId = Convert.ToInt32(lblStudentId.Text);
                                ExamMarkFirstSecondThirdExaminer emfsteModel = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(courseHistoryId, examTemplateItemId);

                                InsertOrUpdateFirstExaminerTotalMark(emfsteModel, courseHistoryId, examTemplateItemId, totalMark, examinerType);

                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 1, qustion1Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 2, qustion2Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 3, qustion3Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 4, qustion4Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 5, qustion5Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 6, qustion6Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 7, qustion7Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 8, qustion8Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 9, qustion9Mark, examinerType);
                                InsertOrUpdateExamQuestionMark(studentId, courseHistoryId, examTemplateItemId, 10, qustion10Mark, examinerType);
                            
                                markSavedStudentCounter = markSavedStudentCounter + 1;
                                //markSavedMessage = markSavedMessage + Convert.ToString(lblStudentRoll.Text) + ", ";
                            }
                            else 
                            {
                                //examMarkMoreCounter = examMarkMoreCounter + 1;
                                //examMarkMoreCounterErrorMessage = examMarkMoreCounterErrorMessage + Convert.ToString(lblStudentRoll.Text) + ", ";
                            }
                        }
                        else 
                        {
                            //asnwerMoreCounter = asnwerMoreCounter + 1;
                            //asnwerMoreCounterErrorMessage = asnwerMoreCounterErrorMessage + Convert.ToString(lblStudentRoll.Text) + ", ";
                        }
                    }
                    //btnLoad_Click(null, null);
                    //if(asnwerMoreCounter > 0)
                    //{
                    //    asnwerMoreCounterErrorMessage = "Student answer count more than question need to answered for following Student Id :  " + asnwerMoreCounterErrorMessage;
                    //}

                    //if (examMarkMoreCounter > 0)
                    //{
                    //    examMarkMoreCounterErrorMessage = "Student total mark more than exam mark for following Student Id :  " + examMarkMoreCounterErrorMessage;
                    //}

                    if (markSavedStudentCounter > 0)
                    {
                        markSavedMessage = "Exam mark submitted successfully.";
                    }
                    //else
                    //{
                    //    markSavedMessage = "Exam mark could not submitted for following Student Id : " + markSavedMessage;
                    //}
                }

                lblMsg.Text = markSavedMessage;
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private bool CheckAllExamMark()
        {
            int needToBeAnswered = Convert.ToInt32(ddlQuestionNo.SelectedValue);
            decimal examMark = Convert.ToDecimal(lblExamMark.Text);
            bool output = true;

            for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
            {
                GridViewRow row = ResultEntryGrid.Rows[i];
                Label lblStudentId = (Label)row.FindControl("lblStudentId");
                Label lblStudentRoll = (Label)row.FindControl("lblStudentRoll");
                Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                Label lblExamMarkFirstSecondThirdExaminerId = (Label)row.FindControl("lblExamMarkFirstSecondThirdExaminerId");
                TextBox txtQustion1Mark = (TextBox)row.FindControl("txtQustion1Mark");
                TextBox txtQustion2Mark = (TextBox)row.FindControl("txtQustion2Mark");
                TextBox txtQustion3Mark = (TextBox)row.FindControl("txtQustion3Mark");
                TextBox txtQustion4Mark = (TextBox)row.FindControl("txtQustion4Mark");
                TextBox txtQustion5Mark = (TextBox)row.FindControl("txtQustion5Mark");
                TextBox txtQustion6Mark = (TextBox)row.FindControl("txtQustion6Mark");
                TextBox txtQustion7Mark = (TextBox)row.FindControl("txtQustion7Mark");
                TextBox txtQustion8Mark = (TextBox)row.FindControl("txtQustion8Mark");
                TextBox txtQustion9Mark = (TextBox)row.FindControl("txtQustion9Mark");
                TextBox txtQustion10Mark = (TextBox)row.FindControl("txtQustion10Mark");
                Label lblRemark = (Label)row.FindControl("lblRemark");
                Label lblTotalMark = (Label)row.FindControl("lblTotalMark"); 
                
                int answeredCounter = 0;

                decimal? qustion1Mark = null;
                if (!string.IsNullOrEmpty(txtQustion1Mark.Text))
                {
                    qustion1Mark = Convert.ToDecimal(txtQustion1Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion2Mark = null;
                if (!string.IsNullOrEmpty(txtQustion2Mark.Text))
                {
                    qustion2Mark = Convert.ToDecimal(txtQustion2Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion3Mark = null;
                if (!string.IsNullOrEmpty(txtQustion3Mark.Text))
                {
                    qustion3Mark = Convert.ToDecimal(txtQustion3Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion4Mark = null;
                if (!string.IsNullOrEmpty(txtQustion4Mark.Text))
                {
                    qustion4Mark = Convert.ToDecimal(txtQustion4Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion5Mark = null;
                if (!string.IsNullOrEmpty(txtQustion5Mark.Text))
                {
                    qustion5Mark = Convert.ToDecimal(txtQustion5Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion6Mark = null;
                if (!string.IsNullOrEmpty(txtQustion6Mark.Text))
                {
                    qustion6Mark = Convert.ToDecimal(txtQustion6Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion7Mark = null;
                if (!string.IsNullOrEmpty(txtQustion7Mark.Text))
                {
                    qustion7Mark = Convert.ToDecimal(txtQustion7Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion8Mark = null;
                if (!string.IsNullOrEmpty(txtQustion8Mark.Text))
                {
                    qustion8Mark = Convert.ToDecimal(txtQustion8Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion9Mark = null;
                if (!string.IsNullOrEmpty(txtQustion9Mark.Text))
                {
                    qustion9Mark = Convert.ToDecimal(txtQustion9Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                decimal? qustion10Mark = null;
                if (!string.IsNullOrEmpty(txtQustion10Mark.Text))
                {
                    qustion10Mark = Convert.ToDecimal(txtQustion10Mark.Text);
                    answeredCounter = answeredCounter + 1;
                }
                string remarks = string.Empty;
                if (answeredCounter > needToBeAnswered)
                {
                    remarks = remarks + "Answer limit cross. ";
                    output = false;
                }

                decimal totalMark = Convert.ToDecimal(Convert.ToDecimal(qustion1Mark) + Convert.ToDecimal(qustion2Mark) + Convert.ToDecimal(qustion3Mark) + Convert.ToDecimal(qustion4Mark) + Convert.ToDecimal(qustion5Mark) + Convert.ToDecimal(qustion6Mark) + Convert.ToDecimal(qustion7Mark) + Convert.ToDecimal(qustion8Mark) + Convert.ToDecimal(qustion9Mark) + Convert.ToDecimal(qustion10Mark));
                lblTotalMark.Text = Convert.ToString(totalMark);
                if (totalMark > examMark)
                {
                    remarks = remarks + " Total mark cross exam mark.";
                    output = false;
                }

                lblRemark.Text = remarks;
            }
            return output;
        }

        private void InsertOrUpdateFirstExaminerTotalMark(ExamMarkFirstSecondThirdExaminer emfsteModel, int courseHistoryId, int examTemplateItemId, decimal totalMark, int examinerType)
        {
            if (emfsteModel != null)
            {
                if (examinerType == 1)
                {
                    emfsteModel.FirstExaminerMark = totalMark;
                }
                if (examinerType == 2)
                {
                    emfsteModel.SecondExaminerMark = totalMark;
                }
                if (examinerType == 3)
                {
                    emfsteModel.ThirdExaminerMark = totalMark;
                }

                if ((!string.IsNullOrEmpty(emfsteModel.FirstExaminerMark.ToString()) && emfsteModel.FirstExaminerMark > 0) &&
                                (!string.IsNullOrEmpty(emfsteModel.SecondExaminerMark.ToString()) && emfsteModel.SecondExaminerMark > 0))
                {
                    #region ThirdExaminerStatus (Active/In-Active)
                    decimal percentageDiff = CheckPercentageDiff(Convert.ToDecimal(emfsteModel.FirstExaminerMark), Convert.ToDecimal(emfsteModel.SecondExaminerMark));
                    if (percentageDiff <= 19.99M)
                    {
                        emfsteModel.ThirdExaminerStatus = 0;
                    }
                    else
                    {
                        emfsteModel.ThirdExaminerStatus = 1;
                    }
                    #endregion

                }
                else
                {
                    emfsteModel.ThirdExaminerStatus = 0;
                }


                emfsteModel.IsAbsent = false;
                emfsteModel.ModifiedBy = userObj.Id;
                emfsteModel.ModifiedDate = DateTime.Now;
                bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(emfsteModel);
                if (isUpdate == true)
                {
                    //isInsertOrUpdateCount++;
                    //InsertOrUpdateExamQuestionMark(scmModel, 1);
                }
            }
            else
            {
                ExamMarkFirstSecondThirdExaminer model = new ExamMarkFirstSecondThirdExaminer();

                model.CourseHistoryId = courseHistoryId;
                model.ExamTemplateItemId = examTemplateItemId;
                
                if (examinerType == 1)
                {
                    model.FirstExaminerMark = totalMark;
                }
                if (examinerType == 2)
                {
                    model.SecondExaminerMark = totalMark;
                }
                if (examinerType == 3)
                {
                    model.ThirdExaminerMark = totalMark;
                }
                model.IsAbsent = false;
                model.CreatedBy = userObj.Id;
                model.CreatedDate = DateTime.Now;
                int examMarkFirstSecondThirdExaminerId = ExamMarkFirstSecondThirdExaminerManager.Insert(model);

                if (examMarkFirstSecondThirdExaminerId > 0)
                {
                    //isInsertOrUpdateCount++;
                    //InsertOrUpdateExamQuestionMark(scmModel, 1);
                }
            }
        }

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

        private void InsertOrUpdateExamQuestionMark(int studentId, int courseHistoryId, int examTemplateItemId, int questionNo, decimal? questionMark, int ExaminerType)
        {
            try
            {
                ExamMarkQuestion emq = ExamMarkQuestionManager.GetByStudentIdCourseHistoryId(studentId, courseHistoryId, questionNo);

                if (emq != null)
                {
                    // Update

                    emq.ExamTemplateItemId = examTemplateItemId;
                    if (ExaminerType == 1)
                    {
                        emq.FirstExaminerMark = questionMark;
                    }
                    if (ExaminerType == 2)
                    {
                        emq.SecondExaminerMark = questionMark;
                    }
                    if (ExaminerType == 3)
                    {
                        emq.ThirdExaminerMark = questionMark;
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
                    model.QuestionNo = questionNo;
                    if (ExaminerType == 1)
                    {
                        model.FirstExaminerMark = questionMark;
                    }
                    else if (ExaminerType == 2)
                    {
                        model.SecondExaminerMark = questionMark;
                    }
                    else if (ExaminerType == 3)
                    {
                        model.ThirdExaminerMark = questionMark;
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
            catch (Exception ex)
            {
                //MessageView("Something went wrong; Error: " + ex.Message.ToString(), "fail");
            }
        }

        protected void btnFinalExamMarkSubmitToCommittee_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int examinerType = Convert.ToInt32(lblExaminerType.Text);
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label lblStudentId = (Label)row.FindControl("lblStudentId");
                    Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    Label lblExamMarkFirstSecondThirdExaminerId = (Label)row.FindControl("lblExamMarkFirstSecondThirdExaminerId");

                    int courseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                    int examTemplateItemId = Convert.ToInt32(lblExamTemplateItemId.Text);
                    int studentId = Convert.ToInt32(lblStudentId.Text);

                    ExamMarkFirstSecondThirdExaminer emfsteModel = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryIdExamTemplateTypeId(courseHistoryId, examTemplateItemId);
                    if (emfsteModel != null)
                    {
                        if (examinerType == 1)
                        {
                            int statusId = Convert.ToInt16(emfsteModel.FirstExaminerMarkSubmissionStatus);
                            statusId = statusId + 1;
                            emfsteModel.FirstExaminerMarkSubmissionStatus = statusId;
                            emfsteModel.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;
                        }
                        if (examinerType == 2)
                        {
                            int statusId = Convert.ToInt16(emfsteModel.SecondExaminerMarkSubmissionStatus);
                            statusId = statusId + 1;
                            emfsteModel.SecondExaminerMarkSubmissionStatus = statusId;
                            emfsteModel.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;
                        }
                        if (examinerType == 3)
                        {
                            int statusId = Convert.ToInt16(emfsteModel.ThirdExaminerMarkSubmissionStatus);
                            statusId = statusId + 1;
                            emfsteModel.ThirdExaminerMarkSubmissionStatus = statusId;
                            emfsteModel.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                        }

                        emfsteModel.ModifiedBy = userObj.Id;
                        emfsteModel.ModifiedDate = DateTime.Now;

                        bool isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(emfsteModel);

                        if (isUpdate == true)
                        {
                            //updateCount++;
                        }
                    }
                    else 
                    {
                        ExamMarkFirstSecondThirdExaminer model = new ExamMarkFirstSecondThirdExaminer();

                        model.CourseHistoryId = courseHistoryId;
                        model.ExamTemplateItemId = examTemplateItemId;

                        if (examinerType == 1)
                        {
                            model.FirstExaminerMarkSubmissionStatus = 1;
                            model.FirstExaminerMarkSubmissionStatusDate = DateTime.Now;
                        }
                        if (examinerType == 2)
                        {
                            model.SecondExaminerMarkSubmissionStatus = 1;
                            model.SecondExaminerMarkSubmissionStatusDate = DateTime.Now;
                        }
                        if (examinerType == 3)
                        {
                            model.ThirdExaminerMarkSubmissionStatus = 1;
                            model.ThirdExaminerMarkSubmissionStatusDate = DateTime.Now;
                        }

                        model.ModifiedBy = userObj.Id;
                        model.ModifiedDate = DateTime.Now;

                        int examMarkFirstSecondThirdExaminerId = ExamMarkFirstSecondThirdExaminerManager.Insert(model);

                        if (examMarkFirstSecondThirdExaminerId > 0)
                        {
                            //isInsertOrUpdateCount++;
                            //InsertOrUpdateExamQuestionMark(scmModel, 1);
                        }
                    }                   
                }
                btnLoad_Click(null, null);
                lblMsg.Text = "Exam mark submitted to exam committee.";
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        //protected void ResultEntryGrid_OnRowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    try
        //    {
        //        lblMsg.Text = string.Empty;
        //        if (e.Row.RowType == DataControlRowType.DataRow)
        //        {
        //            //GridViewRow row = (GridViewRow)e.NamingContainer;
        //            //int index = row.RowIndex;
        //            //Label statusLabel = (Label)e.Row.FindControl("statusLabel");

        //            TextBox txtQustion1Mark = (TextBox)e.Row.FindControl("txtQustion1Mark");
        //            TextBox txtQustion2Mark = (TextBox)e.Row.FindControl("txtQustion2Mark");
        //            TextBox txtQustion3Mark = (TextBox)e.Row.FindControl("txtQustion3Mark");
        //            TextBox txtQustion4Mark = (TextBox)e.Row.FindControl("txtQustion4Mark");
        //            TextBox txtQustion5Mark = (TextBox)e.Row.FindControl("txtQustion5Mark");
        //            TextBox txtQustion6Mark = (TextBox)e.Row.FindControl("txtQustion6Mark");
        //            TextBox txtQustion7Mark = (TextBox)e.Row.FindControl("txtQustion7Mark");
        //            TextBox txtQustion8Mark = (TextBox)e.Row.FindControl("txtQustion8Mark");
        //            TextBox txtQustion9Mark = (TextBox)e.Row.FindControl("txtQustion9Mark");
        //            TextBox txtQustion10Mark = (TextBox)e.Row.FindControl("txtQustion10Mark");


        //            //if (statusLabel != null)
        //            //{
        //            //    if (statusLabel.Text.ToString() == "Absent")
        //            //    {
        //            //        CheckBox cb1 = (CheckBox)e.Row.FindControl("chkStatus");
        //            //        cb1.Checked = true;
        //            //    }

        //            //}
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}        
    }
}