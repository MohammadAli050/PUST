using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;

namespace EMS.Module.admin
{
    public partial class BatchSetup : BasePage
    {
        User user = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                base.CheckPage_Load();
                string loginId = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
                user = UserManager.GetByLogInId(loginId);
                if (!IsPostBack && !IsCallback)
                {
                    ucDepartment.LoadDropDownList();
                    ddlLoadDepartment();
                    int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                    
                    ucProgram.LoadDropDownList();
                    ucProgram.LoadDropdownByDepartmentId(departmentId);
                }
            }
            catch (Exception) { }
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

        protected void ucDepartment_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = string.Empty;
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                //ucProgram2.LoadDropdownByDepartmentId(departmentId);
                ////LoadAdmissionSessionDropDownList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //ClearGridView();
                lblMsg.Text = string.Empty;
                int programId = Convert.ToInt32(ucProgram.selectedValue);
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
        //            admissionSessionDropDownList.DataTextField = "Code";
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

        protected void loadButton_Click(object sender, EventArgs e)
        {
            try
            {
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int acacalId = Convert.ToInt32(ucAdmissionSession.selectedValue);

                if (programId > 0) 
                {
                    List<Batch> batchList = BatchManager.GetAllByProgramIdAcacalId(programId, acacalId);
                    if(batchList.Count > 0)
                    {
                        gvBatch.DataSource = batchList;
                        gvBatch.DataBind();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
            try
            {
                lblMsg.Text = string.Empty;
                txtBatchNo.Text = string.Empty;
            }
            catch (Exception)
            {
                throw;
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

        protected void ucAdmissionSession2_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
            Batch batchObj = new Batch();
            if (string.IsNullOrEmpty(lblBatchId.Text))
            {
                batchObj.ProgramId = Convert.ToInt32(ucProgram2.selectedValue);
                batchObj.AcaCalId = Convert.ToInt32(ucAdmissionSession2.selectedValue);
                batchObj.BatchNO = Convert.ToInt32(txtBatchNo.Text.Trim());
                batchObj.CreatedBy = BaseCurrentUserObj.Id;
                batchObj.CreatedDate = DateTime.Now;
                batchObj.ModifiedBy = BaseCurrentUserObj.Id;
                batchObj.ModifiedDate = DateTime.Now;

                int batchId = BatchManager.Insert(batchObj);
                if (batchId > 0)
                {
                    lblMessage.Text = "Batch created successfully.";
                }
                else
                {
                    lblMessage.Text = "Batch could not created successfully.";
                }
            }
            else 
            {
                int batchId = Convert.ToInt32(lblBatchId.Text);
                if (batchId > 0)
                {
                    batchObj = BatchManager.GetById(batchId);
                    if (batchObj != null)
                    {
                        batchObj.ProgramId = Convert.ToInt32(ucProgram2.selectedValue);
                        batchObj.AcaCalId = Convert.ToInt32(ucAdmissionSession2.selectedValue);
                        batchObj.BatchNO = Convert.ToInt32(txtBatchNo.Text.Trim());
                        batchObj.CreatedBy = BaseCurrentUserObj.Id;
                        batchObj.CreatedDate = DateTime.Now;
                        batchObj.ModifiedBy = BaseCurrentUserObj.Id;
                        batchObj.ModifiedDate = DateTime.Now;

                        bool result = BatchManager.Update(batchObj);
                        if (result)
                        {
                            lblMessage.Text = "Batch edited successfully.";
                        }
                        else
                        {
                            lblMessage.Text = "Batch could not edited successfully.";
                        }
                    }
                }
            }
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            this.ModalPopupExtender2.Show();
            LinkButton btn = (LinkButton)sender;
            int batchId = int.Parse(btn.CommandArgument.ToString());

            if (batchId > 0)
            {
                Batch batchObj = BatchManager.GetById(batchId);
                if (batchObj!= null) 
                {
                    lblBatchId.Text = Convert.ToString(batchId);
                    int departmentId = ProgramManager.GetById(batchObj.ProgramId).DeptID;
                    ddlLoadDepartment();
                    ddlDepartment.SelectedValue = Convert.ToString(departmentId);
                    ucProgram2.LoadDropDownList();
                    ucProgram2.LoadDropdownByDepartmentId(departmentId);
                    ucProgram2.selectedValue = Convert.ToString(batchObj.ProgramId);
                    ucAdmissionSession2.LoadDropDownList();
                    ucAdmissionSession2.selectedValue = Convert.ToString(batchObj.AcaCalId);
                    txtBatchNo.Text = Convert.ToString(batchObj.BatchNO);
                }
            }
        }
    }
}