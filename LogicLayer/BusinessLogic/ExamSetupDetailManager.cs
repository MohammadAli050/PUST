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
    public class ExamSetupDetailManager
    {

        public static int Insert(ExamSetupDetail examsetupdetail)
        {
            int id = RepositoryManager.ExamSetupDetail_Repository.Insert(examsetupdetail);
            return id;
        }

        public static bool Update(ExamSetupDetail examsetupdetail)
        {
            bool isExecute = RepositoryManager.ExamSetupDetail_Repository.Update(examsetupdetail);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamSetupDetail_Repository.Delete(id);
            return isExecute;
        }

        public static ExamSetupDetail GetById(int? id)
        {
            ExamSetupDetail examsetupdetail = RepositoryManager.ExamSetupDetail_Repository.GetById(id);

            return examsetupdetail;
        }

        public static List<ExamSetupDetail> GetAll()
        {
            List<ExamSetupDetail> list = RepositoryManager.ExamSetupDetail_Repository.GetAll();

            return list;
        }


        public static ExamSetupDetail GetByExamSetupIdSemesterNo(int examSetupId, int semesterNo)
        {
            ExamSetupDetail examsetupdetail = RepositoryManager.ExamSetupDetail_Repository.GetByExamSetupIdSemesterNo(examSetupId, semesterNo);

            return examsetupdetail;
        }


        public static List<ExamSetupDetail> GetAllByExamSetupId(int examSetupId)
        {
            List<ExamSetupDetail> list = RepositoryManager.ExamSetupDetail_Repository.GetAllByExamSetupId(examSetupId);

            return list;
        }

    }
}
