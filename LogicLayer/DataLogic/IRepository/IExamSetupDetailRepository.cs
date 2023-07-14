using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamSetupDetailRepository
    {
        int Insert(ExamSetupDetail examsetupdetail);
        bool Update(ExamSetupDetail examsetupdetail);
        bool Delete(int ExamSetupDetailId);
        ExamSetupDetail GetById(int? ExamSetupDetailId);
        List<ExamSetupDetail> GetAll();


        ExamSetupDetail GetByExamSetupIdSemesterNo(int examSetupId, int semesterNo);
        List<ExamSetupDetail> GetAllByExamSetupId(int examSetupId);
    }
}
