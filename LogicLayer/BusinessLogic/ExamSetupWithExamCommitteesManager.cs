using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;

namespace LogicLayer.BusinessLogic
{
    public class ExamSetupWithExamCommitteesManager
    {
        public static int Insert(ExamSetupWithExamCommittees examsetupwithexamcommittees)
        {
            int id = RepositoryManager.ExamSetupWithExamCommittees_Repository.Insert(examsetupwithexamcommittees);
            return id;
        }

        public static bool Update(ExamSetupWithExamCommittees examsetupwithexamcommittees)
        {
            bool isExecute = RepositoryManager.ExamSetupWithExamCommittees_Repository.Update(examsetupwithexamcommittees);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamSetupWithExamCommittees_Repository.Delete(id);
            return isExecute;
        }

        public static ExamSetupWithExamCommittees GetById(int? id)
        {
            ExamSetupWithExamCommittees examsetupwithexamcommittees = RepositoryManager.ExamSetupWithExamCommittees_Repository.GetById(id);
            return examsetupwithexamcommittees;
        }

        public static List<ExamSetupWithExamCommittees> GetAll()
        {
            List<ExamSetupWithExamCommittees> list = RepositoryManager.ExamSetupWithExamCommittees_Repository.GetAll();
            return list;
        }



        public static ExamSetupWithExamCommittees GetByExamSetupId(int examSetupId)
        {
            ExamSetupWithExamCommittees examsetupwithexamcommittees = RepositoryManager.ExamSetupWithExamCommittees_Repository.GetByExamSetupId(examSetupId);
            return examsetupwithexamcommittees;
        }
    }
}

