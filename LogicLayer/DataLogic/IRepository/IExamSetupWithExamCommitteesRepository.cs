using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamSetupWithExamCommitteesRepository
    {
        int Insert(ExamSetupWithExamCommittees examsetupwithexamcommittees);
        bool Update(ExamSetupWithExamCommittees examsetupwithexamcommittees);
        bool Delete(int ID);
        ExamSetupWithExamCommittees GetById(int? ID);
        List<ExamSetupWithExamCommittees> GetAll();

        ExamSetupWithExamCommittees GetByExamSetupId(int examSetupId);
    }
}

