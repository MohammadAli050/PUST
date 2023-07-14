using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using LogicLayer.BusinessObjects;
using LogicLayer.DataLogic.DAFactory;
using LogicLayer.BusinessObjects.DTO;
namespace LogicLayer.BusinessLogic
{
    public class ExaminorSetupsManager
    {
        public static int Insert(ExaminorSetups examinorsetup)
        {
            int id = RepositoryManager.ExaminorSetup_Repository.Insert(examinorsetup);
            return id;
        }

        public static bool Update(ExaminorSetups examinorsetup)
        {
            bool isExecute = RepositoryManager.ExaminorSetup_Repository.Update(examinorsetup);
            return isExecute;
        }

        public static bool Delete(int id)
        {
            bool isExecute = RepositoryManager.ExamSetup_Repository.Delete(id);
            return isExecute;
        }

        public static ExaminorSetups GetById(int id)
        {
            ExaminorSetups examSetup = RepositoryManager.ExaminorSetup_Repository.GetById(id);
            return examSetup;
        }

        public static ExaminorSetups GetByAcaCalSecId(int acaCalId)
        {
            ExaminorSetups examSetup = RepositoryManager.ExaminorSetup_Repository.GetByAcaCalSecId(acaCalId);
            return examSetup;
        }

        public static List<ExaminorSetups> GetAll()
        {
            List<ExaminorSetups> list = RepositoryManager.ExaminorSetup_Repository.GetAll();
            return list;
        }
        public static List<ExaminorSetupsDTO> ExaminerSetupGetAllByAcaCalProgram(int ProgramId, int yearno, int semesterno, int examid)
        {
            List<ExaminorSetupsDTO> list = RepositoryManager.ExaminorSetup_Repository.ExaminerSetupGetAllByAcaCalProgram(ProgramId, yearno, semesterno, examid);
            return list;
        }

       
    }
}
