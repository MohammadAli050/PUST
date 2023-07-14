using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class ExamTemplateItem
    {
        public int ExamTemplateItemId { get; set; }
        public int ExamTemplateId { get; set; }
        public string ExamName { get; set; }
        public decimal ExamMark { get; set; }
        public decimal PassMark { get; set; }
        public int ColumnSequence { get; set; }
        public int PrintColumnSequence { get; set; }
        public int ColumnType { get; set; }
        public int CalculationType { get; set; }
        public decimal DivideBy { get; set; }
        public decimal MultiplyBy { get; set; }
        public bool ShowInTabulation { get; set; }
        public string TabulationTitle { get; set; }
        public string Attribute1 { get; set; }
        public string Attribute2 { get; set; }
        public string Attribute3 { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool IsFinalExam { get; set; }
        public int MultipleExaminer { get; set; }

        public bool SingleQuestionAnswer { get; set; }
        public bool ShowAllContinuousInSubTotal { get; set; }
        public bool ShowAllInGrandTotal { get; set; }
        public int NumberOfExaminer { get; set; }


        public string ColumnTypeName
        {
            get { return Enum.GetName(typeof(CommonUtility.CommonEnum.ExamTemplateItemColumnType), Convert.ToInt32(ColumnType)); }
        }

        public ExamTemplate ExamTemplate { get { return ExamTemplateManager.GetById(ExamTemplateId); } }
    }
}

