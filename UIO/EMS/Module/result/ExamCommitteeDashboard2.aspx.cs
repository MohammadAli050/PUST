using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module.admin
{
    public partial class ExamCommitteeDashboard2 : BasePage
    {
        string _pageUrl = HttpContext.Current.Request.Url.AbsoluteUri;
        string _pageName = Convert.ToString(CommonUtility.CommonEnum.PageName.QuestionSetterAndScriptExaminerSetup);
        string _pageId = Convert.ToString(Convert.ToInt32(CommonUtility.CommonEnum.PageName.QuestionSetterAndScriptExaminerSetup));

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser UserObj = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //base.CheckPage_Load();
            UserObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                btnLoadTabulation.Visible = false;
                //btnUpdateInfo.Visible = false;
                //hdnIsUpdate.Value = "0";
                if (UserObj.RoleID == 7) // Exam Committee Chairman
                    ucDepartment.LoadDropDownListFromExamCommitteeWithUserAccess(UserObj.Id, UserObj.RoleID);
                else
                    ucDepartment.LoadDropDownListWithUserAccess(UserObj.Id, UserObj.RoleID);
                ucDepartment_DepartmentSelectedIndexChanged(null, null);
                LoadHeldInInformation();
            }
        }

        #region On Load Methods

        private void LoadHeldInInformation()
        {
            try
            {
                int ProgramId = Convert.ToInt32(ucProgram.selectedValue);

                DataTable DataTableHeldInList = CommonMethodsForGetHeldIn.GetExamHeldInInformation(ProgramId, 0, 0, 0);

                ddlHeldIn.Items.Clear();
                ddlHeldIn.AppendDataBoundItems = true;
                ddlHeldIn.Items.Add(new ListItem("Select", "0"));

                if (DataTableHeldInList != null && DataTableHeldInList.Rows.Count > 0)
                {
                    ddlHeldIn.DataTextField = "ExamName";
                    ddlHeldIn.DataValueField = "RelationId";
                    ddlHeldIn.DataSource = DataTableHeldInList;
                    ddlHeldIn.DataBind();

                }

                //ddlHeldIn.Items.Insert(0, new ListItem("Select", "0"));

                //ddlHeldIn.SelectedValue = "0";
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region On Selected Index Changed Methods

        protected void ucDepartment_DepartmentSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                LoadHeldInInformation();
                ClearGridView();
                LoadData();
            }
            catch (Exception)
            {
            }
        }

        protected void ucProgram_ProgramSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadHeldInInformation();
                ClearGridView();
                LoadData();
            }
            catch (Exception ex)
            {
            }
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

        #endregion

        private void LoadData()
        {
            try
            {
                ClearGridView();

                int HeldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });

                DataTable DataTableRoutineList = DataTableManager.GetDataFromQuery("GetExamCommitteeDashboardData", parameters1);

                if (DataTableRoutineList != null && DataTableRoutineList.Rows.Count > 0)
                {

                    gvScheduleList.DataSource = DataTableRoutineList;
                    gvScheduleList.DataBind();
                    Session["ScheduleList"] = DataTableRoutineList;
                    ViewState["sortdrSchedule"] = "Asc";
                    //btnUpdateInfo.Visible = true;

                    if (UserObj.RoleID != 1 && UserObj.RoleID != 8)
                        btnLoadTabulation.Visible = false;
                    else
                        btnLoadTabulation.Visible = true;

                }
                try
                {

                    #region Load Tabulator Information

                    lblTabulator1.Text = "";
                    lblTabulator2.Text = "";
                    lblTabulator3.Text = "";
                    lblChairman.Text = "";
                    lblMember1.Text = "";
                    lblMember2.Text = "";
                    lblMemberExt.Text = "";

                    var headerInformation = (from ti in ucamContext.ExamHeldInRelationWiseTabulatorInformations
                                             join e1 in ucamContext.Employees on ti.TabulatorOneId equals e1.EmployeeID into e1_jointable
                                             from e1j in e1_jointable.DefaultIfEmpty()
                                             join p1 in ucamContext.People on e1j.PersonId equals p1.PersonID into p1_jointable
                                             from p1j in p1_jointable.DefaultIfEmpty()
                                             join e2 in ucamContext.Employees on ti.TabulatorTwoId equals e2.EmployeeID into e2_jointable
                                             from e2j in e2_jointable.DefaultIfEmpty()
                                             join p2 in ucamContext.People on e2j.PersonId equals p2.PersonID into p2_jointable
                                             from p2j in p2_jointable.DefaultIfEmpty()
                                             join e3 in ucamContext.Employees on ti.TabulatorThreeId equals e3.EmployeeID into e3_jointable
                                             from e3j in e3_jointable.DefaultIfEmpty()
                                             join p3 in ucamContext.People on e3j.PersonId equals p3.PersonID into p3_jointable
                                             from p3j in p3_jointable.DefaultIfEmpty()
                                             join ec in ucamContext.ExamSetupWithExamCommittees on ti.HeldInProgramRelationId equals ec.HeldInProgramRelationId into ec_jointable
                                             from ecj in ec_jointable.DefaultIfEmpty()
                                             join e4 in ucamContext.Employees on ecj.ExamCommitteeChairmanId equals e4.EmployeeID into e4_jointable
                                             from e4j in e4_jointable.DefaultIfEmpty()
                                             join p4 in ucamContext.People on e4j.PersonId equals p4.PersonID into p4_jointable
                                             from p4j in p4_jointable.DefaultIfEmpty()
                                             join e5 in ucamContext.Employees on ecj.ExamCommitteeMemberOneId equals e5.EmployeeID into e5_jointable
                                             from e5j in e5_jointable.DefaultIfEmpty()
                                             join p5 in ucamContext.People on e5j.PersonId equals p5.PersonID into p5_jointable
                                             from p5j in p5_jointable.DefaultIfEmpty()
                                             join e6 in ucamContext.Employees on ecj.ExamCommitteeMemberTwoId equals e6.EmployeeID into e6_jointable
                                             from e6j in e6_jointable.DefaultIfEmpty()
                                             join p6 in ucamContext.People on e6j.PersonId equals p6.PersonID into p6_jointable
                                             from p6j in p6_jointable.DefaultIfEmpty()
                                             join p7 in ucamContext.ExternalCommitteeMemberInformations on ecj.ExamCommitteeExternalMemberId equals p7.ExternalId into p7_jointable
                                             from p7j in p7_jointable.DefaultIfEmpty()
                                             where ti.HeldInProgramRelationId == HeldInRelationId
                                             select new
                                             {
                                                 Tabulator1Name = p1j.FullName,
                                                 Tabulator2Name = p2j.FullName,
                                                 Tabulator3Name = p3j.FullName,
                                                 ChairmanName = p4j.FullName,
                                                 Member1Name = p5j.FullName,
                                                 Member2Name = p6j.FullName,
                                                 ExternalMemberName = p7j.Name
                                             }).FirstOrDefault();

                    if (headerInformation != null)
                    {
                        lblChairman.Text = headerInformation.ChairmanName;
                        lblMember1.Text = headerInformation.Member1Name;
                        lblMember2.Text = headerInformation.Member2Name;
                        lblMemberExt.Text = headerInformation.ExternalMemberName;
                        lblTabulator1.Text = headerInformation.Tabulator1Name;
                        lblTabulator2.Text = headerInformation.Tabulator2Name;
                        lblTabulator3.Text = headerInformation.Tabulator3Name;
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                }

                GridRebind();
            }
            catch (Exception ex)
            {
            }
        }

        private void GridRebind()
        {
            try
            {
                for (int i = 0; i < gvScheduleList.Rows.Count; i++)
                {
                    GridViewRow row = gvScheduleList.Rows[i];

                    try
                    {
                        Button btnCourseTeacherMark = (Button)row.FindControl("btnCourseTeacherMark");
                        Button btnInSEMark = (Button)row.FindControl("btnInSEMark");
                        Button btnExSEMark = (Button)row.FindControl("btnExSEMark");
                        Button btnTEMark = (Button)row.FindControl("btnTEMark");
                        Button btn100Mark = (Button)row.FindControl("btn100Mark");
                        HiddenField hdnStatusId = (HiddenField)row.FindControl("hdnStatusId");

                        int StatusId = Convert.ToInt32(hdnStatusId.Value);

                        if (UserObj.RoleID == 1 || UserObj.RoleID == 8)
                        {
                            if (StatusId > 0)
                                btnCourseTeacherMark.Visible = true;
                            else
                                btnCourseTeacherMark.Visible = false;

                            if (StatusId < 2)
                                btnInSEMark.Visible = false;
                            else
                                btnInSEMark.Visible = true;

                            if (StatusId < 3)
                                btnExSEMark.Visible = false;
                            else
                                btnExSEMark.Visible = true;

                            if (StatusId < 4)
                                btnTEMark.Visible = false;
                            else
                                btnTEMark.Visible = true;

                            if (StatusId < 5)
                                btn100Mark.Visible = false;
                            else
                                btn100Mark.Visible = true;
                        }
                        else
                        {

                            btnCourseTeacherMark.Visible = false;
                            btnInSEMark.Visible = false;
                            btnExSEMark.Visible = false;
                            btnTEMark.Visible = false;
                            btn100Mark.Visible = false;
                        }
                    }
                    catch (Exception ex)
                    {
                    }



                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Gridview Methods

        //protected void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        CheckBox chk = (CheckBox)sender;

        //        if (chk.Checked)
        //        {
        //            chk.Text = "Unselect All";
        //        }
        //        else
        //        {
        //            chk.Text = "Select All";
        //        }

        //        foreach (GridViewRow row in gvScheduleList.Rows)
        //        {
        //            CheckBox ckBox = (CheckBox)row.FindControl("ChkActive");
        //            ckBox.Checked = chk.Checked;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        protected void gvScheduleList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                var list = Session["ScheduleList"];

                if (list != null)
                {
                    gvScheduleList.PageIndex = e.NewPageIndex;
                    gvScheduleList.DataSource = list;
                    gvScheduleList.DataBind();
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        private int CalculatedGridSelectedItem()
        {
            int ItemSelected = 0;
            try
            {
                for (int i = 0; i < gvScheduleList.Rows.Count; i++)
                {
                    GridViewRow row = gvScheduleList.Rows[i];
                    CheckBox scheduleCheckd = (CheckBox)row.FindControl("ChkActive");

                    if (scheduleCheckd.Checked == true)
                    {
                        ItemSelected = ItemSelected + 1;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return ItemSelected;
        }


        private void ClearGridView()
        {
            gvScheduleList.DataSource = null;
            gvScheduleList.DataBind();
            //btnUpdateInfo.Visible = false;
        }

        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);
        }

        protected void btnCourseTeacherMark_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            int acaCalSecId = Convert.ToInt32(btn.CommandArgument);

            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

            string url = string.Format("../../Module/result/Report/RptContinuousAssessment.aspx?mmi=4153552c775d4d494e63&d={0}&p={1}&h={2}&a={3}", departmentId, programId, heldInRelationId, acaCalSecId);
            string script = string.Format("window.open('{0}');", url);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newPage" + UniqueID, script, true);
        }

        protected void btn100Mark_Click(object sender, EventArgs e)
        {
            Button btn = (Button)(sender);
            int acaCalSecId = Convert.ToInt32(btn.CommandArgument);

            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            int programId = Convert.ToInt32(ucProgram.selectedValue);
            int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

            string url = string.Format("../../Module/result/Report/RptConsolidatedMarksSheet.aspx?mmi=4153552c775d4d494e63&d={0}&p={1}&h={2}&a={3}", departmentId, programId, heldInRelationId, acaCalSecId);
            string script = string.Format("window.open('{0}');", url);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newPage" + UniqueID, script, true);
        }

        protected void btnInSEMark_Click(object sender, EventArgs e)
        {
            try
            {
                Button btn = (Button)(sender);
                int acaCalSecId = Convert.ToInt32(btn.CommandArgument);

                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                string url = string.Format("../../Module/result/Report/RptFirstSecondAndThirdExaminerMark.aspx?mmi=4153552c775d4d494e63&d={0}&p={1}&h={2}&a={3}", departmentId, programId, heldInRelationId, acaCalSecId);
                string script = string.Format("window.open('{0}');", url);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newPage" + UniqueID, script, true);
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnLoadTabulation_Click(object sender, EventArgs e)
        {
            try
            {

                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                int programId = Convert.ToInt32(ucProgram.selectedValue);
                int heldInRelationId = Convert.ToInt32(ddlHeldIn.SelectedValue);

                string url = string.Format("../../Module/result/Report/RptTabulationSheet.aspx?mmi=4153552c775d4d494e63&d={0}&p={1}&h={2}", departmentId, programId, heldInRelationId);
                string script = string.Format("window.open('{0}');", url);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "newPage" + UniqueID, script, true);
            }
            catch (Exception ex)
            {
            }
        }

    }
}