using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS.Module
{
    public partial class ViewPDF : BasePage
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            if (Page.Request.ServerVariables["http_user_agent"].ToLower().Contains("safari"))
            {
                Page.ClientTarget = "uplevel";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!this.IsPostBack)
                {
                    string filePath = Server.MapPath("~/Module/Pdf/") + Request.QueryString["FN"];
                    lblPath.Text = filePath;

                    if (File.Exists(filePath))
                    {
                        this.Response.ContentType = "application/pdf";
                        this.Response.AppendHeader("Content-Disposition;", "attachment;filename=" + Request.QueryString["FN"]);
                        this.Response.WriteFile(filePath);
                        this.Response.Flush();

                        File.Delete(filePath);
                    }
                }
            }
            catch (Exception Ex)
            {
                throw Ex;

            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {

        }
    }
}