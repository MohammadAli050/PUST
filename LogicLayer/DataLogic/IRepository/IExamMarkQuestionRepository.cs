using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamMarkQuestionRepository
    {
        int Insert(ExamMarkQuestion exammarkquestion);
        bool Update(ExamMarkQuestion exammarkquestion);
        bool Delete(int ID);
        ExamMarkQuestion GetById(int? ID);
        List<ExamMarkQuestion> GetAll();

        ExamMarkQuestion GetByStudentIdCourseHistoryId(int studentId, int courseHistoryId, int questionNo);

    }
}
