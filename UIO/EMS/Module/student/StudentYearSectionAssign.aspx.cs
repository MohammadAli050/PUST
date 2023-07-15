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

namespace EMS.Module.student
{
    public partial class StudentYearSectionAssign : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.CheckPage_Load();
            base.CheckPage_Load();
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
                LoadYearSectionDDL();
                pnlAssign.Visible = false;
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
            try
            {
                lblMsg.Text = string.Empty;
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
            try
            {
                lblMsg.Text = string.Empty;
                ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void LoadYearSectionDDL()
        {
            List<YearSection> yearSectionList = new List<YearSection>();
            yearSectionList = YearSectionManager.GetAll();

            ddlYearSection.Items.Clear();
            ddlYearSection.AppendDataBoundItems = true;

            if (yearSectionList != null)
            {
                ddlYearSection.Items.Add(new ListItem("-Select-", "0"));
                ddlYearSection.DataTextField = "Name";
                ddlYearSection.DataValueField = "Id";
                if (yearSectionList != null)
                {
                    ddlYearSection.DataSource = yearSectionList.OrderBy(b => b.Id).ToList();
                    ddlYearSection.DataBind();
                }
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadStudent();
        }

        private void LoadStudent()
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int currentSessionId = Convert.ToInt32(ucSession.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0 && currentSessionId > 0)
            {
                List<Student> studentList = StudentManager.GetAllByProgramIdYearNoSemsterNoCurrentSessionId(programId, yearNo, semesterNo, currentSessionId);
                if (studentList.Count > 0 && studentList != null)
                {
                    pnlAssign.Visible = true;
                    gvStudentList.DataSource = studentList;
                    gvStudentList.DataBind();
                }
            }
            else
            {
                lblMsg.Text = "Please select program, year, semester and current session.";
            }
        }

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
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

                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAssignYearSection_Click(object sender, EventArgs e)
        {
            try 
            { 
                this.mp1.Show();
                if (isStudentChecked())
                {
                    int checkedCounter = 0;
                    int updateInsertCount = 0;
                    int yearSectionId = Convert.ToInt32(ddlYearSection.SelectedValue);
                    foreach (GridViewRow row in gvStudentList.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                        Label lblStudentId = (Label)row.FindControl("lblStudentID");
                        if (ckBox.Checked)
                        {
                            checkedCounter = checkedCounter + 1;
                            int studentId = Convert.ToInt32(lblStudentId.Text);
                            StudentAdditionalInfo stdAddInfoInsertObj = new StudentAdditionalInfo();

                            if (studentId > 0)
                            {
                                StudentAdditionalInfo stdAddInfoUpdateObj = StudentAdditionalInfoManager.GetByStudentId(studentId);
                                if (stdAddInfoUpdateObj == null)
                                {
                                    stdAddInfoInsertObj.StudentId = studentId;
                                    stdAddInfoInsertObj.YearSectionId = yearSectionId;
                                    stdAddInfoInsertObj.CreatedBy = BaseCurrentUserObj.Id;
                                    stdAddInfoInsertObj.CreatedDate = DateTime.Now;
                                    stdAddInfoInsertObj.ModifiedBy = BaseCurrentUserObj.Id;
                                    stdAddInfoInsertObj.ModifiedDate = DateTime.Now;
                                    int result = StudentAdditionalInfoManager.Insert(stdAddInfoInsertObj);
                                    if (result > 0)
                                    {
                                        updateInsertCount = updateInsertCount + 1;
                                    }
                                }
                                else
                                {
                                    stdAddInfoUpdateObj.StudentId = studentId;
                                    stdAddInfoUpdateObj.YearSectionId = yearSectionId;
                                    stdAddInfoUpdateObj.ModifiedBy = BaseCurrentUserObj.Id;
                                    stdAddInfoUpdateObj.ModifiedDate = DateTime.Now;
                                    bool result = StudentAdditionalInfoManager.Update(stdAddInfoUpdateObj);
                                    if (result)
                                    {
                                        updateInsertCount = updateInsertCount + 1;
                                    }
                                }
                            }
                        }
                    }

                    if (updateInsertCount == checkedCounter)
                    {
                        lblPopUpMsg.Text = "Student year section assigned successfully";
                    }
                    else if (updateInsertCount > 0) { lblPopUpMsg.Text = "Student year section assigned successfully. "; }
                    else if (updateInsertCount == 0) { lblPopUpMsg.Text = "Student year section could not assigned successfully. "; }

                    LoadStudent();
                }
                else { lblPopUpMsg.Text = "Please select some student to assign year section."; }
            }
            catch (Exception ex) { }
        }

        public bool isStudentChecked()
        {
            this.mp1.Show();
            int checkedCounter = 0;
            foreach (GridViewRow row in gvStudentList.Rows)
            {
                CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                Label lblStudentId = (Label)row.FindControl("lblStudentID");
                if (ckBox.Checked)
                {
                    checkedCounter = checkedCounter + 1;
                }
            }

            if (checkedCounter > 0)
            { return true; }
            else { return false; }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            try {
                lblPopUpMsg.Text = string.Empty;
                this.mp1.Hide(); }
            catch (Exception ex) { }
        }

        protected void btnShowYearSection_Click(object sender, EventArgs e)
        {
            int checkCount = 0;
            foreach (GridViewRow row in gvStudentList.Rows)
            {
                CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");

                if (ckBox.Checked) checkCount++;
            }

            if (checkCount > 0)
                mp1.Show();
            //else
            //    ShowAlertMessage("No item selected.");
        }
    }
}