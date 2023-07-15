using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Common;
//using BussinessObject;
using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using CommonUtility;
using System.Globalization;

public partial class MainMasterPageV2 : BaseMasterPage
{
    #region Functions

    //private void MakeMenu(List<BussinessObject.Role_Menu> menuPermissions)
    //{
    //    BussinessObject.UIUMSUser boUser = new BussinessObject.UIUMSUser();
    //    boUser = (BussinessObject.UIUMSUser)base.GetFromSession(Constants.SESSIONCURRENT_USER);

    //    if (menuPermissions != null || menuPermissions.Count > 0)
    //    {
    //        menuPermissions = menuPermissions.OrderBy(m => m.Menu.Sequence).ToList();

    //        foreach (BussinessObject.Role_Menu menuPermission in menuPermissions)
    //        {

    //            if (menuPermission.Menu.ParentID == 0)
    //            {
    //                MenuItem mnuItem = new MenuItem(menuPermission.Menu.Name, menuPermission.Menu.Id.ToString());
    //                mnuItem.ToolTip = menuPermission.Menu.URL +"?mmi=" + UtilityManager.Encrypt( menuPermission.Menu.Id.ToString());
    //                mnuItem.NavigateUrl = menuPermission.Menu.URL +"?mmi=" +UtilityManager.Encrypt( menuPermission.Menu.Id.ToString());
    //                menuMain.Items.Add(mnuItem);
    //            }
    //        }

    //        #region Load Special Menu
    //        List<LogicLayer.BusinessObjects.UserMenu> userMenus = LogicLayer.BusinessLogic.UserMenuManager.GetAll(boUser.Id).Where(d => d.ValidTo.Value.Date >= DateTime.Now.Date).ToList();
           
    //        if (userMenus != null && userMenus.Count > 0)
    //        {
    //            MenuItem specialMenu = new MenuItem("Others *", "special");
    //            specialMenu.ToolTip = "~/SpecialMenu/SpecialMenuHome.aspx?mmi=" + UtilityManager.Encrypt("-1");
    //            specialMenu.NavigateUrl = "~/SpecialMenu/SpecialMenuHome.aspx?mmi=" + UtilityManager.Encrypt("-1");
    //            menuMain.Items.Add(specialMenu);
    //        }
    //        #endregion
    //    }
    //}

    //private void MakeSuperMenu(List<BussinessObject.UIUMSMenu> menuPermissions)
    //{
    //    if (menuPermissions != null || menuPermissions.Count > 0)
    //    {
    //        base.RemoveFromSession(Constants.SESSIONSUPERUSERMENU);
    //        base.AddToSession(Constants.SESSIONSUPERUSERMENU, menuPermissions);

    //        foreach (BussinessObject.UIUMSMenu menuPermission in menuPermissions)
    //        {
    //            if (menuPermission.ParentID == 0)
    //            {
    //                MenuItem mnuItem = new MenuItem(menuPermission.Name, menuPermission.Id.ToString());
    //                mnuItem.ToolTip = menuPermission.URL;
    //                menuMain.Items.Add(mnuItem);
    //            }
    //        }
    //    }
    //}

    private void MakeMenu(List<LogicLayer.BusinessObjects.Menu> menuList)
    {
        menuList = menuList.Where(x => x.Menu_ID != 356 && x.Menu_ID != 437).ToList();
        if (menuList != null && menuList.Count > 0)
        {
            menuList = menuList.OrderBy(m => m.Name).ToList();

            foreach (LogicLayer.BusinessObjects.Menu menu in menuList)
            {

                if (menu.ParentMnu_ID == 0)
                {
                    MenuItem mnuItem = new MenuItem(menu.Name, menu.Menu_ID.ToString());
                    //mnuItem.ToolTip = menuPermission.Menu.URL + "?mmi=" + UtilityManager.Encrypt(menuPermission.Menu.Id.ToString());
                    mnuItem.NavigateUrl = menu.URL + "?mmi=" + UtilityManager.Encrypt(menu.Menu_ID.ToString());
                    menuMain.Items.Add(mnuItem);

                    List<LogicLayer.BusinessObjects.Menu> childMenuList = menuList.Where(m => m.ParentMnu_ID == menu.Menu_ID).ToList();

                    if (childMenuList != null && childMenuList.Count > 0)
                    {
                        AddChildItems(menuList, childMenuList, mnuItem);

                    }
                }
            }
        }
    }

    private void AddChildItems(List<LogicLayer.BusinessObjects.Menu> totalMenuList, List<LogicLayer.BusinessObjects.Menu> menuList, MenuItem menuItem)
    {
        foreach (LogicLayer.BusinessObjects.Menu menu in menuList)
        {
            MenuItem mnuItem = new MenuItem(menu.Name, menu.Menu_ID.ToString());
            //mnuItem.ToolTip = menuPermission.URL + "?mmi=" + UtilityManager.Encrypt(menuPermission.Id.ToString());
            mnuItem.NavigateUrl = menu.URL + "?mmi=" + UtilityManager.Encrypt(menu.Menu_ID.ToString());
            menuItem.ChildItems.Add(mnuItem);

            List<LogicLayer.BusinessObjects.Menu> childMenuList = totalMenuList.Where(m => m.ParentMnu_ID == menu.Menu_ID).ToList();

            if (childMenuList != null && childMenuList.Count > 0)
            {
                AddChildItems(totalMenuList, childMenuList, mnuItem);
            }
        }
    }


    #endregion

    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (base.IsSessionVariableExists(Constants.SESSIONCURRENT_USER))
                {
                    BussinessObject.UIUMSUser boUser = new BussinessObject.UIUMSUser();
                    boUser = (BussinessObject.UIUMSUser)base.GetFromSession(Constants.SESSIONCURRENT_USER);

                    if (boUser.RoleID > 0)
                    {
                        //MakeMenu(BussinessObject.Role_Menu.GetMenusByRoleID(boUser.RoleID));
                        MakeMenu(MenuManager.GetByRoleId(boUser.RoleID));
                    }
                    else
                    {
                        //MakeSuperMenu(BussinessObject.UIUMSMenu.Get());
                    }

                    User user = UserManager.GetByLogInId(boUser.LogInID);
                    if (user != null)
                    {
                        Person person = PersonManager.GetByUserId(user.User_ID);
                        if(person != null)
                        {
                            lblAvatarName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(person.FullName.ToString().ToLower());
                            if (person.Employee != null && !string.IsNullOrEmpty(person.PhotoPath))
                            {
                                imgAvatar.ImageUrl = "~/Upload/Avatar/Teacher/" + person.PhotoPath;
                            }
                            else if (person.PhotoPath != null && person.PhotoPath != "")
                            {
                                imgAvatar.ImageUrl = "~/Upload/Avatar/" + person.PhotoPath;
                            }
                            else
                            {
                                if (imgAvatar.ImageUrl.Contains("avatarMale"))
                                {
                                    if (person.Gender != null)
                                    {
                                        if (person.Gender.ToLower() == "female")
                                            imgAvatar.ImageUrl = imgAvatar.ImageUrl.Replace("avatarMale", "avatarFemale");
                                        else
                                            imgAvatar.ImageUrl = imgAvatar.ImageUrl.Replace("avatarMale", "avatarMale");
                                    }
                                }
                                else if (imgAvatar.ImageUrl.Contains("avatarFemale"))
                                {
                                    if (person.Gender != null)
                                    {
                                        if (person.Gender.ToLower() == "male")
                                            imgAvatar.ImageUrl = imgAvatar.ImageUrl.Replace("avatarFemale", "avatarMale");
                                        else
                                            imgAvatar.ImageUrl = imgAvatar.ImageUrl.Replace("avatarFemale", "avatarMale");
                                    }
                                }
                            }
                        }
                        else
                        {
                            lblAvatarName.Text = "---||---";
                        }
                    }
                        
                    lbtnUserName.Text = boUser.LogInID;
                    LoadCalenderIsActive();
                    LoadCalenderIsRegistration();
                }
                else
                {
                    Response.Redirect(Constants.URLLOGINPAGE);
                }
            }

        }
        catch (Exception Ex)
        {
            // Utilities.ShowMassage(lblMsg, Color.Red, Ex.Message);
        }
    }

    private void LoadCalenderIsActive()
    {
        List<AcademicCalender> collection = AcademicCalenderManager.GetIsCurrentAllSession().ToList();

        string currentSession = "";
        int f = 0;

        foreach (AcademicCalender item in collection)
        {
            if (f != 0)
            {
                currentSession += " / ";
            }

            currentSession += item.FullCode;

            if (item.CalenderUnitTypeID == 1)
            {
                currentSession += " - Term";
            }
            else if (item.CalenderUnitTypeID == 2)
            {
                currentSession += " - Sem";
            }
            else 
            {
                currentSession += " ";
            }

            f = 1;
        }

        lblCurrent.Text = currentSession; 
    }

    private void LoadCalenderIsRegistration()
    {

        List<AcademicCalender> collection = AcademicCalenderManager.GetIsActiveRegistrationAllSession().ToList();

        string currentSession = "";
        int f = 0;

        foreach (AcademicCalender item in collection)
        {
            if (f != 0)
            {
                currentSession += " / ";
            }

            currentSession += item.FullCode;

            if (item.CalenderUnitTypeID == 1)
            {
                currentSession += " - Term";
            }
            else if (item.CalenderUnitTypeID == 2)
            {
                currentSession += " - Sem";
            }
            else
            {
                currentSession += " ";
            }

            f = 1;
        }

        lblRegistration.Text = currentSession; 
         
    }


    protected void mnuMain_MenuItemClick(object sender, MenuEventArgs e)
    {
        try
        {
            //if (base.IsSessionVariableExists(Constants.SESSIONMSTRMENUID))
            //{
            //    base.RemoveFromSession(Constants.SESSIONMSTRMENUID);
            //}
            //base.AddToSession(Constants.SESSIONMSTRMENUID, menuMain.SelectedItem.Value);
            //Response.Redirect(menuMain.SelectedItem.ToolTip, false);

        }
        catch (Exception Ex)
        {
            // Utilities.ShowMassage(lblMsg, Color.Red, Ex.Message);
        }
    }

    protected void lbtnLogOut_Click(object sender, EventArgs e)
    {
        string loginId = SessionManager.GetObjFromSession<string>(Constants.SESSIONCURRENT_LOGINID);
        LogLoginLogoutManager.Insert(DateTime.Now, loginId, "Log out", "Successful", "", ""); 
        base.RemoveFromSession(Constants.SESSIONCURRENT_USER);
        base.RemoveFromSession(Constants.SESSIONMENUID);
        base.RemoveFromSession(Constants.SESSIONMSTRMENUID);
        FormsAuthentication.SignOut();
        Session.Abandon();
        Response.Redirect("~/Security/Login.aspx", true);
    }
    protected void lbtnHome_Click(object sender, EventArgs e)
    {

        Response.Redirect("~/Security/Home.aspx");
    }
    #endregion

    protected void lbtnUserName_Click(object sender, EventArgs e)
    {

    }
}
