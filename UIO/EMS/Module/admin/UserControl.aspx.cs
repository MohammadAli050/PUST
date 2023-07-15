using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CommonUtility;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;


namespace EMS.Module.admin
{
    public partial class UserControl : BasePage
    {
        BussinessObject.UIUMSUser userObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            //base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                LoadDropdownList();
                ucAccessableProgram.LoadDropDownList();

                if (userObj.RoleID == 1)
                    txtPassword.Visible = true;
                else
                    txtPassword.Visible = false;

            }
            //txtPassword.Attributes["value"] = txtPassword.Text;
        }

        private void ClearAll()
        {
            ddlRole.SelectedIndex = -1;
            txtUserId.Text = "";
            //txtPassword.Attributes.Add("Value", "");
            txtPassword.Text = "";
            txtStartDate.Text = "";
            txtEndDate.Text = "";
            chkAccessList.Items.Clear();
            lblMsg.Text = "";
            lblValidate.Text = "";
            //ddlOffice.SelectedIndex = 0;
        }
        protected void onTeacherSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
        }
        protected void onUserTypeSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearAll();
            ddlTeacher.Items.Clear();
            LoadTeacherCombo(Convert.ToInt32(ddlUserType.SelectedValue));
        }
        protected void onOfficeSelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void LoadDropdownList()
        {
            //LoadTeacherCombo();
            LoadRoleCombo();
            LoadUserType();
            //LoadOfficeCombo();
        }

        //private void LoadOfficeCombo()
        //{
        //    ddlOffice.Items.Clear();
        //    ddlOffice.AppendDataBoundItems = true;
        //    List<LogicLayerCFO_Office.BusinessObjectsCFO_Office.Office> officeList = OfficeManager.GetAll();
        //    if (officeList.Count > 0)
        //    {
        //        ddlOffice.Items.Add(new ListItem("-Select-", "0"));
        //        ddlOffice.DataTextField = "OfficeName";
        //        ddlOffice.DataValueField = "OfficeId";

        //        ddlOffice.DataSource = officeList;
        //        ddlOffice.DataBind();
        //    }
        //}
        private void LoadUserType()
        {
            ddlUserType.AppendDataBoundItems = true;
            List<EmployeeType> employeeTypeList = EmployeeTypeManager.GetAll();
            if (userObj.RoleID == 2)
            {
                employeeTypeList = employeeTypeList.Where(x => x.EmployeTypeName == "Teacher").ToList();
            }
            if (employeeTypeList != null)
            {
                ddlUserType.Items.Add(new ListItem("-Select-", "0"));
                ddlUserType.DataTextField = "EmployeTypeName";
                ddlUserType.DataValueField = "EmployeeTypeId";

                ddlUserType.DataSource = employeeTypeList;
                ddlUserType.DataBind();
            }
        }
        private void LoadRoleCombo()
        {
            ddlRole.AppendDataBoundItems = true;
            List<Role> roleList = RoleManager.GetAll();
            if (userObj.RoleID != 1)
            {
                roleList = roleList.Where(x => !x.RoleName.Contains("ESCL")).ToList();
            }

            if (roleList != null)
            {
                ddlRole.Items.Add(new ListItem("-Select-", "0"));
                ddlRole.DataTextField = "RoleName";
                ddlRole.DataValueField = "ID";

                ddlRole.DataSource = roleList;
                ddlRole.DataBind();
            }
        }
        private void LoadTeacherCombo(int typeId)
        {
            try
            {
                ddlTeacher.AppendDataBoundItems = true;
                List<Employee> employeeList = EmployeeManager.GetAllByTypeId(typeId);
                if (userObj.RoleID == 2)
                {
                    User user = UserManager.GetByLogInId(userObj.LogInID);
                    Employee employeeObj = EmployeeManager.GetByPersonId(user.Person.PersonID);
                    if (employeeObj != null)
                        employeeList = employeeList.Where(x => x.DeptID == employeeObj.DeptID).ToList();
                }

                if (employeeList != null)
                {
                    employeeList = employeeList.Where(x => x.LoginIdAndName != null).ToList();
                    employeeList = employeeList.OrderBy(x => x.LoginIdAndName).ToList();
                    ddlTeacher.Items.Add(new ListItem("-Select-", "0"));
                    ddlTeacher.DataTextField = "LoginIdAndName";
                    ddlTeacher.DataValueField = "PersonId";

                    ddlTeacher.DataSource = employeeList;
                    ddlTeacher.DataBind();
                }
            }
            catch { }
        }
        protected void btnAddClicked(object sender, EventArgs e)
        {
            if (ucAccessableProgram.selectedValue != "0")
            {
                bool isExist = false;
                ListItem newitem = new ListItem();
                newitem.Text = ucAccessableProgram.selectedText;
                newitem.Value = ucAccessableProgram.selectedValue;
                newitem.Selected = true;
                foreach (ListItem l in chkAccessList.Items)
                {
                    if (l.Value == newitem.Value)
                        isExist = true;
                }
                if (!isExist)
                    chkAccessList.Items.Add(newitem);
            }
        }
        protected void btnAllProgClicked(object sender, EventArgs e)
        {
            chkAccessList.Items.Clear();
            List<Program> programList = new List<Program>();
            programList = ProgramManager.GetAll();
            foreach (Program p in programList)
            {
                ListItem newitem = new ListItem();
                newitem.Text = p.ShortName;
                newitem.Value = Convert.ToString(p.ProgramID);
                newitem.Selected = true;
                chkAccessList.Items.Add(newitem);
            }
        }
        protected void btnValidateClick(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUserId.Text))
            {
                User userObj = UserManager.GetByLogInId(txtUserId.Text.Trim());
                if (userObj == null)
                    lblValidate.Text = "Available";
                else lblValidate.Text = "Unavailable";
            }
        }
        protected void btnSendPassword_Click(object sender, EventArgs e)
        {
            try
            {
                //Person personObj = PersonManager.GetById(Convert.ToInt32(ddlTeacher.SelectedValue));
                //if (!string.IsNullOrEmpty(txtPassword.Text) && !string.IsNullOrEmpty(txtUserId.Text))
                //{
                //    if (!string.IsNullOrEmpty(personObj.Email))
                //    {
                //        string msg = "Your Login ID: " + txtUserId.Text + " and Password: " + txtPassword.Text;
                //        bool resultMail = Sendmail.sendEmail("BUP", personObj.Email, "", "User Login Information", msg);
                //        if (resultMail)
                //            lblMsg.Text = "Email sent.";
                //    }
                //    else lblMsg.Text = "User does not have an Email address. Please update Email from User Profile.";
                //}
                //else
                //    lblMsg.Text = "User's Login ID and Password can't be blank.";
            }
            catch
            {
                lblMsg.Text = "User not found.";
            }
        }

        protected void btnSearchClick(object sender, EventArgs e)
        {
            try
            {
                int typeId = Convert.ToInt32(ddlUserType.SelectedValue);
                ClearAll();
                ddlTeacher.Items.Clear();
                if (string.IsNullOrEmpty(txtNameSearch.Text.Trim()))
                    LoadTeacherCombo(typeId);
                else
                {
                    ddlTeacher.AppendDataBoundItems = true;
                    List<Employee> employeeList = EmployeeManager.GetAllByTypeId(typeId);
                    employeeList = employeeList.Where(x => x.LoginIdAndName.ToLower().Contains(txtNameSearch.Text.ToLower())).ToList();
                    if (employeeList != null)
                    {
                        employeeList = employeeList.OrderBy(x => x.LoginIdAndName).ToList();
                        ddlTeacher.DataTextField = "LoginIdAndName";
                        ddlTeacher.DataValueField = "PersonID";

                        ddlTeacher.DataSource = employeeList;
                        ddlTeacher.DataBind();
                    }
                }
            }
            catch { }
        }
        protected void btnClearProgClicked(object sender, EventArgs e)
        {
            chkAccessList.Items.Clear();
        }
        protected void btnLoadTeacherClick(object sender, EventArgs e)
        {
            ClearAll();
            try
            {
                Person personObj = PersonManager.GetById(Convert.ToInt32(ddlTeacher.SelectedValue));
                User userObj = personObj.Users.FirstOrDefault();
                if (userObj != null)
                {
                    ddlRole.SelectedValue = Convert.ToString(userObj.RoleID);
                    txtUserId.Text = userObj.LogInID;
                    //txtPassword.Attributes.Add("Value", userObj.Password);
                    txtPassword.Text = userObj.Password;
                    txtStartDate.Text = Convert.ToDateTime(userObj.RoleExistStartDate).ToString("dd/MM/yyyy");
                    txtEndDate.Text = Convert.ToDateTime(userObj.RoleExistEndDate).ToString("dd/MM/yyyy");
                    if (userObj.IsActive == true)
                        chkIsActive.Checked = true;
                    else chkIsActive.Checked = false;

                    UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userObj.User_ID);
                    if (uapObj != null)
                    {
                        chkAllCourse.Checked = uapObj.IsAllCourse == true ? true : false;
                        string[] accessCode = uapObj.AccessPattern.Split('-');
                        foreach (string s in accessCode)
                        {
                            if (!string.IsNullOrEmpty(s))
                            {
                                Program program = ProgramManager.GetById(Convert.ToInt32(s));
                                ListItem newitem = new ListItem();
                                newitem.Text = program.ShortName;
                                newitem.Value = Convert.ToString(program.ProgramID);
                                newitem.Selected = true;
                                chkAccessList.Items.Add(newitem);
                            }
                        }
                    }

                }
            }
            catch { }
        }
        protected void btnSaveClicked(object sender, EventArgs e)
        {
            string loginID = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
            User loggedInUser = UserManager.GetByLogInId(loginID);
            string accessCode = "";
            foreach (ListItem li in chkAccessList.Items)
            {
                if (li.Selected)
                    accessCode += li.Value + "-";
            }
            try
            {
                if (string.IsNullOrEmpty(txtPassword.Text.Trim()))
                {
                    lblMsg.Text = "Please type a password.";
                    return;
                }

                Person personObj = PersonManager.GetById(Convert.ToInt32(ddlTeacher.SelectedValue));
                User userObj = personObj.Users.FirstOrDefault();
                if (userObj != null)
                {
                    userObj.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                    User testUserObj = UserManager.GetByLogInId(txtUserId.Text.Trim());
                    if (testUserObj != null && userObj.LogInID != txtUserId.Text.Trim())
                    {
                        lblMsg.Text = "Choose another Login ID. It's already taken.";
                        return;
                    }
                    userObj.LogInID = txtUserId.Text.Trim();
                    if (!string.IsNullOrEmpty(txtPassword.Text))
                        userObj.Password = txtPassword.Text;
                    userObj.RoleExistStartDate = string.IsNullOrEmpty(txtStartDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", null);
                    userObj.RoleExistEndDate = string.IsNullOrEmpty(txtEndDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", null);
                    userObj.IsActive = chkIsActive.Checked == true ? true : false;
                    userObj.ModifiedBy = loggedInUser.User_ID;
                    userObj.ModifiedDate = DateTime.Now;
                    bool isUpdated = UserManager.Update(userObj);
                    if (isUpdated)
                        lblMsg.Text = "Updated";

                    UserAccessProgram uap = UserAccessProgramManager.GetByUserId(userObj.User_ID);
                    if (uap != null)
                    {
                        uap.AccessPattern = accessCode;
                        uap.AccessStartDate = string.IsNullOrEmpty(txtStartDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", null);
                        uap.AccessEndDate = string.IsNullOrEmpty(txtEndDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", null);
                        uap.IsAllCourse = chkAllCourse.Checked == true ? true : false;
                        uap.ModifiedBy = loggedInUser.User_ID;
                        uap.ModifiedDate = DateTime.Now;
                        UserAccessProgramManager.Update(uap);
                    }
                    else
                    {
                        UserAccessProgram uapObj = new UserAccessProgram();
                        uapObj.User_ID = userObj.User_ID;
                        uapObj.AccessPattern = accessCode;
                        uapObj.AccessStartDate = string.IsNullOrEmpty(txtStartDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", null);
                        uapObj.AccessEndDate = string.IsNullOrEmpty(txtEndDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", null);
                        uapObj.IsAllCourse = chkAllCourse.Checked == true ? true : false;
                        uapObj.CreatedBy = loggedInUser.User_ID;
                        uapObj.CreatedDate = DateTime.Now;
                        uapObj.ModifiedBy = loggedInUser.User_ID;
                        uapObj.ModifiedDate = DateTime.Now;
                        UserAccessProgramManager.Insert(uapObj);
                    }
                    //EmployeeOfficeRelation empOffObj = EmployeeOfficeRelationManager.GetByPersonId(personObj.PersonID);
                    //if (empOffObj != null)
                    //{
                    //    empOffObj.OfficeId = Convert.ToInt32(ddlOffice.SelectedValue);
                    //    empOffObj.ModifiedBy = loggedInUser.User_ID;
                    //    empOffObj.ModifiedDate = DateTime.Now;
                    //    EmployeeOfficeRelationManager.Update(empOffObj);
                    //}
                    //else
                    //{
                    //    EmployeeOfficeRelation employeOffice = new EmployeeOfficeRelation();
                    //    employeOffice.OfficeId = Convert.ToInt32(ddlOffice.SelectedValue);
                    //    employeOffice.PersonId = personObj.PersonID;
                    //    employeOffice.EmployeeId = personObj.Employee.EmployeeID;
                    //    employeOffice.CreatedBy = loggedInUser.User_ID;
                    //    employeOffice.CreatedDate = DateTime.Now;
                    //    employeOffice.ModifiedBy = loggedInUser.User_ID;
                    //    employeOffice.ModifiedDate = DateTime.Now;
                    //    EmployeeOfficeRelationManager.Insert(employeOffice);
                    //}
                }
                else
                {
                    User user = UserManager.GetByLogInId(txtUserId.Text.Trim());
                    if (user != null)
                    {
                        lblMsg.Text = "Choose another Login ID. It's already taken.";
                        return;
                    }

                    BussinessObject.UIUMSUser newUser = new BussinessObject.UIUMSUser();
                    newUser.Id = 0;
                    newUser.RoleID = Convert.ToInt32(ddlRole.SelectedValue);
                    newUser.LogInID = txtUserId.Text.Trim();
                    newUser.Password = txtPassword.Text;
                    newUser.RoleExistStartDate = string.IsNullOrEmpty(txtStartDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", null);
                    newUser.RoleExistEndDate = string.IsNullOrEmpty(txtEndDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", null);
                    newUser.IsActive = chkIsActive.Checked == true ? true : false;
                    newUser.CreatorID = loggedInUser.User_ID;
                    newUser.CreatedDate = DateTime.Now;
                    newUser.ModifiedDate = DateTime.Now;
                    newUser.ModifierID = loggedInUser.User_ID;

                    int userId = BussinessObject.UIUMSUser.Save(newUser);
                    if (userId > 0)
                    {
                        lblMsg.Text = "Saved!";
                        user = UserManager.GetByLogInId(txtUserId.Text.Trim());

                        UserInPerson uip = new UserInPerson();
                        uip.PersonID = personObj.PersonID;
                        uip.User_ID = user.User_ID;
                        uip.CreatedBy = loggedInUser.User_ID;
                        uip.CreatedDate = DateTime.Now;
                        uip.ModifiedBy = loggedInUser.User_ID;
                        uip.ModifiedDate = DateTime.Now;
                        UserInPersonManager.Insert(uip);

                        UserAccessProgram uap = new UserAccessProgram();
                        uap.User_ID = user.User_ID;
                        uap.AccessPattern = accessCode;
                        uap.AccessStartDate = string.IsNullOrEmpty(txtStartDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtStartDate.Text.Trim(), "dd/MM/yyyy", null);
                        uap.AccessEndDate = string.IsNullOrEmpty(txtEndDate.Text.Trim()) ? DateTime.Now : DateTime.ParseExact(txtEndDate.Text.Trim(), "dd/MM/yyyy", null);
                        uap.IsAllCourse = chkAllCourse.Checked == true ? true : false;
                        uap.CreatedBy = loggedInUser.User_ID;
                        uap.CreatedDate = DateTime.Now;
                        uap.ModifiedBy = loggedInUser.User_ID;
                        uap.ModifiedDate = DateTime.Now;
                        UserAccessProgramManager.Insert(uap);

                        //EmployeeOfficeRelation empOffObj = EmployeeOfficeRelationManager.GetByPersonId(personObj.PersonID);
                        //if (empOffObj != null)
                        //{
                        //    empOffObj.OfficeId = Convert.ToInt32(ddlOffice.SelectedValue);
                        //    empOffObj.ModifiedBy = loggedInUser.User_ID;
                        //    empOffObj.ModifiedDate = DateTime.Now;
                        //    EmployeeOfficeRelationManager.Update(empOffObj);
                        //}
                        //else
                        //{
                        //    EmployeeOfficeRelation employeOffice = new EmployeeOfficeRelation();
                        //    employeOffice.OfficeId = Convert.ToInt32(ddlOffice.SelectedValue);
                        //    employeOffice.PersonId = personObj.PersonID;
                        //    employeOffice.EmployeeId = personObj.Employee.EmployeeID;
                        //    employeOffice.CreatedBy = loggedInUser.User_ID;
                        //    employeOffice.CreatedDate = DateTime.Now;
                        //    employeOffice.ModifiedBy = loggedInUser.User_ID;
                        //    employeOffice.ModifiedDate = DateTime.Now;
                        //    EmployeeOfficeRelationManager.Insert(employeOffice);
                        //}
                    }
                }
            }
            catch { }
        }

    }
}