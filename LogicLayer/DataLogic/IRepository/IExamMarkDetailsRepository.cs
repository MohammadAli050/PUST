using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamMarkDetailsRepository
    {
        int Insert(ExamMarkDetails exammarkdetails);
        bool Update(ExamMarkDetails exammarkdetails);
        bool Delete(int ExamMarkDetailId);
        ExamMarkDetails GetById(int? ExamMarkDetailId);
        List<ExamMarkDetails> GetAll();
        List<ExamMarkDTO> GetByExamMarkDtoByParameter(int programId, int yearNo, int semesterNo, int courseId, int versionId, int acaCalSectionId, int examTemplateBasicItemId);
        ExamMarkDetails GetByCourseHistoryIdExamTemplateItemId(int studentCourseHistoryId, int examTemplateItemId);


        List<ExamMarkDetails> GetAllByCourseHistoryIdColumnTypeIdMultipleExaminerIdExamStatusId(int CourseHistoryId, int? ColumnTypeId, int? MultipleExaminerId, int? ExamStatusId);


    }
}

