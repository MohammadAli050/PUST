using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using LogicLayer.BusinessObjects.RO;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamTemplateRepository
    {
        int Insert(ExamTemplate examtemplate);
        bool Update(ExamTemplate examtemplate);
        bool Delete(int ExamTemplateId);
        ExamTemplate GetById(int? ExamTemplateId);
        List<ExamTemplate> GetAll();
        ExamTemplate GetExamTemplateByName(string examTemplateName);
        ExamTemplate GetBySyllabusDetailIdAcaCalId(int syllabusDetailId, int acaCalId);
        void ProcessFirstSecondThirdExaminerMarkToExamMark(int acacalSectionId);
        List<rContinuousAssessmentMark> GetContinuousMarkBySectionID(int SectionId);
    }
}

