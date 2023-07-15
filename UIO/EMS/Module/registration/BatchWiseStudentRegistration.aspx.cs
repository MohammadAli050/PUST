using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.registration
{
    public partial class BatchWiseStudentRegistration : BasePage
    {
        public class StudentList
        {
            public int StudentId { get; set; }
            public string Roll { get; set; }
            public string Name { get; set; }
            public string Course { get; set; }
            public int CourseStatus { get; set; }
            public decimal AssignedCredit { get; set; }
            public decimal OccupiedCredit { get; set; }
        }

        #region Events
        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        int userId = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            userId = userObj.Id;
            pnlMessage.Visible = false;
            lblCount.Text = "0";

            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //int programId = Convert.ToInt32(ucProgram.selectedValue);
                //LoadYearDDL(programId);

                //ddlSemester.Items.Clear();
                //ddlSemester.AppendDataBoundItems = true;
                //ddlSemester.Items.Add(new ListItem("-Select-", "0"));
                //LoadCurrentRegSessions();
                //LoadlevelTermDDL(Convert.ToInt32(ucProgram.selectedValue));
                //LoadTermDropDown(0);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //int programId = Convert.ToInt32(ucProgram.selectedValue);
                //LoadYearDDL(programId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void OnProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            //ucBatch.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
            try
            {
                //ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                //int programId = Convert.ToInt32(ucProgram.selectedValue);
                //LoadYearDDL();
            }
            catch (Exception ex)
            { }
        }

        protected void ucSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            //LoadStudent();
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

        protected void txtStudentId_TextChanged(object sender, EventArgs e)
        {
            try
            {
                //int acaCalId = Convert.ToInt32(ucSession.selectedValue);
                LogicLayer.BusinessObjects.Student studentObj = null;//StudentManager.GetByRollAcacalIdWithOpenCourse(Convert.ToString(txtStudentId.Text.Trim()), acaCalId);
                if (studentObj != null)
                {
                    gvStudentList.DataSource = null;
                    gvStudentList.DataBind();

                    List<LogicLayer.BusinessObjects.Student> studentObjList = new List<LogicLayer.BusinessObjects.Student>();


                    if (studentObjList != null)
                    {
                        gvStudentList.DataSource = studentObjList;
                        gvStudentList.DataBind();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void LoadData()
        {
            try
            {
                int currentSessionId = Convert.ToInt32(ucCurrentSession.selectedValue);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

                lblMessage.Text = "";
                try
                {
                    List<Student> studentList = new List<Student>();

                    if (programId > 0 && yearNo > 0 && semesterNo > 0)
                    {
                        studentList = StudentManager.GetAllByProgramIdYearNoSemsterNoCurrentSessionIdForRegistration(programId, yearNo, semesterNo, currentSessionId);
                    }
                    
                    pnlHOD.Visible = true;
                    pnlAdvisor.Visible = true;
                    pnlAdmissionOffice.Visible = true;
                    pnlRegisterOffice.Visible = true;
                    btnApproveRegisterOffice.Visible = true;
                    

                    gvStudentList.DataSource = studentList;
                    gvStudentList.DataBind();
                }
                catch (Exception ex)
                {
                }

            }
            catch (Exception)
            {
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        #endregion

        #region Methods
        private void ClearGrid()
        {
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();

            lblCount.Text = "0";
        }

        private void ShowMessage(string msg)
        {
            pnlMessage.Visible = true;

            lblMessage.Text = msg;
            lblMessage.ForeColor = Color.Red;

        }
        #endregion

        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkHeader = (CheckBox)gvStudentList.HeaderRow.FindControl("chkSelectAllStudent");
                if (chkHeader.Checked)
                {
                    for (int i = 0; i < gvStudentList.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentList.Rows[i];
                        //Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = true;
                    }
                }
                if (!chkHeader.Checked)
                {
                    for (int i = 0; i < gvStudentList.Rows.Count; i++)
                    {
                        GridViewRow row = gvStudentList.Rows[i];
                        //Label studentId = (Label)row.FindControl("lblStudentId");
                        CheckBox studentCheckd = (CheckBox)row.FindControl("CheckBox");
                        studentCheckd.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }    

        protected void gvStudentList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "View")
            {
                string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
                var id = _pageUrl.Substring(_pageUrl.LastIndexOf('=') + 1);
                int StudentId = Convert.ToInt32(e.CommandArgument);

                Session["StudentId"] = Convert.ToString(StudentId);
                //Response.Redirect("SingleStudentRegistrationV2.aspx?mmi=" + id);

                string url = string.Format("SingleStudentRegistrationV2.aspx?mmi={0}", id);
                string script = "<script type='text/javascript'>window.open('" + url + "')</script>";
                this.ClientScript.RegisterStartupScript(this.GetType(), "script", script);
            }
        }

        private int CountGridStudent()
        {
            int studentCounter = 0;
            for (int i = 0; i < gvStudentList.Rows.Count; i++)
            {
                GridViewRow row = gvStudentList.Rows[i];
                CheckBox ckBox = (CheckBox)row.FindControl("CheckBox");

                if (ckBox.Checked == true)
                {
                    studentCounter = studentCounter + 1;
                }
            }
            return studentCounter;
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.Checked)
                {
                    chk.ForeColor = Color.Red;
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
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnApproveRegisterOffice_Click(object sender, EventArgs e)
        {
            int Count = CountGridStudent();
            Student student = new Student();
            if (Count > 0)
            {
                try
                {
                    int checkedCounter = 0;
                    int insertCounter = 0;
                    bool isInserted = false;
                    foreach (GridViewRow row in gvStudentList.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("CheckBox");
                        if (ckBox.Checked)
                        {
                            checkedCounter = checkedCounter + 1;
                            Label lblStudentId = (Label)row.FindControl("lblStudentId");
                            int StudentId = Convert.ToInt32(lblStudentId.Text);

                            if (true)//Student Block
                            {
                                student = StudentManager.GetById(StudentId);
                                if (student != null)
                                {
                                    int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
                                    int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);
                                    List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentIdYearNoSemesterNoExamId(student.StudentID, yearNo, semesterNo, 0);
                                    //rwList = RegistrationWorksheetManager.getEligibleCourseForFinalRegistration(rwList, acaCalId);
                                    if (rwList.Count > 0)
                                    {
                                        foreach (var item in rwList)
                                        {
                                            RegistrationWorksheet obj = item;
                                            obj.RetakeNo = 2;
                                            obj.IsRegistered = true;
                                            obj.ModifiedBy = Convert.ToInt32(BaseCurrentUserObj.Id);
                                            obj.ModifiedDate = DateTime.Now;
                                            bool isUpdated = RegistrationWorksheetManager.Update(obj);
                                            if (isUpdated)
                                            {
                                                isInserted = InsertCourseHistory(obj);
                                            }

                                            //#region SubmitLog

                                            //LogGeneralManager.Insert(
                                            //                        DateTime.Now,
                                            //                        "",
                                            //                        "",
                                            //                        BaseCurrentUserObj.LogInID,
                                            //                        item.FormalCode,
                                            //                        "",
                                            //                        "Approve Registration",
                                            //                        BaseCurrentUserObj.LogInID + " approved " + item.CourseTitle + ", " + item.FormalCode + " for registration",
                                            //                        BaseCurrentUserObj.LogInID + " Approve Registration",
                                            //                        ((int)CommonEnum.PageName.Registration).ToString(),
                                            //                        CommonEnum.PageName.Registration.ToString(),
                                            //                        _pageUrl,
                                            //                        student.Roll);
                                            //#endregion
                                        }
                                    }
                                }
                                if (isInserted)
                                {
                                    insertCounter = insertCounter + 1;
                                }
                            }
                        }
                    }

                    ShowAlertMessage("Student checked for registration : " + checkedCounter + " and successfully registration done for : " + insertCounter + " students");
                }
                catch
                {
                }
                finally
                {
                    LoadData();
                }
            }
            else
            {
                ShowAlertMessage("Please Select Student.");
            }
        }

        private bool InsertCourseHistory(RegistrationWorksheet registrationWorksheetObj)
        {
            StudentCourseHistory studentCourseHistory = new StudentCourseHistory();
            List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(registrationWorksheetObj.StudentID, registrationWorksheetObj.OriginalCalID);
            studentCourseHistory = studentCourseHistoryList.Find(o => o.CourseStatusID == (int)CommonEnum.CourseStatus.Regular &&
                                                                        o.CourseID == registrationWorksheetObj.CourseID &&
                                                                        o.VersionID == registrationWorksheetObj.VersionID);
            bool isInserted = false;
            if (studentCourseHistory == null)
            {
                StudentCourseHistory studentCourseHistoryInserObj = new StudentCourseHistory();
                studentCourseHistoryInserObj.StudentID = Convert.ToInt32(registrationWorksheetObj.StudentID);
                studentCourseHistoryInserObj.CourseStatusID = (int)CommonEnum.CourseStatus.Regular;
                studentCourseHistoryInserObj.RetakeNo = 9;
                studentCourseHistoryInserObj.AcaCalID = registrationWorksheetObj.OriginalCalID;
                studentCourseHistoryInserObj.CourseID = registrationWorksheetObj.CourseID;
                studentCourseHistoryInserObj.VersionID = registrationWorksheetObj.VersionID;
                studentCourseHistoryInserObj.CourseCredit = registrationWorksheetObj.Credits;
                studentCourseHistoryInserObj.AcaCalSectionID = registrationWorksheetObj.AcaCal_SectionID;
                studentCourseHistoryInserObj.YearNo = registrationWorksheetObj.YearNo;
                studentCourseHistoryInserObj.SemesterNo = registrationWorksheetObj.SemesterNo;
                studentCourseHistoryInserObj.ExamId = registrationWorksheetObj.ExamId;
                studentCourseHistoryInserObj.YearId = registrationWorksheetObj.YearId;
                studentCourseHistoryInserObj.SemesterId = registrationWorksheetObj.SemesterId;
                studentCourseHistoryInserObj.CourseStatusDate = DateTime.Now;
                studentCourseHistoryInserObj.CreatedBy = userObj.Id;
                studentCourseHistoryInserObj.CreatedDate = DateTime.Now;
                studentCourseHistoryInserObj.ModifiedBy = userObj.Id;
                studentCourseHistoryInserObj.ModifiedDate = DateTime.Now;
                int result = StudentCourseHistoryManager.Insert(studentCourseHistoryInserObj);
                if(result > 0)
                {
                    isInserted = true;
                }
                else
                {
                    isInserted = false;
                }
            }
            return isInserted;
        }

        protected void gvStudentList_OnRowCreated(object sender, GridViewRowEventArgs e)
        {
            Button btnAdd = (Button)e.Row.Cells[0].FindControl("btnSelect");
            if (btnAdd != null)
            {
                ScriptManager.GetCurrent(this).RegisterPostBackControl(btnAdd);
            }
        }
    }
}