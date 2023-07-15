using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DataAccess;
using System.Data.SqlClient;
using System.Drawing;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using Microsoft.Reporting.WebForms;

public partial class RptCalendarDistribution : BasePage
{
    #region Events

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            base.CheckPage_Load();

            if (!IsPostBack)
            {
                ucDepartment.LoadDropDownList();
                int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
                ucProgram.LoadDropdownByDepartmentId(departmentId);
                FillTreeCombo();
                FillLinkedCalendars();
            }
        }
        catch (Exception Ex)
        {
            ShowMessage(Ex.Message, Color.Red);
        }
    }

    protected void ddlPrograms_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.Visible = false;

            ClearMessagelbl();

            FillTreeCombo();
        }
        catch (Exception Ex)
        {
            ShowMessage(Ex.Message, Color.Red);
        }
    }

    protected void ddlTreeMasters_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ClearMessagelbl();

            FillLinkedCalendars();
        }
        catch (Exception Ex)
        {
            ShowMessage(Ex.Message, Color.Red);
        }
    }

    protected void ddlLinkedCalendars_SelectedIndexChanged(object sender, EventArgs e)
    {
        ReportViewer1.Visible = false;

        ClearMessagelbl();
    }

    protected void OnDepartmentSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            ReportViewer1.Visible = false;
            int departmentId = Convert.ToInt32(ucDepartment.selectedValue);
            ucProgram.LoadDropdownByDepartmentId(departmentId);
            FillTreeCombo();
            FillLinkedCalendars();
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Methods

    private void FillTreeCombo()
    {
        ddlTreeMasters.Items.Clear();

        if (Convert.ToInt32(ucProgram.selectedValue) >= 0)
        {
            //if (Session["Programs"] != null)
            //{

            //}
            var program = ProgramManager.GetById(Convert.ToInt32(ucProgram.selectedValue));
            //List<Program> programs = (List<Program>)Session["Programs"];
            //var program = from prog in programs where prog.ProgramID == Int32.Parse(ucProgram.selectedValue) select prog;
            //var program = programs.Where(x => x.ProgramID == Convert.ToInt32(ucProgram.selectedValue)).ToList();

            List<TreeMaster> treeMasters = TreeMasterManager.GetAllProgramID(program.ProgramID);

            if (treeMasters != null && treeMasters.Count > 0)
            {
                ddlTreeMasters.Enabled = true;

                foreach (TreeMaster treeMaster in treeMasters)
                {
                    ListItem item = new ListItem();
                    item.Value = treeMaster.TreeMasterID.ToString();
                    item.Text = treeMaster.Node_Name;
                    ddlTreeMasters.Items.Add(item);
                }

                if (Session["TreeMaster"] != null)
                {
                    Session.Remove("TreeMaster");
                }
                if (Session["TreeMasters"] != null)
                {
                    Session.Remove("TreeMasters");
                }
                Session.Add("TreeMasters", treeMasters);

                ddlTreeMasters.SelectedIndex = 0;
                ddlTreeMasters_SelectedIndexChanged(null, null);
            }
            else
            {
                if (Session["TreeMaster"] != null)
                {
                    Session.Remove("TreeMaster");
                }
                if (Session["TreeMasters"] != null)
                {
                    Session.Remove("TreeMasters");
                }
                if (Session["TreeCalMaster"] != null)
                {
                    Session.Remove("TreeCalMaster");
                }
                if (Session["TreeCalMasters"] != null)
                {
                    Session.Remove("TreeCalMasters");
                }
                ddlTreeMasters.Enabled = false;
                ddlLinkedCalendars.Items.Clear();
                ddlLinkedCalendars.Enabled = false;
                ShowMessage("No Tree found for the selected Program", Color.Red);
            }
        }
    }

    private void FillLinkedCalendars()
    {
        ddlLinkedCalendars.Items.Clear();

        if (ddlTreeMasters.Items.Count > 0)
        {
            if (Session["TreeMasters"] != null)
            {
                List<TreeMaster> treeMasters = (List<TreeMaster>)Session["TreeMasters"];
                var treeMaster = from treeMas in treeMasters where treeMas.TreeMasterID == Int32.Parse(ddlTreeMasters.SelectedValue) select treeMas;

                List<TreeCalendarMaster> _treeCalMasters = TreeCalendarMasterManager.GetAllByTreeMasterID(treeMaster.ElementAt(0).TreeMasterID);
                Session.Add("TreeMaster", treeMaster.ElementAt(0));

                if (_treeCalMasters != null)
                {
                    ddlLinkedCalendars.Enabled = true;

                    ListItem item = new ListItem();
                    item.Value = 0.ToString();
                    item.Text = "<New Link>";
                    ddlLinkedCalendars.Items.Add(item);
                    foreach (TreeCalendarMaster treeCalMaster in _treeCalMasters)
                    {
                        item = new ListItem();
                        item.Value = treeCalMaster.TreeCalendarMasterID.ToString();
                        item.Text = treeCalMaster.Name;
                        ddlLinkedCalendars.Items.Add(item);
                    }

                    if (Session["TreeCalMaster"] != null)
                    {
                        Session.Remove("TreeCalMaster");
                    }
                    if (Session["TreeCalMasters"] != null)
                    {
                        Session.Remove("TreeCalMasters");
                    }
                    Session.Add("TreeCalMasters", _treeCalMasters);

                    ddlLinkedCalendars.SelectedIndex = 0;
                }
                else
                {
                    if (Session["TreeCalMaster"] != null)
                    {
                        Session.Remove("TreeCalMaster");
                    }
                    if (Session["TreeCalMasters"] != null)
                    {
                        Session.Remove("TreeCalMasters");
                    }
                    ddlLinkedCalendars.Enabled = false;
                    ShowMessage("No Distribution were found for the selected tree", Color.Red);
                }
            }
        }
    }

    private void ClearMessagelbl()
    {
        lblMessage.Text = string.Empty;
        lblMessage.ForeColor = Color.Red;
    }

    private void ShowMessage(string message, Color color)
    {
        lblMessage.Text = string.Empty;
        lblMessage.Text = message;
        lblMessage.ForeColor = color;
    }

    #endregion

    protected void btnLoad_Click(object sender, EventArgs e)
    {
        int programId = Convert.ToInt32(ucProgram.selectedValue);
        int treeCalMasId = Convert.ToInt32(ddlLinkedCalendars.SelectedItem.Value);

        if (treeCalMasId != 0)
        {
            LoadCourseDistribution(programId, treeCalMasId);
        }
        else
        {
            ShowMessage("Select a Linked Caleneder !", Color.Red);
            return;
        }

    }

    private void LoadCourseDistribution(int ProgramId, int TreeCalMasId)
    {
        try
        {
            List<rCourseDistribution> list = CalenderUnitDistributionManager.GetCourseDistributionByProgramIdAndTreeCalMasId(ProgramId, TreeCalMasId);

            Program prgObj = ProgramManager.GetById(ProgramId);
            string prgName = "";
            if (prgObj != null)
                prgName = prgObj.DetailName;

            ReportParameter p1 = new ReportParameter("Program", prgName);
            ReportParameter p2 = new ReportParameter("Distribution", ddlTreeMasters.SelectedItem.Text.ToString() + "_" + ddlLinkedCalendars.SelectedItem.Text.ToString());
            if (list != null && list.Count != 0)
            {
                ReportViewer1.Visible = true;

                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Module/setup/Report/RptCalendarDistribution.rdlc");

                ReportViewer1.LocalReport.DataSources.Clear();
                this.ReportViewer1.LocalReport.SetParameters(new ReportParameter[] { p1, p2 });

                ReportDataSource rds1 = new ReportDataSource("CourseDistributionDataSet", list);
                ReportViewer1.LocalReport.DataSources.Add(rds1);
                ClearMessagelbl();
            }
            else
            {
                ReportViewer1.Visible = false;
                ShowMessage("No Data Found !", Color.Red);
            }
        }
        catch (Exception)
        {
            ReportViewer1.Visible = false;

            ShowMessage("Something Went Wrong !", Color.Red);
        }
    }
}
