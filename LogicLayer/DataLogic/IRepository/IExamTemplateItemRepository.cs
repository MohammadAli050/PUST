using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamTemplateItemRepository
    {
        int Insert(ExamTemplateItem examtemplateitem);
        bool Update(ExamTemplateItem examtemplateitem);
        bool Delete(int ExamTemplateItemId);
        ExamTemplateItem GetById(int? ExamTemplateItemId);
        List<ExamTemplateItem> GetAll();

        ExamTemplateItem GetByExamNameColumnSequence(int examTemplateId, decimal examMark, int columnSequence);
        List<ExamTemplateItem> GetByExamTemplateId(int examTemplateId);
        List<ExamTemplateItem> GetBasicWithOutFinalTemplateItemByExamTemplateId(int examTemplateId);
    }
}

