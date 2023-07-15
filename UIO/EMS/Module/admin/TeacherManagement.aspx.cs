using ClosedXML.Excel;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
//using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class Teacher_Management : BasePage
    {
        private string _TeacherList = "SessionTeacherList";
        BussinessObject.UIUMSUser BaseCurrentUserObj = null;
        private int personID;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            if (!IsPostBack)
            {
                Session["NewImage"] = 0;
                //txtGPAGraduate.Text = "0";
                //txtGPAHigherSecondary.Text = "0";
                //txtGPASecondary.Text = "0";
                //txtGPAUndergraduate.Text = "0";
                //txtGW4SHigherSecondary.Text = "0";
                //txtGW4SSecondary.Text = "0";
                LoadDropDowns();
                LoadRole();
            }
        }
        private void LoadRole()
        {
            try
            {
                ddlUserRole.Items.Clear();
                ddlUserRole.Items.Add(new ListItem("All", "0"));
                List<Role> RoleList = RoleManager.GetAll();
                ddlUserRole.AppendDataBoundItems = true;

                if (RoleList != null && RoleList.Any())
                    RoleList = RoleList.Where(x => x.ID != 1).ToList();

                if (RoleList != null && RoleList.Any())
                {
                    ddlUserRole.DataTextField = "RoleName";
                    ddlUserRole.DataValueField = "ID";
                    ddlUserRole.DataSource = RoleList;
                    ddlUserRole.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }
        private void LoadDropDowns()
        {
            try
            {
                ddlGender.Items.Clear();
                ddlGender.Items.Add(new ListItem("Select", "0"));
                List<Value> genderList = ValueManager.GetByValueSetId(3);
                ddlGender.AppendDataBoundItems = true;
                if (genderList.Count > 0)
                {
                    ddlGender.DataTextField = "ValueName";
                    ddlGender.DataValueField = "ValueID";
                    ddlGender.DataSource = genderList;
                    ddlGender.DataBind();
                }
            }
            catch { }

            try
            {
                ddlStatus.Items.Clear();
                ddlStatus.Items.Add(new ListItem("Select", "0"));
                List<Value> statusList = ValueManager.GetByValueSetId(2);
                ddlStatus.AppendDataBoundItems = true;
                if (statusList.Count > 0)
                {
                    ddlStatus.DataTextField = "ValueName";
                    ddlStatus.DataValueField = "ValueID";
                    ddlStatus.DataSource = statusList;
                    ddlStatus.DataBind();
                }
            }
            catch { }

            try
            {
                ddlStatus1.Items.Clear();
                ddlStatus1.Items.Add(new ListItem("All", "0"));
                List<Value> statusList1 = ValueManager.GetByValueSetId(2);
                ddlStatus1.AppendDataBoundItems = true;
                if (statusList1.Count > 0)
                {
                    ddlStatus1.DataTextField = "ValueName";
                    ddlStatus1.DataValueField = "ValueID";
                    ddlStatus1.DataSource = statusList1;
                    ddlStatus1.DataBind();
                }
            }
            catch { }

            try
            {
                ddlReligion.Items.Clear();
                ddlReligion.Items.Add(new ListItem("Select", "0"));
                List<Value> religionList = ValueManager.GetByValueSetId(4);
                ddlReligion.AppendDataBoundItems = true;
                if (religionList.Count > 0)
                {
                    ddlReligion.DataTextField = "ValueName";
                    ddlReligion.DataValueField = "ValueID";
                    ddlReligion.DataSource = religionList;
                    ddlReligion.DataBind();
                }
            }
            catch { }

            //try
            //{
            //    ddlCategory.Items.Clear();
            //    ddlCategory.Items.Add(new ListItem("Select", "0"));
            //    List<Value> categoryList = ValueManager.GetByValueSetId(1);
            //    ddlCategory.AppendDataBoundItems = true;
            //    if (categoryList.Count > 0)
            //    {
            //        ddlCategory.DataTextField = "ValueName";
            //        ddlCategory.DataValueField = "ValueID";
            //        ddlCategory.DataSource = categoryList;
            //        ddlCategory.DataBind();
            //    }
            //}
            //catch { }

            try
            {
                ddlMaritalStat.Items.Clear();
                ddlMaritalStat.Items.Add(new ListItem("Select", "0"));
                List<Value> maritalStatusList = ValueManager.GetByValueSetId(6);
                ddlMaritalStat.AppendDataBoundItems = true;
                if (maritalStatusList.Count > 0)
                {
                    ddlMaritalStat.DataTextField = "ValueName";
                    ddlMaritalStat.DataValueField = "ValueID";
                    ddlMaritalStat.DataSource = maritalStatusList;
                    ddlMaritalStat.DataBind();
                }
            }
            catch { }
            #region Employee Type
            ddlEmployeeType.Items.Clear();
            //ddlEmployeeType.Items.Add(new ListItem("-Select-", "0"));
            ddlEmployeeType.AppendDataBoundItems = true;
            List<EmployeeType> empTypeList = EmployeeTypeManager.GetAll();
            if (empTypeList != null)
            {
                ddlEmployeeType.DataValueField = "EmployeeTypeId";
                ddlEmployeeType.DataTextField = "EmployeTypeName";
                ddlEmployeeType.DataSource = empTypeList;
                ddlEmployeeType.DataBind();
            }
            #endregion
            try
            {
                ddlTeacherType.Items.Clear();
                ddlTeacherType.Items.Add(new ListItem("Select", "0"));
                List<Value> employeeStatusList = ValueManager.GetByValueSetId(5);
                ddlTeacherType.AppendDataBoundItems = true;
                if (employeeStatusList.Count > 0)
                {
                    ddlTeacherType.DataTextField = "ValueName";
                    ddlTeacherType.DataValueField = "ValueID";
                    ddlTeacherType.DataSource = employeeStatusList;
                    ddlTeacherType.DataBind();
                }
            }
            catch { }

            //#region Exam

            //List<PreviousExamType> examTypeList = PreviousExamTypeManager.GetAll();
            //List<PreviousExamType> collection = new List<PreviousExamType>();

            #region Department

            ddlDepartment.Items.Clear();
            ddlDepartment.Items.Add(new ListItem("-Select-", "0"));
            ddlDepartment.AppendDataBoundItems = true;
            List<Department> deptList = DepartmentManager.GetAll();

            if (deptList != null && deptList.Count > 0)
            {
                ddlDepartment.DataValueField = "DeptID";
                ddlDepartment.DataTextField = "DetailedName";

                ddlDepartment.DataSource = deptList.OrderBy(x => x.DetailedName);
                ddlDepartment.DataBind();
            }

            #endregion


            #region Department

            ddlDepartment1.Items.Clear();
            ddlDepartment1.Items.Add(new ListItem("-All-", "0"));
            ddlDepartment1.AppendDataBoundItems = true;
            List<Department> deptList1 = DepartmentManager.GetAll();

            if (deptList1 != null && deptList1.Count > 0)
            {
                ddlDepartment1.DataValueField = "DeptID";
                ddlDepartment1.DataTextField = "DetailedName";

                ddlDepartment1.DataSource = deptList1.OrderBy(x => x.DetailedName);
                ddlDepartment1.DataBind();
            }

            #endregion

            //#region Board
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
            //#endregion

            //#region ExamType Secondary
            //ddlExamTypeSecondary.Items.Clear();
            //ddlExamTypeSecondary.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeSecondary.AppendDataBoundItems = true;
            //ddlExamTypeSecondary.DataTextField = "Code";
            //ddlExamTypeSecondary.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.SSC).ToList();

            //    ddlExamTypeSecondary.DataSource = collection;
            //    ddlExamTypeSecondary.DataBind();
            //}
            //#endregion

            //#region ExamType Higher Secondary
            //ddlExamTypeHigherSecondary.Items.Clear();
            //ddlExamTypeHigherSecondary.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeHigherSecondary.AppendDataBoundItems = true;
            //ddlExamTypeHigherSecondary.DataTextField = "Code";
            //ddlExamTypeHigherSecondary.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.HSC).ToList();

            //    ddlExamTypeHigherSecondary.DataSource = collection;
            //    ddlExamTypeHigherSecondary.DataBind();
            //}
            //#endregion

            //#region ExamType Undergraduate
            //ddlExamTypeUndergraduate.Items.Clear();
            //ddlExamTypeUndergraduate.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeUndergraduate.AppendDataBoundItems = true;
            //ddlExamTypeUndergraduate.DataTextField = "Code";
            //ddlExamTypeUndergraduate.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Bachelor).ToList();

            //    ddlExamTypeUndergraduate.DataSource = collection;
            //    ddlExamTypeUndergraduate.DataBind();
            //}
            //#endregion

            //#region ExamType Graduate
            //ddlExamTypeGraduate.Items.Clear();
            //ddlExamTypeGraduate.Items.Add(new ListItem("-Select-", "0"));
            //ddlExamTypeGraduate.AppendDataBoundItems = true;
            //ddlExamTypeGraduate.DataTextField = "Code";
            //ddlExamTypeGraduate.DataValueField = "PreviousExamTypeId";

            //if (examTypeList != null)
            //{
            //    collection = examTypeList.Where(p => p.EducationCategory == (int)CommonEnum.EducationCategory.Bachelor).ToList();

            //    ddlExamTypeGraduate.DataSource = collection;
            //    ddlExamTypeGraduate.DataBind();
            //}
            //#endregion

            //#endregion


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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            if (btnPopUpSave.Text.Equals("Update"))
            {
                if (txtTeacherCode.Text.Trim() == hfTeacherCodeChanged.Value)
                {
                    SaveOrEdit("Edit");
                }
                else
                {
                    if (lblValidationStat.Text.Equals("Available"))
                        SaveOrEdit("Edit");
                    else
                        lblValidationStat.Text = "Plz validate first!";
                }
            }
            else if (btnPopUpSave.Text.Equals("Save"))
            {
                if (lblValidationStat.Text.Equals("Available"))
                    SaveOrEdit("Save");
                else
                    lblValidationStat.Text = "Plz validate first!";
            }
        }

        private void SaveOrEdit(string mode)
        {

            try
            {
                if (string.IsNullOrEmpty(txtNameInEnglish.Text) || string.IsNullOrEmpty(txtName.Text)
                    || string.IsNullOrEmpty(txtSMSContact.Text)
                    || string.IsNullOrEmpty(txtEmailOfficial.Text)
                    //|| !string.IsNullOrEmpty(txtNameInEnglish.Text)
                    )
                {
                    ModalPopupExtender1.Show();
                    ShowAlertMessage("Please Enter All Required Field");
                    return;
                }


                BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
                if (!string.IsNullOrEmpty(txtName.Text.Trim()))
                {
                    int a = Convert.ToInt32(ViewState["TeacherEditId"]);

                    List<EmployeeInfo> emList = SessionManager.GetListFromSession<EmployeeInfo>(_TeacherList);
                    EmployeeInfo teacher = emList != null ? emList.Find(x => x.EmployeeID == Convert.ToInt32(ViewState["TeacherEditId"])) : null;

                    // Employee teacher = emList != null ? emList.Find(x => x.EmployeeID == Convert.ToInt32(ViewState["TeacherEditId"])) : null;
                    //Person person = teacher == null ? new Person() : teacher.BasicInfo;
                    Person person = new Person();
                    if (mode.Equals("Edit"))
                    {
                        person.PersonID = teacher.PersonId;
                    }
                    person.FullName = txtNameInEnglish.Text;
                    person.BanglaName = txtName.Text;
                    person.Email = txtEmailOfficial.Text;
                    person.FatherName = txtFatherName.Text;
                    person.MotherName = txtMotherName.Text;
                    person.Nationality = txtNationality.Text;
                    person.SMSContactSelf = txtSMSContact.Text;
                    person.Phone = txtSMSContact.Text;
                    person.DOB = (!txtDob.Text.Trim().Equals("")) ? DateTime.ParseExact(txtDob.Text, "d/M/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    if (!ddlReligion.SelectedValue.Equals("0"))
                        person.ReligionId = Convert.ToInt32(ddlReligion.SelectedItem.Value);
                    if (!ddlGender.SelectedValue.Equals("0"))
                        person.Gender = ddlGender.SelectedValue;
                    if (!ddlMaritalStat.SelectedValue.Equals("0"))
                        person.MatrialStatus = ddlMaritalStat.SelectedValue;


                    person.PhotoPath = lblPhotoPath.Text;

                    if (PhotoUploadProcess() != "")
                        person.PhotoPath = PhotoUploadProcess();
                    else
                    {
                        if (hfPhotoPath.Value != "")
                        {
                            string test = hfPhotoPath.Value;
                            string[] array = test.Split('.');
                            if (array[0] == person.PersonID.ToString())
                                person.PhotoPath = hfPhotoPath.Value;
                        }

                    }
                    /*  
                      
                      else
                      {
                          if (hfPhotoPath.Value != "")
                              person.PhotoPath = hfPhotoPath.Value;

                      }

                      if (teacher == null)
                          teacher = new Employee();
                      if (mode.Equals("Save"))
                      {
                          if (string.IsNullOrEmpty(txtTeacherCode.Text))
                          {
                              lblMsg.Text = "Please input Teacher Code and try again.";
                              return;
                          }*/
                    person.TypeId = 57;

                    person.CreatedBy = BaseCurrentUserObj.Id;
                    person.CreatedDate = DateTime.Now;
                    //int p = 0;
                    if (mode.Equals("Edit"))
                    {
                        bool issaved = PersonManager.Update(person);  //HS
                    }
                    else if (mode.Equals("Save"))
                    {

                        person.PersonID = PersonManager.Insert(person);
                    }

                    // Insert / Update Employee Table
                    Employee anemployee = new Employee();
                    // If "Save" mode, means new Employee to be created, If "Edit" mode, Existing Employee to be edited
                    if (mode.Equals("Edit"))
                    {
                        anemployee.EmployeeID = teacher.EmployeeID;
                        anemployee.PersonId = teacher.PersonId;
                    }
                    if (mode.Equals("Save"))
                    {
                        anemployee.PersonId = person.PersonID;
                    }
                    anemployee.Code = txtTeacherCode.Text;
                    anemployee.Remarks = txtRemarks.Text;
                    anemployee.DOJ = (!txtDoj.Text.Trim().Equals("")) ? DateTime.ParseExact(txtDoj.Text, "d/M/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                    anemployee.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                    anemployee.Program = txtProgram.Text.Trim();
                    //if (Convert.ToInt32(ddlDepartment.SelectedValue) == 0)
                    //{
                    //    ShowAlertMessage("Please select Department DropDown");
                    //    return;
                    //}
                    //else
                    //{
                    anemployee.DeptID = Convert.ToInt32(ddlDepartment.SelectedValue);
                    //}
                    anemployee.LibraryCardNo = txtLibCard.Text;
                    anemployee.EmployeeTypeId = Convert.ToInt32(ddlEmployeeType.SelectedValue);
                    anemployee.Designation = ddlDesignation.SelectedValue.ToString();
                    anemployee.HallInfoId = Convert.ToInt32(ddlHallInfo.SelectedValue);
                    anemployee.TeacherTypeId = Convert.ToInt32(ddlTeacherType.SelectedValue);
                    anemployee.CreatedBy = BaseCurrentUserObj.Id;
                    anemployee.CreatedDate = DateTime.Now;
                    if (mode.Equals("Edit"))
                    {
                        bool issaved = EmployeeManager.Update(anemployee);  //HS
                    }
                    else if (mode.Equals("Save"))
                    {
                        int personID = EmployeeManager.Insert(anemployee);

                    }
                    //int employeeID = EmployeeManager.Insert(anemployee);

                    // Insert in User Table, if it is a new employee, during update no changes is done in user and userinperson tables
                    if (mode.Equals("Save"))
                    {
                        User userObj = new User();
                        userObj.User_ID = 0;
                        userObj.LogInID = txtTeacherCode.Text;
                        userObj.Password = "123456#";
                        userObj.RoleID = 5;
                        userObj.IsActive = true;

                        userObj.RoleExistEndDate = DateTime.Now;
                        userObj.RoleExistStartDate = DateTime.Now;
                        userObj.CreatedBy = BaseCurrentUserObj.Id;
                        userObj.CreatedDate = DateTime.Now;
                        userObj.ModifiedBy = 1;
                        userObj.ModifiedDate = DateTime.Now;
                        int insertedUserID = UserManager.Insert(userObj);

                        if (insertedUserID != 0)
                        {
                            UserInPerson userInPerson = new UserInPerson();
                            userInPerson.User_ID = insertedUserID;
                            userInPerson.PersonID = anemployee.PersonId;
                            userInPerson.CreatedBy = BaseCurrentUserObj.Id;
                            userInPerson.CreatedDate = DateTime.Now;
                            userInPerson.ModifiedBy = BaseCurrentUserObj.Id;
                            userInPerson.ModifiedDate = DateTime.Now;
                            int isInserted = UserInPersonManager.Insert(userInPerson);
                        }
                    }


                    //ExamInsertOrUpdate(personID);
                    //PersonAdditionalInfoInsertOrUpdate(personID);
                    ContactAndAddressInsertOrUpdate(person.PersonID);

                    BankInformationInsertOrUpdate(person.PersonID);

                }
                //else
                //{
                //    person.PersonID = teacher.BasicInfo.PersonID;
                //    person.ModifiedBy = BaseCurrentUserObj.CreatorID;
                //    person.ModifiedDate = DateTime.Now;
                //    PersonManager.Update(person);
                //    teacher = person.Employee;
                //    teacher.Remarks = txtRemarks.Text;
                //    teacher.DOJ = (!txtDoj.Text.Trim().Equals("")) ? DateTime.ParseExact(txtDoj.Text, "d/M/yyyy", CultureInfo.InvariantCulture) : (DateTime?)null;
                //    teacher.Status = Convert.ToInt32(ddlStatus.SelectedValue);
                //    teacher.Program = txtProgram.Text.Trim();
                //    teacher.DeptID = Convert.ToInt32(ddlDepartment.SelectedValue);
                //    teacher.LibraryCardNo = txtLibCard.Text;
                //    teacher.ModifiedBy = BaseCurrentUserObj.Id;
                //    teacher.ModifiedDate = DateTime.Now;
                //    teacher.Code = txtTeacherCode.Text;
                //    teacher.EmployeeTypeId = 1;
                //    EmployeeManager.Update(teacher);
                //    //ExamInsertOrUpdate(person.PersonID);
                //    PersonAdditionalInfoInsertOrUpdate(person.PersonID);
                //    ContactAndAddressInsertOrUpdate(person.PersonID);
                //}

                LoadGridView();
                Clear();
                ModalPopupExtender1.Hide();
                ShowAlertMessage("Succesfully Saved.");

                //}
                //else
                //{
                //    ShowAlertMessage("Plz fill all the fields properly!");
                //}

            }
            catch
            {
                ShowAlertMessage("Something went wrong! Plz try again!");
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

        //private void PersonAdditionalInfoInsertOrUpdate(int personId)
        //{
        //    if (personId > 0)
        //    {
        //        PersonAdditionalInfo pInfo = PersonAdditionalInfoManager.GetByPersonId(personId);
        //        if (pInfo == null)
        //        {
        //            PersonAdditionalInfo addInfo = new PersonAdditionalInfo();
        //            addInfo.PersonId = personId;
        //            addInfo.PersonCategoryEnumValueId = Convert.ToInt32(ddlCategory.SelectedValue);
        //            if (ddlCategory.SelectedValue == "3" || ddlCategory.SelectedValue == "4" || ddlCategory.SelectedValue == "5")
        //                addInfo.IsMilitary = true;
        //            else addInfo.IsMilitary = false;

        //            PersonAdditionalInfoManager.Insert(addInfo);
        //        }
        //        else
        //        {
        //            pInfo.PersonCategoryEnumValueId = Convert.ToInt32(ddlCategory.SelectedValue);
        //            if (ddlCategory.SelectedValue == "3" || ddlCategory.SelectedValue == "4" || ddlCategory.SelectedValue == "5")
        //                pInfo.IsMilitary = true;
        //            else pInfo.IsMilitary = false;
        //            PersonAdditionalInfoManager.Update(pInfo);
        //        }
        //    }
        //}

        private void ContactAndAddressInsertOrUpdate(int personID)
        {
            //if (!string.IsNullOrEmpty(hdnMailing.Value))
            //{
            //    Address address = AddressManager.GetById(Convert.ToInt32(hdnMailing.Value));
            //    address.AddressLine = txtMailingAddress.Text;
            //    address.ModifiedBy = BaseCurrentUserObj.Id;
            //    address.ModifiedDate = DateTime.Now;
            //    AddressManager.Update(address);
            //}
            //else
            //{
            //    Address address = new Address();
            //    address.AddressLine = txtMailingAddress.Text;
            //    address.CreatedBy = BaseCurrentUserObj.Id;
            //    address.CreatedDate = DateTime.Now;
            //    address.ModifiedBy = BaseCurrentUserObj.Id;
            //    address.ModifiedDate = DateTime.Now;
            //    address.PersonId = personID;
            //    address.AddressTypeId = 4;
            //    AddressManager.Insert(address);
            //}

            if (!string.IsNullOrEmpty(hdnContact.Value))
            {
                ContactDetails contact = ContactDetailsManager.GetContactDetailsByPersonID(Convert.ToInt32(hdnContact.Value));
                contact.Mobile1 = txtMobile1.Text;
                contact.Mobile2 = txtMobile2.Text;
                contact.PhoneEmergency = txtPhnEmergency.Text;
                contact.PhoneOffice = txtPhnOff.Text;
                contact.PhoneResidential = txtPhnRes.Text;
                contact.EmailPersonal = txtEmailPersonal.Text;
                contact.EmailOther = txtEmailOther.Text;
                contact.EmailOffice = txtEmailOfficial.Text;
                ContactDetailsManager.Update(contact);
            }
            else
            {
                ContactDetails contact = new ContactDetails();
                contact.PersonID = personID;
                contact.Mobile1 = txtMobile1.Text;
                contact.Mobile2 = txtMobile2.Text;
                contact.PhoneEmergency = txtPhnEmergency.Text;
                contact.PhoneOffice = txtPhnOff.Text;
                contact.PhoneResidential = txtPhnRes.Text;
                contact.EmailPersonal = txtEmailPersonal.Text;
                contact.EmailOther = txtEmailOther.Text;
                contact.EmailOffice = txtEmailOfficial.Text;
                ContactDetailsManager.Insert(contact);
            }
        }

        //private void ExamInsertOrUpdate(int personID)
        //{
        //    BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
        //#region Exam
        ////================Exam Insert================//
        //#region Education Info
        //PersonPreviousExam candidateExam = null;
        //bool sucess;
        //decimal value;

        //#region Secondary

        //if (Convert.ToInt32(ddlExamTypeSecondary.SelectedValue) > 0)
        //{
        //    string[] examIds = hdnSecondary.Value.Split('_');

        //    PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));
        //    exam.Board = Convert.ToInt32(ddlBoardSecondary.SelectedValue);
        //    exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPASecondary.Text) ? "0" : txtGPASecondary.Text);
        //    exam.GPAW4S = Convert.ToDecimal(string.IsNullOrEmpty(txtGW4SSecondary.Text) ? "0" : txtGW4SSecondary.Text);
        //    exam.InstituteName = txtInstituteSecondary.Text;
        //    exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearSecondary.Text) ? "0" : txtPassingYearSecondary.Text);
        //    exam.Result = ddlResultTypeSecondary.SelectedItem.Text;
        //    exam.ResultId = Convert.ToInt32(ddlResultTypeSecondary.SelectedValue);
        //    int secondaryExamId = 0;
        //    if (examIds[1].Equals("0"))
        //    {
        //        exam.CreatedBy = BaseCurrentUserObj.Id;
        //        exam.CreatedOn = DateTime.Now;
        //        secondaryExamId = PreviousExamManager.Insert(exam);
        //    }
        //    else
        //    {
        //        exam.ModifiedBy = BaseCurrentUserObj.Id;
        //        exam.ModifiedOn = DateTime.Now;
        //        PreviousExamManager.Update(exam);
        //    }

        //    candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
        //    if (examIds[0].Equals("0"))
        //    {
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeSecondary.SelectedValue);
        //        candidateExam.CreatedBy = BaseCurrentUserObj.Id;
        //        candidateExam.CreatedOn = DateTime.Now;
        //        candidateExam.PersonId = personID;
        //        candidateExam.PreviousExamId = secondaryExamId;
        //        PersonPreviousExamManager.Insert(candidateExam);
        //    }
        //    else
        //    {
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeSecondary.SelectedValue);
        //        PersonPreviousExamManager.Update(candidateExam);
        //    }
        //}
        //#endregion

        //#region Higher Secondary
        //if (Convert.ToInt32(ddlExamTypeHigherSecondary.SelectedValue) > 0)
        //{
        //    string[] examIds = hdnHigherSecondary.Value.Split('_');

        //    PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));
        //    exam.Board = Convert.ToInt32(ddlBoardHigherSecondary.SelectedValue);
        //    exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPAHigherSecondary.Text) ? "0" : txtGPAHigherSecondary.Text);
        //    exam.GPAW4S = Convert.ToDecimal(string.IsNullOrEmpty(txtGW4SHigherSecondary.Text) ? "0" : txtGW4SHigherSecondary.Text);
        //    exam.InstituteName = txtInstituteHigherSecondary.Text;
        //    exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearHigherSecondary.Text) ? "0" : txtPassingYearHigherSecondary.Text);
        //    exam.CreatedBy = BaseCurrentUserObj.Id;
        //    exam.CreatedOn = DateTime.Now;
        //    exam.Result = ddlResultTypeHigherSecondary.SelectedItem.Text;
        //    exam.ResultId = Convert.ToInt32(ddlResultTypeHigherSecondary.SelectedValue);
        //    int hsecondaryExamId = 0;

        //    if (examIds[1].Equals("0"))
        //    {
        //        exam.CreatedBy = BaseCurrentUserObj.Id;
        //        exam.CreatedOn = DateTime.Now;
        //        hsecondaryExamId = PreviousExamManager.Insert(exam);
        //    }
        //    else
        //    {
        //        exam.ModifiedBy = BaseCurrentUserObj.Id;
        //        exam.ModifiedOn = DateTime.Now;
        //        PreviousExamManager.Update(exam);
        //    }

        //    candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
        //    if (examIds[0].Equals("0"))
        //    {
        //        candidateExam.PersonId = personID;
        //        candidateExam.PreviousExamId = hsecondaryExamId;
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeHigherSecondary.SelectedValue);
        //        candidateExam.CreatedBy = BaseCurrentUserObj.Id;
        //        candidateExam.CreatedOn = DateTime.Now;
        //        PersonPreviousExamManager.Insert(candidateExam);
        //    }
        //    else
        //    {
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeHigherSecondary.SelectedValue);
        //        PersonPreviousExamManager.Update(candidateExam);
        //    }
        //}
        //#endregion

        //#region Undergraduate
        //if (Convert.ToInt32(ddlExamTypeUndergraduate.SelectedValue) > 0)
        //{
        //    string[] examIds = hdnUnderGrad.Value.Split('_');

        //    PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));
        //    exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPAUndergraduate.Text) ? "0" : txtGPAUndergraduate.Text);
        //    exam.InstituteName = txtInstituteUndergraduate.Text;
        //    exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearUndergraduate.Text) ? "0" : txtPassingYearUndergraduate.Text);
        //    exam.CreatedBy = BaseCurrentUserObj.Id;
        //    exam.CreatedOn = DateTime.Now;
        //    exam.Result = ddlResultTypeUndergraduate.SelectedItem.Text;
        //    exam.ResultId = Convert.ToInt32(ddlResultTypeUndergraduate.SelectedValue);
        //    int uExamId = 0;

        //    if (examIds[1].Equals("0"))
        //    {
        //        exam.CreatedBy = BaseCurrentUserObj.Id;
        //        exam.CreatedOn = DateTime.Now;
        //        uExamId = PreviousExamManager.Insert(exam);
        //    }
        //    else
        //    {
        //        exam.ModifiedBy = BaseCurrentUserObj.Id;
        //        exam.ModifiedOn = DateTime.Now;
        //        PreviousExamManager.Update(exam);
        //    }

        //    candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
        //    if (examIds[0].Equals("0"))
        //    {
        //        candidateExam = new PersonPreviousExam();
        //        candidateExam.PersonId = personID;
        //        candidateExam.PreviousExamId = uExamId;
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeUndergraduate.SelectedValue);
        //        candidateExam.CreatedBy = BaseCurrentUserObj.Id;
        //        candidateExam.CreatedOn = DateTime.Now;
        //        PersonPreviousExamManager.Insert(candidateExam);
        //    }
        //    else
        //    {
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeUndergraduate.SelectedValue);
        //        PersonPreviousExamManager.Update(candidateExam);
        //    }
        //}
        //#endregion

        //#region Graduate
        //if (Convert.ToInt32(ddlExamTypeGraduate.SelectedValue) > 0)
        //{
        //    string[] examIds = hdnGrad.Value.Split('_');

        //    PreviousExam exam = examIds[1].Equals("0") ? new PreviousExam() : PreviousExamManager.GetById(Convert.ToInt32(examIds[1]));

        //    exam.GPA = Convert.ToDecimal(string.IsNullOrEmpty(txtGPAGraduate.Text) ? "0" : txtGPAGraduate.Text);

        //    exam.InstituteName = txtInstituteGraduate.Text;
        //    exam.PassingYear = Convert.ToInt32(string.IsNullOrEmpty(txtPassingYearGraduate.Text) ? "0" : txtPassingYearGraduate.Text);
        //    exam.CreatedBy = BaseCurrentUserObj.Id;
        //    exam.CreatedOn = DateTime.Now;
        //    exam.Result = ddlResultTypeGraduate.SelectedItem.Text;
        //    exam.ResultId = Convert.ToInt32(ddlResultTypeGraduate.SelectedValue);
        //    int gExamId = 0;

        //    if (examIds[1].Equals("0"))
        //    {
        //        exam.CreatedBy = BaseCurrentUserObj.Id;
        //        exam.CreatedOn = DateTime.Now;
        //        gExamId = PreviousExamManager.Insert(exam);
        //    }
        //    else
        //    {
        //        exam.ModifiedBy = BaseCurrentUserObj.Id;
        //        exam.ModifiedOn = DateTime.Now;
        //        PreviousExamManager.Update(exam);
        //    }

        //    candidateExam = examIds[0].Equals("0") ? new PersonPreviousExam() : PersonPreviousExamManager.GetById(Convert.ToInt32(examIds[0]));
        //    if (examIds[0].Equals("0"))
        //    {
        //        candidateExam.PersonId = personID;
        //        candidateExam.PreviousExamId = gExamId;
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeGraduate.SelectedValue);
        //        candidateExam.CreatedBy = BaseCurrentUserObj.Id;
        //        candidateExam.CreatedOn = DateTime.Now;
        //        PersonPreviousExamManager.Insert(candidateExam);
        //    }
        //    else
        //    {
        //        candidateExam.PreviousExamTypeId = Convert.ToInt32(ddlExamTypeGraduate.SelectedValue);
        //        PersonPreviousExamManager.Update(candidateExam);
        //    }
        //}
        //#endregion

        //#endregion
        ////================Exam Insert================//
        //#endregion
        //}

        private void Clear()
        {
            txtName.Text = "";
            txtTeacherCode.Text = "";
            txtNameInEnglish.Text = "";
            txtFatherName.Text = "";
            txtMotherName.Text = "";
            txtNationality.Text = "";
            txtProgram.Text = "";
            txtDob.Text = null;
            ddlGender.SelectedIndex = 0;
            ddlMaritalStat.SelectedIndex = 0;
            ddlReligion.SelectedIndex = 0;
            ddlStatus.SelectedIndex = 0;
            //ddlCategory.SelectedIndex = 0;
            ddlEmployeeType.SelectedIndex = 0;
            ddlDesignation.SelectedIndex = 0;
            ddlHallInfo.SelectedIndex = 0;
            ddlTeacherType.SelectedIndex = 0;

            txtAcNo.Text = "";
            txtBankName.Text = "";
            txtBranch.Text = "";
            txtRoutingNo.Text = "";

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
            txtRemarks.Text = string.Empty;
            ddlDepartment.SelectedValue = "0";
            txtDoj.Text = string.Empty;
            lblValidationStat.Text = "";
            txtTeacherCode.Enabled = true;
            //hdnGrad.Value = "0_0";
            //hdnHigherSecondary.Value = "0_0";
            //hdnSecondary.Value = "0_0";
            //hdnUnderGrad.Value = "0_0";
            ViewState.Remove("TeacherEditId");
            hdnContact.Value = string.Empty;
            txtMobile1.Text = string.Empty;
            txtMobile2.Text = string.Empty;
            txtPhnOff.Text = string.Empty;
            txtPhnRes.Text = string.Empty;
            txtPhnEmergency.Text = string.Empty;
            txtEmailOfficial.Text = string.Empty;
            txtEmailOther.Text = string.Empty;
            txtEmailPersonal.Text = string.Empty;
            //hdnMailing.Value = string.Empty;
            //txtMailingAddress.Text = string.Empty;
            txtLibCard.Text = string.Empty;
            txtSMSContact.Text = string.Empty;
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
                List<EmployeeInfo> teacherList = SessionManager.GetListFromSession<EmployeeInfo>(_TeacherList);
                EmployeeInfo teacher = teacherList.Find(x => x.EmployeeID == Convert.ToInt32(id));
                ViewState.Add("TeacherEditId", teacher.EmployeeID);
                txtName.Text = teacher.FullName;
                txtTeacherCode.Text = teacher.Code.ToString();
                hfTeacherCodeChanged.Value = teacher.Code.ToString();
                txtLibCard.Text = teacher.LibraryCardNo;

                LogicLayer.BusinessObjects.Person person = PersonManager.GetById(teacher.PersonId);
                int PersonId = Convert.ToInt32(teacher.PersonId);
                txtName.Text = person.BanglaName;
                txtNameInEnglish.Text = person.FullName;
                txtFatherName.Text = person.FatherName;
                txtMotherName.Text = person.MotherName;
                txtNationality.Text = person.Nationality;
                txtSMSContact.Text = person.SMSContactSelf;
                txtDob.Text = (person.DOB != null) ? person.DOB.Value.ToString("d/M/yyyy") : null;
                ddlReligion.SelectedValue = ddlReligion.Items.FindByValue(person.ReligionName) != null ? person.ReligionName : "0";
                ddlGender.SelectedValue = ddlGender.Items.FindByValue(person.Gender) != null ? person.Gender : "0";
                ddlMaritalStat.SelectedValue = ddlMaritalStat.Items.FindByValue(person.MatrialStatus) != null ? person.MatrialStatus : "0";
                txtEmailOfficial.Text = person.Email;


                Employee emp = EmployeeManager.GetById(Convert.ToInt32(id));
                if (emp != null)
                {
                    ddlEmployeeType.SelectedValue = emp.EmployeeTypeId.ToString();
                    ddlTeacherType.SelectedValue = emp.TeacherTypeId.ToString();

                    if (emp.Designation != null)
                    {
                        try
                        {
                            ddlDesignation.SelectedValue = emp.Designation.Trim().ToString();
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    if (emp.HallInfoId != null)
                    {
                        try
                        {
                            ddlHallInfo.SelectedValue = emp.HallInfoId.ToString();
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                }
                lblPhotoPath.Text = "";

                #region Hidden Field Value Assign
                hfPersonID.Value = person.PersonID.ToString();
                #endregion
                if (person.PhotoPath != null)
                {
                    imgPhoto.ImageUrl = "~/Upload/Avatar/Teacher/" + person.PhotoPath + "?v=" + DateTime.Now;
                    lblPhotoPath.Text = person.PhotoPath;
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

                ddlDepartment.SelectedValue = teacher.DeptID != 0 ? teacher.DeptID.ToString() : "0";
                ddlStatus.SelectedValue = teacher.Status != 0 ? teacher.Status.ToString() : "0";
                txtProgram.Text = teacher.Program;
                //txtRemarks.Text = teacher.Remarks;
                txtDoj.Text = (teacher.DOJ != null) ? teacher.DOJ.Value.ToString("d/M/yyyy") : null;


                List<Address> AllAddressList = AddressManager.GetAddressByPersonId(person.PersonID);



                ContactDetails contact = ContactDetailsManager.GetContactDetailsByPersonID(person.PersonID);
                if (contact != null)
                {
                    hdnContact.Value = contact.PersonID.ToString();
                    txtMobile1.Text = contact.Mobile1;
                    txtMobile2.Text = contact.Mobile2;
                    txtPhnOff.Text = contact.PhoneOffice;
                    txtPhnRes.Text = contact.PhoneResidential;
                    txtPhnEmergency.Text = contact.PhoneEmergency;
                    txtEmailOfficial.Text = contact.EmailOffice;
                    txtEmailOther.Text = contact.EmailOther;
                    txtEmailPersonal.Text = contact.EmailPersonal;
                }
                //PersonAdditionalInfo addInfo = PersonAdditionalInfoManager.GetByPersonId(person.PersonID);
                //if (addInfo != null)
                //{
                //    ddlCategory.SelectedValue = addInfo.PersonCategoryEnumValueId.ToString();
                //}

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

                ModalPopupExtender1.Show();
            }
            catch { };
        }

        //protected void lnkEmailSend_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        LinkButton linkButton = new LinkButton();
        //        linkButton = (LinkButton)sender;
        //        string id = Convert.ToString(linkButton.CommandArgument);
        //        List<EmployeeInfo> teacherList = SessionManager.GetListFromSession<EmployeeInfo>(_TeacherList);
        //        EmployeeInfo teacher = teacherList.Find(x => x.EmployeeID == Convert.ToInt32(id));


        //        if (string.IsNullOrEmpty(teacher.Email) == false && string.IsNullOrEmpty(teacher.LogInID) == false && string.IsNullOrEmpty(teacher.Password) == false)
        //        {
        //            bool issent = SendLoginCredentialInEmail(teacher.Email, teacher.LogInID, teacher.Password);

        //            if (issent)
        //            {
        //                ShowAlertMessage("Email Send Successfully");
        //            }
        //            else
        //            {
        //                ShowAlertMessage("Error Occurd. Email Send Unsuccessful.");
        //            }
        //        }
        //        else
        //        {
        //            ShowAlertMessage("Invalid Data. No Email Found or No User Account Found. Please update Email or Update User Account then try again.");
        //        }
        //    }
        //    catch { };
        //}


        protected void btnLoad_Click(object sender, EventArgs e)
        {
            LoadGridView();
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
                        Session["NewImage"] = 1;
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
            List<EmployeeInfo> teacherList = EmployeeManager.GetEmployeeInfoAllByNameOrCode(txtSearchTeacherName.Text, txtSearchCode.Text.Trim(), ddlDepartment1.SelectedValue, ddlStatus1.SelectedValue);

            if (teacherList != null && teacherList.Any())
            {

                try
                {
                    string desg = ddlDesgFilter.SelectedValue.ToString();
                    if (desg != "All")
                    {
                        teacherList = teacherList.Where(x => x.Designation == desg).ToList();
                    }

                    int RoleId = Convert.ToInt32(ddlUserRole.SelectedValue);
                    if (RoleId != 0)
                    {
                        teacherList = teacherList.Where(x => x.RoleId == RoleId).ToList();
                    }
                }
                catch (Exception ex)
                {
                }

                foreach (var item in teacherList)
                {
                    try
                    {
                        Person perObj = PersonManager.GetById(item.PersonId);
                        if (perObj != null)
                        {
                            item.Remarks = perObj.PhotoPath.Length > 0 ? "~/Upload/Avatar/Teacher/" + perObj.PhotoPath + "?v=" + DateTime.Now : perObj.Gender.ToLower() == "female" ? "~/Upload/Avatar/Female.jpg" : "~/Upload/Avatar/Male.jpg";
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }

                teacherList = teacherList.OrderBy(o => o.Code).ToList();
                SessionManager.SaveListToSession<EmployeeInfo>(teacherList, _TeacherList);

                Session["TeacherList"] = null;
                Session["TeacherList"] = teacherList;
            }

            gvTeacherList.DataSource = teacherList;
            gvTeacherList.DataBind();

        }

        protected void btnDowload_Click(object sender, EventArgs e)
        {
            try
            {
                string DeptId = ddlDepartment1.SelectedItem.Text;



                // AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(acaCalSectionId);
                string examExcelFileName = DeptId;

                DataTable table = new DataTable();

                List<EmployeeInfo> teacherList = EmployeeManager.GetEmployeeInfoAllByNameOrCode(txtSearchTeacherName.Text, txtSearchCode.Text.Trim(), ddlDepartment1.SelectedValue, ddlStatus1.SelectedValue);

                if (teacherList != null && teacherList.Count > 0)
                {
                    table.Columns.Add("SL", typeof(string));
                    table.Columns.Add("ID", typeof(string));
                    table.Columns.Add("Name", typeof(string));
                    table.Columns.Add("BanglaName", typeof(string));
                    table.Columns.Add("Department Name", typeof(string));
                    table.Columns.Add("Status", typeof(string));
                    table.Columns.Add("Contact Number", typeof(string));
                    table.Columns.Add("Email", typeof(string));
                    table.Columns.Add("User_ID", typeof(string));
                    table.Columns.Add("Designation", typeof(string));


                    for (int i = 0; i < teacherList.Count; i++)
                    {
                        DataRow newRow;
                        //ExamMarkNewALLDTO studentObj = new ExamMarkNewALLDTO();

                        EmployeeInfo studentObj = teacherList[i];

                        object[] rowArray = new object[10];
                        rowArray[0] = i + 1;
                        rowArray[1] = studentObj.Code;
                        rowArray[2] = studentObj.FullName;
                        rowArray[3] = studentObj.BanglaName;
                        rowArray[4] = studentObj.DeptName;
                        rowArray[5] = studentObj.StatusDetails;
                        rowArray[6] = studentObj.SMSContactSelf;
                        rowArray[7] = studentObj.Email;
                        rowArray[8] = studentObj.LogInID;
                        rowArray[9] = studentObj.Designation;

                        newRow = table.NewRow();
                        newRow.ItemArray = rowArray;
                        table.Rows.Add(newRow);
                    }

                    CreateExcel(table, examExcelFileName);

                }
            }
            catch { }

        }

        private void CreateExcel(DataTable table, string examExcelFileName)
        {


            string filepath = Server.MapPath(@"~\Upload");
            HttpFileCollection uploadedFiles = Request.Files;
            string fileName = examExcelFileName + ".xlsx"; // +'_';            
            filepath = filepath + "\\" + fileName;
            FileInfo newFile = new FileInfo(filepath);
            if (System.IO.File.Exists(filepath))
            {
                System.IO.File.Delete(filepath);
            }
            using (XLWorkbook wb = new XLWorkbook())
            {

                var worksheet = wb.Worksheets.Add(table, "validation");
                Response.Clear();
                Response.Buffer = true;
                Response.Charset = "";
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=" + fileName);
                using (MemoryStream MyMemoryStream = new MemoryStream())
                {
                    wb.SaveAs(MyMemoryStream);
                    MyMemoryStream.WriteTo(Response.OutputStream);
                    Response.Flush();
                    //Response.End();
                    Response.Close();
                }
            }
        }


        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Clear();
            ModalPopupExtender1.Show();
            btnPopUpSave.Text = "Save";
        }

        protected void btnValidate_Click(object sender, EventArgs e)
        {
            ModalPopupExtender1.Show();
            if (!string.IsNullOrEmpty(txtTeacherCode.Text.Trim()) && EmployeeManager.ValidateEmployee(txtTeacherCode.Text.Trim()))
            {
                lblValidationStat.Text = "Available";

            }
            else
            {
                lblValidationStat.Text = "Unavailable";
            }
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

        protected void gvTeacherList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                List<EmployeeInfo> teachersList = (List<EmployeeInfo>)Session["TeacherList"];

                if (teachersList != null && teachersList.Count > 0)
                {
                    string sortdirection = string.Empty;
                    if (Session["direction"] != null)
                    {
                        if (Session["direction"].ToString() == "ASC")
                        {
                            sortdirection = "DESC";
                        }
                        else
                        {
                            sortdirection = "ASC";
                        }
                    }
                    else
                    {
                        sortdirection = "DESC";
                    }
                    Session["direction"] = sortdirection;
                    Sort(teachersList, e.SortExpression.ToString(), sortdirection);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        private void Sort(List<EmployeeInfo> list, String sortBy, String sortDirection)
        {
            if (sortDirection == "ASC")
            {
                list.Sort(new GenericComparer<EmployeeInfo>(sortBy, (int)SortDirection.Ascending));
            }
            else
            {
                list.Sort(new GenericComparer<EmployeeInfo>(sortBy, (int)SortDirection.Descending));
            }
            gvTeacherList.DataSource = list;
            gvTeacherList.DataBind();
        }

        //public static bool SendLoginCredentialInEmail(string Email, string LoginId, string Password)
        //{
        //    try
        //    {
        //        bool isSent = false;

        //        string body = "You're receiving this e-mail because you requested UCAM Login Credential for MIST UCAM. <br /><br />" +
        //                    "Login ID: " + LoginId + " <br /><br />" +
        //                    "Password: " + Password + " <br /><br />" +
        //                    "Thanks You!";

        //        isSent = Sendmail.sendEmailViaEduMIST("MIST Support", Email, "mcam@mist.ac.bd", "UCAM Password Recovery", body);



        //        return isSent;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}