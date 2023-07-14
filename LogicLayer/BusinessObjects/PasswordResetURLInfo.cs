using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class PasswordResetURLInfo
    {
        public int Id { get; set; }
        public string LoginId { get; set; }
        public string GeneratedId { get; set; }
        public string OfficialEmail { get; set; }
        public DateTime Validity { get; set; }
        public bool IsPasswordReset { get; set; }
        public bool IsEmailSend { get; set; }
    }
}
