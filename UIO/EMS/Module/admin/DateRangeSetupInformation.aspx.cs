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
    public partial class DateRangeSetupInformation : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.FormFillUpApplicationManageByHallProvost);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.FormFillUpApplicationManageByHallProvost));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                DivInfo.Visible = false;
                LoadExamYear();
                LoadHeldInInformation();
                LoadActivityType();
                FillStartTime("12:35 PM");
                FillEndTime("12:35 PM");

                DateTime startDate = DateTime.Now;
                DateTime endDate = DateTime.Now;
                DateFromTextBox.Text = startDate.ToString("dd/MM/yyyy");
                DateToTextBox.Text = endDate.ToString("dd/MM/yyyy");
            }
        }


        #region On Load Methods

        private void LoadExamYear()
        {
            try
            {
                ddlExamYear.Items.Clear();

                int startYear = 2003;

                for (int MaxYear = DateTime.Now.Year + 1; MaxYear >= startYear; MaxYear--)
                {
                    ddlExamYear.Items.Add(new ListItem(Convert.ToString(MaxYear)));
                }
                ddlExamYear.SelectedValue = DateTime.Now.Year.ToString();

            }
            catch (Exception ex)
            {
            }
        }

        private void LoadHeldInInformation()
        {
            try
            {
                int ExamYear = Convert.ToInt32(ddlExamYear.SelectedValue);

                var HeldInList = ucamContext.ExamHeldInInformations.Where(x => (x.Year == ExamYear.ToString() || ExamYear == 0) && x.IsActive == true).ToList();

                if (HeldInList != null && HeldInList.Any())
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "Id";
                    ddlHeldIn.DataSource = HeldInList.OrderBy(x => x.Id);
                    ddlHeldIn.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadActivityType()
        {
            try
            {

                ddlActivityType.Items.Clear();
                ddlActivityType.AppendDataBoundItems = true;
                ddlActivityType.Items.Add(new ListItem("Select", "0"));

                var ActivityTypelist = ucamContext.DateRangeActivityTypeInformations.Where(x => x.ActiveStatus == 1).ToList();

                if (ActivityTypelist != null && ActivityTypelist.Any())
                {
                    ddlActivityType.DataTextField = "TypeName";
                    ddlActivityType.DataValueField = "Id";
                    ddlActivityType.DataSource = ActivityTypelist.OrderBy(x => x.Id);
                    ddlActivityType.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        void FillStartTime(string time)
        {
            try
            {
                DateTime dt = DateTime.Parse(time);
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (dt.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                TimeSelector1.SetTime(dt.Hour, dt.Minute, am_pm);

            }
            catch (Exception ex)
            { }
        }

        void FillEndTime(string time)
        {
            try
            {
                DateTime dt = DateTime.Parse(time);
                MKB.TimePicker.TimeSelector.AmPmSpec am_pm;
                if (dt.ToString("tt") == "AM")
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.AM;
                }
                else
                {
                    am_pm = MKB.TimePicker.TimeSelector.AmPmSpec.PM;
                }
                TimeSelector2.SetTime(dt.Hour, dt.Minute, am_pm);

            }
            catch (Exception ex)
            { }
        }

        #endregion

        #region On Change Methods


        protected void ddlExamYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            LoadHeldInInformation();
            LoadData();
        }
        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                LoadData();
            }
            catch (Exception ex)
            {
            }
        }
        protected void ddlActivityType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                LoadData();
            }
            catch (Exception ex)
            {
            }
        }


        #endregion



        private void LoadData()
        {
            try
            {
                hdnSaveUpdateId.Value = "0";

                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int ActivityTypeId = Convert.ToInt32(ddlActivityType.SelectedValue);

                var ItemList = ucamContext.DateRangeSetupInformations.Where(x => (x.HeldInId == HeldInId || HeldInId == 0) && (x.DateRangeActivityTypeId == ActivityTypeId || ActivityTypeId == 0)).ToList();

                if (ItemList != null && ItemList.Any())
                {
                    DivInfo.Visible = false;

                    foreach (var item in ItemList)
                    {
                        try
                        {
                            var TypeObj = ucamContext.DateRangeActivityTypeInformations.Find(item.DateRangeActivityTypeId);
                            if (TypeObj != null)
                            {
                                item.Attribute3 = TypeObj.TypeName;
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }

                    gvItemList.DataSource = ItemList.OrderBy(x => x.DateRangeActivityTypeId).ToList();
                    gvItemList.DataBind();
                }
                else
                {
                    if (ActivityTypeId > 0)
                    {
                        DivInfo.Visible = true;
                        btnSaveUpdate.Visible = true;
                        btnCancel.Visible = true;
                    }
                }


            }
            catch (Exception ex)
            {
            }
        }

        private void ClearGridView()
        {
            try
            {
                gvItemList.DataSource = null;
                gvItemList.DataBind();
                DivInfo.Visible = false;
                btnSaveUpdate.Visible = false;
                btnCancel.Visible = false;
            }
            catch (Exception ex)
            {
            }
        }


        protected void btnSaveUpdate_Click(object sender, EventArgs e)
        {
            try
            {

                int HeldInId = Convert.ToInt32(ddlHeldIn.SelectedValue);
                int ActivityTypeId = Convert.ToInt32(ddlActivityType.SelectedValue);

                int SaveUpdateValue = Convert.ToInt32(hdnSaveUpdateId.Value);

                if (SaveUpdateValue == 0) // Save
                {
                    var IsExists = ucamContext.DateRangeSetupInformations.Where(x => x.HeldInId == HeldInId && x.DateRangeActivityTypeId == ActivityTypeId).FirstOrDefault();

                    if (IsExists == null)
                    {

                        DAL.DateRangeSetupInformation NewObj = new DAL.DateRangeSetupInformation();

                        NewObj.HeldInId = HeldInId;
                        NewObj.DateRangeActivityTypeId = ActivityTypeId;
                        if (chkActive.Checked)
                            NewObj.ActiveStatus = 1;
                        else
                            NewObj.ActiveStatus = 0;

                        DateTime startTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
                        DateTime endTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));
                        NewObj.StartTime = startTime;
                        NewObj.EndTime = endTime;

                        DateTime FromDate = DateTime.ParseExact(DateFromTextBox.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        DateTime ToDate = DateTime.ParseExact(DateToTextBox.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        NewObj.StartDate = FromDate;
                        NewObj.EndDate = ToDate;

                        NewObj.CreatedBy = UserObj.Id;
                        NewObj.CreatedDate = DateTime.Now;

                        ucamContext.DateRangeSetupInformations.Add(NewObj);
                        ucamContext.SaveChanges();

                        if (NewObj.Id > 0)
                        {
                            showAlert("Date Range Setup Successfully");
                            LoadData();
                        }
                        else
                        {
                            showAlert("Setup Failed");
                            return;
                        }
                    }
                    else
                    {
                        showAlert("Setup already exists");
                        return;
                    }
                }
                else // Update
                {
                    var ExistingObj = ucamContext.DateRangeSetupInformations.Find(SaveUpdateValue);
                    if (ExistingObj != null)
                    {
                        if (chkActive.Checked)
                            ExistingObj.ActiveStatus = 1;
                        else
                            ExistingObj.ActiveStatus = 0;

                        DateTime startTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector1.Hour, TimeSelector1.Minute, TimeSelector1.Second, TimeSelector1.AmPm));
                        DateTime endTime = DateTime.Parse(string.Format("{0}:{1}:{2} {3}", TimeSelector2.Hour, TimeSelector2.Minute, TimeSelector2.Second, TimeSelector2.AmPm));
                        ExistingObj.StartTime = startTime;
                        ExistingObj.EndTime = endTime;

                        DateTime FromDate = DateTime.ParseExact(DateFromTextBox.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        DateTime ToDate = DateTime.ParseExact(DateToTextBox.Text.Replace("/", string.Empty), "ddMMyyyy", null);
                        ExistingObj.StartDate = FromDate;
                        ExistingObj.EndDate = ToDate;

                        ExistingObj.ModifiedBy = UserObj.Id;
                        ExistingObj.ModifiedDate = DateTime.Now;


                        ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                        ucamContext.SaveChanges();

                        showAlert("Date Range Setup Updated Successfully");
                        LoadData();
                    }
                }

            }
            catch (Exception ex)
            {

            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DivInfo.Visible = false;
        }


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void Edit_Click(object sender, EventArgs e)
        {
            try
            {
                LinkButton linkButton = new LinkButton();
                linkButton = (LinkButton)sender;
                int id = Convert.ToInt32(linkButton.CommandArgument);
                var temp = ucamContext.DateRangeSetupInformations.Find(id);
                if (temp != null)
                {
                    hdnSaveUpdateId.Value = id.ToString();

                    DateTime startDate = (DateTime)temp.StartDate;
                    DateTime endDate = (DateTime)temp.EndDate;

                    DateFromTextBox.Text = startDate.ToString("dd/MM/yyyy");
                    DateToTextBox.Text = endDate.ToString("dd/MM/yyyy");

                    DateTime StartTime = (DateTime)temp.StartTime;
                    DateTime EndTime = (DateTime)temp.EndTime;

                    FillStartTime(StartTime.Hour + ":" + StartTime.Minute + "" + StartTime.ToString("tt", CultureInfo.InvariantCulture));
                    FillEndTime(EndTime.Hour + ":" + EndTime.Minute + "" + EndTime.ToString("tt", CultureInfo.InvariantCulture));

                    if (temp.ActiveStatus == 1)
                        chkActive.Checked = true;
                    else
                        chkActive.Checked = false;


                    DivInfo.Visible = true;
                    btnSaveUpdate.Visible = true;
                    btnCancel.Visible = true;

                    ddlActivityType.SelectedValue = temp.DateRangeActivityTypeId.ToString();

                }
            }
            catch (Exception ex)
            {
            }
        }

    }
}