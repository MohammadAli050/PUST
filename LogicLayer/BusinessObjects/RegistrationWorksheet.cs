using LogicLayer.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects
{
    public class RegistrationWorksheet
    {
        public int ID {get; set; }
		public int StudentID {get; set; }
		public int CalCourseProgNodeID {get; set; }
		public bool IsCompleted{get; set; }
		public int OriginalCalID {get; set; }
        public string Session { get; set; }
        public int YearId { get; set; }
        public int SemesterId { get; set; }
        public int YearNo { get; set; }
        public int SemesterNo { get; set; }
        public int ExamId { get; set; }
		public bool IsAutoAssign{get; set; }
		public bool IsAutoOpen{get; set; }
		public bool IsRequisitioned{get; set; }
		public bool IsMandatory{get; set; }
		public bool IsManualOpen{get; set; }
		public int TreeCalendarDetailID {get; set; }
		public int TreeCalendarMasterID {get; set; }
		public int TreeMasterID {get; set; }
		public string CalendarMasterName {get; set; }
		public string CalendarDetailName {get; set; }
		public int CourseID {get; set; }
		public int VersionID {get; set; }
        public decimal Credits { get; set; }
		public int Node_CourseID {get; set; }
		public int NodeID {get; set; }
		public bool IsMinorRelated{get; set; }
		public bool IsMajorRelated{get; set; }
		public string NodeLinkName {get; set; }
		public string FormalCode {get; set; }
		public string VersionCode {get; set; }
		public string CourseTitle {get; set; }
		public int AcaCal_SectionID {get; set; }
		public string SectionName {get; set; }
		public int ProgramID {get; set; }
		public int DeptID {get; set; }
		public int Priority {get; set; }
		public int RetakeNo {get; set; }
		public decimal ObtainedGPA{get; set; }
		public string ObtainedGrade {get; set; }
		public int AcaCalYear {get; set; }
		public string BatchCode {get; set; }
		public string AcaCalTypeName {get; set; }
        public decimal CalCrsProgNdCredits { get; set; }
		public bool CalCrsProgNdIsMajorRelated{get; set; }
		public bool IsMultipleACUSpan{get; set; }
		public decimal CompletedCredit{get; set; }
		public decimal CourseCredit{get; set; }
		public int CreatedBy {get; set; }
		public DateTime CreatedDate{get; set; }
		public int ModifiedBy {get; set; }
        public DateTime ModifiedDate { get; set; }
		public int CourseStatusId {get; set; }
		public bool IsRegistered{get; set; }
		public bool IsAdd{get; set; }
		public string ConflictedCourse {get; set; }
		public int SequenceNo {get; set; }
		public bool IsOfferedCourse{get; set; }
		public int CourseResultAcaCalID {get; set; }
		public int PostMajorNodeLevel {get; set; }
		public bool IsCreditCourse{get; set; }
		public int BatchID {get; set; }
        public Nullable<int> HeldInRelationId { get; set; }


        public string ProgramAttribute1
        {
            get
            {
                AcademicCalenderSection acs = AcademicCalenderSectionManager.GetById(AcaCal_SectionID);
                if (acs != null)
                    return acs.Program.Attribute1;
                else
                    return "";
            }
        }

        public string NodeGroup
        {
            get
            {
                return string.IsNullOrEmpty(NodeLinkName) ? "--" : NodeLinkName;
            }
        }
    }
}

