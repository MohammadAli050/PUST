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
    public class ExamMarkSubmissionDateManager
    {


        public static int Insert(ExamMarkSubmissionDate exammarksubmissiondate)
        {
            int id = RepositoryManager.ExamMarkSubmissionDate_Repository.Insert(exammarksubmissiondate);
            return id;
        }

        public static bool Update(ExamMarkSubmissionDate exammarksubmissiondate)
        {
            bool isExecute = RepositoryManager.ExamMarkSubmissionDate_Repository.Update(exammarksubmissiondate);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamMarkSubmissionDate_Repository.Delete(id);
            return isExecute;
        }

        public static ExamMarkSubmissionDate GetById(int? id)
        {
            var exammarksubmissiondate = RepositoryManager.ExamMarkSubmissionDate_Repository.GetById(id);
            return exammarksubmissiondate;
        }

        public static ExamMarkSubmissionDate GetByAcaCalSecId(int acaCalSecId)
        {
            var exammarksubmissiondate = RepositoryManager.ExamMarkSubmissionDate_Repository.GetByAcaCalSecId(acaCalSecId);
            return exammarksubmissiondate;
        }

        public static List<ExamMarkSubmissionDate> GetAll()
        {
            var list = RepositoryManager.ExamMarkSubmissionDate_Repository.GetAll();
            return list;
        }
    }
}

