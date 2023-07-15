using CommonUtility;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Module
{
    public class CommonMethodForChekingRoleExistsInApprovalMatrix
    {
        static PABNA_UCAMEntities ucamContext = new PABNA_UCAMEntities();

        public static bool IsRoleExists(int RoleId, string EventName)
        {
            bool IsExists = false;
            try
            {
                var ApprovalMatrixObj = ucamContext.ApprovalServicesAuthorizationMatrices.Where(x => x.EventName == EventName).FirstOrDefault();

                int EventId = 0;
                EventId = ApprovalMatrixObj == null ? 0 : ApprovalMatrixObj.Id;


                string[] SplittedValue = CommonMethodForStringSplit.SplitString(ApprovalMatrixObj.ApprovalMatrix, '-');

                if (SplittedValue.Length > 0)
                {
                    if (SplittedValue.Contains(RoleId.ToString()))
                        IsExists = true;
                }
            }
            catch (Exception ex)
            {
            }

            return IsExists;

        }

        public static string[] SplittedValue(string EventName)
        {
            string[] Splitted = null;
            try
            {
                var ApprovalMatrixObj = ucamContext.ApprovalServicesAuthorizationMatrices.Where(x => x.EventName == EventName).FirstOrDefault();

                if (ApprovalMatrixObj != null)
                {
                    Splitted = CommonMethodForStringSplit.SplitString(ApprovalMatrixObj.ApprovalMatrix, '-');
                }
            }
            catch (Exception ex)
            {
            }
            return Splitted;
        }
    }
}