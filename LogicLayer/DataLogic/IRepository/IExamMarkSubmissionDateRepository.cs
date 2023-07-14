using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExamMarkSubmissionDateRepository
    {
        int Insert(ExamMarkSubmissionDate exammarksubmissiondate);
        bool Update(ExamMarkSubmissionDate exammarksubmissiondate);
        bool Delete(int ExamMarkSubmissionDateId);
        ExamMarkSubmissionDate GetById(int? ExamMarkSubmissionDateId);
        List<ExamMarkSubmissionDate> GetAll();
        ExamMarkSubmissionDate GetByAcaCalSecId(int acaCalSecId);
    }
}

