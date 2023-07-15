using CommonUtility;
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
    public partial class PasswordRecovery : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }


        [WebMethod]
        public static string CheckLoginID(string loginID)
        {
            string errMessage;
            try
            {

                int flag = PasswordResetURLInfoManager.CheckLoginIdAndOfficialEmailAddressValidity(loginID);
                if (flag == 0)
                {
                    errMessage = "Enter a valid Login ID /  Email Address !";
                }
                else if (flag == 1)
                {
                    errMessage = "কোন প্রাতিষ্ঠানিক ইমেইল এড্রেস দেওয়া নাই। বিভাগে যোগাযোগ করুন।";
                }
                else if (flag == 2)
                {
                    errMessage = "You have already requested for password recovery URL.You can request for password recovery URL once every 10 minutes!";
                }
                else
                {
                    PasswordResetURLInfo passRecoveryInfo = PasswordResetURLInfoManager.GetByLoginId(loginID);

                    bool isSent = SendURLInEmail(passRecoveryInfo);
                    if (isSent)
                    {
                        errMessage = "A password recovery URL will be sent to your official email address " + passRecoveryInfo.OfficialEmail + " within a few minutes. URL will be active for next 10 minutes. Try again after a minutes if you don't get the email! ";
                    }
                    else
                    {
                        errMessage = "An error occurred with Email delivery. Please try again in a few minutes. Contact with Program Section Officer if you don't get email after you try twice or more!";
                    }
                }

                return errMessage;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static bool SendURLInEmail(PasswordResetURLInfo passRecoveryInfo)
        {
            try
            {
                bool isSent = false;
                string body = "We have sent this email in response to your request to reset password for your Login ID is " + passRecoveryInfo.LoginId + " <br /><br />" +
            "Please go to the following page and choose a new password. This link will be expired after 10 minutes from now.Also once you reset your password using this link, this link will be expired !<br /><br />" +
                    // "http://119.148.54.152/ResetPassword.aspx?Id=" + passRecoveryInfo.GeneratedId.ToString() + " <br /><br />" +
                "http://ucam.mbstu.ac.bd/ResetPassword.aspx?Id=" + passRecoveryInfo.GeneratedId.ToString() + " <br /><br />" +
            "If you didn't request this change, you can disregard this email - we have not yet reset your password.";

                if (passRecoveryInfo != null)
                {
                    isSent = Sendmail.sendEmailViaMBSTUSupportEmail("MBSTU Support", passRecoveryInfo.OfficialEmail, "MBSTU.support@gmail.com", "UCAM Password Recovery", body);


                    if (isSent == true)
                    {
                        passRecoveryInfo.IsEmailSend = true;
                        PasswordResetURLInfoManager.Update(passRecoveryInfo);
                    }
                }

                return isSent;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}