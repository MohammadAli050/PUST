using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessLogic
{
    public class ExamMarkQuestionManager
    {
        public static int Insert(ExamMarkQuestion exammarkquestion)
        {
            int id = RepositoryManager.ExamMarkQuestion_Repository.Insert(exammarkquestion);
            return id;
        }

        public static bool Update(ExamMarkQuestion exammarkquestion)
        {
            bool isExecute = RepositoryManager.ExamMarkQuestion_Repository.Update(exammarkquestion);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamMarkQuestion_Repository.Delete(id);
            return isExecute;
        }

        public static ExamMarkQuestion GetById(int? id)
        {
            ExamMarkQuestion exammarkquestion = RepositoryManager.ExamMarkQuestion_Repository.GetById(id);

            return exammarkquestion;
        }

        public static List<ExamMarkQuestion> GetAll()
        {
            List<ExamMarkQuestion> list = RepositoryManager.ExamMarkQuestion_Repository.GetAll();
            return list;
        }

        public static ExamMarkQuestion GetByStudentIdCourseHistoryId(int studentId, int courseHistoryId, int questionNo)
        {
            ExamMarkQuestion exammarkquestion = RepositoryManager.ExamMarkQuestion_Repository.GetByStudentIdCourseHistoryId(studentId, courseHistoryId, questionNo);

            return exammarkquestion;
        }



    }
}
