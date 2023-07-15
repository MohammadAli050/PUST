using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System.Drawing;
using LogicLayer.BusinessObjects.DTO;
using Microsoft.Reporting.WebForms;
using System.IO;

namespace EMS.Module.registration
{
    public partial class SingleStudentRegistrationV2 : BasePage
    {
        private string AddRegistrationWorksheetId = "AddRegistrationWorksheet";
        private string SessionStudentId = "Registration_StudentId";
        BussinessObject.UIUMSUser userObj = null;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            
            if (!IsPostBack)
            {
                btnApproveRegisterOffice.Visible = false;
                //btnDownload.Visible = false;
                LoadCurrentRegSessions();

                User user = UserManager.GetById(userObj.Id);
                if (user.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
                {
                    Student student = StudentManager.GetBypersonID(user.Person.PersonID);
                    if (student != null)
                    {
                        txtStudent.Text = student.Roll.ToString();
                        btnLoad.Visible = false;
                        btnLoad_Click(null, null);
                    }
                }
                else if (Session["StudentId"] != null)
                {
                    int id = Convert.ToInt32(Session["StudentId"]);
                    Student obj = StudentManager.GetById(id);
                    txtStudent.Text = obj.Roll.ToString();
                    btnLoad_Click(null, null);
                }
                else
                {
                    btnLoad.Visible = true;
                    //CleareGrid();
                }
            }
        }

        #region Load Student
        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            
            if (string.IsNullOrEmpty(txtStudent.Text))
            {
                ShowAlertMessage(" Please provide a Student Id.");
                return;
            }           

            if (userObj.RoleID == Convert.ToInt32(CommonEnum.Role.Student) && userObj.LogInID != txtStudent.Text.Trim())
            {
                ShowAlertMessage("You are unable to view other Student.");
                return;
            }
            Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
            if (student != null)
            {
                LoadStudent(student);
                btnApproveRegisterOffice.Visible = true;
                GridReBind();
            }
            else 
            {
                ShowAlertMessage("Student Not Found.");
                return;
            }
        }

        private void LoadStudent(Student studentObj)
        {
            FillStudentInfo(studentObj);
            LoadStudentCourse(studentObj.StudentID);
        }

        private void FillStudentInfo(Student student)
        {
            lblProgram.Text = student.Program.ShortName;
            //lblBatch.Text = student.Batch.BatchNO.ToString();
            lblName.Text = student.BasicInfo.FullName;
        }

        private void LoadStudentCourse(int studentId)
        {
            List<RegistrationWorksheet> collection = null;
           
            Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
            int sectionCount = 0;
            decimal creditCount = 0;
            decimal AssignCredit = 0;

            if (student == null)
            {
                ShowAlertMessage(" Student not found.");
                return;
            }
            // AcademicCalender acaCal = AcademicCalenderManager.GetActiveRegistrationCalender();
            int AcademicCalenderID = Convert.ToInt32(ddlSession.SelectedValue);
            //List<AcademicCalender> activeRegistrationSession = AcademicCalenderManager.GetActiveRegistrationCalenders();

            collection = RegistrationWorksheetManager.GetAllOpenCourseWhichSectionIsMatchInStudentBatchByStudentID(studentId, AcademicCalenderID);

            if (collection != null && collection.Count > 0)
            {
                string courses = "";

                foreach (var item in collection)
                {
                    if (item.IsAdd == true)
                    {
                        courses += item.FormalCode + ", ";
                    }
                }
                //lblForwardedCourse.Text = courses;
            }

            if (collection != null)
            {
                sectionCount = collection.Count(c => c.IsRegistered == true);
                creditCount = collection.Where(c => c.IsRegistered == true).Sum(c => c.Credits);
                collection = collection.Where(c => c.CourseStatusId != -1).ToList();
                AssignCredit = collection.Where(x => x.IsAutoAssign == true && x.AcaCal_SectionID > 0).Sum(x => x.Credits);
                
                gvCourseRegistration.DataSource = collection.ToList().OrderBy(c => c.FormalCode);
                gvCourseRegistration.DataBind();
                
                if (userObj.RoleID == 8)
                {
                    if (gvCourseRegistration.Columns.Count > 0)
                    {
                        gvCourseRegistration.Columns[6].Visible = false;
                        gvCourseRegistration.Columns[10].Visible = false;
                        gvCourseRegistration.Columns[11].Visible = false;
                        gvCourseRegistration.Columns[17].Visible = false;
                        //gvCourseRegistration.Columns[8].Visible = false;
                    }
                }

                //ButtonEnableDisableBasedOnRoleAndCurrentRegStatus(collection);
                GridReBind();
            }

            lblSectionCount.Text = "Course Registered: " + sectionCount;
            lblCreditCount.Text = "Total Credit: " + creditCount;
            lblAssignCredit.Text = "Section Assigned Credit: " + AssignCredit;
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        #endregion Load Student

        protected void ddlSession_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnLoad_Click(null, null);
        }

        private void LoadCurrentRegSessions()
        {
            ddlSession.Items.Clear();
            List<AcademicCalender> activeRegistrationSession = AcademicCalenderManager.GetActiveRegistrationCalenders().OrderByDescending(d => d.AcademicCalenderID).ToList();
            if (activeRegistrationSession.Count > 0)
            {
                ddlSession.DataTextField = "FullCode";
                ddlSession.DataValueField = "AcademicCalenderID";
                ddlSession.DataSource = activeRegistrationSession;
                ddlSession.DataBind();
            }
        }

        private void GridReBind()
        {
            for (int i = 0; i < gvCourseRegistration.Rows.Count; i++)
            {
                GridViewRow row = gvCourseRegistration.Rows[i];
                Button btnAddSection = (Button)row.FindControl("btnSectionAdd");
                CheckBox chkActive = (CheckBox)row.FindControl("ChkActive");
                Button btnRemoveCourse = (Button)row.FindControl("btnRemoveCourse");
                Label lblReg = (Label)row.FindControl("lblReg");
                DropDownList ddlCourseStatus = (DropDownList)row.FindControl("ddlCourseStatus");
                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Student))
                {
                    btnAddSection.Visible = false;
                    btnAddSection.Enabled = false;
                    chkActive.Visible = false;
                    chkActive.Enabled = false;
                    btnRemoveCourse.Visible = false;
                    btnRemoveCourse.Enabled = false;
                    ddlCourseStatus.Visible = false;
                    ddlCourseStatus.Enabled = false;
                }
                else
                {
                    if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ESCL))
                    {
                        //btnAddSection.Visible = true;
                        //btnAddSection.Enabled = true;
                        //btnRemoveCourse.Visible = true;
                        //btnRemoveCourse.Enabled = true;
                        //chkActive.Visible = true;
                        //chkActive.Enabled = true;
                        //ddlCourseStatus.Visible = true;
                        //ddlCourseStatus.Enabled = true;
                    }
                    else
                    {
                        btnAddSection.Visible = true;
                        btnAddSection.Enabled = true;
                        btnRemoveCourse.Visible = false;
                        btnRemoveCourse.Enabled = false;
                        chkActive.Visible = true;
                        chkActive.Enabled = true;
                        ddlCourseStatus.Visible = false;
                        ddlCourseStatus.Enabled = false;
                    }
                }

                if (Convert.ToString(lblReg.Text) == "Done")
                {
                    ddlCourseStatus.Enabled = false;
                }
                else { ddlCourseStatus.Enabled = true; }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Hide();
        }

        protected void btnAddCourseCancel_Click(object sender, EventArgs e)
        {

        }

        protected void gvAddCourse_SelectedIndexChanged(object sender, EventArgs e)
        {
            int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
            Student studentObj = StudentManager.GetByRoll(txtStudent.Text.Trim());

            if (studentObj != null)
            {
                GridViewRow row = gvAddCourse.SelectedRow;
                Label lblCourseID = (Label)row.FindControl("lblCourseID");
                Label lblVersionID = (Label)row.FindControl("lblVersionID");
                Label lblCourseFailCounter = (Label)row.FindControl("lblCourseFailCounter");

                if (!string.IsNullOrEmpty(lblCourseID.Text) && !string.IsNullOrEmpty(lblVersionID.Text))
                {
                    int courseId = Convert.ToInt32(lblCourseID.Text);
                    int versionId = Convert.ToInt32(lblVersionID.Text);
                    int failCounter = Convert.ToInt32(lblCourseFailCounter.Text);

                    if (courseId > 0 && versionId > 0)
                    {
                        //if (RegistrationWorksheetManager.checkCourse(failCounter, userObj.RoleID, acaCalId))
                        //{
                        //    InsertWorkSheet(studentObj, courseId, versionId, acaCalId);
                        //}
                        //else
                        //{
                        //    ShowAlertMessage("Student is not eligible to take this course.");
                        //    return;
                        //}
                    }
                }
            }
            ModalPopupExtender2.Show();
        }

        protected void btnApproveRegisterOffice_Click(object sender, EventArgs e)
        {
            Student student = StudentManager.GetByRoll(txtStudent.Text.Trim());
            try
            {
                int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                if (student != null)
                {
                    if (true)//Student Block
                    {
                        List<RegistrationWorksheet> rwList = RegistrationWorksheetManager.GetByStudentIDAcacalID(student.StudentID, acaCalId);
                        //rwList = RegistrationWorksheetManager.getEligibleCourseSubmitHeadToAdmissionOffice(rwList, acaCalId);
                        if (rwList.Count > 0)
                        {
                            foreach (var registrationWorksheetObj in rwList)
                            {
                                int regStage = 2;
                                bool isUpdated = UpdateRegistrationWorkSheet(registrationWorksheetObj, regStage);
                                if (isUpdated)
                                {
                                    InsertCourseHistory(registrationWorksheetObj);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // lblMessage.Text = "Registration not complete.";
                ShowAlertMessage("Registration not complete.");

            }
            finally
            {
                if (student != null)
                    LoadStudentCourse(student.StudentID);
            }
        }

        private bool UpdateRegistrationWorkSheet(RegistrationWorksheet registrationWorksheetObj, int regStage)
        {
            registrationWorksheetObj.RetakeNo = regStage;
            registrationWorksheetObj.IsRegistered = true;
            registrationWorksheetObj.ModifiedBy = Convert.ToInt32(userObj.Id);
            registrationWorksheetObj.ModifiedDate = DateTime.Now;
            bool result = RegistrationWorksheetManager.Update(registrationWorksheetObj);
            return result;
        }

        private void InsertCourseHistory(RegistrationWorksheet registrationWorksheetObj)
        {
            StudentCourseHistory studentCourseHistory = new StudentCourseHistory();
            List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(registrationWorksheetObj.StudentID, registrationWorksheetObj.OriginalCalID);
            studentCourseHistory = studentCourseHistoryList.Find(o => o.CourseStatusID == (int)CommonEnum.CourseStatus.Regular &&
                                                                        o.CourseID == registrationWorksheetObj.CourseID &&
                                                                        o.VersionID == registrationWorksheetObj.VersionID);
            if (studentCourseHistory == null)
            {
                StudentCourseHistory studentCourseHistoryInserObj = new StudentCourseHistory();
                studentCourseHistoryInserObj.StudentID = Convert.ToInt32(registrationWorksheetObj.StudentID);
                studentCourseHistoryInserObj.CourseStatusID = (int)CommonEnum.CourseStatus.Regular;
                if (registrationWorksheetObj.AcaCalTypeName == "IM")
                {
                    studentCourseHistoryInserObj.RetakeNo = 12;
                }
                else if (registrationWorksheetObj.AcaCalTypeName == "R")
                {
                    studentCourseHistoryInserObj.RetakeNo = 9;
                }
                studentCourseHistoryInserObj.AcaCalID = registrationWorksheetObj.OriginalCalID;
                studentCourseHistoryInserObj.CourseID = registrationWorksheetObj.CourseID;
                studentCourseHistoryInserObj.VersionID = registrationWorksheetObj.VersionID;
                studentCourseHistoryInserObj.CourseCredit = registrationWorksheetObj.Credits;
                studentCourseHistoryInserObj.AcaCalSectionID = registrationWorksheetObj.AcaCal_SectionID;
                studentCourseHistoryInserObj.CourseStatusDate = DateTime.Now;
                studentCourseHistoryInserObj.CreatedBy = userObj.Id;
                studentCourseHistoryInserObj.CreatedDate = DateTime.Now;
                studentCourseHistoryInserObj.ModifiedBy = userObj.Id;
                studentCourseHistoryInserObj.ModifiedDate = DateTime.Now;
                int i = StudentCourseHistoryManager.Insert(studentCourseHistoryInserObj);
            }
        }

        protected void btnRemoveCourse_Click(object sender, EventArgs e)
        {
            try
            {
                if (userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.Advisor) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.ESCL) || userObj.RoleID == Convert.ToInt32(CommonUtility.CommonEnum.Role.BRURAdmin))
                {
                    int acaCalId = Convert.ToInt32(ddlSession.SelectedValue);

                    Button btn = (Button)sender;
                    int id = int.Parse(btn.CommandArgument.ToString());
                    Session[AddRegistrationWorksheetId] = id;

                    RegistrationWorksheet registrationWorksheet = RegistrationWorksheetManager.GetById(id);
                    if (registrationWorksheet.IsRegistered == true)
                    {
                        ShowAlertMessage("Course already registered.");
                        return;
                    }

                    if (registrationWorksheet != null)
                    {
                        List<StudentCourseHistory> studentCourseHistoryList = StudentCourseHistoryManager.GetAllByStudentIdAcaCalId(registrationWorksheet.StudentID, acaCalId);
                        StudentCourseHistory studentCourseHistoryObj = studentCourseHistoryList.Where(d => d.CourseID == registrationWorksheet.CourseID && d.VersionID == registrationWorksheet.VersionID).FirstOrDefault();
                        bool isWorkSheetDeleted = RegistrationWorksheetManager.Delete(registrationWorksheet.ID);
                        if (studentCourseHistoryObj != null)
                        {
                            BillHistory billHistoryObj = BillHistoryManager.GetByStudentCourseHistoryId(studentCourseHistoryObj.ID);
                            bool isCourseDeleted = StudentCourseHistoryManager.Delete(studentCourseHistoryObj.ID);

                            if (billHistoryObj != null)
                            {
                                //CollectionHistory collectionHistoryObj = CollectionHistoryManager.GetByBillHistoryMasterId(billHistoryObj.BillHistoryMasterId).Where(d => d.BillHistoryId == billHistoryObj.BillHistoryId).FirstOrDefault();
                                //bool isBillDeleted = BillHistoryManager.Delete(billHistoryObj.BillHistoryId);

                                //if (collectionHistoryObj != null)
                                //{
                                //    bool isCollectionDeleted = CollectionHistoryManager.Delete(collectionHistoryObj.CollectionHistoryId);
                                //}
                            }
                        }
                    }
                    btnLoad_Click(null, null);
                }
            }
            catch (Exception ex) { }
        }

        protected void ddlCourseStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
            DropDownList ddlCourseStatus = (DropDownList)gvr.FindControl("ddlCourseStatus");
            Label lblWorkSheetId = (Label)gvr.FindControl("lblWorkSheetId");

            int courseStatusId = Convert.ToInt32(ddlCourseStatus.SelectedItem.Value);
            string courseStatus = Convert.ToString(ddlCourseStatus.SelectedItem.Text);
            int workSheetId = Convert.ToInt32(lblWorkSheetId.Text);
            if (courseStatusId > 0 && workSheetId > 0)
            {
                RegistrationWorksheet registrationWorkSheetObj = RegistrationWorksheetManager.GetById(workSheetId);
                if (registrationWorkSheetObj != null)
                {
                    registrationWorkSheetObj.AcaCalTypeName = courseStatus;
                    bool result = RegistrationWorksheetManager.Update(registrationWorkSheetObj);
                    if (result)
                    {
                        btnLoad_Click(null, null);
                        ShowAlertMessage(" Course Reg Type Changed Successfully. ");
                        return;
                    }
                }
            }
        }

    }
}