using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class BillHistory
    {

        public int BillHistoryId { get; set; }
        public int BillHistoryMasterId { get; set; }
        public int StudentCourseHistoryId { get; set; }
        public int StudentId { get; set; }
        public int TypeDefinitionId { get; set; }
        public int AcaCalId { get; set; }         // AcademicCalendarId
        public decimal Fees { get; set; }
        public string Remark { get; set; }
        public DateTime BillingDate { get; set; }
        public bool IsDeleted { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public string Attribute4 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        //public int BillHistoryId {get; set; }
        //public int StudentId {get; set; }
        public int FeeSetupId { get; set; }
        //public int FundTypeId { get; set; }
        //public int AcaCalId {get; set; }
        //public decimal Fees { get; set; }
        //public string Remark {get; set; }
        //public DateTime BillingDate{get; set; }
        //public bool IsDeleted{get; set; }
        //public int BillHistoryMasterId {get; set; }
        //public string Attribute1 {get; set; }
        //public string Attribute2 {get; set; }
        //public string Attribute3 {get; set; }
        //public string Attribute4 {get; set; }
        //public int CreatedBy {get; set; }
        //public DateTime CreatedDate { get; set; }
        //public int ModifiedBy {get; set; }
        //public DateTime ModifiedDate { get; set; }

        public string TypeName
        {
            get
            {
                TypeDefinition td = TypeDefinitionManager.GetById(TypeDefinitionId);
                return td.Definition;
            }
        }

        public FeeSetup FeeSetup
        {
            get
            {
                return FeeSetupManager.GetById(FeeSetupId);
            }
        }

        public int FundTypeId { get; set; }
        public string FundName { get; set; }
        public string AccountNo { get; set; }
    }
}

