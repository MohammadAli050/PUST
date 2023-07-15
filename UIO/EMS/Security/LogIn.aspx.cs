using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Common;
using BussinessObject;
using CommonUtility;
using LogicLayer.BusinessLogic;

public partial class Security_Default : BasePage
{
    static readonly object lockThis = new object();

    #region Functions
    private UIUMSUser MakeSysAdmin(UIUMSUser su)
    {
        su = new UIUMSUser();
        su.LogInID = "SuperUser";
        su.Password = "x";
        su.IsActive = true;
        su.RoleID = 0;
        su.RoleExistStartDate = DateTime.Now;
        su.RoleExistEndDate = DateTime.MaxValue;
        su.CreatorID = -2;
        su.CreatedDate = DateTime.Now;
        //sysAdmin.ModifierID = -1;
        //sysAdmin.ModifiedDate = DateTime.Now;

        UIUMSUser.SaveSysAdmin(su);
        return su;
    }
    #endregion

    #region Events
    protected void Page_PreInit(object sender, EventArgs e)
    {
        if (Page.Request.ServerVariables["http_user_agent"].ToLower().Contains("safari"))
        {
            Page.ClientTarget = "uplevel";
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void logMain_Authenticate(object sender, AuthenticateEventArgs e)
    {
        try
        {
            //this Authentication is made in web.config. it is used for system admin user.
            //if (FormsAuthentication.Authenticate(logMain.UserName, logMain.Password))
            //{
            //    UIUMSUser su = null;
            //    if (!UIUMSUser.IsExist("SuperUser"))
            //    {
            //        su = MakeSysAdmin(su);
            //    }
            //    else
            //    {
            //        su = UIUMSUser.GetByLogInID("SuperUser", false);
            //    }
            //    UIUMSUser.CurrentUser = su;

            //    base.AddToSession(Constants.SESSIONCURRENT_USER, UIUMSUser.CurrentUser);
            //    FormsAuthentication.RedirectFromLoginPage(logMain.UserName, false);
            //}
            //else 

            lock (lockThis)
            {

                if (UIUMSUser.Login(logMain.UserName, logMain.Password))
                {
                    UIUMSUser userObj = UIUMSUser.GetByLogInID(logMain.UserName, true);

                    LogLoginLogoutManager.Insert(DateTime.Now, userObj.LogInID, "Login", "Successful", GetIPAddress(), UtilityManager.Encrypt(logMain.Password));
                    base.AddToSession(Constants.SESSIONCURRENT_USER, userObj);
                    base.AddToSession(Constants.SESSIONCURRENT_LOGINID, userObj.LogInID);
                    base.AddToSession(Constants.SESSIONCURRENT_ROLEID, userObj.RoleID);


                    // FormsAuthentication.RedirectFromLoginPage(logMain.UserName, false);
                    //HttpCookie userInfoCookie = new HttpCookie(ConstantValue.Cookie_Authentication);
                    //userInfoCookie["UserName"] = logMain.UserName;
                    //userInfoCookie["UserPassword"] = logMain.Password;
                    ////userInfoCookie["MainMenue"] = "0";
                    //Response.Cookies.Add(userInfoCookie);


                    /////////////////////////////////////////////////////////////////

                    //CheckBox ckBox = (CheckBox)logMain.FindControl("chkRememberMe");
                    //if (ckBox.Checked)
                    //{
                    //    string uid = UtilityManager.Encrypt(logMain.UserName);
                    //    string pwd = UtilityManager.Encrypt(logMain.Password);
                    //    FormsAuthenticationTicket ticket_uid = new FormsAuthenticationTicket(1, uid, DateTime.Now, DateTime.Now.AddMinutes(60), false, pwd, FormsAuthentication.FormsCookiePath);

                    //    string encTicket_uid = FormsAuthentication.Encrypt(ticket_uid);

                    //    Response.Cookies.Add(new HttpCookie(FormsAuthentication.FormsCookieName, encTicket_uid));
                    //}

                    /////////////////////////////////////////////////////////////////



                    if (userObj.RoleID == 4) // Hardcode: Check that user is student. 
                    {
                        if (userObj.LogInID == userObj.Password)
                        {
                            string loginid = userObj.LogInID;
                            userObj = null;
                            //userInfoCookie.Expires = DateTime.Now;                       
                            base.RemoveFromSession(Constants.SESSIONCURRENT_USER);

                            Response.Redirect("~/UserManagement/Admin/PasswordChange.aspx?loginid=" + loginid);
                        }
                        //else
                        //{
                        //    Response.Redirect("~/Module/FormFillUp/StudentFormFillUpApply.aspx" + "?mmi=" + UtilityManager.Encrypt("-1"), false);
                        //}
                    }
                    //else if (userObj.RoleID == 11)// Department of Chairman
                    //{
                    //    Response.Redirect("~/Module/FormFillUp/FormFillUpApplicationManage.aspx" + "?mmi=" + UtilityManager.Encrypt("-1"), false);
                    //}
                    //else if (userObj.RoleID == 12)// Hall Provost
                    //{
                    //    Response.Redirect("~/Module/FormFillUp/FormFillUpApplicationManageByHallProvost.aspx" + "?mmi=" + UtilityManager.Encrypt("-1"), false);
                    //}
                    //else if (userObj.RoleID == 13)// Academic Section
                    //{
                    //    Response.Redirect("~/Module/FormFillUp/FormFillUpApplicationManageByAcademicSection.aspx" + "?mmi=" + UtilityManager.Encrypt("-1"), false);
                    //}
                    else
                    {
                        Response.Redirect("Home.aspx" + "?mmi=" + UtilityManager.Encrypt("-1"), false);
                    }
                }
                else
                {
                    logMain.FailureText = "You are not authorized to run this application!</br>Try with "
                        + "correct user name and password. ";
                    LogLoginLogoutManager.Insert(DateTime.Now, logMain.UserName, "Login", "Failed ", GetIPAddress(), logMain.Password);
                }
            }
        }
        catch (Exception Ex)
        {
            logMain.FailureText = Ex.Message;
            LogLoginLogoutManager.Insert(DateTime.Now, logMain.UserName, "Login", "Failed ", GetIPAddress(), logMain.Password);
        }
    }

    protected string GetIPAddress()
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipAddress))
        {
            string[] addresses = ipAddress.Split(',');
            if (addresses.Length != 0)
            {
                return addresses[0];
            }
        }

        return context.Request.ServerVariables["REMOTE_ADDR"];

    }
    #endregion
}
