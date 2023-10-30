using LogicLayer.DataLogic.IRepository;
using LogicLayer.DataLogic.SQLRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using LogicLayer.DataLogic.IRepository;
//using LogicLayer.DataLogic.SQLRepository;
using LogicLayer.DataLogic;

namespace LogicLayer.DataLogic.DAFactory
{
    public class RepositoryManager
    {
        public static IAcademicCalenderRepository AcademicCalender_Repository
        {
            get
            {
                IAcademicCalenderRepository repository = new SQLAcademicCalenderRepository();
                return repository;
            }
        }

        public static IMenuRepository Menu_Repository
        {
            get
            {
                IMenuRepository repository = new SQLMenuRepository();
                return repository;
            }
        }

        public static IPersonRepository Person_Repository
        {
            get
            {
                IPersonRepository repository = new SQLPersonRepository();
                return repository;
            }
        }

        public static IDepartmentRepository Department_Repository
        {
            get
            {
                IDepartmentRepository repository = new SQLDepartmentRepository();
                return repository;
            }
        }

        public static IProgramRepository Program_Repository
        {
            get
            {
                IProgramRepository repository = new SQLProgramRepository();
                return repository;
            }
        }

        public static IGradeRepository Grade_Repository
        {
            get
            {
                IGradeRepository repository = new SqlGradeRepository();
                return repository;
            }
        }

        public static IRoleRepository Role_Repository
        {
            get
            {
                IRoleRepository repository = new SQLRoleRepository();
                return repository;
            }
        }

        public static IRoleMenuRepository RoleMenu_Repository
        {
            get
            {
                IRoleMenuRepository repository = new SQLRoleMenuRepository();
                return repository;
            }
        }

        public static IUserInPersonRepository UserInPerson_Repository
        {
            get
            {
                IUserInPersonRepository repository = new SQLUserInPersonRepository();
                return repository;
            }
        }

        public static IUserRepository User_Repository
        {
            get
            {
                IUserRepository repository = new SQLUserRepository();
                return repository;
            }
        }

        public static IEmployeeRepository Employee_Repository
        {
            get
            {
                IEmployeeRepository repository = new SQLEmployeeRepository();
                return repository;
            }
        }
        public static IPersonAdditionalInfoRepository PersonAdditionalInfo_Repository
        {
            get
            {
                IPersonAdditionalInfoRepository repository = new SqlPersonAdditionalInfoRepository();
                return repository;
            }
        }
        public static IUserMenuRepository UserMenu_Repository
        {
            get
            {
                IUserMenuRepository repository = new SQLUserMenuRepository();
                return repository;
            }
        }

        public static IUsrPermsnRepository UsrPermsn_Repository
        {
            get
            {
                IUsrPermsnRepository repository = new SQLUsrPermsnRepository();
                return repository;
            }
        }

        public static ILogLoginLogoutRepository LogLoginLogout_Repository
        {
            get
            {
                ILogLoginLogoutRepository repository = new SqlLogLoginLogoutRepository();
                return repository;
            }
        }

        public static IUserAccessProgramRepository UserAccessProgram_Repository
        {
            get
            {
                IUserAccessProgramRepository repository = new SqlUserAccessProgramRepository();
                return repository;
            }
        }

        public static IBatchRepository Batch_Repository
        {
            get
            {
                IBatchRepository repository = new SQLBatchRepository();
                return repository;
            }
        }

        public static ICourseRepository Course_Repository
        {
            get
            {
                ICourseRepository repository = new SQLCourseRepository();
                return repository;
            }
        }

        public static IAcademicCalenderSectionRepository AcademicCalenderSection_Repository
        {
            get
            {
                IAcademicCalenderSectionRepository repository = new SQLAcademicCalenderSectionRepository();
                return repository;
            }
        }

        public static ITreeCalendarDetailRepository TreeCalendarDetail_Repository
        {
            get
            {
                ITreeCalendarDetailRepository repository = new SQLTreeCalendarDetailRepository();
                return repository;
            }
        }

        public static ITreeCalendarMasterRepository TreeCalendarMaster_Repository
        {
            get
            {
                ITreeCalendarMasterRepository repository = new SQLTreeCalendarMasterRepository();
                return repository;
            }
        }

        public static ITreeDetailRepository TreeDetail_Repository
        {
            get
            {
                ITreeDetailRepository repository = new SQLTreeDetailRepository();
                return repository;
            }
        }

        public static ITreeMasterRepository TreeMaster_Repository
        {
            get
            {
                ITreeMasterRepository repository = new SQLTreeMasterRepository();
                return repository;
            }
        }

        public static IStudentRepository Student_Repository
        {
            get
            {
                IStudentRepository repository = new SQLStudentRepository();
                return repository;
            }
        }

        public static ITypeDefinitionRepository TypeDefinition_Repository
        {
            get
            {
                ITypeDefinitionRepository repository = new SQLTypeDefinitionRepository();
                return repository;
            }
        }

        public static ICalenderUnitTypeRepository CalenderUnitType_Repository
        {
            get
            {
                ICalenderUnitTypeRepository repository = new SQLCalenderUnitTypeRepository();
                return repository;
            }
        }

        public static IValueRepository Value_Repository
        {
            get
            {
                IValueRepository repository = new SQLValueRepository();
                return repository;
            }
        }

        public static IValueSetRepository ValueSet_Repository
        {
            get
            {
                IValueSetRepository repository = new SQLValueSetRepository();
                return repository;
            }
        }

        public static IAddressRepository Address_Repository
        {
            get
            {
                IAddressRepository repository = new SQLAddressRepository();
                return repository;
            }
        }

        public static IPersonPreviousExamRepository PersonPreviousExam_Repository
        {
            get
            {
                IPersonPreviousExamRepository repository = new SQLPersonPreviousExamRepository();
                return repository;
            }
        }

        public static IPreviousExamRepository PreviousExam_Repository
        {
            get
            {
                IPreviousExamRepository repository = new SQLPreviousExamRepository();
                return repository;
            }
        }

        public static IPreviousExamTypeRepository PreviousExamType_Repository
        {
            get
            {
                IPreviousExamTypeRepository repository = new SQLPreviousExamTypeRepository();
                return repository;
            }
        }

        public static IContactDetailsRepository ContactDetails_Repository
        {
            get
            {
                IContactDetailsRepository repository = new SQLContactDetailsRepository();
                return repository;
            }
        }

        public static ICourseListByNodeDTORepository CourseListByNodeDTO_Repository
        {
            get
            {
                ICourseListByNodeDTORepository repository = new SQLCourseListByNodeDTORepository();
                return repository;
            }
        }

        public static IOfferedCourseRepository OfferedCourse_Repository
        {
            get
            {
                IOfferedCourseRepository repository = new SQLOfferedCourseRepository();
                return repository;
            }
        }

        public static INodeRepository Node_Repository
        {
            get
            {
                INodeRepository repository = new SQLNodeRepository();
                return repository;
            }
        }

        public static ICalenderUnitMasterRepository CalenderUnitMaster_Repository
        {
            get
            {
                ICalenderUnitMasterRepository repository = new SQLCalenderUnitMasterRepository();
                return repository;
            }
        }

        public static ICalCourseProgNodeRepository CalCourseProgNode_Repository
        {
            get
            {
                ICalCourseProgNodeRepository repository = new SQLCalCourseProgNodeRepository();
                return repository;
            }
        }

        public static IVNodeSetRepository VNodeSet_Repository
        {
            get
            {
                IVNodeSetRepository repository = new SQLVNodeSetRepository();
                return repository;
            }
        }

        public static IVNodeSetMasterRepository VNodeSetMaster_Repository
        {
            get
            {
                IVNodeSetMasterRepository repository = new SQLVNodeSetMasterRepository();
                return repository;
            }
        }

        public static IOperatorRepository Operator_Repository
        {
            get
            {
                IOperatorRepository repository = new SQLOperatorRepository();
                return repository;
            }
        }

        public static INode_CourseRepository Node_Course_Repository
        {
            get
            {
                INode_CourseRepository repository = new SQLNode_CourseRepository();
                return repository;
            }
        }

        public static ILogGeneralRepository LogGeneral_Repository
        {
            get
            {
                ILogGeneralRepository repository = new SQLLogGeneralRepository();
                return repository;
            }
        }

        public static IExamTemplateMasterRepository ExamTemplateMaster_Repository
        {
            get
            {
                IExamTemplateMasterRepository repository = new SqlExamTemplateMasterRepository();
                return repository;
            }
        }

        public static IStudentCourseHistoryRepository StudentCourseHistory_Repository
        {
            get
            {
                IStudentCourseHistoryRepository repository = new SQLStudentCourseHistoryRepository();
                return repository;
            }
        }

        public static IExamMarkDetailsRepository ExamMarkDetails_Repository
        {
            get
            {
                IExamMarkDetailsRepository repository = new SqlExamMarkDetailsRepository();
                return repository;
            }
        }

        public static IRoomInformationRepository RoomInformation_Repository
        {
            get
            {
                IRoomInformationRepository repository = new SQLRoomInformationRepository();
                return repository;
            }
        }

        public static ICourseStatusRepository CourseStatus_Repository
        {
            get
            {
                ICourseStatusRepository repository = new SQLCourseStatusRepository();
                return repository;
            }
        }

        public static ITimeSlotPlanRepository TimeSlotPlan_Repository
        {
            get
            {
                ITimeSlotPlanRepository repository = new SQLTimeSlotPlanRepository();
                return repository;
            }
        }

        public static IShareProgramInSectionRepository ShareProgramInSection_Repository
        {
            get
            {
                IShareProgramInSectionRepository repository = new SQLShareProgramInSectionRepository();
                return repository;
            }
        }

        public static IShareBatchInSectionRepository ShareBatchInSection_Repository
        {
            get
            {
                IShareBatchInSectionRepository repository = new SQLShareBatchInSectionRepository();
                return repository;
            }
        }

        public static IGradeSheetTemplateRepository GradeSheetTemplate_Repository
        {
            get
            {
                IGradeSheetTemplateRepository repository = new SQLGradeSheetTemplateRepository();
                return repository;
            }
        }

        public static IGradeDetailsRepository GradeDetails_Repository
        {
            get
            {
                IGradeDetailsRepository repository = new SQLGradeDetailsRepository();
                return repository;
            }
        }

        public static IExamTemplateCalculativeFormulaRepository ExamTemplateCalculativeFormula_Repository
        {
            get
            {
                IExamTemplateCalculativeFormulaRepository repository = new SqlExamTemplateCalculativeFormulaRepository();
                return repository;
            }
        }

        public static IExamTemplateCalculationTypeRepository ExamTemplateCalculationType_Repository
        {
            get
            {
                IExamTemplateCalculationTypeRepository repository = new SqlExamTemplateCalculationTypeRepository();
                return repository;
            }
        }

        public static IExamTemplateBasicItemDetailsRepository ExamTemplateBasicItemDetails_Repository
        {
            get
            {
                IExamTemplateBasicItemDetailsRepository repository = new SqlExamTemplateBasicItemDetailsRepository();
                return repository;
            }
        }

        public static IExamMetaTypeRepository ExamMetaType_Repository
        {
            get
            {
                IExamMetaTypeRepository repository = new SqlExamMetaTypeRepository();
                return repository;
            }
        }

        public static IExamMarkMasterRepository ExamMarkMaster_Repository
        {
            get
            {
                IExamMarkMasterRepository repository = new SqlExamMarkMasterRepository();
                return repository;
            }
        }

        public static IExamDefinitionRepository ExamDefinition_Repository
        {
            get
            {
                IExamDefinitionRepository repository = new SqlExamDefinitionRepository();
                return repository;
            }
        }

        public static IStudentAdditionalInfoRepository StudentAdditionalInfo_Repository
        {
            get
            {
                IStudentAdditionalInfoRepository repository = new SqlStudentAdditionalInfoRepository();
                return repository;
            }
        }

        public static IYearSectionRepository YearSection_Repository
        {
            get
            {
                IYearSectionRepository repository = new SqlYearSectionRepository();
                return repository;
            }
        }

        public static IYearRepository Year_Repository
        {
            get
            {
                IYearRepository repository = new SqlYearRepository();
                return repository;
            }
        }

        public static ISemesterRepository Semester_Repository
        {
            get
            {
                ISemesterRepository repository = new SqlSemesterRepository();
                return repository;
            }
        }

        public static IRegistrationWorksheetRepository RegistrationWorksheet_Repository
        {
            get
            {
                IRegistrationWorksheetRepository repository = new SqlRegistrationWorksheetRepository();
                return repository;
            }
        }

        public static IFundTypeRepository FundType_Repository
        {
            get
            {
                IFundTypeRepository repository = new SqlFundTypeRepository();
                return repository;
            }
        }

        public static IFeeSetupRepository FeeSetup_Repository
        {
            get
            {
                IFeeSetupRepository repository = new SqlFeeSetupRepository();
                return repository;
            }
        }

        public static IFeeGroupDetailRepository  FeeGroupDetail_Repository
        {
            get
            {
                IFeeGroupDetailRepository repository = new SqlFeeGroupDetailRepository();
                return repository;
            }
        }

        public static IFeeGroupMasterRepository FeeGroupMaster_Repository
        {
            get
            {
                IFeeGroupMasterRepository repository = new SqlFeeGroupMasterRepository();
                return repository;
            }
        }
        
        public static IBillHistoryRepository BillHistory_Repository
        {
            get
            {
                IBillHistoryRepository repository = new SqlBillHistoryRepository();
                return repository;
            }
        }

        public static IBillHistoryMasterRepository BillHistoryMaster_Repository
        {
            get
            {
                IBillHistoryMasterRepository repository = new SqlBillHistoryMasterRepository();
                return repository;
            }
        }

        public static ICollectionHistoryRepository CollectionHistory_Repository
        {
            get
            {
                ICollectionHistoryRepository repository = new SQLCollectionHistoryRepository();
                return repository;
            }
        }

        public static ICalenderUnitDistributionRepository CalenderUnitDistribution_Repository
        {
            get
            {
                ICalenderUnitDistributionRepository repository = new SqlCalenderUnitDistributionRepository();
                return repository;
            }
        }

        public static ICourseExtendOneRepository CourseExtendOne_Repository
        {
            get
            {
                ICourseExtendOneRepository repository = new SqlCourseExtendOneRepository();
                return repository;
            }
        }

        public static IExamTemplateItemRepository ExamTemplateItem_Repository
        {
            get
            {
                IExamTemplateItemRepository repository = new SqlExamTemplateItemRepository();
                return repository;
            }
        }

        public static IExamTemplateRepository ExamTemplate_Repository
        {
            get
            {
                IExamTemplateRepository repository = new SqlExamTemplateRepository();
                return repository;
            }
        }

        public static IExamMarkEquationColumnOrderRepository ExamMarkEquationColumnOrder_Repository
        {
            get
            {
                IExamMarkEquationColumnOrderRepository repository = new SqlExamMarkEquationColumnOrderRepository();
                return repository;
            }
        }
        public static IEmployeeTypeRepository EmployeeType_Repository
        {
            get
            {
                IEmployeeTypeRepository repository = new SqlEmployeeTypeRepository();
                return repository;
            }
        }

        //public static IExamMarkRepository ExamMark_Repository
        //{
        //    get
        //    {
        //        IExamMarkRepository repository = new SqlExamMarkRepository();
        //        return repository;
        //    }
        //}

        public static IExamSetupRepository ExamSetup_Repository
        {
            get
            {
                IExamSetupRepository repository = new SQLExamSetupRepository();
                return repository;
            }
        }
        public static IExaminorSetupRepository ExaminorSetup_Repository
        {
            get
            {
                IExaminorSetupRepository repository = new SQLExaminorSetupRepository();
                return repository;
            }
        }

        public static IExamMarkFirstSecondThirdExaminerRepository ExamMarkFirstSecondThirdExaminer_Repository
        {
            get
            {
                IExamMarkFirstSecondThirdExaminerRepository repository = new SqlExamMarkFirstSecondThirdExaminerRepository();
                return repository;
            }
        }

        public static IExamMarkQuestionRepository ExamMarkQuestion_Repository
        {
            get
            {
                IExamMarkQuestionRepository repository = new SqlExamMarkQuestionRepository();
                return repository;
            }
        }

        public static IExamSetupDetailRepository ExamSetupDetail_Repository
        {
            get
            {
                IExamSetupDetailRepository repository = new SqlExamSetupDetailRepository();
                return repository;
            }
        }

        public static IExamSetupWithExamCommitteesRepository ExamSetupWithExamCommittees_Repository
        {
            get
            {
                IExamSetupWithExamCommitteesRepository repository = new SqlExamSetupWithExamCommitteesRepository();
                return repository;
            }
        }

        public static IPreviousEducationRepository PreviousEducation_Repository
        {
            get
            {
                IPreviousEducationRepository repository = new SqlPreviousEducationRepository();
                return repository;
            }
        }

        public static IExamMarkSubmissionDateRepository ExamMarkSubmissionDate_Repository
        {
            get
            {
                IExamMarkSubmissionDateRepository repository = new SqlExamMarkSubmissionDateRepository();
                return repository;
            }
        }

        public static IExaminerSetupStudentWiseRepository ExaminerSetupStudentWise_Repository
        {
            get
            {
                IExaminerSetupStudentWiseRepository repository = new SqlExaminerSetupStudentWiseRepository();
                return repository;
            }
        }

        public static IDataTableRepository DataTable_Repository
        {
            get
            {
                IDataTableRepository repository = new SqlDataTableRepository();
                return repository;
            }
        }

        public static IStudentApplicationFormRepository StudentApplicationForm_Repository
        {
            get
            {
                IStudentApplicationFormRepository repository = new SQLStudentApplicationFormRepository();
                return repository;
            }
        }


        public static IPasswordResetURLInfoRepository PasswordResetURLInfo_Repository
        {
            get
            {
                IPasswordResetURLInfoRepository repository = new SqlPasswordResetURLInfoRepository();
                return repository;
            }
        }

        public static IStudentAttendancePercentageStatusRepository StudentAttendancePercentageStatus_Repository
        {
            get
            {
                IStudentAttendancePercentageStatusRepository repository = new SqlStudentAttendancePercentageStatusRepository();
                return repository;
            }
        }

    }
}