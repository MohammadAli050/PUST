using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class UserControl_ProgramUserControl : System.Web.UI.UserControl
{
    public event EventHandler ProgramSelectedIndexChanged;
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
            selectedValue = ddlProgram.SelectedValue;
            selectedText = ddlProgram.SelectedItem.Text;
        }
        catch (Exception)
        {
        }
    }

    public void LoadDropDownList()
    {
        List<Program> programList = new List<Program>();
        programList = ProgramManager.GetAll();
        programList = programList.OrderBy(x => x.ShortName).ToList();
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;

        if (programList != null)
        {
            //programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();
            ddlProgram.Items.Add(new ListItem("-Select-", "0"));
            ddlProgram.DataTextField = "ShortName";
            ddlProgram.DataValueField = "ProgramID";

            ddlProgram.DataSource = programList;
            ddlProgram.DataBind();

            ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
            selectedValue = programList.FirstOrDefault().ProgramID.ToString();
            selectedText = programList.FirstOrDefault().Attribute1;
        }
    }

    public void LoadDropDownListWithoutSelectionProgram()
    {
        List<Program> programList = new List<Program>();
        programList = ProgramManager.GetAll();
        programList = programList.OrderBy(x => x.ShortName).ToList();
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;

        ddlProgram.Items.Add(new ListItem("-Select-", "-1"));
        if (programList != null)
        {
            //programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();
            
            ddlProgram.DataTextField = "ShortName";
            ddlProgram.DataValueField = "ProgramID";

            ddlProgram.DataSource = programList;
            ddlProgram.DataBind();

            //ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
            //selectedValue = programList.FirstOrDefault().ProgramID.ToString();
            //selectedText = programList.FirstOrDefault().Attribute1;
        }
    }

    public void LoadDropDownListWithoutSelectionProgramAll()
    {
        List<Program> programList = new List<Program>();
        programList = ProgramManager.GetAll();
        programList = programList.OrderBy(x => x.ShortName).ToList();
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;

        ddlProgram.Items.Add(new ListItem("--All--", "0"));
        if (programList != null)
        {
            //programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();

            ddlProgram.DataTextField = "ShortName";
            ddlProgram.DataValueField = "ProgramID";

            ddlProgram.DataSource = programList;
            ddlProgram.DataBind();

            //ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
            //selectedValue = programList.FirstOrDefault().ProgramID.ToString();
            //selectedText = programList.FirstOrDefault().Attribute1;
        }
    }

    public void LoadDropDownListOld()
    {
        List<Program> programList = new List<Program>();
        programList = ProgramManager.GetAll();

        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;

        if (programList != null)
        {
            programList = programList.OrderBy(o => o.Attribute1).ToList();
            ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
            ddlProgram.DataTextField = "ShortName";
            ddlProgram.DataValueField = "ProgramID";

            ddlProgram.DataSource = programList;
            ddlProgram.DataBind();

        }
    }


    //public void LoadDropDownList(int programId)
    //{
    //    List<Program> programList = new List<Program>();
    //    programList = ProgramManager.GetAll().Where(d => d.DeptID == programId).ToList();

    //    ddlProgram.Items.Clear();
    //    ddlProgram.AppendDataBoundItems = true;

    //    if (programList != null)
    //    {
    //        programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();
    //        ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
    //        ddlProgram.DataTextField = "ShortName";
    //        ddlProgram.DataValueField = "ProgramID";

    //        ddlProgram.DataSource = programList;
    //        ddlProgram.DataBind();

    //        ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
    //        selectedValue = programList.FirstOrDefault().ProgramID.ToString();
    //        selectedText = programList.FirstOrDefault().Attribute1;
    //    }
    //}

    //public void LoadDropDownListDefault()  // changed to public by HS, for use in BillManualEntry
    //{
    //    ddlProgram.Items.Clear();
    //    ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
    //}

    //public void LoadDropDownListBySession(int programTypeId)
    //{
    //    //AcademicCalender ac = AcademicCalenderManager.GetAll();

    //    List<Program> programList = new List<Program>();
    //    programList = ProgramManager.GetAll().Where(d => d.ProgramTypeID == programTypeId).ToList();

    //    ddlProgram.Items.Clear();
    //    ddlProgram.AppendDataBoundItems = true;

    //    if (programList != null)
    //    {
    //        programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();
    //        ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
    //        ddlProgram.DataTextField = "ShortName";
    //        ddlProgram.DataValueField = "ProgramID";

    //        ddlProgram.DataSource = programList;
    //        ddlProgram.DataBind();

    //        ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
    //        selectedValue = programList.FirstOrDefault().ProgramID.ToString();
    //        selectedText = programList.FirstOrDefault().Attribute1;
    //    }
    //}

    //public void LoadDropDownListBySessionOld(int programTypeId)
    //{
    //    //AcademicCalender ac = AcademicCalenderManager.GetAll();

    //    List<Program> programList = new List<Program>();
    //    programList = ProgramManager.GetAll().Where(d => d.ProgramTypeID == programTypeId).ToList();

    //    ddlProgram.Items.Clear();
    //    ddlProgram.AppendDataBoundItems = true;

    //    if (programList != null)
    //    {
    //        programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.Attribute1).ToList();
    //        ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
    //        ddlProgram.DataTextField = "ShortName";
    //        ddlProgram.DataValueField = "ProgramID";

    //        ddlProgram.DataSource = programList;
    //        ddlProgram.DataBind();

    //    }
    //}

    protected void ddlProgram_SelectedIndexChanged(object sender, EventArgs e)
    {
        selectedValue = ddlProgram.SelectedValue;
        selectedText = ddlProgram.SelectedItem.Text;

        if (ProgramSelectedIndexChanged != null)
            ProgramSelectedIndexChanged(this, e);
    }

    internal void SelectedValue(int id)
    {
        ddlProgram.SelectedValue = id.ToString();
        selectedValue = ddlProgram.SelectedValue;
        selectedText = ddlProgram.SelectedItem.Text;
    }

    internal void SelectedIndex(int id)
    {
        ddlProgram.SelectedIndex = id;
        selectedValue = ddlProgram.SelectedValue;
        selectedText = ddlProgram.SelectedItem.Text;
    }

    internal void Enable(bool TrueOrFalse)
    {
        ddlProgram.Enabled = TrueOrFalse;
    }

    public void LoadDropdownWithUserAccess(int userID)
    {
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;
        try
        {
            UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userID);
            if (uapObj != null && !string.IsNullOrEmpty(uapObj.AccessPattern))
            {
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
                    programList = programList.OrderBy(o => o.ProgramID).ToList();
                    ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
                    ddlProgram.DataTextField = "ShortName";
                    ddlProgram.DataValueField = "ProgramID";

                    ddlProgram.DataSource = programList;
                    ddlProgram.DataBind();

                    ddlProgram.SelectedValue = "0";//programList.FirstOrDefault().ProgramID.ToString();
                    selectedValue = programList.FirstOrDefault().ProgramID.ToString();
                    selectedText = programList.FirstOrDefault().Attribute1;
                }
            }
            else
            {
                //LoadDropDownList();
            }
        }
        catch { //LoadDropDownList(); 
        }

    }

    public void LoadDropdownWithUserAccessV2(int userID)
    {
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;
        try
        {
            UserAccessProgram uapObj = UserAccessProgramManager.GetByUserId(userID);
            if (uapObj != null && !string.IsNullOrEmpty(uapObj.AccessPattern))
            {
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
                    programList = programList.OrderBy(o => o.ProgramID).ToList();
                    ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
                    ddlProgram.DataTextField = "ShortName";
                    ddlProgram.DataValueField = "ProgramID";

                    ddlProgram.DataSource = programList;
                    ddlProgram.DataBind();

                    ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
                    selectedValue = programList.FirstOrDefault().ProgramID.ToString();
                    selectedText = programList.FirstOrDefault().Attribute1;
                }
            }
            else
            {
                LoadDropDownList();
            }
        }
        catch { LoadDropDownList(); }

    }

    public void LoadDropDownListByCalanderUnitMasterId(int CalendarUnitMasterId)
    {
        List<Program> programList = new List<Program>();
        //programList = ProgramManager.GetAllByCalendarUnitMasterId(CalendarUnitMasterId);
        programList = ProgramManager.GetAll();
        if (programList != null && programList.Count > 0)
        {

        }
        else
        {
            programList = ProgramManager.GetAll();
        }

        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;

        if (programList != null && programList.Count > 0)
        {
            programList = programList.OrderBy(o => o.Attribute1).ToList();
            ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
            ddlProgram.DataTextField = "ShortName";
            ddlProgram.DataValueField = "ProgramID";

            ddlProgram.DataSource = programList;
            ddlProgram.DataBind();

            ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
            selectedValue = programList.FirstOrDefault().ProgramID.ToString();
            selectedText = programList.FirstOrDefault().Attribute1;
        }
    }

    public void LoadDropdownByTeacherId(int TeacherId)
    {
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;
        try
        {
            //List<Program> programList = new List<Program>();
            List<Program> programList = ProgramManager.GetAllByTeacherId(TeacherId);
            //programList = ProgramManager.GetAll();
            if (programList != null)
            {
                //programList = programList.Where(d => d.ProgramTypeID == 0).OrderBy(o => o.ProgramID).ToList();
                ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
                ddlProgram.DataTextField = "ShortName";
                ddlProgram.DataValueField = "ProgramID";

                ddlProgram.DataSource = programList;
                ddlProgram.DataBind();

                ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
                selectedValue = programList.FirstOrDefault().ProgramID.ToString();
                selectedText = programList.FirstOrDefault().Attribute1;
            }

        }
        catch { }

    }

    public void LoadDropdownByDepartmentId(int departmentId)
    {
        ddlProgram.Items.Clear();
        ddlProgram.AppendDataBoundItems = true;
        try
        {
            List<Program> programList = new List<Program>();
            //List<Program> programList = ProgramManager.GetAllByTeacherId(TeacherId);
            programList = ProgramManager.GetAll();
            if (programList != null)
            {
                programList = programList.Where(d => d.DeptID == departmentId).OrderBy(o => o.ProgramID).ToList();
                ddlProgram.Items.Add(new ListItem("-Select Program-", "0"));
                ddlProgram.DataTextField = "ShortName";
                ddlProgram.DataValueField = "ProgramID";

                ddlProgram.DataSource = programList;
                ddlProgram.DataBind();

                ddlProgram.SelectedValue = programList.FirstOrDefault().ProgramID.ToString();
                selectedValue = programList.FirstOrDefault().ProgramID.ToString();
                selectedText = programList.FirstOrDefault().Attribute1;
            }

        }
        catch { }

    }


}