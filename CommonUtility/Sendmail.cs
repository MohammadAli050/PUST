using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public class Sendmail
    {
        public static bool sendEmail(String senderName, String toAddr, String ccAddr, String subject, String body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.EnableSsl = true;

            client.Credentials = new NetworkCredential("bup.attendance@gmail.com", "12345678@#");

            MailAddress from = new MailAddress("bup.attendance@gmail.com", senderName);

            MailAddress to = new MailAddress(toAddr);

            MailMessage message = new MailMessage(from, to);

            if (ccAddr.Trim() != "")
            {
                string[] strArray = ccAddr.Trim().Split(new char[] { ';' });

                for (int i = 0; i < strArray.Length; i++)
                {
                    message.CC.Add(strArray[i].Trim());
                }
            }

            message.Subject = subject;

            message.IsBodyHtml = true;

            message.Body = body;

            try
            {
                client.Send(message);

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool sendEmailViaEduBup(String senderName, String toAddr, String ccAddr, String subject, String body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.UseDefaultCredentials = false;

            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("edusoft.bup@gmail.com", "edusoftconsultants#");
            client.Host = "smtp.gmail.com";

            MailAddress from = new MailAddress("edusoft.bup@gmail.com", senderName);

            MailAddress to = new MailAddress(toAddr);

            MailMessage message = new MailMessage(from, to);

            if (ccAddr.Trim() != "")
            {
                string[] strArray = ccAddr.Trim().Split(new char[] { ';' });

                for (int i = 0; i < strArray.Length; i++)
                {
                    message.CC.Add(strArray[i].Trim());
                }
            }

            message.Subject = subject;

            message.IsBodyHtml = true;

            message.Body = body;

            try
            {
                client.Send(message);

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }



        #region All New Methods

        public static bool sendEmailViaMBSTUSupportEmail(String senderName, String toAddr, String ccAddr, String subject, String body)
        {
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587);

            client.UseDefaultCredentials = false;

            client.EnableSsl = true;

            #region Email Credential
            //"ESCl.MBSTU@gmail.com"
            // EdusoftConsultant@MBSTU
            // App Password :xwxwavjujenuslls
            // Recovery Phone : 01615040699
            // Recover Email : mali131050@gmail.com

            //"ESCl.MBSTU2@gmail.com"
            // EdusoftConsultant@MBSTU
            // App Password :vqriwqcddbqjpfjj
            // Recovery Phone : 01615040699
            // Recover Email : mali131050@gmail.com

            //"ESCl.MBSTU3@gmail.com"
            // EdusoftConsultant@MBSTU
            // App Password :tnkpftayqknvlkun
            // Recovery Phone : 01615040699
            // Recover Email : mali131050@gmail.com
            

            #endregion

            DateTime CurrentDate = DateTime.Now;


            string MailSendingEmail = "",Password="";

            try
            {
                int Minutes = Convert.ToInt32(CurrentDate.Minute);

                if (Minutes < 30)
                {
                    MailSendingEmail = "ESCl.MBSTU@gmail.com";
                    Password = "xwxwavjujenuslls";
                }
                else 
                {
                    MailSendingEmail = "ESCl.MBSTU2@gmail.com";
                    Password = "vqriwqcddbqjpfjj";
                }
                //else
                //{
                //    MailSendingEmail = "ESCl.MBSTU3@gmail.com";
                //    Password = "tnkpftayqknvlkun";
                //}
            }
            catch (Exception ex)
            {
            }



            client.Credentials = new NetworkCredential(MailSendingEmail, Password);

            client.Host = "smtp.gmail.com";

            MailAddress from = new MailAddress(MailSendingEmail, senderName);

            MailAddress to = new MailAddress(toAddr);

            MailMessage message = new MailMessage(from, to);

            if (ccAddr.Trim() != "")
            {
                string[] strArray = ccAddr.Trim().Split(new char[] { ';' });

                for (int i = 0; i < strArray.Length; i++)
                {
                    message.CC.Add(strArray[i].Trim());
                }
            }

            message.Subject = subject;

            message.IsBodyHtml = true;

            message.Body = body;

            try
            {
                client.Send(message);

                return true;
            }

            catch (Exception ex)
            {
                return false;
            }
        }


        #endregion



    }
}
