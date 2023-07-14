using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LogicLayer.BusinessObjects;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IFeeSetupRepository
    {
        int Insert(FeeSetup feesetup);
        bool Update(FeeSetup feesetup);
        bool Delete(int FeeSetupId);
        FeeSetup GetById(int? FeeSetupId);
        List<FeeSetup> GetAll();
        List<FeeSetup> GetAllByProgramIdAcaCalIdScholarshipStatusAndGovNonGov(int programId, int? acaCalId, int scholarshipStatusId, int govNonGovId);
        List<FeeGroupMaster> GetAllFeeGroupByProgramIdAcaCalId(int? programId, int? acaCalId);
    }
}

