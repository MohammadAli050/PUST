using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamTemplateBasicItemDetailsRepository
    {
        int Insert(ExamTemplateBasicItemDetails examtemplatebasicitemdetails);
        bool Update(ExamTemplateBasicItemDetails examtemplatebasicitemdetails);
        bool Delete(int ExamTemplateBasicItemId);
        ExamTemplateBasicItemDetails GetById(int? ExamTemplateBasicItemId);
        List<ExamTemplateBasicItemDetails> GetAll();
        List<ExamTemplateBasicItemDetails> GetByExamTemplateMasterId(int examTemplateMasterId);
        List<ExamTemplateBasicItemDetails> GetInCourseExamByTemplateMasterId(int examTemplateMasterId);
        List<ExamTemplateBasicItemDetails> GetByAcaCalSecIdEmployeeId(int AcaCalSecId, int EmployeeId, int AcaCalSectionFacultyTypeId);
        List<ExamTemplateBasicItemDetails> GetFinalExamByTemplateMasterId(int examTemplateMasterId);         
    }
}

