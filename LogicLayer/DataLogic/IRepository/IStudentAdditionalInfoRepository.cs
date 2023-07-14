using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IStudentAdditionalInfoRepository
    {
        int Insert(StudentAdditionalInfo studentadditionalinfo);
        bool Update(StudentAdditionalInfo studentadditionalinfo);
        bool Delete(int InfoId);
        StudentAdditionalInfo GetById(int? InfoId);
        List<StudentAdditionalInfo> GetAll();
        StudentAdditionalInfo GetByStudentId(int studentId);
    }
}

