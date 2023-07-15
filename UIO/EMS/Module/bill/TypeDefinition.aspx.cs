using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using CommonUtility;
using System.Drawing;

namespace EMS.Module.bill
{
    public partial class TypeDefinition : BasePage
    {
        int userId = 0;
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.TypeDefinition);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.TypeDefinition));

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            string loginID = GetFromSession(Constants.SESSIONCURRENT_LOGINID).ToString();
            User user = UserManager.GetByLogInId(loginID);
            if (user != null)
                userId = user.User_ID;
            //pnlMessage.Visible = false;

            if (!IsPostBack)
            {
                FillList();
                LoadFundTypeDropdownList();
            }
        }
        
        public void LoadFundTypeDropdownList()
        {
            var fundTypeList = FundTypeManager.GetAll();

            fundTypeDropDownList.Items.Add(new ListItem("Select", "-1"));
            fundTypeDropDownList.AppendDataBoundItems = true;

            if (fundTypeList.Any())
            {
                fundTypeDropDownList.DataValueField = "FundTypeId";
                fundTypeDropDownList.DataTextField = "FundName";
                fundTypeDropDownList.DataSource = fundTypeList;
                fundTypeDropDownList.DataBind();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            saveButton.Visible = true;
            btnUpdate.Visible = false;
            btnCancel.Visible = false;
        }

        private void FillList()
        {
            List<LogicLayer.BusinessObjects.TypeDefinition> typeList = TypeDefinitionManager.GetAll().OrderBy(td => td.Type).ThenBy(m => m.Definition).ToList();
            gvwCollection.DataSource = typeList;
            gvwCollection.DataBind();
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            try
            {
                saveButton.Visible = false;
                btnUpdate.Visible = true;
                btnCancel.Visible = true;
                LinkButton btn = (LinkButton)sender;
                int id = int.Parse(btn.CommandArgument.ToString());

                LogicLayer.BusinessObjects.TypeDefinition typeDefinitionObj = new LogicLayer.BusinessObjects.TypeDefinition();
                typeDefinitionObj = TypeDefinitionManager.GetById(id);
                hdnTypeDefinitionId.Value = Convert.ToString(id);
                ddlType.Text = typeDefinitionObj.Type;
                txtDefinition.Text = typeDefinitionObj.Definition;
                fundTypeDropDownList.SelectedIndex = typeDefinitionObj.FundTypeId;
                //ddlAccHead.SelectedValue = Convert.ToString(typeDefinitionObj.AccountsID);
                //txtPriority.Text = Convert.ToString(typeDefinitionObj.Priority);
                chkIsLifetimeOnceBilling.Checked = Convert.ToBoolean(typeDefinitionObj.IsLifetimeOnce);
                chkIsCourseSpecificBilling.Checked = Convert.ToBoolean(typeDefinitionObj.IsCourseSpecific);
                chkIsPerAcaCalBilling.Checked = Convert.ToBoolean(typeDefinitionObj.IsPerAcaCal);
                chkIsAnnualBilling.Checked = Convert.ToBoolean(typeDefinitionObj.IsAnnual);
                //chkIsReportEnableField.Checked = Convert.ToBoolean(typeDefinitionObj.IsReportEnableField);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)sender;
                int id = int.Parse(btn.CommandArgument.ToString());
                TypeDefinitionManager.Delete(id);
                #region Log Insert
                try
                {
                    LogGeneralManager.Insert(
                       DateTime.Now,
                       "",
                       BaseAcaCalCurrent.FullCode,
                       BaseCurrentUserObj.LogInID,
                       "",
                       "",
                       "Type Definition Setup",
                       BaseCurrentUserObj.LogInID + " Deleted ",
                       "normal",
                       _pageId,
                       _pageName,
                       _pageUrl,
                       "");
                }
                catch (Exception ex) { }

                #endregion
                FillList();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int fundTypeId = Convert.ToInt32(fundTypeDropDownList.SelectedValue);
                if (fundTypeId == -1)
                {
                    lblMsg.Text = "Select fund type";
                    return;
                }
                int id = Convert.ToInt32(hdnTypeDefinitionId.Value);

                LogicLayer.BusinessObjects.TypeDefinition typeDefinitionObj =
                    new LogicLayer.BusinessObjects.TypeDefinition
                    {
                        TypeDefinitionID = id,
                        Type = ddlType.SelectedValue,
                        Definition = txtDefinition.Text,
                        FundTypeId = fundTypeId,
                        IsPerAcaCal = chkIsPerAcaCalBilling.Checked,
                        IsCourseSpecific = chkIsCourseSpecificBilling.Checked,
                        IsLifetimeOnce = chkIsLifetimeOnceBilling.Checked,
                        IsAnnual = chkIsAnnualBilling.Checked,
                        ModifiedBy = BaseCurrentUserObj.Id,
                        ModifiedDate = DateTime.Now
                    };
                //typeDefinitionObj = TypeDefinitionManager.GetById(id);
                //typeDefinitionObj.AccountsID = Convert.ToInt32(ddlAccHead.SelectedValue);
                //typeDefinitionObj.Priority = string.IsNullOrEmpty(txtPriority.Text) ? 0 : Convert.ToInt32(txtPriority.Text);
                //typeDefinitionObj.IsReportEnableField = chkIsReportEnableField.Checked;

                bool result = TypeDefinitionManager.Update(typeDefinitionObj);

                if (result)
                {
                    FillList();
                    lblMsg.Text = "Type definition edited successfully";
                    #region Log Insert
                    try
                    {
                        LogGeneralManager.Insert(
                           DateTime.Now,
                           "",
                           BaseAcaCalCurrent.FullCode,
                           BaseCurrentUserObj.LogInID,
                           "",
                           "",
                           "Type Definition Setup",
                           BaseCurrentUserObj.LogInID + " edited Type: " + ddlType.Text + ", Definition: " + txtDefinition.Text,
                           "normal",
                           _pageId,
                           _pageName,
                           _pageUrl,
                           "");
                    }
                    catch (Exception ex) { }

                    #endregion
                    saveButton.Visible = true;
                    btnUpdate.Visible = false;
                    btnCancel.Visible = false;
                }
                else
                {
                    lblMsg.Text = "Type definition could not edited successfully";
                    #region Log Insert
                    try
                    {
                        LogGeneralManager.Insert(
                           DateTime.Now,
                           "",
                           BaseAcaCalCurrent.FullCode,
                           BaseCurrentUserObj.LogInID,
                           "",
                           "",
                           "Type Definition Setup",
                           BaseCurrentUserObj.LogInID + " attempted to edit Type: " + ddlType.Text + ", Definition: " + txtDefinition.Text,
                           "normal",
                           _pageId,
                           _pageName,
                           _pageUrl,
                           "");
                    }
                    catch (Exception ex) { }

                    #endregion
                    saveButton.Visible = false;
                    btnUpdate.Visible = true;
                    btnCancel.Visible = true;
                }
            }
            catch (Exception ex) { }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
            //    txtDefinition.Text = string.Empty;
            //    LoadFundTypeDropdownList();

            //    //txtPriority.Text = string.Empty;
            //    chkIsLifetimeOnceBilling.Checked = false;
            //    chkIsCourseSpecificBilling.Checked = false;
            //    chkIsPerAcaCalBilling.Checked = false;
            //    chkIsAnnualBilling.Checked = false;
        }

        public void Clear()
        {
            txtDefinition.Text = string.Empty;
            LoadFundTypeDropdownList();

            //txtPriority.Text = string.Empty;
            chkIsLifetimeOnceBilling.Checked = false;
            chkIsCourseSpecificBilling.Checked = false;
            chkIsPerAcaCalBilling.Checked = false;
            chkIsAnnualBilling.Checked = false;
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {

        }

        protected void saveButton_OnClick(object sender, EventArgs e)
        {
            try
            {
                lblMsg.Text = String.Empty;
                int fundTypeId = Convert.ToInt32(fundTypeDropDownList.SelectedValue);
                if (fundTypeId == -1)
                {
                    lblMsg.Text = "Select fund type";
                    return;
                }

                LogicLayer.BusinessObjects.TypeDefinition typeDefinitionObj = new LogicLayer.BusinessObjects.TypeDefinition();
                typeDefinitionObj.Type = ddlType.SelectedValue;
                typeDefinitionObj.Definition = txtDefinition.Text;
                typeDefinitionObj.FundTypeId = fundTypeId;
                //typeDefinitionObj.AccountsID = Convert.ToInt32(ddlAccHead.SelectedValue);
                //typeDefinitionObj.Priority = string.IsNullOrEmpty(txtPriority.Text) ? 0 : Convert.ToInt32(txtPriority.Text);
                typeDefinitionObj.IsPerAcaCal = chkIsPerAcaCalBilling.Checked;
                typeDefinitionObj.IsCourseSpecific = chkIsCourseSpecificBilling.Checked;
                typeDefinitionObj.IsLifetimeOnce = chkIsLifetimeOnceBilling.Checked;
                typeDefinitionObj.IsAnnual = chkIsAnnualBilling.Checked;
                //typeDefinitionObj.IsReportEnableField = chkIsReportEnableField.Checked;
                typeDefinitionObj.CreatedBy = BaseCurrentUserObj.Id;
                typeDefinitionObj.CreatedDate = DateTime.Now;
                //typeDefinitionObj.ModifiedBy = BaseCurrentUserObj.Id;
                //typeDefinitionObj.ModifiedDate = DateTime.Now;
                var typeDefinitionList = TypeDefinitionManager.GetAll();
                var a = typeDefinitionList.Any(m => m.Definition == txtDefinition.Text);
                if (a)
                {
                    lblMsg.Text = "Type definition already exist. You can not insert same definition.";
                    AlertMessage(lblMsg.Text);
                    Clear();
                    return;
                }
                int result = TypeDefinitionManager.Insert(typeDefinitionObj);

                if (result > 0)
                {

                    lblMsg.Text = "Type definition created successfully";
                    FillList();
                    #region Log Insert
                    try
                    {
                        LogGeneralManager.Insert(
                           DateTime.Now,
                           "",
                           BaseAcaCalCurrent.FullCode,
                           BaseCurrentUserObj.LogInID,
                           "",
                           "",
                           "Type Definition Setup",
                           BaseCurrentUserObj.LogInID + " saved Type: " + ddlType.Text + ", Definition: " + txtDefinition.Text,
                           "normal",
                           _pageId,
                           _pageName,
                           _pageUrl,
                           "");
                    }
                    catch (Exception ex) { }

                    #endregion
                }
                else
                {
                    lblMsg.Text = "Type definition could not created successfully";
                    #region Log Insert
                    try
                    {
                        LogGeneralManager.Insert(
                           DateTime.Now,
                           "",
                           BaseAcaCalCurrent.FullCode,
                           BaseCurrentUserObj.LogInID,
                           "",
                           "",
                           "Type Definition Setup",
                           BaseCurrentUserObj.LogInID + " attempted to save Type: " + ddlType.Text + ", Definition: " + txtDefinition.Text,
                           "normal",
                           _pageId,
                           _pageName,
                           _pageUrl,
                           "");
                    }
                    catch (Exception ex) { }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        public void AlertMessage(string message)
        {
            //ScriptManager.RegisterClientScriptBlock(page, page.GetType(), "alertMessage", "alert('" + message + "')", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + message + "')", true);
        }

    }
}