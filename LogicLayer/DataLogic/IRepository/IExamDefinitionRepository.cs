using LogicLayer.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamDefinitionRepository
    {
        int Insert(ExamDefinition examdefinition);
        bool Update(ExamDefinition examdefinition);
        bool Delete(int ExamDefinitionId);
        ExamDefinition GetById(int? ExamDefinitionId);
        List<ExamDefinition> GetAll();
        ExamDefinition GetByAcaCalIdExamTypeId(int? acaCalId, int? ExamTypeId);
        ExamDefinition GetByAcaCalId(int acaCalId);
    }
}
