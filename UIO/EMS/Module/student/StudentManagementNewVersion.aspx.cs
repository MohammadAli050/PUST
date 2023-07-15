using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System.Data.Entity;

namespace EMS.Module.student
{
    public partial class StudentManagementNewVersion : BasePage
    {
        private string _StudentList = "SessionStudentList";
        BussinessObject.UIUMSUser BaseCurrentUserObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.CheckPage_Load();
            base.CheckPage_Load();
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                //txtGPAGraduate.Text = "0";
                //txtGPAHigherSecondary.Text = "0";
                //txtGPASecondary.Text = "0";
                //txtGPAUndergraduate.Text = "0";
                //txtGW4SHigherSecondary.Text = "0";
                //txtGW4SSecondary.Text = "0";
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
                LoadDropDowns();

                DateTime endDate = DateTime.Now;

                txtDOB.Text = endDate.ToString("dd/MM/yyyy");
                LoadModalDropDown();
            }
        }

        private void LoadModalDropDown()
        {
            LoadGender();
            LoadMaritalStatus();
            LoadBloodGroup();
            LoadReligion();
            LoadQuata();
            LoadParentsJob();
            LoadHallInformation();

        }

        private void LoadHallInformation()
        {
            try
            {
                DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();
                var HallList = ucamContext.HallInformations.Where(x => x.ActiveStatus == 1).ToList();

                ddlHallInfo.Items.Clear();
                ddlHallInfo.AppendDataBoundItems = true;
                ddlHallInfo.Items.Add(new ListItem("Select", "0"));


                if (HallList != null && HallList.Any())
                {
                    ddlHallInfo.DataTextField = "HallCode";
                    ddlHallInfo.DataValueField = "Id";
                    ddlHallInfo.DataSource = HallList.OrderBy(x => x.HallCode);
                    ddlHallInfo.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadGender()
        {
            List<Value> genderList = ValueManager.GetByValueSetId(3);//Value set Id 3 for Gender
            genderList = genderList.OrderBy(x => x.ValueCode).ToList();
            ddlGender.Items.Clear();
            ddlGender.AppendDataBoundItems = true;
            ddlGender.Items.Add(new ListItem("-Select-", "0"));
            if (genderList != null)
            {
                ddlGender.DataTextField = "ValueName";
                ddlGender.DataValueField = "ValueID";

                ddlGender.DataSource = genderList;
                ddlGender.DataBind();
            }
        }

        private void LoadMaritalStatus()
        {
            List<Value> maritalList = ValueManager.GetByValueSetId(6);//Value set Id 6 for Marital Status
            maritalList = maritalList.OrderBy(x => x.ValueCode).ToList();
            ddlmaritalStatus.Items.Clear();
            ddlmaritalStatus.AppendDataBoundItems = true;
            ddlmaritalStatus.Items.Add(new ListItem("-Select-", "0"));
            if (maritalList != null)
            {
                ddlmaritalStatus.DataTextField = "ValueName";
                ddlmaritalStatus.DataValueField = "ValueID";

                ddlmaritalStatus.DataSource = maritalList;
                ddlmaritalStatus.DataBind();
            }
        }

        private void LoadBloodGroup()
        {
            List<Value> bloodList = ValueManager.GetByValueSetId(7);//Value set Id 7 for Blood Group
            bloodList = bloodList.OrderBy(x => x.ValueCode).ToList();
            ddlBloodGroup.Items.Clear();
            ddlBloodGroup.AppendDataBoundItems = true;
            ddlBloodGroup.Items.Add(new ListItem("-Select-", "0"));
            if (bloodList != null)
            {
                ddlBloodGroup.DataTextField = "ValueName";
                ddlBloodGroup.DataValueField = "ValueID";

                ddlBloodGroup.DataSource = bloodList;
                ddlBloodGroup.DataBind();
            }
        }

        private void LoadReligion()
        {
            List<Value> religionList = ValueManager.GetByValueSetId(4);//Value set Id 4 for Religion
            religionList = religionList.OrderBy(x => x.ValueCode).ToList();
            ddlReligion.Items.Clear();
            ddlReligion.AppendDataBoundItems = true;
            ddlReligion.Items.Add(new ListItem("-Select-", "0"));
            if (religionList != null)
            {
                ddlReligion.DataTextField = "ValueName";
                ddlReligion.DataValueField = "ValueID";

                ddlReligion.DataSource = religionList;
                ddlReligion.DataBind();
            }
        }

        private void LoadQuata()
        {
            List<Value> quataList = ValueManager.GetByValueSetId(9);//Value set Id 8 for Quata
            quataList = quataList.OrderBy(x => x.ValueCode).ToList();
            ddlQuata.Items.Clear();
            ddlQuata.AppendDataBoundItems = true;
            ddlQuata.Items.Add(new ListItem("-Select-", "0"));
            if (quataList != null)
            {
                ddlQuata.DataTextField = "ValueName";
                ddlQuata.DataValueField = "ValueID";

                ddlQuata.DataSource = quataList;
                ddlQuata.DataBind();
            }
        }

        private void LoadParentsJob()
        {
            List<Value> jobList = ValueManager.GetByValueSetId(9);//Value set Id 9 for Job
            jobList = jobList.OrderBy(x => x.ValueCode).ToList();
            ddlParentJob.Items.Clear();
            ddlParentJob.AppendDataBoundItems = true;
            ddlParentJob.Items.Add(new ListItem("-Select-", "0"));
            if (jobList != null)
            {
                ddlParentJob.DataTextField = "ValueName";
                ddlParentJob.DataValueField = "ValueID";

                ddlParentJob.DataSource = jobList;
                ddlParentJob.DataBind();
            }
        }

        private void LoadYearNoDDL()
        {
            List<YearDistinctDTO> yearList = new List<YearDistinctDTO>();
            yearList = YearManager.GetAllDistinct();
            yearList = yearList.OrderBy(x => x.YearNo).ToList();


            ddlYearNo.Items.Clear();
            ddlYearNo.AppendDataBoundItems = true;
            ddlYearNo.Items.Add(new ListItem("-Select-", "0"));
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
            ddlSemesterNo.Items.Add(new ListItem("-Select-", "0"));
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
                gvStudentList.DataSource = null;
                gvStudentList.DataBind();
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
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();
            lblMsg.Text = string.Empty;
            ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
        }

        protected void ddlYearNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();
        }

        protected void ddlSemesterNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();
        }

        protected void ucSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();
        }

        private void LoadDropDowns()
        {
            //ddlGender.Items.Clear();
            //ddlGender.Items.Add(new ListItem("Select", "0"));
            //ddlGender.Items.Add(new ListItem("Male", "Male"));
            //ddlGender.Items.Add(new ListItem("Female", "Female"));

            //ddlReligion.Items.Clear();
            //ddlReligion.Items.Add(new ListItem("Select", "0"));
            //ddlReligion.Items.Add(new ListItem("Islam", "1"));
            //ddlReligion.Items.Add(new ListItem("Hindu", "2"));
            //ddlReligion.Items.Add(new ListItem("Buddha", "3"));
            //ddlReligion.Items.Add(new ListItem("Christian", "4"));

            //ddlMaritalStat.Items.Clear();
            //ddlMaritalStat.Items.Add(new ListItem("Select", "0"));
            //ddlMaritalStat.Items.Add(new ListItem("Married", "Married"));
            //ddlMaritalStat.Items.Add(new ListItem("Unmarried", "Unmarried"));

            #region Exam

            List<PreviousExamType> examTypeList = PreviousExamTypeManager.GetAll();
            List<PreviousExamType> collection = new List<PreviousExamType>();

            //ddlCampus.Items.Clear();
            //ddlCampus.Items.Add(new ListItem("-Select-", "0"));
            //ddlCampus.AppendDataBoundItems = true;
            //List<Campus> campusList = CampusManager.GetAll();
            //if (campusList != null)
            //{
            //    ddlCampus.DataValueField = "CampusId";
            //    ddlCampus.DataTextField = "CampusName";
            //    ddlCampus.DataSource = campusList;
            //    ddlCampus.DataBind();
            //}

            //ddlProgram.Items.Clear();
            //ddlProgram.Items.Add(new ListItem("-Select-", "0"));
            //ddlProgram.AppendDataBoundItems = true;
            //List<Program> programList = ProgramManager.GetAll();
            //if (programList != null)
            //{
            //    ddlProgram.DataValueField = "ProgramID";
            //    ddlProgram.DataTextField = "ShortName";
            //    ddlProgram.DataSource = programList;
            //    ddlProgram.DataBind();
            //}
            #region Board
            //ddlBoardSecondary.Items.Clear();
            //ddlBoardSecondary.Items.Add(new ListItem("-Select-", "0"));
            //ddlBoardHigherSecondary.Items.Clear();
            //ddlBoardHigherSecondary.Items.Add(new ListItem("-Select-", "0"));
            //String[] names = Enum.GetNames(typeof(CommonEnum.EducationBoard));
            //int[] values = (int[])Enum.GetValues(typeof(CommonEnum.EducationBoard));

            //for (int i = 0; i < names.Length; i++)
            //{
            //    ddlBoardSecondary.Items.Add(new ListItem(names[i], values[i].ToString()));
            //    ddlBoardHigherSecondary.Items.Add(new ListItem(names[i], values[i].ToString()));
            //}
            #endregion

            #region ExamType Secondary
            //ddlExamTypeSecondary.Items.Clear();
            //ddlExamTypeSecondary.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeSecondary.AppendDataBoundItems = true;
            //ddlExamTypeSecondary.DataTextField = "Code";
            //ddlExamTypeSecondary.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Secondary).ToList();

            //    ddlExamTypeSecondary.DataSource = collection;
            //    ddlExamTypeSecondary.DataBind();
            //}
            #endregion

            #region ExamType Higher Secondary
            //ddlExamTypeHigherSecondary.Items.Clear();
            //ddlExamTypeHigherSecondary.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeHigherSecondary.AppendDataBoundItems = true;
            //ddlExamTypeHigherSecondary.DataTextField = "Code";
            //ddlExamTypeHigherSecondary.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Higher_Secondary).ToList();

            //    ddlExamTypeHigherSecondary.DataSource = collection;
            //    ddlExamTypeHigherSecondary.DataBind();
            //}
            #endregion

            #region ExamType Undergraduate
            //ddlExamTypeUndergraduate.Items.Clear();
            //ddlExamTypeUndergraduate.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeUndergraduate.AppendDataBoundItems = true;
            //ddlExamTypeUndergraduate.DataTextField = "Code";
            //ddlExamTypeUndergraduate.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Undergraduate).ToList();

            //    ddlExamTypeUndergraduate.DataSource = collection;
            //    ddlExamTypeUndergraduate.DataBind();
            //}
            #endregion

            #region ExamType Graduate
            //ddlExamTypeGraduate.Items.Clear();
            //ddlExamTypeGraduate.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeGraduate.AppendDataBoundItems = true;
            //ddlExamTypeGraduate.DataTextField = "Code";
            //ddlExamTypeGraduate.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Graduate).ToList();

            //    ddlExamTypeGraduate.DataSource = collection;
            //    ddlExamTypeGraduate.DataBind();
            //}
            #endregion

            #endregion

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Validation().Length > 0)
            {
                ShowAlertMessage(Validation());
                ModalPopupExtender1.Show();
            }
            else
            {
                ModalPopupExtender1.Show();
                int StudentId = Convert.ToInt32(hdnStudentId.Value);
                int PersonId = Convert.ToInt32(hfPersonID.Value);
                if (StudentId > 0 && PersonId > 0)
                {
                    SavePersonBasicInfo(PersonId, StudentId);
                    SaveParmanentAddress(PersonId);
                    SavePresentAddress(PersonId);
                    SaveSSCResult(PersonId);
                    SaveHSCResult(PersonId);
                    DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();
                    var ExistingStudentObj = ucamContext.Students.Find(StudentId);
                    if (ExistingStudentObj != null)
                    {
                        ExistingStudentObj.HallInfoId = Convert.ToInt32(ddlHallInfo.SelectedValue);

                        ucamContext.Entry(ExistingStudentObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();
                    }

                    ShowAlertMessage("Inofrmation Updated Successfully");
                }

            }
        }

        private void Clear()
        {
            txtRoll.Text = string.Empty;
            txtName.Text = string.Empty;
            txtreg.Text = string.Empty;
            txtdept.Text = string.Empty;

            txtprogram.Text = string.Empty;
            txtDOB.Text = string.Empty;
            ddlGender.SelectedValue = "0";
            ddlmaritalStatus.SelectedValue = "0";

            ddlBloodGroup.SelectedValue = "0";
            ddlReligion.SelectedValue = "0";
            ddlParentJob.SelectedValue = "0";
            ddlQuata.SelectedValue = "0";

            txtPhone.Text = string.Empty;
            txtSMS.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtNationality.Text = string.Empty;

            txtFather.Text = string.Empty;
            txtFatherphone.Text = string.Empty;
            txtMother.Text = string.Empty;
            txtMotherphone.Text = string.Empty;

            txtGuardianName.Text = string.Empty;
            txtGuardianPhone.Text = string.Empty;
            txtGuardianEmail.Text = string.Empty;

            txtParApp.Text = string.Empty;
            txtParHouse.Text = string.Empty;
            txtParRoad.Text = string.Empty;
            txtParPostcode.Text = string.Empty;

            txtParPost.Text = string.Empty;
            txtParPoliceStation.Text = string.Empty;
            txtParDistrict.Text = string.Empty;
            txtParCountry.Text = string.Empty;
            txtParArea.Text = string.Empty;

            txtPreApp.Text = string.Empty;
            txtPreHouse.Text = string.Empty;
            txtPreRoad.Text = string.Empty;
            txtPrePostcode.Text = string.Empty;

            txtPrePost.Text = string.Empty;
            txtPrePoliceStation.Text = string.Empty;
            txtPreDistrict.Text = string.Empty;
            txtPreCountry.Text = string.Empty;
            txtPreArea.Text = string.Empty;

            txtSSCMajor.Text = string.Empty;
            txtSSCInstitution.Text = string.Empty;
            txtSSCBoard.Text = string.Empty;
            txtSSCResult.Text = string.Empty;
            txtSSCDuration.Text = string.Empty;
            txtSSCYearOfPassing.Text = string.Empty;
            txtSSCSession.Text = string.Empty;

            txtHSCMajor.Text = string.Empty;
            txtHSCInstitution.Text = string.Empty;
            txtHSCBoard.Text = string.Empty;
            txtHSCResult.Text = string.Empty;
            txtHSCDuration.Text = string.Empty;
            txtHSCYearOfPassing.Text = string.Empty;
            txtHSCSession.Text = string.Empty;

            txtAcNo.Text = "";
            txtBankName.Text = "";
            txtBranch.Text = "";
            txtRoutingNo.Text = "";
            ddlHallInfo.SelectedValue = "0";

            //txtName.Text = "";
            //txtFatherName.Text = "";
            //txtMotherName.Text = "";
            //txtNationality.Text = "";
            //txtDob.Text = null;
            //ddlGender.SelectedIndex = 0;
            //ddlMaritalStat.SelectedIndex = 0;
            //ddlReligion.SelectedIndex = 0;
            //ddlBoardSecondary.SelectedIndex = 0;
            //ddlExamTypeGraduate.SelectedIndex = 0;
            //ddlExamTypeHigherSecondary.SelectedIndex = 0;
            //ddlExamTypeSecondary.SelectedIndex = 0;
            //ddlExamTypeUndergraduate.SelectedIndex = 0;
            //ddlResultTypeGraduate.SelectedIndex = 0;
            //ddlResultTypeHigherSecondary.SelectedIndex = 0;
            //ddlResultTypeSecondary.SelectedIndex = 0;
            //ddlResultTypeUndergraduate.SelectedIndex = 0;
            //txtGPAGraduate.Text = "0";
            //txtGPAHigherSecondary.Text = "0";
            //txtGPASecondary.Text = "0";
            //txtGPAUndergraduate.Text = "0";
            //txtGW4SHigherSecondary.Text = "0";
            //txtGW4SSecondary.Text = "0";
            //txtInstituteGraduate.Text = string.Empty;
            //txtInstituteHigherSecondary.Text = string.Empty;
            //txtInstituteSecondary.Text = string.Empty;
            //txtInstituteUndergraduate.Text = string.Empty;
            //txtPassingYearGraduate.Text = string.Empty;
            //txtPassingYearHigherSecondary.Text = string.Empty;
            //txtPassingYearSecondary.Text = string.Empty;
            //txtPassingYearUndergraduate.Text = string.Empty;
            //hdnGrad.Value = "0_0";
            //hdnHigherSecondary.Value = "0_0";
            //hdnSecondary.Value = "0_0";
            //hdnUnderGrad.Value = "0_0";
            //ViewState.Remove("StudentEditId");
            //hdnContact.Value = string.Empty;
            //txtStudentRoll.Text = string.Empty;
            //ddlSession.Items.Clear();
            //txtPhone.Text = string.Empty;
            //txtPhnGuardian.Text = string.Empty;
            //txtEmailPersonal.Text = string.Empty;
            //txtSMSContact.Text = string.Empty;
            //hdnMailing.Value = string.Empty;
            //txtMailingAddress.Text = string.Empty;
            LoadDropDowns();

        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {
                hfPersonID.Value = "0";
                hdnStudentId.Value = "0";
                hfPhotoPath.Value = "0";
                Clear();
                CheckBox1.Checked = false;
                btnPopUpSave.Text = "Update All";
                ////txtTeacherCode.Enabled = false;
                LinkButton linkButton = new LinkButton();
                linkButton = (LinkButton)sender;
                string id = Convert.ToString(linkButton.CommandArgument);
                //List<Student> studentList = SessionManager.GetListFromSession<Student>(_StudentList);
                Student student = StudentManager.GetById(Convert.ToInt32(id));// studentList.Find(x => x.StudentID == Convert.ToInt32(id));
                Person person = student.BasicInfo;
                if (student != null)
                {
                    int PersonId = student.PersonID;

                    hfPersonID.Value = student.PersonID.ToString();
                    hdnStudentId.Value = student.StudentID.ToString();

                    #region Basic Info

                    txtRoll.Text = student.Roll;
                    txtName.Text = student.BasicInfo.FullName;
                    txtreg.Text = student.StudentAdditionalInformation == null ? "" : student.StudentAdditionalInformation.RegistrationNo;

                    txtprogram.Text = student.Program.ShortName;
                    txtdept.Text = student.DepartmentName;

                    txtDOB.Text = student.BasicInfo.DOB == null ? "" : Convert.ToDateTime(student.BasicInfo.DOB).ToString("dd/MM/yyyy");
                    txtPhone.Text = student.BasicInfo.Phone == null ? "" : student.BasicInfo.Phone;
                    txtSMS.Text = student.BasicInfo.SMSContactSelf == null ? "" : student.BasicInfo.SMSContactSelf;
                    txtEmail.Text = student.BasicInfo.Email == null ? "" : student.BasicInfo.Email;
                    txtNationality.Text = student.BasicInfo.Nationality == null ? "" : student.BasicInfo.Nationality;
                    txtFather.Text = student.BasicInfo.FatherName == null ? "" : student.BasicInfo.FatherName;
                    txtMother.Text = student.BasicInfo.MotherName == null ? "" : student.BasicInfo.MotherName;
                    txtGuardianName.Text = student.BasicInfo.GuardianName == null ? "" : student.BasicInfo.GuardianName;
                    txtGuardianPhone.Text = student.BasicInfo.SMSContactGuardian == null ? "" : student.BasicInfo.SMSContactGuardian;
                    txtGuardianEmail.Text = student.BasicInfo.GuardianEmail == null ? "" : student.BasicInfo.GuardianEmail;

                    try
                    {
                        txtFatherphone.Text = student.StudentAdditionalInformation == null || student.StudentAdditionalInformation.FatherPhoneNumber == null ? "" : student.StudentAdditionalInformation.FatherPhoneNumber;
                        txtMotherphone.Text = student.StudentAdditionalInformation == null || student.StudentAdditionalInformation.MotherPhoneNumber == null ? "" : student.StudentAdditionalInformation.MotherPhoneNumber;

                    }
                    catch (Exception ex)
                    {
                    }

                    if (student.BasicInfo.Gender != null)
                    {
                        LoadGender();
                        if (student.BasicInfo.Gender == "-Select-")
                            ddlGender.SelectedValue = "0";
                        else
                            ddlGender.SelectedValue = student.BasicInfo.Gender.ToString();

                    }
                    if (student.BasicInfo.MatrialStatus != null)
                    {
                        if (student.BasicInfo.MatrialStatus == "-Select-")
                            ddlmaritalStatus.SelectedValue = "0";
                        else
                            ddlmaritalStatus.SelectedValue = student.BasicInfo.MatrialStatus.ToString();
                    }

                    if (student.BasicInfo.BloodGroup != null)
                    {
                        if (student.BasicInfo.BloodGroup == "-Select-")
                            ddlBloodGroup.SelectedValue = "0";
                        else
                            ddlBloodGroup.SelectedValue = student.BasicInfo.BloodGroup.ToString();
                    }

                    try
                    {
                        if (student.HallInfoId != null)
                            ddlHallInfo.SelectedValue = student.HallInfoId.ToString();
                        if (student.BasicInfo.ReligionId != 0)
                            ddlReligion.SelectedValue = student.BasicInfo.ReligionId.ToString();
                        if (student.StudentAdditionalInformation.QuataId != 0)
                            ddlQuata.SelectedValue = student.StudentAdditionalInformation.QuataId.ToString();
                        if (student.StudentAdditionalInformation.ParentsJobId != 0)
                            ddlParentJob.SelectedValue = student.StudentAdditionalInformation.ParentsJobId.ToString();

                    }
                    catch (Exception ex)
                    {
                    }


                    #endregion


                    #region Bank Info

                    DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();
                    var ExistingObj = ucamContext.PersonAdditionalInfoes.Where(x => x.PersonId == PersonId).FirstOrDefault();
                    if (ExistingObj != null)
                    {
                        txtAcNo.Text = ExistingObj.BankAccountNo == null ? "" : ExistingObj.BankAccountNo;
                        txtBankName.Text = ExistingObj.BankName == null ? "" : ExistingObj.BankName;
                        txtBranch.Text = ExistingObj.BankBranchName == null ? "" : ExistingObj.BankBranchName;
                        txtRoutingNo.Text = ExistingObj.BankRoutingNo == null ? "" : ExistingObj.BankRoutingNo;

                    }

                    #endregion

                    #region Address
                    List<Address> Add = AddressManager.GetAddressByPersonId(student.PersonID);
                    if (Add != null && Add.Count > 0)
                    {

                        //Parmanent Address
                        Address par = Add.Where(x => x.AddressTypeId == Convert.ToInt32(CommonEnum.AddressType.PermanentAddress)).FirstOrDefault();
                        if (par != null)
                        {
                            txtParApp.Text = par.AppartmentNo == null ? "" : par.AppartmentNo;
                            txtParRoad.Text = par.RoadNo == null ? "" : par.RoadNo;
                            txtParHouse.Text = par.HouseNo == null ? "" : par.HouseNo;
                            txtParPost.Text = par.PostOffice == null ? "" : par.PostOffice;
                            txtParPostcode.Text = par.PostCode == null ? "" : par.PostCode;
                            txtParPoliceStation.Text = par.PoliceStation == null ? "" : par.PoliceStation;
                            txtParDistrict.Text = par.District == null ? "" : par.District;
                            txtParCountry.Text = par.Country == null ? "" : par.Country;
                            txtParArea.Text = par.AreaInfo == null ? "" : par.AreaInfo;
                        }

                        //Present Address
                        Address pres = Add.Where(x => x.AddressTypeId == Convert.ToInt32(CommonEnum.AddressType.PresentAddress)).FirstOrDefault();
                        if (pres != null)
                        {
                            txtPreApp.Text = pres.AppartmentNo == null ? "" : pres.AppartmentNo;
                            txtPreRoad.Text = pres.RoadNo == null ? "" : pres.RoadNo;
                            txtPreHouse.Text = pres.HouseNo == null ? "" : pres.HouseNo;
                            txtPrePost.Text = pres.PostOffice == null ? "" : pres.PostOffice;
                            txtPrePostcode.Text = pres.PostCode == null ? "" : pres.PostCode;
                            txtPrePoliceStation.Text = pres.PoliceStation == null ? "" : pres.PoliceStation;
                            txtPreDistrict.Text = pres.District == null ? "" : pres.District;
                            txtPreCountry.Text = pres.Country == null ? "" : pres.Country;
                            txtPreArea.Text = pres.AreaInfo == null ? "" : pres.AreaInfo;
                        }
                    }


                    #endregion

                    #region Previous Result
                    List<PreviousEducation> result = PreviousEducationManager.GetAllByPersonId(student.PersonID);
                    if (result != null && result.Count > 0)
                    {
                        //SSC Result
                        PreviousEducation ssc = result.Where(x => x.EducationCategoryId == Convert.ToInt32(CommonEnum.EducationCategory.SSC)).FirstOrDefault();
                        if (ssc != null)
                        {
                            txtSSCMajor.Text = ssc.ConcentratedMajor == null ? "" : ssc.ConcentratedMajor;
                            txtSSCInstitution.Text = ssc.InstituteName == null ? "" : ssc.InstituteName;
                            txtSSCBoard.Text = ssc.Board == null ? "" : ssc.Board;
                            txtSSCResult.Text = ssc.Result == null ? "" : ssc.Result;
                            txtSSCDuration.Text = ssc.Duration == null ? "" : ssc.Duration;
                            txtSSCYearOfPassing.Text = ssc.PassingYear == null ? "" : ssc.PassingYear.ToString();
                            txtSSCSession.Text = ssc.Session == null ? "" : ssc.Session;
                        }

                        //HSC Result
                        PreviousEducation hsc = result.Where(x => x.EducationCategoryId == Convert.ToInt32(CommonEnum.EducationCategory.HSC)).FirstOrDefault();
                        if (hsc != null)
                        {
                            txtHSCMajor.Text = hsc.ConcentratedMajor == null ? "" : hsc.ConcentratedMajor;
                            txtHSCInstitution.Text = hsc.InstituteName == null ? "" : hsc.InstituteName;
                            txtHSCBoard.Text = hsc.Board == null ? "" : hsc.Board;
                            txtHSCResult.Text = hsc.Result == null ? "" : hsc.Result;
                            txtHSCDuration.Text = hsc.Duration == null ? "" : hsc.Duration;
                            txtHSCYearOfPassing.Text = hsc.PassingYear == null ? "" : hsc.PassingYear.ToString();
                            txtHSCSession.Text = hsc.Session == null ? "" : ssc.Session;
                        }
                    }
                    #endregion
                }

                if (person.PhotoPath != null)
                {
                    imgPhoto.ImageUrl = "~/Upload/Avatar/Student/" + person.PhotoPath + "?v=" + DateTime.Now; ;
                }
                else
                {
                    if (person.Gender != null)
                    {
                        if (person.Gender.ToLower() == "female")
                            imgPhoto.ImageUrl = "~/Images/photoGirl.png";
                        else
                            imgPhoto.ImageUrl = "~/Images/photoBoy.png";
                    }
                    else
                        imgPhoto.ImageUrl = "~/Images/photoBoy.png";
                }

                ModalPopupExtender1.Show();
            }
            catch { };


        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadGridView();
        }

        private void LoadSession(int programId)
        {
            Program program = ProgramManager.GetById(programId);

            List<AcademicCalender> sessionList = new List<AcademicCalender>();
            if (program != null)
                sessionList = AcademicCalenderManager.GetAll(program.CalenderUnitMasterID);

            //ddlSession.Items.Clear();
            //ddlSession.AppendDataBoundItems = true;

            //if (sessionList != null)
            //{
            //    ddlSession.Items.Add(new ListItem("-Select Session-", "0"));
            //    ddlSession.DataTextField = "FullCode";
            //    ddlSession.DataValueField = "AcademicCalenderID";

            //    ddlSession.DataSource = sessionList;
            //    ddlSession.DataBind();
            //}
        }
        protected void OnPopupProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            //int programId = Convert.ToInt32(ddlProgram.SelectedValue);
            //LoadSession(programId);
        }
        private string PhotoUploadProcess()
        {
            string photoName = "";
            try
            {
                int PersonId = Convert.ToInt32(hfPersonID.Value);
                if (this.FileUploadPhoto.HasFile)
                {
                    #region Check File Type

                    string fileName = FileUploadPhoto.FileName;
                    string fileExtension = Path.GetExtension(fileName);
                    fileExtension = fileExtension.ToLower();

                    string[] acceptedFileTypes = new string[3];
                    acceptedFileTypes[0] = ".jpg";
                    acceptedFileTypes[1] = ".jpeg";
                    acceptedFileTypes[2] = ".png";

                    bool acceptFile = false;
                    for (int i = 0; i < 3; i++)
                    {
                        if (fileExtension == acceptedFileTypes[i])
                        {
                            acceptFile = true;
                            break;
                        }
                    }
                    if (!acceptFile)
                    {
                        ShowAlertMessage("The file you are trying to upload is not a permitted file type!");
                        return "";
                    }

                    #endregion

                    #region Check File Size

                    int fileSize = FileUploadPhoto.PostedFile.ContentLength / 1024;
                    if (fileSize >= 200)
                    {
                        ShowAlertMessage("Filesize of image is too large. Maximum file size permitted is 200KB");
                        return "";
                    }
                    else
                    {
                        //lblMsg.Text = fileSize.ToString();
                    }

                    #endregion

                    string tempDirectoryPath = "~/Upload/TempAvatar/";
                    string fullDirectoryPath = "~/Upload/Avatar/Student/";

                    photoName = UploadImage(tempDirectoryPath, fullDirectoryPath, FileUploadPhoto, 150, 150);
                    if (photoName != "")
                    {

                        hfPhotoPath.Value = photoName + fileExtension;
                        imgPhoto.ImageUrl = fullDirectoryPath + photoName + fileExtension;
                        ShowAlertMessage("Photo Upload Complete");
                        if (PersonId.ToString() == photoName)
                        {
                            Person per = PersonManager.GetById(PersonId);
                            if (per != null)
                            {
                                per.PhotoPath = photoName + fileExtension;
                                PersonManager.Update(per);
                                imgPhoto.ImageUrl = "~/Upload/Avatar/Student/" + per.PhotoPath;
                            }
                        }
                        photoName += fileExtension;
                    }
                }
                else
                {
                    // ShowAlertMessage("Please select PHOTO first");
                }
            }
            catch { }
            finally { }

            return photoName;
        }

        private void LoadGridView()
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int currentSessionId = Convert.ToInt32(ucSession.selectedValue);
            int yearNo = Convert.ToInt32(ddlYearNo.SelectedValue);
            int semesterNo = Convert.ToInt32(ddlSemesterNo.SelectedValue);

            if (programId > 0)
            {
                List<Student> studentList = StudentManager.GetAllByProgramIdYearNoSemsterNoCurrentSessionId(programId, yearNo, semesterNo, currentSessionId);

                if (!string.IsNullOrEmpty(txtStudentId.Text))
                {
                    string Roll=txtStudentId.Text.Trim();
                    studentList = studentList.Where(x => x.Roll == Roll).ToList();
                }

                if (studentList != null && studentList.Any())
                {
                    DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

                    var HallList = ucamContext.HallInformations.ToList();

                    foreach (var item in studentList)
                    {
                        try
                        {
                            int HallId = item.HallInfoId == null ? 0 : Convert.ToInt32(item.HallInfoId);
                            var HallObj = HallList.Where(x => x.Id == HallId).FirstOrDefault();
                            if (HallObj != null)
                            {
                                item.Attribute3 = HallObj.HallCode;
                            }

                            var yearSemester = ucamContext.StudentYearSemesterHistories.Where(x => x.StudentId == item.StudentID && x.IsActive == true).FirstOrDefault();
                            if (yearSemester != null)
                            {
                                var HeldIn = ucamContext.ExamHeldInAndProgramRelations.Where(x => x.Id == yearSemester.HeldInProgramRelationId).FirstOrDefault();
                                if (HeldIn != null)
                                {
                                    item.Attribute1 = HeldIn.YearNo.ToString();
                                    item.Attribute2 = HeldIn.SemesterNo.ToString();
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }


                        try
                        {
                            if (item.BasicInfo.PhotoPath != null)
                                item.History = item.BasicInfo.PhotoPath.Length > 0 ? "~/Upload/Avatar/Student/" + item.BasicInfo.PhotoPath + "?v=" + DateTime.Now : item.Gender.ToLower() == "female" ? "~/Upload/Avatar/Female.jpg" : "~/Upload/Avatar/Male.jpg";
                            else
                                item.History = item.Gender != null ? item.Gender.ToLower() == "female" ? "~/Upload/Avatar/Female.jpg" : "~/Upload/Avatar/Male.jpg" : "";
                        }
                        catch (Exception ex)
                        {
                            item.Attribute3 = item.Gender.ToLower() == "female" ? "~/Upload/Avatar/Female.jpg" : "~/Upload/Avatar/Male.jpg";
                        }
                    }
                }

                gvStudentList.DataSource = studentList;
                gvStudentList.DataBind();

                //if (studentList != null && studentList.Count > 0)
                //{
                //    SessionManager.SaveListToSession<Student>(studentList, _StudentList);
                //}
            }
            else
            {
                lblMsg.Text = "Please select program.";
            }
        }
        protected void btnValidate_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            //if (!string.IsNullOrEmpty(txtStudentRoll.Text.Trim()) )
            //{
            //    string roll = txtStudentRoll.Text.Trim();
            //    Student studentObj = StudentManager.GetByRoll(roll);
            //    if (studentObj != null)
            //    {
            //        lblValidationStat.Text = "Unavailable";
            //    }
            //    else
            //    {
            //        lblValidationStat.Text = "Available";
            //    }
            //}
            //else
            //{
            //    lblValidationStat.Text = "Please input roll first!";
            //}
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Clear();
            ModalPopupExtender1.Show();
            btnPopUpSave.Text = "Save";
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                string phoneName = PhotoUploadProcess();
                ModalPopupExtender1.Show();
            }
            catch { }
            finally { }
        }

        private string UploadImage(string tempDirectoryPath, string fullDirectoryPath, FileUpload fileUploadPhoto, int y, int x)
        {
            string uniqueFileNameString = hfPersonID.Value;
            try
            {
                string orginalFileName = fileUploadPhoto.PostedFile.FileName;

                string tempfileName = uniqueFileNameString + orginalFileName;

                string tempPhotoPath = Path.Combine(tempDirectoryPath, tempfileName);
                if (!Directory.Exists(Server.MapPath(tempDirectoryPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(tempDirectoryPath));
                }
                if (File.Exists(tempPhotoPath))
                {
                    File.Delete(tempfileName);
                    //tempfileName = "1" + tempfileName;
                    tempPhotoPath = Path.Combine(tempDirectoryPath, tempfileName);
                }
                fileUploadPhoto.PostedFile.SaveAs(Server.MapPath(tempPhotoPath));

                string fileExtension = Path.GetExtension(tempPhotoPath).ToLower();

                string fileNameToSave = uniqueFileNameString + fileExtension;

                string fullPhotoPath = Path.Combine(fullDirectoryPath, fileNameToSave);
                if (!Directory.Exists(Server.MapPath(fullDirectoryPath)))
                {
                    Directory.CreateDirectory(Server.MapPath(fullDirectoryPath));
                    imgPhoto.ImageUrl = fullDirectoryPath;
                }
                if (File.Exists(Server.MapPath(fullPhotoPath)))
                {
                    System.IO.File.Delete(Server.MapPath(fullPhotoPath));
                }

                ImageUtility.ResizeImage(Server.MapPath(tempPhotoPath), Server.MapPath(fullPhotoPath), y, x);

                try
                {
                    System.IO.File.Delete(Server.MapPath(tempPhotoPath));

                }
                catch { }
            }
            catch { return ""; }
            finally { }

            return uniqueFileNameString;
        }

        private string Validation()
        {
            string errorString = string.Empty;
            List<string> errorList = new List<string>();

            //if (string.IsNullOrWhiteSpace(txtName.Text))
            //    errorList.Add("Name");

            //if (ddlProgram.SelectedValue == "0")
            //    errorList.Add("Program");

            //if (ddlSession.SelectedValue == "0")
            //    errorList.Add("Session");

            //if (string.IsNullOrWhiteSpace(txtSMSContact.Text))
            //    errorList.Add("SMS Contact");

            if (errorList != null && errorList.Count > 0)
            {
                foreach (var item in errorList)
                {
                    if (errorList.Count > 1)
                    {
                        errorString += ", " + item.ToString();
                    }
                    else
                    {
                        errorString += item.ToString();
                    }
                }

                errorString += " not filled.";
            }

            return errorString;
        }


        #region Add New Code For Information Update

        protected void btnSaveBasic_Click(object sender, EventArgs e)
        {
            int PersonId = Convert.ToInt32(hfPersonID.Value);
            int StudentId = Convert.ToInt32(hdnStudentId.Value);
            if (PersonId > 0 && StudentId > 0)
            {
                SavePersonBasicInfo(PersonId, StudentId);
                DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();
                var ExistingStudentObj = ucamContext.Students.Find(StudentId);
                if (ExistingStudentObj != null)
                {
                    ExistingStudentObj.HallInfoId = Convert.ToInt32(ddlHallInfo.SelectedValue);

                    ucamContext.Entry(ExistingStudentObj).State = EntityState.Modified;
                    ucamContext.SaveChanges();
                }
                ShowAlertMessage("Basic Information Updated Successfully");
                ModalPopupExtender1.Show();
            }
        }

        private void SavePersonBasicInfo(int PersonId, int StudentId)
        {
            Person perObj = PersonManager.GetById(PersonId);
            if (perObj != null)
            {
                //Only admin can change a Student Name
                if (BaseCurrentUserObj.RoleID == 1)
                    perObj.FullName = txtName.Text.Trim();

                if (!string.IsNullOrEmpty(txtDOB.Text))
                    perObj.DOB = DateTime.ParseExact(txtDOB.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                perObj.Gender = ddlGender.SelectedValue.ToString();
                perObj.MatrialStatus = ddlmaritalStatus.SelectedValue.ToString();
                perObj.BloodGroup = ddlBloodGroup.SelectedValue.ToString();
                perObj.ReligionId = Convert.ToInt32(ddlReligion.SelectedValue);
                if (!string.IsNullOrEmpty(txtPhone.Text))
                    perObj.Phone = txtPhone.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtSMS.Text))
                    perObj.SMSContactSelf = txtSMS.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtEmail.Text))
                    perObj.Email = txtEmail.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtNationality.Text))
                    perObj.Nationality = txtNationality.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtFather.Text))
                    perObj.FatherName = txtFather.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtMother.Text))
                    perObj.MotherName = txtMother.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtGuardianName.Text))
                    perObj.GuardianName = txtGuardianName.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtGuardianPhone.Text))
                    perObj.SMSContactGuardian = txtGuardianPhone.Text.Trim().ToString();
                if (!string.IsNullOrEmpty(txtGuardianEmail.Text))
                    perObj.GuardianEmail = txtGuardianEmail.Text.Trim().ToString();

                perObj.ModifiedBy = BaseCurrentUserObj.Id;
                perObj.ModifiedDate = DateTime.Now;

                PersonManager.Update(perObj);

                BankInformationInsertOrUpdate(PersonId);

                Student student = StudentManager.GetById(StudentId);
                if (student.StudentAdditionalInformation != null)
                {
                    StudentAdditionalInfo sai = student.StudentAdditionalInformation;
                    sai.QuataId = Convert.ToInt32(ddlQuata.SelectedValue);
                    sai.ParentsJobId = Convert.ToInt32(ddlParentJob.SelectedValue);
                    if (!string.IsNullOrEmpty(txtFatherphone.Text))
                        sai.FatherPhoneNumber = txtFatherphone.Text.Trim().ToString();
                    if (!string.IsNullOrEmpty(txtMotherphone.Text))
                        sai.MotherPhoneNumber = txtMotherphone.Text.Trim().ToString();

                    sai.ModifiedBy = BaseCurrentUserObj.Id;
                    sai.ModifiedDate = DateTime.Now;

                    StudentAdditionalInfoManager.Update(sai);
                }

            }
        }


        private void BankInformationInsertOrUpdate(int personId)
        {
            try
            {
                DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();
                var ExistingObj = ucamContext.PersonAdditionalInfoes.Where(x => x.PersonId == personId).FirstOrDefault();
                if (ExistingObj == null) // New Entry
                {
                    DAL.PersonAdditionalInfo NewObj = new DAL.PersonAdditionalInfo();

                    NewObj.PersonId = personId;
                    NewObj.BankAccountNo = string.IsNullOrEmpty(txtAcNo.Text) ? "" : txtAcNo.Text.Trim();
                    NewObj.BankName = string.IsNullOrEmpty(txtBankName.Text) ? "" : txtBankName.Text.Trim();
                    NewObj.BankBranchName = string.IsNullOrEmpty(txtBranch.Text) ? "" : txtBranch.Text.Trim();
                    NewObj.BankRoutingNo = string.IsNullOrEmpty(txtRoutingNo.Text) ? "" : txtRoutingNo.Text.Trim();

                    ucamContext.PersonAdditionalInfoes.Add(NewObj);
                    ucamContext.SaveChanges();

                }
                else // Update Existing Entry
                {
                    ExistingObj.BankAccountNo = string.IsNullOrEmpty(txtAcNo.Text) ? "" : txtAcNo.Text.Trim();
                    ExistingObj.BankName = string.IsNullOrEmpty(txtBankName.Text) ? "" : txtBankName.Text.Trim();
                    ExistingObj.BankBranchName = string.IsNullOrEmpty(txtBranch.Text) ? "" : txtBranch.Text.Trim();
                    ExistingObj.BankRoutingNo = string.IsNullOrEmpty(txtRoutingNo.Text) ? "" : txtRoutingNo.Text.Trim();

                    ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                    ucamContext.SaveChanges();

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnParAdd_Click(object sender, EventArgs e)
        {
            int PersonId = Convert.ToInt32(hfPersonID.Value);
            if (PersonId > 0)
            {
                SaveParmanentAddress(PersonId);
                ShowAlertMessage("Permanent Address Updated Successfully");
                ModalPopupExtender1.Show();
            }
        }

        private void SaveParmanentAddress(int PersonId)
        {
            Address parAdd = AddressManager.GetAddressByPersonId(PersonId).Where(x => x.AddressTypeId == Convert.ToInt32(CommonEnum.AddressType.PermanentAddress)).FirstOrDefault();
            if (parAdd != null)
            {
                parAdd.AppartmentNo = txtParApp.Text.Trim();
                parAdd.HouseNo = txtParHouse.Text.Trim();
                parAdd.RoadNo = txtParRoad.Text.Trim();
                parAdd.AreaInfo = txtParArea.Text.Trim();
                parAdd.PostOffice = txtParPost.Text.Trim();
                parAdd.PoliceStation = txtParPoliceStation.Text.Trim();
                parAdd.Country = txtParCountry.Text.Trim();
                parAdd.PostCode = txtParPostcode.Text.Trim();
                parAdd.District = txtParDistrict.Text.Trim();

                parAdd.ModifiedBy = BaseCurrentUserObj.Id;
                parAdd.ModifiedDate = DateTime.Now;

                AddressManager.Update(parAdd);
            }
            else
            {
                Address newObj = new Address();

                newObj.PersonId = PersonId;
                newObj.AddressTypeId = Convert.ToInt32(CommonEnum.AddressType.PermanentAddress);
                newObj.AppartmentNo = txtParApp.Text.Trim();
                newObj.HouseNo = txtParHouse.Text.Trim();
                newObj.RoadNo = txtParRoad.Text.Trim();
                newObj.AreaInfo = txtParArea.Text.Trim();
                newObj.PostOffice = txtParPost.Text.Trim();
                newObj.PoliceStation = txtParPoliceStation.Text.Trim();
                newObj.Country = txtParCountry.Text.Trim();
                newObj.PostCode = txtParPostcode.Text.Trim();
                newObj.District = txtParDistrict.Text.Trim();

                newObj.CreatedBy = BaseCurrentUserObj.Id;
                newObj.CreatedDate = DateTime.Now;

                AddressManager.Insert(newObj);
            }
        }

        protected void btnPreAdd_Click(object sender, EventArgs e)
        {
            int PersonId = Convert.ToInt32(hfPersonID.Value);
            if (PersonId > 0)
            {
                SavePresentAddress(PersonId);
                ShowAlertMessage("Present Address Updated Successfully");
                ModalPopupExtender1.Show();
            }
        }

        private void SavePresentAddress(int PersonId)
        {
            Address preAdd = AddressManager.GetAddressByPersonId(PersonId).Where(x => x.AddressTypeId == Convert.ToInt32(CommonEnum.AddressType.PresentAddress)).FirstOrDefault();
            if (preAdd != null)
            {
                preAdd.AppartmentNo = txtPreApp.Text.Trim();
                preAdd.HouseNo = txtPreHouse.Text.Trim();
                preAdd.RoadNo = txtPreRoad.Text.Trim();
                preAdd.AreaInfo = txtPreArea.Text.Trim();
                preAdd.PostOffice = txtPrePost.Text.Trim();
                preAdd.PoliceStation = txtPrePoliceStation.Text.Trim();
                preAdd.Country = txtPreCountry.Text.Trim();
                preAdd.PostCode = txtPrePostcode.Text.Trim();
                preAdd.District = txtPreDistrict.Text.Trim();

                preAdd.ModifiedBy = BaseCurrentUserObj.Id;
                preAdd.ModifiedDate = DateTime.Now;

                AddressManager.Update(preAdd);
            }
            else
            {
                Address newObj = new Address();

                newObj.PersonId = PersonId;
                newObj.AddressTypeId = Convert.ToInt32(CommonEnum.AddressType.PresentAddress);
                newObj.AppartmentNo = txtPreApp.Text.Trim();
                newObj.HouseNo = txtPreHouse.Text.Trim();
                newObj.RoadNo = txtPreRoad.Text.Trim();
                newObj.AreaInfo = txtPreArea.Text.Trim();
                newObj.PostOffice = txtPrePost.Text.Trim();
                newObj.PoliceStation = txtPrePoliceStation.Text.Trim();
                newObj.Country = txtPreCountry.Text.Trim();
                newObj.PostCode = txtPrePostcode.Text.Trim();
                newObj.District = txtPreDistrict.Text.Trim();

                newObj.CreatedBy = BaseCurrentUserObj.Id;
                newObj.CreatedDate = DateTime.Now;

                AddressManager.Insert(newObj);
            }
        }

        protected void btnSSCAdd_Click(object sender, EventArgs e)
        {
            int PersonId = Convert.ToInt32(hfPersonID.Value);
            if (PersonId > 0)
            {
                SaveSSCResult(PersonId);
                ShowAlertMessage("SSC Result Updated Successfully");
                ModalPopupExtender1.Show();
            }
        }

        private void SaveSSCResult(int PersonId)
        {
            PreviousEducation ssc = PreviousEducationManager.GetAllByPersonId(PersonId)
                .Where(x => x.EducationCategoryId == Convert.ToInt32(CommonEnum.EducationCategory.SSC)).FirstOrDefault();
            if (ssc != null)
            {
                ssc.ConcentratedMajor = txtSSCMajor.Text.Trim();
                ssc.InstituteName = txtSSCInstitution.Text.Trim();
                ssc.Board = txtSSCBoard.Text.Trim();
                ssc.Result = txtSSCResult.Text.Trim();
                ssc.Duration = txtSSCDuration.Text.Trim();
                if (!string.IsNullOrEmpty(txtSSCYearOfPassing.Text))
                    ssc.PassingYear = Convert.ToInt32(txtSSCYearOfPassing.Text.Trim());
                ssc.Session = txtSSCSession.Text.Trim();


                ssc.ModifiedBy = BaseCurrentUserObj.Id;
                ssc.ModifiedDate = DateTime.Now;

                PreviousEducationManager.Update(ssc);
            }
            else
            {
                PreviousEducation newObj = new PreviousEducation();

                newObj.PersonId = PersonId;
                newObj.EducationTypeId = 1;
                newObj.EducationCategoryId = Convert.ToInt32(CommonEnum.EducationCategory.SSC);
                newObj.ConcentratedMajor = txtSSCMajor.Text.Trim();
                newObj.InstituteName = txtSSCInstitution.Text.Trim();
                newObj.Board = txtSSCBoard.Text.Trim();
                newObj.Result = txtSSCResult.Text.Trim();
                newObj.Duration = txtSSCDuration.Text.Trim();
                if (!string.IsNullOrEmpty(txtSSCYearOfPassing.Text))
                    newObj.PassingYear = Convert.ToInt32(txtSSCYearOfPassing.Text.Trim());
                newObj.Session = txtSSCSession.Text.Trim();

                newObj.CreatedBy = BaseCurrentUserObj.Id;
                newObj.CreatedDate = DateTime.Now;

                PreviousEducationManager.Insert(newObj);
            }
        }

        protected void btnHSCAdd_Click(object sender, EventArgs e)
        {
            int PersonId = Convert.ToInt32(hfPersonID.Value);
            if (PersonId > 0)
            {
                SaveHSCResult(PersonId);
                ShowAlertMessage("HSC Result Updated Successfully");
                ModalPopupExtender1.Show();
            }
        }

        private void SaveHSCResult(int PersonId)
        {
            PreviousEducation hsc = PreviousEducationManager.GetAllByPersonId(PersonId)
                .Where(x => x.EducationCategoryId == Convert.ToInt32(CommonEnum.EducationCategory.HSC)).FirstOrDefault();
            if (hsc != null)
            {
                hsc.ConcentratedMajor = txtHSCMajor.Text.Trim();
                hsc.InstituteName = txtHSCInstitution.Text.Trim();
                hsc.Board = txtHSCBoard.Text.Trim();
                hsc.Result = txtHSCResult.Text.Trim();
                hsc.Duration = txtHSCDuration.Text.Trim();
                if (!string.IsNullOrEmpty(txtHSCYearOfPassing.Text))
                    hsc.PassingYear = Convert.ToInt32(txtHSCYearOfPassing.Text.Trim());
                hsc.Session = txtHSCSession.Text.Trim();


                hsc.ModifiedBy = BaseCurrentUserObj.Id;
                hsc.ModifiedDate = DateTime.Now;

                PreviousEducationManager.Update(hsc);
            }
            else
            {
                PreviousEducation newObj = new PreviousEducation();

                newObj.PersonId = PersonId;
                newObj.EducationTypeId = 1;
                newObj.EducationCategoryId = Convert.ToInt32(CommonEnum.EducationCategory.HSC);
                newObj.ConcentratedMajor = txtHSCMajor.Text.Trim();
                newObj.InstituteName = txtHSCInstitution.Text.Trim();
                newObj.Board = txtHSCBoard.Text.Trim();
                newObj.Result = txtHSCResult.Text.Trim();
                newObj.Duration = txtHSCDuration.Text.Trim();
                if (!string.IsNullOrEmpty(txtHSCYearOfPassing.Text))
                    newObj.PassingYear = Convert.ToInt32(txtHSCYearOfPassing.Text.Trim());
                newObj.Session = txtHSCSession.Text.Trim();

                newObj.CreatedBy = BaseCurrentUserObj.Id;
                newObj.CreatedDate = DateTime.Now;

                PreviousEducationManager.Insert(newObj);
            }
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (CheckBox1.Checked)
            {
                txtPreApp.Text = txtParApp.Text;
                txtPreHouse.Text = txtParHouse.Text;
                txtPreRoad.Text = txtParRoad.Text;
                txtPrePostcode.Text = txtParPostcode.Text;

                txtPrePost.Text = txtParPost.Text;
                txtPrePoliceStation.Text = txtParPoliceStation.Text;
                txtPreDistrict.Text = txtParDistrict.Text;
                txtPreCountry.Text = txtParCountry.Text;
                txtPreArea.Text = txtParArea.Text;
            }
            ModalPopupExtender1.Show();
        }
        #endregion




    }
}