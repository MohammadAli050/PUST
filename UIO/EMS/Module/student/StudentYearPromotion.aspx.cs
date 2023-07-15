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
    public partial class StudentYearPromotion : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        BussinessObject.UIUMSUser BaseCurrentUserObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
                LoadYearNoPopUpDDL();
                LoadSemesterNoPopUpDDL();
                pnlAssign.Visible = false;
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

        private void LoadYearNoPopUpDDL()
        {
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();


            ddlYearNoPopUp.Items.Clear();
            ddlYearNoPopUp.AppendDataBoundItems = true;
            ddlYearNoPopUp.Items.Add(new ListItem("-Select-", "-1"));
            if (yearList != null && yearList.Count > 0)
            {
                ddlYearNoPopUp.DataTextField = "YearNoName";
                ddlYearNoPopUp.DataValueField = "YearNo";

                ddlYearNoPopUp.DataSource = yearList;
                ddlYearNoPopUp.DataBind();
            }
        }

        private void LoadSemesterNoPopUpDDL()
        {
            List<SemesterDistinctDTO> semesterList = new List<SemesterDistinctDTO>();
            semesterList = SemesterManager.GetAllDistinct();
            semesterList = semesterList.OrderBy(x => x.SemesterNo).ToList();


            ddlSemesterNoPopUp.Items.Clear();
            ddlSemesterNoPopUp.AppendDataBoundItems = true;
            ddlSemesterNoPopUp.Items.Add(new ListItem("-Select-", "-1"));
            if (semesterList != null && semesterList.Count > 0)
            {
                ddlSemesterNoPopUp.DataTextField = "SemesterNoName";
                ddlSemesterNoPopUp.DataValueField = "SemesterNo";

                ddlSemesterNoPopUp.DataSource = semesterList;
                ddlSemesterNoPopUp.DataBind();
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadStudent();            
        }

        private void LoadStudent()
        {
            //List<Student> studentList = StudentManager.GetAllByProgramIdSessionId(Convert.ToInt32(ucProgram.selectedValue), Convert.ToInt32(ucSession.selectedValue));

            //gvStudentList.DataSource = studentList;
            //gvStudentList.DataBind();

            lblMsg.Text = string.Empty;

            int programId = Convert.ToInt32(ucProgram.selectedValue);            
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
            int currentSessionId = Convert.ToInt32(ucFilterCurrentSession.selectedValue);

            if (programId > 0 && yearNo > 0 && semesterNo > 0 && currentSessionId> 0)
            {
                List<Student> studentList = StudentManager.GetAllByProgramIdYearNoSemsterNoCurrentSessionId(programId, yearNo, semesterNo, currentSessionId);

                pnlAssign.Visible = studentList.Count > 0;
                gvStudentList.DataSource = studentList.Count > 0 ? studentList : null;
                gvStudentList.DataBind();                
            }
            else
            {
                pnlAssign.Visible = false;
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
            int yearNo = Convert.ToInt32(ddlYearNoPopUp.SelectedValue);
            if (yearNo == 0)
            {
                ShowAlertMessage("Year not selected");
                return;
            }

            int semesterNo = Convert.ToInt32(ddlSemesterNoPopUp.SelectedValue);
            if (semesterNo == 0)
            {
                ShowAlertMessage("Semester not selected");
                return;
            }

            int count = 0;

            string rollList = string.Empty;
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int yearId = 0;
            int semesterId = 0;

            if (programId > 0 && yearNo > 0 && semesterNo > 0)
            {
                Year yearObj = YearManager.GetByProgramId(programId).Where(d => d.YearNo == yearNo).FirstOrDefault();
                if (yearObj != null)
                {
                    yearId = yearObj.YearId;
                    Semester semesterObj = SemesterManager.GetByYearId(yearId).Where(d => d.SemesterNo == semesterNo).FirstOrDefault();
                    if (semesterObj != null)
                    {
                        semesterId = semesterObj.SemesterId;
                    }
                }
                    
                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    Label lblStudentId = (Label)row.FindControl("lblStudentID");
                    if (ckBox.Checked)
                    {
                        int studentId = Convert.ToInt32(lblStudentId.Text);

                        var student = StudentManager.GetById(studentId);
                        StudentAdditionalInfo stdAddInfoInsertObj = new StudentAdditionalInfo();

                        if (studentId > 0)
                        {
                            StudentAdditionalInfo stdAddInfoUpdateObj = StudentAdditionalInfoManager.GetByStudentId(studentId);
                            if (stdAddInfoUpdateObj == null)
                            {
                                stdAddInfoInsertObj.StudentId = studentId;
                                stdAddInfoInsertObj.YearId = yearId;
                                stdAddInfoInsertObj.SemesterId = semesterId;
                                stdAddInfoInsertObj.YearNo = yearNo;
                                stdAddInfoInsertObj.SemesterNo = semesterNo;
                                stdAddInfoInsertObj.CreatedBy = BaseCurrentUserObj.Id;
                                stdAddInfoInsertObj.CreatedDate = DateTime.Now;
                                int result = StudentAdditionalInfoManager.Insert(stdAddInfoInsertObj);
                            }
                            else
                            {
                                stdAddInfoUpdateObj.StudentId = studentId;
                                stdAddInfoUpdateObj.YearId = yearId;
                                stdAddInfoUpdateObj.SemesterId = semesterId;
                                stdAddInfoUpdateObj.YearNo = yearNo;
                                stdAddInfoUpdateObj.SemesterNo = semesterNo;
                                stdAddInfoUpdateObj.ModifiedBy = BaseCurrentUserObj.Id;
                                stdAddInfoUpdateObj.ModifiedDate = DateTime.Now;
                                bool result = StudentAdditionalInfoManager.Update(stdAddInfoUpdateObj);
                            }

                            rollList += student.Roll + ", ";
                            count++;
                        }
                    }
                }
            }
            if (count > 0)
            {
                ShowAlertMessage("Year & Semester successfully updated.");
                //#region log_Insert
                //LogGeneralManager.Insert(
                //    DateTime.Now,
                //    BaseAcaCalCurrent.Code,
                //    BaseAcaCalCurrent.FullCode,
                //    BaseCurrentUserObj.LogInID,
                //    "",
                //    "",
                //    "Year & Semester Edit",
                //    BaseCurrentUserObj.LogInID + " Change Year & Semester for Student Ids :" + rollList,
                //    "Student Year & Semester change for Student Ids: " + rollList,
                //    "",
                //    "StudentYearPromotion.aspx",
                //    _pageUrl,
                //     "");
                //#endregion
            }

            //ddlYear.ClearSelection();
            //ddlSemester.Items.Clear();

            LoadStudent();
        }

        protected void btnCurrentSessionAssign_Click(object sender, EventArgs e)
        {
            try
            {
                int currentSessionId = Convert.ToInt32(ucCurrentSession.selectedValue);

                if (currentSessionId == 0)
                {
                    ShowAlertMessage("Current session not selected");
                    return;
                }

                int count = 0;
                string rollList = string.Empty;

                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    Label lblStudentId = (Label)row.FindControl("lblStudentID");
                    if (ckBox.Checked)
                    {
                        int studentId = Convert.ToInt32(lblStudentId.Text);
                        
                        if (studentId > 0)
                        {
                            var stdAddInfoUpdateObj = StudentManager.GetById(studentId);
                            if (stdAddInfoUpdateObj != null)
                            {
                                stdAddInfoUpdateObj.ActiveSession = currentSessionId;
                                stdAddInfoUpdateObj.ModifiedBy = BaseCurrentUserObj.Id;
                                stdAddInfoUpdateObj.ModifiedDate = DateTime.Now;
                                bool result = StudentManager.Update(stdAddInfoUpdateObj);

                                if (result)
                                {
                                    count++;
                                    rollList += stdAddInfoUpdateObj.Roll + ", ";
                                }
                            }
                        }
                    }
                }

                if (count > 0) 
                {
                    ShowAlertMessage("Current session successfully updated.");               
                    #region log_Insert
                    LogGeneralManager.Insert(
                        DateTime.Now,
                        BaseAcaCalCurrent.Code,
                        BaseAcaCalCurrent.FullCode,
                        BaseCurrentUserObj.LogInID,
                        "",
                        "",
                        "Current Session Edit",
                        BaseCurrentUserObj.LogInID + " Change Current Session for Student Ids :" + rollList,
                        "Student Current Session change for Student Ids : " + rollList,
                        "",
                        "StudentYearPromotion.aspx",
                        _pageUrl,
                         "");
                    #endregion
                }                    
                else
                    ShowAlertMessage("Student(s) not found.");

                ucCurrentSession.SelectedValue(0);

                LoadStudent();
            }
            catch (Exception ex)
            {
            }
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        protected void btnShowPromoteDemote_Click(object sender, EventArgs e)
        {
            //ddlYear.ClearSelection();
            //ddlSemester.Items.Clear();
            int checkCount = 0;
            foreach (GridViewRow row in gvStudentList.Rows)
            {
                CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");

                if (ckBox.Checked) checkCount++;
            }

            if (checkCount > 0)
                ModalPopupExtender1.Show();
            else
                ShowAlertMessage("No item selected.");
        }

        protected void btnShowCurrentSession_Click(object sender, EventArgs e)
        {
            ucCurrentSession.SelectedValue(0);
            int checkCount = 0;
            foreach (GridViewRow row in gvStudentList.Rows)
            {
                CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");

                if (ckBox.Checked) checkCount++;
            }

            if (checkCount > 0)
                ModalPopupExtender2.Show();
            else
                ShowAlertMessage("No item selected.");
        }

        protected void ucCurrentSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }
    }
}