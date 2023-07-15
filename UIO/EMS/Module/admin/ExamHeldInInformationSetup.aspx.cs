using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ExamHeldInInformationSetup : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.ExamHeldInInformationSetup);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.ExamHeldInInformationSetup));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                LoadYearDDL();
                LoadMonthDDL();
                pnlAddAndUpdate.Visible = false;
            }
        }

        private void LoadYearDDL()
        {
            try
            {
                ddlStartYear.Items.Clear();
                ddlEndYear.Items.Clear();

                int startYear = 2003;

                for (int MaxYear = DateTime.Now.Year + 1; MaxYear >= startYear; MaxYear--)
                {
                    ddlStartYear.Items.Add(new ListItem(Convert.ToString(MaxYear)));
                    ddlEndYear.Items.Add(new ListItem(Convert.ToString(MaxYear)));
                }

                ddlStartYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlEndYear.SelectedValue = DateTime.Now.Year.ToString();

            }
            catch (Exception ex)
            {
            }
        }

        private void LoadMonthDDL()
        {
            try
            {
                ddlStartMonth.Items.Clear();
                ddlEndMonth.Items.Clear();

                var months = CultureInfo.CurrentCulture.DateTimeFormat.MonthNames;
                for (int i = 0; i < months.Length - 1; i++)
                {
                    ddlStartMonth.Items.Add(new ListItem(months[i], months[i].ToString()));
                    ddlEndMonth.Items.Add(new ListItem(months[i], months[i].ToString()));

                }
            }
            catch (Exception ex)
            {
            }
        }



        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                Session["HeldInList"] = null;

                int AcacalId = Convert.ToInt32(admissionSession.selectedValue);

                var HeldInList = ucamContext.ExamHeldInInformations.Where(x => (x.IsDeleted == null || x.IsDeleted == false) && (x.AcacalId == AcacalId || AcacalId == 0)).ToList();

                if (HeldInList != null && HeldInList.Any())
                {
                    gvHeldInList.DataSource = HeldInList;
                    gvHeldInList.DataBind();
                    Session["HeldInList"] = HeldInList;
                }
                else
                    ClearGridView();

            }
            catch (Exception ex)
            {
            }
        }

        protected void admissionSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            pnlAddAndUpdate.Visible = false;
        }
        private void ClearGridView()
        {
            gvHeldInList.DataSource = null;
            gvHeldInList.DataBind();
        }

        protected void gvHeldInList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var list = Session["HeldInList"];

                if (list != null)
                {
                    gvHeldInList.PageIndex = e.NewPageIndex;
                    gvHeldInList.DataSource = list;
                    gvHeldInList.DataBind();
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
                pnlAddAndUpdate.Visible = !pnlAddAndUpdate.Visible;
                hdnSetupId.Value = "0";
                ClearField();
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

                string HeldInName = "", Remarks = "", StartYear = "", StartMonth = "", EndYear = "", EndMonth = "";

                DateTime? StartDate = null, EndDate = null, SubmissionDate = null, PublishDate = null;

                int AcacalId = Convert.ToInt32(ucAdmissionSession.selectedValue);

                if (!string.IsNullOrEmpty(txtExamStartDate.Text))
                    StartDate = DateTime.ParseExact(txtExamStartDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);

                if (!string.IsNullOrEmpty(txtExamEndDate.Text))
                    EndDate = DateTime.ParseExact(txtExamEndDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);

                if (!string.IsNullOrEmpty(txtResultSubmissionDate.Text))
                    SubmissionDate = DateTime.ParseExact(txtResultSubmissionDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);

                if (!string.IsNullOrEmpty(txtResultPublishDate.Text))
                    PublishDate = DateTime.ParseExact(txtResultPublishDate.Text.Replace("/", string.Empty), "ddMMyyyy", null);

                if (!string.IsNullOrEmpty(txtRemarks.Text))
                    Remarks = txtRemarks.Text.Trim();

                if (!string.IsNullOrEmpty(txtHeldInName.Text))
                    HeldInName = txtHeldInName.Text.Trim();

                StartYear = ddlStartYear.SelectedValue.ToString();
                StartMonth = ddlStartMonth.SelectedValue.ToString();

                EndYear = ddlEndYear.SelectedValue.ToString();
                EndMonth = ddlEndMonth.SelectedValue.ToString();

                if (AcacalId == 0)
                {
                    showAlert("Please select a Academic Session");
                    return;
                }


                if (SetupId == 0) // Insert New Member
                {
                    DAL.AcademicCalender ac = ucamContext.AcademicCalenders.Find(AcacalId);

                    string Year = "", Session = "";

                    if (ac != null)
                    {
                        Year = ac.Year.ToString();
                        Session = ac.Code;
                    }

                    DAL.ExamHeldInInformation NewObj = new DAL.ExamHeldInInformation();

                    NewObj.AcacalId = AcacalId;
                    NewObj.Year = Year;
                    NewObj.Session = Session;
                    NewObj.ExamName = HeldInName;
                    NewObj.ExamStartDate = StartDate;
                    NewObj.ExamEndDate = EndDate;
                    NewObj.LastDateOfResultSubmission = SubmissionDate;
                    NewObj.ResultPublishDate = PublishDate;
                    NewObj.HeldInStartMonth = StartMonth;
                    NewObj.HeldInEndMonth = EndMonth;
                    NewObj.HeldInStartYear = StartYear;
                    NewObj.HeldInEndYear = EndYear;
                    NewObj.Remarks = Remarks;
                    NewObj.IsActive = chkIsActive.Checked;
                    NewObj.CreatedBy = UserObj.Id;
                    NewObj.CreatedDate = DateTime.Now;


                    ucamContext.ExamHeldInInformations.Add(NewObj);
                    ucamContext.SaveChanges();

                    if (NewObj.Id > 0)
                    {
                        showAlert("Held In Added Successfully");
                        ClearField();
                        btnLoad_Click(null, null);
                        pnlAddAndUpdate.Visible = true;
                        return;
                    }


                }
                else// Update Existing Member
                {
                    var ExistingObj = ucamContext.ExamHeldInInformations.Find(SetupId);

                    if (ExistingObj != null)
                    {

                        ExistingObj.ExamName = HeldInName;
                        ExistingObj.ExamStartDate = StartDate;
                        ExistingObj.ExamEndDate = EndDate;
                        ExistingObj.LastDateOfResultSubmission = SubmissionDate;
                        ExistingObj.ResultPublishDate = PublishDate;
                        ExistingObj.HeldInStartMonth = StartMonth;
                        ExistingObj.HeldInEndMonth = EndMonth;
                        ExistingObj.HeldInStartYear = StartYear;
                        ExistingObj.HeldInEndYear = EndYear;
                        ExistingObj.Remarks = Remarks;
                        ExistingObj.IsActive = chkIsActive.Checked;
                        ExistingObj.ModifiedBy = UserObj.Id;
                        ExistingObj.ModifiedDate = DateTime.Now;

                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();
                        showAlert("Held In Updated Successfully");
                        btnLoad_Click(null, null);
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
            try
            {
                ClearField();
                pnlAddAndUpdate.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }

        private void ClearField()
        {
            try
            {
                ucAdmissionSession.selectedValue = "0";
                txtHeldInName.Text = string.Empty;
                txtExamStartDate.Text = string.Empty;
                txtExamEndDate.Text = string.Empty;
                ddlStartYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlStartMonth.SelectedValue = "January";
                ddlEndYear.SelectedValue = DateTime.Now.Year.ToString();
                ddlEndMonth.SelectedValue = "January";
                txtResultSubmissionDate.Text = string.Empty;
                txtResultPublishDate.Text = string.Empty;
                txtRemarks.Text = string.Empty;
                chkIsActive.Checked = false;
            }
            catch (Exception ex)
            {
            }
        }


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void EditHeldIn_Click(object sender, EventArgs e)
        {
            try
            {
                ClearField();
                LinkButton btn = (LinkButton)(sender);
                int SetupId = Convert.ToInt32(btn.CommandArgument);
                if (SetupId > 0)
                {
                    var ExistingObj = ucamContext.ExamHeldInInformations.Find(SetupId);
                    if (ExistingObj != null)
                    {
                        pnlAddAndUpdate.Visible = true;
                        hdnSetupId.Value = SetupId.ToString();

                        ucAdmissionSession.SelectedValue(Convert.ToInt32(ExistingObj.AcacalId));
                        txtHeldInName.Text = ExistingObj.ExamName.ToString();

                        if (ExistingObj.ExamStartDate != null)
                            txtExamStartDate.Text =Convert.ToDateTime(ExistingObj.ExamStartDate).ToString("dd/MM/yyyy");

                        if (ExistingObj.ExamEndDate != null)
                            txtExamEndDate.Text = Convert.ToDateTime(ExistingObj.ExamEndDate).ToString("dd/MM/yyyy");

                        ddlStartYear.SelectedValue = ExistingObj.HeldInStartYear.ToString();
                        ddlStartMonth.SelectedValue = ExistingObj.HeldInStartMonth.ToString();

                        ddlEndYear.SelectedValue = ExistingObj.HeldInEndYear.ToString();
                        ddlEndMonth.SelectedValue = ExistingObj.HeldInEndMonth.ToString();

                        if (ExistingObj.ResultPublishDate != null)
                            txtResultPublishDate.Text = Convert.ToDateTime(ExistingObj.ResultPublishDate).ToString("dd/MM/yyyy");

                        if (ExistingObj.LastDateOfResultSubmission != null)
                            txtResultSubmissionDate.Text = Convert.ToDateTime(ExistingObj.LastDateOfResultSubmission).ToString("dd/MM/yyyy");

                        txtRemarks.Text = ExistingObj.Remarks;

                        chkIsActive.Checked = Convert.ToBoolean(ExistingObj.IsActive);

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }



    }
}