using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamDefinition
    {
        public int ExamDefinitionId { get; set; }
        public string ExamDescription { get; set; }
        public string ExamShortName { get; set; }
        public int AcaCalId { get; set; }
        public string ProgramType { get; set; }
        public int ExamTypeId { get; set; }

        public DateTime LastDateOfIncourseMarkSubmission { get; set; }
        public DateTime StartDateOfTermFinal { get; set; }
        public DateTime EndDateOfTermFinal { get; set; }
        public DateTime LastDateOfTermFinalMarkSubmission { get; set; }
        public DateTime LastDateOfScrutinization { get; set; }
        public DateTime LastDateOfTabulation { get; set; }
        public DateTime ResultPublicationDate { get; set; }

        public DateTime StartDateOfTermFinalBMA { get; set; }
        public DateTime EndDateOfTermFinalBMA { get; set; }
        public string ExamShortNameBMA { get; set; }
        

        public string Remarks { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
