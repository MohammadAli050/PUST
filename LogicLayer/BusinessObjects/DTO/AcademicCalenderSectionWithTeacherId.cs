using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicLayer.BusinessObjects.DTO
{
    [Serializable]
    public class AcademicCalenderSectionWithTeacherId
    {
        public int AcaCal_SectionID { get; set; }
        public int AcademicCalenderID { get; set; }
        public int CourseID { get; set; }
        public int VersionID { get; set; }
        public string SectionName { get; set; }
        public int Capacity { get; set; }
        public string Session { get; set; }
        public int YearNo { get; set; }
        public int SemesterNo { get; set; }
        public int ExamId { get; set; }
        public int YearId { get; set; }
        public int SemesterId { get; set; }
        public int RoomInfoOneID { get; set; }
        public int RoomInfoTwoID { get; set; }
        public int RoomInfoThreeID { get; set; }
        public int DayOne { get; set; }
        public int DayTwo { get; set; }
        public int DayThree { get; set; }
        public int TimeSlotPlanOneID { get; set; }
        public int TimeSlotPlanTwoID { get; set; }
        public int TimeSlotPlanThreeID { get; set; }
        public int TeacherOneID { get; set; }
        public int TeacherTwoID { get; set; }
        public int TeacherThreeID { get; set; }
        public int DeptID { get; set; }
        public int ProgramID { get; set; }
        public int TypeDefinitionID { get; set; }
        public int Occupied { get; set; }
        public int ShareSection { get; set; }
        public int BasicExamTemplateId { get; set; }
        public int CalculativeExamTemplateId { get; set; }

        public string SectionDefination { get; set; }
        public int OnlineGradeSheetTemplateID { get; set; }
        public int SectionGenderID { get; set; }
        public string Remarks { get; set; }

        public string RoomConflict { get; set; }
        public string FacultyConflict { get; set; }

        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int CampusId { get; set; }

        public int TeacherID { get; set; }
    }
}
