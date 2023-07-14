using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamMarkRepository
    {
        int Insert(ExamMark exammark);
        bool Update(ExamMark exammark);
        bool Delete(int Id);
        ExamMark GetById(int? Id);
        List<ExamMark> GetAll();

        List<ExamMarkDTO> GetByExamMarkDtoByParameter(int programId, int yearNo, int semesterNo, int courseId, int versionId, int acaCalSectionId, int examTemplateItemId);

        ExamMark GetStudentMarkByExamId(int courseHistoryId, int examId);
        List<TempStudentExamMarkColumnWise> GetAllMarkColumnWise(int acaCalSectionId);

        int InsertStudentCourseQuestionMarksMaster(StudentsCourseMarks aStudentCourseMarks);
        bool InsertStudentCourseQuestionMarksDetail(Marks aStudentCourseMarks);
        List<Marks> GetAllQuestionMarksByCourseHistoryIdAndExamTypeItemId(int studentCourseHistoryId, int examTypeItemId);
        bool DeleteStudentCourseQuestionMarksMasterDetail(StudentsCourseMarks aStudentCourseMarks);

        ExamMark GetByCourseHistoryId(int courseHistoryId);

    }
}

