using LogicLayer.BusinessLogic;
using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EMS
{
    public partial class StudentImageMigration : BasePage
    {

        DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        BussinessObject.UIUMSUser userObj = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            base.CheckPage_Load();
            userObj = (BussinessObject.UIUMSUser)GetFromSession(Constants.SESSIONCURRENT_USER);

            if (!IsPostBack)
            {
                LoadImageNumberDDL();
            }
        }

        private void LoadImageNumberDDL()
        {
            try
            {
                for (int i = 1; i <= 1000; i++)
                {
                    ddlNumber.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnMigrate_Click(object sender, EventArgs e)
        {
            try
            {
                string rootFolderPath = @"D:/Photo/GivenPhoto/";
                string destinationPath = @"D:/Photo/MigratedPhoto/";
                DirectoryInfo dir = new DirectoryInfo(rootFolderPath);
                FileInfo[] fileList = dir.GetFiles();
                int i = 1;
                int count = 0;
                int length = Convert.ToInt32(ddlNumber.SelectedValue);
                foreach (FileInfo file in fileList)
                {
                    if (i <= length)
                    {
                        string fileToMove = rootFolderPath + file;
                        string Name = "";
                        Name = Path.GetFileNameWithoutExtension(file.Name);
                        if (Name != "")
                        {
                            string PersonId = "";
                            var std = ucamContext.Students.Where(x => x.Attribute3 == Name).FirstOrDefault();
                            if (std != null)
                            {
                                destinationPath = @"D:/Photo/MigratedPhoto/";
                                PersonId = std.PersonID.ToString() + ".jpg";
                                string moveTo = destinationPath + PersonId;
                                //moving file
                                File.Move(fileToMove, moveTo);
                                count++;
                            }
                            else
                            {
                                PersonId = Name + ".jpg";
                                destinationPath = @"D:/Photo/StudentNotFound/";
                                string moveTo = destinationPath + PersonId;
                                //moving file
                                File.Move(fileToMove, moveTo);
                            }

                        }
                        i++;
                    }

                }

                if (count > 0)
                    showAlert(count + " Students Image Converted Successfully");
            }
            catch (Exception ex)
            {

            }
        }


        protected void showAlert(string msg)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "SweetAlert", "swal('" + msg + "');", true);

        }
    }
}