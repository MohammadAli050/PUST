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

namespace EMS.Module.student
{
    public partial class StudentManagement : BasePage
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
                txtGPAGraduate.Text = "0";
                txtGPAHigherSecondary.Text = "0";
                txtGPASecondary.Text = "0";
                txtGPAUndergraduate.Text = "0";
                txtGW4SHigherSecondary.Text = "0";
                txtGW4SSecondary.Text = "0";
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
                //LoadAdmissionSessionDropDownList();
                //int programId = Convert.ToInt32(ucProgram.selectedValue);
                LoadYearNoDDL();
                LoadSemesterNoDDL();
                LoadDropDowns();
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
                ddlYearNo.DataTextField = "YearNo";
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
                ddlSemesterNo.DataTextField = "SemesterNo";
                ddlSemesterNo.DataValueField = "SemesterNo";

                ddlSemesterNo.DataSource = semesterList;
                ddlSemesterNo.DataBind();

            }
        }

        protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
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
            ucSession.LoadDropDownList(Convert.ToInt32(ucProgram.selectedValue));
        }
        
        //public void LoadAdmissionSessionDropDownList()
        //{
        //    try
        //    {
        //        int programId = Convert.ToInt32(ucProgram.selectedValue);
        //        var academicCalenderList = AcademicCalenderManager.GetAll().Where(x => x.CalenderUnitTypeID == 1).OrderByDescending(m => m.AcademicCalenderID).ToList();
        //        admissionSessionDropDownList.Items.Clear();
        //        admissionSessionDropDownList.Items.Add(new ListItem("Select", "-1"));
        //        admissionSessionDropDownList.AppendDataBoundItems = true;
        //        if (programId == -1)
        //        {
        //            lblMsg.Text = "select program.";
        //            return;
        //        }

        //        if (academicCalenderList.Any())
        //        {
        //            admissionSessionDropDownList.DataTextField = "FullCode";
        //            admissionSessionDropDownList.DataValueField = "AcademicCalenderID";
        //            admissionSessionDropDownList.DataSource = academicCalenderList;
        //            admissionSessionDropDownList.DataBind();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }


        //}

        private void LoadDropDowns()
        {
            ddlGender.Items.Clear();
            ddlGender.Items.Add(new ListItem("Select", "0"));
            ddlGender.Items.Add(new ListItem("Male", "Male"));
            ddlGender.Items.Add(new ListItem("Female", "Female"));

            ddlReligion.Items.Clear();
            ddlReligion.Items.Add(new ListItem("Select", "0"));
            ddlReligion.Items.Add(new ListItem("Islam", "1"));
            ddlReligion.Items.Add(new ListItem("Hindu", "2"));
            ddlReligion.Items.Add(new ListItem("Buddha", "3"));
            ddlReligion.Items.Add(new ListItem("Christian", "4"));

            ddlMaritalStat.Items.Clear();
            ddlMaritalStat.Items.Add(new ListItem("Select", "0"));
            ddlMaritalStat.Items.Add(new ListItem("Married", "Married"));
            ddlMaritalStat.Items.Add(new ListItem("Unmarried", "Unmarried"));

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

            ddlProgram.Items.Clear();
            ddlProgram.Items.Add(new ListItem("-Select-", "0"));
            ddlProgram.AppendDataBoundItems = true;
            List<Program> programList = ProgramManager.GetAll();
            if (programList != null)
            {
                ddlProgram.DataValueField = "ProgramID";
                ddlProgram.DataTextField = "ShortName";
                ddlProgram.DataSource = programList;
                ddlProgram.DataBind();
            }
            #region Board
            ddlBoardSecondary.Items.Clear();
            ddlBoardSecondary.Items.Add(new ListItem("-Select-", "0"));
            ddlBoardHigherSecondary.Items.Clear();
            ddlBoardHigherSecondary.Items.Add(new ListItem("-Select-", "0"));
            String[] names = Enum.GetNames(typeof(CommonEnum.EducationBoard));
            int[] values = (int[])Enum.GetValues(typeof(CommonEnum.EducationBoard));

            for (int i = 0; i < names.Length; i++)
            {
                ddlBoardSecondary.Items.Add(new ListItem(names[i], values[i].ToString()));
                ddlBoardHigherSecondary.Items.Add(new ListItem(names[i], values[i].ToString()));
            }
            #endregion

            #region ExamType Secondary
            ddlExamTypeSecondary.Items.Clear();
            ddlExamTypeSecondary.Items.Add(new ListItem("-Select-", "0"));
            ddlExamTypeSecondary.AppendDataBoundItems = true;
            ddlExamTypeSecondary.DataTextField = "Code";
            ddlExamTypeSecondary.DataValueField = "PreviousExamTypeId";

            if (examTypeList != null)
            {
                collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Secondary).ToList();

                ddlExamTypeSecondary.DataSource = collection;
                ddlExamTypeSecondary.DataBind();
            }
            #endregion

            #region ExamType Higher Secondary
            ddlExamTypeHigherSecondary.Items.Clear();
            ddlExamTypeHigherSecondary.Items.Add(new ListItem("-Select-", "0"));
            ddlExamTypeHigherSecondary.AppendDataBoundItems = true;
            ddlExamTypeHigherSecondary.DataTextField = "Code";
            ddlExamTypeHigherSecondary.DataValueField = "PreviousExamTypeId";

            if (examTypeList != null)
            {
                collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Higher_Secondary).ToList();

                ddlExamTypeHigherSecondary.DataSource = collection;
                ddlExamTypeHigherSecondary.DataBind();
            }
            #endregion

            #region ExamType Undergraduate
            ddlExamTypeUndergraduate.Items.Clear();
            ddlExamTypeUndergraduate.Items.Add(new ListItem("-Select-", "0"));
            ddlExamTypeUndergraduate.AppendDataBoundItems = true;
            ddlExamTypeUndergraduate.DataTextField = "Code";
            ddlExamTypeUndergraduate.DataValueField = "PreviousExamTypeId";

            if (examTypeList != null)
            {
                collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Undergraduate).ToList();

                ddlExamTypeUndergraduate.DataSource = collection;
                ddlExamTypeUndergraduate.DataBind();
            }
            #endregion

            #region ExamType Graduate
            ddlExamTypeGraduate.Items.Clear();
            ddlExamTypeGraduate.Items.Add(new ListItem("-Select-", "0"));
            ddlExamTypeGraduate.AppendDataBoundItems = true;
            ddlExamTypeGraduate.DataTextField = "Code";
            ddlExamTypeGraduate.DataValueField = "PreviousExamTypeId";

            if (examTypeList != null)
            {
                collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Graduate).ToList();

                ddlExamTypeGraduate.DataSource = collection;
                ddlExamTypeGraduate.DataBind();
            }
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
                if (btnPopUpSave.Text.Equals("Update"))
                {
                    if (txtStudentRoll.Text.Trim() == hfStudentRollChanged.Value)
                    {
                        SaveOrEdit("Edit");
                    }
                }
                else if (btnPopUpSave.Text.Equals("Save"))
                {
                    if (lblValidationStat.Text.Equals("Available"))
                        SaveOrEdit("Save");
                    else
                        lblValidationStat.Text = "Plz check roll first!";
                }
            }            
        }

        private void SaveOrEdit(string mode)
        {
            try
            {
                BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    //int a = Convert.ToInt32(ViewState["StudentEditId"]);
                    List<Student> studentList = SessionManager.GetListFromSession<Student>(_StudentList);
                    Student student = studentList != null ? studentList.Find(x => x.StudentID == Convert.ToInt32(ViewState["StudentEditId"])) : null;
                    Person person = student == null ? new Person() : student.BasicInfo;
                    person.FullName = txtName.Text;
                    person.FatherName = txtFatherName.Text;
                    person.MotherName = txtMotherName.Text;
                    person.Nationality = txtNationality.Text;
                    person.SMSContactSelf = txtSMSContact.Text;
                    person.Phone = txtPhone.Text;
                    person.SMSContactGuardian = txtPhnGuardian.Text;
                    person.Email = txtEmailPersonal.Text;
                    person.DOB = (!txtDob.Text.Trim().Equals("")) ? DateTime.ParseExact(txtDob.Text.Replace("-","/"), "d/M/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    if (!ddlReligion.SelectedValue.Equals("0"))
                        person.ReligionId = Convert.ToInt32(ddlReligion.SelectedItem.Value);
                    if (!ddlGender.SelectedValue.Equals("0"))
                        person.Gender = ddlGender.SelectedValue;
                    if (!ddlMaritalStat.SelectedValue.Equals("0"))
                        person.MatrialStatus = ddlMaritalStat.SelectedValue;

                    if (PhotoUploadProcess() != "")
                        person.PhotoPath = PhotoUploadProcess();
                    else
                    {
                        if (hfPhotoPath.Value != "")
                            person.PhotoPath = hfPhotoPath.Value;

                    }
                    try
                    {
                        Value valueObj = ValueManager.GetAll().Where(x => x.ValueName == "Student").FirstOrDefault();
                        if (valueObj != null)
                            person.TypeId = valueObj.ValueID;
                    }
                    catch { }

                    if (student == null)
                        student = new Student();
                    if (mode.Equals("Save"))
                    {
                        if (string.IsNullOrEmpty(txtStudentRoll.Text))
                        {
                            lblMsg.Text = "Please input student roll and try again.";
                            return;
                        }
                        LogicLayer.BusinessObjects.Batch batchObj = new Batch();
                        if (ddlProgram.SelectedValue != "0" && ddlSession.SelectedValue != "0") //&& ddlCampus.SelectedValue != "0"
                            batchObj = BatchManager.GetAllByProgram(Convert.ToInt32(ddlProgram.SelectedValue)).Where(d => d.AcaCalId == Convert.ToInt32(ddlSession.SelectedValue)).FirstOrDefault();
                        else
                        {
                            lblMsg.Text = "Please select Program, Campus and Session";
                            return;
                        }
                        person.CreatedBy = BaseCurrentUserObj.Id;
                        person.CreatedDate = DateTime.Now;
                        int personID = PersonManager.Insert(person);
                        student.PersonID = personID;
                        student.Roll = txtStudentRoll.Text.Trim();
                        
                        //student.CampusId = Convert.ToInt32(ddlCampus.SelectedValue);
                        student.ProgramID = Convert.ToInt32(ddlProgram.SelectedValue);
                        student.StudentAdmissionAcaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                        if(batchObj!= null)
                            student.BatchId = batchObj.BatchId;
                        student.IsActive = true;
                        student.GradeMasterId = 2;
                        //teacher.EmployeeTypeId = Convert.ToInt32(ddlEmployeeType.SelectedValue);
                        student.CreatedBy = BaseCurrentUserObj.Id;
                        student.CreatedDate = DateTime.Now;
                        int studentID = StudentManager.Insert(student);
                        
                        ExamInsertOrUpdate(personID);

                        ContactAndAddressInsertOrUpdate(personID);
                    }
                    else
                    {
                        person.PersonID = student.BasicInfo.PersonID;
                        person.ModifiedBy = BaseCurrentUserObj.CreatorID;
                        person.ModifiedDate = DateTime.Now;
                        PersonManager.Update(person);
                        //student = person.Student;

                        student.ModifiedBy = BaseCurrentUserObj.Id;
                        student.ModifiedDate = DateTime.Now;
                        LogicLayer.BusinessObjects.Batch batchObj = new Batch();
                        if (ddlProgram.SelectedValue != "0" && ddlSession.SelectedValue != "0")//&& ddlCampus.SelectedValue != "0"
                            batchObj = BatchManager.GetAllByProgramIdAcacalId(Convert.ToInt32(ddlProgram.SelectedValue), Convert.ToInt32(ddlSession.SelectedValue)).FirstOrDefault();
                        if (batchObj != null)
                            student.BatchId = batchObj.BatchId;
                        //student.CampusId = Convert.ToInt32(ddlCampus.SelectedValue);
                        student.ProgramID = Convert.ToInt32(ddlProgram.SelectedValue);
                        student.StudentAdmissionAcaCalId = Convert.ToInt32(ddlSession.SelectedValue);
                        //teacher.EmployeeTypeId = Convert.ToInt32(ddlEmployeeType.SelectedValue);
                        //StudentManager.Update(student);
                        ExamInsertOrUpdate(person.PersonID);
                        ContactAndAddressInsertOrUpdate(person.PersonID);
                    }

                    LoadGridView();
                    Clear();
                    ModalPopupExtender1.Hide();
                    ShowAlertMessage("Succesfull!");

                }
                else
                {
                    ShowAlertMessage("Plz fill all the fields properly!");
                }

            }
            catch
            {
                ShowAlertMessage("Something went wrong! Plz try again!");
            }
        }

        private void ContactAndAddressInsertOrUpdate(int personID)
        {
            if (!string.IsNullOrEmpty(hdnMailing.Value))
            {
                Address address = AddressManager.GetById(Convert.ToInt32(hdnMailing.Value));
                address.AddressLine = txtMailingAddress.Text;
                address.ModifiedBy = BaseCurrentUserObj.Id;
                address.ModifiedDate = DateTime.Now;
                //AddressManager.Update(address);
            }
            else
            {
                Address address = new Address();
                address.AddressLine = txtMailingAddress.Text;
                address.CreatedBy = BaseCurrentUserObj.Id;
                address.CreatedDate = DateTime.Now;
                address.ModifiedBy = BaseCurrentUserObj.Id;
                address.ModifiedDate = DateTime.Now;
                address.PersonId = personID;
                address.AddressTypeId = 4;
                //AddressManager.Insert(address);
            }

            //if (!string.IsNullOrEmpty(hdnContact.Value))
            //{
            //    ContactDetails contact = ContactDetailsManager.GetContactDetailsByPersonID(Convert.ToInt32(hdnContact.Value));
            //    contact.Mobile1 = txtMobile1.Text;
            //    contact.PhoneResidential = txtPhnRes.Text;
            //    contact.EmailPersonal = txtEmailPersonal.Text;
            //    ContactDetailsManager.Update(contact);
            //}
            //else
            //{
            //    ContactDetails contact = new ContactDetails();
            //    contact.PersonID = personID;
            //    contact.Mobile1 = txtMobile1.Text;
            //    contact.PhoneResidential = txtPhnRes.Text;
            //    contact.EmailPersonal = txtEmailPersonal.Text;
            //    ContactDetailsManager.Insert(contact);
            //}
        }

        private void ExamInsertOrUpdate(int personID)
        {
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            #region Exam
            //================Exam Insert================//
            #region Education Info
            PersonPreviousExam candidateExam = null;
            bool sucess;
            decimal value;

            #region Secondary

            if (Convert.ToInt32(ddlExamTypeSecondary.SelectedValue) > 0)
            {
                string[] examIds = hdnSecondary.Value.Split('_');

                PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));
                exam.Board = Convert.ToInt32(ddlBoardSecondary.SelectedValue);
                exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPASecondary.Text) ? "0" : txtGPASecondary.Text);
                exam.GPAW4S = Convert.ToDecimal(string.IsNullOrEmpty(txtGW4SSecondary.Text) ? "0" : txtGW4SSecondary.Text);
                exam.InstituteName = txtInstituteSecondary.Text;
                exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearSecondary.Text) ? "0" : txtPassingYearSecondary.Text);
                exam.Result = ddlResultTypeSecondary.SelectedItem.Text;
                exam.ResultId = Convert.ToInt32(ddlResultTypeSecondary.SelectedValue);
                int secondaryExamId = 0;
                if (examIds[1].Equals("0"))
                {
                    exam.CreatedBy = BaseCurrentUserObj.Id;
                    exam.CreatedOn = DateTime.Now;
                    secondaryExamId = PreviousExamManager.Insert(exam);
                }
                else
                {
                    exam.ModifiedBy = BaseCurrentUserObj.Id;
                    exam.ModifiedOn = DateTime.Now;
                    PreviousExamManager.Update(exam);
                }

                candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
                if (examIds[0].Equals("0"))
                {
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeSecondary.SelectedValue);
                    candidateExam.CreatedBy = BaseCurrentUserObj.Id;
                    candidateExam.CreatedOn = DateTime.Now;
                    candidateExam.PersonId = personID;
                    candidateExam.PreviousExamId = secondaryExamId;
                    PersonPreviousExamManager.Insert(candidateExam);
                }
                else
                {
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeSecondary.SelectedValue);
                    PersonPreviousExamManager.Update(candidateExam);
                }
            }
            #endregion

            #region Higher Secondary
            if (Convert.ToInt32(ddlExamTypeHigherSecondary.SelectedValue) > 0)
            {
                string[] examIds = hdnHigherSecondary.Value.Split('_');

                PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));
                exam.Board = Convert.ToInt32(ddlBoardHigherSecondary.SelectedValue);
                exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPAHigherSecondary.Text) ? "0" : txtGPAHigherSecondary.Text);
                exam.GPAW4S = Convert.ToDecimal(string.IsNullOrEmpty(txtGW4SHigherSecondary.Text) ? "0" : txtGW4SHigherSecondary.Text);
                exam.InstituteName = txtInstituteHigherSecondary.Text;
                exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearHigherSecondary.Text) ? "0" : txtPassingYearHigherSecondary.Text);
                exam.CreatedBy = BaseCurrentUserObj.Id;
                exam.CreatedOn = DateTime.Now;
                exam.Result = ddlResultTypeHigherSecondary.SelectedItem.Text;
                exam.ResultId = Convert.ToInt32(ddlResultTypeHigherSecondary.SelectedValue);
                int hsecondaryExamId = 0;

                if (examIds[1].Equals("0"))
                {
                    exam.CreatedBy = BaseCurrentUserObj.Id;
                    exam.CreatedOn = DateTime.Now;
                    hsecondaryExamId = PreviousExamManager.Insert(exam);
                }
                else
                {
                    exam.ModifiedBy = BaseCurrentUserObj.Id;
                    exam.ModifiedOn = DateTime.Now;
                    PreviousExamManager.Update(exam);
                }

                candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
                if (examIds[0].Equals("0"))
                {
                    candidateExam.PersonId = personID;
                    candidateExam.PreviousExamId = hsecondaryExamId;
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeHigherSecondary.SelectedValue);
                    candidateExam.CreatedBy = BaseCurrentUserObj.Id;
                    candidateExam.CreatedOn = DateTime.Now;
                    PersonPreviousExamManager.Insert(candidateExam);
                }
                else
                {
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeHigherSecondary.SelectedValue);
                    PersonPreviousExamManager.Update(candidateExam);
                }
            }
            #endregion

            #region Undergraduate
            if (Convert.ToInt32(ddlExamTypeUndergraduate.SelectedValue) > 0)
            {
                string[] examIds = hdnUnderGrad.Value.Split('_');

                PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));
                exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPAUndergraduate.Text) ? "0" : txtGPAUndergraduate.Text);
                exam.InstituteName = txtInstituteUndergraduate.Text;
                exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearUndergraduate.Text) ? "0" : txtPassingYearUndergraduate.Text);
                exam.CreatedBy = BaseCurrentUserObj.Id;
                exam.CreatedOn = DateTime.Now;
                exam.Result = ddlResultTypeUndergraduate.SelectedItem.Text;
                exam.ResultId = Convert.ToInt32(ddlResultTypeUndergraduate.SelectedValue);
                int uExamId = 0;

                if (examIds[1].Equals("0"))
                {
                    exam.CreatedBy = BaseCurrentUserObj.Id;
                    exam.CreatedOn = DateTime.Now;
                    uExamId = PreviousExamManager.Insert(exam);
                }
                else
                {
                    exam.ModifiedBy = BaseCurrentUserObj.Id;
                    exam.ModifiedOn = DateTime.Now;
                    PreviousExamManager.Update(exam);
                }

                candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
                if (examIds[0].Equals("0"))
                {
                    candidateExam = new PersonPreviousExam();
                    candidateExam.PersonId = personID;
                    candidateExam.PreviousExamId = uExamId;
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeUndergraduate.SelectedValue);
                    candidateExam.CreatedBy = BaseCurrentUserObj.Id;
                    candidateExam.CreatedOn = DateTime.Now;
                    PersonPreviousExamManager.Insert(candidateExam);
                }
                else
                {
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeUndergraduate.SelectedValue);
                    PersonPreviousExamManager.Update(candidateExam);
                }
            }
            #endregion

            #region Graduate
            if (Convert.ToInt32(ddlExamTypeGraduate.SelectedValue) > 0)
            {
                string[] examIds = hdnGrad.Value.Split('_');

                PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));

                exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPAGraduate.Text) ? "0" : txtGPAGraduate.Text);

                exam.InstituteName = txtInstituteGraduate.Text;
                exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearGraduate.Text) ? "0" : txtPassingYearGraduate.Text);
                exam.CreatedBy = BaseCurrentUserObj.Id;
                exam.CreatedOn = DateTime.Now;
                exam.Result = ddlResultTypeGraduate.SelectedItem.Text;
                exam.ResultId = Convert.ToInt32(ddlResultTypeGraduate.SelectedValue);
                int gExamId = 0;

                if (examIds[1].Equals("0"))
                {
                    exam.CreatedBy = BaseCurrentUserObj.Id;
                    exam.CreatedOn = DateTime.Now;
                    gExamId = PreviousExamManager.Insert(exam);
                }
                else
                {
                    exam.ModifiedBy = BaseCurrentUserObj.Id;
                    exam.ModifiedOn = DateTime.Now;
                    PreviousExamManager.Update(exam);
                }

                candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
                if (examIds[0].Equals("0"))
                {
                    candidateExam.PersonId = personID;
                    candidateExam.PreviousExamId = gExamId;
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeGraduate.SelectedValue);
                    candidateExam.CreatedBy = BaseCurrentUserObj.Id;
                    candidateExam.CreatedOn = DateTime.Now;
                    PersonPreviousExamManager.Insert(candidateExam);
                }
                else
                {
                    candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeGraduate.SelectedValue);
                    PersonPreviousExamManager.Update(candidateExam);
                }
            }
            #endregion

            #endregion
            //================Exam Insert================//
            #endregion
        }

        private void Clear()
        {
            txtName.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            txtNationality.Text = "";
            txtDob.Text = null;
            ddlGender.SelectedIndex = 0;
            ddlMaritalStat.SelectedIndex = 0;
            ddlReligion.SelectedIndex = 0;
            ddlBoardSecondary.SelectedIndex = 0;
            ddlExamTypeGraduate.SelectedIndex = 0;
            ddlExamTypeHigherSecondary.SelectedIndex = 0;
            ddlExamTypeSecondary.SelectedIndex = 0;
            ddlExamTypeUndergraduate.SelectedIndex = 0;
            ddlResultTypeGraduate.SelectedIndex = 0;
            ddlResultTypeHigherSecondary.SelectedIndex = 0;
            ddlResultTypeSecondary.SelectedIndex = 0;
            ddlResultTypeUndergraduate.SelectedIndex = 0;
            txtGPAGraduate.Text = "0";
            txtGPAHigherSecondary.Text = "0";
            txtGPASecondary.Text = "0";
            txtGPAUndergraduate.Text = "0";
            txtGW4SHigherSecondary.Text = "0";
            txtGW4SSecondary.Text = "0";
            txtInstituteGraduate.Text = string.Empty;
            txtInstituteHigherSecondary.Text = string.Empty;
            txtInstituteSecondary.Text = string.Empty;
            txtInstituteUndergraduate.Text = string.Empty;
            txtPassingYearGraduate.Text = string.Empty;
            txtPassingYearHigherSecondary.Text = string.Empty;
            txtPassingYearSecondary.Text = string.Empty;
            txtPassingYearUndergraduate.Text = string.Empty;
            hdnGrad.Value = "0_0";
            hdnHigherSecondary.Value = "0_0";
            hdnSecondary.Value = "0_0";
            hdnUnderGrad.Value = "0_0";
            ViewState.Remove("StudentEditId");
            hdnContact.Value = string.Empty;
            txtStudentRoll.Text = string.Empty;
            ddlSession.Items.Clear();
            txtPhone.Text = string.Empty;
            txtPhnGuardian.Text = string.Empty;
            txtEmailPersonal.Text = string.Empty;
            txtSMSContact.Text = string.Empty;
            hdnMailing.Value = string.Empty;
            txtMailingAddress.Text = string.Empty;
            LoadDropDowns();

        }

        protected void lnkEdit_Click(object sender, EventArgs e)
        {
            try
            {                
                Clear();
                btnPopUpSave.Text = "Update";
                //txtTeacherCode.Enabled = false;
                LinkButton linkButton = new LinkButton();
                linkButton = (LinkButton)sender;
                string id = Convert.ToString(linkButton.CommandArgument);
                List<Student> studentList = SessionManager.GetListFromSession<Student>(_StudentList);
                Student student = studentList.Find(x => x.StudentID == Convert.ToInt32(id));
                ViewState.Add("StudentEditId", student.StudentID);
                hfStudentRollChanged.Value = student.Roll;
                txtName.Text = student.BasicInfo.FullName;
                LogicLayer.BusinessObjects.Person person = student.BasicInfo;
                txtName.Text = person.FullName;
                LoadSession(student.ProgramID);
                //ddlCampus.SelectedValue = Convert.ToString(student.CampusId);
                ddlProgram.SelectedValue = Convert.ToString(student.ProgramID);
                ddlSession.SelectedValue = Convert.ToString(student.StudentAdmissionAcaCalId);
                //ddlEmployeeType.SelectedValue = Convert.ToString(teacher.EmployeeTypeId);
                txtFatherName.Text = person.FatherName;
                txtMotherName.Text = person.MotherName;
                txtNationality.Text = person.Nationality;
                txtSMSContact.Text = person.SMSContactSelf;
                txtPhone.Text = person.Phone;
                txtPhnGuardian.Text = person.SMSContactGuardian;
                txtEmailPersonal.Text = person.Email;
                txtStudentRoll.Text = student.Roll;
                txtStudentRoll.Enabled = false;
                btnValidate.Visible = false;
                person.TypeId = 11;
                #region Hidden Field Value Assign
                hfPersonID.Value = person.PersonID.ToString();
                #endregion
                if (person.PhotoPath != null)
                {
                    imgPhoto.ImageUrl = "~/Upload/Avatar/Teacher/" + person.PhotoPath;
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
                txtDob.Text = (person.DOB != null) ? person.DOB.Value.ToString("d/M/yyyy") : null;
                ddlReligion.SelectedValue = string.IsNullOrEmpty(person.ReligionName) == true ? "0" : person.ReligionId.ToString();
                ddlGender.SelectedValue = ddlGender.Items.FindByValue(person.Gender) != null ? person.Gender : "0";
                ddlMaritalStat.SelectedValue = ddlMaritalStat.Items.FindByValue(person.MatrialStatus) != null ? person.MatrialStatus : "0";

                #region not_In_Use
                //int i = 0;
                //List<PreviousExamType> examTypeList = PreviousExamTypeManager.GetAll();
                //foreach (PersonPreviousExam ex in person.PreviousExams)
                //{
                //    PreviousExam pe = PreviousExamManager.GetById((int)ex.PreviousExamId);
                //    PreviousExamType exmTy = PreviousExamTypeManager.GetById((int)ex.PreviousExamTypeId);

                //    if (exmTy.EducationCategory == 1)
                //    {
                //        ddlBoardSecondary.SelectedValue = pe.Board.ToString();
                //        ddlExamTypeSecondary.SelectedValue = ex.PreviousExamTypeId.ToString();
                //        ddlResultTypeSecondary.SelectedValue = pe.ResultId.ToString();
                //        txtGPASecondary.Text = pe.GPA.ToString();
                //        txtGW4SSecondary.Text = pe.GPAW4S.ToString();
                //        txtInstituteSecondary.Text = pe.InstituteName;
                //        txtPassingYearSecondary.Text = pe.PassingYear > 0 ? pe.PassingYear.ToString() : string.Empty;
                //        hdnSecondary.Value = ex.PersonPreviousExamId.ToString() + "_" + pe.PreviousExamId.ToString();
                //    }
                //    else if (exmTy.EducationCategory == 2)
                //    {
                //        ddlBoardHigherSecondary.SelectedValue = pe.Board.ToString();
                //        ddlExamTypeHigherSecondary.SelectedValue = ex.PreviousExamTypeId.ToString();
                //        ddlResultTypeHigherSecondary.SelectedValue = pe.ResultId.ToString();
                //        txtGPAHigherSecondary.Text = pe.GPA.ToString();
                //        txtGW4SHigherSecondary.Text = pe.GPAW4S.ToString();
                //        txtInstituteHigherSecondary.Text = pe.InstituteName;
                //        txtPassingYearHigherSecondary.Text = pe.PassingYear > 0 ? pe.PassingYear.ToString() : string.Empty;
                //        hdnHigherSecondary.Value = ex.PersonPreviousExamId.ToString() + "_" + pe.PreviousExamId.ToString();
                //    }
                //    else if (exmTy.EducationCategory == 3)
                //    {
                //        ddlExamTypeUndergraduate.SelectedValue = ex.PreviousExamTypeId.ToString();
                //        ddlResultTypeUndergraduate.SelectedValue = pe.ResultId.ToString();
                //        txtGPAUndergraduate.Text = pe.GPA.ToString();
                //        txtInstituteUndergraduate.Text = pe.InstituteName;
                //        txtPassingYearUndergraduate.Text = pe.PassingYear > 0 ? pe.PassingYear.ToString() : string.Empty;
                //        hdnUnderGrad.Value = ex.PersonPreviousExamId.ToString() + "_" + pe.PreviousExamId.ToString();
                //    }
                //    else if (exmTy.EducationCategory == 4)
                //    {
                //        ddlExamTypeGraduate.SelectedValue = ex.PreviousExamTypeId.ToString();
                //        ddlResultTypeGraduate.SelectedValue = pe.ResultId.ToString();
                //        txtGPAGraduate.Text = pe.GPA.ToString();
                //        txtInstituteGraduate.Text = pe.InstituteName;
                //        txtPassingYearGraduate.Text = pe.PassingYear > 0 ? pe.PassingYear.ToString() : string.Empty;
                //        hdnGrad.Value = ex.PersonPreviousExamId.ToString() + "_" + pe.PreviousExamId.ToString();
                //    }
                //}

                //Address mailAddress = AddressManager.GetAddressByPersonId(person.PersonID).Where(x => x.AddressTypeId == 4).SingleOrDefault();
                //if (mailAddress != null)
                //{
                //    hdnMailing.Value = mailAddress.AddressId.ToString();
                //    txtMailingAddress.Text = mailAddress.AddressLine;
                //}
                //ContactDetails contact = ContactDetailsManager.GetContactDetailsByPersonID(person.PersonID);
                //if (contact != null)
                //{
                //    hdnContact.Value = contact.PersonID.ToString();
                //    txtMobile1.Text = contact.Mobile1;
                //    txtMobile2.Text = contact.Mobile2;
                //    txtPhnOff.Text = contact.PhoneOffice;
                //    txtPhnRes.Text = contact.PhoneResidential;
                //    txtPhnEmergency.Text = contact.PhoneEmergency;
                //    txtEmailOfficial.Text = contact.EmailOffice;
                //    txtEmailOther.Text = contact.EmailOther;
                //    txtEmailPersonal.Text = contact.EmailPersonal;
                //}
                #endregion

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

            ddlSession.Items.Clear();
            ddlSession.AppendDataBoundItems = true;

            if (sessionList != null)
            {
                ddlSession.Items.Add(new ListItem("-Select Session-", "0"));
                ddlSession.DataTextField = "FullCode";
                ddlSession.DataValueField = "AcademicCalenderID";

                ddlSession.DataSource = sessionList;
                ddlSession.DataBind();
            }
        }
        protected void OnPopupProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            int programId = Convert.ToInt32(ddlProgram.SelectedValue);
            LoadSession(programId);
        }
        private string PhotoUploadProcess()
        {
            string photoName = "";
            try
            {
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
                    string fullDirectoryPath = "~/Upload/Avatar/Teacher/";

                    photoName = UploadImage(tempDirectoryPath, fullDirectoryPath, FileUploadPhoto, 150, 150);
                    if (photoName != "")
                    {
                        hfPhotoPath.Value = photoName + fileExtension;
                        imgPhoto.ImageUrl = fullDirectoryPath + photoName + fileExtension;
                        ShowAlertMessage("Photo Upload Complete");
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

            if (programId > 0 && yearNo > 0 && semesterNo > 0 && currentSessionId > 0)
            {
                List<Student> studentList = StudentManager.GetAllByProgramIdYearNoSemsterNoCurrentSessionId(programId, yearNo, semesterNo, currentSessionId);

                gvStudentList.DataSource = studentList;
                gvStudentList.DataBind();

                if (studentList != null && studentList.Count > 0)
                {
                    SessionManager.SaveListToSession<Student>(studentList, _StudentList);
                }
            }
            else 
            {
                lblMsg.Text = "Please select program, year, semester and current session.";
            }
        }
        protected void btnValidate_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            if (!string.IsNullOrEmpty(txtStudentRoll.Text.Trim()) )
            {
                string roll = txtStudentRoll.Text.Trim();
                Student studentObj = StudentManager.GetByRoll(roll);
                if (studentObj != null)
                {
                    lblValidationStat.Text = "Unavailable";
                }
                else
                {
                    lblValidationStat.Text = "Available";
                }
            }
            else
            {
                lblValidationStat.Text = "Please input roll first!";
            }
        }
        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            txtStudentRoll.Enabled = true;
            btnValidate.Visible = true;
            Clear();
            ModalPopupExtender1.Show();
            btnPopUpSave.Text = "Save";
        }

        private void ShowAlertMessage(string msg)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "ServerControlScript", "alert('" + msg + "');", true);
        }

        #region  ddl Result Type Change
        protected void ResultTypeSecondary_Change(object sender, EventArgs e)
        {
            if (ddlResultTypeSecondary.SelectedValue != "0")
            {
                txtGW4SSecondary.Text = "";
                txtGPASecondary.Text = "";

                txtGW4SSecondary.ReadOnly = true;
                txtGPASecondary.ReadOnly = true;

            }
            else
            {
                txtGW4SSecondary.ReadOnly = false;
                txtGPASecondary.ReadOnly = false;

            }
        }
        protected void ResultTypeHigherSecondary_Change(object sender, EventArgs e)
        {
            if (ddlResultTypeHigherSecondary.SelectedValue != "0")
            {
                txtGW4SHigherSecondary.Text = "";
                txtGPAHigherSecondary.Text = "";

                txtGW4SHigherSecondary.ReadOnly = true;
                txtGPAHigherSecondary.ReadOnly = true;
            }
            else
            {
                txtGW4SHigherSecondary.ReadOnly = false;
                txtGPAHigherSecondary.ReadOnly = false;
            }
        }
        protected void ResultTypeUndergraduate_Change(object sender, EventArgs e)
        {
            if (ddlResultTypeUndergraduate.SelectedValue != "0")
            {
                txtGPAUndergraduate.Text = "";

                txtGPAUndergraduate.ReadOnly = true;
            }
            else
            {
                txtGPAUndergraduate.ReadOnly = false;
            }
        }
        protected void ResultTypeGraduate_Change(object sender, EventArgs e)
        {
            if (ddlResultTypeGraduate.SelectedValue != "0")
            {
                txtGPAGraduate.Text = "";

                txtGPAGraduate.ReadOnly = true;
            }
            else
            {
                txtGPAGraduate.ReadOnly = false;
            }
        }
        #endregion

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
            
            if (string.IsNullOrWhiteSpace(txtName.Text))
                errorList.Add("Name");

            if (ddlProgram.SelectedValue == "0")
                errorList.Add("Program");

            if (ddlSession.SelectedValue == "0")
                errorList.Add("Session");

            if (string.IsNullOrWhiteSpace(txtSMSContact.Text))
                errorList.Add("SMS Contact");

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
    }
}