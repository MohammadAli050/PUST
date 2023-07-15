using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS
{
    public partial class ResetPassword : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //lblmsg.Text = "Password should be developed using the following rule" + "<br />"
            //    + "a) Minimum 8 characters" + "<br />"
            //    + "b) One uppercase alphabet" + "<br />"
            //    + "c) One lowercase alphabet" + "<br />"
            //    + "d) One numeric" + "<br />"
            //    + "e) One special character  !@#$%^&*"
            //    ;

            int Id = Convert.ToInt32(Request.QueryString["Id"]);

            HttpContext.Current.Session["GeneratedID"] = null;
            HttpContext.Current.Session["GeneratedID"] = Id;

            PasswordResetURLInfo passResetURLInfo = PasswordResetURLInfoManager.GetById(Id);

            if (passResetURLInfo != null)
            {

            }
            else
            {
                Response.Redirect("~/PageExpiredURL.aspx");
            }

            if (!IsPostBack)
            {

            }

        }


        [WebMethod]
        public static string UpdatePassword(string ChangedPassword)
        {
            try
            {
                string errMessage = "";

                int GeneratedId = Convert.ToInt32(HttpContext.Current.Session["GeneratedID"]);
                PasswordResetURLInfo passResetURLInfo = PasswordResetURLInfoManager.GetById(GeneratedId);

                if (passResetURLInfo != null)
                {
                    User user = UserManager.GetByLogInId(passResetURLInfo.LoginId);

                    user.Password = ChangedPassword;
                    user.ModifiedBy = 1;
                    user.ModifiedDate = DateTime.Now;

                    bool isUpdate = UserManager.Update(user);
                    if (isUpdate)
                    {
                        passResetURLInfo.IsPasswordReset = true;
                        PasswordResetURLInfoManager.Update(passResetURLInfo);

                        errMessage = "Your password has been changed successfully!";
                    }
                }


                return errMessage;
            }
            catch (Exception)
            {
                return "";
            }
        }


        public void SendEmail()
        {
            try
            {

            }
            catch (Exception)
            { }
        }
    }
}