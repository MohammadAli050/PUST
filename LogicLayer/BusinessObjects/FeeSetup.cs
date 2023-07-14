using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class FeeSetup
    {
        public int FeeSetupId { get; set; }
        public int AcademicCalendarId { get; set; } // StudentAdmissionAcaCalId, session student admitted
        public int ProgramId { get; set; }
        public int? BatchId { get; set; }
        public int TypeDefinitionId { get; set; }
        public int FundTypeId { get; set; }
        public decimal Amount { get; set; }
        public int ScholarshipStatusType { get; set; }
        public int GovNonGovType { get; set; }
        public int CalenderUnitTypeId { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }


        public AcademicCalender Session
        {
            get
            {
                return AcademicCalenderManager.GetById(AcademicCalendarId);
            }
        }

        public Program Program
        {
            get
            {
                return ProgramManager.GetById(ProgramId);
            }
        }

        public TypeDefinition Type
        {
            get
            {
                return TypeDefinitionManager.GetById(TypeDefinitionId);
            }
        }

    }
}

