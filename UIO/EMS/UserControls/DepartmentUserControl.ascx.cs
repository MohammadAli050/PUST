
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.UserControls
{
    public partial class DepartmentUserControl : System.Web.UI.UserControl
    {
        public event EventHandler DepartmentSelectedIndexChanged;
        public string selectedValue = string.Empty;
        public string selectedText = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    //LoadDropDownList();               
                }
                selectedValue = ddlDepartment.SelectedValue;
                selectedText = ddlDepartment.SelectedItem.Text;
            }
            catch (Exception)
            {
            }
        }

        public void LoadDropDownListWithoutSelectionFirstDept()
        {
            List<Department> departmentList = new List<Department>();
            departmentList = DepartmentManager.GetAll();

            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;

            if (departmentList != null)
            {
                departmentList = departmentList.OrderBy(o => o.DetailedName).ToList();
                ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));
                ddlDepartment.DataTextField = "DetailedName";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                //ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                //selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                //selectedText = departmentList.FirstOrDefault().Name;
            }
        }

        public void LoadDropDownList()
        {
            List<Department> departmentList = new List<Department>();
            departmentList = DepartmentManager.GetAll();

            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;

            if (departmentList != null)
            {
                departmentList = departmentList.OrderBy(o => o.DetailedName).ToList();
                ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));
                ddlDepartment.DataTextField = "DetailedName";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedText = departmentList.FirstOrDefault().Name;
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValue = ddlDepartment.SelectedValue;
            selectedText = ddlDepartment.SelectedItem.Text;

            if (DepartmentSelectedIndexChanged != null)
                DepartmentSelectedIndexChanged(this, e);
        }

        internal void SelectedValue(int id)
        {
            List<Department> departmentList = new List<Department>();
            departmentList = DepartmentManager.GetAll();

            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;

            if (departmentList != null)
            {
                departmentList = departmentList.OrderBy(o => o.DetailedName).ToList();
                ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));
                ddlDepartment.DataTextField = "DetailedName";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                ddlDepartment.SelectedValue = id.ToString();
                selectedValue = ddlDepartment.SelectedValue;
                selectedText = ddlDepartment.SelectedItem.Text;
            }
        }

        internal void SelectedIndex(int id)
        {
            ddlDepartment.SelectedIndex = id;
            selectedValue = ddlDepartment.SelectedValue;
            selectedText = ddlDepartment.SelectedItem.Text;
        }

        internal void Enabled(bool TrueOrFalse)
        {
            ddlDepartment.Enabled = TrueOrFalse;
        }

        public void LoadDropdownWithUserAccess(int userID)
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;
            try
            {
                UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userID);
                if (uapObj != null && !string.IsNullOrEmpty(uapObj.AccessPattern))
                {
                    List<Department> departmentList = new List<Department>();
                    List<Program> programList = new List<Program>();
                    string[] accessCode = uapObj.AccessPattern.Split('-');
                    foreach (string s in accessCode)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            Program program = ProgramManager.GetById(Convert.ToInt32(s));
                            programList.Add(program);
                        }
                    }
                    if (programList != null)
                    {
                        List<int> departmentId = programList.Select(d => d.DeptID).Distinct().ToList();
                        //programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();
                        for (int i = 0; i < departmentId.Count; i++)
                        {
                            Department department = DepartmentManager.GetById(departmentId[i]);
                            departmentList.Add(department);
                        }
                    }

                    ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));
                    ddlDepartment.DataTextField = "DetailedName";
                    ddlDepartment.DataValueField = "DeptID";

                    ddlDepartment.DataSource = departmentList;
                    ddlDepartment.DataBind();

                    ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                    selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                    selectedText = departmentList.FirstOrDefault().Name;
                }
                else
                {
                    //LoadDropDownList();
                }
            }
            catch { LoadDropDownList(); }

        }

        #region With All Selected Option

        public void LoadDropDownListWithAllOption()
        {
            List<Department> departmentList = new List<Department>();
            departmentList = DepartmentManager.GetAll();

            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;
            ddlDepartment.Items.Add(new ListItem("-All-", "0"));

            if (departmentList != null && departmentList.Any())
            {
                departmentList = departmentList.OrderBy(o => o.DetailedName).ToList();
                ddlDepartment.DataTextField = "DetailedName";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedText = departmentList.FirstOrDefault().Name;
            }
        }

        public void LoadDropdownWithUserAccessWithAllOption(int userID)
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;
            try
            {
                UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userID);
                if (uapObj != null && !string.IsNullOrEmpty(uapObj.AccessPattern))
                {
                    List<Department> departmentList = new List<Department>();
                    List<Program> programList = new List<Program>();
                    string[] accessCode = uapObj.AccessPattern.Split('-');
                    foreach (string s in accessCode)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            Program program = ProgramManager.GetById(Convert.ToInt32(s));
                            programList.Add(program);
                        }
                    }
                    if (programList != null && programList.Any())
                    {
                        List<int> departmentId = programList.Select(d => d.DeptID).Distinct().ToList();
                        for (int i = 0; i < departmentId.Count; i++)
                        {
                            Department department = DepartmentManager.GetById(departmentId[i]);
                            departmentList.Add(department);
                        }
                    }

                    ddlDepartment.Items.Add(new ListItem("-All-", "0"));
                    ddlDepartment.DataTextField = "DetailedName";
                    ddlDepartment.DataValueField = "DeptID";

                    ddlDepartment.DataSource = departmentList;
                    ddlDepartment.DataBind();

                    ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                    selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                    selectedText = departmentList.FirstOrDefault().Name;
                }
                else
                {
                    //LoadDropDownList();
                }
            }
            catch { LoadDropDownList(); }

        }

        #endregion


        public void LoadDropDownByUserIdWithClassTakenAccess(int userID)
        {
            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;
            ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));

            try
            {

                User user = UserManager.GetById(userID);
                if (user != null)
                {
                    if (user.RoleID != 1 && user.RoleID != 2 && user.RoleID != 8)
                    {
                        Employee empObj = EmployeeManager.GetByPersonId(user.Person.PersonID);
                        if (empObj != null)
                        {
                            List<SqlParameter> parameters1 = new List<SqlParameter>();
                            parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                            parameters1.Add(new SqlParameter { ParameterName = "@EmployeeId", SqlDbType = System.Data.SqlDbType.Int, Value = empObj.EmployeeID });

                            DataTable departmentList = DataTableManager.GetDataFromQuery("GetAllCourseListByEmployeeIdAndHeldInId", parameters1);
                            if (departmentList != null && departmentList.Rows.Count > 0)
                            {
                                DataTable distinctdepartment = departmentList.DefaultView.ToTable(true, "DeptID", "DetailedName");

                                ddlDepartment.DataTextField = "DetailedName";
                                ddlDepartment.DataValueField = "DeptID";

                                ddlDepartment.DataSource = distinctdepartment;
                                ddlDepartment.DataBind();
                            }
                        }
                    }
                    else
                        LoadDropDownList();
                }
            }
            catch (Exception ex) { LoadDropDownList(); }

        }


        public void LoadDropDownListWithUserAccess(int UserId, int RoleId)
        {
            List<Department> departmentList = new List<Department>();
            List<Department> tempdepartmentList = DepartmentManager.GetAll();


            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;
            ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));


            #region MyRegion

            if (RoleId != 1 && RoleId != 8) // Admin And COE
            {
                List<Program> programList = new List<Program>();

                UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(UserId);
                if (uapObj != null && !string.IsNullOrEmpty(uapObj.AccessPattern))
                {
                    string[] accessCode = uapObj.AccessPattern.Split('-');
                    foreach (string s in accessCode)
                    {
                        if (!string.IsNullOrEmpty(s))
                        {
                            Program program = ProgramManager.GetById(Convert.ToInt32(s));
                            programList.Add(program);
                        }
                    }
                }
                if (programList != null && programList.Any())
                {
                    foreach (var item in programList)
                    {
                        var Dept = tempdepartmentList.Where(x => x.DeptID == item.DeptID).FirstOrDefault();
                        var AlreadyExists = departmentList.Where(x => x.DeptID == item.DeptID).FirstOrDefault();

                        if (Dept != null && AlreadyExists == null)
                            departmentList.Add(Dept);

                    }
                }

            }
            else
                departmentList = tempdepartmentList;

            #endregion

            if (departmentList != null && departmentList.Any())
            {
                departmentList = departmentList.OrderBy(o => o.DetailedName).ToList();
                ddlDepartment.DataTextField = "DetailedName";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedText = departmentList.FirstOrDefault().Name;
            }
        }



        public void LoadDropDownListFromExamCommitteeWithUserAccess(int UserId, int RoleId)
        {
            List<Department> departmentList = new List<Department>();
            List<Department> tempdepartmentList = DepartmentManager.GetAll();


            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;
            ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));


            #region MyRegion

            if (RoleId != 1 && RoleId != 8) // Admin And COE
            {
                var usr = UserManager.GetById(UserId);
                if (usr != null)
                {
                    var Emp = EmployeeManager.GetByPersonId(usr.Person.PersonID);
                    if (Emp != null)
                    {

                        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

                        var list = ucamContext.ExamSetupWithExamCommittees.Where(x => x.ExamCommitteeChairmanId == Emp.EmployeeID).ToList();
                        if (list != null && list.Any())
                        {
                            foreach (var item in list)
                            {
                                var Dept = tempdepartmentList.Where(x => x.DeptID == item.ExamCommitteeChairmanDeptId).FirstOrDefault();
                                var AlreadyExists = departmentList.Where(x => x.DeptID == item.ExamCommitteeChairmanDeptId).FirstOrDefault();

                                if (Dept != null && AlreadyExists == null)
                                    departmentList.Add(Dept);
                            }
                        }
                    }
                }

            }
            else
                departmentList = tempdepartmentList;

            #endregion

            if (departmentList != null && departmentList.Any())
            {
                departmentList = departmentList.OrderBy(o => o.DetailedName).ToList();
                ddlDepartment.DataTextField = "DetailedName";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedValue = departmentList.FirstOrDefault().DeptID.ToString();
                selectedText = departmentList.FirstOrDefault().Name;
            }
        }
    }
}