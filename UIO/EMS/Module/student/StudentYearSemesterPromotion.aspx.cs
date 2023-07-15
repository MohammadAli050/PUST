using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using LogicLayer.BusinessLogic;
using System.Data.Entity;

namespace EMS.Module.student
{
    public partial class StudentYearSemesterPromotion : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.StudentYearSemesterPromotion);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.StudentYearSemesterPromotion));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                btnPromote.Visible = false;
                ucDepartment.LoadDropDownList();
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                ucAdmissionSession.selectedValue = "0";
                //LoadYear();
                //LoadSemester();
                LoadHeldInInformation();
            }
        }

        private void LoadYear()
        {
            try
            {
                //var YearList = ucamContext.Years.ToList();
                //ddlYear.Items.Clear();
                //ddlYear.AppendDataBoundItems = true;
                //ddlYear.Items.Add(new ListItem("-select-", "0"));

                //if (YearList != null && YearList.Any())
                //{
                //    ddlYear.DataTextField = "YearName";
                //    ddlYear.DataValueField = "YearNo";
                //    ddlYear.DataSource = YearList.OrderBy(x => x.YearNo);
                //    ddlYear.DataBind();
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadSemester()
        {
            try
            {
                //var SemesterList = ucamContext.Semesters.ToList();

                //ddlSemester.Items.Clear();
                //ddlSemester.AppendDataBoundItems = true;
                //ddlSemester.Items.Add(new ListItem("-select-", "0"));

                //if (SemesterList != null && SemesterList.Any())
                //{
                //    ddlSemester.DataTextField = "SemesterName";
                //    ddlSemester.DataValueField = "SemesterNo";
                //    ddlSemester.DataSource = SemesterList.OrderBy(x => x.SemesterNo);
                //    ddlSemester.DataBind();
                //}


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                //int AcacalId = Convert.ToInt32(ucAcademicSession.selectedValue);
                //int YearNo = Convert.ToInt32(ddlYear.SelectedValue);
                //int SemesterNo = Convert.ToInt32(ddlSemester.SelectedValue);

                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                parameters2.Add(new SqlParameter { ParameterName = "@AcacalId", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                parameters2.Add(new SqlParameter { ParameterName = "@YearNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                parameters2.Add(new SqlParameter { ParameterName = "@SemesterNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });

                DataTable DataTableHeldInList = DataTableManager.GetDataFromQuery("GetAllHeldInInformation", parameters2);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("-select-", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "RelationId";
                    ddlHeldIn.DataSource = DataTableHeldInList;
                    ddlHeldIn.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
                ClearGridView();
            }
            catch (Exception)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            LoadHeldInInformation();
            ClearGridView();
        }

        protected void ucAdmissionSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {

            ClearGridView();
        }

        protected void ucAcademicSession_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            LoadHeldInInformation();
        }


        protected void ddlYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            LoadHeldInInformation();
        }

        protected void ddlSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
            LoadHeldInInformation();
        }

        protected void btnLoad_Click(object sender, EventArgs e)
        {
            try
            {
                ClearGridView();
                Session["datatablePromo"] = null;
                ViewState["sortdrPromo"] = "";

                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                int SessionId = Convert.ToInt32(ucAdmissionSession.selectedValue);
                int HeldInRelationId = 0;
                if (ddlHeldIn.SelectedValue != "")
                    HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                if (ProgramId == 0)
                {
                    showAlert("Please select a program");
                    return;
                }

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                parameters1.Add(new SqlParameter { ParameterName = "@SessionId", SqlDbType = System.Data.SqlDbType.Int, Value = SessionId });
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable dt = DataTableManager.GetDataFromQuery("GetStudentListWithHeldInByProgramAndAdmissionSession", parameters1);

                if (dt != null && dt.Rows.Count > 0)
                {
                    gvStudentList.DataSource = dt;
                    gvStudentList.DataBind();

                    btnPromote.Visible = true;

                    Session["datatablePromo"] = dt;
                    ViewState["sortdrPromo"] = "Asc";
                }
                else
                {
                    showAlert("No Data Found");
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }


        protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chk = (CheckBox)sender;

                if (chk.Checked)
                {
                    chk.Text = "Unselect All";
                }
                else
                {
                    chk.Text = "Select All";
                }

                foreach (GridViewRow row in gvStudentList.Rows)
                {
                    CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
                    ckBox.Checked = chk.Checked;
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void ddlHeldIn_SelectedIndexChanged(object sender, EventArgs e)
        {
            ClearGridView();
        }

        private void ClearGridView()
        {
            btnPromote.Visible = false;
            gvStudentList.DataSource = null;
            gvStudentList.DataBind();
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            try
            {
                int RelationId = Convert.ToInt32(modalHeldIn.SelectedValue);

                int SelectedStudent = CountGridStudent();

                if (SelectedStudent > 0)
                {
                    int Insert = 0;

                    foreach (GridViewRow row in gvStudentList.Rows)
                    {
                        CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");

                        if (ckBox.Checked)
                        {
                            HiddenField hdnStudentId = (HiddenField)row.FindControl("hdnStudentRoll");
                            HiddenField hdnPromotionHistoryId = (HiddenField)row.FindControl("hdnPromotionHistoryId");

                            int StudentId = hdnStudentId.Value == null ? 0 : Convert.ToInt32(hdnStudentId.Value);
                            int PromotionHistoryId = hdnPromotionHistoryId.Value == null ? 0 : Convert.ToInt32(hdnPromotionHistoryId.Value);

                            if (StudentId > 0)
                            {
                                if (PromotionHistoryId > 0)// Already Student Exists
                                {
                                    var ExistingList = ucamContext.StudentYearSemesterHistories.AsNoTracking().Where(x => x.StudentId == StudentId
                                        && x.IsActive == true).ToList();

                                    if (ExistingList != null && ExistingList.Any())
                                    {
                                        foreach (var ExistingObj in ExistingList)
                                        {
                                            ExistingObj.IsActive = false;
                                            ExistingObj.IsClosed = true;
                                            ExistingObj.ModifiedBy = UserObj.Id;
                                            ExistingObj.ModifiedDate = DateTime.Now;

                                            ucamContext.Entry(ExistingObj).State = EntityState.Modified;
                                            ucamContext.SaveChanges();
                                        }
                                    }

                                    int InsertId = InsertNewEntry(StudentId, RelationId);
                                    if (InsertId > 0)
                                    {
                                        Insert++;
                                    }

                                }
                                else // New Entry
                                {
                                    int InsertId = InsertNewEntry(StudentId, RelationId);
                                    if (InsertId > 0)
                                    {
                                        Insert++;
                                    }
                                }
                            }

                        }
                    }

                    string Msg = "";
                    if (Insert == 0)
                        Msg = "Data Update Failed";
                    else
                    {
                        Msg = "Information Updated Successfully";
                    }

                    showAlert(Msg);
                    btnLoad_Click(null, null);
                    return;

                }
                else
                {
                    showAlert("Please select minimum one student");
                    return;
                }


            }
            catch (Exception ex)
            {
            }
        }

        private int InsertNewEntry(int StudentId, int RelationId)
        {
            int Id = 0;
            try
            {
                DAL.StudentYearSemesterHistory NewObj = new DAL.StudentYearSemesterHistory();

                NewObj.StudentId = StudentId;
                NewObj.HeldInProgramRelationId = RelationId;
                NewObj.IsActive = true;
                NewObj.IsClosed = false;
                NewObj.CreatedBy = UserObj.Id;
                NewObj.CreatedDate = DateTime.Now;

                ucamContext.StudentYearSemesterHistories.Add(NewObj);
                ucamContext.SaveChanges();

                if (NewObj.Id > 0)
                    Id = NewObj.Id;

            }
            catch (Exception ex)
            {
            }

            return Id;
        }

        protected void btnPromote_Click(object sender, EventArgs e)
        {
            int SelectedStudent = CountGridStudent();
            if (SelectedStudent > 0)
            {

                ModalPopUpExamNameInformation.Show();
                //LoadModalYear();
                //LoadModalSemester();
                LoadModalHeldIn();
            }
            else
            {
                showAlert("Please select minimum one student");
                return;
            }
        }

        private int CountGridStudent()
        {
            int studentCounter = 0;
            for (int i = 0; i < gvStudentList.Rows.Count; i++)
            {
                GridViewRow row = gvStudentList.Rows[i];
                CheckBox studentCheckd = (CheckBox)row.FindControl("ChkActive");

                if (studentCheckd.Checked == true)
                {
                    studentCounter = studentCounter + 1;
                }
            }
            return studentCounter;
        }

        private void LoadModalYear()
        {
            try
            {
                //var YearList = ucamContext.Years.ToList();
                //ddlModalYear.Items.Clear();
                //ddlModalYear.AppendDataBoundItems = true;
                //ddlModalYear.Items.Add(new ListItem("-select-", "0"));

                //if (YearList != null && YearList.Any())
                //{
                //    ddlModalYear.DataTextField = "YearName";
                //    ddlModalYear.DataValueField = "YearNo";
                //    ddlModalYear.DataSource = YearList.OrderBy(x => x.YearNo);
                //    ddlModalYear.DataBind();
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void LoadModalSemester()
        {
            try
            {
                //var SemesterList = ucamContext.Semesters.ToList();

                //ddlModalSemester.Items.Clear();
                //ddlModalSemester.AppendDataBoundItems = true;
                //ddlModalSemester.Items.Add(new ListItem("-select-", "0"));

                //if (SemesterList != null && SemesterList.Any())
                //{
                //    ddlModalSemester.DataTextField = "SemesterName";
                //    ddlModalSemester.DataValueField = "SemesterNo";
                //    ddlModalSemester.DataSource = SemesterList.OrderBy(x => x.SemesterNo);
                //    ddlModalSemester.DataBind();
                //}


            }
            catch (Exception ex)
            {
            }
        }

        private void LoadModalHeldIn()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);
                //int AcacalId = Convert.ToInt32(AdmissionSessionUserControl1.selectedValue);
                //int YearNo = Convert.ToInt32(ddlModalYear.SelectedValue);
                //int SemesterNo = Convert.ToInt32(ddlModalSemester.SelectedValue);

                List<SqlParameter> parameters2 = new List<SqlParameter>();
                parameters2.Add(new SqlParameter { ParameterName = "@ProgramId", SqlDbType = System.Data.SqlDbType.Int, Value = ProgramId });
                parameters2.Add(new SqlParameter { ParameterName = "@AcacalId", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                parameters2.Add(new SqlParameter { ParameterName = "@YearNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });
                parameters2.Add(new SqlParameter { ParameterName = "@SemesterNo", SqlDbType = System.Data.SqlDbType.Int, Value = 0 });

                DataTable DataTableHeldInList = DataTableManager.GetDataFromQuery("GetAllHeldInInformation", parameters2);

                modalHeldIn.Items.Clear();
                modalHeldIn.AppendDataBoundItems = true;
                modalHeldIn.Items.Add(new ListItem("-select-", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    modalHeldIn.DataTextField = "ExamName";
                    modalHeldIn.DataValueField = "RelationId";
                    modalHeldIn.DataSource = DataTableHeldInList;
                    modalHeldIn.DataBind();

                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void AdmissionSessionUserControl1_SessionSelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopUpExamNameInformation.Show();
            LoadModalHeldIn();
        }

        protected void ddlModalYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopUpExamNameInformation.Show();
            LoadModalHeldIn();
        }

        protected void ddlModalSemester_SelectedIndexChanged(object sender, EventArgs e)
        {
            ModalPopUpExamNameInformation.Show();
            LoadModalHeldIn();
        }

        protected void gvStudentList_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {

                DataTable dtrslt = (DataTable)Session["datatablePromo"];
                if (dtrslt != null && dtrslt.Rows.Count > 0)
                {
                    if (Convert.ToString(ViewState["sortdrPromo"]) == "Asc")
                    {
                        dtrslt.DefaultView.Sort = e.SortExpression + " Desc";
                        ViewState["sortdrPromo"] = "Desc";
                    }
                    else
                    {
                        dtrslt.DefaultView.Sort = e.SortExpression + " Asc";
                        ViewState["sortdrPromo"] = "Asc";
                    }
                    gvStudentList.DataSource = dtrslt;
                    gvStudentList.DataBind();
                }
            }

            catch (Exception ex)
            {

            }
        }



    }
}