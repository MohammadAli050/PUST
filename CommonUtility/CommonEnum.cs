using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtility
{
    public static class CommonEnum
    {
        public enum ValueSet
        {
            Day = 1,
            ExamType = 2,
            PersonType = 3,
            SectionGender = 4
        }

        public enum ControlId
        {
            // Syntex
            //PageName_ControlName = value
            PreAdvising_lnkBtnOpenCourse = 1,
            Registration_lnkBtnOpenCourse = 2,
        }

        public enum PersonType
        {
            Student = 11,
            Faculty = 12,
            Employee = 13
        }

        public enum CourseStatus
        {
            Running = 6,
            Regular = 9,
            Retake = 11,
            Improvement = 12
        }

        public enum CourseRegistrationStatus
        {
            Regular = 9,
            Backlog = 11,
            Improvement = 12,
            SpecialCourse = 13
        }

        public enum SectionGender
        {
            Male = 14,
            Female = 15,
            Mixed = 16
        }

        public enum DiscountType
        {

        }

        public enum PaymentType
        {
            Cash = 1,
            Bank = 2
        }

        public enum CertificateType
        {
            Provisional = 1,
            Main = 2
        }

        public enum PageName
        {
            BillCollection = 100101,
            FeeSetup = 100102,
            LateFineEntry = 100103,
            BillManualEntry = 100104,
            StudentGeneralBill = 100105,
            StudentDiscountInitial = 100106,
            StudentDiscountCurrent = 100107,
            StudentGradeChange = 100108,
            StudentCourseDrop = 100109,
            Registration = 100110,
            ForceRegistration = 100111,
            Admin_CourseDropApprove = 100112,
            StudentDiscountInitialPage = 100113,
            StudentDiscountCurrentPage = 100114,
            StudentBlock = 100115,
            Admin_ClassRoutine = 100116,
            SMSSetup = 10117,
            ExamStudentPresent = 100118,
            ExamStatusEntry = 100119,
            LateRegistrationFineEntry = 100120,
            CurriculumDistributionNew = 100158,
            EquivalentCourseUI = 100123,
            PreRequisiteUI = 100124,
            CourseExplorer = 100132,
            ClassRoutine = 100133,
            TreeMaster = 100134,
            StudentCourseHistoryEdit = 100135,
            SectionChangeAfterReg = 100136,
            UnRegistration = 100137,
            DayScheduleMaster = 100138,
            CurriculumBuilder = 100139,
            ExamMarkSubmit = 100140,
            ResultPublishCourseWise = 100141,
            ResultPublishStudentWise = 100142,
            ClassAttendanceEntry = 100143,
            ClassAttendanceDelete = 100144,
            PasswordChangeByAdmin = 100145,
            ExamResultUnSubmit = 100146,
            ResultProcess = 100147,
            CourseOfferAndCount = 100148,
            StudentActive = 100149,
            ResultUnSubmitBySection = 100150,
            PasswordChangeByUser = 100151,
            StudentRegistrationNoEntry = 100152,
            TypeDefinition = 100153,
            BillGeneration = 100154,
            BillPosting = 100155,
            StudentBillHistory = 100156,
            BillDelete = 100157,
            BillEdit = 100158,
            StudentResultHistory = 100159,
            UserControlForUVuser = 100160,
            StudentInformationMigration = 100161,
            StudentYearSemesterPromotion = 100162,
            StudentCourseSectionEnrollment = 100163,
            ExternalCommitteeMemberInformationSetup = 100164,
            ExamHeldInInformationSetup = 100165,
            CourseTeacherAndTemplateAssign = 100166,
            ExamMarkEntryNewVersion = 100167,
            QuestionSetterAndScriptExaminerSetup = 100168,
            StudentFormFillUpApply = 100169,
            FormFillUpApplicationManage = 100170,
            FormFillUpApplicationManageByHallProvost = 100171,
            FormFillUpApplicationManageByAcademicSection = 100172,
            RptStudentAdmitCard = 100173,
            StudentReAdmission = 100174,
            MarkEntryRelatedDeadlineSetup=100175,
            ExamMarkDateRangeSetup=100176
        }

        public enum AddressType
        {
            PresentAddress = 1,
            PermanentAddress = 2,
            GuardianAddress = 3,
            MailingAddress = 4
        }

        public enum Degree
        {
            Under_Graduate = 1,
            Graduate = 2,
            Other = 3
        }

        public enum Gender
        {
            Male = 19,
            Female = 21,
        }

        public enum MaritalStatus
        {
            single = 1,
            married = 2,
            divorced = 3,
            widowed = 4,
            Other = 5
        }

        public enum TeacherType
        {
            FullTime = 1,
            PartTime = 2,
            HalfTime = 3
        }

        public enum EducationBoard
        {
            Dhaka = 1,
            Rajshahi = 2,
            Comilla = 3,
            Jessore = 4,
            Chittagong = 5,
            Barisal = 6,
            Sylhet = 7,
            Dinajpur = 8,
            Madrasah = 9,
            Other = 10,
            Edexel = 11,
            Cambridge = 12
        }

        public enum EducationCategory
        {
            LowerPSC = 66,
            PSC = 67,
            JSC = 68,
            SSC = 69,
            HSC = 70,
            Diploma = 71,
            Bachelor = 72,
            Masters = 73,
            Doctoral = 74,
            Other = 75
        }

        public enum Religion
        {
            Islam = 1,
            Hinduism = 2,
            Christianity = 3,
            Buddhism = 4,
            Other = 5,
            Jainism = 6,
            Judaism = 7,
            Sikhism = 8
        }

        public enum ActivityType
        {
            FormFillUpStudentApplication = 1,
            FormFillUpDepartmentofChairman = 2,
            FormFillUpHallProvost = 3,
            FormFillUpAcademicSection = 4,
            TeacherEvaluationSummery = 26
        }

        public enum ExamTemplateType
        {
            Basic = 1,
            Calculative = 2
        }

        public enum ExamCalculationType
        {
            Average = 1,
            BestOne = 2,
            BestTwo = 3,
            BestThree = 4,
            BestFour = 4,
            Sum = 5
        }

        public enum Role
        {
            ESCL = 1,
            BRURAdmin = 2,
            Student = 4,
            Advisor = 3
        }

        public enum CourseType
        {
            R = 9,
            RT = 10,
            IM = 12
        }

        //public enum ExamTemplateType
        //{
        //    Basic = 1,
        //    Calculative = 2
        //}

        //public enum ExamCalculationType
        //{
        //    Average = 1,
        //    BestOne = 2,
        //    BestTwo = 3,
        //    BestThree = 4,
        //    Sum = 5
        //}
        public enum ExamTemplateItemColumnType
        {
            Basic = 1,
            Calculative = 2,
            Grade = 3
        }
        public enum ExamTemplateItemColumnCalculationType
        {
            Percentage = 1,
            Average = 2,
            Sum = 3,
            Best_One = 4,
            Best_Two = 5,
            Best_Three = 6,
        }
        public enum ThirdExaminarExamination
        {
            No_Examiner = 0,
            More_Than_0_Examiner = 1
        }
    }
}
