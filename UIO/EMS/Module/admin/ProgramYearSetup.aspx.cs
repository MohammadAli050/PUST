using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ProgramYearSetup : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            BaseCurrentUserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);
            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
            }
        }

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropDownList();
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                ddlLoadDepartment();
                ucProgram2.LoadDropDownList();
                ucProgram2.LoadDropdownByDepartmentId(departmentId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ddlLoadDepartment()
        {
            List<Department> departmentList = new List<Department>();
            departmentList = DepartmentManager.GetAll();

            ddlDepartment.Items.Clear();
            ddlDepartment.AppendDataBoundItems = true;

            if (departmentList != null)
            {
                departmentList = departmentList.OrderBy(o => o.DeptID).ToList();
                ddlDepartment.Items.Add(new ListItem("-Select Department-", "0"));
                ddlDepartment.DataTextField = "Name";
                ddlDepartment.DataValueField = "DeptID";

                ddlDepartment.DataSource = departmentList;
                ddlDepartment.DataBind();

                ddlDepartment.SelectedValue = departmentList.FirstOrDefault().DeptID.ToString();
            }
        }

        protected void ucDepartment2_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
            try
            {
                lblMsg.Text = string.Empty;
                int departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                ucProgram2.LoadDropdownByDepartmentId(departmentId);
                //LoadAdmissionSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ucProgram2_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
            try
            {
                //ClearGridView();
                lblMsg.Text = string.Empty;
                int programId = 0; //Convert.ToInt32(ucProgram2.selectedValue);
                if (programId == -1)
                {
                    return;
                }
                //LoadAdmissionSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            List<Year> yearList = YearManager.GetByProgramId(programId);
            if (yearList!= null)
            {
                gvYearList.DataSource = yearList;
                gvYearList.DataBind();
            }
        }

        protected void btnAddYear_Click(object sender, EventArgs e)
        {
            ModalPopupExtender2.Show();
        }

        protected void btnAddNewYear_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
            try
            {
                int departmentId = Convert.ToInt32(ddlDepartment.SelectedValue);
                int programId = Convert.ToInt32(ucProgram2.selectedValue);
                string yearName = Convert.ToString(txtYear.Text.Trim());
                if (departmentId > 0 && programId > 0 && !string.IsNullOrEmpty(yearName))
                {
                    Year yearObj = new Year();
                    yearObj.ProgramId = programId;
                    yearObj.DepartmentId = departmentId;
                    yearObj.YearName = yearName;
                    yearObj.CreatedBy = BaseCurrentUserObj.Id;
                    yearObj.CreatedDate = DateTime.Now;
                    yearObj.ModifiedBy = BaseCurrentUserObj.Id;
                    yearObj.ModifiedDate = DateTime.Now;

                    bool isExist = YearManager.IsYearNameExist(programId, yearName);
                    if (!isExist)
                    {
                        int result = YearManager.Insert(yearObj);
                        if (result > 0)
                        {
                            lblMessage.Text = "New year created succesfully.";
                        }
                    }
                    else { lblMessage.Text = "Provided year already exist for this program."; }
                }
                btnLoad_Click(null, null);
            }
            catch (Exception)
            {
                throw;
            }
        }
       
    }
}