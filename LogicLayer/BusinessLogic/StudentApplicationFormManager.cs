using LogicLayer.BusinessObjects.RO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class StudentApplicationFormManager
    {
        public static List<StudentAppliFormOfficialAndPersonalRO> GetStudentApplicationDetailsByOfficialId(int officialId)
        {
            return RepositoryManager.StudentApplicationForm_Repository.GetStudentApplicationDetailsByOfficialId(officialId);
        }

        public static List<StudentAppliFormEducationInfoRO> GetStudentApplicationEducationDetailsByOfficialId(int officialInfoId)
        {
            return RepositoryManager.StudentApplicationForm_Repository.GetStudentApplicationEducationDetailsByOfficialId(officialInfoId);
        }

        public static List<StudentAppliFromPreviousSemesterInfoRO> GetStudentApplicationPreviousSemesterDetailsByOfficialId(int officialInfoId)
        {
            return RepositoryManager.StudentApplicationForm_Repository.GetStudentApplicationPreviousSemesterDetailsByOfficialId(officialInfoId);
        }

        public static List<StudentAppliFormAppliedCourseRO> GetStudentApplicationAppliedCourseDetailsByOfficialId(int officialInfoId)
        {
            return RepositoryManager.StudentApplicationForm_Repository.GetStudentApplicationAppliedCourseDetailsByOfficialId(officialInfoId);
        }
    }
}
