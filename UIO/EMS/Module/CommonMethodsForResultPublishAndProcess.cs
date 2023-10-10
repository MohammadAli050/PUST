using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EMS.Module
{
    public class CommonMethodsForResultPublishAndProcess
    {
        static DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        public static int ResultPublish(int HeldInRelationId,string LoginId,int UserId)
        {
            int ReturnValue = 0;
            try
            {
                // Get All Section List

                var SectionList = ucamContext.AcademicCalenderSections.Where(x => x.HeldInRelationId == HeldInRelationId).ToList();

                if (SectionList != null && SectionList.Any())
                {
                    foreach (var SectionItem in SectionList)
                    {
                        try
                        {
                            int SectionId = Convert.ToInt32(SectionItem.AcaCal_SectionID);

                            var SectionStatusObj = ucamContext.SectionWiseResultSubmissionStatus.Where(x => x.AcacalSectionId == SectionId).FirstOrDefault();

                            // Only 100% Marks Final Submitted Section Will Be Published

                            if (SectionStatusObj != null && SectionStatusObj.StatusId == 5)
                            {
                                // Get All Student List of this Section With Marks

                                List<SqlParameter> parameters1 = new List<SqlParameter>();
                                parameters1.Add(new SqlParameter { ParameterName = "@SectionId", SqlDbType = System.Data.SqlDbType.Int, Value = SectionId });
                                parameters1.Add(new SqlParameter { ParameterName = "@LoginId", SqlDbType = System.Data.SqlDbType.NVarChar, Value = LoginId });
                                parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserId });

                                DataTable dt = DataTableManager.GetDataFromQuery("StudentExamMarkPublishBySectionId", parameters1);

                                if (dt != null && dt.Rows.Count > 0)
                                {
                                    ReturnValue++;

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }

            }
            catch (Exception ex)
            {
            }

            return ReturnValue;
        }

        public static void ResultProcess(int HeldInRelationId,int StudentId,int UserId)
        {
            try
            {

                List<SqlParameter> parameters1 = new List<SqlParameter>();
                parameters1.Add(new SqlParameter { ParameterName = "@HeldInRelationId", SqlDbType = System.Data.SqlDbType.Int, Value = HeldInRelationId });
                parameters1.Add(new SqlParameter { ParameterName = "@StudentId", SqlDbType = System.Data.SqlDbType.NVarChar, Value = StudentId });
                parameters1.Add(new SqlParameter { ParameterName = "@UserId", SqlDbType = System.Data.SqlDbType.Int, Value = UserId });

                DataTable dt = DataTableManager.GetDataFromQuery("ResultProcessByHeldInOrStudentId", parameters1);

            }
            catch (Exception ex)
            {
            }
        }

    }
}