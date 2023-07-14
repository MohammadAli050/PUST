using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class FeeGroupMaster
    {
        public int FeeGroupMasterId { get; set; }
        public int FeeGroupDetailId { get; set; }
        public string FeeGroupName { get; set; }
        public int? ProgramId { get; set; }
        public int? StudentAdmissionAcaCalId { get; set; }
        public int FundTypeId { get; set; }
        public string Remarks { get; set; }
        public bool IsActive { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        //do not map properties
        public int TypeDefinitionId { get; set; }
        public decimal Amount { get; set; }

        public string ProgramName { get; set; }
        public string ProgramDetailName { get; set; }
        public string BatchNO { get; set; }
        public string FundName { get; set; }
        public string AccountNo { get; set; }

        public AcademicCalender Session
        {
            get
            {
                if (StudentAdmissionAcaCalId != null)
                    return AcademicCalenderManager.GetById(StudentAdmissionAcaCalId);
                return null;
            }
        }

        public Program Program
        {
            get
            {
                if (ProgramId != null)
                    return ProgramManager.GetById(ProgramId);
                return null;
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

