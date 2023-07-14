using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class PersonAdditionalInfo
    {
        public int PersonAdditionalInfoId { get; set; }
        public int PersonId { get; set; }
        public int PersonCategoryEnumValueId { get; set; }
        public int PersonStatusEnumValueId { get; set; }
        public string SpouseName { get; set; }
        public bool IsMilitary { get; set; }
        public string NationalIdNumber { get; set; }
        public string BirthCertificateNumber { get; set; }
        public string PersonalNo { get; set; }
        public bool IsMigrate { get; set; }
        public int BankId { get; set; }
        public string BankAccountNo { get; set; }
    }
}
