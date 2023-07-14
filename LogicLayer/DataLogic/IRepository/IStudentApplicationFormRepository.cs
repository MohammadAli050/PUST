using LogicLayer.BusinessObjects.RO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IStudentApplicationFormRepository
    {
        List<StudentAppliFormOfficialAndPersonalRO> GetStudentApplicationDetailsByOfficialId(int officialInfoId);
        List<StudentAppliFormEducationInfoRO> GetStudentApplicationEducationDetailsByOfficialId(int officialInfoId);
        List<StudentAppliFromPreviousSemesterInfoRO> GetStudentApplicationPreviousSemesterDetailsByOfficialId(int officialInfoId);
        List<StudentAppliFormAppliedCourseRO> GetStudentApplicationAppliedCourseDetailsByOfficialId(int officialInfoId);

    }
}
