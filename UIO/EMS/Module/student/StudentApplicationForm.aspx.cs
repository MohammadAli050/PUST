using LogicLayer.BusinessLogic;
using LogicLayer.DataEntity;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.student
{
    public partial class StudentApplicationForm : System.Web.UI.Page
    {
        StudentApplicationFormModel dbObj = new StudentApplicationFormModel();

        protected void Page_Load(object sender, EventArgs e)
        {            
            try
            {
                if (!IsPostBack)
                {
                    LoadNationality();
                    LoadEducationBoard();
                    LoadYear(ddlStudentExamYear);
                    LoadYear(ddlYear);
                    LoadYear(ddlExamYear);
                    LoadCourseCode();
                    LoadExaminarType();
                    LoadEducationInfo();
                    LoadPreviousSemesterCourseCode();
                    LoadProgram();
                    LoadSemester();
                    LoadSession();

                    if (Session["OfcId"] != null)
                    {                        
                        StudentApplicationOfficialInfoId.Value = Session["OfcId"].ToString();

                        int officialId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);
                        var officialInfo = dbObj.StudentApplicationOfficialInformations.Find(officialId);
                        if (officialInfo != null)
                        {                            
                            if (officialInfo.IsFinalSubmit.Value)
                            {
                                TogglePanel(false);
                                FinalMsg.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        TogglePanel(false);
                        FinalMsg.Visible = false;
                        Session["final"] = null;
                    }

                    LoadAllData();
                }

                if (Session["OfcId"] == null)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "toBottom();", true);
                }

                Session["OfcId"] = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadAllData()
        {
            LoadOfficialInformation();
            LoadPersonalInformation();
            LoadEducationalInformation();
            LoadPreviousSemseterInformation();
            LoadExpectedExamDetail();
        }

        private void LoadNationality()
        {
            try
            {
                var nationality = dbObj.Nationalities.ToList();

                ddlNationality.AppendDataBoundItems = true;
                ddlNationality.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));

                if (nationality.Count > 0)
                {
                    ddlNationality.DataSource = nationality.OrderBy(x => x.NationalityEng).ToList();
                    ddlNationality.DataTextField = "NationalityEng";
                    ddlNationality.DataValueField = "Id";
                    ddlNationality.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadEducationBoard()
        {
            try
            {
                var boardList = dbObj.EducationBoards.ToList();

                ddlEducationBoard.AppendDataBoundItems = true;
                ddlEducationBoard.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));

                if (boardList.Count > 0)
                {
                    ddlEducationBoard.DataSource = boardList.OrderBy(x => x.BoardEng).ToList();
                    ddlEducationBoard.DataTextField = "BoardEng";
                    ddlEducationBoard.DataValueField = "Id";
                    ddlEducationBoard.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadYear(DropDownList ddl)
        {
            ddl.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));
            for (int i = 1990; i <= DateTime.Now.Year; i++)
            {
                ddl.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }
        }

        private void LoadCourseCode()
        {
            var courseList = CourseManager.GetAll();

            ddlCourseCode.AppendDataBoundItems = true;
            ddlCourseCode.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));

            if (courseList.Count > 0)
            {
                ddlCourseCode.DataSource = courseList;
                ddlCourseCode.DataTextField = "CourseFullInfo";
                ddlCourseCode.DataValueField = "CourseID";
                ddlCourseCode.DataBind();
            }
        }

        private void LoadPreviousSemesterCourseCode()
        {
            var courseList = CourseManager.GetAll();

            ddlPreviousSemesterCourseCode.AppendDataBoundItems = true;
            ddlPreviousSemesterCourseCode.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));

            if (courseList.Count > 0)
            {
                ddlPreviousSemesterCourseCode.DataSource = courseList;
                ddlPreviousSemesterCourseCode.DataTextField = "CourseFullInfo";
                ddlPreviousSemesterCourseCode.DataValueField = "CourseID";
                ddlPreviousSemesterCourseCode.DataBind();
            }
        }

        private void LoadExaminarType()
        {
            try
            {
                ddlExaminarType.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));
                ddlExaminarType.Items.Add(new ListItem("নিয়মিত", "1"));
                ddlExaminarType.Items.Add(new ListItem("মানোন্নয়ন", "2"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadEducationInfo()
        {
            try
            {
                var educationExam = dbObj.Education.ToList();

                ddlExam.AppendDataBoundItems = true;
                ddlExam.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));

                if (educationExam.Count > 0)
                {
                    ddlExam.DataSource = educationExam;
                    ddlExam.DataTextField = "ExamNameEng";
                    ddlExam.DataValueField = "EducationId";
                    ddlExam.DataBind();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadProgram()
        {
            try
            {
                var programList = ProgramManager.GetAll();

                ddlProgram.AppendDataBoundItems = true;
                ddlProgram.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));

                if (programList.Count > 0)
                {
                    ddlProgram.DataSource = programList.OrderBy(x => x.ShortName).ToList();
                    ddlProgram.DataTextField = "ShortName";
                    ddlProgram.DataValueField = "ProgramID";
                    ddlProgram.DataBind();

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadSemester()
        {
            try
            {
                ddlStudentExamSemester.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));
                ddlStudentExamSemester.Items.Add(new ListItem("1st Semester", "1"));
                ddlStudentExamSemester.Items.Add(new ListItem("2nd Semester", "2"));
                ddlStudentExamSemester.Items.Add(new ListItem("3rd Semester", "3"));
                ddlStudentExamSemester.Items.Add(new ListItem("4th Semester", "4"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadSession()
        {
            try
            {
                ddlStudentExamSession.Items.Add(new ListItem("-নির্বাচন করুন-", "0"));
                ddlStudentExamSession.Items.Add(new ListItem("1st Year", "1"));
                ddlStudentExamSession.Items.Add(new ListItem("2nd Year", "2"));
                ddlStudentExamSession.Items.Add(new ListItem("3rd Year", "3"));
                ddlStudentExamSession.Items.Add(new ListItem("4th Year", "4"));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadEducationalInformation()
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter { ParameterName = "@OfficialId", SqlDbType = System.Data.SqlDbType.Int, Value = officialInfoId });

                DataTable dt = DataTableManager.GetDataFromQuery("StudentApplicationFromEducationInformationByOfficialId", parameters);

                grdEducation.DataSource = dt.Rows.Count > 0 ? dt : null;
                grdEducation.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadPreviousSemseterInformation()
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter { ParameterName = "@OfficialId", SqlDbType = System.Data.SqlDbType.Int, Value = officialInfoId });

                DataTable dt = DataTableManager.GetDataFromQuery("StudentApplicationFormPreviousSemesterExamInfoByOfficialId", parameters);

                grdPreviousSemester.DataSource = dt.Rows.Count > 0 ? dt : null;
                grdPreviousSemester.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadExpectedExamDetail()
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter { ParameterName = "@OfficialId", SqlDbType = System.Data.SqlDbType.Int, Value = officialInfoId });

                DataTable dt = DataTableManager.GetDataFromQuery("StudentApplicationFormExpectedExamCourseByOfficialId", parameters);

                grdExpectedCourse.DataSource = dt.Rows.Count > 0 ? dt : null;
                grdExpectedCourse.DataBind();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadOfficialInformation()
        {
            try
            {
                int officialInfoId = string.IsNullOrEmpty(StudentApplicationOfficialInfoId.Value) ? 0 : Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                var officialInfo = dbObj.StudentApplicationOfficialInformations.FirstOrDefault(x => x.StudentIDNo == txtIDNo.Text.Trim() || x.StudentApplicationOfficialInfoId == officialInfoId);

                if (officialInfo != null)
                {
                    txtDepartment.Text = officialInfo.AppliedDepartment;
                    txtFaculty.Text = officialInfo.AppliedFaculty;
                    txtHall.Text = officialInfo.AppliedHall;
                    ddlExaminarType.SelectedValue = officialInfo.StudentTypeId.ToString();
                    txtIDNo.Text = officialInfo.StudentIDNo;
                    txtRegistrationNo.Text = officialInfo.StudentRegNo;
                    txtStudentAcademicYear.Text = officialInfo.StudentAcademicYear;
                    ddlProgram.SelectedValue = officialInfo.AppliedProgramId == null ? "0" : officialInfo.AppliedProgramId.ToString();
                    ddlStudentExamSession.SelectedValue = officialInfo.AppliedSessionId == null ? "0" : officialInfo.AppliedSessionId.ToString();
                    ddlStudentExamSemester.SelectedValue = officialInfo.AppliedSemesterId == null ? "0" : officialInfo.AppliedSemesterId.ToString();
                    ddlStudentExamYear.SelectedValue = officialInfo.AppliedYearId == null ? "0" : officialInfo.AppliedYearId.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadPersonalInformation()
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);
                var personalInfo = dbObj.StudentApplicationFormPersonalInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                if (personalInfo != null)
                {
                    txtNameBng.Text = personalInfo.AppliedStudentNameBng;
                    txtNameEng.Text = personalInfo.AppliedStudentNameEng;
                    txtMotherName.Text = personalInfo.AppliedStudentMotherName;
                    txtFatherName.Text = personalInfo.AppliedStudentFatherName;
                    txtGuardianName.Text = personalInfo.AppliedStudentGuardianName;
                    ddlNationality.SelectedValue = personalInfo.StudentNationalityId.ToString();
                    txtDOB.Text = String.Format("{0:dd/MM/yyyy}", personalInfo.AppliedStudentDOB);
                    txtMobile.Text = personalInfo.AppliedStudentMobile;
                    txtPresentAddress.Text = personalInfo.AppliedStudentPresentAddress;
                    txtPermanentAddress.Text = personalInfo.AppliedStudentPermanentAddress;
                    imgPhoto.ImageUrl = string.IsNullOrEmpty(personalInfo.PhotoPath) ? "" : personalInfo.PhotoPath;
                    imgSignature.ImageUrl = string.IsNullOrEmpty(personalInfo.SignaturePath) ? "" : personalInfo.SignaturePath;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void ClearOfficialInfo()
        {
            StudentApplicationOfficialInfoId.Value = string.Empty;
            txtDepartment.Text = string.Empty;
            txtFaculty.Text = string.Empty;
            txtHall.Text = string.Empty;
            ddlExaminarType.ClearSelection();
            txtIDNo.Text = string.Empty;
            txtRegistrationNo.Text = string.Empty;
            ddlProgram.ClearSelection();
            ddlStudentExamSemester.ClearSelection();
            ddlStudentExamSession.ClearSelection();
            ddlStudentExamYear.ClearSelection();
        }

        private void ClearEducation()
        {
            ddlExam.ClearSelection();
            txtSchoolOrCollege.Text = string.Empty;
            ddlYear.ClearSelection();
            ddlEducationBoard.ClearSelection();
            txtRoll.Text = string.Empty;
            txtGrade.Text = string.Empty;
            txtExamShortDetail.Text = string.Empty;
            hdnEducationInfoId.Value = string.Empty;
        }

        private void ClearPreviousSemeseter()
        {
            txtPreviousSemesterExamName.Text = string.Empty;
            txtPreviousSemesterExamResult.Text = string.Empty;
            ddlPreviousSemesterCourseCode.ClearSelection();
            ddlExamYear.ClearSelection();
            txtPreviousSemesterCourseGP.Text = string.Empty;
            hdnPreviousSemeseterId.Value = string.Empty;
        }

        private void ClearExpectedCourse()
        {
            ddlCourseCode.ClearSelection();
            txtCourseSummary.Text = string.Empty;
            hdnExamCourseId.Value = string.Empty;
        }

        private void ClearImage()
        {
            imgPhoto.ImageUrl = "";
            imgSignature.ImageUrl = "";
        }


        protected void MessageView(string msg, string status)
        {
            if (status == "success")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "green");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-success";


            }
            else if (status == "fail")
            {
                lblMessage.Text = string.Empty;
                lblMessage.Text = msg.ToString();
                lblMessage.Attributes.CssStyle.Add("font-weight", "bold");
                lblMessage.Attributes.CssStyle.Add("color", "crimson");

                messagePanel.Visible = true;
                messagePanel.CssClass = "alert alert-danger";
            }
            else if (status == "clear")
            {
                lblMessage.Text = string.Empty;
                messagePanel.Visible = false;
            }

        }

        private void AlertMessage(string msg)
        {
            ScriptManager.RegisterStartupScript(this, GetType(), "MyScript", "alert('" + msg + "');", true);
        }

        private void TogglePanel(bool status)
        {
            personalDiv.Visible = status;
            EducationDiv.Visible = status;
            PreviousSemesterDiv.Visible = status;
            CourseDiv.Visible = status;
            picAndSigDiv.Visible = status;
        }


        protected void btnOfficialInfo_Click(object sender, EventArgs e)
        {
            try
            {
                int sessionId = Convert.ToInt32(ddlStudentExamSemester.SelectedValue);
                int semesterId = Convert.ToInt32(ddlStudentExamSemester.SelectedValue);
                int yearId = Convert.ToInt32(ddlStudentExamYear.SelectedValue);
                var officialInfo = dbObj.StudentApplicationOfficialInformations.FirstOrDefault(x => x.StudentIDNo == txtIDNo.Text.Trim() 
                    && x.StudentRegNo == txtRegistrationNo.Text.Trim()
                    && x.AppliedSemesterId == semesterId
                    && x.AppliedSessionId == sessionId
                    && x.AppliedYearId == yearId);                

                if (officialInfo != null)
                {
                    if (officialInfo.IsFinalSubmit.HasValue && officialInfo.IsFinalSubmit.Value)
                    {
                        hdnOfficialIdForAdmitCard.Value = officialInfo.StudentApplicationOfficialInfoId.ToString();
                        FinalMsg.Visible = true;
                        TogglePanel(false);
                        return;
                    }
                    else
                    {
                        hdnOfficialIdForAdmitCard.Value = string.Empty;
                        FinalMsg.Visible = false;                        
                    }

                    officialInfo.AppliedDepartment = txtDepartment.Text;
                    officialInfo.AppliedFaculty = txtFaculty.Text;
                    officialInfo.AppliedHall = txtHall.Text;
                    officialInfo.StudentTypeId = Convert.ToInt32(ddlExaminarType.SelectedValue);
                    officialInfo.StudentIDNo = txtIDNo.Text;
                    officialInfo.StudentRegNo = txtRegistrationNo.Text;
                    officialInfo.StudentAcademicYear = txtStudentAcademicYear.Text;
                    officialInfo.AppliedSessionId = Convert.ToInt32(ddlStudentExamSession.SelectedValue);
                    officialInfo.AppliedSemesterId = Convert.ToInt32(ddlStudentExamSemester.SelectedValue);
                    officialInfo.AppliedYearId = Convert.ToInt32(ddlStudentExamYear.SelectedValue);
                    officialInfo.AppliedProgramId = Convert.ToInt32(ddlProgram.SelectedValue);
                    officialInfo.IsDelete = false;
                    officialInfo.IpAddress = GetIPAddress();
                    officialInfo.IsFinalSubmit = false;
                    officialInfo.ModifiedBy = -1;
                    officialInfo.ModifiedDate = DateTime.Now;

                    dbObj.SaveChanges();                    
                    StudentApplicationOfficialInfoId.Value = officialInfo.StudentApplicationOfficialInfoId.ToString();
                }
                else
                {
                    var newOfficialInfo = new StudentApplicationOfficialInformation();

                    newOfficialInfo.AppliedDepartment = txtDepartment.Text;
                    newOfficialInfo.AppliedFaculty = txtFaculty.Text;
                    newOfficialInfo.AppliedHall = txtHall.Text;
                    newOfficialInfo.StudentTypeId = Convert.ToInt32(ddlExaminarType.SelectedValue);
                    newOfficialInfo.StudentIDNo = txtIDNo.Text;
                    newOfficialInfo.StudentRegNo = txtRegistrationNo.Text;
                    newOfficialInfo.StudentAcademicYear = txtStudentAcademicYear.Text;
                    newOfficialInfo.AppliedSessionId = Convert.ToInt32(ddlStudentExamSession.SelectedValue);
                    newOfficialInfo.AppliedSemesterId = Convert.ToInt32(ddlStudentExamSemester.SelectedValue);
                    newOfficialInfo.AppliedYearId = Convert.ToInt32(ddlStudentExamYear.SelectedValue);
                    newOfficialInfo.AppliedProgramId = Convert.ToInt32(ddlProgram.SelectedValue);
                    newOfficialInfo.IsDelete = false;
                    newOfficialInfo.IpAddress = GetIPAddress();
                    newOfficialInfo.IsFinalSubmit = false;
                    newOfficialInfo.CreatedBy = -1;
                    newOfficialInfo.CreatedDate = DateTime.Now;

                    dbObj.StudentApplicationOfficialInformations.Add(newOfficialInfo);
                    dbObj.SaveChanges();

                    StudentApplicationOfficialInfoId.Value = newOfficialInfo.StudentApplicationOfficialInfoId.ToString();
                }

                AlertMessage("Saved successful");

                LoadOfficialInformation();
                LoadPersonalInformation();
                LoadEducationalInformation();
                LoadPreviousSemseterInformation();
                LoadExpectedExamDetail();

                TogglePanel(true);
                ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "GetInfo()", true);
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }            
        }

        protected void btnPersonalInfoSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);
                var personalInfo = dbObj.StudentApplicationFormPersonalInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                if (personalInfo == null)
                {
                    var newPersonalInfo = new StudentApplicationFormPersonalInformation();

                    newPersonalInfo.StudentApplicationOfficialInfoId = officialInfoId;
                    newPersonalInfo.AppliedStudentNameEng = txtNameEng.Text;
                    newPersonalInfo.AppliedStudentNameBng = txtNameBng.Text;
                    newPersonalInfo.AppliedStudentMotherName = txtMotherName.Text;
                    newPersonalInfo.AppliedStudentFatherName = txtFatherName.Text;
                    newPersonalInfo.AppliedStudentGuardianName = txtGuardianName.Text;
                    newPersonalInfo.StudentNationalityId = Convert.ToInt32(ddlNationality.SelectedValue);
                    newPersonalInfo.AppliedStudentDOB = DateTime.ParseExact(txtDOB.Text.Replace('-', '/'), "dd/MM/yyyy", null);
                    newPersonalInfo.AppliedStudentMobile = lblMobilePrefix.Text + txtMobile.Text;
                    newPersonalInfo.AppliedStudentPresentAddress = txtPresentAddress.Text;
                    newPersonalInfo.AppliedStudentPermanentAddress = txtPermanentAddress.Text;
                    newPersonalInfo.CreatedBy = -1;
                    newPersonalInfo.CreatedDate = DateTime.Now;

                    dbObj.StudentApplicationFormPersonalInformations.Add(newPersonalInfo);
                    dbObj.SaveChanges();
                }
                else
                {
                    personalInfo.AppliedStudentNameEng = txtNameEng.Text;
                    personalInfo.AppliedStudentNameBng = txtNameBng.Text;
                    personalInfo.AppliedStudentMotherName = txtMotherName.Text;
                    personalInfo.AppliedStudentFatherName = txtFatherName.Text;
                    personalInfo.AppliedStudentGuardianName = txtGuardianName.Text;
                    personalInfo.StudentNationalityId = Convert.ToInt32(ddlNationality.SelectedValue);
                    personalInfo.AppliedStudentDOB = DateTime.ParseExact(txtDOB.Text.Replace('-', '/'), "dd/MM/yyyy", null);
                    personalInfo.AppliedStudentMobile = lblMobilePrefix.Text + txtMobile.Text;
                    personalInfo.AppliedStudentPresentAddress = txtPresentAddress.Text;
                    personalInfo.AppliedStudentPermanentAddress = txtPermanentAddress.Text;
                    personalInfo.ModifiedBy = -1;
                    personalInfo.ModifiedDate = DateTime.Now;
                    
                    dbObj.SaveChanges();
                }

                AlertMessage("Saved successful");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                LoadPersonalInformation();
            }
        }

        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];

        }

        protected void btnEducationSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int educationInfoId = string.IsNullOrEmpty(hdnEducationInfoId.Value) ? 0 : Convert.ToInt32(hdnEducationInfoId.Value);
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                if (educationInfoId == 0)
                {
                    var newEducationInfo = new StudentApplicationFormEducationInfo();

                    newEducationInfo.StudentApplicationOfficialInfoId = officialInfoId; 
                    newEducationInfo.AppliedStudentExamNameId = Convert.ToInt32(ddlExam.SelectedValue);
                    newEducationInfo.AppliedStudentSchoolOrCollege = txtSchoolOrCollege.Text;
                    newEducationInfo.AppliedStudentYearId = Convert.ToInt32(ddlYear.SelectedValue);
                    newEducationInfo.AppliedStudentBoardId = Convert.ToInt32(ddlEducationBoard.SelectedValue);
                    newEducationInfo.AppliedStudentRoll = txtRoll.Text;
                    newEducationInfo.AppliedStudentGrade = Convert.ToDecimal(txtGrade.Text);
                    newEducationInfo.AppliedStudentExamRemarks = txtExamShortDetail.Text;
                    newEducationInfo.CreatedBy = -1;
                    newEducationInfo.CreatedDate = DateTime.Now;

                    dbObj.StudentApplicationFormEducationInfoes.Add(newEducationInfo);
                    dbObj.SaveChanges();
                }
                else
                {
                    var educationInfo = dbObj.StudentApplicationFormEducationInfoes.FirstOrDefault(x => x.StudentApplicationFormEducationId == educationInfoId && x.StudentApplicationOfficialInfoId == officialInfoId);

                    if (educationInfo != null)
                    {
                        educationInfo.AppliedStudentExamNameId = Convert.ToInt32(ddlExam.SelectedValue);
                        educationInfo.AppliedStudentSchoolOrCollege = txtSchoolOrCollege.Text;
                        educationInfo.AppliedStudentYearId = Convert.ToInt32(ddlYear.SelectedValue);
                        educationInfo.AppliedStudentBoardId = Convert.ToInt32(ddlEducationBoard.SelectedValue);
                        educationInfo.AppliedStudentRoll = txtRoll.Text;
                        educationInfo.AppliedStudentGrade = Convert.ToDecimal(txtGrade.Text);
                        educationInfo.AppliedStudentExamRemarks = txtExamShortDetail.Text;
                        educationInfo.ModifiedBy = -1;
                        educationInfo.ModifiedDate = DateTime.Now;

                        dbObj.SaveChanges();                        
                    }
                }
                ClearEducation();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                LoadEducationalInformation();
            }
        }

        protected void lnkEducationEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;
                int id = Convert.ToInt32(lnkButton.CommandArgument);

                var education = dbObj.StudentApplicationFormEducationInfoes.Find(id);

                if (education != null)
                {
                    hdnEducationInfoId.Value = education.StudentApplicationFormEducationId.ToString();
                    ddlExam.SelectedValue = education.AppliedStudentExamNameId.ToString();
                    txtSchoolOrCollege.Text = education.AppliedStudentSchoolOrCollege;
                    ddlYear.SelectedValue = education.AppliedStudentYearId.ToString();
                    ddlEducationBoard.SelectedValue = education.AppliedStudentBoardId.ToString();
                    txtRoll.Text = education.AppliedStudentRoll;
                    txtGrade.Text = education.AppliedStudentGrade.ToString();
                    txtExamShortDetail.Text = education.AppliedStudentExamRemarks;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void lnkEducationDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;
                int id = Convert.ToInt32(lnkButton.CommandArgument);

                var education = dbObj.StudentApplicationFormEducationInfoes.Find(id);

                if (education != null)
                {
                    dbObj.StudentApplicationFormEducationInfoes.Remove(education);
                    dbObj.SaveChanges();
                }

                LoadEducationalInformation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void btnPreviousSemesterInfoSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int previousSemseterId = string.IsNullOrEmpty(hdnPreviousSemeseterId.Value) ? 0 : Convert.ToInt32(hdnPreviousSemeseterId.Value);
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                var previousSemeseterCgpaCheck = dbObj.StudentApplicationFormPreviousSemesterInfoes.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                if (previousSemeseterCgpaCheck != null)
                {
                    if (Convert.ToDecimal(txtPreviousSemesterExamResult.Text) != previousSemeseterCgpaCheck.PreviousSemesterResult)
                    {
                        AlertMessage("ফলাফল (GPA/CGPA) সঠিক নয়। পুনরায় চেক করুন।");
                        return;
                    }
                }


                if (previousSemseterId == 0)
                {
                    var newPreviousSemesterInfo = new StudentApplicationFormPreviousSemesterInfo();

                    newPreviousSemesterInfo.StudentApplicationOfficialInfoId = officialInfoId;
                    newPreviousSemesterInfo.PreviousSemesterExamName = txtPreviousSemesterExamName.Text;
                    newPreviousSemesterInfo.PreviousSemesterResult = Convert.ToDecimal(txtPreviousSemesterExamResult.Text);
                    newPreviousSemesterInfo.PreviousSemseterCourseId = Convert.ToInt32(ddlPreviousSemesterCourseCode.SelectedValue);
                    newPreviousSemesterInfo.PreviousSemesterExamYearId = Convert.ToInt32(ddlExamYear.SelectedValue);
                    newPreviousSemesterInfo.PreviousSemseterCourseGP = Convert.ToDecimal(txtPreviousSemesterCourseGP.Text);
                    newPreviousSemesterInfo.CreatedBy = -1;
                    newPreviousSemesterInfo.CreatedDate = DateTime.Now;

                    dbObj.StudentApplicationFormPreviousSemesterInfoes.Add(newPreviousSemesterInfo);
                    dbObj.SaveChanges();
                }
                else
                {
                    var previousSemeseterInfo = dbObj.StudentApplicationFormPreviousSemesterInfoes.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId && x.PreviousSemesterId == previousSemseterId);

                    if (previousSemeseterInfo != null)
                    {                        
                        previousSemeseterInfo.PreviousSemesterExamName = txtPreviousSemesterExamName.Text;
                        previousSemeseterInfo.PreviousSemesterResult = Convert.ToDecimal(txtPreviousSemesterExamResult.Text);
                        previousSemeseterInfo.PreviousSemseterCourseId = Convert.ToInt32(ddlPreviousSemesterCourseCode.SelectedValue);
                        previousSemeseterInfo.PreviousSemesterExamYearId = Convert.ToInt32(ddlExamYear.SelectedValue);
                        previousSemeseterInfo.PreviousSemseterCourseGP = Convert.ToDecimal(txtPreviousSemesterCourseGP.Text);
                        previousSemeseterInfo.ModifiedBy = -1;
                        previousSemeseterInfo.ModifiedDate = DateTime.Now;

                        dbObj.SaveChanges();                        
                    }
                }

                ClearPreviousSemeseter();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                LoadPreviousSemseterInformation();
            }
        }

        protected void lnkPreviousSemesterEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;
                int id = Convert.ToInt32(lnkButton.CommandArgument);

                var previousSemester = dbObj.StudentApplicationFormPreviousSemesterInfoes.Find(id);

                if (previousSemester != null)
                {
                    hdnPreviousSemeseterId.Value = previousSemester.PreviousSemesterId.ToString();
                    txtPreviousSemesterExamName.Text = previousSemester.PreviousSemesterExamName;
                    txtPreviousSemesterExamResult.Text = previousSemester.PreviousSemesterResult.ToString();
                    ddlPreviousSemesterCourseCode.SelectedValue = previousSemester.PreviousSemseterCourseId.ToString();
                    ddlExamYear.SelectedValue = previousSemester.PreviousSemesterExamYearId.ToString();
                    txtPreviousSemesterCourseGP.Text = previousSemester.PreviousSemseterCourseGP.ToString();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void lnkPreviousSemesterDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;
                int id = Convert.ToInt32(lnkButton.CommandArgument);

                var previousSemester = dbObj.StudentApplicationFormPreviousSemesterInfoes.Find(id);

                if (previousSemester != null)
                {
                    dbObj.StudentApplicationFormPreviousSemesterInfoes.Remove(previousSemester);
                    dbObj.SaveChanges();
                }

                LoadPreviousSemseterInformation();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }              

        protected void btnExamCourseSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                int examCourseId = string.IsNullOrEmpty(hdnExamCourseId.Value) ? 0 : Convert.ToInt32(hdnExamCourseId.Value);
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                if (examCourseId == 0)
                {
                    var newExamCourse = new StudentApplicationFormAppliedCourse();

                    newExamCourse.StudentApplicationOfficialInfoId = officialInfoId;
                    newExamCourse.AppliedCourseCodeId = Convert.ToInt32(ddlCourseCode.SelectedValue);
                    newExamCourse.AppliedCourseSummary = txtCourseSummary.Text;
                    newExamCourse.CreatedBy = -1;
                    newExamCourse.CreatedDate = DateTime.Now;

                    dbObj.StudentApplicationFormAppliedCourses.Add(newExamCourse);
                    dbObj.SaveChanges();
                }
                else
                {
                    var examCourse = dbObj.StudentApplicationFormAppliedCourses.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId && x.AppliedCourseId == examCourseId);

                    if (examCourse != null)
                    {
                        examCourse.AppliedCourseCodeId = Convert.ToInt32(ddlCourseCode.SelectedValue);
                        examCourse.AppliedCourseSummary = txtCourseSummary.Text;
                        examCourse.ModifiedBy = -1;
                        examCourse.ModifiedDate = DateTime.Now;

                        dbObj.SaveChanges();                        
                    }
                }
                ClearExpectedCourse();
            }                                                      
            catch (Exception ex)                                   
            {                                                      
                Console.WriteLine(ex.Message);
            }
            finally
            {
                LoadExpectedExamDetail();
            }
        }

        protected void lnkExpectedCourseEdit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;
                int id = Convert.ToInt32(lnkButton.CommandArgument);

                var course = dbObj.StudentApplicationFormAppliedCourses.Find(id);

                if (course != null)
                {
                    hdnExamCourseId.Value = course.AppliedCourseId.ToString();
                    ddlCourseCode.SelectedValue = course.AppliedCourseCodeId.ToString();
                    txtCourseSummary.Text = course.AppliedCourseSummary;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void lnkExpectedCourseDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton lnkButton = (LinkButton)sender;
                int id = Convert.ToInt32(lnkButton.CommandArgument);

                var course = dbObj.StudentApplicationFormAppliedCourses.Find(id);

                if (course != null)
                {
                    dbObj.StudentApplicationFormAppliedCourses.Remove(course);
                    dbObj.SaveChanges();
                }

                LoadExpectedExamDetail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void btnImageUpload_Click(object sender, EventArgs e)
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                if (imgPhotoUpload.HasFile)
                {
                    int fileSize = imgPhotoUpload.PostedFile.ContentLength / 1024;
                    if (fileSize >= 200)
                    {
                        MessageView("Size of image is too large. Maximum file size permitted is 200KB", "fail");
                        TogglePanel(true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "ToggleDiv(true)", true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", "alert('Size of image is too large. Maximum file size permitted is 200KB')", true);
                        return;
                    }
                    
                    if (!IsFileTypeAccepted(imgPhotoUpload))
                    {
                        MessageView("File type not accpted. Only jpg, jpeg & png format accepted.", "fail");
                        TogglePanel(true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "ToggleDiv(true)", true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", "alert('File type not accpted. Only jpg, jpeg & png format accepted.')", true);
                        return;
                    }

                    System.Drawing.Image img = System.Drawing.Image.FromStream(imgPhotoUpload.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    if (height > 300 || width > 300)
                    {
                        MessageView("Photo dimensions must not exceed 300 X 300 pixel.", "fail");
                        TogglePanel(true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "ToggleDiv(true)", true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", "alert('Photo dimensions must not exceed 300 KB.')", true);
                        return;
                    }


                    var officialInfo = dbObj.StudentApplicationOfficialInformations.Find(officialInfoId);
                    var personalInfo = dbObj.StudentApplicationFormPersonalInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                    if (officialInfo != null)
                    {
                        string fileExtension = Path.GetExtension(imgPhotoUpload.FileName);
                        string filePath = "~/Upload/AppliPhoto/" + officialInfo.StudentIDNo.ToString() + fileExtension;

                        if (personalInfo != null)
                        {
                            personalInfo.PhotoPath = filePath;
                            personalInfo.ModifiedDate = DateTime.Now;

                            dbObj.SaveChanges();

                            imgPhoto.ImageUrl = personalInfo.PhotoPath;
                        }


                        if (File.Exists(Server.MapPath(personalInfo.PhotoPath)))
                        {
                            File.Delete(Server.MapPath(personalInfo.PhotoPath));
                            imgPhotoUpload.SaveAs(Server.MapPath(filePath));                            
                        }
                        else
                        {
                            imgPhotoUpload.SaveAs(Server.MapPath(filePath));                            
                        }

                        MessageView("", "clear");

                        imgPhoto.ImageUrl = personalInfo.PhotoPath;
                    }                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void btnSignatureUpload_Click(object sender, EventArgs e)
        {
            try
            {
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);

                if (imgSignatureUpload.HasFile)
                {
                    int fileSize = imgSignatureUpload.PostedFile.ContentLength / 1024;
                    if (fileSize >= 200)
                    {
                        MessageView("Size of image is too large. Maximum file size permitted is 200KB", "fail");
                        TogglePanel(true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "ToggleDiv(true)", true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", "alert('Size of image is too large. Maximum file size permitted is 200KB')", true);
                        return;
                    }

                    if (!IsFileTypeAccepted(imgSignatureUpload))
                    {
                        MessageView("File type not accpted. Only jpg, jpeg & png format accepted.", "fail");
                        TogglePanel(true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "ToggleDiv(true)", true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", "alert('File type not accpted. Only jpg, jpeg & png format accepted.')", true);
                        return;
                    }

                    System.Drawing.Image img = System.Drawing.Image.FromStream(imgSignatureUpload.PostedFile.InputStream);
                    int height = img.Height;
                    int width = img.Width;
                    if (height > 80 || width > 100)
                    {
                        MessageView("Signature dimensions must not exceed 100 X 80 pixel.", "fail");
                        TogglePanel(true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), this.GetType(), "function", "ToggleDiv(true)", true);
                        //ScriptManager.RegisterClientScriptBlock((sender as Control), GetType(), "alert", "alert('Photo dimensions must not exceed 300 KB.')", true);
                        return;
                    }

                    var officialInfo = dbObj.StudentApplicationOfficialInformations.Find(officialInfoId);
                    var personalInfo = dbObj.StudentApplicationFormPersonalInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                    if (officialInfo != null)
                    {
                        string fileExtension = Path.GetExtension(imgSignatureUpload.FileName);
                        string filePath = "~/Upload/AppliSignature/" + officialInfo.StudentIDNo.ToString() + fileExtension;


                        if (personalInfo != null)
                        {
                            personalInfo.SignaturePath = filePath;
                            personalInfo.ModifiedDate = DateTime.Now;

                            dbObj.SaveChanges();

                            imgSignature.ImageUrl = personalInfo.SignaturePath;
                        }

                        if (File.Exists(Server.MapPath(personalInfo.SignaturePath)))
                        {
                            File.Delete(Server.MapPath(personalInfo.SignaturePath));
                            imgSignatureUpload.SaveAs(Server.MapPath(filePath));
                        }
                        else
                        {
                            imgSignatureUpload.SaveAs(Server.MapPath(filePath));
                        }

                        MessageView("", "clear");
                        imgSignature.ImageUrl = personalInfo.SignaturePath;
                    }   
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool IsFileTypeAccepted(FileUpload file)
        {
            bool acceptFile = false;
            string fileExtension = Path.GetExtension(file.FileName);
            fileExtension = fileExtension.ToLower();

            string[] acceptedFileTypes = new string[3];
            acceptedFileTypes[0] = ".jpg";
            acceptedFileTypes[1] = ".jpeg";
            acceptedFileTypes[2] = ".png";
            
            for (int i = 0; i < 3; i++)
            {
                if (fileExtension == acceptedFileTypes[i])
                {
                    acceptFile = true;
                    break;
                }
            }

            return acceptFile;
        }        

        [WebMethod]
        public static string InfoUpdateStatus(int officialId)
        {
            var dbObj = new StudentApplicationFormModel();

            var studentOfficialInfo = dbObj.StudentApplicationOfficialInformations.Find(officialId);

            return studentOfficialInfo != null ? "1" : "0";
        }

        protected void btnFinalSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Session["OfcId"] = null;
                Session["final"] = null;
                int officialInfoId = Convert.ToInt32(StudentApplicationOfficialInfoId.Value);                
                Response.Redirect("~/Module/student/RptStudentApplicationForm.aspx?Id=" + officialInfoId.ToString(), true);
                //if (officialInfoId > 0)
                //{
                //    var officialInfo = dbObj.StudentApplicationOfficialInformations.FirstOrDefault(x => x.StudentApplicationOfficialInfoId == officialInfoId);

                //    if (officialInfo != null)
                //    {
                //        officialInfo.IsFinalSubmit = true;
                //        officialInfo.ModifiedBy = -1;
                //        officialInfo.ModifiedDate = DateTime.Now;

                //        dbObj.SaveChanges();

                //        ClearOfficialInfo();
                //        ClearEducation();
                //        ClearPreviousSemeseter();
                //        ClearExpectedCourse();
                //        ClearImage();

                //        grdEducation.DataSource = null;
                //        grdEducation.DataBind();

                //        grdExpectedCourse.DataSource = null;
                //        grdExpectedCourse.DataBind();

                //        grdPreviousSemester.DataSource = null;
                //        grdPreviousSemester.DataBind();

                //        TogglePanel(false);
                //        FinalMsg.Visible = true;
                //        Page.ClientScript.RegisterStartupScript(this.GetType(), "myScript", "alert('Final submit done!');", true);                        
                //    }
                //}
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected void ddlCourseCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var courseId = Convert.ToInt32(ddlCourseCode.SelectedValue);

                if (courseId > 0)
                {
                    var courseDetail = CourseManager.GetByCourseIdVersionId(courseId, 1);
                    txtCourseSummary.Text = string.IsNullOrEmpty(courseDetail.Title) ? "" : courseDetail.Title;
                }
                else
                {
                    txtCourseSummary.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
         }

        protected void lnkAdminCardDownlad_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(hdnOfficialIdForAdmitCard.Value))
                {
                    LoadReport(Convert.ToInt32(hdnOfficialIdForAdmitCard.Value));
                }               
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private void LoadReport(int officialId)
        {
            var studentOfficialInfo = StudentApplicationFormManager.GetStudentApplicationDetailsByOfficialId(officialId);            
            var studentAppliedCourseInfo = StudentApplicationFormManager.GetStudentApplicationAppliedCourseDetailsByOfficialId(officialId);

            if (studentOfficialInfo != null)
            {
                var department = studentOfficialInfo.FirstOrDefault().Department.ToString();
                var year = studentOfficialInfo.FirstOrDefault().AppliedYear.ToString();
                var semester = studentOfficialInfo.FirstOrDefault().AppliedSemester;
                var session = studentOfficialInfo.FirstOrDefault().AppliedSession;
                var program = studentOfficialInfo.FirstOrDefault().AppliedProgram;

                ReportViewer1.LocalReport.DataSources.Clear();

                ReportParameter p1 = new ReportParameter("PhotoPath", new Uri(Server.MapPath(studentOfficialInfo.FirstOrDefault().PhotoPath)).AbsoluteUri);
                ReportParameter p2 = new ReportParameter("Department", department);
                ReportParameter p3 = new ReportParameter("Year", year);
                ReportParameter p4 = new ReportParameter("Session", session);
                ReportParameter p5 = new ReportParameter("Semester", semester);

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/student/Report/AdmitCard.rdlc");
                ReportViewer1.LocalReport.EnableExternalImages = true;
                ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2, p3, p4, p5 });

                ReportDataSource rds1 = new ReportDataSource("OfficialDataSet", studentOfficialInfo);
                ReportDataSource rds2 = new ReportDataSource("AppliedCourseDataSet", studentAppliedCourseInfo);

                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ReportViewer1.LocalReport.DataSources.Add(rds2);

                Warning[] warnings;
                string[] streamids;
                string mimeType;
                string encoding;
                string filenameExtension;

                byte[] bytes = ReportViewer1.LocalReport.Render("PDF", null, out mimeType, out encoding, out filenameExtension, out streamids, out warnings);

                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = mimeType;
                Response.AppendHeader("Content-Disposition", "attachment; filename=AdmitCard_" + studentOfficialInfo.FirstOrDefault().StudentIDNo.ToString() + "." + filenameExtension);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
            }
        }
    }
}