using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EMS.Module
{
    public class MisscellaneousCommonMethods
    {
        static DAL.PABNA_UCAMEntities ucamContext = new DAL.PABNA_UCAMEntities();

        public static bool TimeCheck(DateTime StartDate, DateTime StartTime, DateTime EndDate, DateTime EndTime)
        {
            DateTime actualStartDate = Convert.ToDateTime(StartDate.Date + StartTime.TimeOfDay);
            DateTime actualEndDate = Convert.ToDateTime(EndDate.Date + EndTime.TimeOfDay);
            if (actualStartDate <= DateTime.Now && DateTime.Now <= actualEndDate)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string GetHeldInName(int HeldInRelationId)
        {
            string Name = "";
            try
            {
                var HeldInRelationObj = ucamContext.ExamHeldInAndProgramRelations.Find(HeldInRelationId);

                if (HeldInRelationObj != null)
                {
                    int Year = 0, Semester = 0, Program = 0;
                    string YearName = "", SemesterName = "", ProgramName = "";


                    Year = HeldInRelationObj.YearNo == null ? 0 : Convert.ToInt32(HeldInRelationObj.YearNo);
                    Semester = HeldInRelationObj.SemesterNo == null ? 0 : Convert.ToInt32(HeldInRelationObj.SemesterNo);
                    Program = HeldInRelationObj.ProgramId == null ? 0 : Convert.ToInt32(HeldInRelationObj.ProgramId);



                    var yearObj = YearManager.GetById(Year);
                    if (yearObj != null)
                        YearName = yearObj.YearName;

                    var semesterObj = SemesterManager.GetById(Semester);
                    if (semesterObj != null)
                        SemesterName = semesterObj.SemesterName;

                    var Prg = ProgramManager.GetById(Program);
                    if (Prg != null)
                        ProgramName = Prg.ShortName;

                    if(ProgramName!=null)
                    {
                        string[] prgName = ProgramName.Split('_');
                        if (prgName.Length > 0)
                            ProgramName = prgName[0];
                    }

                    Name = YearName + " " + SemesterName + " " + ProgramName;

                }
            }
            catch (Exception ex)
            {
            }

            return Name;

        }



        public static void InsertLog(string LogInID, string EventName, string Message, string Roll, string Course, string pageId, string pageName, string pageUrl)
        {
            LogGeneralManager.Insert(
                                      DateTime.Now,
                                      "",
                                      "",
                                      LogInID,
                                      Course,
                                      "",
                                      EventName,
                                      Message,
                                      "Normal",
                                      pageId.ToString(),
                                      pageName.ToString(),
                                      pageUrl,
                                      Roll);
        }

    }
}