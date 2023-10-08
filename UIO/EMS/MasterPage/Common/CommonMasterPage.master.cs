using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Common;
using bo = BussinessObject;
using System.Drawing;
using LogicLayer.BusinessLogic;
using CommonUtility;
using LogicLayer.BusinessObjects;
using System.Globalization;

public partial class CommonMasterPage : BaseMasterPage
{
    #region Events
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblRoleName.Text = "";
                bo.UIUMSUser boUser = new bo.UIUMSUser();
                boUser = (bo.UIUMSUser)base.GetFromSession(Constants.SESSIONCURRENT_USER);

                if (boUser != null)
                {
                    //LoadRoot(boUser, menuMain, Convert.ToInt32(base.GetFromSession(Constants.SESSIONMSTRMENUID)));
                    MakeMenu(MenuManager.GetByRoleId(boUser.RoleID));
                    if (Convert.ToInt32(base.GetFromSession(Constants.SESSIONMSTRMENUID)) == -1)
                    {
                        base.LoadSpecialMenu(boUser, menuMain);
                    }

                    User user = UserManager.GetByLogInId(boUser.LogInID);
                    if (user != null)
                    {
                        Person person = PersonManager.GetByUserId(user.User_ID);
                        if (person != null)
                        {
                            lblAvatarName.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(person.FullName.ToString().ToLower());
                            if (person.Employee != null && !string.IsNullOrEmpty(person.PhotoPath))
                            {
                                imgAvatar.ImageUrl = "~/Upload/Avatar/Teacher/" + person.PhotoPath;
                            }
                            else if (person.PhotoPath != null && person.PhotoPath != "")
                            {
                                imgAvatar.ImageUrl = "~/Upload/Avatar/Student/" + person.PhotoPath;
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

                    #region Role Name

                    try
                    {
                        Role roleObj = RoleManager.GetById(boUser.RoleID);
                        if (roleObj != null)
                            lblRoleName.Text = "My Role : " + roleObj.RoleName;
                    }
                    catch (Exception ex)
                    {
                    }

                    #endregion

                }
            }
        }
        catch (Exception Ex)
        {
            //Utilities.ShowMassage(lblMsg, Color.Red, Ex.Message);
        }
    }
    //
    //
    //

    private void MakeMenu(List<LogicLayer.BusinessObjects.Menu> menuList)
    {
        if (menuList != null && menuList.Count > 0)
        {
            //menuList = menuList.OrderBy(m => m.Name).ToList();
            menuList = menuList.OrderBy(m => m.Sequence).ToList();

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


    public void LoadRootSelf(bo.UIUMSUser user, System.Web.UI.WebControls.Menu mnuAdmin, int menuID)
    {
        MenuItem mnuItem = null;

        if (user.RoleID > 0)
        {
            var menuList = from element in user.RoleMenus
                           where element.Menu.ParentID == menuID
                           select element;

            mnuAdmin.Items.Clear();

            if (menuList.Count() > 0)
            {
                foreach (BussinessObject.Role_Menu rMenu in menuList)
                {
                    mnuItem = new MenuItem(rMenu.Menu.Name, rMenu.Menu.Id.ToString());
                    mnuItem.NavigateUrl = rMenu.Menu.URL + "?mmi=" + UtilityManager.Encrypt(rMenu.Menu.Id.ToString());
                    mnuAdmin.Items.Add(mnuItem);
                    LoadChild(rMenu.Menu.ChildMenus, mnuItem, user.RoleMenus);
                }
            }
        }
        else
        {
            List<bo.UIUMSMenu> menus = (List<bo.UIUMSMenu>)GetFromSession(Constants.SESSIONSUPERUSERMENU);
            if (menus != null)
            {
                var menuList = from element in menus
                               where element.ParentID == menuID
                               select element;

                mnuAdmin.Items.Clear();

                if (menuList.Count() > 0)
                {
                    foreach (bo.UIUMSMenu menu in menuList)
                    {
                        mnuItem = new MenuItem(menu.Name, menu.Id.ToString());
                        mnuItem.NavigateUrl = menu.URL + "?mmi=" + UtilityManager.Encrypt(menu.Id.ToString());
                        mnuAdmin.Items.Add(mnuItem);
                        LoadChild(menu.ChildMenus, mnuItem, user.RoleMenus);
                    }
                }
            }
        }
    }

    private void LoadChild(List<bo.UIUMSMenu> childMenus, MenuItem mnuItem, List<bo.Role_Menu> RoleMenuList)
    {
        if (childMenus != null && childMenus.Count > 0)
        {
            foreach (bo.UIUMSMenu childMenu in childMenus)
            {
                BussinessObject.Role_Menu emn = RoleMenuList.Where(r => r.MenuID == childMenu.Id).SingleOrDefault();

                if (emn != null)
                {
                    if (childMenu.URL.Trim().Length > 0)
                    {
                        MenuItem mnuChild = new MenuItem(childMenu.Name, childMenu.Id.ToString());
                        mnuChild.Selectable = true;
                        mnuChild.NavigateUrl = childMenu.URL + "?mmi=" + UtilityManager.Encrypt(childMenu.Id.ToString());
                        mnuItem.ChildItems.Add(mnuChild);
                        LoadChild(childMenu.ChildMenus, mnuChild);
                    }
                }
            }
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

    protected void lbtnLogOut_Click(object sender, EventArgs e)
    {
        string loginId = SessionManager.GetObjFromSession<string>(Constants.SESSIONCURRENT_LOGINID);
        LogLoginLogoutManager.Insert(DateTime.Now, loginId, "Log out", "Successful", "", "");
        base.RemoveFromSession(Constants.SESSIONCURRENT_USER);
        base.RemoveFromSession(Constants.SESSIONMENUID);
        base.RemoveFromSession(Constants.SESSIONMSTRMENUID);
        FormsAuthentication.SignOut();
        Session.Abandon();

        Response.Redirect("~/Security/Login.aspx");
    }

    protected void lbtnHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Security/Home.aspx");
    }

    protected void lbtnUserName_Click(object sender, EventArgs e)
    {

    }
    #endregion
}
