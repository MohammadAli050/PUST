using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    [Serializable]
    public class TimeSlotPlanNew
    {
        public int TimeSlotPlanID { get; set; }
        public int StartHour { get; set; }
        public int StartMin { get; set; }
        public int StartAMPM { get; set; }
        public int EndHour { get; set; }
        public int EndMin { get; set; }
        public int EndAMPM { get; set; }
        public int Type { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }

        #region Custom Property

        public string StartHourText
        {
            get
            {
                return StartHour.ToString().PadLeft(2, '0').ToString();
            }
        }

        public string StartMinText
        {
            get
            {
                return StartMin.ToString().PadLeft(2, '0').ToString();
            }
        }

        public string EndHourText
        {
            get
            {
                return EndHour.ToString().PadLeft(2, '0').ToString();
            }
        }

        public string EndMinText
        {
            get
            {
                return EndMin.ToString().PadLeft(2, '0').ToString();
            }
        }

        #endregion
    }
}
