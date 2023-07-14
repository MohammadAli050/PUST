using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class Address
    {
        public int AddressId { get; set; }
        public int PersonId { get; set; }
        public int AddressTypeId { get; set; }
        public string AddressLine { get; set; }
        public string AppartmentNo { get; set; }
        public string HouseNo { get; set; }
        public string RoadNo { get; set; }
        public string AreaInfo { get; set; }
        public string PostOffice { get; set; }
        public string PoliceStation { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string Division { get; set; }
        public string PostCode { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
    }
}
