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
using Newtonsoft.Json;
using LogicLayer.BusinessObjects.RO;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace EMS.Module.result
{
    public partial class ExamResultSubmit_NewVersion : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        int userId = 0;
        int employeeId = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
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

                if (userObj.Id == 5)// Only for Zarin Apu
                {
                    btnMarkEntry.Visible = true;
                }
                else
                    btnMarkEntry.Visible = false;

                //ucProgram.LoadDropdownWithUserAccess(userObj.Id);
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
                ReportViewer1.Visible = false;
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
                ReportViewer1.Visible = false;
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
                ReportViewer1.Visible = false;
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

        protected void ddlCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                ReportViewer1.Visible = false;
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int courseId = Convert.ToInt32(courseVersion[0]);
                int versionId = Convert.ToInt32(courseVersion[1]);
                int acaCalSection = Convert.ToInt32(courseVersion[2]);

                LoadContinousExam(acaCalSection);
                //List<ExamResultDTO> examResultDTOList = ExamTemplateManager.GetExamResultDTO(courseId, versionId, acaCalSection).ToList();
            }
            catch (Exception)
            {
                throw;
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

        protected void ddlContinousExam_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                ReportViewer1.Visible = false;
                btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }

        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                btnFinalSubmitAll.Visible = true;
                ResultEntryGrid.Visible = true;
                ReportViewer1.Visible = false;
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
                    lblExamTemplateItemId.Text = Convert.ToString(examTemplateItemObj.ExamTemplateItemId);

                    ExamMarkMaster examMarkMasterObj = ExamMarkMasterManager.GetByAcaCalSectionIdExamTemplateItemId(acaCalSectionId, examTemplateItemId);
                    if (examMarkMasterObj != null)
                    {
                        if (examMarkMasterObj.IsFinalSubmit == false)
                        {
                            //if (examMarkMasterObj.ExamDate != null)
                            //{
                            //    DateTime dt = (DateTime)examMarkMasterObj.ExamDate;
                            //    txtExamDate.Text = dt.ToString("dd/MM/yyyy");
                            //}
                            //else
                            //{
                            //    DateTime dt = DateTime.Now;
                            //    txtExamDate.Text = "";
                            //}

                            LoadResultGrid(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
                        }
                        else
                        {
                            pnlTotalMark.Visible = false;
                            ResultEntryGrid.DataSource = null;
                            ResultEntryGrid.DataBind();
                            ResultEntryGrid.EmptyDataText = "Result sumitted to Exam Committee.";
                            lblMsg.Text = "Exam mark already submitted to Exam Committee.";
                        }
                    }
                    else
                    {
                        //txtExamDate.Text = "";
                        LoadResultGrid(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
                        //InsertExamMarkMaster(ExamTemplateItem examTemplateItemObj, int acaCalSection)
                        //txtTotalMark.Text = Convert.ToString(examTemplateItemObj.ExamMark);
                    }
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

        private void LoadResultGrid(int programId, int yearNo, int semesterNo, int courseId, int versionId, int acaCalSectionId, int examTemplateItemId)
        {
            List<ExamMarkDTO> studentList = ExamMarkDetailsManager.GetByExamMarkDtoByParameter(programId, yearNo, semesterNo, courseId, versionId, acaCalSectionId, examTemplateItemId);
            if (studentList != null)
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
            }
            GridRebind();
        }

        private void GridRebind()
        {
            for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
            {
                GridViewRow row = ResultEntryGrid.Rows[i];
                Label lblExamStatus = (Label)row.FindControl("lblExamStatus");
                TextBox mark = (TextBox)row.FindControl("txtMark");
                if (Convert.ToString(lblExamStatus.Text) == "2")
                {
                    mark.Text = "Ab";
                }
            }
        }

        protected void btnFinalSubmitAll_Clicked(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                string course = ddlCourse.SelectedValue;
                string[] courseVersion = course.Split('_');

                int acaCalSection = Convert.ToInt32(courseVersion[2]);

                bool result = ExamMarkMasterManager.FinalSubmitByAcacalSectionId(acaCalSection, true);
                if (result)
                {
                    lblMsg.Text = "Exam marks of selected course submitted to Exam Committee. Now you will be not able to submit any marks of selected course.";
                    btnLoad_Click(null, null);
                }
                else
                {
                    lblMsg.Text = "Exam marks of selected course could not submitted to Exam Committee. Please try again.";
                }
            }
            catch { }
        }

        protected void btnSubmitAllMark_Click(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                int examTemplateItemId = Convert.ToInt32(lblExamTemplateItemId.Text);
                ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
                if (examTemplateItemId > 0)
                {
                    examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                }

                if (examTemplateItemObj != null)
                {
                    string course = ddlCourse.SelectedValue;
                    string[] courseVersion = course.Split('_');

                    int acaCalSection = Convert.ToInt32(courseVersion[2]);

                    int examMarkMasterId = ExamMarkMasterManager.InsertExamMarkMaster(examTemplateItemObj, acaCalSection, userObj.Id);
                    if (examMarkMasterId > 0)
                    {
                        if (CheckAllMark(examTemplateItemObj.ExamMark))
                        {
                            SubmitAllExamMark(examMarkMasterId, examTemplateItemObj.ExamTemplateItemId, examTemplateItemObj.ExamMark);
                            btnLoad_Click(null, null);
                        }
                    }
                    else
                    {
                        lblMsg.Text = "To submit exam mark, exam not found.";
                    }
                }
                else
                {
                    lblMsg.Text = "Exam not found.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        //private int InsertExamMarkMaster(ExamTemplateItem examTemplateItemObj, int acaCalSection)
        //{
        //    int examMarkMasterId = 0;
        //    try
        //    {                
        //        ExamMarkMaster examMarkMasterNewObj = ExamMarkMasterManager.GetByAcaCalSectionIdExamTemplateItemId(acaCalSection, examTemplateItemObj.ExamTemplateItemId);
        //        if (examMarkMasterNewObj == null)
        //        {
        //            if (examTemplateItemObj != null && acaCalSection > 0)
        //            {
        //                ExamMarkMaster examMarkMasterObj = new ExamMarkMaster();
        //                examMarkMasterObj.ExamMarkEntryDate = DateTime.Now;
        //                examMarkMasterObj.ExamMark = examTemplateItemObj.ExamMark;
        //                //if (string.IsNullOrEmpty(txtExamDate.Text))
        //                //    examMarkMasterObj.FinalSubmissionDate = DateTime.Now;
        //                //else
        //                //    examMarkMasterObj.FinalSubmissionDate = DateTime.ParseExact(txtExamDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);
        //                examMarkMasterObj.IsFinalSubmit = false;
        //                examMarkMasterObj.ExamTemplateItemId = examTemplateItemObj.ExamTemplateItemId;
        //                examMarkMasterObj.AcaCalSectionId = acaCalSection;
        //                examMarkMasterObj.CreatedBy = userObj.Id;
        //                examMarkMasterObj.CreatedDate = DateTime.Now;
        //                examMarkMasterObj.ModifiedBy = userObj.Id;
        //                examMarkMasterObj.ModifiedDate = DateTime.Now;


        //                int result = ExamMarkMasterManager.Insert(examMarkMasterObj);
        //                if (result > 0)
        //                {
        //                    examMarkMasterId = result;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            examMarkMasterId = examMarkMasterNewObj.ExamMarkMasterId;
        //        }
        //        return examMarkMasterId;
        //    }
        //    catch (Exception ex)
        //    {
        //        return examMarkMasterId;
        //    }
        //}

        private void SubmitAllExamMark(int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark)
        {
            try
            {
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    TextBox mark = (TextBox)row.FindControl("txtMark");
                    CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");

                    Nullable<decimal> studentMark = 0;
                    int courseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                    bool isAbsent = chkStatus.Checked;
                    if (courseHistoryId > 0)
                    {
                        //if (Convert.ToString(mark) != string.Empty)
                        //{
                        if (Convert.ToString(mark.Text) == "Ab" || string.IsNullOrEmpty(mark.Text))
                        {
                            studentMark = null;
                        }
                        else
                        {
                            studentMark = Convert.ToDecimal(mark.Text);
                        }
                        //}

                        InsertEditExamMarkDetails(courseHistoryId, isAbsent, examMarkMasterId, examTemplateItemId, examTemplateExamMark, studentMark);
                    }
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private bool InsertEditExamMarkDetails(int studentCourseHistoryId, bool isAbsent, int examMarkMasterId, int examTemplateItemId, decimal examTemplateExamMark, Nullable<decimal> studentMark)
        {
            ExamMarkDetails examMarkDetailsObj = ExamMarkDetailsManager.GetByCourseHistoryIdExamTemplateItemId(studentCourseHistoryId, examTemplateItemId);
            Nullable<decimal> mark = studentMark; // GetStudentMark(studentCourseHistoryId);
            if (examMarkDetailsObj == null)
            {
                //ExamMarkMaster examMarkMaserObj = ExamMarkMasterManager.GetById(examMarkMasterId);

                ExamMarkDetails examMarkDetails = new ExamMarkDetails();
                examMarkDetails.ExamMarkMasterId = examMarkMasterId;
                examMarkDetails.CourseHistoryId = studentCourseHistoryId;
                if (!isAbsent && mark != null)
                {
                    examMarkDetails.Marks = Convert.ToDecimal(mark);
                    examMarkDetails.ConvertedMark = Convert.ToDecimal(mark);
                    examMarkDetails.ExamStatus = 1;
                }
                else
                {
                    examMarkDetails.ExamStatus = 2;
                    examMarkDetails.Marks = null;
                    examMarkDetails.ConvertedMark = null;
                }
                examMarkDetails.IsFinalSubmit = false;
                examMarkDetails.ExamMarkTypeId = 0;
                examMarkDetails.ExamTemplateItemId = examTemplateItemId;
                examMarkDetails.CreatedBy = userObj.Id;
                examMarkDetails.CreatedDate = DateTime.Now;
                examMarkDetails.ModifiedBy = userObj.Id;
                examMarkDetails.ModifiedDate = DateTime.Now;

                int result = ExamMarkDetailsManager.Insert(examMarkDetails);
                if (result > 0)
                {
                    lblMsg.Text = "Student marks inserted successfully.";
                    return true;
                }
                else
                {
                    lblMsg.Text = "Student marks could not inserted.";
                    return false;
                }
            }
            else
            {
                if (examMarkDetailsObj.IsFinalSubmit == false)
                {
                    examMarkDetailsObj.ExamMarkMasterId = examMarkMasterId;
                    if (!isAbsent)
                    {
                        examMarkDetailsObj.Marks = Convert.ToDecimal(mark);
                        examMarkDetailsObj.ConvertedMark = Convert.ToDecimal(mark);
                        examMarkDetailsObj.ExamStatus = 1;
                    }
                    else
                    {
                        examMarkDetailsObj.ExamStatus = 2;
                        examMarkDetailsObj.Marks = 0;
                        examMarkDetailsObj.ConvertedMark = 0;
                    }
                    examMarkDetailsObj.ExamTemplateItemId = examTemplateItemId;
                    examMarkDetailsObj.ExamMarkTypeId = 0;
                    examMarkDetailsObj.IsFinalSubmit = false;
                    examMarkDetailsObj.ExamMarkTypeId = 0;
                    examMarkDetailsObj.ModifiedBy = userObj.Id;
                    examMarkDetailsObj.ModifiedDate = DateTime.Now;
                    bool result = ExamMarkDetailsManager.Update(examMarkDetailsObj);
                    if (result)
                    {
                        lblMsg.Text = "Student marks edited successfully.";
                        return true;
                    }
                    else
                    {
                        lblMsg.Text = "Student marks could not edited.";
                        return false;
                    }
                }
                else
                {
                    lblMsg.Text = "You can't submit mark now. Final submit complete.";
                    return false;
                }
            }
        }

        protected void ResultEntryGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ResultSubmit")
                {
                    lblMsg.Text = string.Empty;
                    int examTemplateItemId = Convert.ToInt32(lblExamTemplateItemId.Text);

                    GridViewRow row = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                    Label lblCourseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    TextBox mark = (TextBox)row.FindControl("txtMark");
                    CheckBox chkStatus = (CheckBox)row.FindControl("chkStatus");

                    int studentCourseHistoryId = Convert.ToInt32(lblCourseHistoryId.Text);
                    bool isAbsent = chkStatus.Checked;

                    ExamTemplateItem examTemplateItemObj = new ExamTemplateItem();
                    if (examTemplateItemId > 0)
                    {
                        examTemplateItemObj = ExamTemplateItemManager.GetById(examTemplateItemId);
                    }
                    if (examTemplateItemObj != null)
                    {
                        string course = ddlCourse.SelectedValue;
                        string[] courseVersion = course.Split('_');
                        Nullable<decimal> studentExamMark = Convert.ToDecimal(mark.Text);
                        int acaCalSection = Convert.ToInt32(courseVersion[2]);
                        int examMarkMasterId = ExamMarkMasterManager.InsertExamMarkMaster(examTemplateItemObj, acaCalSection, userObj.Id);
                        if (examMarkMasterId > 0)
                        {
                            if (CheckMark(studentExamMark, examTemplateItemObj.ExamMark))
                            {
                                InsertEditExamMarkDetails(studentCourseHistoryId, isAbsent, examMarkMasterId, examTemplateItemObj.ExamTemplateItemId, examTemplateItemObj.ExamMark, studentExamMark);
                                btnLoad_Click(null, null);
                            }
                            else
                            {
                                lblMsg.Text = "Student marks could not be more then exam mark.";
                            }
                        }
                        else
                        {
                            lblMsg.Text = "To submit exam mark, exam not found.";
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Exam not found.";
                    }
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
                //if (e.Row.RowType == DataControlRowType.DataRow)
                //{
                //    //GridViewRow row = (GridViewRow)e.NamingContainer;
                //    //int index = row.RowIndex;
                //    Label statusLabel = (Label)e.Row.FindControl("statusLabel");
                //    if (statusLabel != null)
                //    {
                //        if (statusLabel.Text.ToString() == "Absent")
                //        {
                //            CheckBox cb1 = (CheckBox)e.Row.FindControl("chkStatus");
                //            cb1.Checked = true;
                //        }

                //    }
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }



        protected void chkStatus_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                GridViewRow row = ((GridViewRow)((CheckBox)sender).NamingContainer);
                int index = row.RowIndex;
                TextBox txtMark = (TextBox)ResultEntryGrid.Rows[index].FindControl("txtMark");
                CheckBox cb1 = (CheckBox)ResultEntryGrid.Rows[index].FindControl("chkStatus");

                if (cb1.Checked)
                {
                    txtMark.Text = "Ab";
                    txtMark.Enabled = false;
                }
                else
                {
                    txtMark.Text = string.Empty;
                    txtMark.Enabled = true;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        //private decimal GetStudentMark(int lblCourseHistoryId)
        //{
        //    decimal studentMark = 0;
        //    try
        //    {
        //        for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
        //        {
        //            GridViewRow row = ResultEntryGrid.Rows[i];
        //            Label courseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
        //            TextBox mark = (TextBox)row.FindControl("txtMark");
        //            if (Convert.ToString(mark) != string.Empty)
        //            {
        //                if (Convert.ToInt32(courseHistoryId.Text) == lblCourseHistoryId)
        //                {
        //                    if (Convert.ToString(mark.Text) == "Ab")
        //                    {
        //                        studentMark = 0;
        //                    }
        //                    else
        //                    {
        //                        studentMark = Convert.ToDecimal(mark.Text);
        //                    }

        //                    break;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return studentMark;
        //}

        private bool CheckMark(Nullable<decimal> studentMark, decimal examMark)
        {
            if (studentMark == null)
            {
                return true;
            }
            else
            {
                double stuMark = Convert.ToDouble(studentMark);
                double exmMark = Convert.ToDouble(examMark);

                if (stuMark > exmMark)
                {
                    return false;
                }
                else if (stuMark >= 0 && stuMark <= exmMark)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool CheckAllMark(decimal examMark)
        {
            try
            {
                bool result = true;
                string msgText = null;
                for (int i = 0; i < ResultEntryGrid.Rows.Count; i++)
                {
                    GridViewRow row = ResultEntryGrid.Rows[i];
                    Label courseHistoryId = (Label)row.FindControl("lblCourseHistoryId");
                    Label studentRoll = (Label)row.FindControl("lblStudentRoll");
                    TextBox mark = (TextBox)row.FindControl("txtMark");

                    Nullable<decimal> studentMark = 0;
                    if (Convert.ToString(mark.Text) == "Ab" || string.IsNullOrEmpty(mark.Text))
                    {
                        studentMark = null;
                    }
                    else
                    {
                        studentMark = Convert.ToDecimal(mark.Text);//GetStudentMark(Convert.ToInt32(courseHistoryId.Text));

                        bool chkResult = CheckMark(studentMark, examMark);
                        if (chkResult == false)
                        {
                            result = false;
                            msgText = msgText + Convert.ToString(studentRoll.Text) + ", ";
                        }

                        lblMsg.Text = "Student marks could not be more then exam mark. Student Id : " + msgText;
                    }
                }
                return result;
            }
            catch (Exception)
            {
                return false;
            }
        }

        [System.Web.Services.WebMethod]
        public static string GetExamMarks(string courseSection)
        {
            string[] courseVersionSection = courseSection.Split('_');
            int courseId = Convert.ToInt32(courseVersionSection[0]);
            int versionId = Convert.ToInt32(courseVersionSection[1]);
            int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);

            List<ExamResultDTO> examResultDTOList = ExamTemplateManager.GetExamResultDTO(courseId, versionId, acaCalSectionId).ToList();
            List<ExamResultDTO> newList = new List<ExamResultDTO>();

            CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoObj = ExamMarkMasterManager.GetTeacherInfoAPIBySectionId(acaCalSectionId);

            var jsonData = new
            {
                list1 = examResultDTOList,
                list2 = countinousMarkTeacherInfoObj

            };

            string json = JsonConvert.SerializeObject(jsonData, Formatting.Indented, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });


            return json;
        }

        protected void btnLoadReport_Click(object sender, EventArgs e)
        {

            try
            {
                string courseSection = ddlCourse.SelectedValue.ToString();
                string[] courseVersionSection = courseSection.Split('_');
                int courseId = Convert.ToInt32(courseVersionSection[0]);
                int versionId = Convert.ToInt32(courseVersionSection[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);
                if (acaCalSectionId > 0)
                {
                    ExamTemplateManager.ProcessContinuousAssessmentMark(acaCalSectionId, userObj.Id);

                    AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);

                    List<rContinuousAssessmentMark> list = ExamTemplateManager.GetContinuousMarkBySectionID(acaCalSectionId);

                    string Mark = "";
                    bool IsViva = false;

                    int programId = Convert.ToInt32(ucProgram.selectedValue), yearNo = Convert.ToInt32(ddlYearNo.SelectedValue), semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue)
                       , sessionId = Convert.ToInt32(ucFilterCurrentSession.selectedValue);


                    string Exam = ddlExam.SelectedItem.ToString();
                    string[] ExamNameSplit = Exam.Split(' ');
                    string ExamName = "", ExamYear = "";
                    if (ExamNameSplit[4] != "")
                        ExamName = ExamNameSplit[4] + " Examination";
                    if (ExamNameSplit[5] != "")
                        ExamYear = ExamNameSplit[5];

                    List<ExamTemplateItem> tmpList = ExamTemplateItemManager.GetByExamTemplateId(acs.BasicExamTemplateId).Where(x => x.Attribute1 == "Continuous").ToList();

                    List<rExamCommitteePersonInfo> list2 = new List<rExamCommitteePersonInfo>();

                    if (tmpList != null && tmpList.Count > 0)
                    {
                        Mark = tmpList.Sum(x => x.ExamMark).ToString();
                        if (tmpList.Where(x => x.Attribute2 == "Viva").Count() > 0)
                        {
                            IsViva = true;

                            list2 = ExamSetupManager.GetAllExamCommitteePersonInfoByParameter(programId, yearNo, semesterNo, Convert.ToInt32(ExamYear), sessionId);
                        }

                    }

                    Course crs = CourseManager.GetByCourseIdVersionId(courseId, versionId);
                    CountinousMarkTeacherInfoAPI_DTO countinousMarkTeacherInfoObj = ExamMarkMasterManager.GetTeacherInfoAPIBySectionId(acaCalSectionId);

                    string teacherName = "", teacherDept = "";

                    if (countinousMarkTeacherInfoObj != null)
                    {
                        teacherName = countinousMarkTeacherInfoObj.TeacherName == null ? "" : countinousMarkTeacherInfoObj.TeacherName;
                        teacherDept = countinousMarkTeacherInfoObj.DepartmentName == null ? "" : countinousMarkTeacherInfoObj.DepartmentName;
                    }


                    if (list.Count > 0)
                    {
                        ResultEntryGrid.Visible = false;
                        ResultEntryGrid.DataSource = null;
                        ResultEntryGrid.DataBind();
                        btnFinalSubmitAll.Visible = false;
                        lblExamMark.Text = string.Empty;


                        ReportParameter p1 = new ReportParameter("Year", ddlYearNo.SelectedItem.Text.ToString());
                        ReportParameter p2 = new ReportParameter("Semester", ddlSemesterNo.SelectedItem.Text.ToString());
                        ReportParameter p3 = new ReportParameter("Department", ucDepartment.selectedText.ToString());
                        ReportParameter p4 = new ReportParameter("Course", crs.FormalCode.ToString());
                        ReportParameter p5 = new ReportParameter("TeacherName", teacherName.ToString());
                        ReportParameter p6 = new ReportParameter("DepartmentName", teacherDept.ToString());
                        ReportParameter p7 = new ReportParameter("SessionName", countinousMarkTeacherInfoObj.SessionName.ToString());
                        ReportParameter p8 = new ReportParameter("ExamMark", Mark);
                        ReportParameter p9 = new ReportParameter("ExamName", ExamName);
                        ReportParameter p10 = new ReportParameter("ExamYear", ExamYear);
                        ReportParameter p11 = new ReportParameter("CourseTitle", crs.Title.ToString());

                        if (IsViva)
                        {

                            ReportParameter p12 = new ReportParameter("ChairmanName", list2[0].ChairmanName.ToString());
                            ReportParameter p13 = new ReportParameter("ChairmanDept", list2[0].ChairmanDept.ToString());

                            ReportParameter p14 = new ReportParameter("ExaminerOne", list2[0].MemberOneName.ToString());
                            ReportParameter p15 = new ReportParameter("ExaminerOneDept", list2[0].MemberOneDept.ToString());

                            ReportParameter p16 = new ReportParameter("ExaminerTwo", list2[0].MemberTwoName.ToString());
                            ReportParameter p17 = new ReportParameter("ExaminerTwoDept", list2[0].MemberTwoDept.ToString());

                            ReportParameter p18 = new ReportParameter("ExternalName", list2[0].ExternalMemberName.ToString());
                            ReportParameter p19 = new ReportParameter("ExternalDept", list2[0].ExternalMemberDept.ToString());

                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptContinuousAssessmentMarkViva.rdlc");
                            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11, p12, p13, p14, p15, p16, p17, p18, p19 });

                        }
                        else
                        {
                            ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptContinuousAssessmentMark.rdlc");
                            this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });
                        }

                        ReportDataSource rds = new ReportDataSource("DataSet", list);

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);





                        ReportViewer1.Visible = true;




                        //#region Pdf File Create And View In New Page
                        //string Name = "RptContinuousAssessmentMark" + acaCalSectionId;

                        //if (File.Exists(Server.MapPath("~/Module/pdf/" + Name + ".pdf")))
                        //{
                        //    File.Delete(Server.MapPath("~/Module/pdf/" + Name + ".pdf"));
                        //}


                        //Warning[] warnings;
                        //string[] streamids;
                        //string mimeType;
                        //string encoding;
                        //string filenameExtension;

                        //byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                        //ReportViewer1.LocalReport.Refresh();

                        //using (FileStream fs = new FileStream(Server.MapPath("~/Module/pdf/" + Name + ".pdf"), FileMode.Create))
                        //{
                        //    fs.Write(bytes, 0, bytes.Length);
                        //}

                        //string url = string.Format("/brur/Module/ViewPDF.aspx?FN={0}", Name + ".pdf");
                        //string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
                        //this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);

                        //#endregion





                    }
                    else
                    {
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.Visible = false;
                        return;
                    }

                }

            }
            catch
            {

            }


        }

        protected void btnMarkEntry_Click(object sender, EventArgs e)
        {
            try
            {
                string courseSection = ddlCourse.SelectedValue.ToString();
                string[] courseVersionSection = courseSection.Split('_');
                int courseId = Convert.ToInt32(courseVersionSection[0]);
                int versionId = Convert.ToInt32(courseVersionSection[1]);
                int acaCalSectionId = Convert.ToInt32(courseVersionSection[2]);
                if (acaCalSectionId > 0)
                {
                    bool result = ExamMarkMasterManager.AutoMarkEntryForContinuousAssessmentTemplateIdBySectionId(acaCalSectionId);
                    if (result)
                    {
                        showAlert("Mark Entry Successfully.");
                    }
                    else
                    {
                        showAlert("Mark Entry Failed.");
                    }
                }

            }
            catch
            {

            }
        }


        protected void showAlert(string msg)
        {
            //ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);

        }

    }
}