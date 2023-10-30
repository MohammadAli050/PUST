using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IStudentAttendancePercentageStatusRepository
    {
        int Insert(StudentAttendancePercentageStatus studentattendancepercentagestatus);
        bool Update(StudentAttendancePercentageStatus studentattendancepercentagestatus);
        bool Delete(int StudentPercentageId);
        StudentAttendancePercentageStatus GetById(int? StudentPercentageId);
        List<StudentAttendancePercentageStatus> GetAll();
    }
}

