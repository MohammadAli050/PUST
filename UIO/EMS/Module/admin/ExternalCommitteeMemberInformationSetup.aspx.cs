using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ExternalCommitteeMemberInformationSetup : BasePage
    {

        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.ExternalCommitteeMemberInformationSetup);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.ExternalCommitteeMemberInformationSetup));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                pnlAddAndUpdate.Visible = false;
                LoadExternalMemberInformation();
            }
        }

        private void LoadExternalMemberInformation()
        {
            try
            {
                pnlAddAndUpdate.Visible = false;
                Session["MemberList"] = null;

                var MemberList = ucamContext.ExternalCommitteeMemberInformations.ToList();

                if (MemberList != null && MemberList.Any())
                {
                    gvExternalList.DataSource = MemberList;
                    gvExternalList.DataBind();

                    Session["MemberList"] = MemberList;

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            try
            {
                clearField();
                pnlAddAndUpdate.Visible = !pnlAddAndUpdate.Visible;
                btnAddUpdate.Text = "Click here to Save Info";
                hdnSetupId.Value = "0";

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnAddUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                int SetupId = Convert.ToInt32(hdnSetupId.Value);

                string Name = txtName.Text.Trim();
                string DeptName = txtDepartment.Text.Trim();
                string DesgName = txtDesignation.Text.Trim();
                string Phone = txtPhone.Text.Trim();
                string Email = txtEmail.Text.Trim();
                string University = txtUniversity.Text.Trim();

                if (Name == "" || DeptName == "" || DesgName == "" || Phone == "" || Email == "" || University == "")
                {
                    showAlert("Please Enter All Values");
                    return;
                }

                var IsMemberFound = ucamContext.ExternalCommitteeMemberInformations.AsNoTracking().Where(x => x.Name == Name && x.Phone == Phone && x.Department == DeptName
                       && x.Designation == DesgName && x.Email == Email && x.UniversityName == University && x.ExternalId != SetupId).FirstOrDefault();

                if (IsMemberFound != null)
                {
                    showAlert("External member already exists");
                    return;
                }

                if (SetupId == 0) // Insert New Member
                {

                    DAL.ExternalCommitteeMemberInformation NewObj = new DAL.ExternalCommitteeMemberInformation();
                    NewObj.Name = Name;
                    NewObj.Department = DeptName;
                    NewObj.Designation = DesgName;
                    NewObj.Phone = Phone;
                    NewObj.Email = Email;
                    NewObj.UniversityName = University;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;

                    ucamContext.ExternalCommitteeMemberInformations.Add(NewObj);
                    ucamContext.SaveChanges();

                    if (NewObj.ExternalId > 0)
                    {
                        showAlert("External Member Information Added Successfully");
                        LoadExternalMemberInformation();
                        clearField();
                        pnlAddAndUpdate.Visible = true;
                        return;
                    }


                }
                else// Update Existing Member
                {
                    var ExistingObj = ucamContext.ExternalCommitteeMemberInformations.Find(SetupId);

                    if (ExistingObj != null)
                    {
                        ExistingObj.Name = Name;
                        ExistingObj.Department = DeptName;
                        ExistingObj.Designation = DesgName;
                        ExistingObj.Phone = Phone;
                        ExistingObj.Email = Email;
                        ExistingObj.UniversityName = University;
                        ExistingObj.ModifiedBy = UserObj.Id;
                        ExistingObj.ModifiedDate = DateTime.Now;

                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        showAlert("External Member Information Updated Successfully");
                        LoadExternalMemberInformation();
                        pnlAddAndUpdate.Visible = false;
                        return;

                    }

                }

            }
            catch (Exception ex)
            {
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            pnlAddAndUpdate.Visible = false;
        }



        protected void EditMember_Click(object sender, EventArgs e)
        {
            try
            {
                clearField();
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.ExternalCommitteeMemberInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        pnlAddAndUpdate.Visible = true;
                        hdnSetupId.Value = SetupId.ToString();


                        txtName.Text = ExistingObj.Name;
                        txtDepartment.Text = ExistingObj.Department;
                        txtDesignation.Text = ExistingObj.Designation;
                        txtPhone.Text = ExistingObj.Phone;
                        txtEmail.Text = ExistingObj.Email;
                        txtUniversity.Text = ExistingObj.UniversityName;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void clearField()
        {
            hdnSetupId.Value = "0";
            txtName.Text = string.Empty;
            txtDepartment.Text = string.Empty;
            txtDesignation.Text = string.Empty;
            txtPhone.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtUniversity.Text = string.Empty;
        }

        protected void DeleteMember_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.ExternalCommitteeMemberInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        ucamContext.ExternalCommitteeMemberInformations.Remove(ExistingObj);
                        ucamContext.SaveChanges();

                        showAlert("External Member Information Removed Successfully");
                        LoadExternalMemberInformation();
                        return;
                    }

                }

            }
            catch (Exception ex)
            {
            }
        }



        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void gvExternalList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var list = Session["MemberList"];

                if (list != null)
                {
                    gvExternalList.PageIndex = e.NewPageIndex;
                    gvExternalList.DataSource = list;
                    gvExternalList.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        
        }
    }
}