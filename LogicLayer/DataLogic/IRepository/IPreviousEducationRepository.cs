using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IPreviousEducationRepository
    {
        int Insert(PreviousEducation previouseducation);
        bool Update(PreviousEducation previouseducation);
        bool Delete(int PreviousEducationId);
        PreviousEducation GetById(int? PreviousEducationId);
        List<PreviousEducation> GetAll();
        List<PreviousEducation> GetAllByPersonId(int PersonId);

    }
}

