using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Module
{
    public class CommonMethodForFacultyDepartmentProgramBatch
    {
        static PABNA_UCAMEntities ucamContext = new PABNA_UCAMEntities();


        #region Faculty

        public static List<FacultyInformation> AllFacultyList()
        {
            List<FacultyInformation> newList = new List<FacultyInformation>();

            try
            {
                newList = ucamContext.FacultyInformations.AsNoTracking().ToList().OrderBy(x => x.FacultyName).ToList();
            }
            catch (Exception ex)
            {
            }
            return newList;

        }

        #endregion

        #region Department

        public static List<Department> AllDepartmentList()
        {
            List<Department> newList = new List<Department>();

            try
            {
                newList = ucamContext.Departments.AsNoTracking().ToList().OrderBy(x => x.Name).ToList();
            }
            catch (Exception ex)
            {
            }
            return newList;
        }
        public static List<Department> AllDepartmentListByFacultyId(int FacultyId)
        {
            List<Department> newList = new List<Department>();

            try
            {
                List<Program> prgList = ucamContext.Programs.AsNoTracking().Where(x => x.FacultyId == FacultyId).ToList();
                if (prgList != null && prgList.Any())
                {
                    foreach (var item in prgList)
                    {
                        try
                        {
                            var IsExists = newList.Where(x => x.DeptID == item.DeptID).FirstOrDefault();
                            if (IsExists == null)
                            {
                                Department NewObj = ucamContext.Departments.Find(item.DeptID);
                                if (NewObj != null)
                                    newList.Add(NewObj);
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
            return newList;
        }

        #endregion

        #region Program

        public static List<Program> AllProgramListByParameter(int FacultyId, int DeptId, int CalenderUnitMasterId, int ProgramTypeId)
        {
            List<Program> newList = new List<Program>();

            try
            {
                newList = ucamContext.Programs.AsNoTracking().ToList();

                if (FacultyId > 0 && newList != null && newList.Any())
                    newList = newList.Where(x => x.FacultyId == FacultyId).ToList();

                if (DeptId > 0 && newList != null && newList.Any())
                    newList = newList.Where(x => x.DeptID == DeptId).ToList();

                if (CalenderUnitMasterId > 0 && newList != null && newList.Any())
                    newList = newList.Where(x => x.CalenderUnitMasterID == CalenderUnitMasterId).ToList();

                if (ProgramTypeId > 0 && newList != null && newList.Any())
                    newList = newList.Where(x => x.ProgramTypeID == ProgramTypeId).ToList();

            }
            catch (Exception ex)
            {
            }
            return newList;
        }

        #endregion

        #region Batch

        public static List<Batch> AllBatchListByProgramId(int ProgramId)
        {
            List<Batch> newList = new List<Batch>();

            try
            {
                newList = ucamContext.Batches.AsNoTracking().ToList();

                if (ProgramId > 0 && newList != null && newList.Any())
                    newList = newList.Where(x => x.ProgramId == ProgramId).ToList();


            }
            catch (Exception ex)
            {
            }
            return newList;
        }

        #endregion

        #region Session

        public static List<AcademicCalender> AllSession()
        {
            List<AcademicCalender> newList = new List<AcademicCalender>();

            try
            {
                newList = ucamContext.AcademicCalenders.AsNoTracking().ToList();

                if (newList != null && newList.Any())
                {
                    var CalenderUnitTypeList = ucamContext.CalenderUnitTypes.ToList();

                    foreach (var item in newList)
                    {
                        var CalenderObj = CalenderUnitTypeList.Where(c => c.CalenderUnitTypeID == item.CalenderUnitTypeID).FirstOrDefault();
                        if (CalenderObj != null)
                        {
                            item.Attribute1 = CalenderObj.TypeName + " " + item.Year;
                        }
                        else
                            item.Attribute1 = "";
                    }
                }

            }
            catch (Exception ex)
            {
            }
            return newList;
        }

        #endregion
    }
}