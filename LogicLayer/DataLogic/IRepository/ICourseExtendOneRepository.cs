using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface ICourseExtendOneRepository
    {
        int Insert(CourseExtendOne courseextendone);
        bool Update(CourseExtendOne courseextendone);
        bool Delete(int CourseExtendId);
        CourseExtendOne GetById(int? CourseExtendId);
        List<CourseExtendOne> GetAll();
        CourseExtendOne GetByCourseIdVersionId(int courseId, int versionId);
    }
}

