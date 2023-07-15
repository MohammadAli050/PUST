using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessObjects;
using System.Net;
using Newtonsoft.Json;

namespace EMS.Module.result
{
    public partial class ExamCommitteeDashboard : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            ScriptManager _scriptMan = ScriptManager.GetCurrent(this);
            _scriptMan.AsyncPostBackTimeout = 36000;

            if (!IsPostBack)
            {
                lblMsg.Text = string.Empty;
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //ucProgram.LoadDropdownWithUserAccess(userObj.Id);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
                if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                {
                    btnTabulationProcess.Visible = true;
                    btnLoadTabulation.Visible = true;
                }
                else
                {
                    btnTabulationProcess.Visible = false;
                    btnLoadTabulation.Visible = false;
                    btnAdd.Visible = false;
                }
                LoadSubmissionDate();
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadExamDropdown(programId, yearNo, semesterNo);
                    RefreshGrid();
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
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                LoadExamDropdown(programId, yearNo, semesterNo);
                RefreshGrid();
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
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    LoadExamDropdown(programId, yearNo, semesterNo);
                    RefreshGrid();
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
                    RefreshGrid();
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
                btnLoad_Click(null, null);
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error Occured. " + ex.Message;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                var examSetupDetail = ExamSetupDetailManager.GetById(examId);
                if (examSetupDetail.ExamSetupId != null && !(userObj.RoleID == 1 || userObj.RoleID == 2))
                {
                    int examSetupId = (int)examSetupDetail.ExamSetupId;
                    var examCommittees = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examSetupId);
                    if (examCommittees != null)
                    {
                        var person = PersonManager.GetByUserId(userObj.Id);
                        var employee = EmployeeManager.GetByPersonId(person.PersonID);
                        if (!(examCommittees.ExamCommitteeChairmanId == employee.EmployeeID ||
                            examCommittees.ExamCommitteeMemberOneId == employee.EmployeeID ||
                            examCommittees.ExamCommitteeMemberTwoId == employee.EmployeeID))
                        {
                            ShowAlertMessage("You have no access of this exam");
                            return;
                        }
                    }
                    
                    
                }
                
                
                if (programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    LoadExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                    {
                        btnTabulationProcess.Visible = true;
                        btnLoadTabulation.Visible = true;
                    }
                }
                else
                {
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                    {
                        btnTabulationProcess.Visible = true;
                        btnLoadTabulation.Visible = true;
                    }
                    lblMsg.Text = "Please select program, year, semester and exam.";
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = "Error Occured. " + ex.Message;
            }
        }

        private void LoadExamCommitteeDashboard(int programId, int yearNo, int semesterNo, int examId)
        {
            lblMsg.Text = string.Empty;
            try
            {
                List<ExamCommitteeDashboardDTO> examCommitteeDashboard = ExamMarkMasterManager.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);


                foreach (var examCommitteeDashboardDto in examCommitteeDashboard)
                {
                    
                    
                        var temp = ExamMarkMasterManager.GetExamCommitteeDashboardExtendByAcaCalSecId(
                            examCommitteeDashboardDto.AcaCal_SectionID);
                        if (temp != null)
                        {
                            examCommitteeDashboardDto.ThirdExaminationEligibleStudentCount =
                                temp.ThirdExaminationEligibleStudentCount;
                            examCommitteeDashboardDto.TotalStudent = temp.TotalStudent;
                            examCommitteeDashboardDto.FirstExaminerMarkSubmissionStatus =
                                temp.FirstExaminerMarkSubmissionStatus % 2;
                            examCommitteeDashboardDto.SecondExaminerMarkSubmissionStatus =
                                temp.SecondExaminerMarkSubmissionStatus % 2;
                            examCommitteeDashboardDto.ThirdExaminerMarkSubmissionStatus =
                                temp.ThirdExaminerMarkSubmissionStatus % 2;
                            examCommitteeDashboardDto.FirstExaminerMarkSubmissionStatusDate =
                                temp.FirstExaminerMarkSubmissionStatusDate;
                            examCommitteeDashboardDto.SecondExaminerMarkSubmissionStatusDate =
                                temp.SecondExaminerMarkSubmissionStatusDate;
                            examCommitteeDashboardDto.ThirdExaminerMarkSubmissionStatusDate =
                                temp.ThirdExaminerMarkSubmissionStatusDate;
                        }
                    

                    var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(examCommitteeDashboardDto.AcaCal_SectionID);
                    if (groupExaminerList != null && groupExaminerList.Any())
                    {
                        string firstExaminer = String.Empty;
                        string secondExaminer = String.Empty;
                        string thirdExaminer = String.Empty;
                        foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                        {
                            if (examinerSetupStudentWise.FirstExaminer > 0)
                            {
                                //Employee employee1 = EmployeeManager.GetById(examinerSetupStudentWise.FirstExaminer);
                                //Person person1 = PersonManager.GetById(employee1.PersonId);
                                if (String.IsNullOrEmpty(firstExaminer))
                                {
                                    firstExaminer = examinerSetupStudentWise.ExaminerName1.BasicInfo.FullName;
                                }
                                else

                                    firstExaminer = firstExaminer + ", " + examinerSetupStudentWise.ExaminerName1.BasicInfo.FullName;
                            }

                            if (examinerSetupStudentWise.SecondExaminer > 0)
                            {
                                //Employee employee2 = EmployeeManager.GetById(examinerSetupStudentWise.SecondExaminer);
                                //Person person2 = PersonManager.GetById(employee2.PersonId);
                                if (String.IsNullOrEmpty(secondExaminer))
                                {
                                    secondExaminer = examinerSetupStudentWise.ExaminerName2.BasicInfo.FullName;
                                }
                                else
                                    secondExaminer = secondExaminer + ", " + examinerSetupStudentWise.ExaminerName2.BasicInfo.FullName;
                            }

                            if (examinerSetupStudentWise.ThirdExaminer > 0)
                            {
                                //Employee employee3 = EmployeeManager.GetById(examinerSetupStudentWise.ThirdExaminer);
                                //Person person3 = PersonManager.GetById(employee3.PersonId);
                                if (String.IsNullOrEmpty(thirdExaminer))
                                {
                                    thirdExaminer = examinerSetupStudentWise.ExaminerName3.BasicInfo.FullName;
                                }
                                else
                                    thirdExaminer = thirdExaminer + ", " + examinerSetupStudentWise.ExaminerName3.BasicInfo.FullName;
                            }

                        }

                        examCommitteeDashboardDto.FirstExaminerName = firstExaminer;
                        examCommitteeDashboardDto.SecondExaminerName = secondExaminer;
                        examCommitteeDashboardDto.ThirdExaminerName = thirdExaminer;

                        var examinerSubmission = ExamMarkMasterManager.GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(examCommitteeDashboardDto.AcaCal_SectionID);
                        if (examinerSubmission.Any())
                        {
                            examCommitteeDashboardDto.ThirdExaminationEligibleStudentCount = examinerSubmission[0].ThirdExaminationEligibleStudentCount;
                            examCommitteeDashboardDto.TotalStudent = examinerSubmission[0].TotalStudent;
                            int firstExaminerMarkSubmissionStatus = 1;
                            int secondExaminerMarkSubmissionStatus = 1;
                            int thirdExaminerMarkSubmissionStatus = 1;
                            foreach (var committeeDashboardDto in examinerSubmission)
                            {
                                if (committeeDashboardDto.FirstExaminerMarkSubmissionStatus % 2 == 0)
                                {
                                    firstExaminerMarkSubmissionStatus = 0;
                                }
                                if (committeeDashboardDto.SecondExaminerMarkSubmissionStatus % 2 == 0)
                                {
                                    secondExaminerMarkSubmissionStatus = 0;
                                }
                                if (committeeDashboardDto.ThirdExaminerMarkSubmissionStatus % 2 == 0)
                                {
                                    thirdExaminerMarkSubmissionStatus = 0;
                                }
                            }
                            examCommitteeDashboardDto.FirstExaminerMarkSubmissionStatus = firstExaminerMarkSubmissionStatus;
                            examCommitteeDashboardDto.SecondExaminerMarkSubmissionStatus = secondExaminerMarkSubmissionStatus;
                            examCommitteeDashboardDto.ThirdExaminerMarkSubmissionStatus = thirdExaminerMarkSubmissionStatus;
                            examCommitteeDashboardDto.FirstExaminerMarkSubmissionStatusDate =
                                temp.FirstExaminerMarkSubmissionStatusDate;
                            examCommitteeDashboardDto.SecondExaminerMarkSubmissionStatusDate =
                                temp.SecondExaminerMarkSubmissionStatusDate;
                            examCommitteeDashboardDto.ThirdExaminerMarkSubmissionStatusDate =
                                temp.ThirdExaminerMarkSubmissionStatusDate;
                        }

                    }
                    else
                    {
                        var tempTeacher = ExamMarkMasterManager.GetExaminerByAcaCalSecId(examCommitteeDashboardDto.AcaCal_SectionID);
                        if (tempTeacher != null)
                        {
                            examCommitteeDashboardDto.FirstExaminerName = tempTeacher.FirstExaminerName;
                            examCommitteeDashboardDto.SecondExaminerName = tempTeacher.SecondExaminerName;
                            examCommitteeDashboardDto.ThirdExaminerName = tempTeacher.ThirdExaminerName;
                        }
                    }

                    var examMarkSubmissionDate = ExamMarkSubmissionDateManager.GetByAcaCalSecId(examCommitteeDashboardDto.AcaCal_SectionID);
                    if (examMarkSubmissionDate != null)
                    {
                        examCommitteeDashboardDto.FirstExaminerMarkSubmissionDate = examMarkSubmissionDate.FirstExaminerSubmissionDate;
                        examCommitteeDashboardDto.SecondExaminerMarkSubmissionDate = examMarkSubmissionDate.SecondExaminerSubmissionDate;
                        examCommitteeDashboardDto.ThirdExaminerMarkSubmissionDate = examMarkSubmissionDate.ThirdExaminerSubmissionDate;
                    }

                }

                int examinerMarkSubmissionStatus = 0;
                foreach (ExamCommitteeDashboardDTO examCommitteeDashboardDto in examCommitteeDashboard)
                {

                    examinerMarkSubmissionStatus = 1;
                    if ((examCommitteeDashboardDto.FirstExaminerMarkSubmissionStatus == 0 ||
                        examCommitteeDashboardDto.SecondExaminerMarkSubmissionStatus == 0) && examCommitteeDashboardDto.Course.TypeDefinitionID != 3)
                    {
                        examinerMarkSubmissionStatus = 0;
                        break;
                    }

                }

                foreach (ExamCommitteeDashboardDTO examCommitteeDashboardDto in examCommitteeDashboard)
                {

                    examCommitteeDashboardDto.ExaminerMarkSubmissionStatus = examinerMarkSubmissionStatus;
                    if (examCommitteeDashboardDto.Course.TypeDefinitionID == 3 && (examCommitteeDashboardDto.FirstExaminerMarkSubmissionStatus == 1 ||
                        examCommitteeDashboardDto.SecondExaminerMarkSubmissionStatus == 1))
                    {
                        examCommitteeDashboardDto.ExaminerMarkSubmissionStatus = 1;
                    }

                }

                if (examCommitteeDashboard.Any())
                {
                    foreach (ExamCommitteeDashboardDTO examCommitteeDashboardDto in examCommitteeDashboard)
                    {
                        ExamSetupWithExamCommittees exam = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examCommitteeDashboardDto.ExamSetupId);
                        if (exam != null && (exam.ExamCommitteeChairmanId == userObj.Id || exam.ExamCommitteeExternalMemberId == userObj.Id || exam.ExamCommitteeMemberOneId == userObj.Id || exam.ExamCommitteeMemberTwoId == userObj.Id || userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7))
                        {
                            gvExamCommitteDashboard.DataSource = examCommitteeDashboard;
                            gvExamCommitteDashboard.DataBind();

                            GridRebind();
                        }
                        else
                        {
                            lblMsg.Text = "Exam Committee is not set yet.";
                        }
                    }

                    btnTabulationProcess.Enabled = true;
                    btnView.Enabled = true;
                    btnLoadTabulation.Enabled = true;

                    if (examCommitteeDashboard[0].ExaminerMarkSubmissionStatus == 0)
                    {
                        btnTabulationProcess.Enabled = false;
                        btnView.Enabled = false;
                        btnLoadTabulation.Enabled = false;
                    }
                }
                else
                {
                    gvExamCommitteDashboard.DataSource = null;
                    gvExamCommitteDashboard.DataBind();
                }

            }
            catch (Exception e)
            {
                lblMsg.Text = "Error Occured. " + e.Message;
            }
        }

        private void GridRebind()
        {
            for (int i = 0; i < gvExamCommitteDashboard.Rows.Count; i++)
            {
                GridViewRow row = gvExamCommitteDashboard.Rows[i];
                Label lblIsContinousMarkSubmit = (Label)row.FindControl("lblIsContinousMarkSubmit");
                Label lblSubmitToCommittee = (Label)row.FindControl("lblSubmitToCommittee");
                Button btnContinuousExamBack = (Button)row.FindControl("btnContinuousExamBack");
                if (!Convert.ToBoolean(lblIsContinousMarkSubmit.Text))
                {
                    btnContinuousExamBack.Visible = false;
                    btnContinuousExamBack.Enabled = false;

                    lblSubmitToCommittee.Visible = false;
                }

                else
                {
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                    {
                        btnContinuousExamBack.Visible = true;
                        btnContinuousExamBack.Enabled = true;
                        lblSubmitToCommittee.Visible = true;
                    }

                }
            }
        }

        private void RefreshGrid()
        {
            try
            {
                gvExamCommitteDashboard.DataSource = null;
                gvExamCommitteDashboard.DataBind();
            }
            catch (Exception e)
            {

            }
        }

        protected void gvExamCommitteDashboard_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "ContinuousExamBack")
                {
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                    int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                    int examId = Convert.ToInt32(ddlExam.SelectedValue);
                    List<ExamCommitteeDashboardDTO> examCommitteeDashboard = ExamMarkMasterManager.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
                    //foreach (var examCommitteeDashboardDto in examCommitteeDashboard)
                    //{

                        //ExamSetupWithExamCommittees exam = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examCommitteeDashboardDto.ExamSetupId);
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                        {
                            int acaCalSectionId = Convert.ToInt32(e.CommandArgument);
                            bool result = ExamMarkMasterManager.ExamMarkBackToTeacherByAcacalSectionId(acaCalSectionId, false);
                            if (result)
                            {
                                btnLoad_Click(null, null);
                                lblMsg.Text = "Exam marks of selected course backed to Course Teacher. Now teacher will able to submit marks of selected course.";
                            }
                            else
                            {
                                lblMsg.Text = "Exam marks of selected course could not back to Course Teacher. Please try again.";
                            }
                        }
                        else
                        {
                            lblMsg.Text = "You don't have access to do this actions.";
                            return;
                        }

                    //}
                }

                if (e.CommandName == "ExaminerExamBack1")
                {
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                    int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                    int examId = Convert.ToInt32(ddlExam.SelectedValue);
                    //List<ExamCommitteeDashboardDTO> examCommitteeDashboard = ExamMarkMasterManager.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
                    //foreach (var examCommitteeDashboardDto in examCommitteeDashboard)
                    //{

                    //    ExamSetupWithExamCommittees exam = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examCommitteeDashboardDto.ExamSetupId);
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                        {

                            int acaCalSectionId = Convert.ToInt32(e.CommandArgument);

                            var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(acaCalSectionId);
                            if (groupExaminerList != null && groupExaminerList.Any())
                            {
                                var examinerSubmission = ExamMarkMasterManager.GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(acaCalSectionId);
                                foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                                {
                                    examinerSetupStudentWise.ExaminerId = examinerSetupStudentWise.FirstExaminer;
                                    examinerSetupStudentWise.ExaminerName = examinerSetupStudentWise.ExaminerName1.CodeAndName;
                                    examinerSetupStudentWise.CreatedBy = 1; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                                    var temp = examinerSubmission.FirstOrDefault(m => m.FirstExaminerId == examinerSetupStudentWise.ExaminerId);
                                    if (temp != null && temp.FirstExaminerMarkSubmissionStatusDate!=null)
                                    {
                                        examinerSetupStudentWise.CreatedDate = (DateTime)temp.FirstExaminerMarkSubmissionStatusDate;
                                    }
                                    //examinerSetupStudentWise.CreatedDate = ; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                                }
                                legendGroupExaminerBack.InnerText = "1st Examiner Back";
                                gvGroupExaminerBack.DataSource = groupExaminerList;
                                gvGroupExaminerBack.DataBind();
                                ModalPopupGroupExaminerBack.Show();
                                return;
                            }

                            var studentCourseHistories = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                            foreach (StudentCourseHistory studentCourseHistory in studentCourseHistories)
                            {
                                var firstSecondThirdExaminer = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(studentCourseHistory.ID);
                                firstSecondThirdExaminer.FirstExaminerMarkSubmissionStatus += 1;
                                firstSecondThirdExaminer.ThirdExaminerStatus = 0;
                                firstSecondThirdExaminer.ModifiedBy = userObj.Id;
                                firstSecondThirdExaminer.ModifiedDate = DateTime.Now;

                                var isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(firstSecondThirdExaminer);

                            }
                            btnLoad_Click(null, null);
                            lblMsg.Text = "Marks has been backed to Examiner.";
                        }
                        else
                        {
                            lblMsg.Text = "You don't have access to do this actions.";
                            return;
                        }

                    //}

                }

                if (e.CommandName == "ExaminerExamBack2")
                {
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                    int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                    int examId = Convert.ToInt32(ddlExam.SelectedValue);
                    //List<ExamCommitteeDashboardDTO> examCommitteeDashboard = ExamMarkMasterManager.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
                    //foreach (var examCommitteeDashboardDto in examCommitteeDashboard)
                    //{

                    //    ExamSetupWithExamCommittees exam = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examCommitteeDashboardDto.ExamSetupId);
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                        {
                            int acaCalSectionId = Convert.ToInt32(e.CommandArgument);

                            var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(acaCalSectionId);
                            if (groupExaminerList != null && groupExaminerList.Any())
                            {
                                var examinerSubmission = ExamMarkMasterManager.GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(acaCalSectionId);
                                foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                                {
                                    examinerSetupStudentWise.ExaminerId = examinerSetupStudentWise.SecondExaminer;
                                    examinerSetupStudentWise.ExaminerName = examinerSetupStudentWise.ExaminerName2.CodeAndName;
                                    examinerSetupStudentWise.CreatedBy = 2; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                                    var temp = examinerSubmission.FirstOrDefault(m => m.SecondExaminerId == examinerSetupStudentWise.ExaminerId);
                                    if (temp != null && temp.SecondExaminerMarkSubmissionStatusDate != null)
                                    {
                                        examinerSetupStudentWise.CreatedDate = (DateTime)temp.SecondExaminerMarkSubmissionStatusDate;
                                    }
                                }
                                legendGroupExaminerBack.InnerText = "2nd Examiner Back";
                                gvGroupExaminerBack.DataSource = groupExaminerList;
                                gvGroupExaminerBack.DataBind();
                                ModalPopupGroupExaminerBack.Show();
                                return;
                            }

                            var studentCourseHistories = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                            foreach (StudentCourseHistory studentCourseHistory in studentCourseHistories)
                            {
                                var firstSecondThirdExaminer = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(studentCourseHistory.ID);
                                firstSecondThirdExaminer.SecondExaminerMarkSubmissionStatus += 1;
                                firstSecondThirdExaminer.ThirdExaminerStatus = 0;
                                firstSecondThirdExaminer.ModifiedBy = userObj.Id;
                                firstSecondThirdExaminer.ModifiedDate = DateTime.Now;

                                var isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(firstSecondThirdExaminer);
                            }
                            btnLoad_Click(null, null);
                            lblMsg.Text = "Marks has been backed to Examiner.";
                        }
                        else
                        {
                            lblMsg.Text = "You don't have access to do this actions.";
                            return;
                        }
                //    }
                }

                if (e.CommandName == "ExaminerExamBack3")
                {
                    int programId = Convert.ToInt32(ucProgram.selectedValue);
                    int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                    int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                    int examId = Convert.ToInt32(ddlExam.SelectedValue);
                    //List<ExamCommitteeDashboardDTO> examCommitteeDashboard = ExamMarkMasterManager.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
                    //foreach (var examCommitteeDashboardDto in examCommitteeDashboard)
                    //{

                    //    ExamSetupWithExamCommittees exam = ExamSetupWithExamCommitteesManager.GetByExamSetupId(examCommitteeDashboardDto.ExamSetupId);
                    if (userObj.RoleID == 1 || userObj.RoleID == 2 || userObj.RoleID == 8 || userObj.RoleID == 7)
                        {
                            int acaCalSectionId = Convert.ToInt32(e.CommandArgument);

                            var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(acaCalSectionId);
                            if (groupExaminerList != null && groupExaminerList.Any())
                            {
                                var examinerSubmission = ExamMarkMasterManager.GetExamCommitteeDashboardExtendForGroupByAcaCalSecId(acaCalSectionId);
                                foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                                {
                                    examinerSetupStudentWise.ExaminerId = examinerSetupStudentWise.ThirdExaminer;
                                    examinerSetupStudentWise.ExaminerName = examinerSetupStudentWise.ExaminerName3.CodeAndName;
                                    examinerSetupStudentWise.CreatedBy = 3; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                                    var temp = examinerSubmission.FirstOrDefault(m => m.ThirdExaminerId == examinerSetupStudentWise.ExaminerId);
                                    if (temp != null && temp.ThirdExaminerMarkSubmissionStatusDate != null)
                                    {
                                        examinerSetupStudentWise.CreatedDate = (DateTime)temp.ThirdExaminerMarkSubmissionStatusDate;
                                    }
                                }
                                legendGroupExaminerBack.InnerText = "3rd Examiner Back";
                                gvGroupExaminerBack.DataSource = groupExaminerList;
                                gvGroupExaminerBack.DataBind();
                                ModalPopupGroupExaminerBack.Show();
                                return;
                            }

                            var studentCourseHistories = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);
                            foreach (StudentCourseHistory studentCourseHistory in studentCourseHistories)
                            {
                                var firstSecondThirdExaminer = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(studentCourseHistory.ID);
                                firstSecondThirdExaminer.ThirdExaminerMarkSubmissionStatus += 1;
                                firstSecondThirdExaminer.ModifiedBy = userObj.Id;
                                firstSecondThirdExaminer.ModifiedDate = DateTime.Now;

                                var isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(firstSecondThirdExaminer);
                            }
                            btnLoad_Click(null, null);
                            lblMsg.Text = "Marks has been backed to Examiner.";
                        }
                        else
                        {
                            lblMsg.Text = "You don't have access to do this actions.";
                            return;
                        }
                    }
                //}
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        protected void cbSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;

                foreach (GridViewRow row in gvExamCommitteDashboard.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("cbSelect");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }


        protected void btnTabulationProcess_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gvExamCommitteDashboard.Rows)
            {
                Label lblAcacalSectionId = (Label)row.FindControl("lblAcacalSectionId");
                Label lblCourseId = (Label)row.FindControl("lblCourseId");
                Label lblVersionId = (Label)row.FindControl("lblVersionId");
                CheckBox ckBox = (CheckBox)row.FindControl("cbSelect");
                //ckBox.Checked = chk.Checked;
                int acacalSectionId = Convert.ToInt32(lblAcacalSectionId.Text);
                int courseId = Convert.ToInt32(lblCourseId.Text);
                int versionId = Convert.ToInt32(lblVersionId.Text);
                bool chk = ckBox.Checked;
                if (chk)
                {
                    ExamTemplateManager.ProcessFirstSecondThirdExaminerMarkToExamMark(acacalSectionId);
                    ExamTemplateManager.GetExamResultTabulationDataTable(courseId, versionId, acacalSectionId, userObj.Id);
                }
            }
            lblMsg.Text = "Tabulation processed successfully.";
            ExecuteResultProcess();

            
        }

        protected void ExecuteResultProcess()
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
            {
                List<SqlParameter> parameterListV3 = new List<SqlParameter>();
                parameterListV3.Add(new SqlParameter { ParameterName = "ProgramId", SqlDbType = SqlDbType.Int, Value = programId });
                parameterListV3.Add(new SqlParameter { ParameterName = "ExamDetailId", SqlDbType = SqlDbType.Int, Value = examId });


                string spNameV3 = "StudentResultProcessV2";

                string connStrV3 = ConfigurationManager.ConnectionStrings["Connection String"].ConnectionString;

                DataTable dtV3 = new DataTable();
                using (SqlConnection connection = new SqlConnection(connStrV3))
                {
                    SqlCommand command = new SqlCommand(spNameV3, connection);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    foreach (var item in parameterListV3)
                    {
                        command.Parameters.Add(item);
                    }

                    try
                    {
                        connection.Open();
                        SqlDataReader reader;
                        reader = command.ExecuteReader();
                        dtV3.Load(reader);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
            }
            else
            {
                lblMsg.Text = "Please select program, year, semester and exam.";
            }
        }

        protected void btnExaminer_Click(object sender, EventArgs e)
        {
            Button btl = (Button)sender;
            int acaCalId = Convert.ToInt32(btl.CommandArgument);
            ExaminorSetups examinerSetup = ExaminorSetupsManager.GetByAcaCalSecId(acaCalId);
            lblID.Text = "0";
            btnExaminerAssign.Text = "Assign Examiner";
            if (examinerSetup != null)
            {
                lblID.Text = examinerSetup.ID.ToString();
                if (examinerSetup.ThirdExaminor > 0)
                {
                    btnExaminerAssign.Text = "Update Examiner";
                }

                AcademicCalenderSection academicCalenderSection = AcademicCalenderSectionManager.GetById(Convert.ToInt32(examinerSetup.AcaCalSectionId));

                if (academicCalenderSection != null)
                {
                    TxtSection.Text = academicCalenderSection.Course.Title + "_" + academicCalenderSection.SectionName;
                    LoadTeacherDropdown();


                    if (ddlEx3.Items.FindByValue(examinerSetup.ThirdExaminor.ToString()) != null)
                    {
                        ddlEx3.SelectedValue = examinerSetup.ThirdExaminor.ToString();
                    }

                }
            }
            ModalPopupExtender1.Show();

        }

        private void LoadTeacherDropdown()
        {

            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            List<Employee> emp = EmployeeManager.GetAllByTypeId(1).Where(x => x.DeptID == departmentId).ToList();


            ddlEx3.Items.Clear();
            ddlEx3.Items.Add(new ListItem("-Select Examiner-", "0"));
            ddlEx3.AppendDataBoundItems = true;

            if (emp.Any())
            {
                foreach (Employee employee in emp)
                {
                    ddlEx3.Items.Add(new ListItem(employee.CodeAndName, employee.EmployeeID.ToString()));
                }
            }
        }

        protected void btnExaminerAssign_OnClick(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(lblID.Text);
            int examiner3 = Convert.ToInt32(ddlEx3.SelectedValue);
            ExaminorSetups obj = ExaminorSetupsManager.GetById(id);
            if (obj != null)
            {
                try
                {
                    obj.ThirdExaminor = examiner3;

                    obj.ModifiedBy = userObj.Id;
                    obj.ModifiedDate = DateTime.Now;
                    bool update = ExaminorSetupsManager.Update(obj);
                    if (update == true)
                    {

                        ShowAlertMessage("Saved / Updated successfully.");

                        //#region Log Insert
                        //try
                        //{
                        //    LogGeneralManager.Insert(
                        //       DateTime.Now,
                        //       BaseAcaCalCurrent.CalendarUnitType_Code + " " + BaseAcaCalCurrent.Year.ToString(),
                        //       BaseAcaCalCurrent.FullCode,
                        //       BaseCurrentUserObj.LogInID,
                        //       "",
                        //       "",
                        //       "Publish date update",
                        //       BaseCurrentUserObj.LogInID + " Publish date update manually for " + Convert.ToString(ucProgram.selectedText),
                        //       "normal",
                        //       _pageId,
                        //       _pageName,
                        //       _pageUrl,
                        //       Convert.ToString(ucProgram.selectedText));
                        //}
                        //catch (Exception ex) { }

                        //#endregion
                    }
                    btnLoad_Click(null, null);
                }
                catch
                {
                }

            }

        }

        protected void btnViewFirstExaminer_Click(object sender, EventArgs e)
        {
            //this.ModalPopupExtenderFirstExaminer.Show();

            Button btn = (Button)sender;
            //int acaCalSectionId = int.Parse(btn.CommandArgument.ToString());

            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            //int acaCalSectionId = Convert.ToInt32(btn.CommandArgument);
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            //int examTemplateItemId = int.Parse(commandArgs[2]);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);
            try
            {
                if (acaCalSectionId > 0)
                {
                    var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(acaCalSectionId);
                    if (groupExaminerList != null && groupExaminerList.Any())
                    {
                        foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                        {
                            examinerSetupStudentWise.ExaminerId = examinerSetupStudentWise.FirstExaminer;
                            examinerSetupStudentWise.ExaminerName = examinerSetupStudentWise.ExaminerName1.CodeAndName;
                            examinerSetupStudentWise.CreatedBy = 1; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                        }

                        legendGroupExaminerView.InnerText = "1st Examiner View";
                        gvGroupExaminerView.DataSource = groupExaminerList;
                        gvGroupExaminerView.DataBind();
                        ModalPopupGroupExaminerView.Show();
                        return;
                    }
                    var examinorSetups = ExaminorSetupsManager.GetByAcaCalSecId(acaCalSectionId);
                    string param = acaCalSectionId.ToString() + "_" + examSetupId.ToString() + "_" + 1 + "_" + examId.ToString() + "_" + examinorSetups.FirstExaminer;
                    string url = "Report/RPTExamMarkEntry.aspx?val=" + param;
                    //Response.Redirect(url, false);
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenWindow", "window.open('" + url + "','_newtab');", true);
                    //return;
                    #region Get StudentCourseHistory
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                    //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                    //{
                    //    ShowAlertMessage("No student is found against this course or section !!");
                    //    return;
                    //}
                    //#endregion

                    //#region Get GridView Details
                    //List<ExamMarkGridViewDTO> emgvDtoList = new List<ExamMarkGridViewDTO>();

                    //foreach (var studentCourseHistory in stdCourseHistoryList)
                    //{
                    //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                    //    Student s = StudentManager.GetById(studentCourseHistory.StudentID);
                    //    ExamMarkFirstSecondThirdExaminer em = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(studentCourseHistory.ID);

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

                    //        emgvDtoList.Add(model);
                    //    }
                    //}

                    //if (emgvDtoList != null && emgvDtoList.Count > 0)
                    //{
                    //    gvExamMarkFirstExaminerView.DataSource = emgvDtoList;
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
                    #endregion



                }
                else
                {
                    ShowAlertMessage("Something went wrong while Loading First Examiner Exam Mark Data");
                }
            }
            catch (Exception ex)
            {

                ShowAlertMessage("Something went wrong while Loading First Examiner Exam Mark Data; Error: " + ex.Message.ToString());
            }
        }

        protected void btnViewSecondExaminer_Click(object sender, EventArgs e)
        {
            //this.ModalPopupExtenderSecondExaminer.Show();

            Button btn = (Button)sender;
            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);
            try
            {
                if (acaCalSectionId > 0 && examSetupId > 0)
                {

                    var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(acaCalSectionId);
                    if (groupExaminerList != null && groupExaminerList.Any())
                    {
                        foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                        {
                            examinerSetupStudentWise.ExaminerId = examinerSetupStudentWise.SecondExaminer;
                            examinerSetupStudentWise.ExaminerName = examinerSetupStudentWise.ExaminerName2.CodeAndName;
                            examinerSetupStudentWise.CreatedBy = 2; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                        }
                        legendGroupExaminerView.InnerText = "2nd Examiner View";
                        gvGroupExaminerView.DataSource = groupExaminerList;
                        gvGroupExaminerView.DataBind();
                        ModalPopupGroupExaminerView.Show();
                        return;
                    }
                    var examinorSetups = ExaminorSetupsManager.GetByAcaCalSecId(acaCalSectionId);
                    string param = acaCalSectionId.ToString() + "_" + examSetupId.ToString() + "_" + 2 + "_" +
                                   examId.ToString() + "_" + examinorSetups.SecondExaminor;
                    string url = "Report/RPTExamMarkEntry.aspx?val=" + param;
                    //Response.Redirect(url, false);
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenWindow", "window.open('" + url + "','_newtab');", true);

                    #region Get StudentCourseHistory
                    //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                    //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                    //{
                    //    ShowAlertMessage("No student is found against this course or section !!");
                    //    return;
                    //}
                    //#endregion

                    //#region Get GridView Details
                    //List<ExamMarkGridViewDTO> emgvDTOList = new List<ExamMarkGridViewDTO>();

                    //foreach (var studentCourseHistory in stdCourseHistoryList)
                    //{
                    //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                    //    Student student = StudentManager.GetById(studentCourseHistory.StudentID);
                    //    ExamMarkFirstSecondThirdExaminer examMarkFirstSecondThirdExaminer = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(studentCourseHistory.ID);

                    //    if (student != null && examMarkFirstSecondThirdExaminer != null)
                    //    {
                    //        model.ExamMarkId = examMarkFirstSecondThirdExaminer.ID;
                    //        model.Roll = student.Roll;
                    //        model.Name = student.BasicInfo.FullName;
                    //        try
                    //        {
                    //            if (!string.IsNullOrEmpty(examMarkFirstSecondThirdExaminer.SecondExaminerMark.ToString()))
                    //            {
                    //                model.Mark = Convert.ToDecimal(examMarkFirstSecondThirdExaminer.SecondExaminerMark);
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

                    //        model.PresentAbsent = Convert.ToBoolean(examMarkFirstSecondThirdExaminer.IsAbsent) == true ? "Absent" : "Present";

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
                    #endregion

                }
                else
                {
                    ShowAlertMessage("Something went wrong while Loading Second Examiner Exam Mark Data");
                }
            }
            catch (Exception ex)
            {

                ShowAlertMessage("Something went wrong while Loading Second Examiner Exam Mark Data; Error: " + ex.Message.ToString());
            }
        }

        protected void btnViewThirdExaminer_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
            int acaCalSectionId = int.Parse(commandArgs[0]);
            int examSetupId = int.Parse(commandArgs[1]);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);
            try
            {
                if (acaCalSectionId > 0 && examSetupId > 0)
                {
                    var groupExaminerList = ExaminerSetupStudentWiseManager.GetExaminersByAcaCalSectionId(acaCalSectionId);
                    if (groupExaminerList != null && groupExaminerList.Any())
                    {
                        foreach (ExaminerSetupStudentWise examinerSetupStudentWise in groupExaminerList)
                        {
                            if (examinerSetupStudentWise.ThirdExaminer > 0)
                            {
                                examinerSetupStudentWise.ExaminerId = examinerSetupStudentWise.ThirdExaminer;
                                examinerSetupStudentWise.ExaminerName = examinerSetupStudentWise.ExaminerName3.CodeAndName;
                                examinerSetupStudentWise.CreatedBy = 3; // Here CreatedBy is used to Identify the examinerNo (1st, 2nd or 3rd)
                            }
                            
                        }
                        legendGroupExaminerView.InnerText = "3rd Examiner View";
                        gvGroupExaminerView.DataSource = groupExaminerList;
                        gvGroupExaminerView.DataBind();
                        ModalPopupGroupExaminerView.Show();
                        return;
                    }
                    var examinorSetups = ExaminorSetupsManager.GetByAcaCalSecId(acaCalSectionId);
                    string param = acaCalSectionId.ToString() + "_" + examSetupId.ToString() + "_" + 3 + "_" + examId.ToString() + "_" + examinorSetups.ThirdExaminor;
                    string url = "Report/RPTExamMarkEntry.aspx?val=" + param;
                    ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenWindow", "window.open('" + url + "','_newtab');", true);
                }
                else
                {
                    ShowAlertMessage("Something went wrong while Loading third Examiner View");
                }
            }
            catch (Exception ex)
            {

                ShowAlertMessage("Something went wrong while Loading third Examiner View; Error: " + ex.Message.ToString());
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        #region Tabulation Data

        protected void btnLoadTabulation_Click(object sender, EventArgs e)
        {
            lblMsg.Text = string.Empty;

            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = 0;

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    examId = Convert.ToInt32(ddlExam.SelectedValue);
                    if (examId > 0)
                    {

                        LoadTabulationData(programId, yearNo, semesterNo, examId);

                    }
                    else
                        lblMsg.Text = "Please select exam.";
                }
                else
                {
                    lblMsg.Text = "Please select program, year, semester and exam.";
                }
            }
            catch (Exception)
            {
                throw;

            }
        }

        private void LoadTabulationData(int programId, int yearNo, int semesterNo, int examId)
        {
            List<RTabulationData> TabulationDataList = ExamMarkMasterManager.GetTabulationDataByProgramYearSemesterExam(programId, yearNo, semesterNo, examId);
            if (TabulationDataList != null && TabulationDataList.Count > 0)
            {
                LoadReport(TabulationDataList);
                DownloadReport();

            }
            else
            {
                lblMsg.Text = "No Data Found!";

            }
        }

        private void LoadReport(List<RTabulationData> TabulationDataList)
        {
            try
            {
                string Exam = ddlExam.SelectedItem.Text.ToString();
                Exam = Exam.Substring(Exam.Length - 4, 4);

                ReportParameter p1 = new ReportParameter("Program", ucProgram.selectedText.ToString());
                ReportParameter p2 = new ReportParameter("Year", ddlYearNo.SelectedItem.Text.ToString());
                ReportParameter p3 = new ReportParameter("Semester", ddlSemesterNo.SelectedItem.Text.ToString());
                ReportParameter p4 = new ReportParameter("Exam", Exam);
                ReportParameter p5 = new ReportParameter("Department", ucDepartment.selectedText.ToString());




                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/TabulationData.rdlc");
                this.ReportViewer1.LocalReport.EnableExternalImages = true;
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5 });

                ReportDataSource rds = new ReportDataSource("DataSet1", TabulationDataList);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds);
                ReportViewer1.Visible = false;
            }
            catch (Exception ex)
            {

            }
        }

        private void DownloadReport()
        {
            try
            {
                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = this.ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                using (FileStream fs = new FileStream(Server.MapPath("~/Upload/ReportPDF/" + "Tabulation" + ".pdf"), FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }
                string fileName = "Tabulation" + ".pdf";

                string FilePath = Server.MapPath("~/Upload/ReportPDF/");


                Response.ContentType = "application/octet-stream";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
                Response.TransmitFile(Server.MapPath("~/Upload/ReportPDF/" + fileName));

                HttpCookie cookie = new HttpCookie("ExcelDownloadFlag");
                cookie.Value = "Flag";
                cookie.Expires = DateTime.Now.AddDays(1);
                Response.AppendCookie(cookie);

                Response.End();
            }
            catch (Exception ex)
            {

            }

        }

        #endregion

        protected void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = 0;

                if (programId > 0 && yearNo > 0 && semesterNo > 0)
                {
                    examId = Convert.ToInt32(ddlExam.SelectedValue);
                    if (examId > 0)
                    {

                        List<RTabulationData> TabulationDataList = ExamMarkMasterManager.GetTabulationDataByProgramYearSemesterExam(programId, yearNo, semesterNo, examId);
                        if (TabulationDataList != null && TabulationDataList.Count > 0)
                        {

                            LoadReport(TabulationDataList);

                            if (File.Exists(Server.MapPath("~/Module/pdf/" + programId + "_" + examId + "_" + "Tabulation" + ".pdf")))
                            {
                                File.Delete(Server.MapPath("~/Module/pdf/" + programId + "_" + examId + "_" + "Tabulation" + ".pdf"));
                            }


                            Warning[] warnings;
                            string[] streamids;
                            string mimeType;
                            string encoding;
                            string filenameExtension;

                            byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                            ReportViewer1.LocalReport.Refresh();

                            using (FileStream fs = new FileStream(Server.MapPath("~/Module/pdf/" + programId + "_" + examId + "_" + "Tabulation" + ".pdf"), FileMode.Create))
                            {
                                fs.Write(bytes, 0, bytes.Length);
                            }

                            string url = string.Format("/brur/Module/ViewPDF.aspx?FN={0}", programId + "_" + examId + "_" + "Tabulation.pdf");
                            string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
                            this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);


                            //Open in same Window
                            //string path = Server.MapPath("~/Module/pdf/" + programId + "_" + examId + "_" + "Tabulation" + ".pdf");
                            //WebClient client = new WebClient();
                            //Byte[] buffer = client.DownloadData(path);
                            //if (buffer != null)
                            //{
                            //    Response.ContentType = "application/pdf";
                            //    Response.AddHeader("content-length", buffer.Length.ToString());
                            //    Response.BinaryWrite(buffer);
                            //}

                        }
                        else
                        {
                            lblMsg.Text = "No Data Found!";

                        }

                    }
                    else
                        lblMsg.Text = "Please select exam.";
                }
                else
                {
                    lblMsg.Text = "Please select program, year, semester and exam.";
                }
            }
            catch (Exception)
            {
                throw;

            }
        }

        [System.Web.Services.WebMethod]
        public static string GetExamMarks(string strAcaCalSectionId)
        {

            //string[] courseVersionSection = courseSection.Split('_');
            //int courseId = Convert.ToInt32(courseVersionSection[0]);
            //int versionId = Convert.ToInt32(courseVersionSection[1]);
            int acaCalSectionId = Convert.ToInt32(strAcaCalSectionId);

            var academicCalenderSection = AcademicCalenderSectionManager.GetById(acaCalSectionId);

            List<ExamResultDTO> examResultDTOList = ExamTemplateManager.GetExamResultDTO(academicCalenderSection.CourseID, academicCalenderSection.VersionID, acaCalSectionId).ToList();
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

        protected void btnThirdExaminerStudentView_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.ModalPopupThirdExaminerStudentList.Show();
                Button btn = (Button)sender;
                string[] commandArgs = btn.CommandArgument.ToString().Split(new char[] { ',' });
                int acaCalSectionId = int.Parse(commandArgs[0]);
                if (acaCalSectionId > 0)
                {
                    var studentList = ExamMarkFirstSecondThirdExaminerManager.GetThirdExaminerEligibleStudentListByAcaCalSecId(acaCalSectionId);
                    if (studentList.Any())
                    {
                        ThirdExaminerStudentListGridView.DataSource = studentList;
                        ThirdExaminerStudentListGridView.DataBind();
                    }
                    else
                    {
                        ThirdExaminerStudentListGridView.DataSource = null;
                        ThirdExaminerStudentListGridView.DataBind();
                    }
                }
            }
            catch (Exception exception)
            {

            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);

                if (programId > 0 && yearNo > 0 && semesterNo > 0 && examId > 0)
                {
                    this.ModalPopupExaminerSubmissionDate.Show();
                    List<ExamCommitteeDashboardDTO> examCommitteeDashboard = ExamMarkMasterManager.GetExamCommitteeDashboard(programId, yearNo, semesterNo, examId);
                    foreach (var examCommitteeDashboardDto in examCommitteeDashboard)
                    {
                        var tempTeacher = ExamMarkMasterManager.GetExaminerByAcaCalSecId(examCommitteeDashboardDto.AcaCal_SectionID);
                        if (tempTeacher != null)
                        {
                            examCommitteeDashboardDto.FirstExaminerName = tempTeacher.FirstExaminerName;
                            examCommitteeDashboardDto.SecondExaminerName = tempTeacher.SecondExaminerName;
                            examCommitteeDashboardDto.ThirdExaminerName = tempTeacher.ThirdExaminerName;
                        }
                        var examMarkSubmissionDate = ExamMarkSubmissionDateManager.GetByAcaCalSecId(examCommitteeDashboardDto.AcaCal_SectionID);
                        if (examMarkSubmissionDate != null)
                        {
                            examCommitteeDashboardDto.FirstExaminerMarkSubmissionDate = examMarkSubmissionDate.FirstExaminerSubmissionDate;
                            examCommitteeDashboardDto.SecondExaminerMarkSubmissionDate = examMarkSubmissionDate.SecondExaminerSubmissionDate;
                            examCommitteeDashboardDto.ThirdExaminerMarkSubmissionDate = examMarkSubmissionDate.ThirdExaminerSubmissionDate;
                        }
                    }

                    if (examCommitteeDashboard.Any())
                    {
                        submissionDateGridView.DataSource = examCommitteeDashboard;
                        submissionDateGridView.DataBind();
                    }

                }
                else
                {
                    btnTabulationProcess.Visible = true;
                    btnLoadTabulation.Visible = true;
                    lblMsg.Text = "Please select program, year, semester and exam.";
                }

            }
            catch (Exception exception)
            {

            }
        }

        protected void sdSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                this.ModalPopupExaminerSubmissionDate.Show();
                CheckBox chk = (CheckBox)sender;

                foreach (GridViewRow row in submissionDateGridView.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("sdSelect");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSubmissionDate()
        {
            //string strSubmissionDate1 = DateTime.Now.ToString("dd/MM/yyyy");
            //txtSubmissionDate1.Text = strSubmissionDate1;

            //string strSubmissionDate2 = DateTime.Now.ToString("dd/MM/yyyy");
            //txtSubmissionDate2.Text = strSubmissionDate2;

            //string strSubmissionDate3 = DateTime.Now.ToString("dd/MM/yyyy");
            //txtSubmissionDate3.Text = strSubmissionDate3;
        }

        protected void saveSubmissionDate_OnClick(object sender, EventArgs e)
        {
            try
            {
                this.ModalPopupExaminerSubmissionDate.Show();

                foreach (GridViewRow row in submissionDateGridView.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("sdSelect");
                    if (ckBox.Checked)
                    {
                        ExamMarkSubmissionDate examMarkSubmissionDate = new ExamMarkSubmissionDate();
                        Label popLabelAcaCalSectionId = (Label)row.FindControl("popLabelAcacalSectionId");
                        if (!String.IsNullOrEmpty(txtSubmissionDate1.Text))
                        {
                            DateTime submissionDate1 = DateTime.ParseExact(txtSubmissionDate1.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                            examMarkSubmissionDate.FirstExaminerSubmissionDate = submissionDate1;
                        }
                        if (!String.IsNullOrEmpty(txtSubmissionDate2.Text))
                        {
                            DateTime submissionDate2 = DateTime.ParseExact(txtSubmissionDate2.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                            examMarkSubmissionDate.SecondExaminerSubmissionDate = submissionDate2;
                        }
                        if (!String.IsNullOrEmpty(txtSubmissionDate3.Text))
                        {
                            DateTime submissionDate3 = DateTime.ParseExact(txtSubmissionDate3.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                            examMarkSubmissionDate.ThirdExaminerSubmissionDate = submissionDate3;
                        }
                        examMarkSubmissionDate.AcaCalSectionId = Convert.ToInt32(popLabelAcaCalSectionId.Text);

                        examMarkSubmissionDate.CreatedBy = userObj.Id;
                        examMarkSubmissionDate.CreatedDate = DateTime.Now;
                        var examMarkSubmission = ExamMarkSubmissionDateManager.GetByAcaCalSecId(examMarkSubmissionDate.AcaCalSectionId);
                        if (examMarkSubmission == null)
                        {
                            var inserted = ExamMarkSubmissionDateManager.Insert(examMarkSubmissionDate);
                        }
                        else
                        {
                            examMarkSubmission.FirstExaminerSubmissionDate = examMarkSubmissionDate.FirstExaminerSubmissionDate;
                            examMarkSubmission.SecondExaminerSubmissionDate = examMarkSubmissionDate.SecondExaminerSubmissionDate;
                            examMarkSubmission.ThirdExaminerSubmissionDate = examMarkSubmissionDate.ThirdExaminerSubmissionDate;
                            examMarkSubmission.ModifiedBy = userObj.Id;
                            examMarkSubmission.ModifiedDate = DateTime.Now;
                            var inserted = ExamMarkSubmissionDateManager.Update(examMarkSubmission);
                        }

                        btnAdd_Click(null, null);
                    }
                }
            }
            catch (Exception exception)
            {

            }
        }

        protected void txtSubmissionDate3_OnTextChanged(object sender, EventArgs e)
        {
            this.ModalPopupExaminerSubmissionDate.Show();
        }

        protected void btnCancel3_OnClick(object sender, EventArgs e)
        {
            btnLoad_Click(null, null);
        }

        protected void btnContinuousView_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string commandArgs = btn.CommandArgument.ToString();
                int acaCalSectionId = Convert.ToInt32(commandArgs);
                var academicCalenderSection = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                int courseId = academicCalenderSection.CourseID;
                int versionId = academicCalenderSection.VersionID;

                if (acaCalSectionId > 0)
                {
                    ExamTemplateManager.ProcessContinuousAssessmentMark(acaCalSectionId, userObj.Id);


                    List<rContinuousAssessmentMark> list = ExamTemplateManager.GetContinuousMarkBySectionID(acaCalSectionId);
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
                        //ResultEntryGrid.Visible = false;
                        //ResultEntryGrid.DataSource = null;
                        //ResultEntryGrid.DataBind();
                        //btnFinalSubmitAll.Visible = false;
                        //lblExamMark.Text = string.Empty;

                        string Exam = ddlExam.SelectedItem.ToString();
                        string[] ExamNameSplit = Exam.Split(' ');
                        string ExamName = "", ExamYear = "";
                        if (ExamNameSplit[4] != "")
                            ExamName = ExamNameSplit[4] + " Examination";
                        if (ExamNameSplit[5] != "")
                            ExamYear = ExamNameSplit[5];

                        ReportParameter p1 = new ReportParameter("Year", ddlYearNo.SelectedItem.Text.ToString());
                        ReportParameter p2 = new ReportParameter("Semester", ddlSemesterNo.SelectedItem.Text.ToString());
                        ReportParameter p3 = new ReportParameter("Department", ucDepartment.selectedText.ToString());
                        ReportParameter p4 = new ReportParameter("Course", crs.FormalCode.ToString());
                        ReportParameter p5 = new ReportParameter("TeacherName", teacherName.ToString());
                        ReportParameter p6 = new ReportParameter("DepartmentName", teacherDept.ToString());
                        ReportParameter p7 = new ReportParameter("SessionName", countinousMarkTeacherInfoObj.SessionName.ToString());
                        ReportParameter p8 = new ReportParameter("ExamMark", countinousMarkTeacherInfoObj.ExamMark.ToString());
                        ReportParameter p9 = new ReportParameter("ExamName", ExamName);
                        ReportParameter p10 = new ReportParameter("ExamYear", ExamYear);
                        ReportParameter p11 = new ReportParameter("CourseTitle", crs.Title.ToString());




                        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/result/Report/RptContinuousAssessmentMark.rdlc");
                        ReportDataSource rds = new ReportDataSource("DataSet", list);


                        this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5, p6, p7, p8, p9, p10, p11 });

                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.LocalReport.DataSources.Add(rds);


                        ReportViewer1.Visible = false;




                        #region Pdf File Create And View In New Page
                        string Name = "RptContinuousAssessmentMark" + acaCalSectionId;

                        if (File.Exists(Server.MapPath("~/Module/pdf/" + Name + ".pdf")))
                        {
                            File.Delete(Server.MapPath("~/Module/pdf/" + Name + ".pdf"));
                        }


                        Warning[] warnings;
                        string[] streamids;
                        string mimeType;
                        string encoding;
                        string filenameExtension;

                        byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);
                        ReportViewer1.LocalReport.Refresh();

                        using (FileStream fs = new FileStream(Server.MapPath("~/Module/pdf/" + Name + ".pdf"), FileMode.Create))
                        {
                            fs.Write(bytes, 0, bytes.Length);
                        }

                        string url = string.Format("/brur/Module/ViewPDF.aspx?FN={0}", Name + ".pdf");
                        string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
                        this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);

                        #endregion





                    }
                    else
                    {
                        ReportViewer1.LocalReport.DataSources.Clear();
                        ReportViewer1.Visible = false;
                        return;
                    }

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        protected void btnPopGroupExaminerView_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                Label lblAcaCalSectionId = (Label)row.FindControl("lblPopGroupExaminerViewAcaCalSectionId");
                Label lblExaminerId = (Label)row.FindControl("lblPopGroupExaminerIdView");
                Label lblExaminerNo = (Label)row.FindControl("lblPopGroupExaminerNoView");
                int acaCalSectionId = Convert.ToInt32(lblAcaCalSectionId.Text);
                int examinerId = Convert.ToInt32(lblExaminerId.Text);
                int examinerNo = Convert.ToInt32(lblExaminerNo.Text);
                int examSetupDetailId = Convert.ToInt32(btn.CommandArgument);
                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                try
                {
                    if (acaCalSectionId > 0)
                    {
                        var examSetupDetail = ExamSetupDetailManager.GetById(examSetupDetailId);
                        string param = acaCalSectionId + "_" + examSetupDetail.ExamSetupId + "_" + examinerNo + "_" + examId + "_" + examinerId;
                        string url = "Report/RPTExamMarkEntry.aspx?val=" + param;
                        //Response.Redirect(url, false);
                        ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "OpenWindow", "window.open('" + url + "','_newtab');", true);
                        //return;
                        #region Get StudentCourseHistory
                        //List<StudentCourseHistory> stdCourseHistoryList = StudentCourseHistoryManager.GetAllByAcaCalSectionId(acaCalSectionId);

                        //if (stdCourseHistoryList == null || stdCourseHistoryList.Count == 0)
                        //{
                        //    ShowAlertMessage("No student is found against this course or section !!");
                        //    return;
                        //}
                        //#endregion

                        //#region Get GridView Details
                        //List<ExamMarkGridViewDTO> emgvDtoList = new List<ExamMarkGridViewDTO>();

                        //foreach (var studentCourseHistory in stdCourseHistoryList)
                        //{
                        //    ExamMarkGridViewDTO model = new ExamMarkGridViewDTO();

                        //    Student s = StudentManager.GetById(studentCourseHistory.StudentID);
                        //    ExamMarkFirstSecondThirdExaminer em = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(studentCourseHistory.ID);

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

                        //        emgvDtoList.Add(model);
                        //    }
                        //}

                        //if (emgvDtoList != null && emgvDtoList.Count > 0)
                        //{
                        //    gvExamMarkFirstExaminerView.DataSource = emgvDtoList;
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
                        #endregion



                    }
                    else
                    {
                        ShowAlertMessage("Something went wrong while Loading First Examiner Exam Mark Data");
                    }
                }
                catch (Exception ex)
                {

                    ShowAlertMessage("Something went wrong while Loading First Examiner Exam Mark Data; Error: " + ex.Message.ToString());
                }
            }
            catch (Exception exception)
            {


            }
        }
        protected void btnPopGroupExaminerBack_OnClick(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                GridViewRow row = (GridViewRow)btn.NamingContainer;
                Label lblAcaCalSectionId = (Label)row.FindControl("lblPopGroupExaminerBackAcaCalSectionId");
                Label lblExaminerId = (Label)row.FindControl("lblPopGroupExaminerIdBack");
                Label lblExaminerNo = (Label)row.FindControl("lblPopGroupExaminerNoBack");
                int acaCalSectionId = Convert.ToInt32(lblAcaCalSectionId.Text);
                int examinerId = Convert.ToInt32(lblExaminerId.Text);
                int examinerNo = Convert.ToInt32(lblExaminerNo.Text);
                var examinerSetupStudentWiseList = ExaminerSetupStudentWiseManager.ExaminerSetupStudentWiseGetByAcaCalSectionIdExaminerIdExaminerNo(acaCalSectionId, examinerId, examinerNo);
                foreach (ExaminerSetupStudentWise examinerSetupStudentWise in examinerSetupStudentWiseList)
                {
                    var firstSecondThirdExaminer = ExamMarkFirstSecondThirdExaminerManager.GetByCourseHistoryId(examinerSetupStudentWise.StudentCourseHistoryId);
                    if (examinerNo == 1)
                    {
                        firstSecondThirdExaminer.FirstExaminerMarkSubmissionStatus += 1;
                        firstSecondThirdExaminer.ThirdExaminerStatus = 0;
                    }
                    if (examinerNo == 2)
                    {
                        firstSecondThirdExaminer.SecondExaminerMarkSubmissionStatus += 1;
                        firstSecondThirdExaminer.ThirdExaminerStatus = 0;
                    }
                    if (examinerNo == 3)
                    {
                        firstSecondThirdExaminer.ThirdExaminerMarkSubmissionStatus += 1;
                    }
                    firstSecondThirdExaminer.ModifiedBy = userObj.Id;
                    firstSecondThirdExaminer.ModifiedDate = DateTime.Now;
                    var isUpdate = ExamMarkFirstSecondThirdExaminerManager.Update(firstSecondThirdExaminer);
                }
                btnLoad_Click(null, null);
                lblMsg.Text = "Marks has been backed to Examiner successfully.";
            }
            catch (Exception exception)
            {


            }
        }

        //protected void btnPopGroupExaminer2View_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception exception)
        //    {


        //    }
        //}
        //protected void btnPopGroupExaminer2Back_OnClick(object sender, EventArgs e)
        //{
        //    try
        //    {

        //    }
        //    catch (Exception exception)
        //    {


        //    }
        //}

    }
}