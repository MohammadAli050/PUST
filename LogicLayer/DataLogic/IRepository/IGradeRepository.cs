using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IGradeRepository
    {
        int Insert(Grade grade);
        bool Update(Grade grade);
        bool Delete(int GradeId);
        Grade GetById(int? GradeId);
        List<Grade> GetAll();
        List<Grade> GetByGradeMasterId(int gradeMasterId);
    }
}

