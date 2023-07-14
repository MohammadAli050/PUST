using LogicLayer.BusinessObjects;
using LogicLayer.BusinessObjects.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.DataLogic.IRepository
{
    public interface IExaminorSetupRepository
    {
        int Insert(ExaminorSetups examsetup);
        bool Update(ExaminorSetups examsetup);
        bool Delete(int ID);
        ExaminorSetups GetById(int? ID);
        List<ExaminorSetups> GetAll();
        List<ExaminorSetupsDTO> ExaminerSetupGetAllByAcaCalProgram(int ProgramId, int yearno, int semesterno, int examid);
        ExaminorSetups GetByAcaCalSecId(int acaCalId);
    }
}
