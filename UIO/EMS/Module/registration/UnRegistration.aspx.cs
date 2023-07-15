using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace EMS.Module.registration
{
    public partial class UnRegistration : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                //lblMsg.Text = string.Empty;
          
                //lblMsg.Text = string.Empty;
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
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            // lblMsg.Text = string.Empty;
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
            //lblMsg.Text = string.Empty;
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
            // lblMsg.Text = string.Empty;
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
            //lblMsg.Text = string.Empty;
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
            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            List<Course> courseList = CourseManager.GetCoursesByExamId(examId);
            ddlCourse.Items.Clear();
            ddlCourse.AppendDataBoundItems = true;
            ddlCourse.Items.Add(new ListItem("-Select-", "0"));
            if (courseList != null && courseList.Count > 0)
            {
                ddlCourse.DataTextField = "FormalCode";
                ddlCourse.DataValueField = "CourseID";

                ddlCourse.DataSource = courseList;
                ddlCourse.DataBind();

            }

            //  lblMsg.Text = string.Empty;
            //try
            //{
            //    btnLoad_Click(null, null);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            
            string message = "";
            if(ddlCourse.SelectedValue == "" || ddlExam.SelectedValue == "")
            {
                message = "Input field is not valid.";
            }
            else if (Convert.ToInt32(ddlCourse.SelectedValue) == 0 || Convert.ToInt32(ddlExam.SelectedValue) == 0)
            {
                message = "Input field is not valid.";

            }
            else
            {
                int courseId = Convert.ToInt32(ddlCourse.SelectedValue);
                string courseCode = ddlCourse.SelectedItem.Text;
                int examId = Convert.ToInt32(ddlExam.SelectedValue);
                List<Student> studentList = StudentManager.GetStudentsByExamIdCourseId(examId, courseId);

                message = "Course - " + courseCode + ", Students Count - " + studentList.Count.ToString();
                gvStudentList.DataSource = null;
                gvStudentList.DataSource = studentList;
                gvStudentList.DataBind();
                stdButton.Visible = true;
            }
         
            //chkStudentList.Items.Clear();
            //foreach (Student item in studentList)
            //{
            //    chkStudentList.Items.Add(new ListItem(item.Roll, item.StudentID.ToString()));
            //}
            lblDeleteMsg.Visible = true;
            lblDeleteMsg.ForeColor = System.Drawing.Color.Red;
            lblDeleteMsg.Text = message;
          

          }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;


                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {


            }
        }
        protected void btnLoad_StudentSubmit(object sender, EventArgs e)
        {
            int courseId = Convert.ToInt32(ddlCourse.SelectedValue);
            int examId = Convert.ToInt32(ddlExam.SelectedValue);
            string unsuccessfulRoll = "";
            string message = "";
            //foreach (ListItem item in chkStudentList.Items)
            //{
            //    if (item.Selected)
            //    {
            //        bool deleteResult = StudentCourseHistoryManager.DeleteByExamIdCourseIdStudentId(examId, courseId, int.Parse(item.Value));
            //        if(deleteResult == false)
            //        {
            //            unsuccessfulRoll += item.Text + ", ";
            //        }
            //    }
            //}

            foreach (GridViewRow row in gvStudentList.Rows)
            {
                CheckBox ckBox = (CheckBox)row.FindControl("ChkSelect");
                Label lblRoll = (Label)row.FindControl("lblRoll");
                Student std = StudentManager.GetByRoll(lblRoll.Text);
                if (ckBox.Checked)
                {
                    bool deleteResult = StudentCourseHistoryManager.DeleteByExamIdCourseIdStudentId(examId, courseId, std.StudentID);
                    if (deleteResult == false)
                    {
                        unsuccessfulRoll += lblRoll.ToString() + ", ";
                    }
                }
            }
            if(unsuccessfulRoll == "")
            {
                message = "Course removal for the checked students have been successful. ";
            }
            else
            {
                message = "Course removal for " +unsuccessfulRoll + " is not successful. ";

            }
            afterDeleteLoad(message);

        }

        protected void afterDeleteLoad(string msg)
        {
            int courseId = Convert.ToInt32(ddlCourse.SelectedValue);
            string courseCode = ddlCourse.SelectedItem.Text;

            int examId = Convert.ToInt32(ddlExam.SelectedValue);

            List<Student> studentList = StudentManager.GetStudentsByExamIdCourseId(examId, courseId);
            string message = "Course - " + courseCode + ", Students Count - " + studentList.Count.ToString();

            lblDeleteMsg.Visible = true;
            lblDeleteMsg.ForeColor = System.Drawing.Color.Red;

            lblDeleteMsg.Text = msg + "  " + message;
            //chkStudentList.Items.Clear();
            //foreach (Student item in studentList)
            //{
            //    chkStudentList.Items.Add(new ListItem(item.Roll, item.StudentID.ToString()));
            //}
            gvStudentList.DataSource = null;
            gvStudentList.DataSource = studentList;
            gvStudentList.DataBind();
            stdButton.Visible = true;

            stdButton.Visible = true;
        }

    }
}