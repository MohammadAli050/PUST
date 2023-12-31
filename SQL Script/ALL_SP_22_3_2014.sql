
GO
/****** Object:  StoredProcedure [dbo].[_Update_SL_1_StudentDiplomaStatus_From_Dip123to131]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_Update_SL_1_StudentDiplomaStatus_From_Dip123to131]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	update UIUEMS_ER_Student set IsDiploma=1 where StudentID in (
select s.StudentID from UIUEMS_ER_Student as s inner join _Diploma123to131 as d on d.StudentID=s.Roll)
END




GO
/****** Object:  StoredProcedure [dbo].[_Update_SL_2_StudentDiplomaStatus_From_StudentWiseWaiverPercentage]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_Update_SL_2_StudentDiplomaStatus_From_StudentWiseWaiverPercentage]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	update UIUEMS_ER_Student set IsDiploma=1 where StudentID in (
	select s.StudentID  from UIUEMS_ER_Student as s inner join 
	(select * from _StudentWiseWeiverPercentage where WaiverPercentage = 40) 
	 as d on d.StudentID=s.Roll)

	
END




GO
/****** Object:  StoredProcedure [dbo].[_Update_SL_3_CourseMACUSStatus]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[_Update_SL_3_CourseMACUSStatus]
AS
BEGIN
	
	SET NOCOUNT ON;
	BEGIN
		update UIUEMS_CC_Course set HasMultipleACUSpan=1 where CourseID in (
		select CourseID from UIUEMS_CC_Course where FormalCode like '%400%');
	END

	BEGIN
		update UIUEMS_CC_Course set HasMultipleACUSpan=1 where CourseID in (
		select CourseID from UIUEMS_CC_Course where FormalCode like '%6000%');
	END
END




GO
/****** Object:  StoredProcedure [dbo].[AcademicCalenderGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AcademicCalenderGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[AcademicCalenderID],
[SLNO],
[CalenderUnitTypeID],
[Year],
[BatchCode],
[IsCurrent],
[IsNext],
[StartDate],
[EndDate],
[FullPayNoFineLstDt],
[FirstInstNoFineLstDt],
[SecInstNoFineLstDt],
[ThirdInstNoFineLstDs],
[AddDropLastDateFull],
[AddDropLastDateHalf],
[LastDateEnrollNoFine],
[LastDateEnrollWFine],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[AdmissionStartDate],
[AdmissionEndDate],
[IsActiveAdmission],
[RegistrationStartDate],
[RegistrationEndDate],
[IsActiveRegistration]


FROM       UIUEMS_CC_AcademicCalender


END




GO
/****** Object:  StoredProcedure [dbo].[AcademicCalenderGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AcademicCalenderGetById]
(
@AcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[AcademicCalenderID],
[SLNO],
[CalenderUnitTypeID],
[Year],
[BatchCode],
[IsCurrent],
[IsNext],
[StartDate],
[EndDate],
[FullPayNoFineLstDt],
[FirstInstNoFineLstDt],
[SecInstNoFineLstDt],
[ThirdInstNoFineLstDs],
[AddDropLastDateFull],
[AddDropLastDateHalf],
[LastDateEnrollNoFine],
[LastDateEnrollWFine],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[AdmissionStartDate],
[AdmissionEndDate],
[IsActiveAdmission],
[RegistrationStartDate],
[RegistrationEndDate],
[IsActiveRegistration]


FROM       UIUEMS_CC_AcademicCalender
WHERE     (AcademicCalenderID = @AcademicCalenderID)

END




GO
/****** Object:  StoredProcedure [dbo].[AllCourseByNodeCursorParam]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ashraf>
-- Create date: <26,4,2011>
-- Description:	<select course info by all types of nodeID>
-- =============================================
CREATE PROCEDURE [dbo].[AllCourseByNodeCursorParam] 
(
	@cursorNode CURSOR VARYING OUTPUT,
	@NodeId int
)
AS
BEGIN
SET NOCOUNT ON;

------------------------------------------------------------------------------------------------
--Select all course from virtual node	
	IF EXISTS(select IsVirtual from UIUEMS_CC_Node where IsVirtual = 1 and NodeID = @NodeId)
		BEGIN 
		SET   @cursorNode = CURSOR 
				 FOR
		
			WITH Tree (NodeID, OperandNodeID)
				AS
				(
					SELECT NodeID, OperandNodeID
					FROM UIUEMS_CC_VNodeSet WHERE NodeID = @NodeId

					UNION ALL

					SELECT UIUEMS_CC_TreeDetail.ParentNodeID, UIUEMS_CC_TreeDetail.ChildNodeID
						 	
					FROM UIUEMS_CC_TreeDetail Inner JOIN Tree 
					ON UIUEMS_CC_TreeDetail.ParentNodeID = Tree.OperandNodeID 
				)
				
				
				SELECT NC.CourseID, NC.VersionID, NC.Node_CourseID 

				FROM Tree INNER JOIN UIUEMS_CC_Node as N
					ON Tree.OperandNodeID = N.NodeID inner join UIUEMS_CC_Node_Course as NC
					ON NC.NodeID = N.NodeID inner join UIUEMS_CC_Course as C
					ON C.CourseID = NC.CourseID and C.VersionID = NC.VersionID

					Order by Tree.NodeID, OperandNodeID,C.FormalCode, C.VersionCode
					
					
		END
------------------------------------------------------------------------------------------------
--Select all course from non leaf node	
	ELSE IF EXISTS(select IsLastLevel from UIUEMS_CC_Node where IsVirtual = 0 and IsLastLevel = 0 and NodeID = @NodeId)
		BEGIN
SET   @cursorNode = CURSOR 
				 FOR
				WITH Tree (ParentNodeID, ChildNodeID)
				AS
				(
					SELECT ParentNodeID, ChildNodeID
					FROM UIUEMS_CC_TreeDetail WHERE ParentNodeID = @NodeId

					UNION ALL

					SELECT UIUEMS_CC_TreeDetail.ParentNodeID, UIUEMS_CC_TreeDetail.ChildNodeID
						 	
					FROM UIUEMS_CC_TreeDetail Inner JOIN Tree 
					ON Tree.ChildNodeID = UIUEMS_CC_TreeDetail.ParentNodeID  
				)
				SELECT  NC.CourseID, NC.VersionID, NC.Node_CourseID

				from Tree INNER JOIN UIUEMS_CC_Node as N

				ON Tree.ChildNodeID = N.NodeID inner join UIUEMS_CC_Node_Course as NC
				ON NC.NodeID = N.NodeID inner join UIUEMS_CC_Course as C
				ON C.CourseID = NC.CourseID and C.VersionID = NC.VersionID

				Order by ParentNodeID, ChildNodeID,C.FormalCode, C.VersionCode
				
		END
------------------------------------------------------------------------------------------------
--Select all course from leaf node	
	ELSE
		BEGIN
SET   @cursorNode = CURSOR 
				 FOR
			select  NC.CourseID, NC.VersionID, NC.Node_CourseID

					from UIUEMS_CC_Node AS N INNER JOIN 
					UIUEMS_CC_Node_Course AS NC ON N.NodeID = NC.NodeID inner join 
					UIUEMS_CC_Course as C ON NC.CourseID = C.CourseID and NC.VersionID = C.VersionID
						where N.NodeID = @NodeId
					Order by N.NodeID, C.FormalCode, C.VersionCode	
						
		END
		OPEN @cursorNode 
END




GO
/****** Object:  StoredProcedure [dbo].[BulkInsertDiscountContinuationSetupFrom_031_To_072]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkInsertDiscountContinuationSetupFrom_031_To_072] 
	@programId int = NULL
AS
BEGIN
	 
	SET NOCOUNT ON;

		declare @AcaCalTbl TABLE ( ID INT IDENTITY(1,1), AcaCalId  INT ) 
		declare @index int, @acaCalId int;

		insert into @AcaCalTbl select AcademicCalenderID from UIUEMS_CC_AcademicCalender where BatchCode between 031 and 072

		--select * from @AcaCalTbl

		set @index=1;

		while @index <= (select max(ID) from @AcaCalTbl) 
		BEGIN

			select  @acaCalId = AcaCalId from  @AcaCalTbl where ID = @index;
			
			INSERT [dbo].[UIUEMS_BL_DiscountContinuationSetup] 
			([BatchAcaCalID], [ProgramID], [TypeDefinitionID], [MinCredits], [MaxCredits], [MinCGPA], [Range], [PercentMin], [PercentMax], [Attribute1], [Attribute2], [Attribute3], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) 
			VALUES ( @acaCalId, @programId, 21, CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.00 AS Decimal(18, 2)), N'Range', CAST(0.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'', N'', N'', -1, CAST(0x0000A2C5012E7E58 AS DateTime), -1, CAST(0x0000A2C5012E7E58 AS DateTime))

			--INSERT [dbo].[UIUEMS_BL_DiscountContinuationSetup] 
			--([BatchAcaCalID], [ProgramID], [TypeDefinitionID], [MinCredits], [MaxCredits], [MinCGPA], [Range], [PercentMin], [PercentMax], [Attribute1], [Attribute2], [Attribute3], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) 
			--VALUES ( @acaCalId, @programId, 21, CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.50 AS Decimal(18, 2)), N'Range', CAST(51.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'', N'', N'', -1, CAST(0x0000A2C5012E7E58 AS DateTime), -1, CAST(0x0000A2C5012E7E58 AS DateTime))

			set @index = @index +1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[BulkInsertDiscountContinuationSetupFrom_073_To_142]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkInsertDiscountContinuationSetupFrom_073_To_142] 
	@programId int = NULL
AS
BEGIN
	 
	SET NOCOUNT ON;

		declare @AcaCalTbl TABLE ( ID INT IDENTITY(1,1), AcaCalId  INT ) 
		declare @index int, @acaCalId int;

		insert into @AcaCalTbl select AcademicCalenderID from UIUEMS_CC_AcademicCalender where BatchCode between 073 and 142

		--select * from @AcaCalTbl

		set @index=1;

		while @index <= (select max(ID) from @AcaCalTbl) 
		BEGIN

			select  @acaCalId = AcaCalId from  @AcaCalTbl where ID = @index;
						
			INSERT [dbo].[UIUEMS_BL_DiscountContinuationSetup] 
			([BatchAcaCalID], [ProgramID], [TypeDefinitionID], [MinCredits], [MaxCredits], [MinCGPA], [Range], [PercentMin], [PercentMax], [Attribute1], [Attribute2], [Attribute3], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate]) 
			VALUES ( @acaCalId, @programId, 21, CAST(9.00 AS Decimal(18, 2)), CAST(0.00 AS Decimal(18, 2)), CAST(3.50 AS Decimal(18, 2)), N'Range', CAST(0.00 AS Decimal(18, 2)), CAST(100.00 AS Decimal(18, 2)), N'', N'', N'', -1, CAST(0x0000A2C5012E7E58 AS DateTime), -1, CAST(0x0000A2C5012E7E58 AS DateTime))

			set @index = @index +1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[BulkInsertGradeWiseRetakeDiscountFrom_031_To_122]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkInsertGradeWiseRetakeDiscountFrom_031_To_122] 
	@programId int = NULL
AS
BEGIN
	 
	SET NOCOUNT ON;

		declare @AcaCalTbl TABLE ( ID INT IDENTITY(1,1), AcaCalId  INT ) 
		declare @index int, @acaCalId int;

		insert into @AcaCalTbl select AcademicCalenderID from UIUEMS_CC_AcademicCalender where BatchCode between 031 and 122

		--select * from @AcaCalTbl

		set @index=1;

		while @index <= (select max(ID) from @AcaCalTbl) 
		BEGIN

			select  @acaCalId = AcaCalId from  @AcaCalTbl where ID=@index;

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			( [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (1, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C60119587A AS DateTime), -1, CAST(0x0000A2C60119587A AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			( [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (2, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C0 AS DateTime), -1, CAST(0x0000A2C6011958C0 AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			([GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (3, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C1 AS DateTime), -1, CAST(0x0000A2C6011958C1 AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (4, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C1 AS DateTime), -1, CAST(0x0000A2C6011958C1 AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (5, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C2 AS DateTime), -1, CAST(0x0000A2C6011958C2 AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (6, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C2 AS DateTime), -1, CAST(0x0000A2C6011958C2 AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (7, @acaCalId, @programId, CAST(75.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C3 AS DateTime), -1, CAST(0x0000A2C6011958C3 AS DateTime),  CAST(75.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (8, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C4 AS DateTime), -1, CAST(0x0000A2C6011958C4 AS DateTime),  CAST(50.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (9, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C4 AS DateTime), -1, CAST(0x0000A2C6011958C4 AS DateTime),  CAST(50.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (10, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C4 AS DateTime), -1, CAST(0x0000A2C6011958C4 AS DateTime),  CAST(50.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			([GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (11, @acaCalId, @programId, CAST(0.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C5 AS DateTime), -1, CAST(0x0000A2C6011958C5 AS DateTime),  CAST(0.00 AS Numeric(18, 2)))

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			([GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (12, @acaCalId, @programId, CAST(0.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C5 AS DateTime), -1, CAST(0x0000A2C6011958C5 AS DateTime),  CAST(0.00 AS Numeric(18, 2)))

			set @index = @index +1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[BulkInsertGradeWiseRetakeDiscountFrom_123_To_142]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[BulkInsertGradeWiseRetakeDiscountFrom_123_To_142] 
	@programId int = NULL
AS
BEGIN
	 
	SET NOCOUNT ON;

		declare @AcaCalTbl TABLE ( ID INT IDENTITY(1,1), AcaCalId  INT ) 
		declare @index int, @acaCalId int;

		insert into @AcaCalTbl select AcademicCalenderID from UIUEMS_CC_AcademicCalender where BatchCode between 123 and 142

		--select * from @AcaCalTbl

		set @index=1;

		while @index <= (select max(ID) from @AcaCalTbl) 
		BEGIN

			select  @acaCalId = AcaCalId from  @AcaCalTbl where ID=@index;

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			( [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (1, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C60119587A AS DateTime), -1, CAST(0x0000A2C60119587A AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			( [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (2, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C0 AS DateTime), -1, CAST(0x0000A2C6011958C0 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			([GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (3, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C1 AS DateTime), -1, CAST(0x0000A2C6011958C1 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (4, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C1 AS DateTime), -1, CAST(0x0000A2C6011958C1 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (5, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C2 AS DateTime), -1, CAST(0x0000A2C6011958C2 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (6, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C2 AS DateTime), -1, CAST(0x0000A2C6011958C2 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (7, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C3 AS DateTime), -1, CAST(0x0000A2C6011958C3 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (8, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C4 AS DateTime), -1, CAST(0x0000A2C6011958C4 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (9, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C4 AS DateTime), -1, CAST(0x0000A2C6011958C4 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			(  [GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (10, @acaCalId, @programId, CAST(50.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C4 AS DateTime), -1, CAST(0x0000A2C6011958C4 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			([GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (11, @acaCalId, @programId, CAST(0.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C5 AS DateTime), -1, CAST(0x0000A2C6011958C5 AS DateTime), NULL)

			INSERT [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount] 
			([GradeId], [SessionId], [ProgramId], [RetakeDiscount], [CreatedBy], [CreatedDate], [ModifiedBy], [ModifiedDate], [RetakeDiscountOnTrOrWav]) 
			VALUES (12, @acaCalId, @programId, CAST(0.00 AS Numeric(18, 2)), -1, CAST(0x0000A2C6011958C5 AS DateTime), -1, CAST(0x0000A2C6011958C5 AS DateTime), NULL)

			set @index = @index +1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[CalenderUnitTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CalenderUnitTypeGetById]
(
@CalenderUnitTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[CalenderUnitTypeID],
[CalenderUnitMasterID],
[TypeName],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_CalenderUnitType
WHERE     (CalenderUnitTypeID = @CalenderUnitTypeID)

END




GO
/****** Object:  StoredProcedure [dbo].[ChartReportPreRegistration]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChartReportPreRegistration] 
@AcademicCalenderID int = null	

AS
BEGIN
	
	SET NOCOUNT ON;
	if(@AcademicCalenderID!=0)
     select count(distinct rw.StudentID) as NumberOfStudent,rw.ProgramID,p.ShortName  from UIUEMS_CC_RegistrationWorksheet as rw
inner join (select ShortName, ProgramID from UIUEMS_CC_Program  ) as p on p.ProgramID = rw.ProgramID
 where SectionName is not null and AcademicCalenderID=@AcademicCalenderID Group by rw.ProgramID,p.ShortName;
 else
  select count(distinct rw.StudentID) as NumberOfStudent,rw.ProgramID,p.ShortName  from UIUEMS_CC_RegistrationWorksheet as rw
inner join (select ShortName, ProgramID from UIUEMS_CC_Program  ) as p on p.ProgramID = rw.ProgramID
 where SectionName is not null Group by rw.ProgramID,p.ShortName;

END

GO
/****** Object:  StoredProcedure [dbo].[ChartReportPreRegistrationMaleFemale]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[ChartReportPreRegistrationMaleFemale]
@AcademicCalenderID int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if(@AcademicCalenderID!=0)
    select Male.NoOfMale as NumberOfMale, Female.NoOfFeMale as NumberOfFeMale,(Case When Male.ShortName is not null then Male.ShortName else Female.ShortName  End) as ShortName from ( select count(Distinct Reg.StudentID) as NoOfMale,Reg.ProgramID,Reg.ShortName from
(select rw.StudentID,rw.ProgramID,p.ShortName  from UIUEMS_CC_RegistrationWorksheet as rw
inner join (select ShortName, ProgramID from UIUEMS_CC_Program  ) as p on p.ProgramID = rw.ProgramID where SectionName is not null and AcademicCalenderID=@AcademicCalenderID) as Reg
inner join (select S.StudentID from UIUEMS_ER_Student as S inner join (Select PersonId from UIUEMS_ER_Person as P where P.Gender='Male') as P
   on S.PersonID =P.PersonID) as M on Reg.StudentID=M.StudentID Group by Reg.ProgramID,Reg.ShortName ) as Male FULL OUTER JOIN

    ( select count(Distinct Reg.StudentID) as NoOfFeMale,Reg.ProgramID,Reg.ShortName from (select rw.StudentID,rw.ProgramID,p.ShortName  from UIUEMS_CC_RegistrationWorksheet as rw
inner join (select ShortName, ProgramID from UIUEMS_CC_Program  ) as p on p.ProgramID = rw.ProgramID where SectionName is not null and AcademicCalenderID=@AcademicCalenderID) as Reg
inner join (select S.StudentID from UIUEMS_ER_Student as S inner join (Select PersonId from UIUEMS_ER_Person as P where P.Gender='FeMale') as P
   on S.PersonID =P.PersonID) as F on Reg.StudentID=F.StudentID Group by Reg.ProgramID,Reg.ShortName ) as Female on Male.ShortName=Female.ShortName;
   else
   select Male.NoOfMale as NumberOfMale, Female.NoOfFeMale as NumberOfFeMale,(Case When Male.ShortName is not null then Male.ShortName else Female.ShortName  End) as ShortName from ( select count(Distinct Reg.StudentID) as NoOfMale,Reg.ProgramID,Reg.ShortName from
(select rw.StudentID,rw.ProgramID,p.ShortName  from UIUEMS_CC_RegistrationWorksheet as rw
inner join (select ShortName, ProgramID from UIUEMS_CC_Program  ) as p on p.ProgramID = rw.ProgramID where SectionName is not null ) as Reg
inner join (select S.StudentID from UIUEMS_ER_Student as S inner join (Select PersonId from UIUEMS_ER_Person as P where P.Gender='Male') as P
   on S.PersonID =P.PersonID) as M on Reg.StudentID=M.StudentID Group by Reg.ProgramID,Reg.ShortName ) as Male FULL OUTER JOIN

    ( select count(Distinct Reg.StudentID) as NoOfFeMale,Reg.ProgramID,Reg.ShortName from (select rw.StudentID,rw.ProgramID,p.ShortName  from UIUEMS_CC_RegistrationWorksheet as rw
inner join (select ShortName, ProgramID from UIUEMS_CC_Program  ) as p on p.ProgramID = rw.ProgramID where SectionName is not null) as Reg
inner join (select S.StudentID from UIUEMS_ER_Student as S inner join (Select PersonId from UIUEMS_ER_Person as P where P.Gender='FeMale') as P
   on S.PersonID =P.PersonID) as F on Reg.StudentID=F.StudentID Group by Reg.ProgramID,Reg.ShortName ) as Female on Male.ShortName=Female.ShortName;

END

GO
/****** Object:  StoredProcedure [dbo].[ChartReportRegistration]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[ChartReportRegistration]
	@AcademicCalenderID int = null	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	if(@AcademicCalenderID!=0)
	select count(distinct CH.StudentID) as NumberOfStudent,S.ProgramID,S.ShortName from UIUEMS_CC_Student_CourseHistory  as CH 
 inner join (select St.StudentID,St.ProgramID,P.ShortName from UIUEMS_ER_Student as St 
 inner join (select ProgramID,ShortName from UIUEMS_CC_Program) as P 
 on St.ProgramId = P.ProgramId) as S
 on CH.StudentID = S.StudentID where CH.CourseStatusID = 7 and CH.AcaCalID=@AcademicCalenderID group by S.ProgramID,S.ShortName;
   
   else
   select count(distinct CH.StudentID) as NumberOfStudent,S.ProgramID,S.ShortName from UIUEMS_CC_Student_CourseHistory  as CH 
 inner join (select St.StudentID,St.ProgramID,P.ShortName from UIUEMS_ER_Student as St 
 inner join (select ProgramID,ShortName from UIUEMS_CC_Program) as P 
 on St.ProgramId = P.ProgramId) as S
 on CH.StudentID = S.StudentID where CH.CourseStatusID = 7 group by S.ProgramID,S.ShortName;

END

GO
/****** Object:  StoredProcedure [dbo].[CourseGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CourseGetAll]
	
AS
BEGIN
	
	SET NOCOUNT ON;
    
	SELECT * from dbo.UIUEMS_CC_Course
END




GO
/****** Object:  StoredProcedure [dbo].[CourseGetAllByProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CourseGetAllByProgram]
(
	@ProgramID Int = NULL
)

As
Begin
	Select * From UIUEMS_CC_Course Where ProgramID = @ProgramID;
End




GO
/****** Object:  StoredProcedure [dbo].[CourseGetByCourseIdVersionId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CourseGetByCourseIdVersionId]
(
	@CourseId Int = NULL,
	@VersionId Int = NULL
)

As
Begin
	Select * From UIUEMS_CC_Course Where CourseId = @CourseId and VersionId = @VersionId;
End




GO
/****** Object:  StoredProcedure [dbo].[CreateBillVoucher]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<saima,,Name>
-- Create date: <Create Date, ,>
-- Description:	<create bill voucher, >
-- =============================================
CREATE PROCEDURE [dbo].[CreateBillVoucher] 
(
	-- Add the parameters for the function here
	
	@Prefix nvarchar(50) = null
   ,@SLNO bigint = null
   ,@DrAccountHeadsID int = null
   ,@CrAccountHeadsID int = null
   ,@Amount money = null
   ,@PostedBy varchar(50) = null
   ,@CourseID int = null
   ,@VersionID int = null
   ,@Remarks nvarchar(500) = null
   ,@CreatedBy int = null
   ,@AcaCalID int = null
)
AS
BEGIN
	
	INSERT INTO [dbo].[UIUEMS_AC_Voucher]
							   ([Prefix]
							   ,[SLNO]
							   ,[DrAccountHeadsID]
							   ,[CrAccountHeadsID]
							   ,[Amount]
							   ,[PostedBy]
							   ,[CourseID]
							   ,[VersionID]
							   ,[Remarks]
							   ,[CreatedBy]
							   ,[CreatedDate]
							   ,AcaCalID
							)
						 VALUES
							   (@Prefix 
							   ,@SLNO 
							   ,@DrAccountHeadsID 
							   ,@CrAccountHeadsID 
							   ,@Amount 
							   ,@PostedBy 
							   ,@CourseID 
							   ,@VersionID
							   ,@Remarks 
							   ,@CreatedBy
							   ,getdate()
							   ,@AcaCalID
								)
								

END






GO
/****** Object:  StoredProcedure [dbo].[CustomRegistrationWorksheetGetAllByProgAcaCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CustomRegistrationWorksheetGetAllByProgAcaCal]
	--Parameter
	@ProgramID Int = NULL,
	@AcademicCalenderID Int = NULL
As
Begin
	SET NOCOUNT ON;
	
	Select CourseID, VersionID, FormalCode, CourseTitle, Count(Case IsAutoOpen When 'True' Then 1 Else NULL End) AutoOpen, Count(Case IsAutoAssign When 'True' Then 1 Else NULL End) AutoAssign 
	From RegistrationWorksheet 
	Where ProgramID = @ProgramID and AcademicCalenderID = @AcademicCalenderID
	Group by CourseID, VersionID, FormalCode, CourseTitle;
	
End



GO
/****** Object:  StoredProcedure [dbo].[ForceOperationGetByParameters]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-08-25 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[ForceOperationGetByParameters]
(
	@Semester Int = NULL,
	@Batch Nvarchar(20) = NULL,
	@Program Int = NULL,
	@Roll Nvarchar(20) = NULL,
	@AutoOpen Int = NULL,
	@PreRegister Int = NULL,
	@Mandatory Int = NULL
)
As


	--Select ws.ID As ID, std.Roll As StudentID, RTrim(Coalesce(pr.FirstName + ' ','') + Coalesce(pr.MiddleName + ' ', '') + Coalesce(pr.LastName + ' ', '')) As StudentName, cs.FormalCode As CourseCode, cs.Title As CourseName, cs.Credits As CourseCredit, ac.BatchCode As Semester, ws.IsMandatory As IsMandatory, ws.IsAutoAssign As IsAutoAssign, ws.IsAutoOpen As IsAutoOpen, ws.Priority As Priority, ws.SequenceNo As SequenceNo From UIUEMS_CC_RegistrationWorksheet ws, UIUEMS_ER_Student std, UIUEMS_ER_Person pr, UIUEMS_CC_Course cs, UIUEMS_CC_AcademicCalender ac Where ws.StudentID = std.StudentID and pr.PersonID = std.PersonID and ws.CourseID = cs.CourseID and ws.VersionID = cs.VersionID and ws.AcademicCalenderID = ac.AcademicCalenderID and ws.AcademicCalenderID = @Semester and SUBSTRING(std.Roll, 1, 3) = @Batch and ws.ProgramID = @Program and std.Roll = @Roll and ws.IsAutoOpen = CONVERT(bit, @AutoOpen) and ws.IsAutoAssign = CONVERT(bit, @PreRegister) and ws.IsMandatory = CONVERT(bit, @Mandatory);
	
	Declare @query nvarchar(max);
	Set @query = 'Select ws.ID As ID, std.Roll As StudentID, RTrim(Coalesce(pr.FirstName + '' '','''') + Coalesce(pr.MiddleName + '' '', '''') + Coalesce(pr.LastName + '' '', '''')) As StudentName, cs.FormalCode As CourseCode, cs.Title As CourseName, cs.Credits As CourseCredit, ac.BatchCode As Semester, ws.IsMandatory As IsMandatory, ws.IsAutoAssign As IsAutoAssign, ws.IsAutoOpen As IsAutoOpen, ws.Priority As Priority, ws.SequenceNo As SequenceNo From UIUEMS_CC_RegistrationWorksheet ws, UIUEMS_ER_Student std, UIUEMS_ER_Person pr, UIUEMS_CC_Course cs, UIUEMS_CC_AcademicCalender ac Where ws.StudentID = std.StudentID and pr.PersonID = std.PersonID and ws.CourseID = cs.CourseID and ws.VersionID = cs.VersionID and ws.AcademicCalenderID = ac.AcademicCalenderID';

	If @Semester != 0
	Begin
	 Set @query += ' And ws.OriginalCalID = ' + CONVERT(nvarchar(max), @Semester);
	End

	If @Batch != '' and @Batch is not null
	Begin
		Set @query += ' And SUBSTRING(std.Roll, 4, 3) = ' + @Batch;
	End

	If @Program != 0
	Begin
		Set @query += ' And ws.ProgramID = ' + CONVERT(nvarchar(max), @Program);
	End

	If @Roll != '' and @Roll is not null
	Begin
		Set @query += ' And std.Roll = ' + @Roll;
	End

	If @AutoOpen != 2
	Begin
		Set @query += ' And  ws.IsAutoOpen = ' + CONVERT(nvarchar(max), @AutoOpen);
	End

	If @PreRegister != 2
	Begin
		Set @query += ' And  ws.IsAutoAssign = ' + CONVERT(nvarchar(max), @PreRegister);
	End

	If @Mandatory != 2
	Begin
		Set @query += ' And  ws.IsMandatory = ' + CONVERT(nvarchar(max), @Mandatory);
	End

	--print @query
	Exec sp_executesql @query


GO
/****** Object:  StoredProcedure [dbo].[Migrate_CourseHistory_BulkData]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Migrate_CourseHistory_BulkData]
AS
BEGIN
	declare VP_courseHistory cursor
	For
	Select StudentID, CourseID, Semester, Grade, TransCredit, TransCreditPoint, TransHoursCompleted, GroupID, Retake, IsSelfStudy, ExcludeInWaiver from CourseRegistered;
	
	declare	@hStudentID nvarchar(255), @hCourseID nvarchar(255), @hSemester nvarchar(255), @hGrade nvarchar(255), @hTransCredit float, @hTransCreditPoint nvarchar(255), @hTransHoursCompleted nvarchar(255), @hGroupID nvarchar(255), @hRetake float, @hIsSelfStudy nvarchar(255), @hExcludeInWaiver nvarchar(255);
	
	open VP_courseHistory
	fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	while @@FETCH_STATUS = 0
	BEGIN
		Declare @StudentID Int, @RetakeNo Int, @ObtainedGPA numeric(18, 2), @ObtainedGrade varchar(2), @GradeId Int, @CourseStatusID Int, @AcaCalID Int, @CourseID Int, @VersionID Int, @CourseCredit numeric(18, 2), @IsConsiderGPA Bit, @CreatedBy Int, @CreatedDate DateTime;
		--Initial Value
		Set @CourseStatusID = NULL;
		Set @IsConsiderGPA = 'False';
		Set @StudentID = NULL;Set @RetakeNo = NULL;Set @ObtainedGPA = NULL;Set @ObtainedGrade = NULL;Set @GradeId = NULL;Set @CourseStatusID = NULL;Set @AcaCalID = NULL;Set @CourseID = NULL;Set @VersionID = NULL;Set @CourseCredit = NULL;Set @IsConsiderGPA = NULL;Set @CreatedBy = NULL;Set @CreatedDate = NULL;
		--Initial Value
		
		--Student Exist ?
		Select @StudentID = StudentID From [dbo].[UIUEMS_ER_Student] Where Roll = @hStudentID;
		If @StudentID Is Null
		Begin
			Print(@hStudentID);
			fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
			Continue;
		End
		--Student Exist ?
		Select @AcaCalID = AcademicCalenderID From [dbo].[UIUEMS_CC_AcademicCalender] Where BatchCode = @hSemester;
		Select @CourseID = CourseID, @VersionID = VersionID, @CourseCredit = Credits  from [dbo].[UIUEMS_CC_Course] where VersionCode = @hCourseID;
		--All Grade Exist ?
		Select @GradeId = GradeId, @ObtainedGrade = Grade, @ObtainedGPA = GradePoint From [dbo].[UIUEMS_CC_GradeDetails] Where Grade = @hGrade;
		If @GradeId Is Not Null
		Begin
			If @hRetake = 0
			Begin
				Set @IsConsiderGPA = 'False';
				If @hGrade != 'F'
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pn';
				Else
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			End
			Else If @hRetake = 1
			Begin
				Set @IsConsiderGPA = 'True';
				If @hGrade = 'S'
				Begin
					Set @IsConsiderGPA = 'False';
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
				End
				Else If @hGrade != 'F'
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
				Else
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			End
			Else If @hRetake = 3
			Begin
				Set @IsConsiderGPA = 'False';
				If @hGrade != 'F'
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
				Else
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			End
		End
		--All Grade Exist ?
		Else If @hGrade = 'I'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'I';
		End
		Else If @hGrade = 'P'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'P';
		End
		Else If @hGrade = 'W'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'W';
		End
		Else If @hGrade = 'X'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'X';
		End
		Else
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'R';
		End
		
		Set @CreatedBy = 1;
		Set @CreatedDate = GetDate();
		
		
		Declare @CourseHistoryId int;
		EXEC [dbo].[Student_CourseHistory_Insert] @CourseHistoryId output, @StudentID=@StudentID, @RetakeNo=@RetakeNo, @ObtainedGPA=@ObtainedGPA, @ObtainedGrade=@ObtainedGrade, @GradeId=@GradeId, @CourseStatusID=@CourseStatusID, @AcaCalID=@AcaCalID, @CourseID=@CourseID, @VersionID=@VersionID, @CourseCredit=@CourseCredit, @IsConsiderGPA=@IsConsiderGPA;
		
		
		fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	END
	close VP_courseHistory
	deallocate VP_courseHistory
END


GO
/****** Object:  StoredProcedure [dbo].[Migrate_CourseHistory_SingleTrimester]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[Migrate_CourseHistory_SingleTrimester]
(
	@BatchCode nvarchar(3) = NULL
)
AS
BEGIN
	declare VP_courseHistory cursor
	For
	Select StudentID, CourseID, Semester, Grade, TransCredit, TransCreditPoint, TransHoursCompleted, GroupID, Retake, IsSelfStudy, ExcludeInWaiver from CourseRegistered Where Semester = @BatchCode;
	
	declare	@hStudentID nvarchar(255), @hCourseID nvarchar(255), @hSemester nvarchar(255), @hGrade nvarchar(255), @hTransCredit float, @hTransCreditPoint nvarchar(255), @hTransHoursCompleted nvarchar(255), @hGroupID nvarchar(255), @hRetake float, @hIsSelfStudy nvarchar(255), @hExcludeInWaiver nvarchar(255);
	
	open VP_courseHistory
	fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	while @@FETCH_STATUS = 0
	BEGIN
		Declare @StudentID Int, @RetakeNo Int, @ObtainedGPA numeric(18, 2), @ObtainedGrade varchar(2), @GradeId Int, @CourseStatusID Int, @AcaCalID Int, @CourseID Int, @VersionID Int, @CourseCredit numeric(18, 2), @IsConsiderGPA Bit, @CreatedBy Int, @CreatedDate DateTime;
		--Initial Value
		Set @CourseStatusID = NULL;
		Set @IsConsiderGPA = 'False';
		Set @StudentID = NULL;Set @RetakeNo = NULL;Set @ObtainedGPA = NULL;Set @ObtainedGrade = NULL;Set @GradeId = NULL;Set @CourseStatusID = NULL;Set @AcaCalID = NULL;Set @CourseID = NULL;Set @VersionID = NULL;Set @CourseCredit = NULL;Set @IsConsiderGPA = NULL;Set @CreatedBy = NULL;Set @CreatedDate = NULL;
		--Initial Value
		
		--Student Exist ?
		Select @StudentID = StudentID From [dbo].[UIUEMS_ER_Student] Where Roll = @hStudentID;
		If @StudentID Is Null
		Begin
			Print('Not Found');
			Print(@hStudentID);
			fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
			Continue;
		End
		--Student Exist ?
		Select @AcaCalID = AcademicCalenderID From [dbo].[UIUEMS_CC_AcademicCalender] Where BatchCode = @hSemester;
		Select @CourseID = CourseID, @VersionID = VersionID, @CourseCredit = Credits  from [dbo].[UIUEMS_CC_Course] where VersionCode = @hCourseID;
		--All Grade Exist ?
		Select @GradeId = GradeId, @ObtainedGrade = Grade, @ObtainedGPA = GradePoint From [dbo].[UIUEMS_CC_GradeDetails] Where Grade = @hGrade;
		If @GradeId Is Not Null
		Begin
			If @hRetake = 0
			Begin
				Set @IsConsiderGPA = 'False';
				If @hGrade != 'F'
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pn';
				Else
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			End
			Else If @hRetake = 1
			Begin
				Set @IsConsiderGPA = 'True';
				If @hGrade = 'S'
				Begin
					Set @IsConsiderGPA = 'False';
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
				End
				Else If @hGrade != 'F'
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
				Else
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			End
			Else If @hRetake = 3
			Begin
				Set @IsConsiderGPA = 'False';
				If @hGrade != 'F'
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
				Else
					Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			End
		End
		--All Grade Exist ?
		Else If @hGrade = 'I'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'I';
		End
		Else If @hGrade = 'P'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'P';
		End
		Else If @hGrade = 'W'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'W';
		End
		Else If @hGrade = 'X'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'X';
		End
		Else
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'R';
		End
		
		Set @CreatedBy = 1;
		Set @CreatedDate = GetDate();
		
		
		If Not Exists(Select * From UIUEMS_CC_Student_CourseHistory Where StudentID = @StudentID and CourseID = @CourseID and VersionID = @VersionID and AcaCalID = @AcaCalID)
		Begin
			Declare @CourseHistoryId int;
			EXEC [dbo].[Student_CourseHistory_Insert] @CourseHistoryId output, @StudentID=@StudentID, @RetakeNo=@RetakeNo, @ObtainedGPA=@ObtainedGPA, @ObtainedGrade=@ObtainedGrade, @GradeId=@GradeId, @CourseStatusID=@CourseStatusID, @AcaCalID=@AcaCalID, @CourseID=@CourseID, @VersionID=@VersionID, @CourseCredit=@CourseCredit, @IsConsiderGPA=@IsConsiderGPA;
		End
		Else
		Begin
			Declare @statusId1 Int, @statusId2 Int, @statusId3 Int;
			Select @statusId1 = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'F';
			Select @statusId2 = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pn';
			Select @statusId3 = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Pt';
			If @hRetake = 1
			Begin
				Update UIUEMS_CC_Student_CourseHistory Set IsConsiderGPA = 'False' Where StudentID = @StudentID and CourseID = @CourseID and VersionID = @VersionID and AcaCalID != @AcaCalID and IsConsiderGPA = 'True' and CourseStatusID = @statusId1;
				Update UIUEMS_CC_Student_CourseHistory Set IsConsiderGPA = 'False', CourseStatusID = @statusId2 Where StudentID = @StudentID and CourseID = @CourseID and VersionID = @VersionID and AcaCalID != @AcaCalID and IsConsiderGPA = 'True' and CourseStatusID = @statusId3;
			End
			Update UIUEMS_CC_Student_CourseHistory Set ObtainedGPA=@ObtainedGPA, ObtainedGrade=@ObtainedGrade, GradeId=@GradeId, CourseStatusID=@CourseStatusID, IsConsiderGPA=@IsConsiderGPA Where StudentID = @StudentID and CourseID = @CourseID and VersionID = @VersionID and AcaCalID = @AcaCalID;
		End		
		
		Set @hStudentID = NULL; Set @hCourseID = NULL; Set @hSemester = NULL; Set @hGrade = NULL; Set @hTransCredit = NULL; Set @hTransCreditPoint = NULL; Set @hTransHoursCompleted = NULL; Set @hGroupID = NULL; Set @hRetake = NULL; Set @hIsSelfStudy = NULL; Set @hExcludeInWaiver = NULL;
		fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver;
	END
	close VP_courseHistory
	deallocate VP_courseHistory
END


GO
/****** Object:  StoredProcedure [dbo].[Migrate_CourseHistory_WithDraw]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-08-17 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_CourseHistory_WithDraw]
(
	@BatchCode nvarchar(3) = NULL
)
AS
BEGIN
	declare VP_courseHistory cursor
	For
	Select StudentID, CourseID, Semester, Grade, TransCredit, TransCreditPoint, TransHoursCompleted, GroupID, Retake, IsSelfStudy, ExcludeInWaiver from CourseRegistered
	Where Semester = @BatchCode and Grade = 'W';
	
	declare	@hStudentID nvarchar(255), @hCourseID nvarchar(255), @hSemester nvarchar(255), @hGrade nvarchar(255), @hTransCredit float, @hTransCreditPoint nvarchar(255), @hTransHoursCompleted nvarchar(255), @hGroupID nvarchar(255), @hRetake float, @hIsSelfStudy nvarchar(255), @hExcludeInWaiver nvarchar(255);
	
	open VP_courseHistory
	fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	while @@FETCH_STATUS = 0
	BEGIN
		Declare @StudentID Int, @CourseStatusID Int, @AcaCalID Int, @CourseID Int, @VersionID Int, @IsConsiderGPA Bit, @ModifiedBy Int, @ModifiedDate DateTime;
		--Initial Value
		Set @CourseStatusID = NULL;
		Set @IsConsiderGPA = 'False';
		Set @StudentID = NULL;Set @CourseStatusID = NULL;Set @AcaCalID = NULL;Set @CourseID = NULL;Set @VersionID = NULL;Set @IsConsiderGPA = NULL;Set @ModifiedBy = NULL;Set @ModifiedDate = NULL;
		--Initial Value
		
		--Student Exist ?
		Select @StudentID = StudentID From [dbo].[UIUEMS_ER_Student] Where Roll = @hStudentID;
		If @StudentID Is Null
		Begin
			Print(@hStudentID);
			fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
			Continue;
		End
		--Student Exist ?
		Select @AcaCalID = AcademicCalenderID From [dbo].[UIUEMS_CC_AcademicCalender] Where BatchCode = @hSemester;
		Select @CourseID = CourseID, @VersionID = VersionID  from [dbo].[UIUEMS_CC_Course] where VersionCode = @hCourseID;
		--All Grade Exist ?
		If @hGrade = 'W'
		Begin
			Set @IsConsiderGPA = 'False';
			Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'W';

			Set @ModifiedBy = 99;
			Set @ModifiedDate = GetDate();

			Update UIUEMS_CC_Student_CourseHistory Set IsConsiderGPA = @IsConsiderGPA, CourseStatusID = @CourseStatusID Where StudentID = @StudentID and AcaCalID = @AcaCalID and CourseID = @CourseID and VersionID = @VersionID;
			print(@StudentID);
		End
		
		fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	END
	close VP_courseHistory
	deallocate VP_courseHistory
END


GO
/****** Object:  StoredProcedure [dbo].[Migrate_SL_1_Dip123to131]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_SL_1_Dip123to131]

@discountAmount int = null
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @tmpDip123to131 table (ID INT IDENTITY(1,1),StudentID int, roll nvarchar(100), programid int, batchid int );

	INSERT INTO @tmpDip123to131 SELECT	tbl.StudentID, tbl.Roll, tbl.ProgramID, tbl.AdmissionCalenderID FROM 
	(SELECT	s.StudentID, s.Roll, s.ProgramID, ad.AdmissionCalenderID FROM _Diploma123to131 as dip 
	inner join UIUEMS_ER_Student as s on s.Roll=dip.StudentID
	inner join UIUEMS_ER_Admission as ad on ad.StudentID = s.StudentID) as tbl

	--select * from @tmpDip123to131

	DECLARE @i int, @Count int, @StudentID int,  @roll nvarchar(100), @programid int, 
	@batchid int, @stdDiscountMaster int, @dt datetime,  @stdDiscountInitial int, @td_DiplomaId int;

	SET @td_DiplomaId = 22;

	SET @i = 1;
	SET @Count = 0;
	SET @dt = GETDATE();

	WHILE @i <= (select MAX(ID) from @tmpDip123to131)
	BEGIN 
			SELECT  
			@StudentID = StudentID,  
			@roll = roll, 
			@programid = programid, 
			@batchid = batchid 
			FROM @tmpDip123to131 WHERE ID = @i;	
			

			IF NOT EXISTS(SELECT * FROM UIUEMS_BL_StudentDiscountMaster WHERE StudentId=@StudentID)
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster OUTPUT,  @StudentID, @batchid, @programid, 'Data migration Diploma', -1, @dt , -1, @dt;
				END
			ELSE
				BEGIN
					SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
											  WHERE (StudentId = @StudentID))
				END

			IF NOT EXISTS(SELECT * FROM UIUEMS_BL_StudentDiscountInitialDetails WHERE StudentDiscountId = @stdDiscountMaster and TypeDefinitionId=@td_DiplomaId)
			BEGIN
				EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial OUTPUT,  @stdDiscountMaster, @td_DiplomaId, @discountAmount, @batchid;
			END

			SET @i = @i + 1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[Migrate_SL_2_UiuAlumanai]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_SL_2_UiuAlumanai]
	
@discountAmount int = null

AS
BEGIN
	
	SET NOCOUNT ON;

	

DECLARE @tmpUiuAlumanai table ( ID INT IDENTITY(1,1),StudentID int, roll nvarchar(100), 
programid int, batchid int );

INSERT INTO @tmpUiuAlumanai SELECT	tbl.StudentID, tbl.Roll, tbl.ProgramID, tbl.AdmissionCalenderID FROM 
(SELECT	s.StudentID, s.Roll, s.ProgramID, ad.AdmissionCalenderID FROM _UiuAlumanai as alm 
inner join UIUEMS_ER_Student as s on s.Roll=alm.StudentID
inner join UIUEMS_ER_Admission as ad on ad.StudentID = s.StudentID) as tbl

--select * from @tmpUiuAlumanai

DECLARE @i int, @Count int, @StudentID int,  @roll nvarchar(100), @programid int, 
@batchid int, @stdDiscountMaster int, @dt datetime,  @stdDiscountInitial int,@td_UiuAlumniId int;

SET @td_UiuAlumniId = 18;

SET @i = 1;
SET @Count = 0;
SET @dt = GETDATE();

while @i <= (SELECT MAX(ID) FROM @tmpUiuAlumanai )
BEGIN 
		SELECT  
		@StudentID = StudentID,  
		@roll = roll, 
		@programid = programid, 
		@batchid = batchid 
		FROM @tmpUiuAlumanai WHERE ID = @i;	

		IF NOT EXISTS(SELECT * FROM UIUEMS_BL_StudentDiscountMaster WHERE StudentId=@StudentID)
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster OUTPUT,  @StudentID, @batchid, @programid, 'Data migration Uiu Alumanai', -1, @dt , -1, @dt;
				END
			ELSE
				BEGIN
					SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
											  WHERE (StudentId = @StudentID))
				END

		IF NOT EXISTS(select * from UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and TypeDefinitionId=@td_UiuAlumniId)
		BEGIN
			EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @td_UiuAlumniId, @discountAmount, @batchid;
		END

		SET @i = @i + 1;
END
	
END



GO
/****** Object:  StoredProcedure [dbo].[Migrate_SL_3_StudentWiseWeiverPercentage]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_SL_3_StudentWiseWeiverPercentage]
	
AS
BEGIN
	
SET NOCOUNT ON;


		DECLARE @tmpStudentWiseWeiverPercentage table ( ID INT IDENTITY(1,1),StudentID int, roll nvarchar(100), 
		programid int, batchid int, waiverPercentage float );

		INSERT INTO @tmpStudentWiseWeiverPercentage SELECT	tbl.StudentID, tbl.Roll, tbl.ProgramID, tbl.AdmissionCalenderID, tbl.WaiverPercentage FROM 
		(SELECT	s.StudentID, s.Roll, s.ProgramID, ad.AdmissionCalenderID, wp.WaiverPercentage  FROM _StudentWiseWeiverPercentage as wp 
		inner join UIUEMS_ER_Student as s on s.Roll=wp.StudentID
		inner join UIUEMS_ER_Admission as ad on ad.StudentID = s.StudentID where wp.WaiverPercentage != 0) as tbl

		--select * from @tmpStudentWiseWeiverPercentage
		--select count(*) from _UiuAlumanai
		DELETE  FROM @tmpStudentWiseWeiverPercentage where roll in(select StudentID from _UiuAlumanai)
		--select count(*) from @tmpStudentWiseWeiverPercentage
		--select count(*) from _Diploma123to131
		DELETE  FROM @tmpStudentWiseWeiverPercentage where roll in(select StudentID from _Diploma123to131)
		--select count(*) from @tmpStudentWiseWeiverPercentage
		--select * from @tmpStudentWiseWeiverPercentage

		DECLARE @i int, @Count int, @StudentID int,  @roll nvarchar(100), @programid int, 
		@batchid int, @stdDiscountMaster int, @dt datetime,  @stdDiscountInitial int, @WaiverPercentage float, @td_DiplomaId int, @td_TutionWaiverId int;

		SET @td_DiplomaId = 22;
		SET @td_TutionWaiverId = 21;

		set @i = 1;
		set @Count = 0;
		set @dt = getdate();

		while @i <= (select max(ID) from @tmpStudentWiseWeiverPercentage )
		BEGIN 
				SELECT  
				@StudentID = StudentID,  
				@roll = roll, 
				@programid = programid, 
				@batchid = batchid, 
				@WaiverPercentage = WaiverPercentage 
				FROM @tmpStudentWiseWeiverPercentage WHERE ID = @i;	

				IF NOT EXISTS(SELECT * FROM UIUEMS_BL_StudentDiscountMaster WHERE StudentId=@StudentID)
						BEGIN
							EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster OUTPUT,  @StudentID, @batchid, @programid, 'Data migration Student Wise Weiver Percentage', -1, @dt , -1, @dt;
						END
					ELSE
						BEGIN
							SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
													  WHERE (StudentId = @StudentID))
						END

				IF NOT EXISTS(SELECT * FROM UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and (TypeDefinitionId = @td_DiplomaId or TypeDefinitionId = @td_TutionWaiverId))
				BEGIN
					IF(@WaiverPercentage = 40)
						BEGIN
							EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @td_DiplomaId, @WaiverPercentage, @batchid;
						END
					ELSE
						BEGIN
							EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @td_TutionWaiverId, @WaiverPercentage, @batchid;
						END
				END

				set @i = @i + 1;
		END	
END



GO
/****** Object:  StoredProcedure [dbo].[Migrate_SL_4_CurrentWaiver]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[Migrate_SL_4_CurrentWaiver]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @tmpCurrentWaiver table (
								ID INT IDENTITY(1,1),
								StudentID int, 
								roll nvarchar(100), 
								programid int, 
								batchid int ,
								WaiverPercentage float );

	INSERT INTO @tmpCurrentWaiver SELECT	
										tbl.StudentID, 
										tbl.Roll, 
										tbl.ProgramID, 
										tbl.AdmissionCalenderID,
										tbl.WaiverPercentage
										FROM (SELECT	
													s.StudentID, 
													s.Roll, 
													s.ProgramID, 
													ad.AdmissionCalenderID ,
													wc.WaiverPercentage FROM _WaiverCurrent as wc 

													inner join UIUEMS_ER_Student as s on s.Roll=wc.StudentID
													inner join UIUEMS_ER_Admission as ad on ad.StudentID = s.StudentID ) as tbl

	--select * from @tmpCurrentWaiver
	

	DECLARE @i INT, 
			@Count INT, 

			@StudentID INT,  
			@roll NVARCHAR(100), 
			@programid INT, 
			@batchid INT, 
			@stdDiscountMaster INT, 
			@dt DATETIME,  
			@stdDiscountInitial INT, 
			@WaiverPercentage FLOAT,

			@TD_TutionWaiverId int = 21,
			@TD_DiplomaId int = 22

	set @i = 1;
	set @Count = 0;
	set @dt = getdate();

	while @i <= (select max(ID) from @tmpCurrentWaiver)
	BEGIN 
			SELECT  
			@StudentID = StudentID,  
			@roll = roll, 
			@programid = programid, 
			@batchid = batchid,
			@WaiverPercentage = WaiverPercentage
			FROM @tmpCurrentWaiver WHERE ID = @i;	

			IF NOT EXISTS(select * from UIUEMS_BL_StudentDiscountMaster where StudentId=@StudentID)
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster output,  @StudentID, @batchid, @programid, 'Data migration Current Waiver', -1, @dt , -1, @dt;
				END
			ELSE
				BEGIN
					SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
											  WHERE (StudentId = @StudentID))
				END

			IF EXISTS(select * from UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and TypeDefinitionId = @TD_DiplomaId )
				BEGIN
					UPDATE UIUEMS_BL_StudentDiscountInitialDetails
					SET TypePercentage = @WaiverPercentage
					WHERE StudentDiscountId=@stdDiscountMaster and TypeDefinitionId=@TD_DiplomaId
				END
			ELSE IF EXISTS(select * from UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and TypeDefinitionId = @TD_TutionWaiverId )
				BEGIN
					UPDATE UIUEMS_BL_StudentDiscountInitialDetails
					SET TypePercentage = @WaiverPercentage
					WHERE StudentDiscountId=@stdDiscountMaster and TypeDefinitionId=@TD_TutionWaiverId
				END
			ELSE
				BEGIN
					IF(@WaiverPercentage = 40)
						BEGIN
							EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @TD_DiplomaId, @WaiverPercentage, @batchid;
						END
					ELSE
						BEGIN
							EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @TD_TutionWaiverId, @WaiverPercentage, @batchid;
						END
				END

			SET @i = @i + 1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[Migrate_SL_5_SiblingSetup]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_SL_5_SiblingSetup]

@discountAmount int = null

AS
BEGIN
	
SET NOCOUNT ON;	



DECLARE @tmpSibling table (
								ID INT IDENTITY(1,1),
								StudentID int, 
								roll nvarchar(100), 
								programid int, 
								batchid int ,
								SiblingId int );

	INSERT INTO @tmpSibling SELECT	
										tbl.StudentID, 
										tbl.Roll, 
										tbl.ProgramID, 
										tbl.AdmissionCalenderID,
										tbl.SiblingId
										FROM (SELECT	
													s.StudentID, 
													s.Roll, 
													s.ProgramID, 
													ad.AdmissionCalenderID,
													s1.StudentID as SiblingId
													--s1.Roll as SiblingRoll
													 FROM _Siblings as T 

													INNER JOIN UIUEMS_ER_Student as s on s.Roll = T.StudentID
													INNER JOIN UIUEMS_ER_Admission as ad on ad.StudentID = s.StudentID 
													inner join UIUEMS_ER_Student as s1 on s1.Roll = T.Siblings) as tbl

	--select * from @tmpSibling
	

	DECLARE @i INT, 
			@Count INT, 
			@StudentID INT,  
			@roll NVARCHAR(100), 
			@programid INT, 
			@batchid INT, 
			@stdDiscountMaster INT, 
			@dt DATETIME,  
			@stdDiscountInitial INT, 
			@SiblingID INT,
			@SiblingSetup INT,
			@GroupId INT,

			@td_SiblingId int
			  
	set @td_SiblingId = 3;

	set @i = 1;
	set @Count = 0;
	set @dt = getdate();

	while @i <= (select max(ID) from @tmpSibling)
	BEGIN 
			SELECT  
			@StudentID = StudentID,  
			@roll = roll, 
			@programid = programid, 
			@batchid = batchid,
			@SiblingID = SiblingID
			FROM @tmpSibling WHERE ID = @i;			
				
			-- Insert Data into SiblingSetup table
			--delete from UIUEMS_AC_SiblingSetup where ApplicantId in (@StudentID, @SiblingID)

			if exists(select ApplicantId from UIUEMS_AC_SiblingSetup where ApplicantId = @StudentID )
				BEGIN
					SET @GroupId = (select GroupID from UIUEMS_AC_SiblingSetup where ApplicantId = @StudentID )
				END
			else if exists(select ApplicantId from UIUEMS_AC_SiblingSetup where ApplicantId = @SiblingID )
				BEGIN
					SET @GroupId = (select GroupID from UIUEMS_AC_SiblingSetup where ApplicantId = @SiblingID )
				END
			ELSE
				BEGIN
					SET @GroupId = (select ISNULL(MAX(GroupID), 0) from UIUEMS_AC_SiblingSetup) + 1
				END

			if NOT EXISTS(select ApplicantId from UIUEMS_AC_SiblingSetup where ApplicantId = @StudentID )
				BEGIN
					EXEC dbo.UIUEMS_AC_SiblingSetupInsert @SiblingSetup output, @GroupId,  @StudentID,  -1, @dt, -1, @dt;
				END
			if NOT EXISTS(select ApplicantId from UIUEMS_AC_SiblingSetup where ApplicantId = @SiblingID )
				BEGIN
					EXEC dbo.UIUEMS_AC_SiblingSetupInsert @SiblingSetup output, @GroupId,  @SiblingID,  -1, @dt, -1, @dt;
				END
			-- End


			-- Student entry START
			IF NOT EXISTS(select * from UIUEMS_BL_StudentDiscountMaster where StudentId = @StudentID)
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster output,  @StudentID, @batchid, @programid, 'Data migration Sibling Setup', -1, @dt, -1, @dt;
				END
			ELSE
				BEGIN
					SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
											  WHERE (StudentId = @StudentID))
				END

				--Details entry
					IF EXISTS(select * from UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and TypeDefinitionId = @td_SiblingId )
						BEGIN
							UPDATE UIUEMS_BL_StudentDiscountInitialDetails
							SET TypePercentage = @discountAmount
							WHERE StudentDiscountId=@stdDiscountMaster and TypeDefinitionId = @td_SiblingId
						END			
					ELSE				
						BEGIN
							EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @td_SiblingId, @discountAmount, @batchid;
						END
				--END
			-- END Student


			-- Sibling entry
			IF NOT EXISTS(select * from UIUEMS_BL_StudentDiscountMaster where StudentId = @SiblingID)
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster output,  @SiblingID, @batchid, @programid, 'Data migration Sibling Setup*', -1, @dt, -1, @dt;
				END
			ELSE
				BEGIN
					SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
											  WHERE (StudentId = @SiblingID))
				END

			--Details entry
			IF EXISTS(select * from UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and TypeDefinitionId = @td_SiblingId )
				BEGIN
					UPDATE UIUEMS_BL_StudentDiscountInitialDetails
					SET TypePercentage = @discountAmount
					WHERE StudentDiscountId=@stdDiscountMaster and TypeDefinitionId = @td_SiblingId
				END			
			ELSE				
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, @td_SiblingId, @discountAmount, @batchid;
				END	

			-- END Sibling						

			SET @i = @i + 1;
	END
END




GO
/****** Object:  StoredProcedure [dbo].[Migrate_SL_6_FreedomFighter]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[Migrate_SL_6_FreedomFighter]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @tmpFreedomFighter table (ID INT IDENTITY(1,1),StudentID int, roll nvarchar(100), programid int, batchid int  );

	INSERT INTO @tmpFreedomFighter SELECT	tbl.StudentID, tbl.Roll, tbl.ProgramID, tbl.AdmissionCalenderID FROM 
	(SELECT	s.StudentID, s.Roll, s.ProgramID, ad.AdmissionCalenderID FROM _FreedomFighter as ff 
	inner join UIUEMS_ER_Student as s on s.Roll=ff.StudentID
	inner join UIUEMS_ER_Admission as ad on ad.StudentID = s.StudentID) as tbl

	--select * from @tmpFreedomFighter

	declare @i int, @Count int, @StudentID int,  @roll nvarchar(100), @programid int, 
	@batchid int, @stdDiscountMaster int, @dt datetime,  @stdDiscountInitial int;

	set @i = 1;
	set @Count = 0;
	set @dt = getdate();

	while @i <= (select max(ID) from @tmpFreedomFighter)
	BEGIN 
			SELECT  
			@StudentID = StudentID,  
			@roll = roll, 
			@programid = programid, 
			@batchid = batchid 
			FROM @tmpFreedomFighter WHERE ID = @i;	

			IF NOT EXISTS(select * from UIUEMS_BL_StudentDiscountMaster where StudentId=@StudentID)
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountMasterInsert @stdDiscountMaster output,  @StudentID, @batchid, @programid, 'Data migration Freedom Fighter', -1, @dt , -1, @dt;
				END
			ELSE
				BEGIN
					SET @stdDiscountMaster = (SELECT  StudentDiscountId AS Expr1 FROM UIUEMS_BL_StudentDiscountMaster
											  WHERE (StudentId = @StudentID))
				END

			IF EXISTS(select * from UIUEMS_BL_StudentDiscountInitialDetails where StudentDiscountId = @stdDiscountMaster and TypeDefinitionId = 19 )
				BEGIN
					UPDATE UIUEMS_BL_StudentDiscountInitialDetails
					SET TypePercentage = 100
					WHERE StudentDiscountId=@stdDiscountMaster and TypeDefinitionId=19
				END
			ELSE
				BEGIN
					EXEC dbo.UIUEMS_BL_StudentDiscountInitialDetailsInsert @stdDiscountInitial output,  @stdDiscountMaster, 19, 100, @batchid;
				END

			set @i = @i + 1;
	END
END




GO
/****** Object:  StoredProcedure [dbo].[MigrateCandidate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[MigrateCandidate](@BatchCode nvarchar(3) = null) --this BatchCode comes from Student Table

AS
BEGIN
	declare @batchId int;
	select @batchId = AcademicCalenderID from [Admission.2.0.0].[dbo].[AcademicCalender] where BatchCode = @BatchCode;
	--Retrieve BatchId from Candidate DB
	declare @stdBatchId int;
	select @stdBatchId = AcademicCalenderID from [UIUEMS.2.0.0].[dbo].[UIUEMS_CC_AcademicCalender] where BatchCode = @BatchCode;
	--Retrieve BatchId from Student DB
	
	declare candidateList cursor
	for
	select StudentID, FirstName, Phone, BirthDate, ProgramId, PaymentSerial, CandidateId, ProvisionAdmission, ProvisionDate, IsPreEnglish, IsPremath, AdmissionFee
	from [Admission.2.0.0].[dbo].[Candidate]
	where BatchId = @batchId and StudentID != '' and StudentID not in (select Roll from [UIUEMS.2.0.0].[dbo].[UIUEMS_ER_Student]);
	--Store Candidate Data List Into Define cursor
	
	declare @studentRollID nvarchar(50), @firstName varchar(50), @Phone varchar(15), @birthDay datetime, @programId int, @paymentSerial nvarchar(50), @candidateId bigint, @provisionAdmission bit, @provisionDate datetime, @isPreEnglish bit, @isPremath bit, @Amount numeric(18, 2);
	--Variable declare for Student table
	open candidateList
	fetch next from candidateList into @studentRollID, @firstName, @phone, @birthDay, @programId, @paymentSerial, @candidateId, @provisionAdmission, @provisionDate, @isPreEnglish, @isPremath, @Amount
	--Fetch 1st ROW
	
	declare @datepicker date;
	set @datepicker = getdate();
	--Common declare
	
	while @@FETCH_STATUS = 0
	BEGIN
		--print(@studentId)
		
		declare @personId int
		EXEC [dbo].[UIUEMS_ER_PersonInsert] @PersonId output, @FirstName=@firstName, @DOB=@birthDay, @Phone=@phone, @CandidateID=@candidateId;
		--print(@PersonId);
		--UIUEMS_ER_Person Insert
		
		declare @AccountsId int;
		EXEC [dbo].[UIUEMS_AC_AccountHeadsInsert] @AccountsID output, @Name = @studentRollID, @ParentID=5, @CreatedBy = 1, @CreatedDate = @datepicker;
		--print(@AccountsID);
		--Student Account Insert
		
		declare @VoucherID int;
		EXEC [dbo].[UIUEMS_AC_VoucherInsert] @VoucherID output, @Amount = @Amount, @CreatedBy = 1, @CreatedDate = @datepicker;
		--print(@VoucherID);
		--Student Voucher Insert
		
		declare @StudentID int, @programRowId int, @isDiploma bit;
		set @programRowId = (select [UIUEMS.2.0.0].[dbo].GetProgramId(@programId));
		set @isDiploma = (select [UIUEMS.2.0.0].[dbo].IsDiploma(@candidateId));
		EXEC [dbo].[UIUEMS_ER_StudentInsert] @StudentID output, @Roll = @studentRollID, @FirstName = @firstName, @DOB = @birthDay, @ProgramID = @programRowId, @PersonId = @personId, @PaymentSlNo = @paymentSerial, @IsActive='True', @IsDiploma = @isDiploma, @AccountHeadsID = @AccountsId, @CandidateId = @candidateId, @IsProvisionalAdmission = @provisionAdmission, @ValidUptoProAdmissionDate = @provisionDate, @Pre_English = @isPreEnglish, @Pre_Math = @isPremath, @CreatedBy = 1, @CreatedDate = @datepicker;
		--print(@StudentID);
		--UIUEMS_ER_Student Insert		
		
		declare @AdmissionID int;
		EXEC [dbo].[UIUEMS_ER_AdmissionInsert] @AdmissionID output, @StudentID = @StudentID, @PersonID = @PersonId, @AdmissionCalenderID = @stdBatchId, @IsLastAdmission = 'True', @CreatedBy = 1, @CreatedDate = @datepicker;
		--print(@AdmissionID);
		--UIUEMS_ER_Admission Insert
		
		
		
		fetch next from candidateList into @studentId, @firstName, @phone, @birthDay, @programId, @paymentSerial, @candidateId, @provisionAdmission, @provisionDate, @isPreEnglish, @isPremath, @Amount
	END
	close candidateList
	deallocate candidateList
END



GO
/****** Object:  StoredProcedure [dbo].[MigrateFee_SL_1_AllFeeSetup]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[MigrateFee_SL_1_AllFeeSetup]
	
AS
BEGIN
	
	SET NOCOUNT ON;

	DECLARE @tmpFreedomFighter table (
	ID INT IDENTITY(1,1),
	programid int, 
	Batchid int,
	PerCreditAmount float,
	AdmissionFormFee float,
	AdmissionFee float,
	LibraryandLaboratoryFee float,
	ExtraCurricularActivitiesFee float,
	IDCautionFee float,
	TrimesterFee float,
	FineForEnteringUiuWithoutID float,
	LateFee float
	 );

	INSERT INTO @tmpFreedomFighter SELECT	tbl.*  FROM 
	(SELECT	
	p.ProgramID, 
	ac.AcademicCalenderID as BatchId, 
	af.PerCreditAmount,
	af.AdmissionFormFee,
	af.AdmissionFee,
	af.LibraryandLaboratoryFee,
	af.ExtraCurricularActivitiesFee,
	af.IDCautionFee,
	af.TrimesterFee,
	af.FineForEnteringUiuWithoutID,
	af.LateFee 
	
	FROM _AllFees as af 
	inner join UIUEMS_CC_Program as p on p.Code = af.Program
	inner join UIUEMS_CC_AcademicCalender as ac on ac.BatchCode = af.Batch) as tbl order by tbl.ProgramID, tbl.BatchId

	--select * from @tmpFreedomFighter

	declare 
	@i int, 
	@Count int,
	@dt datetime,
	@FeeSetupID int, 
	
	@programid int, 
	@Batchid int,
	@PerCreditAmount float,
	@AdmissionFormFee float,
	@AdmissionFee float,
	@LibraryandLaboratoryFee float,
	@ExtraCurricularActivitiesFee float,
	@IDCautionFee float,
	@TrimesterFee float,
	@FineForEnteringUiuWithoutID float,
	@LateFee float

	set @i = 1;
	set @Count = 0;
	set @dt = getdate();

	while @i <= (select max(ID) from @tmpFreedomFighter)
	BEGIN 
			SELECT  
			@programid = programid, 
			@Batchid = Batchid,
			@PerCreditAmount = PerCreditAmount,
			@AdmissionFormFee = AdmissionFormFee,
			@AdmissionFee = AdmissionFee,
			@LibraryandLaboratoryFee = LibraryandLaboratoryFee,
			@ExtraCurricularActivitiesFee = ExtraCurricularActivitiesFee,
			@IDCautionFee = IDCautionFee,
			@TrimesterFee = TrimesterFee,
			@FineForEnteringUiuWithoutID = FineForEnteringUiuWithoutID,
			@LateFee = LateFee
			FROM @tmpFreedomFighter WHERE ID = @i;	

			IF NOT EXISTS(select * from UIUEMS_BL_FeeSetup where ProgramID=@programid and AcaCalID = @Batchid)
				BEGIN
					
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 12, @PerCreditAmount, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 23, @AdmissionFormFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 10, @AdmissionFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 13, @LibraryandLaboratoryFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 14, @ExtraCurricularActivitiesFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 24, @IDCautionFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 25, @TrimesterFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 26, @FineForEnteringUiuWithoutID, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupInsert @FeeSetupID output, @Batchid,	@ProgramID, 27, @LateFee, -1, @dt, -1, @dt;

				END
			ELSE
				BEGIN
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 12, @PerCreditAmount, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 23, @AdmissionFormFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 10, @AdmissionFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 13, @LibraryandLaboratoryFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 14, @ExtraCurricularActivitiesFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 24, @IDCautionFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 25, @TrimesterFee, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 26, @FineForEnteringUiuWithoutID, -1, @dt, -1, @dt;
					EXEC dbo.UIUEMS_BL_FeeSetupUpdate @FeeSetupID , @Batchid,	@ProgramID, 27, @LateFee, -1, @dt, -1, @dt;
				END			

			set @i = @i + 1;
	END
END



GO
/****** Object:  StoredProcedure [dbo].[Migration_AcaCalSection]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_AcaCalSection]
(
	@Semester nvarchar(255) = NULL
)
As
Begin
	Declare @AcaCalId Int, @CurrentDate Datetime;
	Select @AcaCalId = AcademicCalenderID From [dbo].UIUEMS_CC_AcademicCalender Where BatchCode = @Semester;
	Set @CurrentDate = GetDate();

	Declare CourseList Cursor
	For
	Select Distinct CourseID From CourseRegistered Where Semester = @Semester;

	Declare @CourseName nvarchar(255);
	Open CourseList
	Fetch Next From CourseList Into @CourseName;
	while @@FETCH_STATUS = 0
	Begin
		Declare @CourseId Int, @VersionId Int, @ProgramCode varchar(50), @ProgramId Int;
		Select @CourseId = CourseID, @VersionId = VersionID From UIUEMS_CC_Course Where VersionCode = @CourseName;

		Select @ProgramCode = SUBSTRING(StudentID, 1, 3) From CourseRegistered Where CourseID = @CourseName and Semester = @Semester;
		Select @ProgramId = ProgramID From UIUEMS_CC_Program Where Code = @ProgramCode;

		Declare SectionList Cursor
		For
		Select Distinct GroupID From CourseRegistered Where Semester = @Semester and CourseID = @CourseName;

		Declare @SectionName nvarchar(255);
		Open SectionList
		Fetch Next From SectionList Into @SectionName;
		while @@FETCH_STATUS = 0
		Begin
			Declare @AcaCal_SectionID Int;
			
			EXEC [dbo].[UIUEMS_CC_AcademicCalenderSectionInsert] @AcaCal_SectionID Output, @AcademicCalenderID = @AcaCalId, @CourseID = @CourseId, @VersionID = @VersionId, @SectionName = @SectionName, @Capacity = 0, @RoomInfoOneID = 1, @DayOne = 1, @TimeSlotPlanOneID = 1, @TeacherOneID = 1, @DeptID = 1, @ProgramID = @ProgramId,  @CreatedBy = 99, @CreatedDate = @CurrentDate;

			Set @SectionName = NULL;
			Fetch Next From SectionList Into @SectionName;
		End
		close SectionList;
		deallocate SectionList;

		Set @CourseName = NULL;
		Fetch Next From CourseList Into @CourseName;
	End
	close CourseList;
	deallocate CourseList;
End



GO
/****** Object:  StoredProcedure [dbo].[Migration_AcaCalSection_Bulk]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_AcaCalSection_Bulk]
(
	@Semester nvarchar(255) = NULL
)
As
Begin
	Declare @AcaCalId Int, @CurrentDate Datetime;
	Select @AcaCalId = AcademicCalenderID From [dbo].UIUEMS_CC_AcademicCalender Where BatchCode = @Semester;
	Set @CurrentDate = GetDate();

	Declare CourseList Cursor
	For
	Select Distinct CourseID From CourseRegistered Where Semester = @Semester;

	Declare @CourseName nvarchar(255);
	Open CourseList
	Fetch Next From CourseList Into @CourseName;
	while @@FETCH_STATUS = 0
	Begin
		Declare @CourseId Int, @VersionId Int, @ProgramCode varchar(50), @ProgramId Int;
		Select @CourseId = CourseID, @VersionId = VersionID From UIUEMS_CC_Course Where VersionCode = @CourseName;

		Select @ProgramCode = SUBSTRING(StudentID, 1, 3) From CourseRegistered Where CourseID = @CourseName and Semester = @Semester;
		Select @ProgramId = ProgramID From UIUEMS_CC_Program Where Code = @ProgramCode;

		Declare SectionList Cursor
		For
		Select Distinct GroupID From CourseRegistered Where Semester = @Semester and CourseID = @CourseName;

		Declare @SectionName nvarchar(255);
		Open SectionList
		Fetch Next From SectionList Into @SectionName;
		while @@FETCH_STATUS = 0
		Begin
			Declare @AcaCal_SectionID Int;
			
			EXEC [dbo].[UIUEMS_CC_AcademicCalenderSectionInsert] @AcaCal_SectionID Output, @AcademicCalenderID = @AcaCalId, @CourseID = @CourseId, @VersionID = @VersionId, @SectionName = @SectionName, @Capacity = 0, @RoomInfoOneID = 1, @DayOne = 1, @TimeSlotPlanOneID = 1, @TeacherOneID = 1, @DeptID = 1, @ProgramID = @ProgramId,  @CreatedBy = 99, @CreatedDate = @CurrentDate;

			Set @SectionName = NULL;
			Fetch Next From SectionList Into @SectionName;
		End
		close SectionList;
		deallocate SectionList;

		Set @CourseName = NULL;
		Fetch Next From CourseList Into @CourseName;
	End
	close CourseList;
	deallocate CourseList;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_Course]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_Course]

As
Begin
	declare TempCourseContent cursor
	For
	Select Distinct Replace(Replace(CourseId, ' ', ''), '.', '') From CourseContent;
	
	declare @FlagCourseId nvarchar(255);
	open TempCourseContent
	fetch next from TempCourseContent into @FlagCourseId
	
	declare @RealCourseID int;
	set @RealCourseID = 1;
	while @@FETCH_STATUS = 0
	BEGIN
		declare @formalName varchar(255), @formalNameSize int;
		select @formalNameSize = min(len(CourseId)) from CourseContent where Replace(Replace(CourseId, ' ', ''), '.', '') = @FlagCourseId;
		select @formalName = CourseId from CourseContent where len(CourseId) = @formalNameSize and Replace(Replace(CourseId, ' ', ''), '.', '') = @FlagCourseId;
	
		declare courseList cursor
		For	
		select distinct CourseName, CourseId, Credit from CourseContent where Replace(Replace(CourseId, ' ', ''), '.', '') = @FlagCourseId
		
		declare @TempCourseName nvarchar(255), @TempCourseId nvarchar(255), @TempCredit float;
		open courseList
		fetch next from courseList into @TempCourseName, @TempCourseId, @TempCredit;
		
		declare @versionNo int, @CurrentDate Datetime;
		Set @versionNo = 1;
		Set @CurrentDate = GetDate();
		while @@FETCH_STATUS = 0
		BEGIN
			EXEC [dbo].[UIUEMS_CC_Course_Insert] @CourseID = @RealCourseID, @VersionID = @versionNo, @FormalCode = @formalName, @VersionCode = @TempCourseId, @Title = @TempCourseName, @Credits = @TempCredit, @CreatedBy = 99, @CreatedDate = @CurrentDate;
			
			set @versionNo = @versionNo + 1;
			fetch next from courseList into @TempCourseName, @TempCourseId, @TempCredit;
		END
		close courseList
		deallocate courseList
		
		fetch next from TempCourseContent into @FlagCourseId
		set @RealCourseID = @RealCourseID + 1;
	END
	close TempCourseContent;
	deallocate TempCourseContent;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_CourseHistory]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-21 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_CourseHistory]

As
Begin
	declare VP_courseHistory cursor
	For
	Select StudentID, CourseID, Semester, Grade, TransCredit, TransCreditPoint, TransHoursCompleted, GroupID, Retake, IsSelfStudy, ExcludeInWaiver from CourseRegistered;
	
	declare	@hStudentID nvarchar(255), @hCourseID nvarchar(255), @hSemester nvarchar(255), @hGrade nvarchar(255), @hTransCredit float, @hTransCreditPoint nvarchar(255), @hTransHoursCompleted nvarchar(255), @hGroupID nvarchar(255), @hRetake float, @hIsSelfStudy nvarchar(255), @hExcludeInWaiver nvarchar(255);
	
	open VP_courseHistory
	fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	while @@FETCH_STATUS = 0
	BEGIN
		Declare @StudentID Int, @RetakeNo Int, @ObtainedGPA numeric(18, 2), @ObtainedGrade varchar(2), @GradeId Int, @CourseStatusID Int, @AcaCalID Int, @CourseID Int, @VersionID Int, @CourseCredit numeric(18, 2), @IsConsiderGPA Bit, @CreatedBy Int, @CreatedDate DateTime;
		Set @CourseStatusID = NULL;
		Set @IsConsiderGPA = 'False';
		Set @StudentID = NULL;Set @RetakeNo = NULL;Set @ObtainedGPA = NULL;Set @ObtainedGrade = NULL;Set @GradeId = NULL;Set @CourseStatusID = NULL;Set @AcaCalID = NULL;Set @CourseID = NULL;Set @VersionID = NULL;Set @CourseCredit = NULL;Set @IsConsiderGPA = NULL;Set @CreatedBy = NULL;Set @CreatedDate = NULL;
		--StudentId
		Select @StudentID = StudentID From [dbo].[UIUEMS_ER_Student] Where Roll = @hStudentID;
		
		Set @RetakeNo = @hRetake;
		
		Select @GradeId = GradeId, @ObtainedGrade = Grade, @ObtainedGPA = GradePoint From [dbo].[UIUEMS_CC_GradeDetails] Where Grade = @hGrade;
		If @GradeId is NULL
		Begin
			Select @CourseStatusID = CourseStatusID From [dbo].[UIUEMS_ER_CourseStatus] Where Code = @hGrade;
		End
		If @CourseStatusID is NULL
		Begin
			If len(@hGrade) > 2
			Begin
				Set @CourseStatusID = 7;
			End	
		End
		
		Select @AcaCalID = AcademicCalenderID From [dbo].[UIUEMS_CC_AcademicCalender] Where BatchCode = @hSemester;
		
		Select @CourseID = CourseID, @VersionID = VersionID, @CourseCredit = Credits  from [dbo].[UIUEMS_CC_Course] where VersionCode = @hCourseID;
		
		If @hRetake = 1
		Begin
			Set @IsConsiderGPA = 'True';
		End
		Set @CreatedBy = 1;
		Set @CreatedDate = GetDate();
		
		Declare @CourseHistoryId int;
		
		EXEC [dbo].[Student_CourseHistory_Insert] @CourseHistoryId output, @StudentID=@StudentID, @RetakeNo=@RetakeNo, @ObtainedGPA=@ObtainedGPA, @ObtainedGrade=@ObtainedGrade, @GradeId=@GradeId, @CourseStatusID=@CourseStatusID, @AcaCalID=@AcaCalID, @CourseID=@CourseID, @VersionID=@VersionID, @CourseCredit=@CourseCredit, @IsConsiderGPA=@IsConsiderGPA;		
		
		fetch next from VP_courseHistory into @hStudentID, @hCourseID, @hSemester, @hGrade, @hTransCredit, @hTransCreditPoint, @hTransHoursCompleted, @hGroupID, @hRetake, @hIsSelfStudy, @hExcludeInWaiver
	END
	close VP_courseHistory
	deallocate VP_courseHistory
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_CourseWaiver]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_CourseWaiver]

As
Begin
	Declare pv_CourseWaiver Cursor
	For
	Select Distinct StudentID, University From CourseTransferredWeived;

	Declare @pv_StudentID nvarchar(MAX), @pv_University nvarchar(MAX);
	Open pv_CourseWaiver
	Fetch Next From pv_CourseWaiver Into @pv_StudentID, @pv_University;
	While @@FETCH_STATUS = 0
	Begin
	
		-- Start Insert Into UIUEMS_ER_CourseWavTransfr
		Declare @CourseWavTransfrID Int, @StudentID Int, @UniversityName varchar(50), @CourseStatusID Int;
		Set @StudentID = NULL;
		Select @StudentID = StudentID From UIUEMS_ER_Student Where Roll = @pv_StudentID;
		--	@@StudentID	
		If @StudentID is not null
		Begin
			If Len(@pv_University) > 0
			Begin
				Set @UniversityName = @pv_University;
				Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Tr';
			End
			Else If Len(@pv_University) is NULL
			Begin
				Set @UniversityName = NULL;
				Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Wv';
			End
			-- @UniversityName
			-- @CourseStatusID
			
			Declare @CreatedDate DateTime;
			Set @CreatedDate = GetDate();
			EXEC [dbo].[UIUEMS_ER_CourseWavTransfr_Insert] @CourseWavTransfrID output, @StudentID=@StudentID, @UniversityName=@UniversityName, @CourseStatusID=@CourseStatusID, @CreatedBy = 99, @CreatedDate = @CreatedDate;
			-- End Insert Into UIUEMS_ER_CourseWavTransfr
			
			-- Start Value Plus Course List For Distinct Student, University
			if Len(@pv_University) > 0
			Begin
				Declare pv_CourseList Cursor
				For
				Select StudentID, CourseID, Credit, Retake From CourseTransferredWeived Where StudentID = @pv_StudentID and University = @pv_University;
			End
			Else
			Begin
				Declare pv_CourseList Cursor
				For
				Select StudentID, CourseID, Credit, Retake From CourseTransferredWeived Where StudentID = @pv_StudentID and University Is NULL;
			End
			
			Declare @tw_StudentID nvarchar(MAX), @tw_CourseID nvarchar(MAX), @tw_Credit float, @tw_Retake float;
			Open pv_CourseList
			Fetch Next From pv_CourseList Into @tw_StudentID, @tw_CourseID, @tw_Credit, @tw_Retake;
			While @@FETCH_STATUS = 0
			Begin
				Declare @CourseID Int, @VersionID Int, @CourseCredit numeric(18, 2), @CourseHistoryId int;
				Select @CourseID = CourseID, @VersionID = VersionID, @CourseCredit = Credits  from [dbo].[UIUEMS_CC_Course] where VersionCode = @tw_CourseID;
				
				EXEC [dbo].[Student_CourseHistory_Insert] @CourseHistoryId output, @StudentID=@StudentID, @RetakeNo=@tw_Retake, @CourseStatusID=@CourseStatusID, @CourseID=@CourseID, @VersionID=@VersionID, @CourseCredit=@tw_Credit, @IsConsiderGPA='False', @CourseWavTransfrID = @CourseWavTransfrID;
				
				Fetch Next From pv_CourseList Into @tw_StudentID, @tw_CourseID, @tw_Credit, @tw_Retake;
				
			End
			close pv_CourseList
			deallocate pv_CourseList
		End
		-- End Value Plus Course List For Distinct Student, University
		set @pv_StudentID = NULL;
		set @pv_University = NULL;
		set @StudentID = NULL;
		Fetch Next From pv_CourseWaiver Into @pv_StudentID, @pv_University;
	End
	close pv_CourseWaiver
	deallocate pv_CourseWaiver
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_CourseWaiver_Semester]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-10-01 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_CourseWaiver_Semester]

As
Begin
	Declare pv_CourseWaiver Cursor
	For
	Select Distinct StudentID, University From CourseTransferredWeived;

	Declare @pv_StudentID nvarchar(MAX), @pv_University nvarchar(MAX);
	Open pv_CourseWaiver
	Fetch Next From pv_CourseWaiver Into @pv_StudentID, @pv_University;
	While @@FETCH_STATUS = 0
	Begin
	
		-- Start Insert Into UIUEMS_ER_CourseWavTransfr
		Declare @CourseWavTransfrID Int, @StudentID Int, @UniversityName varchar(50), @CourseStatusID Int;
		Set @StudentID = NULL;
		Select @StudentID = StudentID From UIUEMS_ER_Student Where Roll = @pv_StudentID;
		--	@@StudentID	
		If @StudentID is not null
		Begin
			Declare @CourseWavExist int;
			Set @CourseWavExist = 0;

			If Len(@pv_University) > 0
			Begin
				Set @UniversityName = @pv_University;
				Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Tr';
				Select @CourseWavExist = Count(*) From UIUEMS_ER_CourseWavTransfr Where StudentID = @StudentID and UniversityName = @UniversityName;
				if @CourseWavExist > 0
					Select @CourseWavTransfrID = CourseWavTransfrID From UIUEMS_ER_CourseWavTransfr Where StudentID = @StudentID and UniversityName = @UniversityName;
					
			End
			Else If Len(@pv_University) is NULL
			Begin
				Set @UniversityName = NULL;
				Select @CourseStatusID = CourseStatusID From UIUEMS_ER_CourseStatus Where Code = 'Wv';
				Select @CourseWavExist = Count(*) From UIUEMS_ER_CourseWavTransfr Where StudentID = @StudentID and UniversityName Is NULL;
				if @CourseWavExist > 0
					Select @CourseWavTransfrID = CourseWavTransfrID From UIUEMS_ER_CourseWavTransfr Where StudentID = @StudentID and UniversityName Is NULL;
			End
			-- @UniversityName
			-- @CourseStatusID
			
			Declare @CreatedDate DateTime;
			Set @CreatedDate = GetDate();
			
			if @CourseWavExist = 0
			Begin
				EXEC [dbo].[UIUEMS_ER_CourseWavTransfr_Insert] @CourseWavTransfrID output, @StudentID=@StudentID, @UniversityName=@UniversityName, @CourseStatusID=@CourseStatusID, @CreatedBy = 99, @CreatedDate = @CreatedDate;
				print(@pv_StudentID);
			End
			-- End Insert Into UIUEMS_ER_CourseWavTransfr
			
			-- Start Value Plus Course List For Distinct Student, University
			if Len(@pv_University) > 0
			Begin
				Declare pv_CourseList Cursor
				For
				Select StudentID, CourseID, Credit, Retake From CourseTransferredWeived Where StudentID = @pv_StudentID and University = @pv_University;
			End
			Else
			Begin
				Declare pv_CourseList Cursor
				For
				Select StudentID, CourseID, Credit, Retake From CourseTransferredWeived Where StudentID = @pv_StudentID and University Is NULL;
			End
			
			Declare @tw_StudentID nvarchar(MAX), @tw_CourseID nvarchar(MAX), @tw_Credit float, @tw_Retake float;
			Open pv_CourseList
			Fetch Next From pv_CourseList Into @tw_StudentID, @tw_CourseID, @tw_Credit, @tw_Retake;
			While @@FETCH_STATUS = 0
			Begin
				Declare @CourseID Int, @VersionID Int, @CourseCredit numeric(18, 2), @CourseHistoryId int;
				Select @CourseID = CourseID, @VersionID = VersionID, @CourseCredit = Credits  from [dbo].[UIUEMS_CC_Course] where VersionCode = @tw_CourseID;
				
				Declare @CourseHistoryExist int;
				Set @CourseHistoryExist = 0;

				Select @CourseHistoryExist = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @StudentID and CourseID = @CourseID and VersionID = @VersionID;
				if @pv_StudentID = '011082011'
				begin
					print(@CourseHistoryExist);
					print(@CourseID);
					print(@VersionID);
				end
				if @CourseHistoryExist = 0
				Begin
					EXEC [dbo].[Student_CourseHistory_Insert] @CourseHistoryId output, @StudentID=@StudentID, @RetakeNo=@tw_Retake, @CourseStatusID=@CourseStatusID, @CourseID=@CourseID, @VersionID=@VersionID, @CourseCredit=@tw_Credit, @IsConsiderGPA='False', @CourseWavTransfrID = @CourseWavTransfrID;
					print(@pv_StudentID);
				End
				Fetch Next From pv_CourseList Into @tw_StudentID, @tw_CourseID, @tw_Credit, @tw_Retake;
				
			End
			close pv_CourseList
			deallocate pv_CourseList
		End
		-- End Value Plus Course List For Distinct Student, University
		set @pv_StudentID = NULL;
		set @pv_University = NULL;
		set @StudentID = NULL;
		Fetch Next From pv_CourseWaiver Into @pv_StudentID, @pv_University;
	End --Student and University Fetch
	close pv_CourseWaiver
	deallocate pv_CourseWaiver
End



GO
/****** Object:  StoredProcedure [dbo].[Migration_GPA_CGPA_Calc]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-22 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_GPA_CGPA_Calc]

AS
BEGIN
	declare AllStudentID cursor
	For
	Select Distinct StudentID From UIUEMS_CC_Student_CourseHistory;
	
	Open AllStudentID
	Declare @tempStudentID Int;
	Fetch Next From AllStudentID Into @tempStudentID;
	While @@FETCH_STATUS = 0
	Begin
		Declare @minAcaCalID Int, @maxAcaCalID Int;
		Select @minAcaCalID = Min(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		Select @maxAcaCalID = Max(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		
		Declare @i Int, @previousCredit Money, @previousCGPA Money;
		Set @previousCredit = 0;
		Set @previousCGPA = 0;
		Set @i = @minAcaCalID;
		While @i <= @maxAcaCalID
		Begin
			Declare @flagFoundSemester Int;
			Set @flagFoundSemester = 0;
			Select @flagFoundSemester = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i;
			
			If @flagFoundSemester > 0
			Begin
				Declare @tempCredit Numeric(3,1), @tempGPAPlusCredit Money, @tempGPA Money, @TempCGPA Money;
				Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
				
				Set @tempGPA = @tempGPAPlusCredit / @tempCredit;
				Set @TempCGPA = (@previousCGPA*@previousCredit + @tempGPAPlusCredit) / (@tempCredit + @previousCredit);
				Set @previousCredit += @tempCredit;
				Set @previousCGPA = @TempCGPA;
				
				Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
				Values (@i, @tempStudentID, @tempCredit, @TempCGPA, @tempGPA, 1, GetDate());
			End
			
			Set @i += 1;
		End
		
		Fetch Next From AllStudentID Into @tempStudentID;
	End
	Close AllStudentID
	Deallocate AllStudentID
END


GO
/****** Object:  StoredProcedure [dbo].[Migration_GPA_CGPA_Calc_Semester]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-11-19 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_GPA_CGPA_Calc_Semester]
(	@Semester nvarchar(3) = NULL	)

AS
BEGIN
	--Declare Area
	Declare @mAcaCalId Int;
	Declare @tempStudentID Int
	--Declare Area

	--Value Load
	Select @mAcaCalId = AcademicCalenderID From UIUEMS_CC_AcademicCalender Where BatchCode = @Semester;
	--Value Load
	
	declare AllStudentID cursor
	For
	Select Distinct c.StudentID	From UIUEMS_CC_Student_CourseHistory c, dbo.UIUEMS_ER_Student s Where c.StudentID = s.StudentID and AcaCalID = @mAcaCalId;
	
	Open AllStudentID	
	Fetch Next From AllStudentID Into @tempStudentID;
	While @@FETCH_STATUS = 0
	Begin
		Delete From UIUEMS_ER_Student_ACUDetail Where StudentID = @tempStudentID and StdAcademicCalenderID = @mAcaCalId;
		Declare @minAcaCalID Int, @maxAcaCalID Int;
		--Select @minAcaCalID = Min(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		--Select @maxAcaCalID = Max(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		

		Declare @flagFoundSemester Int;
		Set @flagFoundSemester = 0;
		Select @flagFoundSemester = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @mAcaCalId And ObtainedGPA Is Not Null and CourseCredit is not null;
			
		If @flagFoundSemester > 0
		Begin
			Declare @tempCredit Numeric(4,1), @tempCreditTemp numeric(3,1), @tempGPAPlusCredit Numeric(14,2), @tempGPA Numeric(18,2), @TempCGPA Numeric(18,2);
			Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCreditTemp = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @mAcaCalId And ObtainedGPA Is Not Null and CourseCredit is not null;
			--IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
			Set @tempGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCreditTemp);
				
			--Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID < (@i + 1) And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null and CourseCredit is not null;
			--Set @TempCGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCredit);
			Declare @Trimester Int, @StdId Int;
			Set @Trimester = @mAcaCalId; Set @StdId = @tempStudentID;
			set @tempCGPA = (select [dbo].[CGPACalculation](@Trimester, @StdId));
				
			Declare @FlagFound Int;
			Set @FlagFound = 0;
			Select @FlagFound = Count(*) From UIUEMS_ER_Student_ACUDetail Where StdAcademicCalenderID = @mAcaCalId and StudentID = @tempStudentID;
				
			If @FlagFound = 0
			Begin
				Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
				Values (@mAcaCalId, @tempStudentID, @tempCreditTemp, @TempCGPA, @tempGPA, 1, GetDate());
			End
			Else If @FlagFound > 0
			Begin
				Update UIUEMS_ER_Student_ACUDetail Set CGPA = @TempCGPA, GPA = @tempGPA Where StdAcademicCalenderID = @mAcaCalId and StudentID = @tempStudentID;
			End
			--Set @tempCredit = 1; Set @TempCGPA = 0; Set @tempGPA = 0;
		End

		
		Fetch Next From AllStudentID Into @tempStudentID;
	End
	Close AllStudentID
	Deallocate AllStudentID
END


GO
/****** Object:  StoredProcedure [dbo].[Migration_GPA_CGPA_Calc_Varsion_2]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-22 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_GPA_CGPA_Calc_Varsion_2]

AS
BEGIN
	declare AllStudentID cursor
	For
	Select Distinct StudentID From UIUEMS_CC_Student_CourseHistory;
	
	Open AllStudentID
	Declare @tempStudentID Int;
	Fetch Next From AllStudentID Into @tempStudentID;
	While @@FETCH_STATUS = 0
	Begin
		Declare @minAcaCalID Int, @maxAcaCalID Int;
		Select @minAcaCalID = Min(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		Select @maxAcaCalID = Max(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		
		Declare @i Int;
		Set @i = @minAcaCalID;
		While @i <= @maxAcaCalID
		Begin
			Declare @flagFoundSemester Int;
			Set @flagFoundSemester = 0;
			Select @flagFoundSemester = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
			
			If @flagFoundSemester > 0
			Begin
				Declare @tempCredit Numeric(4,1), @tempCreditTemp numeric(3,1), @tempGPAPlusCredit Numeric(14,2), @tempGPA Numeric(18,2), @TempCGPA Numeric(18,2);
				Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCreditTemp = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
				--IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
				Set @tempGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCreditTemp);
				
				Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID < (@i + 1) And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null and CourseCredit is not null;
				Set @TempCGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCredit);
				
				Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
				Values (@i, @tempStudentID, @tempCreditTemp, @TempCGPA, @tempGPA, 1, GetDate());
				--Set @tempCredit = 1; Set @TempCGPA = 0; Set @tempGPA = 0;
			End
			
			Set @i += 1;
		End
		
		Fetch Next From AllStudentID Into @tempStudentID;
	End
	Close AllStudentID
	Deallocate AllStudentID
END


GO
/****** Object:  StoredProcedure [dbo].[Migration_GPA_CGPA_Calc_Varsion_Final]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-22 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_GPA_CGPA_Calc_Varsion_Final]

AS
BEGIN
	declare AllStudentID cursor
	For
	Select Distinct StudentID From UIUEMS_CC_Student_CourseHistory;
	
	Open AllStudentID
	Declare @tempStudentID Int;
	Fetch Next From AllStudentID Into @tempStudentID;
	While @@FETCH_STATUS = 0
	Begin
		--Delete From UIUEMS_ER_Student_ACUDetail Where StudentID = @tempStudentID;
		Declare @minAcaCalID Int, @maxAcaCalID Int;
		Select @minAcaCalID = Min(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		Select @maxAcaCalID = Max(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		
		Declare @i Int;
		Set @i = @minAcaCalID;
		While @i <= @maxAcaCalID
		Begin
			Declare @flagFoundSemester Int;
			Set @flagFoundSemester = 0;
			Select @flagFoundSemester = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
			
			If @flagFoundSemester > 0
			Begin
				Declare @tempCredit Numeric(4,1), @tempCreditTemp numeric(3,1), @tempGPAPlusCredit Numeric(14,2), @tempGPA Numeric(18,2), @TempCGPA Numeric(18,2);
				Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCreditTemp = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
				--IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
				Set @tempGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCreditTemp);
				
				--Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID < (@i + 1) And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null and CourseCredit is not null;
				--Set @TempCGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCredit);
				Declare @Trimester Int, @StdId Int;
				Set @Trimester = @i; Set @StdId = @tempStudentID;
				set @tempCGPA = (select [dbo].[CGPACalculation](@Trimester, @StdId));
		
				Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
				Values (@i, @tempStudentID, @tempCreditTemp, @TempCGPA, @tempGPA, 1, GetDate());
				--Set @tempCredit = 1; Set @TempCGPA = 0; Set @tempGPA = 0;
			End
			
			Set @i += 1;
		End
		
		Fetch Next From AllStudentID Into @tempStudentID;
	End
	Close AllStudentID
	Deallocate AllStudentID
END


GO
/****** Object:  StoredProcedure [dbo].[Migration_GPA_CGPA_Calc_Varsion_Final_Batch]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-22 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_GPA_CGPA_Calc_Varsion_Final_Batch]
(
	@BatchCode nvarchar(3) = NULL
)
AS
BEGIN
	declare AllStudentID cursor
	For
	Select Distinct c.StudentID	From UIUEMS_CC_Student_CourseHistory c, dbo.UIUEMS_ER_Student s Where c.StudentID = s.StudentID and Substring(Roll, 4, 3) = @BatchCode;
	
	Open AllStudentID
	Declare @tempStudentID Int;
	Fetch Next From AllStudentID Into @tempStudentID;
	While @@FETCH_STATUS = 0
	Begin
		Delete From UIUEMS_ER_Student_ACUDetail Where StudentID = @tempStudentID;
		Declare @minAcaCalID Int, @maxAcaCalID Int;
		Select @minAcaCalID = Min(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		Select @maxAcaCalID = Max(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID;
		
		Declare @i Int;
		Set @i = @minAcaCalID;
		While @i <= @maxAcaCalID
		Begin
			Declare @flagFoundSemester Int;
			Set @flagFoundSemester = 0;
			Select @flagFoundSemester = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
			
			If @flagFoundSemester > 0
			Begin
				Declare @tempCredit Numeric(4,1), @tempCreditTemp numeric(3,1), @tempGPAPlusCredit Numeric(14,2), @tempGPA Numeric(18,2), @TempCGPA Numeric(18,2);
				Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCreditTemp = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
				--IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
				Set @tempGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCreditTemp);
				
				--Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID < (@i + 1) And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null and CourseCredit is not null;
				--Set @TempCGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCredit);
				Declare @Trimester Int, @StdId Int;
				Set @Trimester = @i; Set @StdId = @tempStudentID;
				set @tempCGPA = (select [dbo].[CGPACalculation](@Trimester, @StdId));
				
				Declare @FlagFound Int;
				Set @FlagFound = 0;
				Select @FlagFound = Count(*) From UIUEMS_ER_Student_ACUDetail Where StdAcademicCalenderID = @i and StudentID = @tempStudentID;
				
				If @FlagFound = 0
				Begin
					Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
					Values (@i, @tempStudentID, @tempCreditTemp, @TempCGPA, @tempGPA, 1, GetDate());
				End
				Else If @FlagFound > 0
				Begin
					Update UIUEMS_ER_Student_ACUDetail Set CGPA = @TempCGPA, GPA = @tempGPA Where StdAcademicCalenderID = @i and StudentID = @tempStudentID;
				End
				--Set @tempCredit = 1; Set @TempCGPA = 0; Set @tempGPA = 0;
			End
			
			Set @i += 1;
		End
		
		Fetch Next From AllStudentID Into @tempStudentID;
	End
	Close AllStudentID
	Deallocate AllStudentID
END


GO
/****** Object:  StoredProcedure [dbo].[Migration_RoomInformation]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_RoomInformation]

As
Begin
	Declare roomInformation_List Cursor
	For
	Select RMRoomId, RMRoomNo, RMFloorNo, RMCapacity, RMDescription From [dbo].[RoomInformation];
	
	Declare @RMRoomId Int, @RMRoomNo nvarchar(255), @RMFloorNo nvarchar(255), @RMCapacity Int, @RMDescription nvarchar(255);
	Open roomInformation_List
	Fetch Next From roomInformation_List Into @RMRoomId, @RMRoomNo, @RMFloorNo, @RMCapacity, @RMDescription;
	
	while @@FETCH_STATUS = 0
	Begin
		Declare @RoomInfoID Int, @CurrentDate Datetime;
		Set @CurrentDate = GetDate();
		
		EXEC [dbo].[UIUEMS_CC_RoomInformation_Insert] @RoomInfoID Output, @RoomNumber = @RMRoomNo, @RoomFloorNo = @RMFloorNo, @Capacity = @RMCapacity, @Campus = @RMDescription, @CreatedBy = 99, @CreatedDate = @CurrentDate;
		
		Set @RMRoomId=NULL;Set @RMRoomNo=NULL;Set @RMFloorNo=NULL;Set @RMCapacity=NULL;Set @RMDescription=NULL;
		Fetch Next From roomInformation_List Into @RMRoomId, @RMRoomNo, @RMFloorNo, @RMCapacity, @RMDescription;
	End
	close roomInformation_List;
	deallocate roomInformation_List;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_Student_Batch]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_Student_Batch]

As
Begin
	Declare Student_List Cursor
	For
	Select StudentID, Roll From [dbo].[UIUEMS_ER_Student];
	
	Declare @StdId int, @StdRoll nvarchar(15);
	
	Open Student_List
	Fetch Next From Student_List Into @StdId, @StdRoll;
	
	while @@FETCH_STATUS = 0
	Begin
		Declare @AcaCalID int, @CurrentDate DateTime, @AdmissionID int;
		Set @CurrentDate = GetDate();
		Select @AcaCalID = AcademicCalenderID From [dbo].[UIUEMS_CC_AcademicCalender] Where BatchCode = SubString(@StdRoll, 4, 3);
		
		
		EXEC [dbo].[UIUEMS_ER_Admission_Insert] @AdmissionID Output, @StudentID = @StdId, @AdmissionCalenderID = @AcaCalID, @IsRule = 'True', @IsLastAdmission = 'False', @CreatedBy = 99, @CreatedDate = @CurrentDate;
		
		Set @StdId=NULL;Set @StdRoll=NULL;
		Fetch Next From Student_List Into @StdId, @StdRoll;
	End
	close Student_List;
	deallocate Student_List;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_StudentBasicInfo]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_StudentBasicInfo]

As
Begin
	Declare studentBasicInfo_List Cursor
	For
	Select StudentId, StudentFirstName, StudentMidName, StudentLastName, StudentSurName, StudentEmail, DateOfBirth, Sex, MaritalStatus, BloodGroup, Religion, Nationality, FatherName, MotherName, FatherProfession, MotherProfession From [dbo].[StudentBasicInfo];
	
	Declare @StudentId_ValuePlus nvarchar(255), @StudentFirstName nvarchar(255), @StudentMidName nvarchar(255), @StudentLastName nvarchar(255), @StudentSurName nvarchar(255), @StudentEmail nvarchar(255), @DateOfBirth DateTime, @Sex nvarchar(255), @MaritalStatus nvarchar(255), @BloodGroup nvarchar(255), @Religion nvarchar(255), @Nationality nvarchar(255), @FatherName nvarchar(255), @MotherName nvarchar(255), @FatherProfession nvarchar(255), @MotherProfession nvarchar(255);
	
	Open studentBasicInfo_List
	Fetch Next From studentBasicInfo_List Into @StudentId_ValuePlus, @StudentFirstName, @StudentMidName, @StudentLastName, @StudentSurName, @StudentEmail, @DateOfBirth, @Sex, @MaritalStatus, @BloodGroup, @Religion, @Nationality, @FatherName, @MotherName, @FatherProfession, @MotherProfession;
	
	while @@FETCH_STATUS = 0
	Begin
		Declare @FullName varchar(max);
		Set @FullName = '';
		if(Len(@StudentFirstName) > 0)
			Set @FullName = @FullName + @StudentFirstName + ' ';
		
		if(Len(@StudentMidName) > 0)
			Set @FullName = @FullName + @StudentMidName + ' ';
		
		if(Len(@StudentLastName) > 0)
			Set @FullName = @FullName + @StudentLastName + ' ';
		
		if(Len(@StudentSurName) > 0)
			Set @FullName = @FullName + @StudentSurName + ' ';

		Declare @Counter Int;
		Set @Counter = 0;
		Select @Counter = Count(*) From UIUEMS_ER_Student Where Roll = @StudentId_ValuePlus;
		If @Counter = 0
		Begin
			Declare @PersonID Int, @StudentID Int, @CurrentDate Datetime, @ProgramId Int, @ProgramCode varchar(3), @ValueId Int, @ValueSetId Int;
			Set @ProgramId = NULL; Set @ProgramCode = NULL; Set @ValueId = NULL; Set @ValueSetId = NULL;
			Select @ValueSetId = ValueSetID From UIUEMS_ER_ValueSet Where ValueSetName = 'PersonType'; Select @ValueId = ValueID From UIUEMS_ER_Value Where ValueSetID = @ValueSetId;
			Set @ProgramCode = SUBSTRING(@StudentId_ValuePlus, 1, 3); Select @ProgramId = ProgramID From UIUEMS_CC_Program Where Code = @ProgramCode;
			Set @CurrentDate = GetDate();
			
			EXEC [dbo].[UIUEMS_ER_PersonInsert] @PersonID Output, @FirstName = @FullName, @DOB = @DateOfBirth, @Gender = @Sex, @MatrialStatus = @MaritalStatus, @BloodGroup = @BloodGroup, @Religion = @Religion, @Nationality = @Nationality, @FatherName = @FatherName, @FatherProfession = @FatherProfession, @MotherName = @MotherName, @MotherProfession = @MotherProfession, @Email = @StudentEmail, @CreatedBy = 99, @CreatedDate = @CurrentDate, @TypeId = @ValueId;
			EXEC [dbo].[UIUEMS_ER_Student_Insert] @StudentID Output, @Roll = @StudentId_ValuePlus, @ProgramId = @ProgramId, @CreatedBy = 99, @CreatedDate = @CurrentDate, @PersonID = @PersonID;

			Print(@ProgramCode);
			Print(@StudentId_ValuePlus);
			Print(@ValueId);
			Print('-----------------');
		End

		Set @StudentId_ValuePlus = NULL;Set @StudentFirstName = NULL; Set @StudentMidName = NULL; Set @StudentLastName = NULL; Set @StudentSurName = NULL; Set @StudentEmail = NULL; Set @DateOfBirth = NULL; Set @Sex = NULL; Set @MaritalStatus = NULL; Set @BloodGroup = NULL; Set @Religion = NULL; Set @Nationality = NULL; Set @FatherName = NULL; Set @MotherName = NULL; Set @FatherProfession = NULL; Set @MotherProfession = NULL; Set @PersonID = NULL;
		Fetch Next From studentBasicInfo_List Into @StudentId_ValuePlus, @StudentFirstName, @StudentMidName, @StudentLastName, @StudentSurName, @StudentEmail, @DateOfBirth, @Sex, @MaritalStatus, @BloodGroup, @Religion, @Nationality, @FatherName, @MotherName, @FatherProfession, @MotherProfession;
		
	End
	close studentBasicInfo_List;
	deallocate studentBasicInfo_List;
End



GO
/****** Object:  StoredProcedure [dbo].[Migration_StudentBasicInfo_Update_Previous]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_StudentBasicInfo_Update_Previous]

As
Begin
	Declare studentBasicInfo_List Cursor
	For
	Select StudentId, StudentFirstName, StudentMidName, StudentLastName, StudentSurName, StudentEmail, DateOfBirth, Sex, MaritalStatus, BloodGroup, Religion, Nationality, FatherName, MotherName, FatherProfession, MotherProfession From [dbo].[StudentBasicInfo];
	
	Declare @StudentId_ValuePlus nvarchar(255), @StudentFirstName nvarchar(255), @StudentMidName nvarchar(255), @StudentLastName nvarchar(255), @StudentSurName nvarchar(255), @StudentEmail nvarchar(255), @DateOfBirth DateTime, @Sex nvarchar(255), @MaritalStatus nvarchar(255), @BloodGroup nvarchar(255), @Religion nvarchar(255), @Nationality nvarchar(255), @FatherName nvarchar(255), @MotherName nvarchar(255), @FatherProfession nvarchar(255), @MotherProfession nvarchar(255);
	
	Open studentBasicInfo_List
	Fetch Next From studentBasicInfo_List Into @StudentId_ValuePlus, @StudentFirstName, @StudentMidName, @StudentLastName, @StudentSurName, @StudentEmail, @DateOfBirth, @Sex, @MaritalStatus, @BloodGroup, @Religion, @Nationality, @FatherName, @MotherName, @FatherProfession, @MotherProfession;
	
	while @@FETCH_STATUS = 0
	Begin
		Declare @FullName varchar(max);
		Set @FullName = '';
		if(Len(@StudentFirstName) > 0)
			Set @FullName = @FullName + @StudentFirstName + ' ';
		
		if(Len(@StudentMidName) > 0)
			Set @FullName = @FullName + @StudentMidName + ' ';
		
		if(Len(@StudentLastName) > 0)
			Set @FullName = @FullName + @StudentLastName + ' ';
		
		if(Len(@StudentSurName) > 0)
			Set @FullName = @FullName + @StudentSurName + ' ';

		Declare @Counter Int;
		Set @Counter = 0;
		Select @Counter = Count(*) From UIUEMS_ER_Student Where Roll = @StudentId_ValuePlus;
		If @StudentId_ValuePlus = '114123026'
		Begin
			Print(@StudentId_ValuePlus);
			Print(@Counter);
			Print('------------------');
		End
		If @Counter = 0
		Begin
			Print(@StudentId_ValuePlus);
			Print('------------------');
			Declare @PersonID Int, @StudentID Int, @CurrentDate Datetime, @ProgramId Int, @ProgramCode varchar(3), @ValueId Int, @ValueSetId Int;
			Set @ProgramId = NULL; Set @ProgramCode = NULL; Set @ValueId = NULL; Set @ValueSetId = NULL;
			Select @ValueSetId = ValueSetID From UIUEMS_ER_ValueSet Where ValueSetName = 'PersonType'; Select @ValueId = ValueID From UIUEMS_ER_Value Where ValueSetID = @ValueSetId;
			Set @ProgramCode = SUBSTRING(@StudentId_ValuePlus, 1, 3); Select @ProgramId = ProgramID From UIUEMS_CC_Program Where Code = @ProgramCode;
			Set @CurrentDate = GetDate();
			
			EXEC [dbo].[UIUEMS_ER_PersonInsert] @PersonID Output, @FirstName = @FullName, @DOB = @DateOfBirth, @Gender = @Sex, @MatrialStatus = @MaritalStatus, @BloodGroup = @BloodGroup, @Religion = @Religion, @Nationality = @Nationality, @FatherName = @FatherName, @FatherProfession = @FatherProfession, @MotherName = @MotherName, @MotherProfession = @MotherProfession, @Email = @StudentEmail, @CreatedBy = 99, @CreatedDate = @CurrentDate, @TypeId = @ValueId;
			EXEC [dbo].[UIUEMS_ER_Student_Insert] @StudentID Output, @Roll = @StudentId_ValuePlus, @ProgramId = @ProgramId, @CreatedBy = 99, @CreatedDate = @CurrentDate, @PersonID = @PersonID;

			Print(@ProgramCode);
			Print(@StudentId_ValuePlus);
			Print(@ValueId);
			Print('-----------------');
		End

		Set @StudentId_ValuePlus = NULL;Set @StudentFirstName = NULL; Set @StudentMidName = NULL; Set @StudentLastName = NULL; Set @StudentSurName = NULL; Set @StudentEmail = NULL; Set @DateOfBirth = NULL; Set @Sex = NULL; Set @MaritalStatus = NULL; Set @BloodGroup = NULL; Set @Religion = NULL; Set @Nationality = NULL; Set @FatherName = NULL; Set @MotherName = NULL; Set @FatherProfession = NULL; Set @MotherProfession = NULL; Set @PersonID = NULL;
		Fetch Next From studentBasicInfo_List Into @StudentId_ValuePlus, @StudentFirstName, @StudentMidName, @StudentLastName, @StudentSurName, @StudentEmail, @DateOfBirth, @Sex, @MaritalStatus, @BloodGroup, @Religion, @Nationality, @FatherName, @MotherName, @FatherProfession, @MotherProfession;
		
	End
	close studentBasicInfo_List;
	deallocate studentBasicInfo_List;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_StudentBasicInfo_Update_StudentName]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_StudentBasicInfo_Update_StudentName]

As
Begin
	Declare studentBasicInfo_List Cursor
	For
	Select StudentId, StudentFirstName, StudentMidName, StudentLastName, StudentSurName, StudentEmail, DateOfBirth, Sex, MaritalStatus, BloodGroup, Religion, Nationality, FatherName, MotherName, FatherProfession, MotherProfession From [dbo].[StudentBasicInfo];
	
	Declare @StudentId_ValuePlus nvarchar(255), @StudentFirstName nvarchar(255), @StudentMidName nvarchar(255), @StudentLastName nvarchar(255), @StudentSurName nvarchar(255), @StudentEmail nvarchar(255), @DateOfBirth DateTime, @Sex nvarchar(255), @MaritalStatus nvarchar(255), @BloodGroup nvarchar(255), @Religion nvarchar(255), @Nationality nvarchar(255), @FatherName nvarchar(255), @MotherName nvarchar(255), @FatherProfession nvarchar(255), @MotherProfession nvarchar(255);
	
	Open studentBasicInfo_List
	Fetch Next From studentBasicInfo_List Into @StudentId_ValuePlus, @StudentFirstName, @StudentMidName, @StudentLastName, @StudentSurName, @StudentEmail, @DateOfBirth, @Sex, @MaritalStatus, @BloodGroup, @Religion, @Nationality, @FatherName, @MotherName, @FatherProfession, @MotherProfession;
		
	while @@FETCH_STATUS = 0
	Begin
		Declare @FullName varchar(max);
		Set @FullName = '';
		if(Len(@StudentFirstName) > 0)
			Set @FullName = @FullName + @StudentFirstName + ' ';
			
		if(Len(@StudentMidName) > 0)
			Set @FullName = @FullName + @StudentMidName + ' ';
			
		if(Len(@StudentLastName) > 0)
			Set @FullName = @FullName + @StudentLastName + ' ';
			
		if(Len(@StudentSurName) > 0)
			Set @FullName = @FullName + @StudentSurName + ' ';
		
		--Set @FullName = @StudentFirstName + ' ' + @StudentMidName + ' ' + @StudentLastName + ' ' + @StudentSurName;	
		
		Declare @PersonID Int, @StudentID Int, @CurrentDate Datetime;
		Set @CurrentDate = GetDate();
		
		update [dbo].[UIUEMS_ER_Person] Set FirstName = @FullName Where PersonID = (Select PersonId From [dbo].[UIUEMS_ER_Student] Where Roll = @StudentId_ValuePlus);
		print(@FullName);
		print(@StudentId_ValuePlus);
		Set @StudentId_ValuePlus = NULL;Set @StudentFirstName = NULL; Set @StudentMidName = NULL; Set @StudentLastName = NULL; Set @StudentSurName = NULL; Set @StudentEmail = NULL; Set @DateOfBirth = NULL; Set @Sex = NULL; Set @MaritalStatus = NULL; Set @BloodGroup = NULL; Set @Religion = NULL; Set @Nationality = NULL; Set @FatherName = NULL; Set @MotherName = NULL; Set @FatherProfession = NULL; Set @MotherProfession = NULL; Set @PersonID = NULL;
		Fetch Next From studentBasicInfo_List Into @StudentId_ValuePlus, @StudentFirstName, @StudentMidName, @StudentLastName, @StudentSurName, @StudentEmail, @DateOfBirth, @Sex, @MaritalStatus, @BloodGroup, @Religion, @Nationality, @FatherName, @MotherName, @FatherProfession, @MotherProfession;
	End
	close studentBasicInfo_List;
	deallocate studentBasicInfo_List;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_StudentCourseHistory_Update_AcaCalSection]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
--  [temp_MigrateSection] is a temporary table used here. it is deleted first and then populated and
-- finally [AcaCalSectionID] is transferred to [dbo].[UIUEMS_CC_Student_CourseHistory]
-- =============================================
CREATE PROCEDURE [dbo].[Migration_StudentCourseHistory_Update_AcaCalSection]
(
	@Semester nvarchar(255) = NULL
)
As
Begin
begin tran

CREATE TABLE [dbo].[temp_MigrateSection](
 [vpStudentID] [nvarchar](max) NULL,
 [vpCourseID] [nvarchar](max) NULL,
 [vpGroupID] [nvarchar](max) NULL,
 [CourseId] [int] NULL,
 [VersionId] [int] NULL,
 [StudentId] [int] NULL,
 [AcaCalSectionId] [int] NULL,
 [AcademicCalenderID] [int] NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

delete [dbo].[temp_MigrateSection]
INSERT INTO [dbo].[temp_MigrateSection]
           ([vpStudentID]
           ,[vpCourseID]
           ,[vpGroupID])
     (Select StudentID, CourseID, GroupID From CourseRegistered Where Semester = @Semester)

Declare @CourseId Int, @VersionId Int, @StudentId Int;

update temp_MigrateSection
set CourseID =
(Select distinct UIUEMS_CC_Course.CourseID
	         From UIUEMS_CC_Course
		     Where UIUEMS_CC_Course.VersionCode = temp_MigrateSection.vpCourseID)

update temp_MigrateSection
set VersionId =
(Select distinct UIUEMS_CC_Course.VersionId
	         From UIUEMS_CC_Course
		     Where UIUEMS_CC_Course.VersionCode = temp_MigrateSection.vpCourseID)

update temp_MigrateSection
set StudentId =
(Select StudentId
	         From UIUEMS_ER_Student
		     Where UIUEMS_ER_Student.Roll = temp_MigrateSection.vpStudentID)

update temp_MigrateSection
set AcademicCalenderID =
(Select AcademicCalenderID From [dbo].UIUEMS_CC_AcademicCalender Where BatchCode = @Semester)

update temp_MigrateSection
set AcaCalSectionId =
(Select AcaCal_SectionId From UIUEMS_CC_AcademicCalenderSection 
	Where UIUEMS_CC_AcademicCalenderSection.AcademicCalenderID = temp_MigrateSection.AcademicCalenderID 
	   and UIUEMS_CC_AcademicCalenderSection.CourseID = temp_MigrateSection.CourseID 
	   and UIUEMS_CC_AcademicCalenderSection.VersionID = temp_MigrateSection.VersionID
	   and UIUEMS_CC_AcademicCalenderSection.SectionName = temp_MigrateSection.vpGroupID)
	   
update [dbo].[UIUEMS_CC_Student_CourseHistory] 
set AcaCalSectionID =
(Select AcaCalSectionId From  temp_MigrateSection
	Where UIUEMS_CC_Student_CourseHistory.AcaCalID = temp_MigrateSection.AcademicCalenderID 
	   and UIUEMS_CC_Student_CourseHistory.CourseID = temp_MigrateSection.CourseID 
	   and UIUEMS_CC_Student_CourseHistory.VersionID = temp_MigrateSection.VersionID
	   and UIUEMS_CC_Student_CourseHistory.StudentID = temp_MigrateSection.StudentId)

Drop Table temp_MigrateSection;
commit tran
End



GO
/****** Object:  StoredProcedure [dbo].[Migration_StudentCourseHistory_Update_AcaCalSection_Temp]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_StudentCourseHistory_Update_AcaCalSection_Temp]
(
	@Semester nvarchar(255) = NULL
)
As
Begin
	Declare @AcaCalId Int;
	Select @AcaCalId = AcademicCalenderID From [dbo].UIUEMS_CC_AcademicCalender Where BatchCode = @Semester;

	Declare StudentList Cursor
	For
	Select StudentID, CourseID, GroupID From CourseRegistered Where Semester = @Semester;

	Declare @StudentRoll nvarchar(255), @CourseCode nvarchar(255), @GroupID nvarchar(255);
	Open StudentList
	Fetch Next From StudentList Into @StudentRoll, @CourseCode, @GroupID;
	while @@FETCH_STATUS = 0
	Begin
		Declare @CourseId Int, @VersionId Int, @StudentId Int, @AcaCalSectionId Int;
		Set @AcaCalSectionId = 0;Set @StudentId = 0;Set @CourseId = 0;Set @VersionId = 0;

		
		Select @StudentId = StudentID From UIUEMS_ER_Student Where [Roll] = @StudentRoll;
		
		If @StudentId != 0
		Begin
			Select @CourseId = CourseID, @VersionId = VersionID From UIUEMS_CC_Course Where VersionCode = @CourseCode;
			Select @AcaCalSectionId = AcaCal_SectionID From UIUEMS_CC_AcademicCalenderSection Where AcademicCalenderID = @AcaCalId and CourseID = @CourseId and VersionID = @VersionId and SectionName = @GroupID;

			Update UIUEMS_CC_Student_CourseHistory Set AcaCalSectionID = @AcaCalSectionId Where StudentID = @StudentId and AcaCalID = @AcaCalId and CourseID = @CourseId and VersionID = @VersionId;
		End

		Set @StudentRoll = '';Set @CourseCode = '';Set @GroupID = '';
		Fetch Next From StudentList Into @StudentRoll, @CourseCode, @GroupID;
	End
	close StudentList;
	deallocate StudentList;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_StudentCourseHistory_Update_AcaCalSection_VersionCode_Temp]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2014-01-16 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_StudentCourseHistory_Update_AcaCalSection_VersionCode_Temp]
(
	@Semester nvarchar(255) = NULL,
	@VersionCode nvarchar(255)
)
As
Begin
	Declare @AcaCalId Int;
	Select @AcaCalId = AcademicCalenderID From [dbo].UIUEMS_CC_AcademicCalender Where BatchCode = @Semester;

	Declare StudentList Cursor
	For
	Select StudentID, CourseID, GroupID From CourseRegistered Where Semester = @Semester and CourseID = @VersionCode;

	Declare @StudentRoll nvarchar(255), @CourseCode nvarchar(255), @GroupID nvarchar(255);
	Open StudentList
	Fetch Next From StudentList Into @StudentRoll, @CourseCode, @GroupID;
	while @@FETCH_STATUS = 0
	Begin
		Declare @CourseId Int, @VersionId Int, @StudentId Int, @AcaCalSectionId Int;
		Set @AcaCalSectionId = 0;Set @StudentId = 0;

		
		Select @StudentId = StudentID From UIUEMS_ER_Student Where [Roll] = @StudentRoll;
		
		If @StudentId != 0
		Begin
			Select @CourseId = CourseID, @VersionId = VersionID From UIUEMS_CC_Course Where VersionCode = @CourseCode;
			Select @AcaCalSectionId = AcaCal_SectionID From UIUEMS_CC_AcademicCalenderSection Where AcademicCalenderID = @AcaCalId and CourseID = @CourseId and VersionID = @VersionId and SectionName = @GroupID;

			Update UIUEMS_CC_Student_CourseHistory Set AcaCalSectionID = @AcaCalSectionId Where StudentID = @StudentId and AcaCalID = @AcaCalId and CourseID = @CourseId and VersionID = @VersionId;
		End

		Set @StudentRoll = NULL;Set @CourseCode = NULL;Set @GroupID = NULL;
		Fetch Next From StudentList Into @StudentRoll, @CourseCode, @GroupID;
	End
	close StudentList;
	deallocate StudentList;
End


GO
/****** Object:  StoredProcedure [dbo].[Migration_TeacherInfo]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-20 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Migration_TeacherInfo]

As
Begin
	Declare teacherInfo_List Cursor
	For
	Select TeacherName, TeacherId, AcademicBackground, Publish, Phone, Email, WebAddress, MaxNoTobeAdvised, UserID From [dbo].[TeacherInfo];
	
	Declare @TeacherName nvarchar(255), @Code nvarchar(255), @AcademicBackground nvarchar(255), @Publish nvarchar(255), @Phone nvarchar(255), @Email nvarchar(255), @WebAddress nvarchar(255), @MaxNoTobeAdvised float, @UserID nvarchar(255);
	Open teacherInfo_List
	Fetch Next From teacherInfo_List Into @TeacherName, @Code, @AcademicBackground, @Publish, @Phone, @Email, @WebAddress, @MaxNoTobeAdvised, @UserID;
	
	while @@FETCH_STATUS = 0
	Begin
		Declare @PersonID Int, @TeacherID Int, @CurrentDate Datetime, @Counter Int;
		Set @CurrentDate = GetDate(); Set @Counter = 0;
		Select @Counter = Count(*) From UIUEMS_CC_Employee Where Code = @Code;
		
		If @Counter = 0
		Begin
			EXEC [dbo].[UIUEMS_ER_Person_Insert] @PersonID Output, @FirstName = @TeacherName, @Phone = @Phone, @Email = @Email, @CreatedBy = 99, @CreatedDate = @CurrentDate;
			EXEC [dbo].[UIUEMS_CC_Employee_Insert] @TeacherID Output, @Code = @Code, @CreatedBy = 99, @CreatedDate = @CurrentDate, @PersonId = @PersonID;
		End

		Set @TeacherName=NULL;Set @Code=NULL;Set @AcademicBackground=NULL;Set @Publish=NULL;Set @Phone=NULL;Set @Email=NULL;Set @WebAddress=NULL;Set @MaxNoTobeAdvised=NULL;Set @UserID=NULL;
		Fetch Next From teacherInfo_List Into @TeacherName, @Code, @AcademicBackground, @Publish, @Phone, @Email, @WebAddress, @MaxNoTobeAdvised, @UserID;
	End
	close teacherInfo_List;
	deallocate teacherInfo_List;
End


GO
/****** Object:  StoredProcedure [dbo].[NodeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[NodeDeleteById]
(
@NodeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Node]
WHERE NodeID = @NodeID

END




GO
/****** Object:  StoredProcedure [dbo].[NodeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NodeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[NodeID],
[Name],
[IsLastLevel],
[MinCredit],
[MaxCredit],
[MinCourses],
[MaxCourses],
[IsActive],
[IsVirtual],
[IsBundle],
[IsAssociated],
[StartTrimesterID],
[OperatorID],
[OperandNodes],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_Node


END




GO
/****** Object:  StoredProcedure [dbo].[NodeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NodeGetById]
(
@NodeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[NodeID],
[Name],
[IsLastLevel],
[MinCredit],
[MaxCredit],
[MinCourses],
[MaxCourses],
[IsActive],
[IsVirtual],
[IsBundle],
[IsAssociated],
[StartTrimesterID],
[OperatorID],
[OperandNodes],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[IsMajor]

FROM       UIUEMS_CC_Node
WHERE     (NodeID = @NodeID)

END




GO
/****** Object:  StoredProcedure [dbo].[NodeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NodeInsert] 
(
@NodeID int  OUTPUT,
@Name varchar(150)  = NULL,
@IsLastLevel bit  = NULL,
@MinCredit numeric(18, 2) = NULL,
@MaxCredit numeric(18, 2) = NULL,
@MinCourses int = NULL,
@MaxCourses int = NULL,
@IsActive bit  = NULL,
@IsVirtual bit  = NULL,
@IsBundle bit  = NULL,
@IsAssociated bit  = NULL,
@StartTrimesterID int = NULL,
@OperatorID int = NULL,
@OperandNodes int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Node]
(
[NodeID],
[Name],
[IsLastLevel],
[MinCredit],
[MaxCredit],
[MinCourses],
[MaxCourses],
[IsActive],
[IsVirtual],
[IsBundle],
[IsAssociated],
[StartTrimesterID],
[OperatorID],
[OperandNodes],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@NodeID,
@Name,
@IsLastLevel,
@MinCredit,
@MaxCredit,
@MinCourses,
@MaxCourses,
@IsActive,
@IsVirtual,
@IsBundle,
@IsAssociated,
@StartTrimesterID,
@OperatorID,
@OperandNodes,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @NodeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[NodeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[NodeUpdate]
(
@NodeID int  = NULL,
@Name varchar(150)  = NULL,
@IsLastLevel bit  = NULL,
@MinCredit numeric(18, 2) = NULL,
@MaxCredit numeric(18, 2) = NULL,
@MinCourses int = NULL,
@MaxCourses int = NULL,
@IsActive bit  = NULL,
@IsVirtual bit  = NULL,
@IsBundle bit  = NULL,
@IsAssociated bit  = NULL,
@StartTrimesterID int = NULL,
@OperatorID int = NULL,
@OperandNodes int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Node]
   SET

[Name]	=	@Name,
[IsLastLevel]	=	@IsLastLevel,
[MinCredit]	=	@MinCredit,
[MaxCredit]	=	@MaxCredit,
[MinCourses]	=	@MinCourses,
[MaxCourses]	=	@MaxCourses,
[IsActive]	=	@IsActive,
[IsVirtual]	=	@IsVirtual,
[IsBundle]	=	@IsBundle,
[IsAssociated]	=	@IsAssociated,
[StartTrimesterID]	=	@StartTrimesterID,
[OperatorID]	=	@OperatorID,
[OperandNodes]	=	@OperandNodes,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE NodeID = @NodeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[OpenCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashraf>
-- Create date: <18 Sep, 2011 >
-- Description:	<auto open course.>
-- =============================================
CREATE PROCEDURE [dbo].[OpenCourse] 
-- Declare the input parameter(s) for this procedure
@StudentID int
	
AS
BEGIN
	
DECLARE 
		--This three variables will sotre the values for the cursor
		@AcademicCalenderID int, 
		@CourseID int, 
		@VersionID int

--This CURSOR will hold the information of the offered course of the current trimester
declare @Cursor CURSOR 

set @Cursor = CURSOR For
		SELECT AcademicCalenderID, CourseID, VersionID  
		from dbo.UIUEMS_CC_OfferedCourse
		where UIUEMS_CC_OfferedCourse.AcademicCalenderID 
						--This Subquery will get the current trimester's academic calendar ID
						= ( select AcademicCalenderID 
							from dbo.UIUEMS_CC_AcademicCalender 
							where IsCurrent = 1)
-- This section will set the IsAutoOpen of table Student_CalCourseProgNode to true
-- for the studetnID the procedure will get as input parameter
OPEN @Cursor
	fetch NEXT from @Cursor into  
			@AcademicCalenderID , 
			@CourseID , 
			@VersionID 

--initialy update all IsAutoOpen to false.
			update UIUEMS_CC_Student_CalCourseProgNode 
			set IsAutoOpen = 0 
			where StudentID = @StudentID

		while @@FETCH_STATUS = 0
			BEGIN
				update UIUEMS_CC_Student_CalCourseProgNode 
				set IsAutoOpen = 1 
				where StudentID = @StudentID
					and CourseID = @CourseID
					and VersionID = @VersionID


				fetch NEXT from @Cursor into  
						@AcademicCalenderID , 
						@CourseID , 
						@VersionID 
			END

CLOSE @Cursor
DEALLOCATE @Cursor

END




GO
/****** Object:  StoredProcedure [dbo].[PrepareRegistrationWorksheet]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PrepareRegistrationWorksheet] 
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@OpenCourse int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int,
@ReturnValue int OUTPUT

AS
BEGIN
SET NOCOUNT ON;

--STEP 1 START

DECLARE

--* CURSOR TreeCalendarDetail c1
	@c1_TreeCalendarDetailID int,
	@c1_TreeCalendarMasterID int,
--# CURSOR TreeCalendarDetail

--* CURSOR Cal_Course_Prog_Node c2
	@c2_CalCorProgNodeID int,
	@c2_TreeCalendarDetailID int ,
	@c2_OfferedByProgramID int,
	@c2_CourseID int ,
	@c2_VersionID int,
	@c2_Node_CourseID int,
	@c2_NodeID int,
	@c2_NodeLinkName varchar(100),
	@c2_Priority int,
	@c2_Credits decimal(18,2),
	@c2_IsMajorRelated bit,
--# CURSOR Cal_Course_Prog_Node

	@c3_CourseID int ,
	@c3_VersionID int,
	@c3_Node_CourseID int;


declare		@Cursor_TreeCalendarDetail CURSOR
declare		@Cursor_Cal_Course_Prog_Node CURSOR
declare		@Cursor_Node CURSOR


	--* CURSOR TreeCalendarDetail
	set @Cursor_TreeCalendarDetail = CURSOR FOR
	select TreeCalendarDetailID,TreeCalendarMasterID from dbo.UIUEMS_CC_TreeCalendarDetail
		where TreeCalendarMasterID = @TreeCalendarMasterID and TreeMasterID = @TreeMasterID

	OPEN @Cursor_TreeCalendarDetail
		fetch NEXT from @Cursor_TreeCalendarDetail into
				@c1_TreeCalendarDetailID,
				@c1_TreeCalendarMasterID  
				
		while @@FETCH_STATUS = 0
		BEGIN 
			--print( @c1_TreeCalendarDetailID);

					--* CURSOR Cal_Course_Prog_Node			
					set @Cursor_Cal_Course_Prog_Node = CURSOR FOR
					select 
						CalCorProgNodeID,
						TreeCalendarDetailID, 
						OfferedByProgramID, 
						CourseID, 
						VersionID, 
						Node_CourseID, 
						NodeID, 
						NodeLinkName, 
						[Priority], 
						Credits,
						IsMajorRelated
					from dbo.UIUEMS_CC_Cal_Course_Prog_Node
					where TreeCalendarDetailID  = @c1_TreeCalendarDetailID			
					
					 OPEN @Cursor_Cal_Course_Prog_Node
						fetch NEXT from @Cursor_Cal_Course_Prog_Node into
									@c2_CalCorProgNodeID ,
									@c2_TreeCalendarDetailID  ,
									@c2_OfferedByProgramID ,
									@c2_CourseID  ,
									@c2_VersionID ,
									@c2_Node_CourseID ,
									@c2_NodeID ,
									@c2_NodeLinkName ,
									@c2_Priority ,
									@c2_Credits ,
									@c2_IsMajorRelated 						
						
						while @@FETCH_STATUS = 0
						BEGIN 	
							if(@c2_CourseID is null)
								begin									
									DECLARE
										   @crs  CURSOR
											 EXEC AllCourseByNodeCursorParam @crs output, @c2_NodeID
											 
											 fetch NEXT from @crs into
												@c3_CourseID  ,
												@c3_VersionID ,
												@c3_Node_CourseID 
											 WHILE (@@FETCH_STATUS <> -1)
												BEGIN
												-- INSERT COURSE FROM NODE
												
												INSERT into [dbo].[RegistrationWorksheet]
														   ([StudentID]
														   ,[CalCourseProgNodeID]
														   ,[TreeCalendarDetailID]
														   ,[TreeCalendarMasterID]													   
														   ,[CourseID]
														   ,[VersionID]
														   ,[Credits]
														   ,[Node_CourseID]
														   ,[NodeID]													   
														   ,[NodeLinkName]													   
														   ,[Priority]
														   ,[IsMajorRelated]
														   ,[ProgramID]
														   ,[IsAutoOpen]
														   ,[DeptID]
														   ,[AcademicCalenderID]
														    
														  )
													 VALUES
														   (@StudentId
															,@c2_CalCorProgNodeID
															,@c2_TreeCalendarDetailID
															,@c1_TreeCalendarMasterID													  
															,@c3_CourseID  
															,@c3_VersionID 								
															,@c2_Credits
															,@c3_Node_CourseID 
															,@c2_NodeID 
															,@c2_NodeLinkName 
															,@c2_Priority
															,@c2_IsMajorRelated 
															,@c2_OfferedByProgramID 
															,'False'	
															,@DepartmentID
															,@AcademicCalenderID	
														   ) 
												
												 fetch NEXT from @crs into
												@c3_CourseID  ,
												@c3_VersionID ,
												@c3_Node_CourseID
												END;
																		 
										CLOSE @crs;
										DEALLOCATE @crs;
									
								end
							else
								begin
									-- Insert Course 
									
									INSERT into [dbo].[RegistrationWorksheet]
													   ([StudentID]
													   ,[CalCourseProgNodeID]
													   ,[TreeCalendarDetailID]
													   ,[TreeCalendarMasterID]													   
													   ,[CourseID]
													   ,[VersionID]
													   ,[Credits]
													   ,[Node_CourseID]
													   ,[NodeID]													   
													   ,[NodeLinkName]													   
													   ,[Priority]
													   ,[IsMajorRelated]
													   ,[ProgramID]
													   ,[IsAutoOpen]
													    
													  )
												 VALUES
													   (@StudentId
														,@c2_CalCorProgNodeID
														,@c2_TreeCalendarDetailID
														,@c1_TreeCalendarMasterID													  
														,@c2_CourseID  
														,@c2_VersionID
														,@c2_Credits
														,@c2_Node_CourseID 
														,@c2_NodeID 
														,@c2_NodeLinkName 
														,@c2_Priority
														,@c2_IsMajorRelated 
														,@c2_OfferedByProgramID 
														,0		
													   ) 
								end
								
							fetch NEXT from @Cursor_Cal_Course_Prog_Node into
									@c2_CalCorProgNodeID ,
									@c2_TreeCalendarDetailID  ,
									@c2_OfferedByProgramID ,
									@c2_CourseID  ,
									@c2_VersionID ,
									@c2_Node_CourseID ,
									@c2_NodeID ,
									@c2_NodeLinkName ,
									@c2_Priority ,
									@c2_Credits ,
									@c2_IsMajorRelated 		
						END	 
						--# CURSOR Cal_Course_Prog_Node

				fetch NEXT from @Cursor_TreeCalendarDetail into
								@c1_TreeCalendarDetailID,
								@c1_TreeCalendarMasterID 
		END
		--# CURSOR TreeCalendarDetail	
	
	
	CLOSE @Cursor_TreeCalendarDetail
	DEALLOCATE @Cursor_TreeCalendarDetail
	
--STEP 1 END
----------------------------------------------------------------------
--STEP 2 START [update Student Result History]

declare 
@c4_StudentId int,  
@c4_CourseId int, 
@c4_VersionId int, 
@c4_RetakeNo int,  
@c4_ObtainedGPA numeric(18,2), 
@c4_ObtainedGrade varchar(150), 
@c4_CourseStatusID int;

	DECLARE  @Cursor_Student_Result_History CURSOR
	set @Cursor_Student_Result_History = cursor for
		SELECT    
			StudentId,  
			CourseId, 
			VersionId, 
			RetakeNo,  
			ObtainedGPA, 
			ObtainedGrade, 
			CourseStatusID 
		FROM         UIUEMS_CC_Student_CourseHistory
		WHERE     (StudentID = @StudentId) and IsConsiderGPA = 1
		
	OPEN @Cursor_Student_Result_History
	fetch NEXT from @Cursor_Student_Result_History into
				@c4_StudentId,  
				@c4_CourseId, 
				@c4_VersionId, 
				@c4_RetakeNo,  
				@c4_ObtainedGPA, 
				@c4_ObtainedGrade, 
				@c4_CourseStatusID  
			 WHILE (@@FETCH_STATUS <> -1)
				BEGIN
					
					UPDATE  [dbo].[RegistrationWorksheet]
					   SET
						   [RetakeNo] = @c4_RetakeNo
						  ,[ObtainedGPA] = @c4_ObtainedGPA
						  ,[ObtainedGrade] =   @c4_ObtainedGrade   
						  ,[CourseStatusId] = @c4_CourseStatusID
						  ,[IsCompleted] =	CASE @c4_CourseStatusID
												WHEN 6 THEN 1
												ELSE 0
											END
					 WHERE [CourseID] = @c4_CourseId and
						   [VersionID] = @c4_VersionId and 
						   [StudentId] = @c4_StudentId
					
					fetch NEXT from @Cursor_Student_Result_History into
					@c4_StudentId,  
					@c4_CourseId, 
					@c4_VersionId, 
					@c4_RetakeNo,  
					@c4_ObtainedGPA, 
					@c4_ObtainedGrade, 
					@c4_CourseStatusID 
				END
	CLOSE @Cursor_Student_Result_History
	DEALLOCATE @Cursor_Student_Result_History


--STEP 2 END
----------------------------------------------------------------------
--STEP 3 START [eleminate course which are not offered]

declare 
@c5_ProgramId int,  
@c5_CourseId int, 
@c5_VersionId int;

	DECLARE  @Cursor_OfferedCourse CURSOR
	SET @Cursor_OfferedCourse = CURSOR FOR
		SELECT    
			ProgramId,
			CourseId, 
			VersionId 			
		FROM        dbo.UIUEMS_CC_OfferedCourse
		WHERE AcademicCalenderID = @AcademicCalenderID and ProgramID = @ProgramID
		
	OPEN @Cursor_OfferedCourse
	fetch NEXT from @Cursor_OfferedCourse into
				@c5_ProgramId,  
				@c5_CourseId, 
				@c5_VersionId 
				
			 WHILE (@@FETCH_STATUS <> -1)
				BEGIN
					
					UPDATE  [dbo].[RegistrationWorksheet]
					   SET
						   IsAutoOpen = 1
					 WHERE ([CourseID] = @c5_CourseId and
						   [VersionID] = @c5_VersionId and 
						   [ProgramID] = @c5_ProgramId)
					
					fetch NEXT from @Cursor_OfferedCourse into
					@c5_ProgramId,  
					@c5_CourseId, 
					@c5_VersionId
				END
				
	CLOSE @Cursor_OfferedCourse
	DEALLOCATE @Cursor_OfferedCourse


--STEP 3 END
----------------------------------------------------------------------
--STEP 4 START [eleminate course whose prerequisit are not done ]

declare 
@c6_Result int,
@c6_id int,
@c6_ProgramId int,  
@c6_CourseId int, 
@c6_VersionId int,
@c6_Node_CourseID  int;

	DECLARE  @Cursor_RegistrationWorksheet CURSOR
	SET @Cursor_RegistrationWorksheet = CURSOR FOR
		SELECT 
			id,   
			ProgramId,
			CourseId, 
			VersionId,
			Node_CourseID 			
		FROM        dbo.RegistrationWorksheet
		WHERE StudentID = @StudentID and IsAutoOpen = 1
		
	OPEN @Cursor_RegistrationWorksheet
	fetch NEXT from @Cursor_RegistrationWorksheet into
				@c6_id,
				@c6_ProgramId ,  
				@c6_CourseId , 
				@c6_VersionId ,
				@c6_Node_CourseID 
				
			 WHILE (@@FETCH_STATUS <> -1)
				BEGIN
					
					set @c6_Result =  dbo.CheckPrerequisitForRegistrationWorksheet(@c6_Node_CourseID,@c6_ProgramId);
					
					if(@c6_Result = 1)
					
					UPDATE  [dbo].[RegistrationWorksheet]
					   SET
						   IsAutoOpen = 0
					 WHERE (id =  @c6_id)
					
					fetch NEXT from @Cursor_RegistrationWorksheet into
									@c6_id,
									@c6_ProgramId ,  
									@c6_CourseId , 
									@c6_VersionId ,
									@c6_Node_CourseID 
				
				END
				
	CLOSE @Cursor_RegistrationWorksheet
	DEALLOCATE @Cursor_RegistrationWorksheet

--STEP 4 END
----------------------------------------------------------------------
--STEP 5 START [eliminate course whish is out of range by priority]
-- 6 = Passed
-- 3 = Incomplete
-- 7 = Running

update RegistrationWorksheet 
set IsAutoOpen = 0
where id not in(select top (@OpenCourse) id from dbo.RegistrationWorksheet
where IsAutoOpen = 1  and (CourseStatusId  not in(6,7,3) or CourseStatusId is null) order by [Priority])

--STEP 5 END
----------------------------------------------------------------------

--STEP 6 START [Update course Name and title ]

declare 
@c7_id int,
@c7_CourseId int, 
@c7_VersionId int,
@c7_FormalCode varchar(100),
@c7_VersionCode varchar(100),
@c7_Title varchar(100),
@c7_Credits decimal(18,2);

	DECLARE  @Cursor_UpdateCourse CURSOR
	SET @Cursor_UpdateCourse = CURSOR FOR
		SELECT 
			id,
			CourseId, 
			VersionId			
		FROM        dbo.RegistrationWorksheet
		WHERE StudentID = @StudentID
		
	OPEN @Cursor_UpdateCourse
	fetch NEXT from @Cursor_UpdateCourse into
				@c7_id,				
				@c7_CourseId , 
				@c7_VersionId 
				
			 WHILE (@@FETCH_STATUS <> -1)
				BEGIN
									
					select  @c7_FormalCode = FormalCode, 
							@c7_VersionCode = VersionCode,
							@c7_Title =  Title,
							@c7_Credits = Credits							
							from UIUEMS_CC_Course
							where CourseID = @c7_CourseId and VersionID = @c7_VersionId 
												
					UPDATE   RegistrationWorksheet
					   SET 	FormalCode = @c7_FormalCode, 
						    VersionCode = @c7_VersionCode, 
						    CourseTitle	= @c7_Title,
						    Credits = @c7_Credits
					 WHERE (id =  @c7_id)
					
					fetch NEXT from @Cursor_UpdateCourse into
									@c7_id,				
									@c7_CourseId , 
									@c7_VersionId  
				
				END
				
	CLOSE @Cursor_UpdateCourse
	DEALLOCATE @Cursor_UpdateCourse

--STEP 6 END
----------------------------------------------------------------------


--Return result
-- error checking not complete

set @ReturnValue = 1;
return @ReturnValue;
--
END



GO
/****** Object:  StoredProcedure [dbo].[PrepareRegistrationWorksheetNew]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[PrepareRegistrationWorksheetNew] 
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@OpenCourse int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int,

@ReturnValue int OUTPUT,

@AutoOpen numeric(18,2),
@AutoPreRegi numeric(18,2),
@AutoMandatory  numeric(18,2)


AS
BEGIN
SET NOCOUNT ON;



DECLARE

--* CURSOR TreeCalendarDetail c1
	@c1_TreeCalendarDetailID int,
	@c1_TreeCalendarMasterID int,
--# CURSOR TreeCalendarDetail

--* CURSOR Cal_Course_Prog_Node c2
	@c2_CalCorProgNodeID int,
	@c2_TreeCalendarDetailID int ,
	@c2_OfferedByProgramID int,
	@c2_CourseID int ,
	@c2_VersionID int,
	@c2_Node_CourseID int,
	@c2_NodeID int,
	@c2_NodeLinkName varchar(100),
	@c2_Priority int,
	@c2_Credits decimal(18,2),
	@c2_IsMajorRelated bit,
--# CURSOR Cal_Course_Prog_Node

	@c3_CourseID int ,
	@c3_VersionID int,
	@c3_Node_CourseID int;

----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 1 START : Generate course list from tree with priority of calender.

declare		@Cursor_TreeCalendarDetail CURSOR
declare		@Cursor_Cal_Course_Prog_Node CURSOR
declare		@Cursor_Node CURSOR


	--* CURSOR TreeCalendarDetail
	set @Cursor_TreeCalendarDetail = CURSOR FOR
	select TreeCalendarDetailID,TreeCalendarMasterID from dbo.UIUEMS_CC_TreeCalendarDetail
		where TreeCalendarMasterID = @TreeCalendarMasterID and TreeMasterID = @TreeMasterID

	OPEN @Cursor_TreeCalendarDetail
		fetch NEXT from @Cursor_TreeCalendarDetail into
				@c1_TreeCalendarDetailID,
				@c1_TreeCalendarMasterID  
				
		while @@FETCH_STATUS = 0
		BEGIN 
			    	--* CURSOR Cal_Course_Prog_Node			
					set @Cursor_Cal_Course_Prog_Node = CURSOR FOR
					select 
						CalCorProgNodeID,
						TreeCalendarDetailID, 
						OfferedByProgramID, 
						CourseID, 
						VersionID, 
						Node_CourseID, 
						NodeID, 
						NodeLinkName, 
						[Priority], 
						Credits,
						IsMajorRelated
					from dbo.UIUEMS_CC_Cal_Course_Prog_Node
					where TreeCalendarDetailID  = @c1_TreeCalendarDetailID			
					
					 OPEN @Cursor_Cal_Course_Prog_Node
						fetch NEXT from @Cursor_Cal_Course_Prog_Node into
									@c2_CalCorProgNodeID ,
									@c2_TreeCalendarDetailID  ,
									@c2_OfferedByProgramID ,
									@c2_CourseID  ,
									@c2_VersionID ,
									@c2_Node_CourseID ,
									@c2_NodeID ,
									@c2_NodeLinkName ,
									@c2_Priority ,
									@c2_Credits ,
									@c2_IsMajorRelated 						
						
						while @@FETCH_STATUS = 0
						BEGIN 	
							if(@c2_CourseID is null)
								begin									
									DECLARE
										   @crs  CURSOR
											 EXEC AllCourseByNodeCursorParam @crs output, @c2_NodeID
											 
											 fetch NEXT from @crs into
												@c3_CourseID  ,
												@c3_VersionID ,
												@c3_Node_CourseID 
											 WHILE (@@FETCH_STATUS <> -1)
												BEGIN
												
												-- INSERT COURSE FROM NODE
												
												INSERT into [dbo].[RegistrationWorksheet]
														   ([StudentID]
														   ,[CalCourseProgNodeID]
														   ,[TreeCalendarDetailID]
														   ,[TreeCalendarMasterID]													   
														   ,[CourseID]
														   ,[VersionID]
														   ,[Credits]
														   ,[Node_CourseID]
														   ,[NodeID]													   
														   ,[NodeLinkName]													   
														   ,[Priority]
														   ,[IsMajorRelated]
														   ,[ProgramID]
														   ,[DeptID]
														   ,[AcademicCalenderID]
														   
														   ,[IsAutoOpen]
														   ,[IsAutoAssign]
														   ,[Isrequisitioned]
														   ,[IsMandatory]
														   ,[IsManualOpen]
														   
														   
														    
														  )
													 VALUES
														   (@StudentId
															,@c2_CalCorProgNodeID
															,@c2_TreeCalendarDetailID
															,@c1_TreeCalendarMasterID													  
															,@c3_CourseID  
															,@c3_VersionID 								
															,@c2_Credits
															,@c3_Node_CourseID 
															,@c2_NodeID 
															,@c2_NodeLinkName 
															,@c2_Priority
															,@c2_IsMajorRelated 
															,@c2_OfferedByProgramID 
															,@DepartmentID
															,@AcademicCalenderID
															,'False'
															,'False'
															,'False'
															,'False'
															,'False'	
																
														   ) 
												
												 fetch NEXT from @crs into
												@c3_CourseID  ,
												@c3_VersionID ,
												@c3_Node_CourseID
												END;
																		 
										CLOSE @crs;
										DEALLOCATE @crs;
									
								end
							else
								begin
									-- INSERT COURSE 
									
									INSERT into [dbo].[RegistrationWorksheet]
													   ([StudentID]
													   ,[CalCourseProgNodeID]
													   ,[TreeCalendarDetailID]
													   ,[TreeCalendarMasterID]													   
													   ,[CourseID]
													   ,[VersionID]
													   ,[Credits]
													   ,[Node_CourseID]
													   ,[NodeID]													   
													   ,[NodeLinkName]													   
													   ,[Priority]
													   ,[IsMajorRelated]
													   ,[ProgramID]
													   ,[DeptID]
													   ,[AcademicCalenderID]
													   
													   ,[IsAutoOpen]
													   ,[IsAutoAssign]
													   ,[Isrequisitioned]
													   ,[IsMandatory]
													   ,[IsManualOpen]
													    
													  )
												 VALUES
													   (@StudentId
														,@c2_CalCorProgNodeID
														,@c2_TreeCalendarDetailID
														,@c1_TreeCalendarMasterID													  
														,@c2_CourseID  
														,@c2_VersionID
														,@c2_Credits
														,@c2_Node_CourseID 
														,@c2_NodeID 
														,@c2_NodeLinkName 
														,@c2_Priority
														,@c2_IsMajorRelated 
														,@c2_OfferedByProgramID
														,@DepartmentID
														,@AcademicCalenderID 
														,'False'
														,'False'
														,'False'
														,'False'
														,'False'		
													   ) 
								end
								
							fetch NEXT from @Cursor_Cal_Course_Prog_Node into
									@c2_CalCorProgNodeID ,
									@c2_TreeCalendarDetailID  ,
									@c2_OfferedByProgramID ,
									@c2_CourseID  ,
									@c2_VersionID ,
									@c2_Node_CourseID ,
									@c2_NodeID ,
									@c2_NodeLinkName ,
									@c2_Priority ,
									@c2_Credits ,
									@c2_IsMajorRelated 		
						END	 
						--# CURSOR Cal_Course_Prog_Node

				fetch NEXT from @Cursor_TreeCalendarDetail into
								@c1_TreeCalendarDetailID,
								@c1_TreeCalendarMasterID 
		END
		--# CURSOR TreeCalendarDetail	
	
	
	CLOSE @Cursor_TreeCalendarDetail
	DEALLOCATE @Cursor_TreeCalendarDetail
	
--STEP 1 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 2 START [update Student Result History]

declare 
@s2_StudentId int,  
@s2_CourseId int, 
@s2_VersionId int, 
@s2_RetakeNo int,  
@s2_ObtainedGPA numeric(18,2), 
@s2_ObtainedGrade varchar(150), 
@s2_CourseStatusID int;

	DECLARE  @Cursor_Student_Result_History CURSOR
	set @Cursor_Student_Result_History = cursor for
		SELECT    
			StudentId,  
			CourseId, 
			VersionId, 
			RetakeNo,  
			ObtainedGPA, 
			ObtainedGrade, 
			CourseStatusID 
		FROM         UIUEMS_CC_Student_CourseHistory
		WHERE     (StudentID = @StudentId) and IsConsiderGPA = 1
		
	OPEN @Cursor_Student_Result_History
	fetch NEXT from @Cursor_Student_Result_History into
				@s2_StudentId,  
				@s2_CourseId, 
				@s2_VersionId, 
				@s2_RetakeNo,  
				@s2_ObtainedGPA, 
				@s2_ObtainedGrade, 
				@s2_CourseStatusID  
			 WHILE (@@FETCH_STATUS <> -1)
				BEGIN
					
					UPDATE  [dbo].[RegistrationWorksheet]
					   SET
						   [RetakeNo] = @s2_RetakeNo
						  ,[ObtainedGPA] = @s2_ObtainedGPA
						  ,[ObtainedGrade] =   @s2_ObtainedGrade   
						  ,[CourseStatusId] = @s2_CourseStatusID
						  ,[IsCompleted] =	CASE @s2_CourseStatusID
												WHEN 6 THEN 1
												ELSE 0
											END
					 WHERE [CourseID] = @s2_CourseId and
						   [VersionID] = @s2_VersionId and 
						   [StudentId] = @s2_StudentId
					
					fetch NEXT from @Cursor_Student_Result_History into
					@s2_StudentId,  
					@s2_CourseId, 
					@s2_VersionId, 
					@s2_RetakeNo,  
					@s2_ObtainedGPA, 
					@s2_ObtainedGrade, 
					@s2_CourseStatusID 
				END
	CLOSE @Cursor_Student_Result_History
	DEALLOCATE @Cursor_Student_Result_History


--STEP 2 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 3 START [Update course Name and title ]

declare 
@s3_id int,
@s3_CourseId int, 
@s3_VersionId int,
@s3_FormalCode varchar(100),
@s3_VersionCode varchar(100),
@s3_Title varchar(100),
@s3_Credits decimal(18,2);

	DECLARE  @Cursor_UpdateCourse CURSOR
	SET @Cursor_UpdateCourse = CURSOR FOR
		SELECT 
			id,
			CourseId, 
			VersionId			
		FROM        dbo.RegistrationWorksheet
		WHERE StudentID = @StudentID
		
	OPEN @Cursor_UpdateCourse
	fetch NEXT from @Cursor_UpdateCourse into
				@s3_id,				
				@s3_CourseId , 
				@s3_VersionId 
				
			 WHILE (@@FETCH_STATUS <> -1)
				BEGIN
									
					select  @s3_FormalCode = FormalCode, 
							@s3_VersionCode = VersionCode,
							@s3_Title =  Title,
							@s3_Credits = Credits							
							from UIUEMS_CC_Course
							where CourseID = @s3_CourseId and VersionID = @s3_VersionId 
												
					UPDATE   RegistrationWorksheet
					   SET 	FormalCode = @s3_FormalCode, 
						    VersionCode = @s3_VersionCode, 
						    CourseTitle	= @s3_Title,
						    Credits = @s3_Credits
					 WHERE (id =  @s3_id)
					
					fetch NEXT from @Cursor_UpdateCourse into
									@s3_id,				
									@s3_CourseId , 
									@s3_VersionId  
				
				END
				
	CLOSE @Cursor_UpdateCourse
	DEALLOCATE @Cursor_UpdateCourse

--STEP 3 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 4 START : Open Course(Auto open) by locality defination Setup, By priority and GPA range 

DECLARE 
@s4_id int,
@s4_CourseId int, 
@s4_VersionId int,
@s4_SequenceNo int,
@s4_Credit int,
@s4_CreditCount int,
@s4_Priority int,
@s4_Result int,
@s4_Node_CourseID int,
@s4_ProgramId int,
@s4_AssocCourseID int, 
@s4_AssocVersionID int,
@s4_TempId int

DECLARE @S4TempTable TABLE(
Id    int,
CourseId int, 
VersionId int,
[Priority] int
)


set @s4_CreditCount = 1;

DECLARE  @Cursor_AutoOpenCourseByLocality CURSOR
	SET @Cursor_AutoOpenCourseByLocality = CURSOR FOR
		SELECT 
			id ,
			CourseId , 
			VersionId ,
			[Priority] ,
			Node_CourseID,
			ProgramId,
			Credits
		FROM dbo.RegistrationWorksheet
		WHERE (CourseStatusId  not in(6,7,3) or CourseStatusId is null) ORDER BY [Priority]
		
		OPEN @Cursor_AutoOpenCourseByLocality
		FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
		INTO    @s4_id ,
				@s4_CourseId , 
				@s4_VersionId ,
				@s4_Priority,
				@s4_Node_CourseID,
				@s4_ProgramId,
				@s4_Credit
		
		WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
				if(@s4_CreditCount <= 12) -- # eleminate course which are out of range.	
					BEGIN
					
						SET @s4_Result =  dbo.CheckPrerequisitForRegistrationWorksheet(@s4_Node_CourseID,@s4_ProgramId);					
						IF(@s4_Result = 1) -- # eleminate course which prerequisit are not done.	
						BEGIN
												
							INSERT INTO @S4TempTable 
							(Id , CourseId , VersionId, [Priority]) VALUES
							(@s4_id , @s4_CourseId , @s4_VersionId,	@s4_Priority)
							
							SELECT @s4_AssocCourseID = AssocCourseID , @s4_AssocVersionID =AssocVersionID
							FROM UIUEMS_CC_Course WHERE CourseID = @s4_CourseId and   VersionID = @s4_VersionId
							
							IF ( @s4_AssocCourseID is not null)
							BEGIN
								INSERT INTO @S4TempTable 
								(Id , CourseId , VersionId, [Priority]) VALUES
								(@s4_id , @s4_AssocCourseID , @s4_AssocVersionID,	@s4_Priority)
							END
							
						END	
						
						SET @s4_CreditCount = @s4_CreditCount + @s4_Credit;		
					END
				FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
				INTO    @s4_id ,
						@s4_CourseId , 
						@s4_VersionId ,
						@s4_Priority,
						@s4_Node_CourseID,
						@s4_ProgramId,
						@s4_Credit	
				
				END
				
				--SELECT * FROM @S4TempTable;
				
	--#	 Update worksheet	
	DECLARE  @Cursor_TempTable CURSOR
	SET @Cursor_TempTable = CURSOR FOR
		SELECT 
			id 
		FROM @S4TempTable
				
		OPEN @Cursor_TempTable
		FETCH NEXT FROM @Cursor_TempTable 
		INTO    @s4_TempId 
				
				WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
					UPDATE   RegistrationWorksheet
					   SET 	IsAutoOpen = 'True'
					 WHERE (id =  @s4_TempId)
				
					FETCH NEXT FROM @Cursor_TempTable 
					INTO    @s4_TempId 
				END
				
	CLOSE @Cursor_TempTable
	DEALLOCATE @Cursor_TempTable
				
	--# END Update		
				
	CLOSE @Cursor_AutoOpenCourseByLocality
	DEALLOCATE @Cursor_AutoOpenCourseByLocality



--STEP 4 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 5 START : Open Course by Auto Pre-Registration(IsAutoAssign) Setup, By priority and GPA range 


DECLARE 
@s5_id int,
@s5_CourseId int, 
@s5_VersionId int,
@s5_SequenceNo int,
@s5_Credit int,
@s5_CreditCount int,
@s5_Priority int,
@s5_Result int,
@s5_Node_CourseID int,
@s5_ProgramId int,
@s5_AssocCourseID int, 
@s5_AssocVersionID int,
@s5_TempId int

DECLARE @S5TempTable TABLE(
Id    int,
CourseId int, 
VersionId int,
[Priority] int
)


set @s5_CreditCount = 1;

DECLARE  @Cursor_AutoPreRegistration CURSOR
	SET @Cursor_AutoPreRegistration = CURSOR FOR
		SELECT 
			id ,
			CourseId , 
			VersionId ,
			[Priority] ,
			Node_CourseID,
			ProgramId,
			Credits
		FROM dbo.RegistrationWorksheet
		WHERE (CourseStatusId  not in(6,7,3) or CourseStatusId is null) ORDER BY [Priority]
		
		OPEN @Cursor_AutoPreRegistration
		FETCH NEXT FROM @Cursor_AutoPreRegistration 
		INTO    @s5_id ,
				@s5_CourseId , 
				@s5_VersionId ,
				@s5_Priority,
				@s5_Node_CourseID,
				@s5_ProgramId,
				@s5_Credit
		
		WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
				if(@s5_CreditCount <= 12) -- # eleminate course which are out of range.	
					BEGIN
					
						SET @s5_Result =  dbo.CheckPrerequisitForRegistrationWorksheet(@s5_Node_CourseID,@s5_ProgramId);					
						IF(@s5_Result = 1) -- # eleminate course which prerequisit are not done.	
						BEGIN
												
							INSERT INTO @S5TempTable 
							(Id , CourseId , VersionId, [Priority]) VALUES
							(@s5_id , @s5_CourseId , @s5_VersionId,	@s5_Priority)
							
							SELECT @s5_AssocCourseID = AssocCourseID , @s5_AssocVersionID =AssocVersionID
							FROM UIUEMS_CC_Course WHERE CourseID = @s5_CourseId and   VersionID = @s5_VersionId
							
							IF ( @s5_AssocCourseID is not null)
							BEGIN
								INSERT INTO @S5TempTable 
								(Id , CourseId , VersionId, [Priority]) VALUES
								(@s5_id , @s5_AssocCourseID , @s5_AssocVersionID,	@s5_Priority)
							END
							
						END	
						
						SET @s5_CreditCount = @s5_CreditCount + @s5_Credit;		
					END
				FETCH NEXT FROM @Cursor_AutoPreRegistration 
				INTO    @s5_id ,
						@s5_CourseId , 
						@s5_VersionId ,
						@s5_Priority,
						@s5_Node_CourseID,
						@s5_ProgramId,
						@s5_Credit	
				
				END
				
				--SELECT * FROM @S4TempTable;
				
	--#	 Update worksheet	
	DECLARE  @S5_Cursor_TempTable CURSOR
	SET @S5_Cursor_TempTable = CURSOR FOR
		SELECT 
			id 
		FROM @S5TempTable
				
		OPEN @S5_Cursor_TempTable
		FETCH NEXT FROM @S5_Cursor_TempTable 
		INTO    @s5_TempId 
				
				WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
					UPDATE   RegistrationWorksheet
					   SET 	IsAutoAssign = 'True'
					 WHERE (id =  @s5_TempId)
				
					FETCH NEXT FROM @S5_Cursor_TempTable 
					INTO    @s5_TempId 
				END
				
	CLOSE @S5_Cursor_TempTable
	DEALLOCATE @S5_Cursor_TempTable
				
	--# END Update		
				
	CLOSE @Cursor_AutoPreRegistration
	DEALLOCATE @Cursor_AutoPreRegistration



--STEP 5 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 6 START : Open Course by Auto Mandatory Setup, By priority and GPA range 


DECLARE 
@s6_id int,
@s6_CourseId int, 
@s6_VersionId int,
@s6_SequenceNo int,
@s6_Credit int,
@s6_CreditCount int,
@s6_Priority int,
@s6_Result int,
@s6_Node_CourseID int,
@s6_ProgramId int,
@s6_AssocCourseID int, 
@s6_AssocVersionID int,
@s6_TempId int

DECLARE @S6TempTable TABLE(
Id    int,
CourseId int, 
VersionId int,
[Priority] int
)


set @s6_CreditCount = 1;

DECLARE  @Cursor_AutoMandatory CURSOR
	SET @Cursor_AutoMandatory = CURSOR FOR
		SELECT 
			id ,
			CourseId , 
			VersionId ,
			[Priority] ,
			Node_CourseID,
			ProgramId,
			Credits
		FROM dbo.RegistrationWorksheet
		WHERE (CourseStatusId  not in(6,7,3) or CourseStatusId is null) ORDER BY [Priority]
		
		OPEN @Cursor_AutoMandatory
		FETCH NEXT FROM @Cursor_AutoMandatory 
		INTO    @s6_id ,
				@s6_CourseId , 
				@s6_VersionId ,
				@s6_Priority,
				@s6_Node_CourseID,
				@s6_ProgramId,
				@s6_Credit
		
		WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
				if(@s6_CreditCount <= 12) -- # eleminate course which are out of range.	
					BEGIN
					
						SET @s6_Result =  dbo.CheckPrerequisitForRegistrationWorksheet(@s6_Node_CourseID,@s6_ProgramId);					
						IF(@s6_Result = 1) -- # eleminate course which prerequisit are not done.	
						BEGIN
												
							INSERT INTO @S6TempTable 
							(Id , CourseId , VersionId, [Priority]) VALUES
							(@s6_id , @s6_CourseId , @s6_VersionId,	@s6_Priority)
							
							SELECT @s6_AssocCourseID = AssocCourseID , @s6_AssocVersionID =AssocVersionID
							FROM UIUEMS_CC_Course WHERE CourseID = @s6_CourseId and   VersionID = @s6_VersionId
							
							IF ( @s6_AssocCourseID is not null)
							BEGIN
								INSERT INTO @S6TempTable 
								(Id , CourseId , VersionId, [Priority]) VALUES
								(@s6_id , @s6_AssocCourseID , @s6_AssocVersionID,	@s6_Priority)
							END
							
						END	
						
						SET @s6_CreditCount = @s6_CreditCount + @s6_Credit;		
					END
				FETCH NEXT FROM @Cursor_AutoMandatory 
				INTO    @s6_id ,
						@s6_CourseId , 
						@s6_VersionId ,
						@s6_Priority,
						@s6_Node_CourseID,
						@s6_ProgramId,
						@s6_Credit	
				
				END
				
				--SELECT * FROM @S4TempTable;
				
	--#	 Update worksheet	
	DECLARE  @S6_Cursor_TempTable CURSOR
	SET @S6_Cursor_TempTable = CURSOR FOR
		SELECT 
			id 
		FROM @S6TempTable
				
		OPEN @S6_Cursor_TempTable
		FETCH NEXT FROM @S6_Cursor_TempTable 
		INTO    @s6_TempId 
				
				WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
					UPDATE   RegistrationWorksheet
					   SET 	IsAutoAssign = 'True'
					 WHERE (id =  @s6_TempId)
				
					FETCH NEXT FROM @S6_Cursor_TempTable 
					INTO    @s6_TempId 
				END
				
	CLOSE @S6_Cursor_TempTable
	DEALLOCATE @S6_Cursor_TempTable
				
	--# END Update		
				
	CLOSE @Cursor_AutoMandatory
	DEALLOCATE @Cursor_AutoMandatory



--STEP 5 END
----------------------------------------------------------------------
----------------------------------------------------------------------




--Return result
-- error checking not complete

set @ReturnValue = 1;
return @ReturnValue;
--
END



GO
/****** Object:  StoredProcedure [dbo].[RegistrationBillFinal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Saima>
-- create date: <11/08/2011>
-- Description:	billing after registration
-- =============================================
CREATE PROCEDURE [dbo].[RegistrationBillFinal]
	-- Add the parameters for the stored procedure here	 
		@StdID int,  
		@aclID int,
		@progID int,
		@remarks varchar(250),-- check the data type and range for remark field from dbo.UIUEMS_BL_Std_Crs_Bill_Worksheet
		@CreatorID int
	,@return int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
    
	
	-- Billing as per academic calender (from type definition table)
		declare	@v_stdAccHeadID int

		
				select @v_stdAccHeadID = AccountsID from dbo.UIUEMS_AC_AccountHeads 
				where Name = (select Roll from dbo.UIUEMS_ER_Student where StudentID = @StdID)			
				
				declare
					@Amount money
					,@Definition nvarchar(250)
					,@AccountsID int


				----- getting declared fees for that academic calender
				declare feeCrsr cursor LOCAL FAST_FORWARD for (select --fs.FeeSetupID, fs.TypeDefID, 
																	fs.Amount, td.Definition, td.AccountsID 
																	from dbo.UIUEMS_BL_FeeSetup fs, dbo.UIUEMS_AC_TypeDefinition td
																	where fs.AcaCalID = @aclID and fs.ProgramID = @progID 
																			and fs.TypeDefID = td.TypeDefinitionID 
																			and td.IsPerAcaCal = 'true')
				open feeCrsr
					fetch feeCrsr into @Amount, @Definition, @AccountsID

					while(@@fetch_status = 0)
					begin
						---- checking whether bill is created or not. if bill is not created, then creating here
						---- confused about debit and credit. plz correct.
						---- if heads of debit and credit is changed, then replace them in this query (used in if condition) also
						if not exists (select v.SLNO 
										from dbo.UIUEMS_AC_Voucher v, (select SLNO from dbo.UIUEMS_AC_Voucher 
																		where CrAccountHeadsID = @AccountsID)tbl
										where v.DrAccountHeadsID = @v_stdAccHeadID and tbl.SLNO = v.SLNO and AcaCalID = @aclID)
							begin	
									declare @v_SLNO int
									set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1
									EXEC dbo.CreateBillVoucher									 
										   'Bill'
										   ,@v_SLNO
										   ,@aclID
										   ,@v_stdAccHeadID
										   ,null--<CrAccountHeadsID, int,>
										   ,@Amount--<Amount, money,> 
										   ,'System'
										   ,null
										   ,null
										   ,@Definition
										   ,@CreatorID

									
									EXEC dbo.CreateBillVoucher
												   'Bill'
												   ,@v_SLNO
												   ,@aclID
												   ,null--DrAccountHeadsID, int,>
												   ,@AccountsID
												   ,@Amount--<Amount, money,>
												   ,'System'
												   ,null
												   ,null
												   ,@Definition
												   ,@CreatorID
							end

						
						fetch feeCrsr into @Amount, @Definition, @AccountsID 
							 
					end
				close feeCrsr
				deallocate feeCrsr
		
-- registered course billing
	declare
		  @BillWorkSheetId int 
		  ,@StudentId int
		  ,@CalCourseProgNodeID int
		  ,@AcaCalSectionID int
		  ,@SectionTypeId int
		  ,@AcaCalId int
		  ,@CourseId int
		  ,@VersionId int
		  ,@CourseTypeId int
		  ,@ProgramId int
		  ,@RetakeNo int
		  ,@PreviousBestGrade varchar(2)
		  ,@FeeSetupId int
		  ,@PerCreditAmountFee money
		  ,@DiscountTypeId int
		  ,@DiscountPercentage decimal(18, 2)

			-- local varables
			--,@v_isCredit bit
			,@v_HeadName nvarchar(50)
			,@v_BillableRetakeNo int 
			,@v_Credits numeric(18, 2)
			,@v_PerCreditAmount money
			,@v_BalanceAmount money

			,@v_CalenderTypeName varchar(50)
			,@v_Year int
			,@v_remarks varchar(500)
	 
			SELECT @v_CalenderTypeName = UIUEMS_CC_CalenderUnitType.TypeName, @v_Year = UIUEMS_CC_AcademicCalender.Year
			FROM UIUEMS_CC_CalenderUnitType INNER JOIN UIUEMS_CC_AcademicCalender 
			ON UIUEMS_CC_CalenderUnitType.CalenderUnitTypeID = UIUEMS_CC_AcademicCalender.CalenderUnitTypeID
			and UIUEMS_CC_AcademicCalender.AcademicCalenderID = @aclID

		declare stdCcpnCrsr cursor LOCAL FAST_FORWARD for (select [BillWorkSheetId]
																  ,[StudentId]
																  ,[CalCourseProgNodeID]
																  ,[AcaCalSectionID]
																  ,[SectionTypeId]
																  ,[AcaCalId]
																  ,[CourseId]
																  ,[VersionId]
																  ,[CourseTypeId]
																  ,[ProgramId]
																  ,[RetakeNo]
																  ,[PreviousBestGrade]
																  ,[FeeSetupId]
																  ,[Fee]-- [PerCreditAmountFee]check the field name
																  ,[DiscountTypeId]
																  ,[DiscountPercentage]
															from dbo.UIUEMS_BL_Std_Crs_Bill_Worksheet 
															where [ProgramId] = @progID 
																and [StudentId] = @StdID 
																and [AcaCalId] = @aclID)
	open stdCcpnCrsr
			fetch stdCcpnCrsr into @BillWorkSheetId
								  ,@StudentId
								  ,@CalCourseProgNodeID
								  ,@AcaCalSectionID
								  ,@SectionTypeId
								  ,@AcaCalId
								  ,@CourseId
								  ,@VersionId
								  ,@CourseTypeId
								  ,@ProgramId
								  ,@RetakeNo
								  ,@PreviousBestGrade
								  ,@FeeSetupId
								  ,@PerCreditAmountFee
								  ,@DiscountTypeId
								  ,@DiscountPercentage

			while(@@fetch_status = 0)
			begin
			-- credit wise total amount billing
			select @v_HeadName = Name from dbo.UIUEMS_AC_AccountHeads 
			where @v_stdAccHeadID = AccountsID
			--select @v_isCredit = IsCreditCourse from dbo.UIUEMS_CC_Course where CourseID = @CourseID and VersionID = @VersionID

--			SELECT @v_BillableRetakeNo = [BillStartFromRetakeNo]
--			FROM [dbo].[UIUEMS_BL_IsCourseBillable]
--			where [IsCreditCourse] = (select IsCreditCourse 
--										from dbo.UIUEMS_CC_Course 
--										where CourseID = @CourseId 
--											and VersionID = @VersionId)
--				 and [AcaCalID] = @AcaCalId
--				 and ProgramID = @progID
			
			UPDATE [dbo].[UIUEMS_CC_Student_CourseHistory]
			   SET [CourseStatusID] = 7
				  ,[CourseStatusDate] = getdate()
				  ,[ModifiedBy] = @CreatorID
				  ,[ModifiedDate] = getdate()
			 WHERE [StudentID] = @StudentId 
				and [CalCourseProgNodeID] = @CalCourseProgNodeID 
				--and [CourseStatusID] = 8 ----commented out bcz it is assumed that [CourseStatusID] is 8 for this @CalCourseProgNodeID since bill worksheet contains those where [CourseStatusID] is 8
			
--			if(@RetakeNo = null) set @RetakeNo = 0
--			if(@RetakeNo >= @v_BillableRetakeNo)
--				begin
					select @v_Credits = Credits from dbo.UIUEMS_CC_Course where CourseID = @CourseId and VersionID = @VersionId								

					set @v_BalanceAmount = @v_Credits * @PerCreditAmountFee;			
				
					set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

					set @v_remarks = 'Billable head : ' + @v_HeadName + '-' + @v_CalenderTypeName + ' ' + @v_Year + '-' + @remarks
						EXEC dbo.CreateBillVoucher
							   'Bill'
							   ,@v_SLNO
							   ,@v_stdAccHeadID
								,null
							   ,@v_BalanceAmount--<Amount, money,>
							   ,'System'
							   ,@CourseID
							   ,@VersionID
							   ,@v_remarks
							   ,@CreatorID
								

						select @AccountsID = AccountsID from dbo.UIUEMS_AC_TypeDefinition where IsCourseSpecific = 'true' --and Type is like 'Fee%'
						select @v_HeadName = Name from dbo.UIUEMS_AC_AccountHeads where AccountsID = @AccountsID
						set @v_remarks = 'Billable head : ' + @v_HeadName + '-' + @v_CalenderTypeName + ' ' + @v_Year + '-' + @remarks
						EXEC dbo.CreateBillVoucher
							   'Bill'
							   ,@v_SLNO
							   ,null
							   ,@AccountsID
							   ,@v_BalanceAmount--<Amount, money,>
							   ,'System'
							   ,@CourseID
							   ,@VersionID
							   ,@v_remarks
							   ,@CreatorID
							
				
				--end
				-- discount billing
				declare @v_rowcount int
						,@v_priority int
						,@v_typeDefID int
						,@v_accId int
						,@v_flag int
						,@v_DiscountAmount money
				set @v_rowcount = (select count(*) from dbo.UIUEMS_AC_TypeDefinition where Type like 'Discount%')
				set @v_priority = -1
				set @v_flag = 0
				while (@v_flag < @v_rowcount)
					begin
						set @v_flag = @v_flag + 1
						select @v_priority = Priority,@v_typeDefID = TypeDefinitionID, @v_accId = AccountsID  
							from dbo.UIUEMS_AC_TypeDefinition 
							where Priority = (select min(Priority)  
												from dbo.UIUEMS_AC_TypeDefinition 
												where Type like 'Discount%'and Priority > @v_priority)

						if @v_typeDefID = @DiscountTypeId and @DiscountPercentage > 0.00--0 discounts are not billied here
							begin
								set @v_DiscountAmount = @v_BalanceAmount * (@DiscountPercentage / 100)
								set @v_BalanceAmount = @v_BalanceAmount - @v_DiscountAmount
								set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1
								select @v_HeadName = Name from dbo.UIUEMS_AC_AccountHeads 
									where @v_stdAccHeadID = AccountsID
								set @v_remarks = 'Billable head : ' + @v_HeadName + '-' + @v_CalenderTypeName + ' ' + @v_Year + '-' + @remarks
								EXEC dbo.CreateBillVoucher
									   'Bill'
									   ,@v_SLNO
									   ,null
									   ,@v_stdAccHeadID
									   ,@v_DiscountAmount--<Amount, money,>
									   ,'System'
									   ,@CourseID
									   ,@VersionID
									   ,@v_remarks
									   ,@CreatorID									


						select @AccountsID = AccountsID from dbo.UIUEMS_AC_TypeDefinition where TypeDefinitionID = @DiscountTypeId 
						select @v_HeadName = Name from dbo.UIUEMS_AC_AccountHeads where AccountsID = @AccountsID
						set @v_remarks = 'Billable head : ' + @v_HeadName + '-' + @v_CalenderTypeName + ' ' + @v_Year + '-' + @remarks
						EXEC dbo.CreateBillVoucher
							   'Bill'
							   ,@v_SLNO
							   ,@AccountsID
							   ,null							   
							   ,@v_DiscountAmount--<Amount, money,>
							   ,'System'
							   ,@CourseID
							   ,@VersionID
							   ,@v_remarks
							   ,@CreatorID							
								
							end					

					end				 

				fetch stdCcpnCrsr into @BillWorkSheetId
								  ,@StudentId
								  ,@CalCourseProgNodeID
								  ,@AcaCalSectionID
								  ,@SectionTypeId
								  ,@AcaCalId
								  ,@CourseId
								  ,@VersionId
								  ,@CourseTypeId
								  ,@ProgramId
								  ,@RetakeNo
								  ,@PreviousBestGrade
								  ,@FeeSetupId
								  ,@PerCreditAmountFee
								  ,@DiscountTypeId
								  ,@DiscountPercentage
					 
			end
		close stdCcpnCrsr
		deallocate stdCcpnCrsr
set @return = 1
return @return
END






GO
/****** Object:  StoredProcedure [dbo].[RelationBetweenUserPerson-Employee]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-12-09 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[RelationBetweenUserPerson-Employee]
As

Begin
	Declare EmployeeList Cursor
	For Select TeacherId, Code, PersonId From UIUEMS_CC_Employee;

	Declare @teacherId Int, @code Nvarchar(100), @personId Int;

	Open EmployeeList
	Fetch Next From EmployeeList Into @teacherId, @code, @personId;
	While @@FETCH_STATUS = 0
	Begin
		Declare @userExist Int; Set @userExist = Null;

		Select @userExist = User_ID From UIUEMS_AD_User Where LogInID = @code;
		If @userExist Is Null
		Begin
			Declare @roleExistStartDate Datetime, @roleExistEndDate Datetime, @createdDate Datetime;
			Set @createdDate = GetDate(); Set @roleExistStartDate = '2013-07-24'; Set @roleExistEndDate = '2020-12-30';
			Declare @uId Int; Select @uId = Max(USER_ID) From UIUEMS_AD_User; Set @uId = @uId + 1;
			Insert Into UIUEMS_AD_User(User_ID, LogInID, Password, RoleID, RoleExistStartDate, RoleExistEndDate, IsActive, CreatedBy, CreatedDate) Values(@uId, @code, @code, 4, @roleExistStartDate, @roleExistEndDate, 1, -1, @createdDate);
			Insert Into UIUEMS_AD_UserInPerson(User_ID, PersonID) Values(@uId, @personId);
		End
		Else
		Begin
			Declare @userInPersonExist Int; Set @userInPersonExist = Null;
			Select @userInPersonExist = User_ID From UIUEMS_AD_UserInPerson Where User_ID = @userExist;
			If @userInPersonExist Is Null
			Begin
				Insert Into UIUEMS_AD_UserInPerson(User_ID, PersonID) Values(@userExist, @personId);
			End
			Set @userInPersonExist = Null;
		End
		Set @teacherId = Null; Set @code = Null; Set @personId = Null; Set @userExist = Null;
		Fetch Next From EmployeeList Into @teacherId, @code, @personId;
	End
End


GO
/****** Object:  StoredProcedure [dbo].[RelationBetweenUserPerson-Student]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-12-09 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[RelationBetweenUserPerson-Student]
As

Begin
	Declare StudentList Cursor
	For Select StudentId, Roll, PersonId From UIUEMS_ER_Student;

	Declare @studentId Int, @roll nvarchar(15), @personId Int;

	Open StudentList
	Fetch Next From StudentList Into @studentId, @roll, @personId;
	While @@FETCH_STATUS = 0
	Begin
		Declare @userExist Int; Set @userExist = Null;

		Select @userExist = User_ID From UIUEMS_AD_User Where LogInID = @roll;
		If @userExist Is Null
		Begin
			Declare @roleExistStartDate Datetime, @roleExistEndDate Datetime, @createdDate Datetime;
			Set @createdDate = GetDate(); Set @roleExistStartDate = '2013-07-24'; Set @roleExistEndDate = '2020-12-30';
			Declare @uId Int; Select @uId = Max(USER_ID) From UIUEMS_AD_User; Set @uId = @uId + 1;
			Insert Into UIUEMS_AD_User(User_ID, LogInID, Password, RoleID, RoleExistStartDate, RoleExistEndDate, IsActive, CreatedBy, CreatedDate) Values(@uId, @roll, @roll, 2, @roleExistStartDate, @roleExistEndDate, 1, -1, @createdDate);
			Insert Into UIUEMS_AD_UserInPerson(User_ID, PersonID) Values(@uId, @personId);
		End
		Else
		Begin
			Declare @userInPersonExist Int; Set @userInPersonExist = Null;
			Select @userInPersonExist = User_ID From UIUEMS_AD_UserInPerson Where User_ID = @userExist;
			If @userInPersonExist Is Null
			Begin
				Insert Into UIUEMS_AD_UserInPerson(User_ID, PersonID) Values(@userExist, @personId);
			End
			Set @userInPersonExist = Null;
		End
		Set @studentId = Null; Set @roll = Null; Set @personId = Null; Set @userExist = Null;
		Fetch Next From StudentList Into @studentId, @roll, @personId;
	End
End


GO
/****** Object:  StoredProcedure [dbo].[rptAttendanceSheet]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptAttendanceSheet]
(
@AcaCal_SectionID int = null
)

AS
BEGIN
Select S.Roll,P.FirstName from UIUEMS_ER_Person as P,UIUEMS_ER_Student as S
where P.PersonID in
(
Select PersonID from UIUEMS_ER_Student 
where StudentID in
(
Select StudentID from UIUEMS_CC_Student_CourseHistory as CH
where CH.AcaCalSectionID=@AcaCal_SectionID
)
)
and P.PersonID=S.PersonID
END


GO
/****** Object:  StoredProcedure [dbo].[rptClassRoutineByAcaCalIDAndProgramID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptClassRoutineByAcaCalIDAndProgramID] 
	-- Add the parameters for the stored procedure here
	@AcaCalID int = null, 
	@ProgramID int = null
	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT        
		
	(Select FormalCode From UIUEMS_CC_Course as C where C.CourseID = ACS.CourseID AND C.VersionID = ACS.VersionID)  as CourseCode, 
	(Select Title From UIUEMS_CC_Course as C where C.CourseID = ACS.CourseID AND C.VersionID = ACS.VersionID)  as Title,
	(Select Credits From UIUEMS_CC_Course as C where C.CourseID = ACS.CourseID AND C.VersionID = ACS.VersionID)  as Credit,
	
	SectionName, 
			
			(CASE
			WHEN((DayOne Is Null OR DayOne = 0) AND (DayTwo IS Null OR DayTwo = 0))

			THEN
			''

			WHEN   ((DayOne Is Null OR DayOne = 0) AND (DayTwo IS NOT Null OR DayTwo != 0))
			
			THEN 
			ISNULL((select ValueName from UIUEMS_ER_Value where ValueID =  DayTwo),'')

			WHEN   ((DayOne Is not Null OR DayOne ! = 0) AND (DayTwo IS Null OR DayOne = 0))
			
			THEN 
			ISNULL((select ValueName from UIUEMS_ER_Value where ValueID =  DayOne),'')
			 
			WHEN ((DayOne Is not Null OR DayOne ! = 0) AND (DayTwo IS not Null OR DayTwo !=0))
			
			THEN
			ISNULL((select ValueName from UIUEMS_ER_Value where ValueID =  DayOne),'') + '  &  ' +
			ISNULL((select ValueName from UIUEMS_ER_Value where ValueID =  DayTwo),'') 
			
			END) as Day,
			
			
			( select ((convert( varchar, TSP.StartHour) +':'+ convert(varchar,TSP.StartMin)) +'  '+
				(CASE 
					WHEN TSP.StartAMPM = 1 then 'AM' 
					WHEN TSP.StartAMPM = 2 then 'PM' 
					END)+' - '+
				(convert( varchar, TSP.EndHour) +':'+ convert(varchar,TSP.EndMin)) +'  '+
				(CASE 
					WHEN TSP.EndAMPM = 1 then 'AM' 
					WHEN TSP.EndAMPM = 2 then 'PM' 
					END)) from UIUEMS_CC_TimeSlotPlan as TSP
					where TSP.TimeSlotPlanID = ACS.TimeSlotPlanOneID
					) as Time,

					(CASE 
			WHEN ((RoomInfoOneID Is Null OR RoomInfoOneID  = 0) AND (RoomInfoTwoID IS Null OR RoomInfoTwoID  = 0))
			
			THEN
			''

			WHEN   ((RoomInfoOneID Is Null OR RoomInfoOneID = 0) AND (RoomInfoTwoID IS NOT Null OR RoomInfoTwoID ! = 0))
			
			THEN 
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  RoomInfoTwoID),'')

			WHEN   ((RoomInfoOneID Is not Null OR RoomInfoOneID ! = 0) AND (RoomInfoTwoID IS Null OR RoomInfoTwoID = 0))
			
			THEN 
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  RoomInfoOneID),'')
			
			WHEN ((RoomInfoOneID Is not Null OR RoomInfoOneID ! = 0) AND (RoomInfoTwoID IS not Null OR RoomInfoTwoID ! = 0))
			
			THEN
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  RoomInfoOneID),'') + '  &  ' +
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  RoomInfoTwoID),'') 
						
			END) as Room,
		
			 
			(CASE 
				WHEN ((TeacherOneID IS NULL OR TeacherOneID  = 0) AND (TeacherTwoID IS NULL OR TeacherTwoID = 0))
				THEN
				''

				WHEN ((TeacherOneID IS NULL OR TeacherOneID = 0) AND (TeacherTwoID IS NOT NULL OR TeacherTwoID ! = 0))	
				
				THEN
				ISNULL((select Code from UIUEMS_CC_Employee	where TeacherID = 	TeacherTwoID),'')

				WHEN  ((TeacherOneID IS NOT NULL OR TeacherOneID != 0) AND (TeacherTwoID  IS NULL OR TeacherTwoID = 0))
				
				THEN
				ISNULL((select Code from UIUEMS_CC_Employee	where TeacherID = 	TeacherOneID),'')
								
				WHEN ((TeacherOneID IS NOT NULL OR TeacherOneID ! = 0) AND (TeacherTwoID IS NOT NULL OR TeacherTwoID != 0))

				THEN
				ISNULL((select Code from UIUEMS_CC_Employee	where TeacherID = 	TeacherOneID),'') + '  &  ' + 
				ISNULL((select Code from UIUEMS_CC_Employee	where TeacherID = 	TeacherTwoID),'')
								
				END) as Faculty,		 
	 
	 (select ShortName from UIUEMS_CC_Program where ProgramID = @ProgramID) as Program,
	  
	
		(CASE 
					
			WHEN ((ShareProgIDOne Is NULL OR ShareProgIDOne = 0) AND (ShareProgIDTwo Is NULL OR ShareProgIDTwo = 0) AND (ShareProgIDThree Is NULL OR ShareProgIDThree = 0))
			
			THEN
			''

			WHEN ((ShareProgIDOne IS Null OR ShareProgIDOne = 0) AND (ShareProgIDTwo IS Null OR ShareProgIDTwo = 0) AND (ShareProgIDThree IS NOT NULL OR ShareProgIDThree ! = 0))
			
			THEN
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDThree),'')

			WHEN ((ShareProgIDOne Is Null OR ShareProgIDOne = 0) AND (ShareProgIDTwo IS not Null OR ShareProgIDTwo != 0) AND (ShareProgIDThree IS NOT NULL OR ShareProgIDThree ! = 0))
			
			THEN
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDTwo),'') + '  &  ' +
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDThree),'')

			WHEN ((ShareProgIDOne Is not Null OR ShareProgIDOne ! = 0) AND (ShareProgIDTwo IS Null OR ShareProgIDTwo = 0) AND (ShareProgIDThree IS NULL OR ShareProgIDThree = 0))
			
			THEN
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDOne),'')

			WHEN ((ShareProgIDOne Is not Null OR ShareProgIDOne != 0) AND (ShareProgIDTwo IS Null OR ShareProgIDTwo = 0) AND (ShareProgIDThree IS NOT NULL OR ShareProgIDThree ! = 0))
			
			THEN
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDOne),'') + '  &  ' +
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDThree),'')

			WHEN ((ShareProgIDOne Is not Null OR ShareProgIDOne != 0) AND (ShareProgIDTwo IS not Null OR ShareProgIDTwo != 0) AND (ShareProgIDThree IS NULL OR ShareProgIDThree = 0))
			
			THEN
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDOne),'') + '  &  ' +
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDTwo),'') 

			WHEN ((ShareProgIDOne Is not Null OR ShareProgIDOne != 0) AND (ShareProgIDTwo IS not Null OR ShareProgIDTwo != 0) AND (ShareProgIDThree IS NOT NULL OR ShareProgIDThree != 0))
			
			THEN
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDOne),'') + '  &  ' +
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDTwo),'') + '  &  ' +
			ISNULL((select ShortName from UIUEMS_CC_Program where ProgramID =  ShareProgIDThree),'')
						
			END) as SharedPrograms
								
			FROM    UIUEMS_CC_AcademicCalenderSection as ACS
			Where AcademicCalenderID = @AcaCalID AND ProgramID = @ProgramID

END







GO
/****** Object:  StoredProcedure [dbo].[rptGetCourseTeacherInfo]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptGetCourseTeacherInfo]
(
@AcaCal_SectionID int = null
)

AS
BEGIN
 Select p.FirstName as TeacherName,e.Code  from  (select * from (Select a1.TeacherOneID as TeacherID , a1.AcaCal_SectionID from UIUEMS_CC_AcademicCalenderSection as a1
 union
 Select a2.TeacherTwoID, a2.AcaCal_SectionID from UIUEMS_CC_AcademicCalenderSection as a2) as t
 where t.AcaCal_SectionID=@AcaCal_SectionID) as t2 inner join UIUEMS_CC_Employee as e on t2.TeacherID = e.TeacherID
 inner join UIUEMS_ER_Person as p on p.personid = e.personid
END


GO
/****** Object:  StoredProcedure [dbo].[rptStudentCGPAList]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptStudentCGPAList] 
	-- Add the parameters for the stored procedure here
	@StudentRoll nvarchar(20) 
	
	
AS
BEGIN
	
	SET NOCOUNT ON;


Select 
	ac.BatchCode as Semester,
	sd.Credit as Credit,
	Convert(numeric(18,2), sd.GPA) as GPA,
	Convert(numeric(18,2), sd.CGPA) as CGPA
	 
From UIUEMS_ER_Student_ACUDetail as sd Inner Join
UIUEMS_ER_Student as st
On st.StudentID = sd.StudentID
Inner Join UIUEMS_CC_AcademicCalender as ac
On ac.AcademicCalenderID = sd.StdAcademicCalenderID
Where st.Roll = @StudentRoll

END






GO
/****** Object:  StoredProcedure [dbo].[rptStudentClassRoutine]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptStudentClassRoutine] 
	-- Add the parameters for the stored procedure here
	@Roll nvarchar(50) = null, 
	@AcaCalID int = null
	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	CREATE TABLE dbo.Temp
    (
        CourseTitle   nvarchar(150)  NULL, 
        FormalCode    nvarchar(50) NULL,
		Section nvarchar(50) NULL,
		Day nvarchar(50) NULL,
		Room nvarchar(50) NULL,
		Time nvarchar(50) NULL
    );


Declare StudentClassRoutine Cursor for
Select 
c.Title as CourseTitle,
c.FormalCode as FormalCode,
acs.SectionName as Section,
ISNULL((select ValueName from UIUEMS_ER_Value where ValueID =  acs.DayOne),'') as DayOne,

ISNULL((select ValueName from UIUEMS_ER_Value where ValueID =  acs.DayTwo),'') as DayTwo,

(CASE 
			WHEN ((acs.RoomInfoOneID Is Null OR acs.RoomInfoOneID  = 0) AND (acs.RoomInfoTwoID IS Null OR acs.RoomInfoTwoID  = 0))
			
			THEN
			''

			WHEN   ((acs.RoomInfoOneID Is Null OR acs.RoomInfoOneID = 0) AND (acs.RoomInfoTwoID IS NOT Null OR acs.RoomInfoTwoID ! = 0))
			
			THEN 
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  acs.RoomInfoTwoID),'')

			WHEN   ((acs.RoomInfoOneID Is not Null OR acs.RoomInfoOneID ! = 0) AND (acs.RoomInfoTwoID IS Null OR acs.RoomInfoTwoID = 0))
			
			THEN 
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  acs.RoomInfoOneID),'')
			
			WHEN ((acs.RoomInfoOneID Is not Null OR acs.RoomInfoOneID ! = 0) AND (acs.RoomInfoTwoID IS not Null OR acs.RoomInfoTwoID ! = 0))
			
			THEN
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  acs.RoomInfoOneID),'') + '  &  ' +
			ISNULL((select RoomNumber from UIUEMS_CC_RoomInformation where RoomInfoID =  acs.RoomInfoTwoID),'') 
						
			END) as Room,

( select ((convert( varchar, TSP.StartHour) +':'+ convert(varchar,TSP.StartMin)) +'  '+
				(CASE 
					WHEN TSP.StartAMPM = 1 then 'AM' 
					WHEN TSP.StartAMPM = 2 then 'PM' 
					END)+' - '+
				(convert( varchar, TSP.EndHour) +':'+ convert(varchar,TSP.EndMin)) +'  '+
				(CASE 
					WHEN TSP.EndAMPM = 1 then 'AM' 
					WHEN TSP.EndAMPM = 2 then 'PM' 
					END)) from UIUEMS_CC_TimeSlotPlan as TSP
					where TSP.TimeSlotPlanID = acs.TimeSlotPlanOneID
					) as Time

 from UIUEMS_CC_Student_CourseHistory as ch
 Inner Join UIUEMS_CC_Course as c
 On c.CourseID = ch.CourseID AND c.VersionID = ch.VersionID
 Inner Join UIUEMS_ER_Student as s
 On s.StudentID = ch.StudentID
 Inner Join UIUEMS_CC_AcademicCalenderSection as acs
 On acs.AcaCal_SectionID = ch.AcaCalSectionID
  
 Where s.Roll = @Roll AND ch.AcaCalID = @AcaCalID;
 

 Declare 
	@CourseTitle nvarchar(150),
	@FormalCode nvarchar(50),
	@Section nvarchar(50),
	@DayOne nvarchar(50),
	@DayTwo nvarchar(50),
	@Room nvarchar(50),
	@Time nvarchar(50)

open StudentClassRoutine
	fetch next from StudentClassRoutine into @CourseTitle, @FormalCode, @Section, @DayOne, @DayTwo, @Room, @Time  
	--Fetch 1st ROW
			while @@FETCH_STATUS = 0
			BEGIN
			
			if (@DayOne IS NOT NULL OR @DayOne != 0)
		INSERT INTO [dbo].[Temp]
           ([CourseTitle]
           ,[FormalCode]
           ,[Section]
           ,[Day]
           ,[Room]
           ,[Time])
           
     VALUES
           (@CourseTitle
           ,@FormalCode
           ,@Section
           ,@DayOne
           ,@Room
           ,@Time)


		   if (@DayTwo != '')
		INSERT INTO [dbo].[Temp]
           ([CourseTitle]
           ,[FormalCode]
           ,[Section]
           ,[Day]
           ,[Room]
           ,[Time])
           
     VALUES
           (@CourseTitle
           ,@FormalCode
           ,@Section
           ,@DayTwo
           ,@Room
           ,@Time) 

			fetch next from StudentClassRoutine into @CourseTitle, @FormalCode, @Section, @DayOne, @DayTwo, @Room, @Time  
			END
			close StudentClassRoutine
			deallocate StudentClassRoutine
			Select * from Temp
			DROP TABLE dbo.Temp;

END
































 



GO
/****** Object:  StoredProcedure [dbo].[rptStudentMajorDefine]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptStudentMajorDefine] 
	-- Add the parameters for the stored procedure here
	@BatchId int = 0,
	@ProgramId int = 0,
	@roll nvarchar(50) = null
AS
BEGIN
	
	SET NOCOUNT ON;

	Declare @query nvarchar(1000),  @flag int;

	set @query = '	SELECT  s.StudentID, ISNULL( p.FirstName,'''') + ISNULL(p.LastName,'''') as Name, s.Roll, SUM(ch.CompletedCredit)as completedCr, 
	s.Major1NodeID, n1.Name as Major1Name, n2.Name as Major2Name,  s.Major2NodeID 
	FROM UIUEMS_ER_Student as s 
	INNER JOIN UIUEMS_ER_Admission as a on a.StudentID=s.StudentID
	INNER JOIN UIUEMS_CC_Student_CourseHistory as ch on ch.StudentID=s.StudentID
	INNER JOIN UIUEMS_ER_Person as p on p.PersonID = s.PersonID
	left outer JOIN UIUEMS_CC_Node as n1 on n1.NodeID = s.Major1NodeID
	left outer JOIN UIUEMS_CC_Node as n2 on n2.NodeID = s.Major2NodeID ';
	set @flag = 0;


	IF(@ProgramId != 0 and @flag = 0)
	BEGIN
		set @query +=  ' Where s.ProgramID = ' + Cast(@ProgramId as nvarchar(100))  ;
		set @flag = 1;
	END
	ELSE IF(@ProgramId != 0 and @flag = 1)
	BEGIN
		set @query +=  ' and s.ProgramID = ' + Cast(@ProgramId as nvarchar(100)) ;
		set @flag = 1;
	END

	if(@BatchId != 0 and @flag = 0)
	BEGIN
		set @query +=  ' WHERE a.AdmissionCalenderID = ' + Cast(@BatchId as nvarchar(100))  ;
		set @flag = 1;
	END	
	ELSE IF(@BatchId != 0 and @flag = 1)
	BEGIN
		set @query +=  ' and a.AdmissionCalenderID = ' + Cast(@BatchId as nvarchar(100)) ;
		set @flag = 1;
	END
	
	IF(@roll != '' and @flag = 0)
	BEGIN
		set @query +=  ' Where s.Roll like  ''' + Cast(@roll as nvarchar(50)) + '%''' ;
		set @flag = 1;
	END

	ELSE IF(@roll != '' and @flag = 1)
	BEGIN
		set @query +=  ' and s.Roll like  ''' + Cast(@roll as nvarchar(50)) + '%''' ;
		set @flag = 1;
	END


	set @query += '  and ch.CourseStatusID in(11, 10, 6, 7)  and s.IsActive = 1
					GROUP BY s.StudentID, s.Roll, s.Major1NodeID,s.Major2NodeID,n1.Name, n2.Name  ,ISNULL( p.FirstName,'''') + ISNULL(p.LastName,'''') '

	
print @query
Exec sp_executesql @query

END


GO
/****** Object:  StoredProcedure [dbo].[rptStudentResultHistory]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptStudentResultHistory] 
	-- Add the parameters for the stored procedure here
	@StudentRoll nvarchar(20) 
	
	
AS
BEGIN
	
	SET NOCOUNT ON;


Select 
	 
	AC.BatchCode as BatchCode,
	C.FormalCode as Course,
	C.Title as CourseName,
	C.Credits as Credit,
	CH.ObtainedGPA as GPA,
	CH.IsConsiderGPA as IsConsiderGPA,
	CH.ObtainedGrade as Grade
	

From UIUEMS_CC_Student_CourseHistory as CH Inner Join
UIUEMS_ER_Student as S
On S.StudentID = CH.StudentID
Inner Join UIUEMS_CC_Course as C
On C.CourseID = CH.CourseID AND C.VersionID = CH.VersionID
Inner Join UIUEMS_CC_AcademicCalender as AC
On AC.AcademicCalenderID = CH.AcaCalID
Where S.Roll = @StudentRoll  AND CH.ObtainedGrade IS NOT NULL
Order By AC.BatchCode

END






GO
/****** Object:  StoredProcedure [dbo].[rptStudentRoadmap]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptStudentRoadmap] 
	-- Add the parameters for the stored procedure here
	@StudentRoll nvarchar(20) 
	
	
AS
BEGIN
	
	SET NOCOUNT ON;


Select rw.CalendarDetailName as Semester, 
rw.FormalCode as FormalCode,
rw.CourseTitle,
rw.Credits as CreditHr,
rw.ObtainedGrade as Grade
from UIUEMS_CC_RegistrationWorksheet as rw
Inner Join UIUEMS_ER_Student as s
On rw.StudentID = s.StudentID
Inner Join UIUEMS_CC_TreeCalendarDetail as tcd
On tcd.TreeCalendarDetailID = rw.TreeCalendarDetailID
Inner Join UIUEMS_CC_CalenderUnitDistribution as cud
On cud.CalenderUnitDistributionID = tcd.CalendarDetailID
Where s.Roll = @StudentRoll
Order By cud.CalenderUnitDistributionID;

END














GO
/****** Object:  StoredProcedure [dbo].[rptStudentRoadmapLatest]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[rptStudentRoadmapLatest] 
	-- Add the parameters for the stored procedure here
	@StudentRoll nvarchar(20) 
	
	
AS
BEGIN
	
	SET NOCOUNT ON;

--CREATE TABLE RoadMap
--    (
--        CourseID   int NULL, 
--        VersionID  int NULL,
--		NodeID int NULL,
--		NodeName nvarchar(50) NULL,
--		Priority int NULL,
--		Grade nvarchar(50) NULL,
--		SemesterName nvarchar(50) NULL,
--		FormalCode nvarchar(50) NULL,
--		CourseTitle nvarchar(100) NULL,
--		CreditHr nvarchar(50) NULL,
--		SemesterID int NULL


--    );
 Delete FROM [dbo].[RoadMap]
Declare StudentRoadMap Cursor for


Select 
ccpn.CourseID,
ccpn.VersionID,
ccpn.NodeID,
ccpn.NodeLinkName,
ccpn.TreeCalendarDetailID,
ccpn.Priority
from UIUEMS_CC_Cal_Course_Prog_Node as ccpn
Inner Join UIUEMS_CC_TreeCalendarDetail as tcd
On ccpn.TreeCalendarDetailID = tcd.TreeCalendarDetailID
Where OfferedByProgramID = 	
				(Select S.ProgramID from 
					UIUEMS_ER_Student as S
					Where  S.Roll = @StudentRoll)
					AND 
					tcd.TreeCalendarMasterID = (Select TreeCalendarMasterID from UIUEMS_ER_Student Where Roll = @StudentRoll)
					--Order By Priority

Declare 
	@CourseID int,
	@VersionID int,
	@NodeID int,
	@NodeName nvarchar(50),
	@TreeCalendarDetailID int,
	@Priority int,
	@Grade nvarchar(50),
	@FormalCode nvarchar(50),
	@CourseTitle nvarchar(50),
	@SemesterName nvarchar(50),
	@count int,
	@CreditHr nvarchar(50),
	@SemesterID int;
	
	
open StudentRoadMap

	fetch next from StudentRoadMap into @CourseID, @VersionID, @NodeID, @NodeName, @TreeCalendarDetailID, @Priority 
		--Fetch 1st ROW
				while @@FETCH_STATUS = 0
				BEGIN
				Set @FormalCode = NULL; Set @CourseTitle = NULL; Set @SemesterName = NULL; Set @Grade = NULL; Set @CreditHr = NULL;
							If (@NodeName Is Null)
							
							BEGIN
							
							Select
							@FormalCode = rw.FormalCode,
							@CourseTitle = rw.CourseTitle,
							@SemesterName = rw.CalendarDetailName,
							@Grade = rw.ObtainedGrade,
							@CreditHr = rw.Credits,
							@SemesterID = cud.CalenderUnitDistributionID
							From UIUEMS_CC_RegistrationWorksheet as rw
							Inner Join UIUEMS_ER_Student as s
							on rw.StudentID = s.StudentID
							Inner Join UIUEMS_CC_TreeCalendarDetail as tcd
							On tcd.TreeCalendarDetailID = rw.TreeCalendarDetailID
							Inner Join UIUEMS_CC_CalenderUnitDistribution as cud
							On cud.CalenderUnitDistributionID = tcd.CalendarDetailID
							Where s.Roll = @StudentRoll AND rw.CourseID = @CourseID AND rw.VersionID = @VersionID And rw.Priority = @Priority  AND rw.TreeCalendarDetailID = @TreeCalendarDetailID
							--If  @Grade is not null
							--BEGIN
								INSERT INTO [dbo].[RoadMap]
							   ([CourseID]
							   ,[VersionID]
							   ,[NodeID]
							   ,[NodeName]
							   ,[Priority]
							   ,[Grade]
							   ,[SemesterName]
							   ,[FormalCode]
							   ,[CourseTitle]
							   ,[CreditHr]
							   ,[SemesterID])
           
								 VALUES
									   (@CourseID
									   ,@VersionID
									   ,@NodeID
									   ,@NodeName
									   ,@Priority
									   ,@Grade
									   ,@SemesterName
									   ,@FormalCode
									   ,@CourseTitle
									   ,@CreditHr
									   ,@SemesterID)
							--END

							END
							
							
							ELSE

								BEGIN
									Select @count = COUNT(rw.ObtainedGrade)
									
									 From UIUEMS_CC_RegistrationWorksheet as rw
									Inner Join UIUEMS_ER_Student as s
									on rw.StudentID = s.StudentID
									Where s.Roll = @StudentRoll  AND rw.Priority = @Priority AND rw.ObtainedGrade Is Not Null AND rw.TreeCalendarDetailID = @TreeCalendarDetailID
							
									
									If(@count >0)
										BEGIN
											Declare StudentNodeGrade Cursor for
											Select
											rw.NodeLinkName,
											rw.FormalCode,
											rw.CourseTitle,
											rw.CalendarDetailName,
											rw.ObtainedGrade,
											rw.Credits,
											cud.CalenderUnitDistributionID
											From UIUEMS_CC_RegistrationWorksheet as rw
											Inner Join UIUEMS_ER_Student as s
											on rw.StudentID = s.StudentID
											Inner Join UIUEMS_CC_TreeCalendarDetail as tcd
											On tcd.TreeCalendarDetailID = rw.TreeCalendarDetailID
											Inner Join UIUEMS_CC_CalenderUnitDistribution as cud
											On cud.CalenderUnitDistributionID = tcd.CalendarDetailID
											Where s.Roll = @StudentRoll  AND rw.Priority = @Priority AND rw.ObtainedGrade Is Not Null AND rw.TreeCalendarDetailID = @TreeCalendarDetailID
											open StudentNodeGrade

												fetch next from StudentNodeGrade into @NodeName, @FormalCode, @CourseTitle, @SemesterName, @Grade, @CreditHr, @SemesterID 
												while @@FETCH_STATUS = 0
												BEGIN 
											
														INSERT INTO [dbo].[RoadMap]
														   ([CourseID]
														   ,[VersionID]
														   ,[NodeID]
														   ,[NodeName]
														   ,[Priority]
														   ,[Grade]
														   ,[SemesterName]
														   ,[FormalCode]
														   ,[CourseTitle]
														   ,[CreditHr]
														   ,[SemesterID])
           
															 VALUES
																   (@CourseID
																   ,@VersionID
																   ,@NodeID
																   ,@NodeName
																   ,@Priority
																   ,@Grade
																   ,@SemesterName
																   ,@FormalCode
																   ,@CourseTitle
																   ,@CreditHr
																   ,@SemesterID)

														fetch next from StudentNodeGrade into @NodeName, @FormalCode, @CourseTitle, @SemesterName, @Grade, @CreditHr, @SemesterID
													END
													close StudentNodeGrade
													deallocate StudentNodeGrade
												END
										

									ELSE 
										BEGIN

											Select
											@SemesterName =  rw.CalendarDetailName,
											@CreditHr = rw.Credits,
											@SemesterID = cud.CalenderUnitDistributionID
											From UIUEMS_CC_RegistrationWorksheet as rw
											Inner Join UIUEMS_ER_Student as s
											on rw.StudentID = s.StudentID
											Inner Join UIUEMS_CC_TreeCalendarDetail as tcd
											On tcd.TreeCalendarDetailID = rw.TreeCalendarDetailID
											Inner Join UIUEMS_CC_CalenderUnitDistribution as cud
											On cud.CalenderUnitDistributionID = tcd.CalendarDetailID
											Where s.Roll = @StudentRoll  AND rw.Priority = @Priority AND rw.ObtainedGrade Is Null AND rw.TreeCalendarDetailID = @TreeCalendarDetailID
											
											INSERT INTO [dbo].[RoadMap]
											   ([CourseID]
											   ,[VersionID]
											   ,[NodeID]
											   ,[NodeName]
											   ,[Priority]
											   ,[Grade]
											   ,[SemesterName]
											   ,[FormalCode]
											   ,[CourseTitle]
											   ,[CreditHr]
											   ,[SemesterID])
           
												 VALUES
													   (@CourseID
													   ,@VersionID
													   ,@NodeID
													   ,@NodeName
													   ,@Priority
													   ,@Grade
													   ,@SemesterName
													   ,@FormalCode
													   ,@NodeName
													   ,@CreditHr
													   ,@SemesterID)
										END
								END

						
					 
				fetch next from StudentRoadMap into @CourseID, @VersionID, @NodeID, @NodeName, @TreeCalendarDetailID, @Priority
				
				END
close StudentRoadMap
deallocate StudentRoadMap
Select * from RoadMap

Order By Priority

END










GO
/****** Object:  StoredProcedure [dbo].[sp_AdmitCandidate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Ashraf>
-- Create date: <25-06-2011>
-- Description:	<>
-- Note: 
-- =============================================
CREATE PROCEDURE [dbo].[sp_AdmitCandidate] 

	@CANDIDATE_ID int

	,@SubmittedPapersID int 
	,@PersonID int 
	,@CandidateID int 
	,@SSC_C bit 
	,@SSC_M bit 
	,@SSC_T bit 
	,@HSC_C bit 
	,@HSC_M bit 
	,@HSC_T bit 
	,@Bachelor_C bit 
	,@Bachelor_M bit 
	,@Bachelor_T bit 
	,@Masters_C bit 
	,@Masters_M bit 
	,@Masters_T bit 
	,@Photo bit 
	,@CreatedBy int 
	,@CreatedDate datetime 
	,@ModifiedBy int 
	,@ModifiedDate datetime 
	
AS

DECLARE @PersonID_Local int, @StudentID_Local int

BEGIN

select @PersonID_Local = max(personID)+1 from UIUEMS_ER_Person;

--Insert data into person Table
	INSERT INTO [UIUEMS_ER_Person]
           ([PersonID]
           ,[Prefix]
           ,[FirstName]
		   ,[Phone]
           ,[Gender]
           ,[IsActive]
           ,[CreatedBy]
           ,[CreatedDate]           
           ,[Remarks]
		   ,[CandidateID])
		 
		   (SELECT
			@PersonID_Local
		  ,[CANDIDATE_PREFIX]
		  ,[CANDIDATE_FNAME]      
		  ,[CANDIDATE_PHONE]
		  ,[CANDIDATE_GENDER]
		  ,1 AS 'IsActive'
		  ,1 AS 'CreatedBy'
		  ,getdate() AS 'CreatedDate'
		  ,'System Entry' AS 'Remarks'
		  ,[CANDIDATE_ID]
		  FROM [UIUEMS_ER_CANDIDATE] where CANDIDATE_ID =  @CANDIDATE_ID )

--insert data into SubmittedPapers table
INSERT INTO [UIUEMS_ER_SubmittedPapers]
           ([SubmittedPapersID]
           ,[PersonID]
           ,[CandidateID]
           ,[SSC_C]
           ,[SSC_M]
           ,[SSC_T]
           ,[HSC_C]
           ,[HSC_M]
           ,[HSC_T]
           ,[Bachelor_C]
           ,[Bachelor_M]
           ,[Bachelor_T]
           ,[Masters_C]
           ,[Masters_M]
           ,[Masters_T]
           ,[Photo]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
     VALUES
           (
			@SubmittedPapersID  
			,@PersonID  
			,@CandidateID  
			,@SSC_C  
			,@SSC_M  
			,@SSC_T  
			,@HSC_C  
			,@HSC_M  
			,@HSC_T  
			,@Bachelor_C  
			,@Bachelor_M  
			,@Bachelor_T  
			,@Masters_C  
			,@Masters_M  
			,@Masters_T  
			,@Photo  
			,@CreatedBy  
			,@CreatedDate  
			,@ModifiedBy  
			,@ModifiedDate 
			)

END






GO
/****** Object:  StoredProcedure [dbo].[sp_AllCourseByNode]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Ashraf>
-- Create date: <26,4,2011>
-- Description:	<select course info by all types of nodeID>
-- =============================================
CREATE PROCEDURE [dbo].[sp_AllCourseByNode] 
	
	@NodeId int
AS
BEGIN
SET NOCOUNT ON;

------------------------------------------------------------------------------------------------
--Select all course from virtual node	
	IF EXISTS(select IsVirtual from UIUEMS_CC_Node where IsVirtual = 1 and NodeID = @NodeId)
		BEGIN 
			WITH Tree (NodeID, OperandNodeID)
				AS
				(
					SELECT NodeID, OperandNodeID
					FROM UIUEMS_CC_VNodeSet WHERE NodeID = @NodeId

					UNION ALL

					SELECT UIUEMS_CC_TreeDetail.ParentNodeID, UIUEMS_CC_TreeDetail.ChildNodeID
						 	
					FROM UIUEMS_CC_TreeDetail Inner JOIN Tree 
					ON UIUEMS_CC_TreeDetail.ParentNodeID = Tree.OperandNodeID 
				)
				SELECT Tree.NodeID, Tree.OperandNodeID, N.NodeID, N.Name as 'NodeName', NC.CourseID, NC.VersionID,
						NC.Node_CourseID, C.FormalCode, C.VersionCode, C.Title as 'CourseTitle'

				FROM Tree INNER JOIN UIUEMS_CC_Node as N
					ON Tree.OperandNodeID = N.NodeID inner join UIUEMS_CC_Node_Course as NC
					ON NC.NodeID = N.NodeID inner join UIUEMS_CC_Course as C
					ON C.CourseID = NC.CourseID and C.VersionID = NC.VersionID

					Order by Tree.NodeID, OperandNodeID,C.FormalCode, C.VersionCode
		END
------------------------------------------------------------------------------------------------
--Select all course from non leaf node	
	ELSE IF EXISTS(select IsLastLevel from UIUEMS_CC_Node where IsVirtual = 0 and IsLastLevel = 0 and NodeID = @NodeId)
		BEGIN

				WITH Tree (ParentNodeID, ChildNodeID)
				AS
				(
					SELECT ParentNodeID, ChildNodeID
					FROM UIUEMS_CC_TreeDetail WHERE ParentNodeID = @NodeId

					UNION ALL

					SELECT UIUEMS_CC_TreeDetail.ParentNodeID, UIUEMS_CC_TreeDetail.ChildNodeID
						 	
					FROM UIUEMS_CC_TreeDetail Inner JOIN Tree 
					ON Tree.ChildNodeID = UIUEMS_CC_TreeDetail.ParentNodeID  
				)
				SELECT ParentNodeID, ChildNodeID, N.NodeID, N.Name as 'NodeName', NC.CourseID, NC.VersionID,
						NC.Node_CourseID, C.FormalCode, C.VersionCode, C.Title as 'CourseTitle'

				from Tree INNER JOIN UIUEMS_CC_Node as N

				ON Tree.ChildNodeID = N.NodeID inner join UIUEMS_CC_Node_Course as NC
				ON NC.NodeID = N.NodeID inner join UIUEMS_CC_Course as C
				ON C.CourseID = NC.CourseID and C.VersionID = NC.VersionID

				Order by ParentNodeID, ChildNodeID,C.FormalCode, C.VersionCode
		END
------------------------------------------------------------------------------------------------
--Select all course from leaf node	
	ELSE
		BEGIN

			select  N.NodeID, N.Name as 'NodeName', NC.CourseID, NC.VersionID, NC.Node_CourseID, 
					C.FormalCode, C.VersionCode, C.Title as 'CourseTitle' 

					from UIUEMS_CC_Node AS N INNER JOIN 
					UIUEMS_CC_Node_Course AS NC ON N.NodeID = NC.NodeID inner join 
					UIUEMS_CC_Course as C ON NC.CourseID = C.CourseID and NC.VersionID = C.VersionID
						where N.NodeID = @NodeId
					Order by N.NodeID, C.FormalCode, C.VersionCode			
		END
END










GO
/****** Object:  StoredProcedure [dbo].[SP_CALCULATE_CGPA]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CALCULATE_CGPA]
--ThIS value will be spplied from the application to the server
 @STUDENTID INT,
 @ACC_CAL_ID INT,
 @USER_ID INT
--@STUDENT_ROLL NVARCHAR(15)
AS
BEGIN
	
	DECLARE 
		--These variables are local variables
		@CourseId INT,
		@VersionID INT,
		@GradeID INT,
		@Grade NVARCHAR (50),
		@GradePoint NUMERIC,
		@IsConsideredGPA BIT,
		@Credits NUMERIC(18,2),
		@TOTAL_CREDITS DECIMAL,
		@TOTAL_GRADE_POINT DECIMAL,
		@TERM_GPA NUMERIC(18,2),
		@CGPA NUMERIC(18,2)

	-- ***********TEMPORARY PUPOSE************
	--SET @STUDENT_ROLL = '111102001'
	--SET @ACC_CAL_ID = 8
	--SET @ 

	--Since studentid is int and used as foreignkey in Course_histotyTable
	--So, Get the appropiate StudentID of that spplied StudentRoll
--	DECLARE @STUDENT_ID INT
--
--	SET @STUDENT_ID = ( SELECT StudentID 
--						FROM UIUEMS_ER_Student
--						WHERE Roll = @STUDENT_ROLL
--					   )

	--Make sure that all the courses of that curent trimesmer is selected
	--DECLARE COURSE_HISTORY_CURSOR CURSOR 
	--	FOR
	--		SELECT CourseID, GradeID, IsConsiderGPA 
	--		FROM UIUEMS_CC_Student_CourseHistory
	--		WHERE StudentId = @STUDENT_ID 
	--			AND AcaCalID = @ACC_CAL_ID
	--		
	--	
	--	OPEN COURSE_HISTORY_CURSOR
	--		FETCH NEXT FROM COURSE_HISTORY_CURSOR
	--		
	--		WHILE @@FETCH_STATUS = 0
	--		FETCH NEXT FROM COURSE_HISTORY_CURSOR
	--	CLOSE COURSE_HISTORY_CURSOR
	--	DEALLOCATE COURSE_HISTORY_CURSOR


	-- Now select all the courses whose GPA will be calcu;lated

	DECLARE GPA_CONSIDERABLE_COURSES_CURSOR CURSOR 
		FOR
			SELECT CourseID,VersionID, GradeID, IsConsiderGPA 
			FROM UIUEMS_CC_Student_CourseHistory
			WHERE StudentId = @STUDENTID 
				AND IsConsiderGPA = 1 --this 1 stands as boolen 1
		
		OPEN GPA_CONSIDERABLE_COURSES_CURSOR
			FETCH NEXT FROM GPA_CONSIDERABLE_COURSES_CURSOR
			INTO @CourseId, @VersionID, @GradeID, @IsConsideredGPA
			
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SELECT @CourseId ID , @VersionID Version, @GradeID Grade, 
					@IsConsideredGPA ConsiderderGPA
				INSERT INTO temp_term_gpa_calculation 
					(ID, VERSION,GRADEID,ISCONSIDER_GPA)
				VALUES (@CourseId, @VersionID, @GradeID, @IsConsideredGPA)
				FETCH NEXT FROM GPA_CONSIDERABLE_COURSES_CURSOR
				--Populate CourseIds, VersionId, GradeId and gradeIds
					INTO @CourseId, @VersionID, @GradeID, @IsConsideredGPA
				
				-- get the credit by courseid and version id 
				-- and update teh temporary table
								SELECT @Credits = COURSE.CREDITS 
								FROM temp_term_gpa_calculation AS TEMP,UIUEMS_CC_Course AS COURSE
								WHERE TEMP.ID = COURSE.COURSEID AND
										TEMP.VERSION = COURSE.VERSIONID
					UPDATE temp_term_gpa_calculation SET 
						CREDITS = @Credits
				-- Now get the Grade and GradePoint by gradeid 
				-- to update the temporaray table
							SELECT @Grade = g.grade,@GradePoint = g.gradepoint
							FROM temp_term_gpa_calculation AS temp,  
								UIUEMS_CC_GradeDetails AS g
							WHERE g.gradeid = temp.gradeid
					UPDATE temp_term_gpa_calculation SET
						GRADE = @Grade,
						GRADEPOINT = @GradePoint  
			
				--Now take all the GradeID and Count the Total gradepoints
			END
			
		CLOSE GPA_CONSIDERABLE_COURSES_CURSOR
		DEALLOCATE GPA_CONSIDERABLE_COURSES_CURSOR
		
		--Now calculate the total cretis and gradepoints 
		-- from the temporary table 
		SELECT @Credits = SUM(credits),@GradePoint = SUM(GRADEPOINT)
			FROM temp_term_gpa_calculation

		SET @CGPA = @GradePoint/@Credits
		UPDATE UIUEMS_ER_Std_AcademicCalender SET
			CGPA = @CGPA,
			TotalCreditsComleted = @Credits,
			ModifiedBy = @USER_ID,
				ModifiedDate = GETDATE()
			WHERE StudentID = @STUDENTID AND
				  AcademicCalenderID = @ACC_CAL_ID
		TRUNCATE TABLE temp_term_gpa_calculation
		--SELECT @TERM_GPA,@Credits
END

    



GO
/****** Object:  StoredProcedure [dbo].[SP_CALCULATE_TERM_GPA]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_CALCULATE_TERM_GPA]
--These two values will be spplied from the application to the server
 @STUDENTID INT,
 @ACC_CAL_ID INT,
 @USER_ID INT
AS
BEGIN
	
	DECLARE 
		--These variables are local variables
		@CourseId INT,
		@VersionID INT,
		@GradeID INT,
		@Grade NVARCHAR (50),
		@GradePoint NUMERIC,
		@IsConsideredGPA BIT,
		@Credits NUMERIC(18,2),
		@TOTAL_CREDITS DECIMAL,
		@TOTAL_GRADE_POINT NUMERIC(18,2),
		@TERM_GPA NUMERIC(18,2),
		@NUMBER_OF_ROWS INT

	-- ***********TEMPORARY PUPOSE************
--	SET @STUDENT_ROLL = '111102001'
--	SET @ACC_CAL_ID = 8
	--SET @ 

	--Since studentid is int and used as foreignkey in Course_histotyTable
	--So, Get the appropiate StudentID of that spplied StudentRoll
--	DECLARE @STUDENT_ID INT
--
--	SET @STUDENT_ID = ( SELECT StudentID 
--						FROM UIUEMS_ER_Student
--						WHERE Roll = @STUDENT_ROLL
--					   )

	-- Now select all the courses whose GPA will be calculated
	DECLARE GPA_CONSIDERABLE_COURSES_CURSOR CURSOR 
		FOR
			SELECT CourseID,VersionID, GradeID, IsConsiderGPA 
			FROM UIUEMS_CC_Student_CourseHistory
			WHERE StudentId = @STUDENTID 
				AND AcaCalID = @ACC_CAL_ID
				AND IsConsiderGPA = 1 --this 1 stands as boolen 1
		
		OPEN GPA_CONSIDERABLE_COURSES_CURSOR
			FETCH NEXT FROM GPA_CONSIDERABLE_COURSES_CURSOR
			INTO @CourseId, @VersionID, @GradeID, @IsConsideredGPA
			
			WHILE @@FETCH_STATUS = 0
			BEGIN
				SELECT @CourseId ID , @VersionID Version, @GradeID Grade, 
					@IsConsideredGPA ConsiderderGPA
				--Populate CourseIds, VersionId, GradeId and gradeIds
					
				INSERT INTO temp_term_gpa_calculation 
					(ID, VERSION,GRADEID,ISCONSIDER_GPA)
				VALUES (@CourseId, @VersionID, @GradeID, @IsConsideredGPA)
				FETCH NEXT FROM GPA_CONSIDERABLE_COURSES_CURSOR
					INTO @CourseId, @VersionID, @GradeID, @IsConsideredGPA
				-- get the credit by courseid and version id 
				-- and update the temporary table
								SELECT @Credits = COURSE.CREDITS 
								FROM temp_term_gpa_calculation AS TEMP,
									 UIUEMS_CC_Course AS COURSE
								WHERE TEMP.ID = COURSE.COURSEID AND
										TEMP.VERSION = COURSE.VERSIONID
					UPDATE temp_term_gpa_calculation SET 
						CREDITS = @Credits
						
				-- Now get the Grade and GradePoint by gradeid 
				-- to update the temporaray table
							SELECT @Grade = g.grade,@GradePoint = g.gradepoint
							FROM temp_term_gpa_calculation AS temp,  
								UIUEMS_CC_GradeDetails AS g
							WHERE g.gradeid = temp.gradeid
					UPDATE temp_term_gpa_calculation SET
						GRADE = @Grade,
						GRADEPOINT = @GradePoint
						
				-- Now set the total grade points achived in a particular course
				-- Multiply the credits with gradepoint
				SELECT @TOTAL_GRADE_POINT = CREDITS*GRADEPOINT
					FROM temp_term_gpa_calculation
				UPDATE temp_term_gpa_calculation SET
					totalPointintheCourse = @TOTAL_GRADE_POINT
				--Now take all the GradeID and Count the Total gradepoints
			END
			
		CLOSE GPA_CONSIDERABLE_COURSES_CURSOR
		DEALLOCATE GPA_CONSIDERABLE_COURSES_CURSOR
		
		--Now calculate the total cretis and gradepoints 
		-- from the temporary table 
		SELECT @Credits = SUM(credits),@GradePoint = SUM(totalPointintheCourse)
			FROM temp_term_gpa_calculation

		SET @TERM_GPA = @GradePoint/@Credits;
		
		SELECT @NUMBER_OF_ROWS =  COUNT(*)
								FROM UIUEMS_ER_Std_AcademicCalender
								WHERE StudentID = @STUDENTID AND
								AcademicCalenderID = @ACC_CAL_ID
		  
		-- Now check whether a row exists in UIU_ER_STD_AcademicCalender table
		-- of that student for that particular trimester
		-- IF count becomes 0 then insert a new row
		-- else undate the existing row
		IF(@NUMBER_OF_ROWS=0)
			BEGIN 
				INSERT INTO UIUEMS_ER_Std_AcademicCalender 
				(StudentID,AcademicCalenderID,GPA,TotalCreditsPerCalender,CreatedBy,CreatedDate)
				VALUES(@STUDENTID,@ACC_CAL_ID,@TERM_GPA,@Credits,@USER_ID,GETDATE())
			SELECT	@NUMBER_OF_ROWS = @@ROWCOUNT
			END
		ELSE
			BEGIN
				UPDATE UIUEMS_ER_Std_AcademicCalender SET
				GPA = @TERM_GPA,
				TotalCreditsPerCalender = @Credits,
				ModifiedBy = @USER_ID,
				ModifiedDate = GETDATE()
				WHERE StudentID = @STUDENTID AND
					  AcademicCalenderID = @ACC_CAL_ID
				SELECT @NUMBER_OF_ROWS = @@ROWCOUNT
			END
		
		-- Now Remove the data from the temopary table
		TRUNCATE TABLE temp_term_gpa_calculation	
		
		-- return the calculated term GPA and Total credits taken 
		--in that calendar year
		SELECT @NUMBER_OF_ROWS
END

    



GO
/****** Object:  StoredProcedure [dbo].[sp_DoAutoAssign]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---- =============================================
---- Author:		<Saima>
---- Create date: <Create Date,,>
---- Description:	<Get student specific CalCourseProgNodes rows using Student_CalCourseProgNode>
---- =============================================
CREATE PROCEDURE [dbo].[sp_DoAutoAssign]
----	 Add the parame@StuId int,
@v_treemasterID int, @v_stdID int, @v_progID int, @q money output, @cg money output
AS
BEGIN
----	 SET NOCOUNT ON added to prevent extra result sets from
----	 interfering with SELECT statements.
	SET NOCOUNT ON;

--    -- delete existing student specifiq data
--	delete from dbo.UIUEMS_CC_Student_CalCourseProgNode where StudentID = @stdID
    
--	-- prepare worksheet
--	EXEC	dbo.sp_InsertWorksheet2
--			@v_stdID, @v_treemasterID

	-- get student
	declare @cgpa money
	set @cgpa = 0
	select @cgpa = CGPA from dbo.UIUEMS_ER_Student_ACUDetail where StdACUDetailID = (select max(StdACUDetailID) from UIUEMS_ER_Student_ACUDetail where StudentID = @v_stdID)
	
	-- get dept parameter setup - mancredits by the std's cgpa
	declare @ManCGPA1 money
			,@ManCredit1 money
			,@ManRetakeGradeLimit1 nvarchar
			,@ManCGPA2 money
			,@ManCredit2 money
			,@ManRetakeGradeLimit2 nvarchar
			,@ManCGPA3 money
			,@ManCredit3 money
			,@ManRetakeGradeLimit3 nvarchar
			,@MaxCGPA1 money
			,@MaxCredit1 money
			,@MaxCGPA2 money
			,@MaxCredit2 money
			,@MaxCGPA3 money
			,@MaxCredit3 money
			-- local variables
--			,@v_ManCredits1 money
--			,@v_ManCgpa1 money
--			,@v_ManCredits2 money
--			,@v_ManCgpa2 money
			,@v_ManCredits money
			,@v_ManCgpa3 money 

		declare crsr cursor local static for( 
			select ManCGPA1,ManCredit1,ManRetakeGradeLimit1,ManCGPA2,ManCredit2,ManRetakeGradeLimit2,ManCGPA3,ManCredit3,ManRetakeGradeLimit3,MaxCGPA1,MaxCredit1,MaxCGPA2,MaxCredit2,MaxCGPA3,MaxCredit3 
			from dbo.UIUEMS_RG_DeptRegSetUp where ProgramID = @v_progID)
		open crsr

			fetch crsr into @ManCGPA1,@ManCredit1,@ManRetakeGradeLimit1,@ManCGPA2,@ManCredit2,@ManRetakeGradeLimit2,@ManCGPA3,@ManCredit3,@ManRetakeGradeLimit3,@MaxCGPA1,@MaxCredit1,@MaxCGPA2,@MaxCredit2,@MaxCGPA3,@MaxCredit3
			while(@@fetch_status = 0)
			begin
				fetch crsr into @ManCGPA1,@ManCredit1,@ManRetakeGradeLimit1,@ManCGPA2,@ManCredit2,@ManRetakeGradeLimit2,@ManCGPA3,@ManCredit3,@ManRetakeGradeLimit3,@MaxCGPA1,@MaxCredit1,@MaxCGPA2,@MaxCredit2,@MaxCGPA3,@MaxCredit3		
			end
	 
		close crsr
		deallocate crsr

		if(@ManCGPA1 is not null and @cgpa >= @ManCGPA1)
		begin
			set @v_ManCredits = @ManCredit1
		end
		else if (@ManCGPA2 is not null and @cgpa >= @ManCGPA2)
		begin
			set @v_ManCredits = @ManCredit2
		end
		else if (@ManCGPA3 is not null and @cgpa >= @ManCGPA3)
		begin
			set @v_ManCredits = @ManCredit3
		end
		else 
		begin
			set @v_ManCredits = 6 ---- default value. generally it will be used for new student or who got 0 cgpa
		end

		declare
			@CalCourseProgNodeID int 
			,@IsCompleted bit 
			,@OriginalCalID int 
			,@IsAutoAssign bit 
			,@IsAutoOpen bit 
			,@Isrequisitioned bit 
			,@IsMandatory bit 
			,@IsManualOpen bit 
			,@TreeCalendarDetailID int
			,@TreeCalendarMasterID int 
			,@TreeMasterID int 
			,@CalendarMasterName varchar(250) 
			,@CalendarDetailName varchar(250) 
			,@FormalCode varchar(250) 
			,@VersionCode varchar(250) 
			,@CourseTitle varchar(250) 
			,@NodeLinkName varchar(250)
			,@Priority int 
			,@RetakeNo int 
			,@ObtainedGPA numeric(18, 2) 
			,@ObtainedGrade varchar(150) 
			,@AcaCalYear int 
			,@BatchCode varchar(50) 
			,@AcaCalTypeName varchar(50) 

		declare stdCcpnCrsr cursor LOCAL FAST_FORWARD for (select CalCourseProgNodeID,IsCompleted  
																,OriginalCalID,IsAutoAssign,IsAutoOpen  
																,Isrequisitioned,IsMandatory,IsManualOpen  
																,TreeCalendarDetailID,TreeCalendarMasterID,TreeMasterID  
																,CalendarMasterName,CalendarDetailName,FormalCode  
																,VersionCode,CourseTitle,NodeLinkName,Priority  
																,RetakeNo,ObtainedGPA,ObtainedGrade,AcaCalYear  
																,BatchCode,AcaCalTypeName 
															from dbo.UIUEMS_CC_Student_CalCourseProgNode 
															where StudentID = @v_stdID)
		open stdCcpnCrsr
		close stdCcpnCrsr
		deallocate stdCcpnCrsr
set @q = @v_ManCredits
set @cg = @cgpa
--return @q
END





GO
/****** Object:  StoredProcedure [dbo].[sp_DoAutoAssign1]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

---- =============================================
---- Author:		<Saima>
---- submission date: June 07, 2011
---- Description:	<Auto Assign Courses for registration>
----				steps are as followed
----					1.load result into worksheet
----					2. get students current CGPA
----					3. get dept parameter setup - mancredits by the std's cgpa
----					4. get mandetory credits in order to do auto assign
----					5. auto assigning
----			here, auto assign is not done for NodeID
---- =============================================
CREATE PROCEDURE [dbo].[sp_DoAutoAssign1]
----	 Add the parame@StuId int,
@v_stdID int, @return int= 0, @v_progID int, @q money output, @cg money output, @chkPrereq nvarchar(128) output
AS
BEGIN
----	 SET NOCOUNT ON added to prevent extra result sets from
----	 interfering with SELECT statements.
	SET NOCOUNT ON;

	---- 1.load result into worksheet
			EXEC	dbo.sp_LoadResultInWorkSheet1
					@v_stdID, @return output

	---- 2. get students current CGPA
	declare @cgpa money
	set @cgpa = 0
	select @cgpa = CGPA from dbo.UIUEMS_ER_Student_ACUDetail where StdACUDetailID = (select max(StdACUDetailID) from UIUEMS_ER_Student_ACUDetail where StudentID = @v_stdID)
	
	---- 3. get dept parameter setup - mancredits by the std's cgpa
	declare @ManCGPA1 money
			,@ManCredit1 money
			,@ManRetakeGradeLimit1 nvarchar
			,@ManCGPA2 money
			,@ManCredit2 money
			,@ManRetakeGradeLimit2 nvarchar
			,@ManCGPA3 money
			,@ManCredit3 money
			,@ManRetakeGradeLimit3 nvarchar
			,@MaxCGPA1 money
			,@MaxCredit1 money
			,@MaxCGPA2 money
			,@MaxCredit2 money
			,@MaxCGPA3 money
			,@MaxCredit3 money
			-- local variables
--			,@v_ManCredits1 money
--			,@v_ManCgpa1 money
--			,@v_ManCredits2 money
--			,@v_ManCgpa2 money
			,@v_ManCredits money
			,@v_ManCgpa3 money 

		declare crsr cursor local static for( 
			select ManCGPA1,ManCredit1,ManRetakeGradeLimit1,ManCGPA2,ManCredit2,ManRetakeGradeLimit2,ManCGPA3,ManCredit3,ManRetakeGradeLimit3,MaxCGPA1,MaxCredit1,MaxCGPA2,MaxCredit2,MaxCGPA3,MaxCredit3 
			from dbo.UIUEMS_RG_DeptRegSetUp where ProgramID = @v_progID)
		open crsr

			fetch crsr into @ManCGPA1,@ManCredit1,@ManRetakeGradeLimit1,@ManCGPA2,@ManCredit2,@ManRetakeGradeLimit2,@ManCGPA3,@ManCredit3,@ManRetakeGradeLimit3,@MaxCGPA1,@MaxCredit1,@MaxCGPA2,@MaxCredit2,@MaxCGPA3,@MaxCredit3
			while(@@fetch_status = 0)
			begin
				fetch crsr into @ManCGPA1,@ManCredit1,@ManRetakeGradeLimit1,@ManCGPA2,@ManCredit2,@ManRetakeGradeLimit2,@ManCGPA3,@ManCredit3,@ManRetakeGradeLimit3,@MaxCGPA1,@MaxCredit1,@MaxCGPA2,@MaxCredit2,@MaxCGPA3,@MaxCredit3		
			end
	 
		close crsr
		deallocate crsr
		---- 4. get mandetory credits in order to do auto assign
		if(@ManCGPA1 is not null and @cgpa >= @ManCGPA1)
		begin
			set @v_ManCredits = @ManCredit1
		end
		else if (@ManCGPA2 is not null and @cgpa >= @ManCGPA2)
		begin
			set @v_ManCredits = @ManCredit2
		end
		else if (@ManCGPA3 is not null and @cgpa >= @ManCGPA3)
		begin
			set @v_ManCredits = @ManCredit3
		end
		else 
		begin
			set @v_ManCredits = 6 ---- default value. generally it will be used for new student or who got 0 cgpa
		end			

		---- 5. auto assigning
		declare
			--@CalCourseProgNodeID int 
			--,@IsCompleted bit 
			--,@OriginalCalID int ,
			@IsAutoAssign bit 
			,@IsAutoOpen bit 
			--,@Isrequisitioned bit 
			,@IsMandatory bit 
			,@IsManualOpen bit
			,@CourseID int
			,@VersionID int 
			,@Node_CourseID int
			,@NodeID int 
--			,@TreeCalendarDetailID int
--			,@TreeCalendarMasterID int 
--			,@TreeMasterID int 
--			,@CalendarMasterName varchar(250) 
--			,@CalendarDetailName varchar(250) 
--			,@FormalCode varchar(250) 
--			,@VersionCode varchar(250) 
--			,@CourseTitle varchar(250) 
--			,@NodeLinkName varchar(250)
			,@Priority int 
--			,@RetakeNo int 
--			,@ObtainedGPA numeric(18, 2) 
			,@ObtainedGrade varchar(150) 
--			,@AcaCalYear int 
--			,@BatchCode varchar(50) 
--			,@AcaCalTypeName varchar(50)

--			@count int
			,@TotlaAllocatedCredits money
			,@v_NotApplicablePreReqPriority int
					
		set @TotlaAllocatedCredits = 0
		set @v_NotApplicablePreReqPriority = 0 -- used to maintain if a course is not auto assigned
		while(@TotlaAllocatedCredits < @v_ManCredits)
			begin--1
				set @Priority = null
				set @CourseID =null
				set @VersionID =null
				set @Node_CourseID =null
				set @NodeID =null

				set @Priority = (select min(Priority) from UIUEMS_CC_Student_CalCourseProgNode 
								where  IsAutoAssign = 'false' and IsCompleted = 'false' and Priority > @v_NotApplicablePreReqPriority)
				select @CourseID = CourseID,@VersionID = VersionID,@Node_CourseID = Node_CourseID,@NodeID = NodeID
				from UIUEMS_CC_Student_CalCourseProgNode where Priority = @Priority
				
--				if(@NodeID is null)
--					begin--2
--
--							declare @ret int
--							EXEC @ret = CheckPrerequisit @NCrsId = @Node_CourseID,@NId = @NodeID,@progID = @v_progID
--															
--							if @ret = 1
--							begin--5
--								update UIUEMS_CC_Student_CalCourseProgNode set IsAutoAssign = 'true'
--												where Node_CourseID = @Node_CourseID
--								
--											set @TotlaAllocatedCredits = @TotlaAllocatedCredits + (select Credits from UIUEMS_CC_Student_CalCourseProgNode
--																			where Node_CourseID = @Node_CourseID)
--											set @v_NotApplicablePreReqPriority = 0
--
--							end--5
--							else set @v_NotApplicablePreReqPriority = @Priority
--					end		--2	
					declare @ret int
							EXEC @ret = CheckPrerequisit @NCrsId = @Node_CourseID,@NId = @NodeID,@progID = @v_progID
															
							if @ret = 1
							begin--5
								update UIUEMS_CC_Student_CalCourseProgNode set IsAutoAssign = 'true'
												where Priority = @Priority  and StudentID = @v_stdID
								
											set @TotlaAllocatedCredits = @TotlaAllocatedCredits + (select Credits from UIUEMS_CC_Student_CalCourseProgNode
																			where Priority = @Priority and StudentID = @v_stdID)
											set @v_NotApplicablePreReqPriority = 0

							end--5
							else set @v_NotApplicablePreReqPriority = @Priority
										
			end--1
		
set @q = @v_ManCredits
set @cg = @TotlaAllocatedCredits
--return @q
END












GO
/****** Object:  StoredProcedure [dbo].[SP_GET_REGISTERED_STUDENT_OF_TRIMESTER]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GET_REGISTERED_STUDENT_OF_TRIMESTER]
@BATCHID INT,
@ACACALID INT,
@PROGRAMID INT
AS 
BEGIN
	-- GET THE BATCH CODE FROM THE BATCHID
--	DECLARE @BATCHCODE CHAR(3)
--	SELECT @BATCHCODE =  BatchCode 
--					 FROM UIUEMS_CC_AcademicCalender
--					 WHERE AcademicCalenderID = @BATCHID
	DECLARE @STUDENTID INT
	--GET ALL THE STUDENTS OF THE PARTICULAR TRIMESTER AND 
	--SAVE THEM IN A TEMPORARY TABLE
	DECLARE CURSOR_REGISTERED_STUDENT_OF_TRIMESTER CURSOR FOR
		SELECT DISTINCT STUDENTID,ACACALID 
		FROM UIUEMS_CC_Student_CourseHistory
		WHERE ACACALID = @ACACALID 
		OPEN CURSOR_REGISTERED_STUDENT_OF_TRIMESTER
			FETCH NEXT FROM CURSOR_REGISTERED_STUDENT_OF_TRIMESTER
				INTO @STUDENTID,@ACACALID
			WHILE @@FETCH_STATUS = 0
				BEGIN
					-- SAVE THE VALUES IN THE TEMPORARY TABLE
					INSERT INTO Temp_Registered_Student (STUDENTID,ACACALID)
					VALUES(@STUDENTID,@ACACALID)
					FETCH NEXT FROM CURSOR_REGISTERED_STUDENT_OF_TRIMESTER
						INTO @STUDENTID,@ACACALID
					
					
				END
		CLOSE CURSOR_REGISTERED_STUDENT_OF_TRIMESTER
	DEALLOCATE CURSOR_REGISTERED_STUDENT_OF_TRIMESTER

	-- NOW SELECT STUDENTS OF A BATCH FROM THAT TEMPORARY TABLE 
	-- AND STORE THOSE STUDENTS INTO ANOTHER TEMPORARY TABLE
		DECLARE CURSOR_REGISTERED_STUDENT_OF_TRIMESTER_ CURSOR FOR
		SELECT STUDENTID,ACACALID 
		FROM Temp_Registered_Student
		WHERE ACACALID = @BATCHID 
		OPEN CURSOR_REGISTERED_STUDENT_OF_TRIMESTER_
			FETCH NEXT FROM CURSOR_REGISTERED_STUDENT_OF_TRIMESTER_
				INTO @STUDENTID,@ACACALID
			WHILE @@FETCH_STATUS = 0
				BEGIN
					-- SAVE THE VALUES IN THE TEMPORARY TABLE
					INSERT INTO temp_reg_student_of_batch (STUDENTID,ACACALID)
					VALUES(@STUDENTID,@ACACALID)

					FETCH NEXT FROM CURSOR_REGISTERED_STUDENT_OF_TRIMESTER_
						INTO @STUDENTID,@ACACALID
					
				END
		CLOSE CURSOR_REGISTERED_STUDENT_OF_TRIMESTER_
	DEALLOCATE CURSOR_REGISTERED_STUDENT_OF_TRIMESTER_

	-- NOW USE THAT TEMPORARRY TABLE TO GET THE NAMES OF THE SUDENT 
	--OF A PARTICULAR program
	Select SP.StudentID,SP.StudentRoll,SP.Name as StudentName 
	from VIEW_STUDENT_PROGRAM SP, temp_reg_student_of_batch as TEMP
	where SP.StudentID = TEMP.StudentID and 
		  SP.ProgramID = @ProgramID
	TRUNCATE TABLE Temp_Registered_Student
	TRUNCATE TABLE temp_reg_student_of_batch
END
--SELECT * FROM VIEW_STUDENT_PROGRAM



GO
/****** Object:  StoredProcedure [dbo].[SP_GET_STUDENT_GPA_CGPA]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_GET_STUDENT_GPA_CGPA]
 @STUDENT_ID INT,
 @ACACALID_REG INT
AS
BEGIN
	SELECT  RESULT.STUDENTID,STUDENT.ROLL,STUDENT.NAME AS StudentName,
			RESULT.GPA,RESULT.TotalCreditsPerCalender,
			RESULT.CGPA,RESULT.TotalCreditsComleted
	FROM UIUEMS_ER_Std_AcademicCalender AS RESULT,
		 VIEW_STUDENT_PERSON AS STUDENT
	WHERE RESULT.STUDENTID = @STUDENT_ID AND
		  RESULT.StudentID = STUDENT.StudentID AND
		  RESULT.AcademicCalenderID = @ACACALID_REG
END

--EXEC SP_COLUMNS UIUEMS_ER_Std_AcademicCalender
--SELECT * FROM VIEW_STUDENT_PERSON
--EXEC SP_COLUMNS SP_GET_STUDENT_GPA_CGPA



GO
/****** Object:  StoredProcedure [dbo].[sp_GetCalCrsProgNdByStd]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get student specific CalCourseProgNodes rows using Student_CalCourseProgNode>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCalCrsProgNdByStd]
	-- Add the parameters for the stored procedure here
	@stdID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
SELECT     UIUEMS_CC_Cal_Course_Prog_Node.CalCorProgNodeID, UIUEMS_CC_Cal_Course_Prog_Node.OfferedinTrimesterID, 
                      UIUEMS_CC_Cal_Course_Prog_Node.TreeCalendarDetailID, UIUEMS_CC_Cal_Course_Prog_Node.OfferedByProgramID, 
                      UIUEMS_CC_Cal_Course_Prog_Node.CourseID, UIUEMS_CC_Cal_Course_Prog_Node.VersionID, UIUEMS_CC_Cal_Course_Prog_Node.Node_CourseID, 
                      UIUEMS_CC_Cal_Course_Prog_Node.NodeID, UIUEMS_CC_Cal_Course_Prog_Node.NodeLinkName, UIUEMS_CC_Cal_Course_Prog_Node.Priority, 
                      UIUEMS_CC_Cal_Course_Prog_Node.CreatedBy, UIUEMS_CC_Cal_Course_Prog_Node.CreatedDate, UIUEMS_CC_Cal_Course_Prog_Node.ModifiedBy, 
                      UIUEMS_CC_Cal_Course_Prog_Node.ModifiedDate
FROM         UIUEMS_CC_Cal_Course_Prog_Node INNER JOIN
                      UIUEMS_CC_Student_CalCourseProgNode ON 
                      UIUEMS_CC_Cal_Course_Prog_Node.CalCorProgNodeID = UIUEMS_CC_Student_CalCourseProgNode.CalCourseProgNodeID
					  and UIUEMS_CC_Student_CalCourseProgNode.StudentID = @stdID
END




GO
/****** Object:  StoredProcedure [dbo].[sp_GetChildNodesByParent]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get chid nodes from TreeDetail's parent node >
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetChildNodesByParent]
	-- Add the parameters for the stored procedure here
	@nodeID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select node.* from dbo.UIUEMS_CC_Node node, 
		(select * from dbo.UIUEMS_CC_TreeDetail where ParentNodeID = @nodeID)tbl
	where node.NodeID = tbl.ChildNodeID

END




GO
/****** Object:  StoredProcedure [dbo].[sp_GetCourseByNodeCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get course for node_course >
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCourseByNodeCourse]
	-- Add the parameters for the stored procedure here
	@nodeCourseID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from dbo.UIUEMS_CC_Course cs, 
		(select CourseID, VersionID from dbo.UIUEMS_CC_Node_Course where Node_CourseID = @nodeCourseID)tbl
			where cs.CourseID = tbl.CourseID and cs.VersionID = tbl.VersionID
END




GO
/****** Object:  StoredProcedure [dbo].[sp_GetCoursesUnderNode]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get courses through Node_Course table ensuring that the node is last level (in node table) >
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetCoursesUnderNode]
	-- Add the parameters for the stored procedure here
	@nodeID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select cs.* from dbo.UIUEMS_CC_Course cs, 
	(select * from dbo.UIUEMS_CC_Node_Course where NodeID = @nodeID)tbl
	where cs.CourseID=tbl.CourseID and cs.VersionID=tbl.VersionID

END




GO
/****** Object:  StoredProcedure [dbo].[sp_GetNewStdCalCrsProgNd]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get student specific rows for the 1st trimester from Student_CalCourseProgNode>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetNewStdCalCrsProgNd]
	-- Add the parameters for the stored procedure here
	@stdID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
select sccpn.* from UIUEMS_CC_Student_CalCourseProgNode sccpn, 
(select * from UIUEMS_CC_Cal_Course_Prog_Node where TreeCalendarDetailID =
(select Min(TreeCalendarDetailID) from 
(SELECT     UIUEMS_CC_Cal_Course_Prog_Node.* 
FROM         UIUEMS_CC_Cal_Course_Prog_Node INNER JOIN
                      UIUEMS_CC_Student_CalCourseProgNode ON 
                      UIUEMS_CC_Cal_Course_Prog_Node.CalCorProgNodeID = UIUEMS_CC_Student_CalCourseProgNode.CalCourseProgNodeID
					  and UIUEMS_CC_Student_CalCourseProgNode.StudentID = @stdID )tbl ))tb
where sccpn.CalCourseProgNodeID = tb.CalCorProgNodeID and sccpn.StudentID = @stdID
END




GO
/****** Object:  StoredProcedure [dbo].[sp_GetPreReqDetails]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get pre req details>
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetPreReqDetails]
	-- Add the parameters for the stored procedure here
	@nodeID int, @CourseID int, @VersionID int, @progID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
if(@CourseID != -1)
begin
	select * from dbo.UIUEMS_CC_PrerequisiteDetail 
	where PrerequisiteMasterID = (select PrerequisiteMasterID from dbo.UIUEMS_CC_PrerequisiteMaster 
									where NodeCourseID =(select Node_CourseID from dbo.UIUEMS_CC_Node_Course 
															where NodeID = @nodeID and CourseID = @CourseID and VersionID = @VersionID and ProgramID=@progID))
end

else
begin
	select * from dbo.UIUEMS_CC_PrerequisiteDetail 
	where PrerequisiteMasterID = (select PrerequisiteMasterID from dbo.UIUEMS_CC_PrerequisiteMaster 
								where NodeID = @nodeID and ProgramID=@progID)
end
END






GO
/****** Object:  StoredProcedure [dbo].[sp_GetStdCalCrsProgNd]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get student specific rows from Student_CalCourseProgNode priority wise where isComplete is false >
-- =============================================
CREATE PROCEDURE [dbo].[sp_GetStdCalCrsProgNd]
	-- Add the parameters for the stored procedure here
	@stdID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select tbl.[ID]
      ,tbl.[StudentID]
      ,tbl.[CalCourseProgNodeID]
      ,tbl.[IsCompleted]
      ,tbl.[OriginalCalID]
      ,tbl.[IsAutoAssign]
      ,tbl.[IsAutoOpen]
      ,tbl.[Isrequisitioned]
      ,tbl.[IsMandatory]
      ,tbl.[IsManualOpen]
      ,tbl.[CreatedBy]
      ,tbl.[CreatedDate]
      ,tbl.[ModifiedBy]
      ,tbl.[ModifiedDate] from 
(
SELECT [ID]
      ,[StudentID]
      ,[CalCourseProgNodeID]
      ,[IsCompleted]
      ,[OriginalCalID]
      ,[IsAutoAssign]
      ,[IsAutoOpen]
      ,[Isrequisitioned]
      ,[IsMandatory]
      ,[IsManualOpen]
      ,[CreatedBy]
      ,[CreatedDate]
      ,[ModifiedBy]
      ,[ModifiedDate]
  FROM [dbo].[UIUEMS_CC_Student_CalCourseProgNode]
where [IsCompleted] = 'false' and [StudentID] = @stdID
)tbl, dbo.UIUEMS_CC_Cal_Course_Prog_Node ccpn
where tbl.[CalCourseProgNodeID] = ccpn.[CalCorProgNodeID]
order by ccpn.Priority asc

END





GO
/****** Object:  StoredProcedure [dbo].[sp_GetStudentsByProgAdminCalID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[sp_GetStudentsByProgAdminCalID]
@progID int, @adminCalID int
WITH 
EXECUTE AS CALLER
AS
select std.* from UIUEMS_ER_Admission ad, UIUEMS_ER_Student std
where ad.AdmissionCalenderID = @adminCalID and ad.StudentID = std.StudentID and std.ProgramID = @progID



GO
/****** Object:  StoredProcedure [dbo].[sp_ImportStudentCourseHistory]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get courses through Node_Course table ensuring that the node is last level (in node table) >
-- =============================================
CREATE PROCEDURE [dbo].[sp_ImportStudentCourseHistory]
	-- Add the parameters for the stored procedure here
	@nodeID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

			INSERT INTO dbo.UIUEMS_CC_Student_CourseHistory
				   ([StudentID]
				   ,[CalCourseProgNodeID]
				   ,[AcaCalSectionID]
				   ,[RetakeNo]
				   ,[ObtainedTotalMarks]
				   ,[ObtainedGPA]
				   ,[ObtainedGrade]
				   ,[CourseStatusID]
				   ,[CourseStatusDate]
				   ,[AcaCalID]
				   ,[CourseID]
				   ,[VersionID]
				   ,[Node_CourseID]
				   ,[NodeID]
				   ,[CreatedBy]
				   ,[CreatedDate]
				   ,[ModifiedBy]
				   ,[ModifiedDate])
			 (
				select tbl.[StudentID], 
		ccpn.CalCorProgNodeID, 
		null, 
		tbl.[RetakeNo], 
		null, 
		tbl.[GPA], 
		tbl.[Grade],
		tbl.[CourseStatusID],
		null,
		tbl.[AcademicCalenderID],
		tbl.[CourseID],
		tbl.[VersionID],
		tbl.[Node_CourseID],
		null,
		null,
		null,
		null,
		null
		from 
		(
		SELECT stdCr.[Student_CourseID]
			  ,stdCr.[StudentID]
			  ,stdAC.[AcademicCalenderID]
			  ,stdCr.[RetakeNo]
			  ,stdCr.[Node_CourseID]
			  ,Nd.[CourseID]
			  ,Nd.[VersionID]
			  ,stdCs.[CourseStatusID]
			  ,stdCs.[GPA]
			  ,stdCs.[Grade]
		  FROM [dbo].[UIUEMS_ER_Student_Course] stdCr, 
			   [dbo].[UIUEMS_CC_Node_Course] Nd,
			   [dbo].[UIUEMS_ER_Std_AcademicCalender] stdAC,
			   [dbo].[UIUEMS_ER_Std_CourseStatus] stdCs
		where stdCr.[Node_CourseID] = Nd.[Node_CourseID] 
				and stdCr.[StdAcademicCalenderID] = stdAC.[StdAcademicCalenderID] 
				and stdCr.[StudentID] = stdAC.[StudentID]
				and stdCs.[Student_CourseID] = stdCr.[Student_CourseID]
		)tbl, dbo.UIUEMS_CC_Cal_Course_Prog_Node ccpn
		where tbl.[CourseID] = ccpn.CourseID 
			and tbl.[VersionID] = ccpn.VersionID 
			and tbl.[Node_CourseID] = ccpn.Node_CourseID
		)

END





GO
/****** Object:  StoredProcedure [dbo].[sp_InsertIntoStdDiscount]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<insert into dbo.UIUEMS_SM_StdDiscountCurrent, dbo.UIUEMS_SM_StdDiscountHistory >
-- =============================================
CREATE PROCEDURE [dbo].[sp_InsertIntoStdDiscount]
	-- Add the parameters for the stored procedure here
	@admissionID int, 
	@typeDefID int, 
	@typePercentage decimal(18, 2),
	@effectiveAcaCalId int,
	@createdBy int,-- = null, 
	--@createdDate datetime,
	@result int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare 
		@percent decimal(18, 2),
		@acaCalId int 
		set @percent = -1
		set @acaCalId = 0
		set @result = 0
	SET NOCOUNT ON;
	select @percent = TypePercentage--, @acaCalId = EffectiveAcaCalID 
	from dbo.UIUEMS_SM_StdDiscountCurrent
	where AdmissionID = @admissionID and TypeDefID = @typeDefID 

	if @percent != -1-- and @acaCalId != 0
		begin
			if @percent != @typePercentage-- or @acaCalId != @effectiveAcaCalId
				begin
					insert into dbo.UIUEMS_SM_StdDiscountHistory
					([AdmissionID]
					   ,[TypeDefID]
					   ,[TypePercentage]
					   ,[EffectiveAcaCalID]
					   ,[CreatedBy]
					   ,[CreatedDate])
				    VALUES
					   (@admissionID
					   ,@typeDefID
					   ,@percent
					   ,@acaCalId
					   ,@createdBy
					   ,getdate()--@createdDate
					)

					UPDATE [dbo].[UIUEMS_SM_StdDiscountCurrent]
					   SET [TypePercentage] = @typePercentage
						  ,[EffectiveAcaCalID] = @effectiveAcaCalId
						  ,[ModifiedBy] = @createdBy
						  ,[ModifiedDate] = getdate() --@createdDate
					 where AdmissionID = @admissionID and TypeDefID = @typeDefID 
				set @result = 2
				end
		end
	else
		begin
			insert into dbo.UIUEMS_SM_StdDiscountCurrent
					([AdmissionID]
					   ,[TypeDefID]
					   ,[TypePercentage]
					   ,[EffectiveAcaCalID]
					   ,[CreatedBy]
					   ,[CreatedDate])
				    VALUES
					   (@admissionID
					   ,@typeDefID
					   ,@typePercentage
					   ,@effectiveAcaCalId
					   ,@createdBy
					   ,getdate()--@createdDate
					)
			set @result = 1
		end
return @result
END









GO
/****** Object:  StoredProcedure [dbo].[sp_LoadResultInWorkSheet]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<Get student specific CalCourseProgNodes rows using Student_CalCourseProgNode>
-- =============================================
CREATE PROCEDURE [dbo].[sp_LoadResultInWorkSheet]
	-- Add the parameters for the stored procedure here
	@v_stdID int, @v_treemasterID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
    
	-- check that the worksheet of that student exits. if not return
	if not exists (select * from dbo.UIUEMS_CC_Student_CalCourseProgNode where StudentID = @v_stdID)
		return -1

	-- load result
	declare
			@CalCourseProgNodeID int 
			,@IsCompleted bit 
			,@OriginalCalID int  
			,@RetakeNo int 
			,@ObtainedGPA numeric(18, 2) 
			,@ObtainedGrade varchar(150) 
			,@AcaCalYear int 
			,@BatchCode varchar(50) 
			,@AcaCalTypeName varchar(50)
			,@CourseID int
			,@VersionID int 
			,@Node_CourseID int
			,@NodeID int

		declare stdCcpnCrsr cursor LOCAL FAST_FORWARD for (select CalCourseProgNodeID
																,IsCompleted  
																,OriginalCalID
																,RetakeNo
																,ObtainedGPA
																,ObtainedGrade
																,AcaCalYear  
																,BatchCode
																,AcaCalTypeName 
																,CourseID
																,VersionID
																,Node_CourseID
																,NodeID
															from dbo.UIUEMS_CC_Student_CalCourseProgNode 
															where StudentID = @v_stdID)
		open stdCcpnCrsr
			fetch stdCcpnCrsr into @CalCourseProgNodeID 
								,@IsCompleted 
								,@OriginalCalID  
								,@RetakeNo 
								,@ObtainedGPA  
								,@ObtainedGrade  
								,@AcaCalYear 
								,@BatchCode 
								,@AcaCalTypeName 
								,@CourseID 
								,@VersionID 
								,@Node_CourseID 
								,@NodeID 
			while(@@fetch_status = 0)
			begin
				fetch stdCcpnCrsr into @CalCourseProgNodeID 
									,@IsCompleted 
									,@OriginalCalID  
									,@RetakeNo 
									,@ObtainedGPA  
									,@ObtainedGrade  
									,@AcaCalYear 
									,@BatchCode  
									,@AcaCalTypeName
									,@CourseID 
									,@VersionID 
									,@Node_CourseID 
									,@NodeID 

--						if exists (select ) 	
					 
			end
		close stdCcpnCrsr
		deallocate stdCcpnCrsr
END






GO
/****** Object:  StoredProcedure [dbo].[sp_LoadResultInWorkSheet1]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Saima>
-- submission date: <June 07, 2011>
-- Description:	<load results in worksheet (Student_CalCourseProgNode) from UIUEMS_CC_Student_CourseHistory>
--				steps are as followed
--					1. check that the worksheet of that student exits. if not, return from thsi procedure
--					2. load result
--                note : 1. retakeno was not imported yet. the following code written on the basis that retake no is always null
--						 2. equivalent courses was not incorporated
--						 3. results are not loaded for NodeID
--						 4. assumed grade'I' describes iscomplete to true and false for 'W' and 'F'
-- =============================================
CREATE PROCEDURE [dbo].[sp_LoadResultInWorkSheet1]
	-- Add the parameters for the stored procedure here
	@v_stdID int, @return int = 0 output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;    
    
	-- 1. check that the worksheet of that student exits. if not return
	if not exists (select * from dbo.UIUEMS_CC_Student_CalCourseProgNode where StudentID = @v_stdID)
		begin
			set @return = -1
			return @return
		end

	-- 2. load result
	declare
			@CalCourseProgNodeID int 
			,@IsCompleted bit 
			,@OriginalCalID int  
			,@RetakeNo int 
			,@ObtainedGPA numeric(18, 2) 
			,@ObtainedGrade varchar(150) 
			,@AcaCalYear int 
			,@BatchCode varchar(50) 
			,@AcaCalTypeName varchar(50)
			,@CourseID int
			,@VersionID int 
			,@Node_CourseID int
			,@NodeID int

			-- local varables
			,@v_grade  varchar(150)
			,@v_gradeTemp  varchar(150)
			,@v_obGPA numeric(18, 2) 
			,@v_obGPATemp numeric(18, 2)
			,@v_retakeNoTemp int
			,@v_EquiCourseId int
			,@v_EquiVersionId int

		declare stdCcpnCrsr cursor LOCAL FAST_FORWARD for (select CalCourseProgNodeID
																,IsCompleted  
																,OriginalCalID
																,RetakeNo
																,ObtainedGPA
																,ObtainedGrade
																,AcaCalYear  
																,BatchCode
																,AcaCalTypeName 
																,CourseID
																,VersionID
																,Node_CourseID
																,NodeID
															from dbo.UIUEMS_CC_Student_CalCourseProgNode 
															where StudentID = @v_stdID)
		open stdCcpnCrsr
			fetch stdCcpnCrsr into @CalCourseProgNodeID 
								,@IsCompleted 
								,@OriginalCalID  
								,@RetakeNo 
								,@ObtainedGPA  
								,@ObtainedGrade  
								,@AcaCalYear 
								,@BatchCode 
								,@AcaCalTypeName 
								,@CourseID 
								,@VersionID 
								,@Node_CourseID 
								,@NodeID 
			while(@@fetch_status = 0)
			begin
				set @v_grade = ''
				set @v_obGPA = null
				set @v_retakeNoTemp = null
--				set @v_EquiCourseId = 0
--				set @v_EquiVersionId =0

				if(@NodeID is null)
					begin
						set @v_retakeNoTemp = (select count(*) -1 
													from dbo.UIUEMS_CC_Student_CourseHistory
													where StudentID = @v_stdID and Node_CourseID = @Node_CourseID
														 and CourseID = @CourseID and VersionID = @VersionID)
						set @v_obGPA = (select max(ObtainedGPA) 
											from dbo.UIUEMS_CC_Student_CourseHistory
											where StudentID = @v_stdID and Node_CourseID = @Node_CourseID
												 and CourseID = @CourseID and VersionID = @VersionID)
						
						-- gpa can be null  but grade may be not null. it is for I ang W grade
						set @v_grade = (select ObtainedGrade 
										from dbo.UIUEMS_CC_Student_CourseHistory
										where StudentID = @v_stdID and Node_CourseID = @Node_CourseID
											 and CourseID = @CourseID and VersionID = @VersionID
								             and ObtainedGPA = @v_obGPA)

						if (@v_grade is not null)
--									
							update UIUEMS_CC_Student_CalCourseProgNode
														--,OriginalCalID
														set IsCompleted = 'True' 
														,RetakeNo = @v_retakeNoTemp
														,ObtainedGPA = @v_obGPA
														,ObtainedGrade = @v_grade
						where StudentID = @v_stdID and Node_CourseID = @Node_CourseID
						 and CourseID = @CourseID and VersionID = @VersionID
							and (upper(@v_grade) != 'W' or upper(@v_grade) != 'F') -- still assumed grade'I' describes iscomplete to true
					end
--					else -- load result for the presence of node
				fetch stdCcpnCrsr into @CalCourseProgNodeID 
									,@IsCompleted 
									,@OriginalCalID  
									,@RetakeNo 
									,@ObtainedGPA  
									,@ObtainedGrade  
									,@AcaCalYear 
									,@BatchCode  
									,@AcaCalTypeName
									,@CourseID 
									,@VersionID 
									,@Node_CourseID 
									,@NodeID 	
					 
			end
		close stdCcpnCrsr
		deallocate stdCcpnCrsr
set @return = 1
return @return
END












GO
/****** Object:  StoredProcedure [dbo].[sp_Prepare_Std_Crs_Bill_Worksheet]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Ashraf>
-- Create date: <23,07,2011>
-- Description: <prepare bill worksheet for per student>
-- =============================================

CREATE PROCEDURE [dbo].[sp_Prepare_Std_Crs_Bill_Worksheet] 

@StuId int

AS
BEGIN

IF EXISTS(select * from UIUEMS_BL_Std_Crs_Bill_Worksheet where StudentId = @StuId)
BEGIN
	DELETE FROM UIUEMS_BL_Std_Crs_Bill_Worksheet WHERE StudentId = @StuId
END

	-- declare common field
	declare 
			@cm_StudentID int, 
			@cm_CalCourseProgNodeID int, 
			@cm_AcaCalSectionID int, 
			@cm_SectionType int,
			@cm_AcaCalID int, 
			@cm_AdmissionCalenderID int, 
			@cm_CourseID int, 
			@cm_VersionID int,
			@cm_CourseType int, 
			@cm_ProgramID int,
			@cm_RetakeNo int, 
			@cm_ObtainedGrade varchar(2),
			@cm_Remarks varchar(max);

	set @cm_Remarks = 'System input'

	declare 
			@Cursor_cm CURSOR

	-- declare discount field
	declare 
			@dc_TypeDefID int, 
			@dc_TypePercentage decimal(18,2);
	declare 
			@Cursor_dc CURSOR

	-- declare fee setup field
	declare 
			@fs_TypeDefID int, 
			@fs_Amount decimal(18,2);

	declare 
			@Cursor_fs CURSOR

	-- fill common cursor
	set @Cursor_cm = CURSOR For 

		SELECT     
			sch.StudentID, 
			sch.CalCourseProgNodeID, 
			sch.AcaCalSectionID, 
			sec.TypeDefinitionID'SectionType', 
			sch.AcaCalID,
			adm.AdmissionCalenderID, 
			sch.CourseID, 
			sch.VersionID,
			c.TypeDefinitionID'CourseType', 
			p.ProgramID,
			sch.RetakeNo, 
			sch.ObtainedGrade

		FROM			UIUEMS_CC_Student_CourseHistory as sch 
		left outer join UIUEMS_CC_AcademicCalenderSection as sec on sec.AcaCal_SectionID = sch.AcaCalSectionID
		left outer join UIUEMS_ER_Student as s on s.StudentID = sch.StudentID
		left outer join UIUEMS_CC_Program as p on p.ProgramID = s.ProgramID
		left outer join UIUEMS_CC_Course as c on c.CourseID=sch.CourseID and c.VersionID=sch.VersionID
		left outer join UIUEMS_ER_Admission as adm on adm.StudentID = sch.StudentID

		where sch.StudentID = @StuId and sch.AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)
	--

	--Start Batch execution.
		OPEN @Cursor_cm
				fetch NEXT from @Cursor_cm into 
												@cm_StudentID, 
												@cm_CalCourseProgNodeID, 
												@cm_AcaCalSectionID, 
												@cm_SectionType,
												@cm_AcaCalID, 
												@cm_AdmissionCalenderID, 
												@cm_CourseID, 
												@cm_VersionID,
												@cm_CourseType, 
												@cm_ProgramID, 
												@cm_RetakeNo, 
												@cm_ObtainedGrade
				while @@FETCH_STATUS = 0
				BEGIN
					-- discount cursor block
						set @Cursor_dc = CURSOR For
								SELECT	
									sdc.TypeDefID, 
									sdc.TypePercentage
								FROM	UIUEMS_SM_StdDiscountCurrent AS sdc 
										LEFT OUTER JOIN UIUEMS_ER_Admission AS adm ON adm.AdmissionID = sdc.AdmissionID
										WHERE     (adm.StudentID = @StuId)
				
						OPEN @Cursor_dc 
								fetch NEXT from @Cursor_dc into  
															@dc_TypeDefID, 
															@dc_TypePercentage 

								while @@FETCH_STATUS = 0
								BEGIN
									
										INSERT INTO UIUEMS_BL_Std_Crs_Bill_Worksheet
											   (StudentId 
												,CalCourseProgNodeID 
												,AcaCalSectionID 
												,SectionTypeId
												,AcaCalId 
												,CourseId 
												,VersionId 
												,CourseTypeId
												,ProgramId 
												,RetakeNo 
												,PreviousBestGrade 
												,DiscountTypeId 
												,DiscountPercentage
												,Remarks
												,CreatedBy 
												,CreatedDate )
										 VALUES
											   (@cm_StudentID
												,@cm_CalCourseProgNodeID
												,@cm_AcaCalSectionID
												,@cm_SectionType
												,@cm_AcaCalID
												,@cm_CourseID
												,@cm_VersionID
												,@cm_CourseType
												,@cm_ProgramID
												,@cm_RetakeNo
												,@cm_ObtainedGrade
												,@dc_TypeDefID
												,@dc_TypePercentage
												,@cm_Remarks --remarks
												,-2
												,getdate()
												)

									FETCH NEXT from @Cursor_dc into  
																 @dc_TypeDefID, 
																 @dc_TypePercentage

								END
							CLOSE @Cursor_dc
							DEALLOCATE @Cursor_dc
					--discount cursor block END

					--***********************************************************************************************************************************

					--fee setup block
						set @Cursor_fs = CURSOR For						
									SELECT	fs.TypeDefID, 
											fs.Amount									   
									FROM			UIUEMS_ER_Student as s 
									left outer join UIUEMS_CC_Program as p on p.ProgramID = s.ProgramID
									left outer join UIUEMS_ER_Admission as adm on adm.StudentID = s.StudentID
									inner join		UIUEMS_BL_FeeSetup as fs on fs.AcaCalID = adm.AdmissionCalenderID 
																			and	fs.ProgramID = p.ProgramID
									left outer join UIUEMS_AC_TypeDefinition as td on td.TypeDefinitionID = fs.TypeDefID
										where s.StudentID = @StuId and td.Type = 'Fee_PCA';

						OPEN @Cursor_fs 
								fetch NEXT from @Cursor_fs into 
															@fs_TypeDefID,
															@fs_Amount  

								while @@FETCH_STATUS = 0
								BEGIN							
										INSERT INTO UIUEMS_BL_Std_Crs_Bill_Worksheet
											   (StudentId 
												,CalCourseProgNodeID 
												,AcaCalSectionID 
												,SectionTypeId											   
												,AcaCalId 
												,CourseId 
												,VersionId 
												,CourseTypeId											   
												,ProgramId 
												,RetakeNo 
												,PreviousBestGrade											   
												,FeeSetupId 
												,Fee
												,Remarks											   
												,CreatedBy 
												,CreatedDate )
										 VALUES
											   (@cm_StudentID
												,@cm_CalCourseProgNodeID
												,@cm_AcaCalSectionID
												,@cm_SectionType
												,@cm_AcaCalID
												,@cm_CourseID
												,@cm_VersionID
												,@cm_CourseType
												,@cm_ProgramID
												,@cm_RetakeNo
												,@cm_ObtainedGrade
												,@fs_TypeDefID 
												,@fs_Amount
												,@cm_Remarks --remarks
												,-2
												,getdate() )

									FETCH NEXT from @Cursor_fs into  
																 @fs_TypeDefID 
																,@fs_Amount 

								END
							CLOSE @Cursor_fs
							DEALLOCATE @Cursor_fs
					--fee setup block END
					FETCH NEXT from @Cursor_cm into							
												@cm_StudentID, 
												@cm_CalCourseProgNodeID, 
												@cm_AcaCalSectionID, 
												@cm_SectionType, 
												@cm_AcaCalID, 
												@cm_AdmissionCalenderID,
												@cm_CourseID, 
												@cm_VersionID,
												@cm_CourseType, 
												@cm_ProgramID, 
												@cm_RetakeNo, 
												@cm_ObtainedGrade

				END
	CLOSE @Cursor_cm
	DEALLOCATE @Cursor_cm
	--End Batch execution.

END
 







GO
/****** Object:  StoredProcedure [dbo].[sp_PrepareWorksheet]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





-- =============================================
-- Author: <Ashraf>
-- Create date: <11,5,2011>
-- Description: <Prepare Worksheet by student wise>
-- =============================================

CREATE PROCEDURE [dbo].[sp_PrepareWorksheet] 

@StuId int,
@TreemasterID int
--@RowCount1 int output

AS
BEGIN
SET NOCOUNT ON;

DECLARE @RowCount1 int

IF EXISTS(select * from UIUEMS_CC_Student_CalCourseProgNode where StudentID = @StuId)
BEGIN
	Delete from UIUEMS_CC_Student_CalCourseProgNode where StudentID = @StuId
END

INSERT INTO UIUEMS_CC_Student_CalCourseProgNode  
			([StudentID]
			,[CalCourseProgNodeID] 
			,[IsCompleted] 
			,[OriginalCalID] 
			,[IsAutoAssign]
			,[IsAutoOpen]           
			,[Isrequisitioned] 
			,[IsMandatory]   
			,[IsManualOpen] 
			,[CreatedBy] 
			,[CreatedDate] 
			,[ModifiedBy] 
			,[ModifiedDate]
			,[TreeCalendarDetailID] 
			,[TreeCalendarMasterID] 
			,[TreeMasterID] 
			,[CalendarMasterName] 
			,[CalendarDetailName]  
			,[FormalCode] 
			,[VersionCode]  
			,[CourseTitle] 
			,[NodeLinkName] 
			,[Priority]  
			,[RetakeNo]
			,[ObtainedGPA]   
			,[ObtainedGrade]
			,[AcaCalYear]
			,[BatchCode]
			,[AcaCalTypeName]	
			,[CourseID]
			,[VersionID]
			,[Node_CourseID]
			,[NodeID]
			,[ProgramID]
			,[DeptID]
			,[AcademicCalenderID]
			,[IsMultipleACUSpan]
			,[CourseCredit]
			,[CompletedCredit]
			)

(SELECT     
			s.StudentID
			,ccpn.CalCorProgNodeID
			,0
			,null
			,0
			,0
			,0
			,0
			,0
			,1
			,(select getdate()) as 'CreatedDate'
			,null
			,null
			,null
			,null
			,null
			,cum.Name AS 'CalendarMasterName'
			,cud.Name AS 'CalendarDetailName'
			,c.FormalCode
			,c.VersionCode
			,c.Title as 'CourseTitle'
			,ccpn.NodeLinkName
			,ccpn.Priority
			,StuCourseHistory.RetakeNo
			,StuCourseHistory.ObtainedGPA
			,StuCourseHistory.ObtainedGrade
			,StuCourseHistory.AcaCalYear
			,StuCourseHistory.BatchCode 
			,StuCourseHistory.AcaCalTypeName
			,ccpn.CourseID
			,ccpn.VersionID
			,ccpn.Node_CourseID
			,ccpn.NodeID
			,p.ProgramID
			,p.DeptID
			,null  --(select AcademicCalenderID from UIUEMS_CC_AcademicCalender where IsCurrent = 1) AS 'AcademicCalenderID'
			,c.HasMultipleACUSpan
			,c.Credits AS 'CourseCredit'
			,t_cc.CompletedCredit 

FROM         UIUEMS_CC_TreeCalendarMaster AS tcm INNER JOIN
             UIUEMS_CC_TreeCalendarDetail AS tcd ON tcm.TreeCalendarMasterID = tcd.TreeCalendarMasterID INNER JOIN
             UIUEMS_CC_Cal_Course_Prog_Node AS ccpn ON tcd.TreeCalendarDetailID = ccpn.TreeCalendarDetailID INNER JOIN
             UIUEMS_ER_Student AS s ON tcm.TreeMasterID = s.TreeMasterID LEFT OUTER JOIN
             UIUEMS_CC_CalenderUnitMaster AS cum ON tcd.CalendarMasterID = cum.CalenderUnitMasterID LEFT OUTER JOIN
             UIUEMS_CC_CalenderUnitDistribution AS cud ON tcd.CalendarDetailID = cud.CalenderUnitDistributionID LEFT OUTER JOIN
             UIUEMS_CC_Node_Course AS nc ON ccpn.CourseID = nc.CourseID AND ccpn.VersionID = nc.VersionID AND 
             ccpn.Node_CourseID = nc.Node_CourseID LEFT OUTER JOIN
             UIUEMS_CC_Course AS c ON c.CourseID = nc.CourseID AND c.VersionID = nc.VersionID LEFT OUTER JOIN
                          
	(SELECT   ch1.ID, ch1.StudentID, ch1.CalCourseProgNodeID, ch1.AcaCalSectionID, ch1.RetakeNo, 
			  ch1.ObtainedTotalMarks, ch1.ObtainedGPA, ch1.ObtainedGrade, ch1.CourseStatusID, 
			  ch1.CourseStatusDate, ch1.AcaCalID, ch1.CourseID, ch1.VersionID, ch1.Node_CourseID, 
			  ch1.NodeID, ch1.CreatedBy, ch1.CreatedDate, ch1.ModifiedBy, ch1.ModifiedDate,
			  ac.Year as 'AcaCalYear', ac.BatchCode, cut.TypeName as 'AcaCalTypeName'

	FROM      UIUEMS_CC_Student_CourseHistory AS ch1 INNER JOIN
			  (SELECT MAX(ObtainedGPA) AS ObtainedGPA, CourseID, VersionID, Node_CourseID
				FROM UIUEMS_CC_Student_CourseHistory 
			  GROUP BY CourseID, VersionID, Node_CourseID) AS ch2 ON ch2.CourseID = ch1.CourseID 
										AND ch2.VersionID = ch1.VersionID 
										AND ch2.Node_CourseID = ch1.Node_CourseID 
										AND ch2.ObtainedGPA = ch1.ObtainedGPA left outer join
			  UIUEMS_CC_AcademicCalender as ac ON ch1.AcaCalID = ac.AcademicCalenderID left outer join
			  UIUEMS_CC_CalenderUnitType as cut ON ac.CalenderUnitTypeID = cut.CalenderUnitTypeID) AS StuCourseHistory ON 
			  StuCourseHistory.CalCourseProgNodeID = ccpn.CalCorProgNodeID AND StuCourseHistory.StudentID = s.StudentID left outer join
			  dbo.UIUEMS_CC_Program as p ON p.ProgramID = s.ProgramID left outer join 
			  dbo.UIUEMS_CC_Node AS n ON ccpn.NodeID = n.NodeID left outer join
(select CourseID , VersionID, sum(CompletedCredit)as 'CompletedCredit' from UIUEMS_CC_Student_CourseHistory
group by CourseID , VersionID) as t_cc on c.CourseID = t_cc.CourseID and c.VersionID = t_cc.VersionID

WHERE     (s.StudentID = @StuId) AND (s.TreeMasterID = @TreemasterID))

SELECT @RowCount1 = @@ROWCOUNT
SELECT @RowCount1 AS Table1


END























GO
/****** Object:  StoredProcedure [dbo].[sp_Registration]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Ashraf>
-- Create date: <18,6,2011>
-- Description: <transfer data from UIUEMS_CC_Student_CalCourseProgNode to UIUEMS_CC_Student_CourseHistory,
-- This is the final registration task.>
-- =============================================

CREATE PROCEDURE [dbo].[sp_Registration] 

@StuId int,
@ModifiedBy int

AS
BEGIN
SET NOCOUNT ON;

DECLARE @RowCount int

IF EXISTS(select * from UIUEMS_CC_Student_CourseHistory where StudentID = @StuId)
BEGIN
	DELETE FROM UIUEMS_CC_Student_CourseHistory   WHERE StudentID= @StuId
END

INSERT INTO UIUEMS_CC_Student_CourseHistory
           (StudentID
           ,CalCourseProgNodeID
           ,AcaCalSectionID
           ,RetakeNo
--		   ,ObtainedTotalMarks
           ,ObtainedGPA
           ,ObtainedGrade
--		   ,CourseStatusID
--		   ,CourseStatusDate
           ,AcaCalID
           ,CourseID
           ,VersionID
           ,Node_CourseID
           ,NodeID
           ,IsMultipleACUSpan
           ,CourseCredit
           ,CompletedCredit		   
           ,ModifiedBy
           ,ModifiedDate
           )     
    (SELECT 
		StudentID
		,CalCourseProgNodeID
		,AcaCal_SectionID
		,RetakeNo
		,ObtainedGPA
		,ObtainedGrade
		,AcademicCalenderID
		,CourseID
		,VersionID
		,Node_CourseID
		,NodeID
		,IsMultipleACUSpan
		,CourseCredit
		,CompletedCredit		
		,@ModifiedBy
		,getdate()
	FROM UIUEMS_CC_Student_CalCourseProgNode 
	where StudentID = @StuId 
		and SectionName <> ''
		and AcademicCalenderID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1))

SELECT @RowCount = @@ROWCOUNT
SELECT @RowCount AS 'count'

END





















GO
/****** Object:  StoredProcedure [dbo].[sp_SelectProgramByTeacherAndAcaCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[sp_SelectProgramByTeacherAndAcaCal]
AS
	begin
		select s1.ProgramID, s1.TeacherOneID,s1.AcademicCalenderID 
		from UIUEMS_CC_AcademicCalenderSection as s1 
		where s1.TeacherOneID = 5 and s1.AcademicCalenderID =5

		union 

		select s2.ProgramID,s2.TeacherTwoID,s2.AcademicCalenderID 
		from UIUEMS_CC_AcademicCalenderSection  as s2 
		where s2.TeacherTwoID = 2 and s2.AcademicCalenderID =23
	end





GO
/****** Object:  StoredProcedure [dbo].[sp_SelectSection]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashraf>
-- Create date: <11-06-2011>
-- Description:	<Select section information.>
-- Note: All CASE condition are related with software's enum field.
-- =============================================
CREATE PROCEDURE [dbo].[sp_SelectSection] 
@AcademicCalenderID int,
@ProgramID int,
@DeptID int,
@CourseID int,
@VersionID int
	
AS
BEGIN
	
	SET NOCOUNT ON;
		SELECT     
		acs.AcaCal_SectionID, acs.SectionName, 

		((convert( varchar, tsp1.StartHour) +':'+ convert(varchar,tsp1.StartMin)) +' : '+
		(CASE 
			WHEN tsp1.StartAMPM = 1 then 'AM' 
			WHEN tsp1.StartAMPM = 2 then 'PM' 
			END)+' - '+
		(convert( varchar, tsp1.EndHour) +':'+ convert(varchar,tsp1.EndMin)) +' : '+
		(CASE 
			WHEN tsp1.EndAMPM = 1 then 'AM' 
			WHEN tsp1.EndAMPM = 2 then 'PM' 
			END)) AS 'TimeSlot_1' , 
		 
		(CASE	
			WHEN acs.DayOne = 1 then 'Sunday' 
			WHEN acs.DayOne = 2 then 'Monday'
			WHEN acs.DayOne = 3 then 'Tuesday' 
			WHEN acs.DayOne = 4 then 'Wedneseday' 
			WHEN acs.DayOne = 5 then 'Thursday' 
			WHEN acs.DayOne = 6 then 'Friday' 
			WHEN acs.DayOne = 7 then 'Saturday'  
			END) AS 'DayOne',

		((convert( varchar, tsp2.StartHour) +':'+ convert(varchar,tsp2.StartMin)) +' : '+
		(CASE 
			WHEN tsp2.StartAMPM = 1 then 'AM' 
			WHEN tsp2.StartAMPM = 2 then 'PM' 
			END)+' - '+ 
		(convert( varchar, tsp2.EndHour) +':'+ convert(varchar,tsp2.EndMin)) +' : '+
		(CASE 
			WHEN tsp2.EndAMPM = 1 then 'AM' 
			WHEN tsp2.EndAMPM = 2 then 'PM' 
			END)) AS 'TimeSlot_2' ,

		(CASE	
			WHEN acs.DayTwo = 1 then 'Sunday' 
			WHEN acs.DayTwo = 2 then 'Monday'
			WHEN acs.DayTwo = 3 then 'Tuesday' 
			WHEN acs.DayTwo = 4 then 'Wedneseday' 
			WHEN acs.DayTwo = 5 then 'Thursday' 
			WHEN acs.DayTwo = 6 then 'Friday' 
			WHEN acs.DayTwo = 7 then 'Saturday'  
			END) AS 'DayTwo',  
		 
		t1.Code AS 'Faculty_1', 
		t2.Code AS 'Faculty_2', 
		ri1.RoomNumber +'-'+ ri1.RoomName AS 'RoomNo_1',
		ri2.RoomNumber +'-'+ ri2.RoomName AS 'RoomNo_2',
		acs.Capacity, acs.Occupied

		FROM         
		UIUEMS_CC_AcademicCalenderSection AS acs LEFT OUTER JOIN
		UIUEMS_CC_TimeSlotPlan AS tsp1 ON acs.TimeSlotPlanOneID = tsp1.TimeSlotPlanID LEFT OUTER JOIN
		UIUEMS_CC_TimeSlotPlan AS tsp2 ON acs.TimeSlotPlanTwoID = tsp2.TimeSlotPlanID LEFT OUTER JOIN
		UIUEMS_CC_Employee AS t1 ON acs.TeacherOneID = t1.PersonId LEFT OUTER JOIN
		UIUEMS_CC_Employee AS t2 ON acs.TeacherTwoID = t2.PersonId LEFT OUTER JOIN
		UIUEMS_CC_RoomInformation AS ri1 ON acs.RoomInfoOneID = ri1.RoomInfoID LEFT OUTER JOIN
		UIUEMS_CC_RoomInformation AS ri2 ON acs.RoomInfoTwoID = ri2.RoomInfoID 

		where 
			acs.AcademicCalenderID=@AcademicCalenderID AND
			acs.ProgramID=@ProgramID AND
			acs.DeptID=@DeptID AND
			acs.CourseID=@CourseID AND
			acs.VersionID=@VersionID
END




GO
/****** Object:  StoredProcedure [dbo].[sp_SustainableDiscountAndFee]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<saima>
-- Create date: <06082011>
-- Description:	<stublish which discount and fee will sustain by the relationships>
-- =============================================
CREATE PROCEDURE [dbo].[sp_SustainableDiscountAndFee] 
	
	@i_stdID int, 
	@i_acaCalID int,
	@i_progID int,
	@return int = 0 output
AS
BEGIN
SET NOCOUNT ON;

------------------------------------------------------------------------------------------------

declare @BillWorkSheetId int 
      ,@StudentId int
      ,@CalCourseProgNodeID int
      ,@AcaCalSectionID int
      ,@SectionTypeId int
      ,@AcaCalId int
      ,@CourseId int
      ,@VersionId int
      ,@CourseTypeId int
      ,@ProgramId int
      ,@RetakeNo int
      ,@PreviousBestGrade varchar(2)
      ,@FeeSetupId int
      ,@Fee money
      ,@DiscountTypeId int
      ,@DiscountPercentage decimal(18, 2)


declare crsr cursor local static for( 
			select [BillWorkSheetId]
				  ,[StudentId]
				  ,[CalCourseProgNodeID]
				  ,[AcaCalSectionID]
				  ,[SectionTypeId]
				  ,[AcaCalId]
				  ,[CourseId]
				  ,[VersionId]
				  ,[CourseTypeId]
				  ,[ProgramId]
				  ,[RetakeNo]
				  ,[PreviousBestGrade]
				  ,[FeeSetupId]
				  ,[Fee]--[PerCreditAmountFee]
				  ,[DiscountTypeId]
				  ,[DiscountPercentage]
			from dbo.UIUEMS_BL_Std_Crs_Bill_Worksheet 
			where [ProgramId] = @i_progID and [StudentId] = @i_stdID and [AcaCalId] = @i_acaCalID)

			open crsr

			fetch crsr into @BillWorkSheetId
						  ,@StudentId
						  ,@CalCourseProgNodeID
						  ,@AcaCalSectionID
						  ,@SectionTypeId
						  ,@AcaCalId
						  ,@CourseId
						  ,@VersionId
						  ,@CourseTypeId
						  ,@ProgramId
						  ,@RetakeNo
						  ,@PreviousBestGrade
						  ,@FeeSetupId
						  ,@Fee
						  ,@DiscountTypeId
						  ,@DiscountPercentage

			while(@@fetch_status = 0)
			begin
				----------------------------------------------------------------
				-- 1. updating as per relation between discount and course type

					if not exists (select TypeDefDiscountID, TypeDefCourseID 
							from dbo.UIUEMS_BL_RelationBetweenDiscountCourseType 
							where TypeDefDiscountID = @DiscountTypeId 
								and TypeDefCourseID = @CourseTypeId
								and AcaCalID = @AcaCalId
								and ProgramID = @ProgramId)
						begin 
							UPDATE [UIUEMS_BL_Std_Crs_Bill_Worksheet] set DiscountPercentage = 0.00
							where BillWorkSheetId = @BillWorkSheetId
						end


				-- 2. updating as per relation between discount and section

					if not exists (select TypeDefDiscountID, TypeDefID
							from dbo.UIUEMS_BL_RelationBetweenDiscountSectionType 
							where TypeDefDiscountID = @DiscountTypeId 
								and TypeDefID = @SectionTypeId
								and AcaCalID = @AcaCalId
								and ProgramID = @ProgramId) 
						begin 
							UPDATE [UIUEMS_BL_Std_Crs_Bill_Worksheet] set DiscountPercentage = 0.00
							where BillWorkSheetId = @BillWorkSheetId
						end


				-- 3. updating as per relation between discount and retake

					if @RetakeNo is null set @RetakeNo = 0
										
					if @RetakeNo = 0
						begin
							if not exists (select * from dbo.UIUEMS_BL_RelationBetweenDiscountRetake
									where TypeDefDiscountID = @DiscountTypeId
										and RetakeEqualsToZero = 'true'
										and AcaCalID = @AcaCalId
										and ProgramID = @ProgramId)
								UPDATE [UIUEMS_BL_Std_Crs_Bill_Worksheet] set DiscountPercentage = 0.00
								where BillWorkSheetId = @BillWorkSheetId
						end
					else 
						begin
							if not exists (select * from dbo.UIUEMS_BL_RelationBetweenDiscountRetake
										where TypeDefDiscountID = @DiscountTypeId
											and RetakeGreaterThanZero = 'true'
											and AcaCalID = @AcaCalId
											and ProgramID = @ProgramId)
									UPDATE [UIUEMS_BL_Std_Crs_Bill_Worksheet] set DiscountPercentage = 0.00
									where BillWorkSheetId = @BillWorkSheetId
						end

				-- 4. updating percreditamountfee as per is billable course

					declare @v_isCreditCourse bit
					set @v_isCreditCourse = (select IsCreditCourse from dbo.UIUEMS_CC_Course where CourseID = @CourseId and VersionID = @VersionId)

					if not exists (select * from dbo.UIUEMS_BL_IsCourseBillable 
									where BillStartFromRetakeNo >= @RetakeNo
										and IsCreditCourse = @v_isCreditCourse
										and AcaCalID = @AcaCalId
										and ProgramID = @ProgramId)
						UPDATE [UIUEMS_BL_Std_Crs_Bill_Worksheet] set Fee = 0.00
								where BillWorkSheetId = @BillWorkSheetId
				----------------------------------------------------------------
				fetch crsr into @BillWorkSheetId
							  ,@StudentId
							  ,@CalCourseProgNodeID
							  ,@AcaCalSectionID
							  ,@SectionTypeId
							  ,@AcaCalId
							  ,@CourseId
							  ,@VersionId
							  ,@CourseTypeId
							  ,@ProgramId
							  ,@RetakeNo
							  ,@PreviousBestGrade
							  ,@FeeSetupId
							  ,@Fee
							  ,@DiscountTypeId
							  ,@DiscountPercentage

			end
	 
		close crsr
		deallocate crsr
	set @return = 1
	return @return
END




GO
/****** Object:  StoredProcedure [dbo].[sp_test]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_test]
	-- Add the parameters for the stored procedure here
	@programcode varchar(45)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	select * from dbo.UIUEMS_CC_Program where Code = @programcode
END




GO
/****** Object:  StoredProcedure [dbo].[Student_CourseHistory_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-05-21 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[Student_CourseHistory_Insert] 
(
	@ID int Output,
	@StudentID int = NULL,
	@CalCourseProgNodeID int = NULL,
	@AcaCalSectionID int = NULL,
	@RetakeNo int = NULL,
	@ObtainedTotalMarks numeric = NULL,
	@ObtainedGPA numeric(18,2) = NULL,
	@ObtainedGrade varchar(2) = NULL,
	@GradeId int = NULL,
	@CourseStatusID int = NULL,
	@CourseStatusDate datetime = NULL,
	@AcaCalID int = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@CourseCredit numeric = NULL,
	@CompletedCredit numeric = NULL,
	@Node_CourseID int = NULL,
	@NodeID int = NULL,
	@IsMultipleACUSpan bit = NULL,
	@IsConsiderGPA bit = NULL,
	@CourseWavTransfrID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [dbo].[UIUEMS_CC_Student_CourseHistory]
(
	[StudentID],
	[CalCourseProgNodeID],
	[AcaCalSectionID],
	[RetakeNo],
	[ObtainedTotalMarks],
	[ObtainedGPA],
	[ObtainedGrade],
	[GradeId],
	[CourseStatusID],
	[CourseStatusDate],
	[AcaCalID],
	[CourseID],
	[VersionID],
	[CourseCredit],
	[CompletedCredit],
	[Node_CourseID],
	[NodeID],
	[IsMultipleACUSpan],
	[IsConsiderGPA],
	[CourseWavTransfrID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@StudentID,
	@CalCourseProgNodeID,
	@AcaCalSectionID,
	@RetakeNo,
	@ObtainedTotalMarks,
	@ObtainedGPA,
	@ObtainedGrade,
	@GradeId,
	@CourseStatusID,
	@CourseStatusDate,
	@AcaCalID,
	@CourseID,
	@VersionID,
	@CourseCredit,
	@CompletedCredit,
	@Node_CourseID,
	@NodeID,
	@IsMultipleACUSpan,
	@IsConsiderGPA,
	@CourseWavTransfrID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @ID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[StudentDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[StudentDeleteById]
(
@StudentID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Student]
WHERE StudentID = @StudentID

END





GO
/****** Object:  StoredProcedure [dbo].[StudentGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StudentGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_ER_Student


END





GO
/****** Object:  StoredProcedure [dbo].[StudentGetAllByProgAdminCalID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StudentGetAllByProgAdminCalID]
(
@ProgramID int = NULL,
@AcademicCalenderID int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

s.*

FROM	UIUEMS_ER_Student s, UIUEMS_ER_Admission a
WHERE	a.AdmissionCalenderID = @AcademicCalenderID and s.StudentID = a.StudentID and s.ProgramID = @ProgramID


END





GO
/****** Object:  StoredProcedure [dbo].[StudentGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StudentGetById]
(
@StudentID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*
FROM       UIUEMS_ER_Student
WHERE     (StudentID = @StudentID)

END





GO
/****** Object:  StoredProcedure [dbo].[StudentInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StudentInsert] 
(
	@StudentID                  int output,
	@Roll                       nvarchar(15) = NULL,
	
	@ProgramID                  int = NULL,
	@TotalDue                   money = NULL,
	@TotalPaid                  money = NULL,
	@Balance                    money = NULL,
	@TuitionSetUpID             int = NULL,
	@WaiverSetUpID              int = NULL,
	@DiscountSetUpID            int = NULL,
	@RelationTypeID             int = NULL,
	@RelativeID                 int = NULL,
	@TreeMasterID               int = NULL,
	@Major1NodeID               int = NULL,
	@Major2NodeID               int = NULL,
	@Major3NodeID               int = NULL,
	@Minor1NodeID               int = NULL,
	@Minor2NodeID               int = NULL,
	@Minor3NodeID               int = NULL,
	@CreatedBy                  int = NULL,
	@CreatedDate                datetime = NULL,
	@ModifiedBy                 int = NULL,
	@ModifiedDate               datetime = NULL,
	@PersonID                   int = NULL,
	@PaymentSlNo                nvarchar(50) = NULL,
	@IsActive                   bit = NULL,
	@IsDeleted                  bit = NULL,
	@IsDiploma                  bit = NULL,
	@Remarks                    varchar(500) = NULL,
	@AccountHeadsID             int = NULL,
	@CandidateId                int = NULL,
	@IsProvisionalAdmission     bit = NULL,
	@ValidUptoProAdmissionDate  datetime = NULL,
	@Pre_English                bit = NULL,
	@Pre_Math					bit = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS.2.0.0].[dbo].[UIUEMS_ER_Student]
(
	[Roll],
	
	[ProgramID],
	[TotalDue],
	[TotalPaid],
	[Balance],
	[TuitionSetUpID],
	[WaiverSetUpID],
	[DiscountSetUpID],
	[RelationTypeID],
	[RelativeID],
	[TreeMasterID],
	[Major1NodeID],
	[Major2NodeID],
	[Major3NodeID],
	[Minor1NodeID],
	[Minor2NodeID],
	[Minor3NodeID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[PersonID],
	[PaymentSlNo],
	[IsActive],
	[IsDeleted],
	[IsDiploma],
	[Remarks],
	[AccountHeadsID],
	[CandidateId],
	[IsProvisionalAdmission],
	[ValidUptoProAdmissionDate],
	[Pre_English],
	[Pre_Math]
)
 VALUES
(
	@Roll,
	
	@ProgramID,
	@TotalDue,
	@TotalPaid,
	@Balance,
	@TuitionSetUpID,
	@WaiverSetUpID,
	@DiscountSetUpID,
	@RelationTypeID,
	@RelativeID,
	@TreeMasterID,
	@Major1NodeID,
	@Major2NodeID,
	@Major3NodeID,
	@Minor1NodeID,
	@Minor2NodeID,
	@Minor3NodeID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@PersonID,
	@PaymentSlNo,
	@IsActive,
	@IsDeleted,
	@IsDiploma,
	@Remarks,
	@AccountHeadsID,
	@CandidateId,
	@IsProvisionalAdmission,
	@ValidUptoProAdmissionDate,
	@Pre_English,
	@Pre_Math
)
           
SET @StudentID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[StudentUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StudentUpdate]
(
	@StudentID int  = NULL,
	@Roll nvarchar(15)  = NULL,
	@ProgramID int = NULL,
	@TotalDue money = NULL,
	@TotalPaid money = NULL,
	@Balance money = NULL,
	@TuitionSetUpID int = NULL,
	@WaiverSetUpID int = NULL,
	@DiscountSetUpID int = NULL,
	@RelationTypeID int = NULL,
	@RelativeID int = NULL,
	@TreeMasterID int = NULL,
	@Major1NodeID int = NULL,
	@Major2NodeID int = NULL,
	@Major3NodeID int = NULL,
	@Minor1NodeID int = NULL,
	@Minor2NodeID int = NULL,
	@Minor3NodeID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@PersonID int = NULL,
	@PaymentSlNo nvarchar(50) = NULL,
	@IsActive bit = NULL,
	@IsDeleted bit = NULL,
	@IsDiploma bit = NULL,
	@Remarks varchar(500) = NULL,
	@AccountHeadsID int = NULL,
	@CandidateId int = NULL,
	@IsProvisionalAdmission bit = NULL,
	@ValidUptoProAdmissionDate datetime = NULL,
	@Pre_English bit = NULL,
	@Pre_Math bit = NULL,
	@History nvarchar(MAX) = NULL,
	@Attribute1 nvarchar(500) = NULL,
	@Attribute2 nvarchar(500) = NULL,
	@TreeCalendarMasterID int = NULL
)

AS
BEGIN
SET NOCOUNT OFF;
If @ValidUptoProAdmissionDate is null
	set @ValidUptoProAdmissionDate = '';
UPDATE [UIUEMS_ER_Student]
   SET
	[Roll]	=	@Roll,
	[ProgramID]	=	@ProgramID,
	[TotalDue]	=	@TotalDue,
	[TotalPaid]	=	@TotalPaid,
	[Balance]	=	@Balance,
	[TuitionSetUpID]	=	@TuitionSetUpID,
	[WaiverSetUpID]	=	@WaiverSetUpID,
	[DiscountSetUpID]	=	@DiscountSetUpID,
	[RelationTypeID]	=	@RelationTypeID,
	[RelativeID]	=	@RelativeID,
	[TreeMasterID]	=	@TreeMasterID,
	[Major1NodeID]	=	@Major1NodeID,
	[Major2NodeID]	=	@Major2NodeID,
	[Major3NodeID]	=	@Major3NodeID,
	[Minor1NodeID]	=	@Minor1NodeID,
	[Minor2NodeID]	=	@Minor2NodeID,
	[Minor3NodeID]	=	@Minor3NodeID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate,
	[PersonID]	=	@PersonID,
	[PaymentSlNo]	=	@PaymentSlNo,
	[IsActive]	=	@IsActive,
	[IsDeleted]	=	@IsDeleted,
	[IsDiploma]	=	@IsDiploma,
	[Remarks]	=	@Remarks,
	[AccountHeadsID]	=	@AccountHeadsID,
	[CandidateId]	=	@CandidateId,
	[IsProvisionalAdmission]	=	@IsProvisionalAdmission,
	[ValidUptoProAdmissionDate]	=	@ValidUptoProAdmissionDate,
	[Pre_English]	=	@Pre_English,
	[Pre_Math]	=	@Pre_Math,
	[History] = @History,
	[Attribute1] = @Attribute1,
	[Attribute2] = @Attribute2,
	[TreeCalendarMasterID] = @TreeCalendarMasterID


WHERE StudentID = @StudentID
           
END





GO
/****** Object:  StoredProcedure [dbo].[Test]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO






-- =============================================
-- Author:		<Saima>
-- Create date: <Create Date,,>
-- Description:	<insert into dbo.UIUEMS_SM_StdDiscountCurrent, dbo.UIUEMS_SM_StdDiscountHistory >
-- =============================================
CREATE PROCEDURE [dbo].[Test]
	-- Add the parameters for the stored procedure here
	@admissionID int, 
	@typeDefID int, 
	@typePercentage decimal(18, 2),
	@effectiveAcaCalId int,
	@createdBy int,-- = null, 
	--@createdDate datetime,
	@result int output
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	declare 
		@percent decimal(18, 2),
		@acaCalId int 
		set @percent = -1
		set @acaCalId = 0
		set @result = 0
	SET NOCOUNT ON;
	select @percent = TypePercentage--, @acaCalId = EffectiveAcaCalID 
	from dbo.UIUEMS_SM_StdDiscountCurrent
	where AdmissionID = @admissionID and TypeDefID = @typeDefID 
	print @percent
print @typePercentage
	if @percent != -1-- and @acaCalId != 0
		begin
			if @percent != @typePercentage-- or @acaCalId != @effectiveAcaCalId
				begin
					insert into dbo.UIUEMS_SM_StdDiscountHistory
					([AdmissionID]
					   ,[TypeDefID]
					   ,[TypePercentage]
					   ,[EffectiveAcaCalID]
					   ,[CreatedBy]
					   ,[CreatedDate])
				    VALUES
					   (@admissionID
					   ,@typeDefID
					   ,@percent
					   ,@acaCalId
					   ,@createdBy
					   ,getdate()--@createdDate
					)

					UPDATE [dbo].[UIUEMS_SM_StdDiscountCurrent]
					   SET [TypePercentage] = @typePercentage
						  ,[EffectiveAcaCalID] = @effectiveAcaCalId
						  ,[ModifiedBy] = @createdBy
						  ,[ModifiedDate] = getdate() --@createdDate
					 where AdmissionID = @admissionID and TypeDefID = @typeDefID 
				set @result = 2
				end
		end
	else
		begin
			insert into dbo.UIUEMS_SM_StdDiscountCurrent
					([AdmissionID]
					   ,[TypeDefID]
					   ,[TypePercentage]
					   ,[EffectiveAcaCalID]
					   ,[CreatedBy]
					   ,[CreatedDate])
				    VALUES
					   (@admissionID
					   ,@typeDefID
					   ,@typePercentage
					   ,@effectiveAcaCalId
					   ,@createdBy
					   ,getdate()--@createdDate
					)
			set @result = 1
		end
	
return @result
END










GO
/****** Object:  StoredProcedure [dbo].[TransferFromCandidateToStudent]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TransferFromCandidateToStudent]
(
@ReturnValue int Output,
@BatchCode nvarchar(3) = null,
@CreatedBy int = 1,
@CreatedDate Datetime = '2013-04-09'
) --this BatchCode comes from Student Table

AS
BEGIN
	declare @batchId int;
	select @batchId = AcademicCalenderID from [Admission.2.0.0].[dbo].[AcademicCalender] where BatchCode = @BatchCode;
	--Retrieve BatchId from Candidate DB
	
	declare candidateList cursor
	for
	select CandidateId, StudentID, FirstName, Email, BirthDate, Gender, MaritalStatus, BloodGroup, Religion, Nationality, FatherName, MotherName, FatherProfession, MotherProfession, LocalGuardianName , IsPremath, IsPreEnglish
	from [Admission.2.0.0].[dbo].[Candidate]
	where BatchId = @batchId and StudentID is not null;
	--Store Candidate Data List Into Define cursor
	
	declare @candidateId int, @roll nvarchar(50), @studentFirstName  nvarchar(50), @studentEmail  nvarchar(50), @dateOfBirth smalldatetime, @sex  nvarchar(50), @maritalStatus  nvarchar(50), @bloodGroup nvarchar(50), @religion  nvarchar(50), @nationality  nvarchar(50), @fatherName  nvarchar(50), @motherName  nvarchar(50), @fatherProfession nvarchar(50), @motherProfession  nvarchar(50), @localGuardianName  nvarchar(50), @preMath Bit, @preEnglish Bit;
	--Variable declare for Student table
	open candidateList
	fetch next from candidateList into @candidateId, @roll, @studentFirstName, @studentEmail, @dateOfBirth, @sex, @maritalStatus, @bloodGroup, @religion, @nationality, @fatherName, @motherName, @fatherProfession, @motherProfession, @localGuardianName, @preMath, @preEnglish
	--Fetch 1st ROW
	
	declare @datepicker date, @flag int, @isDiploma nvarchar(50), @isDiplomaFlag bit;
	set @datepicker = getdate();
	--Common declare
	
	while @@FETCH_STATUS = 0
	BEGIN
		--print(@studentId)
		
		select @flag = COUNT(*) from [dbo].[UIUEMS_ER_Student] where Roll = @roll;
		IF @flag = 0
			BEGIN
							
			SET @isDiplomaFlag = (select [UIUEMS.2.0.0].[dbo].IsDiploma(@candidateId));
			IF @isDiplomaFlag = 'True'
				BEGIN
				SET @isDiploma = 'Yes';
				END
			
			declare @StudentID  int;
			EXEC [dbo].[StudentInsert] @StudentID  output, @Roll=@roll, @FirstName=@studentFirstName, @DOB=@dateOfBirth, @IsDiploma=@isDiploma, @Pre_English=@preEnglish, @Pre_Math=@preMath, @CreatedBy=@CreatedBy, @CreatedDate=@CreatedDate;
			--print(@PersonId);
			--UIUEMS_ER_Person Insert
			
			END	
		
		fetch next from candidateList into @candidateId, @roll, @studentFirstName, @studentEmail, @dateOfBirth, @sex, @maritalStatus, @bloodGroup, @religion, @nationality, @fatherName, @motherName, @fatherProfession, @motherProfession, @localGuardianName, @preMath, @preEnglish
	END
	close candidateList
	deallocate candidateList
	
	set @ReturnValue = 1;
	return @ReturnValue;
END



GO
/****** Object:  StoredProcedure [dbo].[TreeCalendarMasterGetAllByTreeMasterID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeCalendarMasterGetAllByTreeMasterID]
(@TreeMasterID int = NULL)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeCalendarMasterID],
[TreeMasterID],
[CalendarMasterID],
[Name],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]


FROM	UIUEMS_CC_TreeCalendarMaster
WHERE	[TreeMasterID] = @TreeMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[TreeCalendarMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeCalendarMasterGetById]
(@TreeCalendarMasterID int = NULL)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeCalendarMasterID],
[TreeMasterID],
[CalendarMasterID],
[Name],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]


FROM	UIUEMS_CC_TreeCalendarMaster
WHERE	[TreeCalendarMasterID] = @TreeCalendarMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[TreeMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[TreeMasterDeleteById]
(
@TreeMasterID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_TreeMaster]
WHERE TreeMasterID = @TreeMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[TreeMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeMasterID],
[ProgramID],
[RootNodeID],
[StartTrimesterID],
[RequiredUnits],
[PassingGPA],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_TreeMaster


END




GO
/****** Object:  StoredProcedure [dbo].[TreeMasterGetAllByProgramID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeMasterGetAllByProgramID]
(
@ProgramID int = null
)
AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeMasterID],
[ProgramID],
[RootNodeID],
[StartTrimesterID],
[RequiredUnits],
[PassingGPA],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM	UIUEMS_CC_TreeMaster
WHERE	[ProgramID]	=	@ProgramID

END




GO
/****** Object:  StoredProcedure [dbo].[TreeMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeMasterGetById]
(
@TreeMasterID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeMasterID],
[ProgramID],
[RootNodeID],
[StartTrimesterID],
[RequiredUnits],
[PassingGPA],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_TreeMaster
WHERE     (TreeMasterID = @TreeMasterID)

END




GO
/****** Object:  StoredProcedure [dbo].[TreeMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeMasterInsert] 
(
@TreeMasterID int   OUTPUT,
@ProgramID int  = NULL,
@RootNodeID int  = NULL,
@StartTrimesterID int = NULL,
@RequiredUnits money = NULL,
@PassingGPA money = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_TreeMaster]
(
[TreeMasterID],
[ProgramID],
[RootNodeID],
[StartTrimesterID],
[RequiredUnits],
[PassingGPA],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@TreeMasterID,
@ProgramID,
@RootNodeID,
@StartTrimesterID,
@RequiredUnits,
@PassingGPA,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @TreeMasterID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[TreeMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[TreeMasterUpdate]
(
@TreeMasterID int   = NULL,
@ProgramID int  = NULL,
@RootNodeID int  = NULL,
@StartTrimesterID int = NULL,
@RequiredUnits money = NULL,
@PassingGPA money = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_TreeMaster]
   SET


[ProgramID]	=	@ProgramID,
[RootNodeID]	=	@RootNodeID,
[StartTrimesterID]	=	@StartTrimesterID,
[RequiredUnits]	=	@RequiredUnits,
[PassingGPA]	=	@PassingGPA,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE TreeMasterID = @TreeMasterID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_AccountHeadsDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AC_AccountHeadsDeleteById]
(
@AccountsID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AC_AccountHeads]
WHERE AccountsID = @AccountsID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_AccountHeadsGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_AccountHeadsGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_AccountHeads


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_AccountHeadsGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_AccountHeadsGetById]
(
@AccountsID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_AC_AccountHeads
WHERE     (AccountsID = @AccountsID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_AccountHeadsInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_AccountHeadsInsert] 
(
	@AccountsID		int output,
	@Name			nvarchar(50) = NULL,
	@Code			nvarchar(50) = NULL,
	@ParentID		int = NULL,
	@Tag			varchar(100) = NULL,
	@Remarks		nvarchar(500) = NULL,
	@IsLeaf			bit = NULL,
	@SysMandatory	bit = NULL,
	@CreatedBy		int = NULL,
	@CreatedDate	datetime = NULL,
	@ModifiedBy		int = NULL,
	@ModifiedDate	datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS.2.0.0].[dbo].[UIUEMS_AC_AccountHeads]
(
	[Name],
	[Code],
	[ParentID],
	[Tag],
	[Remarks],
	[IsLeaf],
	[SysMandatory],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@Name,
	@Code,
	@ParentID,
	@Tag,
	@Remarks,
	@IsLeaf,
	@SysMandatory,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @AccountsID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_AccountHeadsUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_AccountHeadsUpdate]
(
@AccountsID int  = NULL,
@Name nvarchar(50)  = NULL,
@Code nvarchar(50) = NULL,
@ParentID int = NULL,
@Tag varchar(100) = NULL,
@Remarks nvarchar(500) = NULL,
@IsLeaf bit = NULL,
@SysMandatory bit = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AC_AccountHeads]
   SET

[Name]	=	@Name,
[Code]	=	@Code,
[ParentID]	=	@ParentID,
[Tag]	=	@Tag,
[Remarks]	=	@Remarks,
[IsLeaf]	=	@IsLeaf,
[SysMandatory]	=	@SysMandatory,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE AccountsID = @AccountsID
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AC_FeeSetupDeleteById]
(
@FeeSetUpID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AC_FeeSetup]
WHERE FeeSetUpID = @FeeSetUpID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_FeeSetupGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_FeeSetup


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_FeeSetupGetById]
(
@FeeSetUpID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT    
*

FROM       UIUEMS_AC_FeeSetup
WHERE     (FeeSetUpID = @FeeSetUpID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupGetByTypeDefinationAndSession]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_FeeSetupGetByTypeDefinationAndSession]
(
@TypeDefinitionID int = null,
@SessionId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT    
*

FROM       UIUEMS_BL_FeeSetup
WHERE     (TypeDefID = @TypeDefinitionID) and (AcaCalID=@SessionId)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupGetByTypeDefinationSessionProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_FeeSetupGetByTypeDefinationSessionProgram]
(
@TypeDefinitionID int = null,
@SessionId int = null,
@ProgramId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT    
*

FROM       UIUEMS_BL_FeeSetup
WHERE     (TypeDefID = @TypeDefinitionID) and (AcaCalID=@SessionId) and (ProgramID = @ProgramId)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_FeeSetupInsert] 
(
@FeeSetUpID int OUTPUT,
@AdmissionID int = NULL,
@TypeDefID int = NULL,
@TypePercentage decimal(18, 2) = NULL,
@EffectiveAcaCalID int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AC_FeeSetup]
(
[FeeSetUpID],
[AdmissionID],
[TypeDefID],
[TypePercentage],
[EffectiveAcaCalID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@FeeSetUpID,
@AdmissionID,
@TypeDefID,
@TypePercentage,
@EffectiveAcaCalID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @FeeSetUpID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_FeeSetupUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_FeeSetupUpdate]
(
@FeeSetUpID int = NULL,
@AdmissionID int = NULL,
@TypeDefID int = NULL,
@TypePercentage decimal(18, 2) = NULL,
@EffectiveAcaCalID int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AC_FeeSetup]
   SET

[AdmissionID]	=	@AdmissionID,
[TypeDefID]	=	@TypeDefID,
[TypePercentage]	=	@TypePercentage,
[EffectiveAcaCalID]	=	@EffectiveAcaCalID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE FeeSetUpID = @FeeSetUpID
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_SiblingSetupDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AC_SiblingSetupDeleteById]
(
@SiblingSetupId int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AC_SiblingSetup]
WHERE SiblingSetupId = @SiblingSetupId

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_SiblingSetupGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_SiblingSetupGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_SiblingSetup


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_SiblingSetupGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_SiblingSetupGetById]
(
@SiblingSetupId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_SiblingSetup
WHERE     (SiblingSetupId = @SiblingSetupId)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_SiblingSetupInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_SiblingSetupInsert] 
(
@SiblingSetupId int   OUTPUT,
@GroupID int = NULL,
@ApplicantId int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AC_SiblingSetup]
(

[GroupID],
[ApplicantId],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(

@GroupID,
@ApplicantId,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @SiblingSetupId = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_SiblingSetupUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_SiblingSetupUpdate]
(
@SiblingSetupId int   = NULL,
@GroupID int = NULL,
@ApplicantId int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AC_SiblingSetup]
   SET

[GroupID]	=	@GroupID,
[ApplicantId]	=	@ApplicantId,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE SiblingSetupId = @SiblingSetupId
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_TypeDefinitionDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AC_TypeDefinitionDeleteById]
(
@TypeDefinitionID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AC_TypeDefinition]
WHERE TypeDefinitionID = @TypeDefinitionID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_TypeDefinitionGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_TypeDefinitionGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_TypeDefinition


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_TypeDefinitionGetAllByType]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_TypeDefinitionGetAllByType]
(
@Type nvarchar(250) = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_TypeDefinition
WHERE     (Type = @Type)

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_TypeDefinitionGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_TypeDefinitionGetById]
(
@TypeDefinitionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_TypeDefinition
WHERE     (TypeDefinitionID = @TypeDefinitionID)

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_TypeDefinitionInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_TypeDefinitionInsert] 
(
	@TypeDefinitionID int   OUTPUT,
	@Type nvarchar(250) = NULL,
	@Definition nvarchar(250) = NULL,
	@AccountsID int = NULL,
	@IsCourseSpecific bit = NULL,
	@IsLifetimeOnce bit = NULL,
	@IsPerAcaCal bit = NULL,
	@Priority int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AC_TypeDefinition]
(
	[TypeDefinitionID],
	[Type],
	[Definition],
	[AccountsID],
	[IsCourseSpecific],
	[IsLifetimeOnce],
	[IsPerAcaCal],
	[Priority],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@TypeDefinitionID,
	@Type,
	@Definition,
	@AccountsID,
	@IsCourseSpecific,
	@IsLifetimeOnce,
	@IsPerAcaCal,
	@Priority,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @TypeDefinitionID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_TypeDefinitionUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_TypeDefinitionUpdate]
(
	@TypeDefinitionID int   = NULL,
	@Type nvarchar(250) = NULL,
	@Definition nvarchar(250) = NULL,
	@AccountsID int = NULL,
	@IsCourseSpecific bit = NULL,
	@IsLifetimeOnce bit = NULL,
	@IsPerAcaCal bit = NULL,
	@Priority int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AC_TypeDefinition]
   SET


	[Type]	=	@Type,
	[Definition]	=	@Definition,
	[AccountsID]	=	@AccountsID,
	[IsCourseSpecific]	=	@IsCourseSpecific,
	[IsLifetimeOnce]	=	@IsLifetimeOnce,
	[IsPerAcaCal]	=	@IsPerAcaCal,
	[Priority]	=	@Priority,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE TypeDefinitionID = @TypeDefinitionID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_VoucherDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AC_VoucherDeleteById]
(
@VoucherID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AC_Voucher]
WHERE VoucherID = @VoucherID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_VoucherGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_VoucherGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_Voucher


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_VoucherGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_VoucherGetById]
(
@VoucherID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AC_Voucher
WHERE     (VoucherID = @VoucherID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_VoucherInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_VoucherInsert] 
(
	@VoucherID			int output,
	@Prefix				nvarchar(50) = NULL,
	@SLNO				bigint = NULL,
	@DrAccountHeadsID	int = NULL,
	@CrAccountHeadsID	int = NULL,
	@Amount				numeric(18, 2) = NULL,
	@PostedBy			varchar(50) = NULL,
	@CourseID			int = NULL,
	@VersionID			int = NULL,
	@Remarks			nvarchar(500) = NULL,
	@AcaCalID			int = NULL,
	@ReferenceNo		varchar(50) = NULL,
	@ChequeNo			varchar(50) = NULL,
	@ChequeBankName		varchar(150) = NULL,
	@ChequeDate			datetime = NULL,
	@CreatedBy			int = NULL,
	@CreatedDate		datetime = NULL,
	@ModifiedBy			int = NULL,
	@ModifiedDate		datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS.2.0.0].[dbo].[UIUEMS_AC_Voucher]
(
	[Prefix],
	[SLNO],
	[DrAccountHeadsID],
	[CrAccountHeadsID],
	[Amount],
	[PostedBy],
	[CourseID],
	[VersionID],
	[Remarks],
	[AcaCalID],
	[ReferenceNo],
	[ChequeNo],
	[ChequeBankName],
	[ChequeDate],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@Prefix,
	@SLNO,
	@DrAccountHeadsID,
	@CrAccountHeadsID,
	@Amount,
	@PostedBy,
	@CourseID,
	@VersionID,
	@Remarks,
	@AcaCalID,
	@ReferenceNo,
	@ChequeNo,
	@ChequeBankName,
	@ChequeDate,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @VoucherID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AC_VoucherUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AC_VoucherUpdate]
(
	@VoucherID int   = NULL,
	@Prefix nvarchar(50) = NULL,
	@SLNO bigint = NULL,
	@DrAccountHeadsID int = NULL,
	@CrAccountHeadsID int = NULL,
	@Amount numeric(18, 2) = NULL,
	@PostedBy varchar(50) = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Remarks nvarchar(500) = NULL,
	@AcaCalID int = NULL,
	@ReferenceNo varchar(50) = NULL,
	@ChequeNo varchar(50) = NULL,
	@ChequeBankName varchar(150) = NULL,
	@ChequeDate datetime = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AC_Voucher]
   SET

	[Prefix]	=	@Prefix,
	[SLNO]	=	@SLNO,
	[DrAccountHeadsID]	=	@DrAccountHeadsID,
	[CrAccountHeadsID]	=	@CrAccountHeadsID,
	[Amount]	=	@Amount,
	[PostedBy]	=	@PostedBy,
	[CourseID]	=	@CourseID,
	[VersionID]	=	@VersionID,
	[Remarks]	=	@Remarks,
	[AcaCalID]	=	@AcaCalID,
	[ReferenceNo]	=	@ReferenceNo,
	[ChequeNo]	=	@ChequeNo,
	[ChequeBankName]	=	@ChequeBankName,
	[ChequeDate]	=	@ChequeDate,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE VoucherID = @VoucherID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_ActiveUserGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_ActiveUserGetAll] 
	-- Add the parameters for the stored procedure here
	@ValueID int = null, 
	@AdmissionCalenderID int = null,
	@LogInID varchar(50) = null
AS
BEGIN
	
	SET NOCOUNT ON;
	IF(@AdmissionCalenderID = 0)
	BEGIN
	IF(@LogInID = '')
	BEGIN
	Select UIUEMS_AD_User.LogInID, UIUEMS_ER_Person.FirstName, UIUEMS_ER_Person.Phone,UIUEMS_AD_User.IsActive,UIUEMS_AD_User.User_ID
	FROM UIUEMS_AD_User,UIUEMS_AD_UserInPerson,UIUEMS_ER_Person,UIUEMS_ER_Student,UIUEMS_ER_Admission
	where  UIUEMS_AD_User.User_ID = UIUEMS_AD_UserInPerson.User_ID
	       and UIUEMS_AD_UserInPerson.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.StudentID = UIUEMS_ER_Admission.StudentID
		   and UIUEMS_ER_Person.TypeId=@ValueID 
	END
	ELSE
	BEGIN
	Select UIUEMS_AD_User.LogInID, UIUEMS_ER_Person.FirstName, UIUEMS_ER_Person.Phone,UIUEMS_AD_User.IsActive,UIUEMS_AD_User.User_ID
	FROM UIUEMS_AD_User,UIUEMS_AD_UserInPerson,UIUEMS_ER_Person,UIUEMS_ER_Student,UIUEMS_ER_Admission
	where  UIUEMS_AD_User.User_ID = UIUEMS_AD_UserInPerson.User_ID
	       and UIUEMS_AD_UserInPerson.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.StudentID = UIUEMS_ER_Admission.StudentID
		   and UIUEMS_ER_Person.TypeId=@ValueID 
		   and UIUEMS_AD_User.LogInID=@LogInID
	END
	
	END
ELSE
	BEGIN
	IF(@LogInID = '')
	BEGIN
	Select UIUEMS_AD_User.LogInID, UIUEMS_ER_Person.FirstName, UIUEMS_ER_Person.Phone,UIUEMS_AD_User.IsActive,UIUEMS_AD_User.User_ID
	FROM UIUEMS_AD_User,UIUEMS_AD_UserInPerson,UIUEMS_ER_Person,UIUEMS_ER_Student,UIUEMS_ER_Admission
	where  UIUEMS_AD_User.User_ID = UIUEMS_AD_UserInPerson.User_ID
	       and UIUEMS_AD_UserInPerson.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.StudentID = UIUEMS_ER_Admission.StudentID
		   and UIUEMS_ER_Person.TypeId=@ValueID
		   and UIUEMS_ER_Admission.AdmissionCalenderID=@AdmissionCalenderID
	END
	ELSE
	BEGIN
	Select UIUEMS_AD_User.LogInID, UIUEMS_ER_Person.FirstName, UIUEMS_ER_Person.Phone,UIUEMS_AD_User.IsActive,UIUEMS_AD_User.User_ID
	FROM UIUEMS_AD_User,UIUEMS_AD_UserInPerson,UIUEMS_ER_Person,UIUEMS_ER_Student,UIUEMS_ER_Admission
	where  UIUEMS_AD_User.User_ID = UIUEMS_AD_UserInPerson.User_ID
	       and UIUEMS_AD_UserInPerson.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.PersonID = UIUEMS_ER_Person.PersonID
		   and UIUEMS_ER_Student.StudentID = UIUEMS_ER_Admission.StudentID
		   and UIUEMS_ER_Person.TypeId=@ValueID
		   and UIUEMS_ER_Admission.AdmissionCalenderID=@AdmissionCalenderID
		   and UIUEMS_AD_User.LogInID=@LogInID
	END
	
END
		

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_MenuDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_MenuDeleteById]
(
@Menu_ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_Menu]
WHERE Menu_ID = @Menu_ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_MenuGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_MenuGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_Menu


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_MenuGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_MenuGetById]
(
@Menu_ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_Menu
WHERE     (Menu_ID = @Menu_ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_MenuInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_MenuInsert] 
(
	@Menu_ID int  OUTPUT,
	@Name varchar(50) = NULL,
	@URL varchar(500) = NULL,
	@ParentMnu_ID int = NULL,
	@Tier int  = NULL,
	@IsSysAdminAccesible bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AD_Menu]
(
	[Menu_ID],
	[Name],
	[URL],
	[ParentMnu_ID],
	[Tier],
	[IsSysAdminAccesible],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@Menu_ID,
	@Name,
	@URL,
	@ParentMnu_ID,
	@Tier,
	@IsSysAdminAccesible,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @Menu_ID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_MenuUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_MenuUpdate]
(
	@Menu_ID int  = NULL,
	@Name varchar(50) = NULL,
	@URL varchar(500) = NULL,
	@ParentMnu_ID int = NULL,
	@Tier int  = NULL,
	@IsSysAdminAccesible bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AD_Menu]
   SET

	[Name]	=	@Name,
	[URL]	=	@URL,
	[ParentMnu_ID]	=	@ParentMnu_ID,
	[Tier]	=	@Tier,
	[IsSysAdminAccesible]	=	@IsSysAdminAccesible,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE Menu_ID = @Menu_ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_Role_MenuDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_Role_MenuDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_Role_Menu]
WHERE ID = @ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_Role_MenuGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_Role_MenuGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_Role_Menu


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_Role_MenuGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_Role_MenuGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_Role_Menu
WHERE     (ID = @ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_Role_MenuInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_Role_MenuInsert] 
(
	@ID int   OUTPUT,
	@RoleID int  = NULL,
	@MenuID int  = NULL,
	@AccessMenuStartDate datetime = NULL,
	@AccessMenuEndDate datetime = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AD_Role_Menu]
(
	[ID],
	[RoleID],
	[MenuID],
	[AccessMenuStartDate],
	[AccessMenuEndDate],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@ID,
	@RoleID,
	@MenuID,
	@AccessMenuStartDate,
	@AccessMenuEndDate,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_Role_MenuUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_Role_MenuUpdate]
(
	@ID int   = NULL,
	@RoleID int  = NULL,
	@MenuID int  = NULL,
	@AccessMenuStartDate datetime = NULL,
	@AccessMenuEndDate datetime = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AD_Role_Menu]
   SET

	[RoleID]	=	@RoleID,
	[MenuID]	=	@MenuID,
	[AccessMenuStartDate]	=	@AccessMenuStartDate,
	[AccessMenuEndDate]	=	@AccessMenuEndDate,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE ID = @ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_RoleDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_RoleDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_Role]
WHERE ID = @ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_RoleGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_RoleGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_Role


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_RoleGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_RoleGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_Role
WHERE     (ID = @ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_RoleInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_RoleInsert] 
(
	@ID int  OUTPUT,
	@RoleName varchar(50)  = NULL,
	@SessionTime int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AD_Role]
(
	[ID],
	[RoleName],
	[SessionTime],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]


)
 VALUES
(
	@ID,
	@RoleName,
	@SessionTime,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_RoleUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_RoleUpdate]
(
	@ID int  = NULL,
	@RoleName varchar(50)  = NULL,
	@SessionTime int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AD_Role]
   SET

[RoleName]	=	@RoleName,
[SessionTime]	=	@SessionTime,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE ID = @ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_Insert]
(
	@User_ID int = NULL,
	@LogInID varchar(50) = NULL,
	@Password varchar(50) = NULL,
	@RoleID int = NULL,
	@RoleExistStartDate datetime = NULL,
	@RoleExistEndDate datetime = NULL,
	@IsActive bit = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_AD_User]
(
	[User_ID],
	[LogInID],
	[Password],
	[RoleID],
	[RoleExistStartDate],
	[RoleExistEndDate],
	[IsActive],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@User_ID,
	@LogInID,
	@Password,
	@RoleID,
	@RoleExistStartDate,
	@RoleExistEndDate,
	@IsActive,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @User_ID = @User_ID;
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_MenuDelete]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_User_MenuDelete]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_User_Menu]
WHERE Id = @Id

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_MenuGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_MenuGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_User_Menu;


END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_MenuGetAllByUserId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_MenuGetAllByUserId]
(
	@UserId Int = NULL
)
AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_AD_User_Menu Where UserId = @UserId;


END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_MenuGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_MenuGetById]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_User_Menu
WHERE     (Id = @Id);

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_MenuInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_MenuInsert]
(
	@Id int OUTPUT,
	@MenuId int = NULL,
	@UserId int = NULL,
	@ValidFrom datetime = NULL,
	@ValidTo datetime = NULL,
	@AddRemove int = NUll,
	@ProgramId int = NULL,
	@DeptId int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_AD_User_Menu]
(
	[MenuId],
	[UserId],
	[ValidFrom],
	[ValidTo],
	[AddRemove],
	[ProgramId],
	[DeptId],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@MenuId,
	@UserId,
	@ValidFrom,
	@ValidTo,
	@AddRemove,
	@ProgramId,
	@DeptId,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @Id = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_MenuUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_AD_User_MenuUpdate]
(
	@Id int = NULL,
	@MenuId int = NULL,
	@UserId int = NULL,
	@ValidFrom datetime = NULL,
	@ValidTo datetime = NULL,
	@AddRemove Int = NULL,
	@ProgramId int = NULL,
	@DeptId int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AD_User_Menu]
   SET

	[MenuId]=@MenuId,
	[UserId]=@UserId,
	[ValidFrom]=@ValidFrom,
	[ValidTo]=@ValidTo,
	[AddRemove]=@AddRemove,
	[ProgramId]=@ProgramId,
	[DeptId]=@DeptId,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE Id = @Id
           
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_ObjectControlDelete]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_User_ObjectControlDelete]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_User_ObjectControl]
WHERE Id = @Id

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_ObjectControlGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_ObjectControlGetAll]

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_AD_User_ObjectControl;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_ObjectControlGetAllByUserId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_ObjectControlGetAllByUserId]
(
	@UserId Int = NULL
)
AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_AD_User_ObjectControl Where UserId = @UserId;


END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_ObjectControlGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_ObjectControlGetById]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_AD_User_ObjectControl WHERE (Id = @Id);

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_ObjectControlInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_ObjectControlInsert]
(
	@Id int OUTPUT,
	@ObjectControlId int = NULL,
	@UserId int = NULL,
	@ValidFrom datetime = NULL,
	@ValidTo datetime = NULL,
	@ProgramId int = NULL,
	@DeptId int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_AD_User_ObjectControl]
(
	[ObjectControlId],
	[UserId],
	[ValidFrom],
	[ValidTo],
	[ProgramId],
	[DeptId],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@ObjectControlId,
	@UserId,
	@ValidFrom,
	@ValidTo,
	@ProgramId,
	@DeptId,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @Id = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_User_ObjectControlUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_User_ObjectControlUpdate]
(
	@Id int = NULL,
	@ObjectControlId int = NULL,
	@UserId int = NULL,
	@ValidFrom datetime = NULL,
	@ValidTo datetime = NULL,
	@ProgramId int = NULL,
	@DeptId int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AD_User_ObjectControl]
	SET

	[ObjectControlId] = @ObjectControlId,
	[UserId] = @UserId,
	[ValidFrom] = @ValidFrom,
	[ValidTo] = @ValidTo,
	[ProgramId] = @ProgramId,
	[DeptId] = @DeptId,
	[CreatedBy] = @CreatedBy,
	[CreatedDate] = @CreatedDate,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE Id = @Id
           
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserCreateAndAccessRelation_For_Faculty]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===========================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-07-23 >
-- Description:	<Softwar Eng.>
-- ===========================

CREATE PROCEDURE [dbo].[UIUEMS_AD_UserCreateAndAccessRelation_For_Faculty]
As
Begin

	Declare @RoleId Int, @CurrentDate Datetime, @ExpireDate Datetime;
	Set @CurrentDate = GETDATE();
	Set @ExpireDate = '2020-12-30';
	Set @RoleId = NULL;
	Select @RoleId = ID From UIUEMS_AD_Role Where RoleName = 'General';
	If @RoleId IS NOT NULL
	Begin

		Declare FacultyList Cursor
		For
		Select p.PersonID, e.Code From UIUEMS_CC_Employee e, UIUEMS_ER_Person p Where e.PersonID = p.PersonID;

		Declare @FacultyCode nvarchar(15), @PersonId Int;
		Open FacultyList;
		Fetch Next From FacultyList Into @PersonId, @FacultyCode;
		While @@FETCH_STATUS = 0
		Begin

			Declare @ExistStatus Int;
			Set @ExistStatus = 0;
			Select @ExistStatus = Count(*) From UIUEMS_AD_User Where LogInID = @FacultyCode;
			If @ExistStatus = 0
			Begin
				Declare @User_ID Int;
				Select @User_ID = Max(User_ID) From UIUEMS_AD_User;
				Set @User_ID += 1;

				EXEC [dbo].[UIUEMS_AD_User_Insert] @User_ID = @User_ID, @LogInID = @FacultyCode, @Password = @FacultyCode, @RoleID = @RoleId, @RoleExistStartDate = @CurrentDate, @RoleExistEndDate = @ExpireDate, @IsActive = 'False', @CreatedBy = -1, @CreatedDate = @CurrentDate;
				Insert Into UIUEMS_AD_UsrPermsn(User_ID, AccessIDPattern, AccessStartDate, AccessEndDate, CreatedBy, CreatedDate, PersonID) Values(@User_ID, '', @CurrentDate, @ExpireDate, -2, @CurrentDate, @PersonId);
				Print(@User_ID);
			End
			Else
				Print(@FacultyCode + ' Is Exist.');

			Set @FacultyCode = NULL; Set @PersonId = NULL;
			Fetch Next From FacultyList Into @PersonId, @FacultyCode;

		End
		Close FacultyList
		Deallocate FacultyList
	End
	Else
		Print('Role Not Found.');

End


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserCreateAndAccessRelation_For_Student]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===========================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-07-23 >
-- Description:	<Softwar Eng.>
-- ===========================

CREATE PROCEDURE [dbo].[UIUEMS_AD_UserCreateAndAccessRelation_For_Student]
As
Begin

	Declare @RoleId Int, @CurrentDate Datetime, @ExpireDate Datetime;
	Set @CurrentDate = GETDATE();
	Set @ExpireDate = '2020-12-30';
	Set @RoleId = NULL;
	Select @RoleId = ID From UIUEMS_AD_Role Where RoleName = 'Student';
	If @RoleId IS NOT NULL
	Begin

		Declare StudentIdList Cursor
		For
		Select p.PersonID, s.Roll From UIUEMS_ER_Student s, UIUEMS_ER_Person p Where s.PersonID = p.PersonID;

		Declare @StudentRoll nvarchar(15), @PersonId Int;
		Open StudentIdList;
		Fetch Next From StudentIdList Into @PersonId, @StudentRoll;
		While @@FETCH_STATUS = 0
		Begin

			Declare @ExistStatus Int;
			Set @ExistStatus = 0;
			Select @ExistStatus = Count(*) From UIUEMS_AD_User Where LogInID = @StudentRoll;
			If @ExistStatus = 0
			Begin
				Declare @User_ID Int;
				Select @User_ID = Max(User_ID) From UIUEMS_AD_User;
				Set @User_ID += 1;

				EXEC [dbo].[UIUEMS_AD_User_Insert] @User_ID = @User_ID, @LogInID = @StudentRoll, @Password = @StudentRoll, @RoleID = @RoleId, @RoleExistStartDate = @CurrentDate, @RoleExistEndDate = @ExpireDate,  @IsActive = 'False', @CreatedBy = -1, @CreatedDate = @CurrentDate;
				Insert Into UIUEMS_AD_UsrPermsn(User_ID, AccessIDPattern, AccessStartDate, AccessEndDate, CreatedBy, CreatedDate, PersonID) Values(@User_ID, @StudentRoll, @CurrentDate, @ExpireDate, -2,  @CurrentDate, @PersonId);
				--Print(@User_ID);
			End
			Else
				Print(@StudentRoll + ' Is Exist.');

			Set @StudentRoll = NULL; Set @PersonId = NULL;
			Fetch Next From StudentIdList Into @PersonId, @StudentRoll;

		End
		Close StudentIdList
		Deallocate StudentIdList
	End
	Else
		Print('Role Not Found.');

End


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserCreateAndAccessRelation_For_Student_Extra]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ===========================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-07-23 >
-- Description:	<Softwar Eng.>
-- ===========================

CREATE PROCEDURE [dbo].[UIUEMS_AD_UserCreateAndAccessRelation_For_Student_Extra]
As
Begin

	Declare @RoleId Int, @CurrentDate Datetime, @ExpireDate Datetime;
	Set @CurrentDate = GETDATE();
	Set @ExpireDate = '2020-12-30';
	Set @RoleId = NULL;
	Select @RoleId = ID From UIUEMS_AD_Role Where RoleName = 'Student';
	If @RoleId IS NOT NULL
	Begin

		Declare StudentIdList Cursor
		For
		Select p.PersonID, s.Roll From UIUEMS_ER_Student s, UIUEMS_ER_Person p Where s.PersonID = p.PersonID;

		Declare @StudentRoll nvarchar(15), @PersonId Int;
		Open StudentIdList;
		Fetch Next From StudentIdList Into @PersonId, @StudentRoll;
		While @@FETCH_STATUS = 0
		Begin

			Declare @ExistStatus Int;
			Set @ExistStatus = 0;
			Select @ExistStatus = Count(*) From UIUEMS_AD_User Where LogInID = @StudentRoll;
			If @ExistStatus = 0
			Begin
				Declare @User_ID Int; Set @User_ID = NULL;
				Select @User_ID = Max(User_ID) From UIUEMS_AD_User;
				Set @User_ID += 1;

				EXEC [dbo].[UIUEMS_AD_User_Insert] @User_ID = @User_ID, @LogInID = @User_ID, @Password = @StudentRoll, @RoleID = @RoleId, @RoleExistStartDate = @CurrentDate, @RoleExistEndDate = @ExpireDate,  @IsActive = 'False', @CreatedBy = -1, @CreatedDate = @CurrentDate;
				Insert Into UIUEMS_AD_UsrPermsn(User_ID, AccessIDPattern, AccessStartDate, AccessEndDate, CreatedBy, CreatedDate, PersonID) Values(@User_ID, @StudentRoll, @CurrentDate, @ExpireDate, -2,  @CurrentDate, @PersonId);
				Print(@StudentRoll + ' Insert User + UsrPer');
			End
			Else
			Begin
				Set @User_ID = NULL;
				Select @User_ID = User_ID From UIUEMS_AD_User Where LogInID = @StudentRoll and RoleId = 2;
				Declare @flagUsrPer Int; Set @flagUsrPer = 0;
				Select @flagUsrPer = Count(*) From UIUEMS_AD_UsrPermsn Where USER_ID = @User_ID;

				If @flagUsrPer = 0
				Begin
					Insert Into UIUEMS_AD_UsrPermsn(User_ID, AccessIDPattern, AccessStartDate, AccessEndDate, CreatedBy, CreatedDate, PersonID) Values(@User_ID, @StudentRoll, @CurrentDate, @ExpireDate, -2,  @CurrentDate, @PersonId);
					Print(@StudentRoll + ' Insert UsrPer');
				End
				Else
					Print(@StudentRoll + ' Is Exist.');
			End

			Set @StudentRoll = NULL; Set @PersonId = NULL;
			Fetch Next From StudentIdList Into @PersonId, @StudentRoll;

		End
		Close StudentIdList
		Deallocate StudentIdList
	End
	Else
		Print('Role Not Found.');

End


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_UserDeleteById]
(
@User_ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_User]
WHERE User_ID = @User_ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_User


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserGetById]
(
@User_ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_User
WHERE     (User_ID = @User_ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserGetByLogInId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserGetByLogInId]
(
@LogInID varchar(50) = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_User
WHERE     (LogInID = @LogInID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserGetByPersonId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-12-02 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserGetByPersonId]
(
	@PersonID int = null
)

As
Begin

Set Nocount On;

Select u.* From UIUEMS_AD_User u, UIUEMS_AD_UserInPerson up Where u.User_ID = up.User_ID And up.PersonID = @PersonID;

End



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserInPersonGetAllByPersonId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-11-05 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserInPersonGetAllByPersonId]
(
@id int = null
)

As
Begin

Set Nocount On;

Select * From UIUEMS_AD_UserInPerson Where (PersonID = @id)

End



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserInPersonInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserInPersonInsert] 
(
	@User_ID int = NULL,
	@PersonID varchar(50)  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT OFF;

INSERT INTO UIUEMS_AD_UserInPerson
(
	[User_ID],
	[PersonID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@User_ID,
	@PersonID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserInsert] 
(
	@User_ID int  OUTPUT,
	@LogInID varchar(50)  = NULL,
	@Password varchar(50)  = NULL,
	@RoleID int = NULL,
	@RoleExistStartDate datetime = NULL,
	@RoleExistEndDate datetime = NULL,
	@IsActive bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

Select @User_ID = MAX([User_ID]) From UIUEMS_AD_User;
Set @User_ID = @User_ID + 1;

INSERT INTO [UIUEMS_AD_User]
(
	[User_ID],
	[LogInID],
	[Password],
	[RoleID],
	[RoleExistStartDate],
	[RoleExistEndDate],
	[IsActive],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@User_ID,
	@LogInID,
	@Password,
	@RoleID,
	@RoleExistStartDate,
	@RoleExistEndDate,
	@IsActive,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @User_ID = @User_ID;
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UserUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UserUpdate]
(
	@User_ID int  = NULL,
	@LogInID varchar(50)  = NULL,
	@Password varchar(50)  = NULL,
	@RoleID int = NULL,
	@RoleExistStartDate datetime = NULL,
	@RoleExistEndDate datetime = NULL,
	@IsActive bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_AD_User]
   SET

	[LogInID]	=	@LogInID,
	[Password]	=	@Password,
	[RoleID]	=	@RoleID,
	[RoleExistStartDate]	=	@RoleExistStartDate,
	[RoleExistEndDate]	=	@RoleExistEndDate,
	[IsActive]	=	@IsActive,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE User_ID = @User_ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UsrPermsnDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AD_UsrPermsnDeleteById]
(
@UsrPermsn_ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AD_UsrPermsn]
WHERE UsrPermsn_ID = @UsrPermsn_ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UsrPermsnGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UsrPermsnGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_UsrPermsn


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UsrPermsnGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UsrPermsnGetById]
(
@UsrPermsn_ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AD_UsrPermsn
WHERE     (UsrPermsn_ID = @UsrPermsn_ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UsrPermsnInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UsrPermsnInsert] 
(
	@UsrPermsn_ID int  OUTPUT,
	@User_ID int  = NULL,
	@AccessIDPattern varchar(500)  = NULL,
	@AccessStartDate datetime = NULL,
	@AccessEndDate datetime = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AD_UsrPermsn]
(
	[User_ID],
	[AccessIDPattern],
	[AccessStartDate],
	[AccessEndDate],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@User_ID,
	@AccessIDPattern,
	@AccessStartDate,
	@AccessEndDate,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @UsrPermsn_ID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AD_UsrPermsnUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AD_UsrPermsnUpdate]
(
	@UsrPermsn_ID int  = NULL,
	@User_ID int  = NULL,
	@AccessIDPattern varchar(500)  = NULL,
	@AccessStartDate datetime = NULL,
	@AccessEndDate datetime = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AD_UsrPermsn]
   SET


	[User_ID]	=	@User_ID,
	[AccessIDPattern]	=	@AccessIDPattern,
	[AccessStartDate]	=	@AccessStartDate,
	[AccessEndDate]	=	@AccessEndDate,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE UsrPermsn_ID = @UsrPermsn_ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrDetailDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AM_FrmDsnrDetailDeleteById]
(
@FrmDsnrDetail_ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AM_FrmDsnrDetail]
WHERE FrmDsnrDetail_ID = @FrmDsnrDetail_ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrDetailGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrDetailGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AM_FrmDsnrDetail


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrDetailGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrDetailGetById]
(
@FrmDsnrDetail_ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AM_FrmDsnrDetail
WHERE     (FrmDsnrDetail_ID = @FrmDsnrDetail_ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrDetailInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrDetailInsert] 
(
	@FrmDsnrDetail_ID int  OUTPUT,
	@FrmDsnrMaster_ID int  = NULL,
	@FieldName varchar(150)  = NULL,
	@FieldType varchar(50)  = NULL,
	@FieldPosition int  = NULL,
	@IsAdmitField bit = NULL,
	@AdmitPosition int = NULL,
	@TableColName varchar(500) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AM_FrmDsnrDetail]
(
	[FrmDsnrDetail_ID],
	[FrmDsnrMaster_ID],
	[FieldName],
	[FieldType],
	[FieldPosition],
	[IsAdmitField],
	[AdmitPosition],
	[TableColName],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@FrmDsnrDetail_ID,
	@FrmDsnrMaster_ID,
	@FieldName,
	@FieldType,
	@FieldPosition,
	@IsAdmitField,
	@AdmitPosition,
	@TableColName,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @FrmDsnrDetail_ID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrDetailUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrDetailUpdate]
(
	@FrmDsnrDetail_ID int  = NULL,
	@FrmDsnrMaster_ID int  = NULL,
	@FieldName varchar(150)  = NULL,
	@FieldType varchar(50)  = NULL,
	@FieldPosition int  = NULL,
	@IsAdmitField bit = NULL,
	@AdmitPosition int = NULL,
	@TableColName varchar(500) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AM_FrmDsnrDetail]
   SET


	[FrmDsnrMaster_ID]	=	@FrmDsnrMaster_ID,
	[FieldName]	=	@FieldName,
	[FieldType]	=	@FieldType,
	[FieldPosition]	=	@FieldPosition,
	[IsAdmitField]	=	@IsAdmitField,
	[AdmitPosition]	=	@AdmitPosition,
	[TableColName]	=	@TableColName,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE FrmDsnrDetail_ID = @FrmDsnrDetail_ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_AM_FrmDsnrMasterDeleteById]
(
@FrmDsnrMaster_ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_AM_FrmDsnrMaster]
WHERE FrmDsnrMaster_ID = @FrmDsnrMaster_ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AM_FrmDsnrMaster


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrMasterGetById]
(
@FrmDsnrMaster_ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_AM_FrmDsnrMaster
WHERE     (FrmDsnrMaster_ID = @FrmDsnrMaster_ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrMasterInsert] 
(
	@FrmDsnrMaster_ID int  OUTPUT,
	@TrimesterID int = NULL,
	@FrmTableName varchar(500) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_AM_FrmDsnrMaster]
(
	[FrmDsnrMaster_ID],
	[TrimesterID],
	[FrmTableName],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@FrmDsnrMaster_ID,
	@TrimesterID,
	@FrmTableName,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @FrmDsnrMaster_ID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_AM_FrmDsnrMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_AM_FrmDsnrMasterUpdate]
(
	@FrmDsnrMaster_ID int  = NULL,
	@TrimesterID int = NULL,
	@FrmTableName varchar(500) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_AM_FrmDsnrMaster]
   SET

	[TrimesterID]	=	@TrimesterID,
	[FrmTableName]	=	@FrmTableName,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE FrmDsnrMaster_ID = @FrmDsnrMaster_ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_BillViewDeleteById]
(
	@BillViewId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_BillView]
WHERE BillViewId = @BillViewId

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewDeleteByStdSessCrsVer]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_BillViewDeleteByStdSessCrsVer]
(
	@StudentID int=NULL,
	@SessionId int = null,
	@CourseID int = null,
	@VersionID int =null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_BillView]
WHERE StudentId = @StudentID and TrimesterId = @SessionId and CourseID = @CourseID and  VersionID = @VersionID

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_BL_BillView

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewGetById]
(
	@BillViewId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_BillView
WHERE     (BillViewId = @BillViewId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewGetByStudent]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewGetByStudent]
(
	@StudentId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_BL_BillView
WHERE     (StudentId = @StudentId) 

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewGetByStudentAccountSession]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewGetByStudentAccountSession]
(
	@StudentId int=NULL,
	@AccountsID int=NULL,
	@SessionId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_BL_BillView
WHERE     (StudentId = @StudentId) and (AccountsID = @AccountsID) and (TrimesterId = @SessionId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewGetByStudentAccountSessionCourseVersion]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewGetByStudentAccountSessionCourseVersion]
(
	@StudentId int=NULL,
	@AccountsID int=NULL,
	@SessionId int=NULL,
	@CourseId int=NULL,
	@VersionId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_BL_BillView
WHERE     (StudentId = @StudentId) and (AccountsID = @AccountsID) and (TrimesterId = @SessionId) and (CourseId = @CourseId) and (VersionId = @VersionId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewGetByStudentSession]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewGetByStudentSession]
(
	@StudentId int=NULL,
	@SessionId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_BL_BillView
WHERE     (StudentId = @StudentId) and (TrimesterId = @SessionId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewInsert]
(
	@BillViewId int OUTPUT,
	@StudentId int = NULL,
	@CourseId int = NULL,
	@VersionId int = NULL,
	@TrimesterId int = NULL,
	@Amount numeric(18,2) = null,
	@Purpose nvarchar(500) = NULL,
	@AccountsID int = NULL,
	@AmountByCollectiveDiscount numeric(18,2) = null,
	@AmountByIterativeDiscount numeric(18,2) = null
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_BillView]
(
	[StudentId],
	[CourseId],
	[VersionId],
	[TrimesterId],
	[Amount],
	[Purpose],
	[AccountsID],
	[AmountByCollectiveDiscount],
	[AmountByIterativeDiscount]
)
 VALUES
(
	@StudentId,
	@CourseId,
	@VersionId,
	@TrimesterId,
	@Amount,
	@Purpose,
	@AccountsID,
	@AmountByCollectiveDiscount,
	@AmountByIterativeDiscount
)           
SET @BillViewId = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_BillViewUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_BillViewUpdate]
(
	@BillViewId int = NULL,
	@StudentId int = NULL,
	@CourseId int = NULL,
	@VersionId int = NULL,
	@TrimesterId int = NULL,
	@Amount numeric(18,2) = null,
	@Purpose nvarchar = NULL,
	@AccountsID int = NULL,	
	@AmountByCollectiveDiscount numeric(18,2) = null,
	@AmountByIterativeDiscount numeric(18,2) = null
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_BillView]
   SET

	[StudentId]=@StudentId,
	[CourseId]=@CourseId,
	[VersionId]=@VersionId,
	[TrimesterId]=@TrimesterId,
	[Amount]=@Amount,
	[Purpose]=@Purpose,
	[AccountsID] = @AccountsID,
	[AmountByCollectiveDiscount] = @AmountByCollectiveDiscount,
	[AmountByIterativeDiscount] = @AmountByIterativeDiscount

WHERE BillViewId = @BillViewId
           
END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_DiscountContinuationSetupDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_DiscountContinuationSetupDeleteById]
(
@DiscountContinuationID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_DiscountContinuationSetup]
WHERE DiscountContinuationID = @DiscountContinuationID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_DiscountContinuationSetupGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_DiscountContinuationSetupGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_DiscountContinuationSetup


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_DiscountContinuationSetupGetAllByBatchProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_DiscountContinuationSetupGetAllByBatchProgram]
(
	@BatchId int = null,
	@ProgramId int = null
)

AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_BL_DiscountContinuationSetup Where BatchAcaCalID = @BatchId and ProgramID = @ProgramId;

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_DiscountContinuationSetupGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_DiscountContinuationSetupGetById]
(
@DiscountContinuationID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_DiscountContinuationSetup
WHERE     (DiscountContinuationID = @DiscountContinuationID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_DiscountContinuationSetupInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_DiscountContinuationSetupInsert] 
(
	@DiscountContinuationID int  OUTPUT,
	@BatchAcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefinitionID int  = NULL,
	@MinCredits decimal(18, 2) = NULL,
	@MaxCredits decimal(18, 2) = NULL,
	@MinCGPA decimal(18, 2) = NULL,
	@Range nvarchar(50) = NULL,
	@PercentMin decimal(18, 2),
	@PercentMax decimal(18, 2),
	@Attribute1 nvarchar(max),
	@Attribute2 nvarchar(max),
	@Attribute3 nvarchar(max),
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_DiscountContinuationSetup]
(
	[BatchAcaCalID],
	[ProgramID],
	[TypeDefinitionID],
	[MinCredits],
	[MaxCredits],
	[MinCGPA],
	[Range],
	[PercentMin],
	[PercentMax],
	[Attribute1],
	[Attribute2],
	[Attribute3],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@BatchAcaCalID,
	@ProgramID,
	@TypeDefinitionID,
	@MinCredits,
	@MaxCredits,
	@MinCGPA,
	@Range,
	@PercentMin,
	@PercentMax,
	@Attribute1,
	@Attribute2,
	@Attribute3,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @DiscountContinuationID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_DiscountContinuationSetupUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_DiscountContinuationSetupUpdate]
(
	@DiscountContinuationID int  = NULL,
	@BatchAcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefinitionID int  = NULL,
	@MinCredits decimal(18, 2) = NULL,
	@MaxCredits decimal(18, 2) = NULL,
	@MinCGPA decimal(18, 2) = NULL,
	@Range nvarchar(50) = NULL,
	@PercentMin decimal(18, 2) = NULL,
	@PercentMax decimal(18, 2) = NULL,
	@Attribute1 nvarchar(max) = NULL,
	@Attribute2 nvarchar(max) = NULL,
	@Attribute3 nvarchar(max) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_BL_DiscountContinuationSetup]
   SET

	[BatchAcaCalID]	=	@BatchAcaCalID,
	[ProgramID]	=	@ProgramID,
	[TypeDefinitionID]	=	@TypeDefinitionID,
	[MinCredits]	=	@MinCredits,
	[MaxCredits]	=	@MaxCredits,
	[MinCGPA]	=	@MinCGPA,
	[Range]	=	@Range,
	[PercentMin] = @PercentMin,
	[PercentMax] = @PercentMax,
	[Attribute1] = @Attribute1,
	[Attribute2] = @Attribute2,
	[Attribute3] = @Attribute3,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE DiscountContinuationID = @DiscountContinuationID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_FeeSetupDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_FeeSetupDeleteById]
(
	@FeeSetupID int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_FeeSetup]
WHERE FeeSetupID = @FeeSetupID

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_FeeSetupGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_FeeSetupGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_FeeSetup


END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_FeeSetupGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_FeeSetupGetById]
(
	@FeeSetupID int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_FeeSetup
WHERE     (FeeSetupID = @FeeSetupID)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_FeeSetupInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_FeeSetupInsert]
(
	@FeeSetupID int OUTPUT,
	@AcaCalID int = NULL,
	@ProgramID int = NULL,
	@TypeDefID int = NULL,
	@Amount decimal(18,2) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_FeeSetup]
(
	[AcaCalID],
	[ProgramID],
	[TypeDefID],
	[Amount],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@AcaCalID,
	@ProgramID,
	@TypeDefID,
	@Amount,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @FeeSetupID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_FeeSetupUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_FeeSetupUpdate]
(
	@FeeSetupID int = NULL,
	@AcaCalID int = NULL,
	@ProgramID int = NULL,
	@TypeDefID int = NULL,
	@Amount decimal(18,2) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_FeeSetup]
   SET

	[AcaCalID]=@AcaCalID,
	[ProgramID]=@ProgramID,
	[TypeDefID]=@TypeDefID,
	[Amount]=@Amount,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE FeeSetupID = @FeeSetupID
           
END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_IsCourseBillableDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_IsCourseBillableDeleteById]
(
@BillableCourseID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_IsCourseBillable]
WHERE BillableCourseID = @BillableCourseID

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_IsCourseBillableGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_IsCourseBillableGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_IsCourseBillable


END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_IsCourseBillableGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_IsCourseBillableGetById]
(
@BillableCourseID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_IsCourseBillable
WHERE     (BillableCourseID = @BillableCourseID)

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_IsCourseBillableInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_IsCourseBillableInsert] 
(
	@BillableCourseID int   OUTPUT,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@BillStartFromRetakeNo int  = NULL,
	@IsCreditCourse bit  = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_IsCourseBillable]
(
	[BillableCourseID],
	[AcaCalID],
	[ProgramID],
	[BillStartFromRetakeNo],
	[IsCreditCourse],
	[CourseID],
	[VersionID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@BillableCourseID,
	@AcaCalID,
	@ProgramID,
	@BillStartFromRetakeNo,
	@IsCreditCourse,
	@CourseID,
	@VersionID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @BillableCourseID = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_IsCourseBillableUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_IsCourseBillableUpdate]
(
	@BillableCourseID int   = NULL,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@BillStartFromRetakeNo int  = NULL,
	@IsCreditCourse bit  = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_IsCourseBillable]
   SET


	[AcaCalID]	=	@AcaCalID,
	[ProgramID]	=	@ProgramID,
	[BillStartFromRetakeNo]	=	@BillStartFromRetakeNo,
	[IsCreditCourse]	=	@IsCreditCourse,
	[CourseID]	=	@CourseID,
	[VersionID]	=	@VersionID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE BillableCourseID = @BillableCourseID
           
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeDeleteById]
(
@RelationBetweenDiscountCourseTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_RelationBetweenDiscountCourseType]
WHERE RelationBetweenDiscountCourseTypeID = @RelationBetweenDiscountCourseTypeID

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_RelationBetweenDiscountCourseType


END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeGetById]
(
@RelationBetweenDiscountCourseTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_RelationBetweenDiscountCourseType
WHERE     (RelationBetweenDiscountCourseTypeID = @RelationBetweenDiscountCourseTypeID)

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeInsert] 
(
	@RelationBetweenDiscountCourseTypeID int   OUTPUT,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefDiscountID int  = NULL,
	@TypeDefCourseID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_RelationBetweenDiscountCourseType]
(
	[RelationBetweenDiscountCourseTypeID],
	[AcaCalID],
	[ProgramID],
	[TypeDefDiscountID],
	[TypeDefCourseID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@RelationBetweenDiscountCourseTypeID,
	@AcaCalID,
	@ProgramID,
	@TypeDefDiscountID,
	@TypeDefCourseID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @RelationBetweenDiscountCourseTypeID = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountCourseTypeUpdate]
(
	@RelationBetweenDiscountCourseTypeID int   = NULL,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefDiscountID int  = NULL,
	@TypeDefCourseID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_RelationBetweenDiscountCourseType]
   SET


	[AcaCalID]	=	@AcaCalID,
	[ProgramID]	=	@ProgramID,
	[TypeDefDiscountID]	=	@TypeDefDiscountID,
	[TypeDefCourseID]	=	@TypeDefCourseID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate



WHERE RelationBetweenDiscountCourseTypeID = @RelationBetweenDiscountCourseTypeID
           
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeDeleteById]
(
@RelationBetweenDiscountRetakeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_RelationBetweenDiscountRetake]
WHERE RelationBetweenDiscountRetakeID = @RelationBetweenDiscountRetakeID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_RelationBetweenDiscountRetake


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeGetById]
(
@RelationBetweenDiscountRetakeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_RelationBetweenDiscountRetake
WHERE     (RelationBetweenDiscountRetakeID = @RelationBetweenDiscountRetakeID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeInsert] 
(
	@RelationBetweenDiscountRetakeID int  OUTPUT,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefDiscountID int  = NULL,
	@RetakeEqualsToZero bit  = NULL,
	@RetakeGreaterThanZero bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_RelationBetweenDiscountRetake]
(
	[RelationBetweenDiscountRetakeID],
	[AcaCalID],
	[ProgramID],
	[TypeDefDiscountID],
	[RetakeEqualsToZero],
	[RetakeGreaterThanZero],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@RelationBetweenDiscountRetakeID,
	@AcaCalID,
	@ProgramID,
	@TypeDefDiscountID,
	@RetakeEqualsToZero,
	@RetakeGreaterThanZero,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @RelationBetweenDiscountRetakeID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountRetakeUpdate]
(
	@RelationBetweenDiscountRetakeID int  = NULL,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefDiscountID int  = NULL,
	@RetakeEqualsToZero bit  = NULL,
	@RetakeGreaterThanZero bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_RelationBetweenDiscountRetake]
   SET


	[AcaCalID]	=	@AcaCalID,
	[ProgramID]	=	@ProgramID,
	[TypeDefDiscountID]	=	@TypeDefDiscountID,
	[RetakeEqualsToZero]	=	@RetakeEqualsToZero,
	[RetakeGreaterThanZero]	=	@RetakeGreaterThanZero,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE RelationBetweenDiscountRetakeID = @RelationBetweenDiscountRetakeID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeDeleteById]
(
@RelationBetweenDiscountSectionTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_RelationBetweenDiscountSectionType]
WHERE RelationBetweenDiscountSectionTypeID = @RelationBetweenDiscountSectionTypeID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_RelationBetweenDiscountSectionType


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeGetById]
(
@RelationBetweenDiscountSectionTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_RelationBetweenDiscountSectionType
WHERE     (RelationBetweenDiscountSectionTypeID = @RelationBetweenDiscountSectionTypeID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeInsert] 
(
	@RelationBetweenDiscountSectionTypeID int  OUTPUT,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefDiscountID int  = NULL,
	@TypeDefID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_RelationBetweenDiscountSectionType]
(
	[RelationBetweenDiscountSectionTypeID],
	[AcaCalID],
	[ProgramID],
	[TypeDefDiscountID],
	[TypeDefID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@RelationBetweenDiscountSectionTypeID,
	@AcaCalID,
	@ProgramID,
	@TypeDefDiscountID,
	@TypeDefID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @RelationBetweenDiscountSectionTypeID = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_RelationBetweenDiscountSectionTypeUpdate]
(
	@RelationBetweenDiscountSectionTypeID int  = NULL,
	@AcaCalID int  = NULL,
	@ProgramID int  = NULL,
	@TypeDefDiscountID int  = NULL,
	@TypeDefID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_RelationBetweenDiscountSectionType]
   SET

	[AcaCalID]	=	@AcaCalID,
	[ProgramID]	=	@ProgramID,
	[TypeDefDiscountID]	=	@TypeDefDiscountID,
	[TypeDefID]	=	@TypeDefID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE RelationBetweenDiscountSectionTypeID = @RelationBetweenDiscountSectionTypeID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetDeleteById]
(
@BillWorkSheetId int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_Std_Crs_Bill_Worksheet]
WHERE BillWorkSheetId = @BillWorkSheetId

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_Std_Crs_Bill_Worksheet


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetGetById]
(
@BillWorkSheetId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_Std_Crs_Bill_Worksheet
WHERE     (BillWorkSheetId = @BillWorkSheetId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetInsert] 
(
	@BillWorkSheetId int  OUTPUT,
	@StudentId int = NULL,
	@CalCourseProgNodeID int = NULL,
	@AcaCalSectionID int = NULL,
	@SectionTypeId int = NULL,
	@AcaCalId int = NULL,
	@CourseId int = NULL,
	@VersionId int = NULL,
	@CourseTypeId int = NULL,
	@ProgramId int = NULL,
	@RetakeNo int = NULL,
	@PreviousBestGrade varchar(2) = NULL,
	@FeeSetupId int = NULL,
	@Fee decimal(18, 2) = NULL,
	@DiscountTypeId int = NULL,
	@DiscountPercentage decimal(18, 2) = NULL,
	@Remarks varchar(max) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_Std_Crs_Bill_Worksheet]
(
	[BillWorkSheetId],
	[StudentId],
	[CalCourseProgNodeID],
	[AcaCalSectionID],
	[SectionTypeId],
	[AcaCalId],
	[CourseId],
	[VersionId],
	[CourseTypeId],
	[ProgramId],
	[RetakeNo],
	[PreviousBestGrade],
	[FeeSetupId],
	[Fee],
	[DiscountTypeId],
	[DiscountPercentage],
	[Remarks],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@BillWorkSheetId,
	@StudentId,
	@CalCourseProgNodeID,
	@AcaCalSectionID,
	@SectionTypeId,
	@AcaCalId,
	@CourseId,
	@VersionId,
	@CourseTypeId,
	@ProgramId,
	@RetakeNo,
	@PreviousBestGrade,
	@FeeSetupId,
	@Fee,
	@DiscountTypeId,
	@DiscountPercentage,
	@Remarks,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @BillWorkSheetId = SCOPE_IDENTITY()
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_Std_Crs_Bill_WorksheetUpdate]
(
	@BillWorkSheetId int  = NULL,
	@StudentId int = NULL,
	@CalCourseProgNodeID int = NULL,
	@AcaCalSectionID int = NULL,
	@SectionTypeId int = NULL,
	@AcaCalId int = NULL,
	@CourseId int = NULL,
	@VersionId int = NULL,
	@CourseTypeId int = NULL,
	@ProgramId int = NULL,
	@RetakeNo int = NULL,
	@PreviousBestGrade varchar(2) = NULL,
	@FeeSetupId int = NULL,
	@Fee decimal(18, 2) = NULL,
	@DiscountTypeId int = NULL,
	@DiscountPercentage decimal(18, 2) = NULL,
	@Remarks varchar(max) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_Std_Crs_Bill_Worksheet]
   SET


	[StudentId]	=	@StudentId,
	[CalCourseProgNodeID]	=	@CalCourseProgNodeID,
	[AcaCalSectionID]	=	@AcaCalSectionID,
	[SectionTypeId]	=	@SectionTypeId,
	[AcaCalId]	=	@AcaCalId,
	[CourseId]	=	@CourseId,
	[VersionId]	=	@VersionId,
	[CourseTypeId]	=	@CourseTypeId,
	[ProgramId]	=	@ProgramId,
	[RetakeNo]	=	@RetakeNo,
	[PreviousBestGrade]	=	@PreviousBestGrade,
	[FeeSetupId]	=	@FeeSetupId,
	[Fee]	=	@Fee,
	[DiscountTypeId]	=	@DiscountTypeId,
	[DiscountPercentage]	=	@DiscountPercentage,
	[Remarks]	=	@Remarks,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE BillWorkSheetId = @BillWorkSheetId
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionCountByProgramBatch]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionCountByProgramBatch]
(@AcaCalSession int = null)

AS
BEGIN
SET NOCOUNT ON;

	SELECT COUNT( DISTINCT s.StudentID) as StudentCount, s.ProgramID, p.ShortName as Program, 
	a.AdmissionCalenderID as AcaCalId , ac.BatchCode, ac.Year,ut.TypeName as UnitTypeName
	FROM UIUEMS_BL_StudentDiscountAndScholarshipPerSession ds 
	INNER JOIN UIUEMS_ER_Student as s on s.StudentID = ds.StudentId 
	INNER JOIN UIUEMS_ER_Admission as a on a.StudentID = s.StudentID
	INNER JOIN UIUEMS_CC_Program as p on p.ProgramID = s.ProgramID
	INNER JOIN UIUEMS_CC_AcademicCalender as ac on ac.AcademicCalenderID = a.AdmissionCalenderID
	inner join UIUEMS_CC_CalenderUnitType as ut on ut.CalenderUnitTypeID = ac.CalenderUnitTypeID
	where ds.AcaCalSession = @AcaCalSession
	GROUP BY s.ProgramID, a.AdmissionCalenderID,p.ShortName,ac.BatchCode,ac.Year,ut.TypeName 

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionDeleteById]
(
	@StudentDiscountAndScholarshipId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_StudentDiscountAndScholarshipPerSession]
WHERE StudentDiscountAndScholarshipId = @StudentDiscountAndScholarshipId

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionDeleteByStudentIdSessionIdTdId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionDeleteByStudentIdSessionIdTdId]
(
	@StudentId int=NULL,
	@SessionId int=NULL,
	@TdId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_StudentDiscountAndScholarshipPerSession]
WHERE StudentId = @StudentId AND  AcaCalSession = @SessionId and TypeDefinitionId = @TdId

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountAndScholarshipPerSession


END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionGetById]
(
	@StudentDiscountAndScholarshipId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountAndScholarshipPerSession
WHERE     (StudentDiscountAndScholarshipId = @StudentDiscountAndScholarshipId)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionInsert]
(
	@StudentDiscountAndScholarshipId int OUTPUT,
	@StudentId int = NULL,
	@TypeDefinitionId int = NULL,
	@Discount numeric(18,2) = NULL,
	@AcaCalSession int = NULL,
	@Remarks nvarchar(1000) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSession]
(
	[StudentId],
	[TypeDefinitionId],
	[Discount],
	[AcaCalSession],
	[Remarks],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@StudentId,
	@TypeDefinitionId,
	@Discount,
	@AcaCalSession,
	@Remarks,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @StudentDiscountAndScholarshipId = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountAndScholarshipPerSessionUpdate]
(
	@StudentDiscountAndScholarshipId int = NULL,
	@StudentId int = NULL,
	@TypeDefinitionId int = NULL,
	@Discount numeric(18,2) = NULL,
	@AcaCalSession int = NULL,
	@Remarks nvarchar(1000) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_StudentDiscountAndScholarshipPerSession]
   SET

	[StudentId]=@StudentId,
	[TypeDefinitionId]=@TypeDefinitionId,
	[Discount]=@Discount,
	[AcaCalSession]=@AcaCalSession,
	[Remarks]=@Remarks,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE StudentDiscountAndScholarshipId = @StudentDiscountAndScholarshipId
           
END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsByStudentDiscountIdAndAcaCalSessionId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsByStudentDiscountIdAndAcaCalSessionId]
(
	@StudentDiscountId int=NULL,
	@AcaCalSessionId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * 
FROM       UIUEMS_BL_StudentDiscountCurrentDetails
WHERE     (StudentDiscountId = @StudentDiscountId and AcaCalSession = @AcaCalSessionId)

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsDeleteById]
(
	@StudentDiscountCurrentDetailsId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_StudentDiscountCurrentDetails]
WHERE StudentDiscountCurrentDetailsId = @StudentDiscountCurrentDetailsId

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGenetareCurrentDiscount]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGenetareCurrentDiscount]
(
	@BatchId int = NULL,
	@SessionId int = NULL,
	@ProgramId int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

DELETE FROM UIUEMS_BL_StudentDiscountCurrentDetails WHERE StudentDiscountCurrentDetailsId IN 
(SELECT sdc.StudentDiscountCurrentDetailsId FROM UIUEMS_BL_StudentDiscountCurrentDetails AS sdc 
INNER JOIN UIUEMS_BL_StudentDiscountMaster AS sdm ON sdm.StudentDiscountId = sdc.StudentDiscountId
WHERE sdm.BatchId = @BatchId and sdm.ProgramId = @ProgramId and sdc.AcaCalSession=@SessionId )

INSERT INTO UIUEMS_BL_StudentDiscountCurrentDetails
SELECT sdi.StudentDiscountId,  sdi.TypeDefinitionId,  sdi.TypePercentage,  @SessionId 
FROM UIUEMS_BL_StudentDiscountInitialDetails as sdi INNER JOIN UIUEMS_BL_StudentDiscountMaster AS sdm ON sdm.StudentDiscountId = sdi.StudentDiscountId
WHERE sdm.BatchId = @BatchId and sdm.ProgramId = @ProgramId

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGenetareCurrentDiscountPerStudent]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGenetareCurrentDiscountPerStudent]
(
	@StudentId int = NULL,
	@BatchId int = NULL,
	@SessionId int = NULL,
	@ProgramId int = NULL
)

AS
BEGIN
SET NOCOUNT ON;


DELETE FROM UIUEMS_BL_StudentDiscountCurrentDetails WHERE StudentDiscountCurrentDetailsId IN 
(SELECT sdc.StudentDiscountCurrentDetailsId FROM UIUEMS_BL_StudentDiscountCurrentDetails AS sdc 
INNER JOIN UIUEMS_BL_StudentDiscountMaster AS sdm ON sdm.StudentDiscountId = sdc.StudentDiscountId
WHERE sdm.BatchId = @BatchId and sdm.ProgramId = @ProgramId and sdc.AcaCalSession=@SessionId and sdm.StudentId = @StudentId)


DECLARE @PreSessionCr int, @PreSessionCGPA decimal(18,2), @PreAcaCalId int, @StudentDiscountId int
 -- @StudentId int, @SessionId int, @BatchId int, @ProgramId int

set @PreAcaCalId = dbo.GetPreviousAcademicCalenderId((select BatchCode from UIUEMS_CC_AcademicCalender where AcademicCalenderID = @SessionId));
set @PreSessionCGPA = (select CGPA from UIUEMS_ER_Student_ACUDetail where StudentID = @StudentId and StdAcademicCalenderID = @PreAcaCalId);
set @PreSessionCr = (select SUM(CourseCredit) from UIUEMS_CC_Student_CourseHistory where StudentID= @StudentId and AcaCalID = @PreAcaCalId)
set @StudentDiscountId = (select StudentDiscountId from UIUEMS_BL_StudentDiscountMaster where StudentId = @StudentId)


----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
DECLARE
@di_StudentDiscountInitialDetailsId INT,
@di_StudentDiscountId INT, 
@di_TypeDefinitionId INT, 
@di_TypePercentage INT, 
@di_AcaCalSession INT,
@di_Index INT;


DECLARE @tmpDisInitial TABLE (
						[ID] INT IDENTITY(1,1),
						[StudentDiscountInitialDetailsId] INT,
						[StudentDiscountId] int, 
						[TypeDefinitionId] int, 
						[TypePercentage] int, 
						[AcaCalSession] int);
						
INSERT INTO @tmpDisInitial  SELECT * FROM UIUEMS_BL_StudentDiscountInitialDetails 
							WHERE StudentDiscountId = @StudentDiscountId

--select * from @tmpDisInitial
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

DECLARE 
@dc_DiscountContinuationID INT,
@dc_BatchAcaCalID INT,
@dc_ProgramID INT,
@dc_TypeDefinitionID  INT,
@dc_MinCredits DECIMAL(18,2), 
@dc_MaxCredits  DECIMAL(18,2),
@dc_MinCGPA DECIMAL(18,2),
@dc_PercentMin DECIMAL(18,2),
@dc_PercentMax  DECIMAL(18,2),
@dc_Index int;

DECLARE @tmpDisContinuation TABLE (
						[ID] INT IDENTITY(1,1),
						[DiscountContinuationID] INT,
						[BatchAcaCalID] int,
						[ProgramID] int,
						[TypeDefinitionID]  int,
						[MinCredits] decimal(18,2), 
						[MaxCredits]  decimal(18,2),
						[MinCGPA] decimal(18,2),
						[PercentMin] decimal(18,2),
						[PercentMax] decimal(18,2)
						);

INSERT INTO @tmpDisContinuation (DiscountContinuationID, 
								BatchAcaCalID, 
								ProgramID, 
								TypeDefinitionID, 
								MinCredits, 
								MaxCredits, 
								MinCGPA,
								PercentMin, 
								PercentMax)	
								SELECT DiscountContinuationID, 
								BatchAcaCalID, 
								ProgramID, 
								TypeDefinitionID, 
								MinCredits, 
								MaxCredits, 
								MinCGPA,
								PercentMin, 
								PercentMax
FROM            UIUEMS_BL_DiscountContinuationSetup
								WHERE  BatchAcaCalID = @BatchId and ProgramID = @ProgramId --and TypeDefinitionID = @di_TypeDefinitionId

--select * from @tmpDisContinuation
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------

DECLARE @OutputParam INT;

SET @di_Index = 1;
WHILE @di_Index <= (SELECT MAX(ID) FROM @tmpDisInitial)
	BEGIN 
		SELECT 
			@di_StudentDiscountInitialDetailsId = StudentDiscountInitialDetailsId,
			@di_StudentDiscountId = StudentDiscountId,
			@di_TypeDefinitionId = TypeDefinitionId, 
			@di_TypePercentage = TypePercentage, 
			@di_AcaCalSession  = AcaCalSession
			FROM @tmpDisInitial
			WHERE ID = @di_Index


			SET @dc_Index = 1;
			while @dc_Index <= (SELECT MAX(ID) from @tmpDisContinuation)
				BEGIN 
					SELECT
					@dc_DiscountContinuationID = DiscountContinuationID,
					@dc_BatchAcaCalID = BatchAcaCalID,
					@dc_ProgramID = ProgramID,
					@dc_TypeDefinitionID  = TypeDefinitionID,
					@dc_MinCredits = MinCredits, 
					@dc_MaxCredits  = MaxCredits,
					@dc_MinCGPA = MinCGPA,
					@dc_PercentMin = PercentMin,
					@dc_PercentMax = PercentMax
					FROM @tmpDisContinuation
					WHERE ID = @dc_Index

					-- if type defination not match, direct insert
					IF(@di_TypeDefinitionId != @dc_TypeDefinitionID)
						BEGIN
							EXEC UIUEMS_BL_StudentDiscountCurrentDetailsInsert 
																			@OutputParam output,
																			@di_StudentDiscountId,
																			@di_TypeDefinitionId,
																			@di_TypePercentage,
																			@SessionId

						END
					ELSE
						BEGIN
							IF( @PreSessionCr >= @dc_MinCredits and 
								@PreSessionCGPA >= @dc_MinCGPA and 
								@di_TypePercentage >= @dc_PercentMin and
								@di_TypePercentage <= @dc_PercentMax)
								BEGIN
									EXEC UIUEMS_BL_StudentDiscountCurrentDetailsInsert 
																				@OutputParam output,
																				@di_StudentDiscountId,
																				@di_TypeDefinitionId,
																				@di_TypePercentage,
																				@SessionId
								END
						END
				SET @dc_Index = @dc_Index + 1;
				END
		SET @di_Index = @di_Index + 1;
	END

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_BL_StudentDiscountCurrentDetails

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGetById]
(
	@StudentDiscountCurrentDetailsId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * 
FROM       UIUEMS_BL_StudentDiscountCurrentDetails
WHERE     (StudentDiscountCurrentDetailsId = @StudentDiscountCurrentDetailsId)

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGetByStudentDiscountId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsGetByStudentDiscountId]
(
	@StudentDiscountId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * 
FROM       UIUEMS_BL_StudentDiscountCurrentDetails
WHERE     (StudentDiscountId = @StudentDiscountId)

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsInsert]
(
	@StudentDiscountCurrentDetailsId int OUTPUT,
	@StudentDiscountId int = NULL,
	@TypeDefinitionId int = NULL,
	@TypePercentage numeric(18,2) = NULL,
	@AcaCalSession int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_StudentDiscountCurrentDetails]
(
	[StudentDiscountId],
	[TypeDefinitionId],
	[TypePercentage],
	[AcaCalSession]
)
 VALUES
(
	@StudentDiscountId,
	@TypeDefinitionId,
	@TypePercentage,
	@AcaCalSession
)           
SET @StudentDiscountCurrentDetailsId = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountCurrentDetailsUpdate]
(
	@StudentDiscountCurrentDetailsId int = NULL,
	@StudentDiscountId int = NULL,
	@TypeDefinitionId int = NULL,
	@TypePercentage numeric(18,2) = NULL,
	@AcaCalSession int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_StudentDiscountCurrentDetails]
   SET

	[StudentDiscountId]=@StudentDiscountId,
	[TypeDefinitionId]=@TypeDefinitionId,
	[TypePercentage]=@TypePercentage,
	[AcaCalSession]=@AcaCalSession

WHERE StudentDiscountCurrentDetailsId = @StudentDiscountCurrentDetailsId
           
END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsDeleteById]
(
	@StudentDiscountInitialDetailsId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_StudentDiscountInitialDetails]
WHERE StudentDiscountInitialDetailsId = @StudentDiscountInitialDetailsId

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT  * FROM       UIUEMS_BL_StudentDiscountInitialDetails

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsGetById]
(
	@StudentDiscountInitialDetailsId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * 
FROM       UIUEMS_BL_StudentDiscountInitialDetails
WHERE     (StudentDiscountInitialDetailsId = @StudentDiscountInitialDetailsId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsGetByStudentDiscountId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsGetByStudentDiscountId]
(
	@StudentDiscountId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * 
FROM       UIUEMS_BL_StudentDiscountInitialDetails
WHERE     (StudentDiscountId = @StudentDiscountId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsInsert]
(
	@StudentDiscountInitialDetailsId int OUTPUT,
	@StudentDiscountId int = NULL,
	@TypeDefinitionId int = NULL,
	@TypePercentage numeric(18,2) = null,
	@AcaCalSession int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_StudentDiscountInitialDetails]
(
	[StudentDiscountId],
	[TypeDefinitionId],
	[TypePercentage],
	[AcaCalSession]
)
 VALUES
(
	@StudentDiscountId,
	@TypeDefinitionId,
	@TypePercentage,
	@AcaCalSession
)           
SET @StudentDiscountInitialDetailsId = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountInitialDetailsUpdate]
(
	@StudentDiscountInitialDetailsId int = NULL,
	@StudentDiscountId int = NULL,
	@TypeDefinitionId int = NULL,
	@TypePercentage numeric(18,2) = null,
	@AcaCalSession int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_StudentDiscountInitialDetails]
   SET

	[StudentDiscountId]=@StudentDiscountId,
	[TypeDefinitionId]=@TypeDefinitionId,
	[TypePercentage]=@TypePercentage,
	[AcaCalSession]=@AcaCalSession

WHERE StudentDiscountInitialDetailsId = @StudentDiscountInitialDetailsId
           
END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_StudentDiscountMasterDeleteById]
(
	@StudentDiscountId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_StudentDiscountMaster]
WHERE StudentDiscountId = @StudentDiscountId

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountMaster


END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterGetByAcaCalIDProgramID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountMasterGetByAcaCalIDProgramID]
(
	@AcaCalID int=NULL,
	@ProgramID int=NULL
)

AS
BEGIN
IF(@AcaCalID!=0 and @ProgramID!=0)
SELECT* FROM UIUEMS_BL_StudentDiscountMaster where BatchId=@AcaCalID and ProgramId=@ProgramID
Else IF(@ProgramID=0 and @AcaCalID!=0)
SELECT* FROM UIUEMS_BL_StudentDiscountMaster where BatchId=@AcaCalID
Else IF(@AcaCalID=0 and @ProgramID!=0)
SELECT* FROM UIUEMS_BL_StudentDiscountMaster where ProgramId=@ProgramID
END

GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountMasterGetById]
(
	@StudentDiscountId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountMaster
WHERE     (StudentDiscountId = @StudentDiscountId)

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterGetByStudentID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountMasterGetByStudentID]
(
	@StudentID int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountMaster
WHERE     (StudentId = @StudentID)

END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountMasterInsert]
(
	@StudentDiscountId int OUTPUT,
	@StudentId int = NULL,
	@BatchId int = NULL,
	@ProgramId int = NULL,
	@Remarks nvarchar(MAX) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_StudentDiscountMaster]
(
	[StudentId],
	[BatchId],
	[ProgramId],
	[Remarks],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@StudentId,
	@BatchId,
	@ProgramId,
	@Remarks,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @StudentDiscountId = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountMasterUpdate]
(
	@StudentDiscountId int = NULL,
	@StudentId int = NULL,
	@BatchId int = NULL,
	@ProgramId int = NULL,
	@Remarks nvarchar = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_StudentDiscountMaster]
   SET

	[StudentId]=@StudentId,
	[BatchId]=@BatchId,
	[ProgramId]=@ProgramId,
	[Remarks]=@Remarks,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE StudentDiscountId = @StudentDiscountId
           
END









GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountWorkSheetDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_BL_StudentDiscountWorkSheetDeleteById]
(
@StdDiscountWorksheetID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_StudentDiscountWorkSheet]
WHERE StdDiscountWorksheetID = @StdDiscountWorksheetID

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountWorkSheetGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountWorkSheetGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountWorkSheet


END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountWorkSheetGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountWorkSheetGetById]
(
@StdDiscountWorksheetID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_StudentDiscountWorkSheet
WHERE     (StdDiscountWorksheetID = @StdDiscountWorksheetID)

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountWorkSheetInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountWorkSheetInsert] 
(
	@StdDiscountWorksheetID int  OUTPUT,
	@StudentID int = NULL,
	@ProgramID int = NULL,
	@AcaCalID int = NULL,
	@AdmissionCalId int = NULL,
	@TotalCrRegInPreviousSession numeric(18, 2) = NULL,
	@GPAinPreviousSession numeric(18, 2) = NULL,
	@CGPAupToPreviousSession numeric(18, 2) = NULL,
	@TotalCrRegInCurrentSession numeric(18, 2) = NULL,
	@DiscountTypeId int = NULL,
	@DiscountPercentage numeric(18, 2) = NULL,
	@Remarks nvarchar(max) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_BL_StudentDiscountWorkSheet]
(
	[StdDiscountWorksheetID],
	[StudentID],
	[ProgramID],
	[AcaCalID],
	[AdmissionCalId],
	[TotalCrRegInPreviousSession],
	[GPAinPreviousSession],
	[CGPAupToPreviousSession],
	[TotalCrRegInCurrentSession],
	[DiscountTypeId],
	[DiscountPercentage],
	[Remarks],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@StdDiscountWorksheetID,
	@StudentID,
	@ProgramID,
	@AcaCalID,
	@AdmissionCalId,
	@TotalCrRegInPreviousSession,
	@GPAinPreviousSession,
	@CGPAupToPreviousSession,
	@TotalCrRegInCurrentSession,
	@DiscountTypeId,
	@DiscountPercentage,
	@Remarks,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @StdDiscountWorksheetID = SCOPE_IDENTITY()
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_StudentDiscountWorkSheetUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_BL_StudentDiscountWorkSheetUpdate]
(
	@StdDiscountWorksheetID int  = NULL,
	@StudentID int = NULL,
	@ProgramID int = NULL,
	@AcaCalID int = NULL,
	@AdmissionCalId int = NULL,
	@TotalCrRegInPreviousSession numeric(18, 2) = NULL,
	@GPAinPreviousSession numeric(18, 2) = NULL,
	@CGPAupToPreviousSession numeric(18, 2) = NULL,
	@TotalCrRegInCurrentSession numeric(18, 2) = NULL,
	@DiscountTypeId int = NULL,
	@DiscountPercentage numeric(18, 2) = NULL,
	@Remarks nvarchar(max) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_StudentDiscountWorkSheet]
   SET


	[StudentID]	=	@StudentID,
	[ProgramID]	=	@ProgramID,
	[AcaCalID]	=	@AcaCalID,
	[AdmissionCalId]	=	@AdmissionCalId,
	[TotalCrRegInPreviousSession]	=	@TotalCrRegInPreviousSession,
	[GPAinPreviousSession]	=	@GPAinPreviousSession,
	[CGPAupToPreviousSession]	=	@CGPAupToPreviousSession,
	[TotalCrRegInCurrentSession]	=	@TotalCrRegInCurrentSession,
	[DiscountTypeId]	=	@DiscountTypeId,
	[DiscountPercentage]	=	@DiscountPercentage,
	[Remarks]	=	@Remarks,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE StdDiscountWorksheetID = @StdDiscountWorksheetID
           
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_VoucherDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_BL_VoucherDeleteById]
(
	@VoucherID int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_BL_Voucher]
WHERE VoucherID = @VoucherID

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_VoucherGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_VoucherGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_Voucher


END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_VoucherGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_VoucherGetById]
(
	@VoucherID int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_BL_Voucher
WHERE     (VoucherID = @VoucherID)

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_VoucherInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_VoucherInsert]
(
	@VoucherID int OUTPUT,
	@VoucherCode nvarchar(50) = NULL,
	@Prefix nvarchar(50) = NULL,
	@SLNO bigint = NULL,
	@AccountHeadsID int = NULL,
	@AccountTypeID int = NULL,
	@Amount numeric(18,2) = NULL,
	@PostedBy varchar(50) = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Remarks nvarchar(1000) = NULL,
	@AcaCalID int = NULL,
	@ReferenceNo varchar(50) = NULL,
	@ChequeNo varchar(50) = NULL,
	@ChequeBankName varchar(150) = NULL,
	@ChequeDate datetime = NULL,
	@IsChequeCleare bit = NULL,
	@ChequeCleareDate datetime = NULL,
	@Adjust int = NULL,
	@GUID uniqueidentifier = null,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@Attribute1 int = NULL,
	@Attribute2 int = NULL,
	@Attribute3 int = NULL,
	@Attribute4 nvarchar(50) = NULL,
	@Attribute5 nvarchar(50) = NULL,
	@Attribute6 nvarchar(50) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_BL_Voucher]
(
	[VoucherCode],
	[Prefix],
	[SLNO],
	[AccountHeadsID],
	[AccountTypeID],
	[Amount],
	[PostedBy],
	[CourseID],
	[VersionID],
	[Remarks],
	[AcaCalID],
	[ReferenceNo],
	[ChequeNo],
	[ChequeBankName],
	[ChequeDate],
	[IsChequeCleare],
	[ChequeCleareDate],
	[Adjust],
	[GUID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[Attribute1],
	[Attribute2],
	[Attribute3],
	[Attribute4],
	[Attribute5],
	[Attribute6]
)
 VALUES
(
	@VoucherCode,
	@Prefix,
	@SLNO,
	@AccountHeadsID,
	@AccountTypeID,
	@Amount,
	@PostedBy,
	@CourseID,
	@VersionID,
	@Remarks,
	@AcaCalID,
	@ReferenceNo,
	@ChequeNo,
	@ChequeBankName,
	@ChequeDate,
	@IsChequeCleare,
	@ChequeCleareDate,
	@Adjust,
	@GUID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@Attribute1,
	@Attribute2,
	@Attribute3,
	@Attribute4,
	@Attribute5,
	@Attribute6
)           
SET @VoucherID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_BL_VoucherUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_BL_VoucherUpdate]
(
	@VoucherID int = NULL,
	@VoucherCode nvarchar(50) = NULL,
	@Prefix nvarchar(50) = NULL,
	@SLNO bigint = null,
	@AccountHeadsID int = NULL,
	@AccountTypeID int = NULL,
	@Amount numeric(18,2) = NULL,
	@PostedBy varchar(50) = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Remarks nvarchar(1000) = NULL,
	@AcaCalID int = NULL,
	@ReferenceNo varchar(50) = NULL,
	@ChequeNo varchar(50) = NULL,
	@ChequeBankName varchar(150) = NULL,
	@ChequeDate datetime = NULL,
	@IsChequeCleare bit = NULL,
	@ChequeCleareDate datetime = NULL,
	@Adjust int = NULL,
	@GUID uniqueidentifier = null,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@Attribute1 int = NULL,
	@Attribute2 int = NULL,
	@Attribute3 int = NULL,
	@Attribute4 nvarchar(50) = NULL,
	@Attribute5 nvarchar(50) = NULL,
	@Attribute6 nvarchar(50) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_BL_Voucher]
   SET

	[VoucherCode]=@VoucherCode,
	[Prefix]=@Prefix,
	[SLNO]=@SLNO,
	[AccountHeadsID]=@AccountHeadsID,
	[AccountTypeID]=@AccountTypeID,
	[Amount]=@Amount,
	[PostedBy]=@PostedBy,
	[CourseID]=@CourseID,
	[VersionID]=@VersionID,
	[Remarks]=@Remarks,
	[AcaCalID]=@AcaCalID,
	[ReferenceNo]=@ReferenceNo,
	[ChequeNo]=@ChequeNo,
	[ChequeBankName]=@ChequeBankName,
	[ChequeDate]=@ChequeDate,
	[IsChequeCleare]=@IsChequeCleare,
	[ChequeCleareDate]=@ChequeCleareDate,
	[Adjust]=@Adjust,
	[GUID]=@GUID,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate,
	[Attribute1]=@Attribute1,
	[Attribute2]=@Attribute2,
	[Attribute3]=@Attribute3,
	[Attribute4]=@Attribute4,
	[Attribute5]=@Attribute5,
	[Attribute6]=@Attribute6

WHERE VoucherID = @VoucherID
           
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_AcademicCalenderDeleteById]
(
@AcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_AcademicCalender]
WHERE AcademicCalenderID = @AcademicCalenderID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerDeleteById]
(
@AcademicCalenderExamSchedulerID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_AcademicCalenderExamScheduler]
WHERE AcademicCalenderExamSchedulerID = @AcademicCalenderExamSchedulerID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_AcademicCalenderExamScheduler


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerGetById]
(
@AcademicCalenderExamSchedulerID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_AcademicCalenderExamScheduler
WHERE     (AcademicCalenderExamSchedulerID = @AcademicCalenderExamSchedulerID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerInsert] 
(
	@AcademicCalenderExamSchedulerID int  OUTPUT,
	@AcaCal_SectionID int  = NULL,
	@RoomInfoOneID int  = NULL,
	@DayOne int  = NULL,
	@TimeSlotPlanOneID int  = NULL,
	@TeacherOneID int  = NULL,
	@TeacherTwoID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@TypeDefinitionID int = NULL,
	@Occupied int = NULL,
	@Date datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_AcademicCalenderExamScheduler]
(
	[AcademicCalenderExamSchedulerID],
	[AcaCal_SectionID],
	[RoomInfoOneID],
	[DayOne],
	[TimeSlotPlanOneID],
	[TeacherOneID],
	[TeacherTwoID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[TypeDefinitionID],
	[Occupied],
	[Date]

)
 VALUES
(
	@AcademicCalenderExamSchedulerID,
	@AcaCal_SectionID,
	@RoomInfoOneID,
	@DayOne,
	@TimeSlotPlanOneID,
	@TeacherOneID,
	@TeacherTwoID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@TypeDefinitionID,
	@Occupied,
	@Date

)
           
SET @AcademicCalenderExamSchedulerID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderExamSchedulerUpdate]
(
	@AcademicCalenderExamSchedulerID int  = NULL,
	@AcaCal_SectionID int  = NULL,
	@RoomInfoOneID int  = NULL,
	@DayOne int  = NULL,
	@TimeSlotPlanOneID int  = NULL,
	@TeacherOneID int  = NULL,
	@TeacherTwoID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@TypeDefinitionID int = NULL,
	@Occupied int = NULL,
	@Date datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_AcademicCalenderExamScheduler]
   SET

	[AcaCal_SectionID]	=	@AcaCal_SectionID,
	[RoomInfoOneID]	=	@RoomInfoOneID,
	[DayOne]	=	@DayOne,
	[TimeSlotPlanOneID]	=	@TimeSlotPlanOneID,
	[TeacherOneID]	=	@TeacherOneID,
	[TeacherTwoID]	=	@TeacherTwoID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate,
	[TypeDefinitionID]	=	@TypeDefinitionID,
	[Occupied]	=	@Occupied,
	[Date]	=	@Date


WHERE AcademicCalenderExamSchedulerID = @AcademicCalenderExamSchedulerID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderInsert] 
(
@AcademicCalenderID int   OUTPUT,
@SLNO int = NULL,
@CalenderUnitTypeID int  = NULL,
@Year int  = NULL,
@BatchCode nvarchar(3)  = NULL,
@IsCurrent bit  = NULL,
@IsNext bit = NULL,
@StartDate datetime = NULL,
@EndDate datetime = NULL,
@FullPayNoFineLstDt datetime = NULL,
@FirstInstNoFineLstDt datetime = NULL,
@SecInstNoFineLstDt datetime = NULL,
@ThirdInstNoFineLstDs datetime = NULL,
@AddDropLastDateFull datetime = NULL,
@AddDropLastDateHalf datetime = NULL,
@LastDateEnrollNoFine datetime = NULL,
@LastDateEnrollWFine datetime = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@AdmissionStartDate datetime = NULL,
@AdmissionEndDate datetime = NULL,
@IsActiveAdmission bit = NULL,
@RegistrationStartDate datetime = NULL,
@RegistrationEndDate datetime = NULL,
@IsActiveRegistration bit = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_AcademicCalender]
(
[AcademicCalenderID],
[SLNO],
[CalenderUnitTypeID],
[Year],
[BatchCode],
[IsCurrent],
[IsNext],
[StartDate],
[EndDate],
[FullPayNoFineLstDt],
[FirstInstNoFineLstDt],
[SecInstNoFineLstDt],
[ThirdInstNoFineLstDs],
[AddDropLastDateFull],
[AddDropLastDateHalf],
[LastDateEnrollNoFine],
[LastDateEnrollWFine],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[AdmissionStartDate],
[AdmissionEndDate],
[IsActiveAdmission],
[RegistrationStartDate],
[RegistrationEndDate],
[IsActiveRegistration]

)
 VALUES
(
@AcademicCalenderID,
@SLNO,
@CalenderUnitTypeID,
@Year,
@BatchCode,
@IsCurrent,
@IsNext,
@StartDate,
@EndDate,
@FullPayNoFineLstDt,
@FirstInstNoFineLstDt,
@SecInstNoFineLstDt,
@ThirdInstNoFineLstDs,
@AddDropLastDateFull,
@AddDropLastDateHalf,
@LastDateEnrollNoFine,
@LastDateEnrollWFine,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate,
@AdmissionStartDate,
@AdmissionEndDate,
@IsActiveAdmission,
@RegistrationStartDate,
@RegistrationEndDate,
@IsActiveRegistration


)
           
SET @AcademicCalenderID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_AcademicCalenderSectionDeleteById]
(
@AcaCal_SectionID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_AcademicCalenderSection]
WHERE AcaCal_SectionID = @AcaCal_SectionID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionDTOSelect]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashraf>
-- Create date: <11-06-2011>
-- Description:	<Select section information.>
-- Note: All CASE condition are related with software's enum field.
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionDTOSelect] 
@AcademicCalenderID int,
@ProgramID int,
@CourseID int,
@VersionID int
	
AS
BEGIN
	
	SET NOCOUNT ON;
		SELECT     
		acs.AcaCal_SectionID, acs.SectionName, 

		((convert( varchar, tsp1.StartHour) +':'+ convert(varchar,tsp1.StartMin)) +' : '+
		(CASE 
			WHEN tsp1.StartAMPM = 1 then 'AM' 
			WHEN tsp1.StartAMPM = 2 then 'PM' 
			END)+' - '+
		(convert( varchar, tsp1.EndHour) +':'+ convert(varchar,tsp1.EndMin)) +' : '+
		(CASE 
			WHEN tsp1.EndAMPM = 1 then 'AM' 
			WHEN tsp1.EndAMPM = 2 then 'PM' 
			END)) AS 'TimeSlot_1' , 
		 
		(CASE	
			WHEN acs.DayOne = 1 then 'Sat' 	
			WHEN acs.DayOne = 2 then 'Sun' 
			WHEN acs.DayOne = 3 then 'Mon'
			WHEN acs.DayOne = 4 then 'Tue' 
			WHEN acs.DayOne = 5 then 'Wed' 
			WHEN acs.DayOne = 6 then 'Thu' 
			WHEN acs.DayOne = 7 then 'Fri' 
			END) AS 'DayOne',

		((convert( varchar, tsp2.StartHour) +':'+ convert(varchar,tsp2.StartMin)) +' : '+
		(CASE 
			WHEN tsp2.StartAMPM = 1 then 'AM' 
			WHEN tsp2.StartAMPM = 2 then 'PM' 
			END)+' - '+ 
		(convert( varchar, tsp2.EndHour) +':'+ convert(varchar,tsp2.EndMin)) +' : '+
		(CASE 
			WHEN tsp2.EndAMPM = 1 then 'AM' 
			WHEN tsp2.EndAMPM = 2 then 'PM' 
			END)) AS 'TimeSlot_2' ,

		(CASE	
			WHEN acs.DayTwo = 1 then 'Sat' 	
			WHEN acs.DayTwo = 2 then 'Sun' 
			WHEN acs.DayTwo = 3 then 'Mon'
			WHEN acs.DayTwo = 4 then 'Tue' 
			WHEN acs.DayTwo = 5 then 'Wed' 
			WHEN acs.DayTwo = 6 then 'Thu' 
			WHEN acs.DayTwo = 7 then 'Fri'  
			END) AS 'DayTwo',  
		 
		t1.Code AS 'Faculty_1', 
		t2.Code AS 'Faculty_2', 
		ri1.RoomNumber +'-'+ ri1.RoomName AS 'RoomNo_1',
		ri2.RoomNumber +'-'+ ri2.RoomName AS 'RoomNo_2',
		acs.Capacity, acs.Occupied

		FROM         
		UIUEMS_CC_AcademicCalenderSection AS acs LEFT OUTER JOIN
		UIUEMS_CC_TimeSlotPlan AS tsp1 ON acs.TimeSlotPlanOneID = tsp1.TimeSlotPlanID LEFT OUTER JOIN
		UIUEMS_CC_TimeSlotPlan AS tsp2 ON acs.TimeSlotPlanTwoID = tsp2.TimeSlotPlanID LEFT OUTER JOIN
		UIUEMS_CC_Employee AS t1 ON acs.TeacherOneID = t1.PersonId LEFT OUTER JOIN
		UIUEMS_CC_Employee AS t2 ON acs.TeacherTwoID = t2.PersonId LEFT OUTER JOIN
		UIUEMS_CC_RoomInformation AS ri1 ON acs.RoomInfoOneID = ri1.RoomInfoID LEFT OUTER JOIN
		UIUEMS_CC_RoomInformation AS ri2 ON acs.RoomInfoTwoID = ri2.RoomInfoID 

		where 
			(acs.AcademicCalenderID=@AcademicCalenderID ) AND			
			((acs.ProgramID = @ProgramID ) or (acs.ShareProgIDOne = @ProgramID ) or (acs.ShareProgIDTwo = @ProgramID ) or (acs.ShareProgIDThree = @ProgramID )) AND
			acs.CourseID=@CourseID AND 	acs.VersionID=@VersionID
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT *  FROM UIUEMS_CC_AcademicCalenderSection


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetAllByAcaCalId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetAllByAcaCalId]
(
@AcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_CC_AcademicCalenderSection
WHERE     (AcademicCalenderID = @AcademicCalenderID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetAllByStudentAcaCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetAllByStudentAcaCal]
(
@StudentId int = null,
@AcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT s.* FROM       UIUEMS_CC_AcademicCalenderSection s, UIUEMS_CC_Student_CourseHistory h
WHERE     s.AcademicCalenderID = @AcademicCalenderID and h.AcaCalID = @AcademicCalenderID and StudentID = @StudentId and s.AcaCal_SectionID = h.AcaCalSectionID;

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetByAcaCalCourseVersionSection]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-09-16 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetByAcaCalCourseVersionSection]
(
	@AcaCalId int = null,
	@CourseID int = null,
	@VersionID int = null,
	@SectionName varchar(250) = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_CC_AcademicCalenderSection
WHERE     (CourseID = @CourseID and VersionID = @VersionID and SectionName = @SectionName and AcademicCalenderID = @AcaCalId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetByCourseVersionSecFac]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-09-05 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetByCourseVersionSecFac]
(
	@CourseID int = null,
	@VersionID int = null,
	@SectionName varchar(250) = null,
	@TeacherOneID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_CC_AcademicCalenderSection
WHERE     (CourseID = @CourseID and VersionID = @VersionID and SectionName = @SectionName and TeacherOneID = @TeacherOneID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetById]
(
@AcaCal_SectionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_CC_AcademicCalenderSection
WHERE     (AcaCal_SectionID = @AcaCal_SectionID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionGetByRoomDayTime]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionGetByRoomDayTime]
(
	@RoomInfoOneID int = NULL,
	@RoomInfoTwoID int = NULL,
	@DayOne int = NULL,
	@DayTwo int = NULL,
	@TimeSlotPlanOneID int = NULL,
	@TimeSlotPlanTwoID int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_CC_AcademicCalenderSection Where
RoomInfoOneID = @RoomInfoOneID and RoomInfoTwoID = @RoomInfoTwoID and
DayOne = @DayOne and DayTwo = @DayTwo and
TimeSlotPlanOneID = @TimeSlotPlanOneID and TimeSlotPlanTwoID = @TimeSlotPlanTwoID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionInsert] 
(
@AcaCal_SectionID int   OUTPUT,
@AcademicCalenderID int  = NULL,
@CourseID int  = NULL,
@VersionID int  = NULL,
@SectionName varchar(250)  = NULL,
@Capacity int  = NULL,
@RoomInfoOneID int  = NULL,
@RoomInfoTwoID int = NULL,
@DayOne int  = NULL,
@DayTwo int = NULL,
@TimeSlotPlanOneID int  = NULL,
@TimeSlotPlanTwoID int = NULL,
@TeacherOneID int  = NULL,
@TeacherTwoID int = NULL,
@DeptID int  = NULL,
@ProgramID int = NULL,
@ShareProgIDOne int = NULL,
@ShareProgIDTwo int = NULL,
@ShareProgIDThree int = NULL,
@ShareDptIDOne int = NULL,
@ShareDptIDTwo int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@TypeDefinitionID int = NULL,
@Occupied int = NULL,
@ShareSection int = NULL,
@GradeSheetTemplateID int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_AcademicCalenderSection]
(
[AcademicCalenderID],
[CourseID],
[VersionID],
[SectionName],
[Capacity],
[RoomInfoOneID],
[RoomInfoTwoID],
[DayOne],
[DayTwo],
[TimeSlotPlanOneID],
[TimeSlotPlanTwoID],
[TeacherOneID],
[TeacherTwoID],
[DeptID],
[ProgramID],
[ShareProgIDOne],
[ShareProgIDTwo],
[ShareProgIDThree],
[ShareDptIDOne],
[ShareDptIDTwo],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[TypeDefinitionID],
[Occupied],
[ShareSection],
[GradeSheetTemplateID]

)
 VALUES
(
@AcademicCalenderID,
@CourseID,
@VersionID,
@SectionName,
@Capacity,
@RoomInfoOneID,
@RoomInfoTwoID,
@DayOne,
@DayTwo,
@TimeSlotPlanOneID,
@TimeSlotPlanTwoID,
@TeacherOneID,
@TeacherTwoID,
@DeptID,
@ProgramID,
@ShareProgIDOne,
@ShareProgIDTwo,
@ShareProgIDThree,
@ShareDptIDOne,
@ShareDptIDTwo,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate,
@TypeDefinitionID,
@Occupied,
@ShareSection,
@GradeSheetTemplateID
)
           
SET @AcaCal_SectionID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderSectionUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderSectionUpdate]
(
@AcaCal_SectionID int   = NULL,
@AcademicCalenderID int  = NULL,
@CourseID int  = NULL,
@VersionID int  = NULL,
@SectionName varchar(250)  = NULL,
@Capacity int  = NULL,
@RoomInfoOneID int  = NULL,
@RoomInfoTwoID int = NULL,
@DayOne int  = NULL,
@DayTwo int = NULL,
@TimeSlotPlanOneID int  = NULL,
@TimeSlotPlanTwoID int = NULL,
@TeacherOneID int  = NULL,
@TeacherTwoID int = NULL,
@DeptID int  = NULL,
@ProgramID int = NULL,
@ShareProgIDOne int = NULL,
@ShareProgIDTwo int = NULL,
@ShareProgIDThree int = NULL,
@ShareDptIDOne int = NULL,
@ShareDptIDTwo int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@TypeDefinitionID int = NULL,
@Occupied int = NULL,
@ShareSection int = NULL,
@GradeSheetTemplateID int = NULL

)

AS
BEGIN
SET NOCOUNT off;

if(@TeacherTwoID = 0)
BEGIN
set @TeacherTwoID = NULL;
END
if(@TimeSlotPlanTwoID = 0)
BEGIN
set @TimeSlotPlanTwoID = NULL;
END
if(@RoomInfoTwoID = 0)
BEGIN
set @RoomInfoTwoID = NULL;
END
if(@DayTwo = 0)
BEGIN
set @DayTwo = NULL;
END
if(@ShareDptIDOne = 0)
BEGIN
set @ShareDptIDOne = NULL;
END
if(@ShareDptIDTwo = 0)
BEGIN
set @ShareDptIDTwo = NULL;
END

if(@ShareSection = 0)
BEGIN
set @ShareSection = NULL;
END
if(@GradeSheetTemplateID = 0)
BEGIN
set @GradeSheetTemplateID = NULL;
END




UPDATE [UIUEMS_CC_AcademicCalenderSection]
   SET

[AcademicCalenderID]	=	@AcademicCalenderID,
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[SectionName]	=	@SectionName,
[Capacity]	=	@Capacity,
[RoomInfoOneID]	=	@RoomInfoOneID,
[RoomInfoTwoID]	=	@RoomInfoTwoID,
[DayOne]	=	@DayOne,
[DayTwo]	=	@DayTwo,
[TimeSlotPlanOneID]	=	@TimeSlotPlanOneID,
[TimeSlotPlanTwoID]	=	@TimeSlotPlanTwoID,
[TeacherOneID]	=	@TeacherOneID,
[TeacherTwoID]	=	@TeacherTwoID,
[DeptID]	=	@DeptID,
[ProgramID]	=	@ProgramID,
[ShareProgIDOne]	=	@ShareProgIDOne,
[ShareProgIDTwo]	=	@ShareProgIDTwo,
[ShareProgIDThree]	=	@ShareProgIDThree,
[ShareDptIDOne]	=	@ShareDptIDOne,
[ShareDptIDTwo]	=	@ShareDptIDTwo,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[TypeDefinitionID]	=	@TypeDefinitionID,
[Occupied]	=	@Occupied,
[ShareSection] = @ShareSection,
[GradeSheetTemplateID] = @GradeSheetTemplateID

WHERE AcaCal_SectionID = @AcaCal_SectionID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_AcademicCalenderUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_AcademicCalenderUpdate]
(
@AcademicCalenderID int   = NULL,
@SLNO int = NULL,
@CalenderUnitTypeID int  = NULL,
@Year int  = NULL,
@BatchCode nvarchar(3)  = NULL,
@IsCurrent bit  = NULL,
@IsNext bit = NULL,
@StartDate datetime = NULL,
@EndDate datetime = NULL,
@FullPayNoFineLstDt datetime = NULL,
@FirstInstNoFineLstDt datetime = NULL,
@SecInstNoFineLstDt datetime = NULL,
@ThirdInstNoFineLstDs datetime = NULL,
@AddDropLastDateFull datetime = NULL,
@AddDropLastDateHalf datetime = NULL,
@LastDateEnrollNoFine datetime = NULL,
@LastDateEnrollWFine datetime = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@AdmissionStartDate datetime = NULL,
@AdmissionEndDate datetime = NULL,
@IsActiveAdmission bit = NULL,
@RegistrationStartDate datetime = NULL,
@RegistrationEndDate datetime = NULL,
@IsActiveRegistration bit = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_AcademicCalender]
   SET

[SLNO]	=	@SLNO,
[CalenderUnitTypeID]	=	@CalenderUnitTypeID,
[Year]	=	@Year,
[BatchCode]	=	@BatchCode,
[IsCurrent]	=	@IsCurrent,
[IsNext]	=	@IsNext,
[StartDate]	=	@StartDate,
[EndDate]	=	@EndDate,
[FullPayNoFineLstDt]	=	@FullPayNoFineLstDt,
[FirstInstNoFineLstDt]	=	@FirstInstNoFineLstDt,
[SecInstNoFineLstDt]	=	@SecInstNoFineLstDt,
[ThirdInstNoFineLstDs]	=	@ThirdInstNoFineLstDs,
[AddDropLastDateFull]	=	@AddDropLastDateFull,
[AddDropLastDateHalf]	=	@AddDropLastDateHalf,
[LastDateEnrollNoFine]	=	@LastDateEnrollNoFine,
[LastDateEnrollWFine]	=	@LastDateEnrollWFine,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[AdmissionStartDate]	=	@AdmissionStartDate,
[AdmissionEndDate]	=	@AdmissionEndDate,
[IsActiveAdmission]	=	@IsActiveAdmission,
[RegistrationStartDate]	=	@RegistrationStartDate,
[RegistrationEndDate]	=	@RegistrationEndDate,
[IsActiveRegistration]	=	@IsActiveRegistration


WHERE AcademicCalenderID = @AcademicCalenderID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeDeleteById]
(
@CalCorProgNodeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Cal_Course_Prog_Node]
WHERE CalCorProgNodeID = @CalCorProgNodeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Cal_Course_Prog_Node


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeGetById]
(
@CalCorProgNodeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Cal_Course_Prog_Node
WHERE     (CalCorProgNodeID = @CalCorProgNodeID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeInsert] 
(
	@CalCorProgNodeID int  OUTPUT,
	@OfferedinTrimesterID int = NULL,
	@TreeCalendarDetailID int  = NULL,
	@OfferedByProgramID int  = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Node_CourseID int = NULL,
	@NodeID int = NULL,
	@NodeLinkName varchar(100) = NULL,
	@Priority int = NULL,
	@Credits decimal(18, 2) = NULL,
	@IsMajorRelated bit = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Cal_Course_Prog_Node]
(
	[CalCorProgNodeID],
	[OfferedinTrimesterID],
	[TreeCalendarDetailID],
	[OfferedByProgramID],
	[CourseID],
	[VersionID],
	[Node_CourseID],
	[NodeID],
	[NodeLinkName],
	[Priority],
	[Credits],
	[IsMajorRelated],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@CalCorProgNodeID,
	@OfferedinTrimesterID,
	@TreeCalendarDetailID,
	@OfferedByProgramID,
	@CourseID,
	@VersionID,
	@Node_CourseID,
	@NodeID,
	@NodeLinkName,
	@Priority,
	@Credits,
	@IsMajorRelated,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @CalCorProgNodeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Cal_Course_Prog_NodeUpdate]
(
	@CalCorProgNodeID int  = NULL,
	@OfferedinTrimesterID int = NULL,
	@TreeCalendarDetailID int  = NULL,
	@OfferedByProgramID int  = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Node_CourseID int = NULL,
	@NodeID int = NULL,
	@NodeLinkName varchar(100) = NULL,
	@Priority int = NULL,
	@Credits decimal(18, 2) = NULL,
	@IsMajorRelated bit = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Cal_Course_Prog_Node]
   SET


	[OfferedinTrimesterID]	=	@OfferedinTrimesterID,
	[TreeCalendarDetailID]	=	@TreeCalendarDetailID,
	[OfferedByProgramID]	=	@OfferedByProgramID,
	[CourseID]	=	@CourseID,
	[VersionID]	=	@VersionID,
	[Node_CourseID]	=	@Node_CourseID,
	[NodeID]	=	@NodeID,
	[NodeLinkName]	=	@NodeLinkName,
	[Priority]	=	@Priority,
	[Credits]	=	@Credits,
	[IsMajorRelated]	=	@IsMajorRelated,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE CalCorProgNodeID = @CalCorProgNodeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitDistributionDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_CalenderUnitDistributionDeleteById]
(
@CalenderUnitDistributionID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_CalenderUnitDistribution]
WHERE CalenderUnitDistributionID = @CalenderUnitDistributionID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitDistributionGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitDistributionGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_CalenderUnitDistribution


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitDistributionGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitDistributionGetById]
(
@CalenderUnitDistributionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_CalenderUnitDistribution
WHERE     (CalenderUnitDistributionID = @CalenderUnitDistributionID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitDistributionInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitDistributionInsert] 
(
	@CalenderUnitDistributionID int  OUTPUT,
	@CalenderUnitMasterID int  = NULL,
	@Name varchar(50)  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_CalenderUnitDistribution]
(
	[CalenderUnitDistributionID],
	[CalenderUnitMasterID],
	[Name],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@CalenderUnitDistributionID,
	@CalenderUnitMasterID,
	@Name,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @CalenderUnitDistributionID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitDistributionUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitDistributionUpdate]
(
	@CalenderUnitDistributionID int  = NULL,
	@CalenderUnitMasterID int  = NULL,
	@Name varchar(50)  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_CalenderUnitDistribution]
   SET


	[CalenderUnitMasterID]	=	@CalenderUnitMasterID,
	[Name]	=	@Name,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE CalenderUnitDistributionID = @CalenderUnitDistributionID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_CalenderUnitMasterDeleteById]
(
@CalenderUnitMasterID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_CalenderUnitMaster]
WHERE CalenderUnitMasterID = @CalenderUnitMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[CalenderUnitMasterID],
[Name],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_CalenderUnitMaster


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitMasterGetById]
(
@CalenderUnitMasterID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
[CalenderUnitMasterID],
[Name],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_CalenderUnitMaster
WHERE     (CalenderUnitMasterID = @CalenderUnitMasterID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitMasterInsert] 
(
@CalenderUnitMasterID int  OUTPUT,
@Name varchar(50)  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_CalenderUnitMaster]
(
[CalenderUnitMasterID],
[Name],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@CalenderUnitMasterID,
@Name,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @CalenderUnitMasterID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitMasterUpdate]
(
@CalenderUnitMasterID int  = NULL,
@Name varchar(50)  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_CalenderUnitMaster]
   SET

[Name]	=	@Name,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE CalenderUnitMasterID = @CalenderUnitMasterID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_CalenderUnitTypeDeleteById]
(
@CalenderUnitTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_CalenderUnitType]
WHERE CalenderUnitTypeID = @CalenderUnitTypeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[CalenderUnitTypeID],
[CalenderUnitMasterID],
[TypeName],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_CalenderUnitType


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitTypeInsert] 
(
@CalenderUnitTypeID int   OUTPUT,
@CalenderUnitMasterID int  = NULL,
@TypeName varchar(50)  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_CalenderUnitType]
(
[CalenderUnitTypeID],
[CalenderUnitMasterID],
[TypeName],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@CalenderUnitTypeID,
@CalenderUnitMasterID,
@TypeName,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @CalenderUnitTypeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CalenderUnitTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CalenderUnitTypeUpdate]
(
@CalenderUnitTypeID int   = NULL,
@CalenderUnitMasterID int  = NULL,
@TypeName varchar(50)  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_CalenderUnitType]
   SET

[CalenderUnitMasterID]	=	@CalenderUnitMasterID,
[TypeName]	=	@TypeName,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE CalenderUnitTypeID = @CalenderUnitTypeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ClassRoutineDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_ClassRoutineDeleteById]
(
@ClassRoutineID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_ClassRoutine]
WHERE ClassRoutineID = @ClassRoutineID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ClassRoutineGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ClassRoutineGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_ClassRoutine


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ClassRoutineGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ClassRoutineGetById]
(
@ClassRoutineID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_ClassRoutine
WHERE     (ClassRoutineID = @ClassRoutineID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ClassRoutineInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ClassRoutineInsert] 
(
	@ClassRoutineID int  OUTPUT,
	@AcaCal_CourseID int  = NULL,
	@Section varchar(5) = NULL,
	@Capacity varchar(50) = NULL,
	@RoomInfoID int  = NULL,
	@TimeSlotPlanID int  = NULL,
	@Day varchar(50) = NULL,
	@TeacherID int  = NULL,
	@ProgramID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_ClassRoutine]
(
	[ClassRoutineID],
	[AcaCal_CourseID],
	[Section],
	[Capacity],
	[RoomInfoID],
	[TimeSlotPlanID],
	[Day],
	[TeacherID],
	[ProgramID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@ClassRoutineID,
	@AcaCal_CourseID,
	@Section,
	@Capacity,
	@RoomInfoID,
	@TimeSlotPlanID,
	@Day,
	@TeacherID,
	@ProgramID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ClassRoutineID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ClassRoutineUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ClassRoutineUpdate]
(
	@ClassRoutineID int  = NULL,
	@AcaCal_CourseID int  = NULL,
	@Section varchar(5) = NULL,
	@Capacity varchar(50) = NULL,
	@RoomInfoID int  = NULL,
	@TimeSlotPlanID int  = NULL,
	@Day varchar(50) = NULL,
	@TeacherID int  = NULL,
	@ProgramID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_ClassRoutine]
   SET


	[AcaCal_CourseID]	=	@AcaCal_CourseID,
	[Section]	=	@Section,
	[Capacity]	=	@Capacity,
	[RoomInfoID]	=	@RoomInfoID,
	[TimeSlotPlanID]	=	@TimeSlotPlanID,
	[Day]	=	@Day,
	[TeacherID]	=	@TeacherID,
	[ProgramID]	=	@ProgramID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE ClassRoutineID = @ClassRoutineID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Course_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_Course_Insert]
(
	@CourseID int = NULL,
	@VersionID int = NULL,
	@FormalCode varchar(50) = NULL,
	@VersionCode varchar(50) = NULL,
	@Title varchar(150) = NULL,
	@AssocCourseID int = NULL,
	@AssocVersionID int = NULL,
	@StartAcademicCalenderID int = NULL,
	@ProgramID int = NULL,
	@CourseContent varchar(500) = NULL,
	@IsCreditCourse bit = NULL,
	@Credits numeric(18, 2) = NULL,
	@IsSectionMandatory bit = NULL,
	@HasEquivalents bit = NULL,
	@HasMultipleACUSpan bit = NULL,
	@IsActive bit = NULL,
	@TypeDefinitionID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_CC_Course]
(
	[CourseID],
	[VersionID],
	[FormalCode],
	[VersionCode],
	[Title],
	[AssocCourseID],
	[AssocVersionID],
	[StartAcademicCalenderID],
	[ProgramID],
	[CourseContent],
	[IsCreditCourse],
	[Credits],
	[IsSectionMandatory],
	[HasEquivalents],
	[HasMultipleACUSpan],
	[IsActive],
	[TypeDefinitionID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@CourseID,
	@VersionID,
	@FormalCode,
	@VersionCode,
	@Title,
	@AssocCourseID,
	@AssocVersionID,
	@StartAcademicCalenderID,
	@ProgramID,
	@CourseContent,
	@IsCreditCourse,
	@Credits,
	@IsSectionMandatory,
	@HasEquivalents,
	@HasMultipleACUSpan,
	@IsActive,
	@TypeDefinitionID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @CourseID = @CourseID;
SET @VersionID = @VersionID;
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanDtlDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_CourseACUSpanDtlDeleteById]
(
@CourseACUSpanDtlID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_CourseACUSpanDtl]
WHERE CourseACUSpanDtlID = @CourseACUSpanDtlID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanDtlGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanDtlGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT  *   

--property

FROM       UIUEMS_CC_CourseACUSpanDtl


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanDtlGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanDtlGetById]
(
@CourseACUSpanDtlID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*
FROM       UIUEMS_CC_CourseACUSpanDtl
WHERE     (CourseACUSpanDtlID = @CourseACUSpanDtlID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanDtlInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanDtlInsert] 
(
	@CourseACUSpanDtlID int  OUTPUT,
	@CourseACUSpanMasID int  = NULL,
	@CreditUnits money  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_CourseACUSpanDtl]
(
	[CourseACUSpanDtlID],
	[CourseACUSpanMasID],
	[CreditUnits],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@CourseACUSpanDtlID,
	@CourseACUSpanMasID,
	@CreditUnits,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @CourseACUSpanDtlID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanDtlUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanDtlUpdate]
(
	@CourseACUSpanDtlID int  = NULL,
	@CourseACUSpanMasID int  = NULL,
	@CreditUnits money  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_CourseACUSpanDtl]
   SET
	[CourseACUSpanMasID]	=	@CourseACUSpanMasID,
	[CreditUnits]	=	@CreditUnits,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE CourseACUSpanDtlID = @CourseACUSpanDtlID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanMasDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_CourseACUSpanMasDeleteById]
(
@CourseACUSpanMasID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_CourseACUSpanMas]
WHERE CourseACUSpanMasID = @CourseACUSpanMasID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanMasGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanMasGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_CourseACUSpanMas


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanMasGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanMasGetById]
(
@CourseACUSpanMasID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_CourseACUSpanMas
WHERE     (CourseACUSpanMasID = @CourseACUSpanMasID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanMasInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanMasInsert] 
(
	@CourseACUSpanMasID int  OUTPUT,
	@CourseID int  = NULL,
	@VersionID int  = NULL,
	@MaxACUNo int = NULL,
	@MinACUNo int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_CourseACUSpanMas]
(
	[CourseACUSpanMasID],
	[CourseID],
	[VersionID],
	[MaxACUNo],
	[MinACUNo],
	[CreatedBy],
	[CreatedDate]

)
 VALUES
(
@CourseACUSpanMasID,
@CourseID,
@VersionID,
@MaxACUNo,
@MinACUNo,
@CreatedBy,
@CreatedDate

)
           
SET @CourseACUSpanMasID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseACUSpanMasUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseACUSpanMasUpdate]
(
@CourseACUSpanMasID int  = NULL,
@CourseID int  = NULL,
@VersionID int  = NULL,
@MaxACUNo int = NULL,
@MinACUNo int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_CourseACUSpanMas]
   SET
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[MaxACUNo]	=	@MaxACUNo,
[MinACUNo]	=	@MinACUNo,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate


WHERE CourseACUSpanMasID = @CourseACUSpanMasID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_CourseGetByCourseIdVersionId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_CourseGetByCourseIdVersionId]
(
	@CourseID int = NULL,
	@VersionID int = NULL
)

As
Begin
	Select * From UIUEMS_CC_Course Where CourseID = @CourseID and VersionID = @VersionID;
End



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_DepartmentDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_DepartmentDeleteById]
(
@DeptID int = null
)

AS
BEGIN
SET NOCOUNT ON;

DELETE FROM [UIUEMS_CC_Department]
WHERE DeptID = @DeptID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_DepartmentGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_DepartmentGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[DeptID],
[Code],
[Name],
[OpeningDate],
[SchoolID],
[DetailedName],
[ClosingDate],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_Department


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_DepartmentGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_DepartmentGetById]
(
@DeptID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[DeptID],
[Code],
[Name],
[OpeningDate],
[SchoolID],
[DetailedName],
[ClosingDate],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_Department
WHERE     (DeptID = @DeptID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_DepartmentInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_DepartmentInsert] 
(
@DeptID int   OUTPUT,
@Code varchar(50)  = NULL,
@Name varchar(100)  = NULL,
@OpeningDate datetime = NULL,
@SchoolID int  = NULL,
@DetailedName varchar(100)  = NULL,
@ClosingDate datetime = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Department]
(
[DeptID],
[Code],
[Name],
[OpeningDate],
[SchoolID],
[DetailedName],
[ClosingDate],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]
)
 VALUES
(
@DeptID,
@Code,
@Name,
@OpeningDate,
@SchoolID,
@DetailedName,
@ClosingDate,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @DeptID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_DepartmentUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_DepartmentUpdate]
(
@DeptID int   = NULL,
@Code varchar(50)  = NULL,
@Name varchar(100)  = NULL,
@OpeningDate datetime = NULL,
@SchoolID int  = NULL,
@DetailedName varchar(100)  = NULL,
@ClosingDate datetime = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Department]
   SET
[Code]	=	@Code,
[Name]	=	@Name,
[OpeningDate]	=	@OpeningDate,
[SchoolID]	=	@SchoolID,
[DetailedName]	=	@DetailedName,
[ClosingDate]	=	@ClosingDate,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE DeptID = @DeptID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Employee_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_Employee_Insert]
(
	@TeacherID int   OUTPUT,
	@Code varchar(100)  = NULL,
	@DeptID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@SchoolId int = NULL,
	@Remarks nvarchar(max) = NULL,
	@History nvarchar(max) = NULL,
	@PersonId int = NULL,
	@Attribute1 nvarchar(max) = NULL,
	@Attribute2 nvarchar(max) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_CC_Employee]
(
	[Code],
	[DeptID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[SchoolId],
	[Remarks],
	[History],
	[PersonId],
	[Attribute1],
	[Attribute2]
)
 VALUES
(
	@Code,
	@DeptID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@SchoolId,
	@Remarks,
	@History,
	@PersonId,
	@Attribute1,
	@Attribute2
)           
SET @TeacherID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EmployeeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_EmployeeDeleteById]
(
@TeacherID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Employee]
WHERE TeacherID = @TeacherID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EmployeeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EmployeeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_CC_Employee


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EmployeeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EmployeeGetById]
(
@TeacherID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Employee
WHERE     (TeacherID = @TeacherID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EmployeeGetByPersonId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EmployeeGetByPersonId]
(
@PersonId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Employee
WHERE     (PersonId = @PersonId)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EmployeeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_EmployeeInsert]
(
	@TeacherID int   OUTPUT,
	@Code varchar(100)  = NULL,
	@DeptID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@SchoolId int = NULL,
	@Remarks nvarchar(max) = NULL,
	@History nvarchar(max) = NULL,
	@PersonId int = NULL,
	@Attribute1 nvarchar(max) = NULL,
	@Attribute2 nvarchar(max) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_CC_Employee]
(
	[Code],
	[DeptID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[SchoolId],
	[Remarks],
	[History],
	[PersonId],
	[Attribute1],
	[Attribute2]
)
 VALUES
(
	@Code,
	@DeptID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@SchoolId,
	@Remarks,
	@History,
	@PersonId,
	@Attribute1,
	@Attribute2
)           
SET @TeacherID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EmployeeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EmployeeUpdate]
(
@TeacherID int   = NULL,
@Code varchar(10)  = NULL,
@DeptID int = NULL,
@User_ID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@SchoolId int = NULL,
@Remarks nvarchar(max) = NULL,
@History nvarchar(max) = NULL,
@PersonId int = NULL,
@Attribute1 nvarchar(max) = NULL,
@Attribute2 nvarchar(max) = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Employee]
   SET

[Code]	=	@Code,
[DeptID]	=	@DeptID,

[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[SchoolId]	=	@SchoolId,
[Remarks]	=	@Remarks,
[History]	=	@History,
[PersonId]	=	@PersonId,
[Attribute1]	=	@Attribute1,
[Attribute2]	=	@Attribute2

WHERE TeacherID = @TeacherID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EquiCourseDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_EquiCourseDeleteById]
(
@EquivalentID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_EquiCourse]
WHERE EquivalentID = @EquivalentID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EquiCourseGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EquiCourseGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_EquiCourse


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EquiCourseGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EquiCourseGetById]
(
@EquivalentID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_EquiCourse
WHERE     (EquivalentID = @EquivalentID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EquiCourseInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EquiCourseInsert] 
(
@EquivalentID int  OUTPUT,
@ParentCourseID int = NULL,
@ParentVersionID int = NULL,
@EquiCourseID int = NULL,
@EquiVersionID int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_EquiCourse]
(
[EquivalentID],
[ParentCourseID],
[ParentVersionID],
[EquiCourseID],
[EquiVersionID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@EquivalentID,
@ParentCourseID,
@ParentVersionID,
@EquiCourseID,
@EquiVersionID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @EquivalentID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormDelete]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_CC_EvaluationFormDelete]
(
	@Id int = NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_EvaluationForm]
WHERE Id = @Id

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_EvaluationForm


END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormGetAllByAcaCalId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormGetAllByAcaCalId]
(
	@AcaCalId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_EvaluationForm
WHERE     (AcaCalId = @AcaCalId)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormGetAllByAcaCalSecId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormGetAllByAcaCalSecId]
(
	@AcaCalSecId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_EvaluationForm
WHERE     (AcaCalSecId = @AcaCalSecId)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormGetAllByPersonId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormGetAllByPersonId]
(
	@PersonId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_EvaluationForm
WHERE     (PersonId = @PersonId)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormGetById]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_CC_EvaluationForm WHERE (Id = @Id);

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormGetByPersonAcaCalSec]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormGetByPersonAcaCalSec]
(
	@PersonId int = NULL,
	@AcaCalSecId int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_CC_EvaluationForm WHERE PersonId = @PersonId and AcaCalSecId = @AcaCalSecId;

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormInsert]
(
	@Id int Output,
	@PersonId int = NULL,
	@AcaCalSecId int = NULL,
	@AcaCalId int = NULL,
	@ExpectedGrade nvarchar(10) = NULL,
	@QSet int = NULL,
	@Q1 int = NULL,
	@Q2 int = NULL,
	@Q3 int = NULL,
	@Q4 int = NULL,
	@Q5 int = NULL,
	@Q6 int = NULL,
	@Q7 int = NULL,
	@Q8 int = NULL,
	@Q9 int = NULL,
	@Q10 int = NULL,
	@Q11 int = NULL,
	@Q12 int = NULL,
	@Q13 int = NULL,
	@Q14 int = NULL,
	@Q15 int = NULL,
	@Q16 int = NULL,
	@Q17 int = NULL,
	@Q18 int = NULL,
	@Q19 int = NULL,
	@Q20 int = NULL,
	@Comment nvarchar(MAX) = NULL,
	@IsFinalSubmit bit = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_CC_EvaluationForm]
(
	[PersonId],
	[AcaCalSecId],
	[AcaCalId],
	[ExpectedGrade],
	[QSet],
	[Q1],
	[Q2],
	[Q3],
	[Q4],
	[Q5],
	[Q6],
	[Q7],
	[Q8],
	[Q9],
	[Q10],
	[Q11],
	[Q12],
	[Q13],
	[Q14],
	[Q15],
	[Q16],
	[Q17],
	[Q18],
	[Q19],
	[Q20],
	[Comment],
	[IsFinalSubmit],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@PersonId,
	@AcaCalSecId,
	@AcaCalId,
	@ExpectedGrade,
	@QSet,
	@Q1,
	@Q2,
	@Q3,
	@Q4,
	@Q5,
	@Q6,
	@Q7,
	@Q8,
	@Q9,
	@Q10,
	@Q11,
	@Q12,
	@Q13,
	@Q14,
	@Q15,
	@Q16,
	@Q17,
	@Q18,
	@Q19,
	@Q20,
	@Comment,
	@IsFinalSubmit,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @Id = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_EvaluationFormUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_EvaluationFormUpdate]
(
	@Id int = NULL,
	@PersonId int = NULL,
	@AcaCalSecId int = NULL,
	@AcaCalId int = NULL,
	@ExpectedGrade nvarchar(10) = NULL,
	@QSet int = NULL,
	@Q1 int = NULL,
	@Q2 int = NULL,
	@Q3 int = NULL,
	@Q4 int = NULL,
	@Q5 int = NULL,
	@Q6 int = NULL,
	@Q7 int = NULL,
	@Q8 int = NULL,
	@Q9 int = NULL,
	@Q10 int = NULL,
	@Q11 int = NULL,
	@Q12 int = NULL,
	@Q13 int = NULL,
	@Q14 int = NULL,
	@Q15 int = NULL,
	@Q16 int = NULL,
	@Q17 int = NULL,
	@Q18 int = NULL,
	@Q19 int = NULL,
	@Q20 int = NULL,
	@Comment nvarchar(max) = NULL,
	@IsFinalSubmit bit = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_CC_EvaluationForm]
   SET
	[PersonId]=@PersonId,
	[AcaCalSecId]=@AcaCalSecId,
	[AcaCalId]=@AcaCalId,
	[ExpectedGrade]=@ExpectedGrade,
	[QSet]=@QSet,
	[Q1]=@Q1,
	[Q2]=@Q2,
	[Q3]=@Q3,
	[Q4]=@Q4,
	[Q5]=@Q5,
	[Q6]=@Q6,
	[Q7]=@Q7,
	[Q8]=@Q8,
	[Q9]=@Q9,
	[Q10]=@Q10,
	[Q11]=@Q11,
	[Q12]=@Q12,
	[Q13]=@Q13,
	[Q14]=@Q14,
	[Q15]=@Q15,
	[Q16]=@Q16,
	[Q17]=@Q17,
	[Q18]=@Q18,
	[Q19]=@Q19,
	[Q20]=@Q20,
	[Comment]=@Comment,
	[IsFinalSubmit]=@IsFinalSubmit,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE Id = @Id
           
END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamMarksAllocationDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_ExamMarksAllocationDeleteById]
(
@ExamMarksAllocationID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_ExamMarksAllocation]
WHERE ExamMarksAllocationID = @ExamMarksAllocationID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamMarksAllocationGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamMarksAllocationGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_ExamMarksAllocation


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamMarksAllocationGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamMarksAllocationGetById]
(
@ExamMarksAllocationID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_ExamMarksAllocation
WHERE     (ExamMarksAllocationID = @ExamMarksAllocationID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamMarksAllocationInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamMarksAllocationInsert] 
(
@ExamMarksAllocationID int  OUTPUT,
@ExamTypeNameID int = NULL,
@AllottedMarks int = NULL,
@ExamName varchar(150) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_ExamMarksAllocation]
(
[ExamMarksAllocationID],
[ExamTypeNameID],
[AllottedMarks],
[ExamName],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@ExamMarksAllocationID,
@ExamTypeNameID,
@AllottedMarks,
@ExamName,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @ExamMarksAllocationID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamMarksAllocationUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamMarksAllocationUpdate]
(
@ExamMarksAllocationID int  = NULL,
@ExamTypeNameID int = NULL,
@AllottedMarks int = NULL,
@ExamName varchar(150) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_ExamMarksAllocation]
   SET

[ExamTypeNameID]	=	@ExamTypeNameID,
[AllottedMarks]	=	@AllottedMarks,
[ExamName]	=	@ExamName,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE ExamMarksAllocationID = @ExamMarksAllocationID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamRoutineDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE  [dbo].[UIUEMS_CC_ExamRoutineDeleteById]
(
@ExamRoutineID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_ExamRoutine]
WHERE ExamRoutineID = @ExamRoutineID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamRoutineGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamRoutineGetAll]

AS
BEGIN
SET NOCOUNT ON;

SELECT
[ExamRoutineID],
[AcaCal_SectionID],
[RoomInfoID],
[TimeSlotPlanID],
[ExamDate],
[TeacherID1],
[TeacherID2],
[ProgramID],
[ExamTypeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]
FROM       UIUEMS_CC_ExamRoutine

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamRoutineGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamRoutineGetById]
(
@ExamRoutineID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT
[ExamRoutineID],
[AcaCal_SectionID],
[RoomInfoID],
[TimeSlotPlanID],
[ExamDate],
[TeacherID1],
[TeacherID2],
[ProgramID],
[ExamTypeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_ExamRoutine
WHERE     (ExamRoutineID = @ExamRoutineID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamRoutineInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamRoutineInsert] 
(
	@ExamRoutineID int Output,
	@AcaCal_SectionID int = NULL,
	@RoomInfoID int = NULL,
	@TimeSlotPlanID int = NULL,
	@ExamDate varchar(50) = NULL,
	@TeacherID1 int = NULL,
	@TeacherID2 int = NULL,
	@ProgramID int = NULL,
	@ExamTypeID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_ExamRoutine]
(
	[AcaCal_SectionID],
	[RoomInfoID],
	[TimeSlotPlanID],
	[ExamDate],
	[TeacherID1],
	[TeacherID2],
	[ProgramID],
	[ExamTypeID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@AcaCal_SectionID,
	@RoomInfoID,
	@TimeSlotPlanID,
	@ExamDate,
	@TeacherID1,
	@TeacherID2,
	@ProgramID,
	@ExamTypeID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @ExamRoutineID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamRoutineUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamRoutineUpdate]
(
	@ExamRoutineID int = NULL,
	@AcaCal_SectionID int = NULL,
	@RoomInfoID int = NULL,
	@TimeSlotPlanID int = NULL,
	@ExamDate datetime = NULL,
	@TeacherID1 int = NULL,
	@TeacherID2 int = NULL,
	@ProgramID int = NULL,
	@ExamTypeID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_CC_ExamRoutine]
   SET
	@AcaCal_SectionID = @AcaCal_SectionID,
	@RoomInfoID = @RoomInfoID,
	@TimeSlotPlanID = @TimeSlotPlanID,
	@ExamDate = @ExamDate,
	@TeacherID1 = @TeacherID1,
	@TeacherID2 = @TeacherID2,
	@ProgramID = @ProgramID,
	@ExamTypeID = @ExamTypeID,
	@CreatedBy = @CreatedBy,
	@CreatedDate = @CreatedDate,
	@ModifiedBy = @ModifiedBy,
	@ModifiedDate = @ModifiedDate
 
WHERE ExamRoutineID = @ExamRoutineID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamTypeNameDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_ExamTypeNameDeleteById]
(
@ExamTypeNameID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_ExamTypeName]
WHERE ExamTypeNameID = @ExamTypeNameID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamTypeNameGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamTypeNameGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_ExamTypeName


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamTypeNameGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamTypeNameGetById]
(
@ExamTypeNameID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_ExamTypeName
WHERE     (ExamTypeNameID = @ExamTypeNameID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamTypeNameInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamTypeNameInsert] 
(
@ExamTypeNameID int  OUTPUT,
@TypeDefinitionID int = NULL,
@Name varchar(150) = NULL,
@TotalAllottedMarks int = NULL,
@Default bit = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_ExamTypeName]
(
[ExamTypeNameID],
[TypeDefinitionID],
[Name],
[TotalAllottedMarks],
[Default],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@ExamTypeNameID,
@TypeDefinitionID,
@Name,
@TotalAllottedMarks,
@Default,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @ExamTypeNameID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ExamTypeNameUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ExamTypeNameUpdate]
(
@ExamTypeNameID int  = NULL,
@TypeDefinitionID int = NULL,
@Name varchar(150) = NULL,
@TotalAllottedMarks int = NULL,
@Default bit = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_ExamTypeName]
   SET

[TypeDefinitionID]	=	@TypeDefinitionID,
[Name]	=	@Name,
[TotalAllottedMarks]	=	@TotalAllottedMarks,
[Default]	=	@Default,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE ExamTypeNameID = @ExamTypeNameID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GetAllOfferedCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_GetAllOfferedCourse]

As
Begin
	Select Distinct c.* from UIUEMS_CC_Course as C,UIUEMS_CC_AcademicCalender as a ,UIUEMS_CC_OfferedCourse ofc 
	where a.AcademicCalenderID=ofc.AcademicCalenderID 
	and 
	a.IsActiveRegistration='True' 
	and
	(ofc.CourseID=c.CourseID and ofc.VersionID=c.VersionID)
	Order by  c.CourseID; 
End



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GetTranscriptResult]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GetTranscriptResult]
(
	@Roll nvarchar(15) = NULL
)

As
Begin
	If @Roll is not null
	Begin
		Select Distinct (acu.TypeName+'  '+Cast(ac.Year as nvarchar)) as AcademicCalender,ch.CourseID,c.Title,
		c.FormalCode,ch.ObtainedGrade,ch.ObtainedGPA,ch.ObtainedTotalMarks,c.Credits,acd.GPA,acd.CGPA,AcademicCalenderID 
		from UIUEMS_CC_Student_CourseHistory as ch,
		UIUEMS_ER_Student as s,UIUEMS_ER_Person as p,UIUEMS_ER_Student_ACUDetail acd,
		UIUEMS_CC_Course c,UIUEMS_CC_AcademicCalender ac,UIUEMS_CC_CalenderUnitType acu
		where s.Roll=@Roll and s.StudentID=ch.StudentID and acd.StudentID=s.StudentID
		and acd.StdAcademicCalenderID=ch.AcaCalID
		and c.CourseID=ch.CourseID and c.VersionID=ch.VersionID
		and ac.AcademicCalenderID=ch.AcaCalID and ac.CalenderUnitTypeID=acu.CalenderUnitTypeID
		and ch.IsConsiderGPA=1
		Order by AcademicCalenderID
	End
End







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GetTranscriptStudentInfo]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GetTranscriptStudentInfo]
(
	@Roll nvarchar(15) = NULL
)

As
Begin
	If @Roll is not null
	Begin
		Select Distinct GetDate() as IssueDate,s.Roll,p.FirstName as FullName,p.dob,s.IsCompleted,pr.DetailName as Degree,
		(acu.TypeName+'  '+Cast(ac.Year as nvarchar)) as EnrollmentTrimester,s.StudentID From 
		UIUEMS_ER_Student as s,UIUEMS_ER_Person as p,
		UIUEMS_CC_AcademicCalender as ac,UIUEMS_CC_CalenderUnitType acu,
		UIUEMS_ER_Admission ad,UIUEMS_CC_Program pr
		where s.Roll=@Roll and p.PersonID=s.PersonID and 
	    s.StudentID=ad.StudentID and ad.AdmissionCalenderID=ac.AcademicCalenderID
		and ac.CalenderUnitTypeID =acu.CalenderUnitTypeID and s.ProgramID=pr.ProgramID
	
	End

End







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GetTranscriptTransferedResult]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GetTranscriptTransferedResult]
(
	@ID nvarchar(15) = NULL
)

As

	Begin
		Select C.FormalCode,C.Title,C.Credits,UniversityName from UIUEMS_CC_Student_CourseHistory as CH,UIUEMS_CC_Course as C,
		UIUEMS_ER_CourseWavTransfr 
		where CH.CourseWavTransfrID in 
	    (Select C.CourseWavTransfrID from UIUEMS_ER_CourseWavTransfr as C where StudentID=@ID and UniversityName is not null)
		and c.CourseID=ch.CourseID and c.VersionID=ch.VersionID and 
		UIUEMS_ER_CourseWavTransfr.CourseWavTransfrID=Ch.CourseWavTransfrID 
	End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GetTranscriptWaiverResult]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GetTranscriptWaiverResult]
(
	@ID nvarchar(15) = NULL
)

As

	Begin
		Select C.FormalCode,C.Title,C.Credits,UniversityName=null from UIUEMS_CC_Student_CourseHistory as CH,UIUEMS_CC_Course as C,
		UIUEMS_ER_CourseWavTransfr 
		where CH.CourseWavTransfrID in 
	    (Select C.CourseWavTransfrID from UIUEMS_ER_CourseWavTransfr as C where StudentID=@ID and UniversityName is null)
		and c.CourseID=ch.CourseID and c.VersionID=ch.VersionID and 
		UIUEMS_ER_CourseWavTransfr.CourseWavTransfrID=Ch.CourseWavTransfrID 
	End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeDetailsDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_GradeDetailsDeleteById]
(
@GradeId int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_GradeDetails]
WHERE GradeId = @GradeId

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeDetailsGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeDetailsGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_GradeDetails


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeDetailsGetByGrade]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeDetailsGetByGrade]
(
@Grade nvarchar(50) = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_GradeDetails
WHERE     (Grade = @Grade)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeDetailsGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeDetailsGetById]
(
@GradeId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_GradeDetails
WHERE     (GradeId = @GradeId)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeDetailsInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeDetailsInsert] 
(
@GradeId int  OUTPUT,
@AcaCalId int = NULL,
@ProgramId int = NULL,
@Sequence int = NULL,
@Grade nvarchar(50) = NULL,
@GradePoint numeric(18, 2) = NULL,
@MinMarks int = NULL,
@MaxMarks int = NULL,
@RetakeDiscount numeric(18, 2) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_GradeDetails]
(
[GradeId],
[AcaCalId],
[ProgramId],
Sequence,
[Grade],
[GradePoint],
[MinMarks],
[MaxMarks],
[RetakeDiscount],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@GradeId,
@AcaCalId,
@ProgramId,
@Sequence,
@Grade,
@GradePoint,
@MinMarks,
@MaxMarks,
@RetakeDiscount,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @GradeId = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeDetailsUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeDetailsUpdate]
(
@GradeId int  = NULL,
@AcaCalId int = NULL,
@ProgramId int = NULL,
@Sequence int = NULL,
@Grade nvarchar(50) = NULL,
@GradePoint numeric(18, 2) = NULL,
@MinMarks int = NULL,
@MaxMarks int = NULL,
@RetakeDiscount numeric(18, 2) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_GradeDetails]
   SET
[AcaCalId]	=	@AcaCalId,
[ProgramId]	=	@ProgramId,
[Sequence] = @Sequence,
[Grade]	=	@Grade,
[GradePoint]	=	@GradePoint,
[MinMarks]	=	@MinMarks,
[MaxMarks]	=	@MaxMarks,
[RetakeDiscount]	=	@RetakeDiscount,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE GradeId = @GradeId
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_GradeSheetDeleteById]
(
@GradeSheetId int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_GradeSheet]
WHERE GradeSheetId = @GradeSheetId

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_GradeSheet


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetGetAllByAcaCalSectionId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetGetAllByAcaCalSectionId]
(
@AcaCal_SectionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_GradeSheet
WHERE     (AcaCal_SectionID = @AcaCal_SectionID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetGetById]
(
@GradeSheetId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_GradeSheet
WHERE     (GradeSheetId = @GradeSheetId)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-09-01 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetInsert] 
(
	@GradeSheetId int  OUTPUT,
	@ExamMarksAllocationID int = NULL,
	@ProgramID int = NULL,
	@AcademicCalenderID int = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@StudentID int = NULL,
	@AcaCal_SectionID int = NULL,
	@TeacherID int  = NULL,
	@ObtainMarks numeric(18, 2) = NULL,
	@ObtainGrade nvarchar(50) = NULL,
	@GradeId int = NULL,
	@IsFinalSubmit bit = NULL,
	@IsTransfer bit = NULL,
	@IsConflictWithRetake bit = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_GradeSheet]
(
	[ExamMarksAllocationID],
	[ProgramID],
	[AcademicCalenderID],
	[CourseID],
	[VersionID],
	[StudentID],
	[AcaCal_SectionID],
	[TeacherID],
	[ObtainMarks],
	[ObtainGrade],
	[GradeId],
	[IsFinalSubmit],
	[IsTransfer],
	[IsConflictWithRetake],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@ExamMarksAllocationID,
	@ProgramID,
	@AcademicCalenderID,
	@CourseID,
	@VersionID,
	@StudentID,
	@AcaCal_SectionID,
	@TeacherID,
	@ObtainMarks,
	@ObtainGrade,
	@GradeId,
	@IsFinalSubmit,
	@IsTransfer,
	@IsConflictWithRetake,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @GradeSheetId = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetTemplateDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_GradeSheetTemplateDeleteById]
(
@GradeSheetTemplateID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_GradeSheetTemplate]
WHERE GradeSheetTemplateID = @GradeSheetTemplateID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetTemplateGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetTemplateGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_GradeSheetTemplate


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetTemplateGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetTemplateGetById]
(
@GradeSheetTemplateID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_GradeSheetTemplate
WHERE     (GradeSheetTemplateID = @GradeSheetTemplateID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetTemplateInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetTemplateInsert] 
(
	@GradeSheetTemplateID int   OUTPUT,
	@Name varchar(100) = NULL,
	@Code varchar(100) = NULL,
	@Path varchar(500) = NULL,
	@IsActive bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_GradeSheetTemplate]
(
[GradeSheetTemplateID],
[Name],
[Code],
[Path],
[IsActive],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]
)
 VALUES
(
@GradeSheetTemplateID,
@Name,
@Code,
@Path,
@IsActive,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @GradeSheetTemplateID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetTemplateUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetTemplateUpdate]
(
@GradeSheetTemplateID int   = NULL,
@Name varchar(100) = NULL,
@Code varchar(100) = NULL,
@Path varchar(500) = NULL,
@IsActive bit  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_GradeSheetTemplate]
   SET


[Name]	=	@Name,
[Code]	=	@Code,
[Path]	=	@Path,
[IsActive]	=	@IsActive,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE GradeSheetTemplateID = @GradeSheetTemplateID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeSheetUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-09-04 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeSheetUpdate]
(
	@GradeSheetId int = NULL,
	@ExamMarksAllocationID int = NULL,
	@ProgramID int = NULL,
	@AcademicCalenderID int = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@StudentID int = NULL,
	@AcaCal_SectionID int = NULL,
	@TeacherID int  = NULL,
	@ObtainMarks numeric(18, 2) = NULL,
	@ObtainGrade nvarchar(50) = NULL,
	@GradeId int = NULL,
	@IsFinalSubmit bit = NULL,
	@IsTransfer bit = NULL,
	@IsConflictWithRetake bit = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_CC_GradeSheet]
SET
	[ExamMarksAllocationID] = @ExamMarksAllocationID,
	[ProgramID] = @ProgramID,
	[AcademicCalenderID] = @AcademicCalenderID,
	[CourseID] = @CourseID,
	[VersionID] = @VersionID,
	[StudentID] = @StudentID,
	[AcaCal_SectionID] = @AcaCal_SectionID,
	[TeacherID] = @TeacherID,
	[ObtainMarks] = @ObtainMarks,
	[ObtainGrade] = @ObtainGrade,
	[GradeId] = @GradeId,
	[IsFinalSubmit] = @IsFinalSubmit,
	[IsTransfer] = @IsTransfer,
	[IsConflictWithRetake] = @IsConflictWithRetake,
	[CreatedBy] = @CreatedBy,
	[CreatedDate] = @CreatedDate,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate


WHERE GradeSheetId = @GradeSheetId
           
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountDeleteById]
(
	@GradeWiseRetakeDiscountId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_GradeWiseRetakeDiscount]
WHERE GradeWiseRetakeDiscountId = @GradeWiseRetakeDiscountId

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_GradeWiseRetakeDiscount


END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountGetById]
(
	@GradeWiseRetakeDiscountId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_GradeWiseRetakeDiscount
WHERE     (GradeWiseRetakeDiscountId = @GradeWiseRetakeDiscountId)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountGetByProgramSession]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountGetByProgramSession]
(
	@ProgramId int=NULL,
	@SessionId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT        
	ISNULL(gwrd.GradeWiseRetakeDiscountId,0) GradeWiseRetakeDiscountId, 
	ISNULL(gd.GradeId, 0) GradeId,
	ISNULL(gwrd.SessionId, 0) SessionId,
	ISNULL(gwrd.ProgramId, 0) ProgramId,
	ISNULL(gwrd.RetakeDiscount, 0) RetakeDiscount,
	ISNULL(gwrd.CreatedBy,0) CreatedBy,
	ISNULL(gwrd.CreatedDate, '') CreatedDate,
	ISNULL(gwrd.ModifiedBy,  0) ModifiedBy,
	ISNULL(gwrd.ModifiedDate, '') ModifiedDate,
	ISNULL(gwrd.RetakeDiscountOnTrOrWav, 0) RetakeDiscountOnTrOrWav

FROM	UIUEMS_CC_GradeDetails AS gd LEFT OUTER JOIN
		(select * from UIUEMS_CC_GradeWiseRetakeDiscount where ProgramId = @ProgramId and SessionId = @SessionId) AS gwrd ON gwrd.GradeId = gd.GradeId
END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountInsert]
(
	@GradeWiseRetakeDiscountId int output,
	@GradeId int = NULL,
	@SessionId int = NULL,
	@ProgramId int = NULL,
	@RetakeDiscount numeric(18,2) = null,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@RetakeDiscountOnTrOrWav numeric(18,2) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_CC_GradeWiseRetakeDiscount]
(
	 
	[GradeId],
	[SessionId],
	[ProgramId],
	[RetakeDiscount],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[RetakeDiscountOnTrOrWav]
)
 VALUES
(
	
	@GradeId,
	@SessionId,
	@ProgramId,
	@RetakeDiscount,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@RetakeDiscountOnTrOrWav
)           
SET @GradeWiseRetakeDiscountId = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_GradeWiseRetakeDiscountUpdate]
(
	@GradeWiseRetakeDiscountId int = NULL,
	@GradeId int = NULL,
	@SessionId int = NULL,
	@ProgramId int = NULL,
	@RetakeDiscount numeric(18,2) = null,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@RetakeDiscountOnTrOrWav numeric(18,2) = null
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_GradeWiseRetakeDiscount]
   SET	
	[GradeId]=@GradeId,
	[SessionId]=@SessionId,
	[ProgramId]=@ProgramId,
	[RetakeDiscount]=@RetakeDiscount,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate,
	[RetakeDiscountOnTrOrWav] = @RetakeDiscountOnTrOrWav

WHERE [GradeWiseRetakeDiscountId] = @GradeWiseRetakeDiscountId
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Node_CourseDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_Node_CourseDeleteById]
(
@Node_CourseID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Node_Course]
WHERE Node_CourseID = @Node_CourseID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Node_CourseGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Node_CourseGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Node_Course


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Node_CourseGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Node_CourseGetById]
(
@Node_CourseID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_CC_Node_Course
WHERE     (Node_CourseID = @Node_CourseID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Node_CourseInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Node_CourseInsert] 
(
@Node_CourseID int  OUTPUT,
@NodeID int = NULL,
@CourseID int = NULL,
@VersionID int = NULL,
@Priority int  = NULL,
@IsActive bit  = NULL,
@PassingGPA numeric(18, 2) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Node_Course]
(
[Node_CourseID],
[NodeID],
[CourseID],
[VersionID],
[Priority],
[IsActive],
[PassingGPA],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@Node_CourseID,
@NodeID,
@CourseID,
@VersionID,
@Priority,
@IsActive,
@PassingGPA,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @Node_CourseID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Node_CourseUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Node_CourseUpdate]
(
@Node_CourseID int  = NULL,
@NodeID int = NULL,
@CourseID int = NULL,
@VersionID int = NULL,
@Priority int  = NULL,
@IsActive bit  = NULL,
@PassingGPA numeric(18, 2) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Node_Course]
   SET
[NodeID]	=	@NodeID,
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[Priority]	=	@Priority,
[IsActive]	=	@IsActive,
[PassingGPA]	=	@PassingGPA,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE Node_CourseID = @Node_CourseID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseActiveInactive]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OfferedCourseActiveInactive]
(
@OfferID int  = NULL,
@AcademicCalenderID int  = NULL,
@DeptID int = NULL,
@ProgramID int = NULL,
@TreeRootID int = NULL,
@CourseID int  = NULL,
@VersionID int  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@Limit int = NULL,
@Occupied int = NULL,
@Attribute1 nvarchar(50) = NULL,
@Attribute2 nvarchar(max) = NULL,
@Node_CourseID int = null,
@IsActive bit = null
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_OfferedCourse]
   SET

[AcademicCalenderID]	=	@AcademicCalenderID,
[DeptID]	=	@DeptID,
[ProgramID]	=	@ProgramID,
[TreeRootID]	=	@TreeRootID,
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[Limit]	=	@Limit,
[Occupied]	=	@Occupied,
[Attribute1]	=	@Attribute1,
[Attribute2]	=	@Attribute2,
Node_CourseID  = @Node_CourseID,
IsActive = @IsActive 

WHERE OfferID = @OfferID

--   update IsOfferedCourse status in RegistrationWorksheet table 
	 
		UPDATE UIUEMS_CC_RegistrationWorksheet
		set IsOfferedCourse = @IsActive
		where CourseID=@CourseID and 
			VersionID=@VersionID and 
			ProgramID=@ProgramID	            
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseDecreaseOccupied]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseDecreaseOccupied]
(	 	
	 @CourseID int 	
	,@VersionID int
	,@ProgramID int
	,@TreeMasterID int	
)
 
AS
BEGIN

	DECLARE @count INT;

	SET @count =(SELECT        Occupied  
					FROM       UIUEMS_CC_OfferedCourse
					WHERE      (CourseID = @CourseID) AND (VersionID = @VersionID) AND (ProgramID = @ProgramID) AND (TreeRootID=@TreeMasterID))


	UPDATE UIUEMS_CC_OfferedCourse 
	SET Occupied = @count - 1
	WHERE  (CourseID = @CourseID) AND (VersionID = @VersionID) AND (ProgramID = @ProgramID) AND (TreeRootID=@TreeMasterID)	 
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseDeleteById]
(
@OfferID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_OfferedCourse]
WHERE OfferID = @OfferID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseDeleteByProgramIdAcaCalId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseDeleteByProgramIdAcaCalId]
(
@ProgramID int = null,
@AcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_OfferedCourse]
WHERE ProgramID = @ProgramID and AcademicCalenderID = @AcademicCalenderID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseDeleteByProgramIdAcaCalIdTreeRootId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseDeleteByProgramIdAcaCalIdTreeRootId]
(
@ProgramID int = null,
@AcademicCalenderID int = null,
@TreeRootID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM UIUEMS_CC_OfferedCourse
WHERE        (ProgramID = @ProgramID) AND (AcademicCalenderID = @AcademicCalenderID) AND (TreeRootID = @TreeRootID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseDTOGetByProgramAcacalTreeroot]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseDTOGetByProgramAcacalTreeroot]
(
@ProgramId int = null,
@AcaCalId int = null,
@TreeRootId int = null
)

AS
BEGIN
SET NOCOUNT OFF;



SELECT        oc.OfferID,  oc.AcademicCalenderID,  oc.DeptID,  oc.ProgramID,  oc.TreeRootID, c.FormalCode, c.VersionCode, c.Title,  oc.CourseID,  oc.VersionID,  oc.Node_CourseID,  oc.Limit,

 (select count(*) from UIUEMS_CC_RegistrationWorksheet  where ProgramID=  oc.ProgramID and TreeMasterID =  oc.TreeRootID and CourseID= oc.CourseID and VersionID= oc.VersionID and IsAutoAssign=1 and OriginalCalID = @AcaCalId) as Assigned, 
 (select count(*) from UIUEMS_CC_RegistrationWorksheet  where CourseID= oc.CourseID and VersionID= oc.VersionID and IsAutoAssign=1 and OriginalCalID = @AcaCalId) as AssignedAll,
 (select count(*) from UIUEMS_CC_RegistrationWorksheet  where ProgramID=  oc.ProgramID and TreeMasterID =  oc.TreeRootID and CourseID= oc.CourseID and VersionID= oc.VersionID and IsAutoOpen=1 and OriginalCalID = @AcaCalId) as Opened, 
 (select count(*) from UIUEMS_CC_RegistrationWorksheet  where CourseID= oc.CourseID and VersionID= oc.VersionID and IsAutoOpen=1 and OriginalCalID = @AcaCalId) as OpenedAll, 
 (select count(*) from UIUEMS_CC_RegistrationWorksheet  where ProgramID=  oc.ProgramID and TreeMasterID =  oc.TreeRootID and CourseID= oc.CourseID and VersionID= oc.VersionID and IsMandatory=1 and OriginalCalID = @AcaCalId) as Mandatory, 
 (select count(*) from UIUEMS_CC_RegistrationWorksheet  where CourseID= oc.CourseID and VersionID= oc.VersionID and IsMandatory=1 and OriginalCalID = @AcaCalId) as MandatoryAll,
 
  oc.CreatedBy,  oc.CreatedDate,  oc.ModifiedBy,  oc.ModifiedDate,  oc.Attribute1,  oc.Attribute2,  oc.IsActive
FROM            UIUEMS_CC_OfferedCourse as oc inner join UIUEMS_CC_Course as c on oc.CourseID=c.CourseID and oc.VersionID=c.VersionID
WHERE        (oc.ProgramID = @ProgramId) AND (oc.TreeRootID = @TreeRootId) AND (oc.AcademicCalenderID = @AcaCalId)

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OfferedCourseGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_CC_OfferedCourse


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OfferedCourseGetById]
(
@OfferID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_CC_OfferedCourse
WHERE     (OfferID = @OfferID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseGetByProgramAcacalTreemasterCourseVersion]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseGetByProgramAcacalTreemasterCourseVersion]
(
@ProgramId int = null,
@AcaCalId int = null,
@TreeRootId int = null,
@CourseID int = null,
@VersionID int = null
)

AS
BEGIN
SET NOCOUNT OFF;



SELECT        oc.OfferID,  oc.AcademicCalenderID,  oc.DeptID,  oc.ProgramID,  oc.TreeRootID,  oc.CourseID,  oc.VersionID,  oc.Node_CourseID,  oc.Limit,
 (select count(*) from UIUEMS_CC_RegistrationWorksheet 
 where ProgramID=  oc.ProgramID and TreeMasterID =  oc.TreeRootID and CourseID= oc.CourseID and VersionID= oc.VersionID and IsAutoAssign=1) as Occupied, 
  oc.CreatedBy,  oc.CreatedDate,  oc.ModifiedBy,  oc.ModifiedDate,  oc.Attribute1,  oc.Attribute2,  oc.IsActive
FROM            UIUEMS_CC_OfferedCourse as oc
WHERE        (ProgramID = @ProgramId) AND (TreeRootID = @TreeRootId) AND (AcademicCalenderID = @AcaCalId) and (CourseID=@CourseID) and (VersionID=@VersionID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseGetByProgramAcacalTreeroot]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseGetByProgramAcacalTreeroot]
(
@ProgramId int = null,
@AcaCalId int = null,
@TreeRootId int = null
)

AS
BEGIN
SET NOCOUNT OFF;



SELECT        oc.OfferID,  oc.AcademicCalenderID,  oc.DeptID,  oc.ProgramID,  oc.TreeRootID,  oc.CourseID,  oc.VersionID,  oc.Node_CourseID,  oc.Limit,
 (select count(*) from UIUEMS_CC_RegistrationWorksheet 
 where ProgramID=  oc.ProgramID and TreeMasterID =  oc.TreeRootID and CourseID= oc.CourseID and VersionID= oc.VersionID and IsAutoAssign=1) as Occupied, 
  oc.CreatedBy,  oc.CreatedDate,  oc.ModifiedBy,  oc.ModifiedDate,  oc.Attribute1,  oc.Attribute2,  oc.IsActive
FROM            UIUEMS_CC_OfferedCourse as oc
WHERE        (ProgramID = @ProgramId) AND (TreeRootID = @TreeRootId) AND (AcademicCalenderID = @AcaCalId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseIncreaseOccupied]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- 841, 2, 3, 2; 

CREATE PROCEDURE  [dbo].[UIUEMS_CC_OfferedCourseIncreaseOccupied]
(	 	
	 @CourseID int 	
	,@VersionID int
	,@ProgramID int	
	,@TreeMasterID int	
)
 
AS
BEGIN
SET NOCOUNT ON;

	DECLARE @count INT, @acaCalId INT;

	Set @acaCalId = (select AcademicCalenderID from UIUEMS_CC_AcademicCalender where IsActiveRegistration=1);

	SET @count =(SELECT        Occupied  
					FROM       UIUEMS_CC_OfferedCourse
					WHERE      (CourseID = @CourseID) AND (VersionID = @VersionID) AND (ProgramID = @ProgramID) AND (TreeRootID=@TreeMasterID) AND (AcademicCalenderID=@acaCalId))
					
	UPDATE UIUEMS_CC_OfferedCourse 
	SET Occupied = isnull( @count, 0) + 1
	WHERE  (CourseID = @CourseID) AND (VersionID = @VersionID) AND (ProgramID = @ProgramID) AND (TreeRootID = @TreeMasterID) AND (AcademicCalenderID = @acaCalId)
END 


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OfferedCourseInsert] 
(
@OfferID int  OUTPUT,
@AcademicCalenderID int  = NULL,
@DeptID int = NULL,
@ProgramID int = NULL,
@TreeRootID int = NULL,
@CourseID int  = NULL,
@VersionID int  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@Limit int = NULL,
@Occupied int = NULL,
@Attribute1 nvarchar(50) = NULL,
@Attribute2 nvarchar(max) = NULL,
@Node_CourseID int = null,
@IsActive bit = null
)

AS
BEGIN
SET NOCOUNT ON;

	IF not exists (select * from UIUEMS_CC_OfferedCourse where  CourseID = @CourseID and  
																VersionID = @VersionID and 
																ProgramID = @ProgramID and 
																AcademicCalenderID = @AcademicCalenderID and 
																TreeRootID = @TreeRootID)
	BEGIN
		INSERT INTO [UIUEMS_CC_OfferedCourse]
		(
		[AcademicCalenderID],
		[DeptID],
		[ProgramID],
		[TreeRootID],
		[CourseID],
		[VersionID],
		[CreatedBy],
		[CreatedDate],
		[ModifiedBy],
		[ModifiedDate],
		[Limit],
		[Occupied],
		[Attribute1],
		[Attribute2],
		[Node_CourseID],
		[IsActive]
		)
		 VALUES
		(
		@AcademicCalenderID,
		@DeptID,
		@ProgramID,
		@TreeRootID,
		@CourseID,
		@VersionID,
		@CreatedBy,
		@CreatedDate,
		@ModifiedBy,
		@ModifiedDate,
		@Limit,
		@Occupied,
		@Attribute1,
		@Attribute2,
		@Node_CourseID,
		@IsActive
		)
           
		SET @OfferID = SCOPE_IDENTITY()
	END
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OfferedCourseUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OfferedCourseUpdate]
(
@OfferID int  = NULL,
@AcademicCalenderID int  = NULL,
@DeptID int = NULL,
@ProgramID int = NULL,
@TreeRootID int = NULL,
@CourseID int  = NULL,
@VersionID int  = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@Limit int = NULL,
@Occupied int = NULL,
@Attribute1 nvarchar(50) = NULL,
@Attribute2 nvarchar(max) = NULL,
@Node_CourseID int = null,
@IsActive bit = null

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_OfferedCourse]
   SET

[AcademicCalenderID]	=	@AcademicCalenderID,
[DeptID]	=	@DeptID,
[ProgramID]	=	@ProgramID,
[TreeRootID]	=	@TreeRootID,
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[Limit]	=	@Limit,
[Occupied]	=	@Occupied,
[Attribute1]	=	@Attribute1,
[Attribute2]	=	@Attribute2,
Node_CourseID  = @Node_CourseID,
IsActive = @IsActive 

WHERE OfferID = @OfferID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OperatorDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_OperatorDeleteById]
(
@OperatorID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Operator]
WHERE OperatorID = @OperatorID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OperatorGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OperatorGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_CC_Operator


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OperatorGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OperatorGetById]
(
@OperatorID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Operator
WHERE     (OperatorID = @OperatorID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OperatorInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OperatorInsert] 
(
@OperatorID int  OUTPUT,
@Name varchar(50)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Operator]
(
[OperatorID],
[Name]

)
 VALUES
(
@OperatorID,
@Name

)
           
SET @OperatorID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_OperatorUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_OperatorUpdate]
(
@OperatorID int  = NULL,
@Name varchar(50)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Operator]
   SET
[Name]	=	@Name
WHERE OperatorID = @OperatorID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteDetailDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_PrerequisiteDetailDeleteById]
(
@PrerequisiteID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_PrerequisiteDetail]
WHERE PrerequisiteID = @PrerequisiteID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteDetailGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteDetailGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_PrerequisiteDetail


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteDetailGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteDetailGetById]
(
@PrerequisiteID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_PrerequisiteDetail
WHERE     (PrerequisiteID = @PrerequisiteID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteDetailGetByProgramId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteDetailGetByProgramId]
(
@ProgramID int = null
)

AS
BEGIN
SET NOCOUNT ON;

select * from UIUEMS_CC_PrerequisiteDetail as pd 
inner join UIUEMS_CC_PrerequisiteMaster as pm on pd.PrerequisiteMasterID=pm.PrerequisiteMasterID
where pm.ProgramID=@ProgramID  

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteDetailInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteDetailInsert] 
(
	@PrerequisiteID int  OUTPUT,
	@PrerequisiteMasterID int = NULL,
	@NodeCourseID int = NULL,
	@PreReqNodeCourseID int = NULL,
	@OperatorID int = NULL,
	@OperatorIDMinOccurance int = NULL,
	@ReqCredits numeric(18, 2) = NULL,
	@NodeID int = NULL,
	@PreReqNodeID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_PrerequisiteDetail]
(
	[PrerequisiteID],
	[PrerequisiteMasterID],
	[NodeCourseID],
	[PreReqNodeCourseID],
	[OperatorID],
	[OperatorIDMinOccurance],
	[ReqCredits],
	[NodeID],
	[PreReqNodeID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@PrerequisiteID,
	@PrerequisiteMasterID,
	@NodeCourseID,
	@PreReqNodeCourseID,
	@OperatorID,
	@OperatorIDMinOccurance,
	@ReqCredits,
	@NodeID,
	@PreReqNodeID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @PrerequisiteID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteDetailUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteDetailUpdate]
(
	@PrerequisiteID int  = NULL,
	@PrerequisiteMasterID int = NULL,
	@NodeCourseID int = NULL,
	@PreReqNodeCourseID int = NULL,
	@OperatorID int = NULL,
	@OperatorIDMinOccurance int = NULL,
	@ReqCredits numeric(18, 2) = NULL,
	@NodeID int = NULL,
	@PreReqNodeID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_PrerequisiteDetail]
   SET


	[PrerequisiteMasterID]	=	@PrerequisiteMasterID,
	[NodeCourseID]	=	@NodeCourseID,
	[PreReqNodeCourseID]	=	@PreReqNodeCourseID,
	[OperatorID]	=	@OperatorID,
	[OperatorIDMinOccurance]	=	@OperatorIDMinOccurance,
	[ReqCredits]	=	@ReqCredits,
	[NodeID]	=	@NodeID,
	[PreReqNodeID]	=	@PreReqNodeID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE PrerequisiteID = @PrerequisiteID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_PrerequisiteMasterDeleteById]
(
@PrerequisiteMasterID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_PrerequisiteMaster]
WHERE PrerequisiteMasterID = @PrerequisiteMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_PrerequisiteMaster


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteMasterGetById]
(
@PrerequisiteMasterID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_PrerequisiteMaster
WHERE     (PrerequisiteMasterID = @PrerequisiteMasterID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteMasterInsert] 
(
	@PrerequisiteMasterID int  OUTPUT,
	@Name varchar(50) = NULL,
	@ProgramID int = NULL,
	@NodeID int = NULL,
	@NodeCourseID int = NULL,
	@ReqCredits numeric(18, 2) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_PrerequisiteMaster]
(
	[PrerequisiteMasterID],
	[Name],
	[ProgramID],
	[NodeID],
	[NodeCourseID],
	[ReqCredits],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@PrerequisiteMasterID,
	@Name,
	@ProgramID,
	@NodeID,
	@NodeCourseID,
	@ReqCredits,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @PrerequisiteMasterID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_PrerequisiteMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_PrerequisiteMasterUpdate]
(
	@PrerequisiteMasterID int  = NULL,
	@Name varchar(50) = NULL,
	@ProgramID int = NULL,
	@NodeID int = NULL,
	@NodeCourseID int = NULL,
	@ReqCredits numeric(18, 2) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_PrerequisiteMaster]
   SET
   
	[Name]	=	@Name,
	[ProgramID]	=	@ProgramID,
	[NodeID]	=	@NodeID,
	[NodeCourseID]	=	@NodeCourseID,
	[ReqCredits]	=	@ReqCredits,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE PrerequisiteMasterID = @PrerequisiteMasterID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_ProgramDeleteById]
(
@ProgramID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Program]
WHERE ProgramID = @ProgramID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[ProgramID],
[Code],
[ShortName],
[TotalCredit],
[DeptID],
[DetailName],
[ProgramTypeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_Program


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramGetById]
(
	@ProgramID int = NULL
)
AS
BEGIN
SET NOCOUNT ON;

SELECT     

[ProgramID],
[Code],
[ShortName],
[TotalCredit],
[DeptID],
[DetailName],
[ProgramTypeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM	UIUEMS_CC_Program
Where ProgramID = @ProgramID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramInsert] 
(
@ProgramID int  OUTPUT,
@Code varchar(50)  = NULL,
@ShortName varchar(100)  = NULL,
@TotalCredit money = NULL,
@DeptID int  = NULL,
@DetailName varchar(100) = NULL,
@ProgramTypeID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Program]
(
[ProgramID],
[Code],
[ShortName],
[TotalCredit],
[DeptID],
[DetailName],
[ProgramTypeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]
)
 VALUES
(
@ProgramID,
@Code,
@ShortName,
@TotalCredit,
@DeptID,
@DetailName,
@ProgramTypeID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @ProgramID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_ProgramTypeDeleteById]
(
@ProgramTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_ProgramType]
WHERE ProgramTypeID = @ProgramTypeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[ProgramTypeID],
[TypeDescription]

FROM       UIUEMS_CC_ProgramType


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramTypeGetById]
(
@ProgramTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[ProgramTypeID],
[TypeDescription]

FROM       UIUEMS_CC_ProgramType
WHERE     (ProgramTypeID = @ProgramTypeID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramTypeInsert] 
(
@ProgramTypeID int   OUTPUT,

@TypeDescription varchar(200)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_ProgramType]
(
[ProgramTypeID],
[TypeDescription]

)
 VALUES
(
@ProgramTypeID,
@TypeDescription

)
           
SET @ProgramTypeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramTypeUpdate]
(
@ProgramTypeID int   = NULL,
@TypeDescription varchar(200)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_ProgramType]
   SET

[TypeDescription]	=	@TypeDescription


WHERE ProgramTypeID = @ProgramTypeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_ProgramUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_ProgramUpdate]
(
@ProgramID int  = NULL,
@Code varchar(50)  = NULL,
@ShortName varchar(100)  = NULL,
@TotalCredit money = NULL,
@DeptID int  = NULL,
@DetailName varchar(100) = NULL,
@ProgramTypeID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Program]
   SET

[Code]	=	@Code,
[ShortName]	=	@ShortName,
[TotalCredit]	=	@TotalCredit,
[DeptID]	=	@DeptID,
[DetailName]	=	@DetailName,
[ProgramTypeID]	=	@ProgramTypeID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE ProgramID = @ProgramID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetAutoMandatory]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetAutoMandatory]
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int,
@CrOpenLimit numeric(18,2),
@ReturnValue int OUTPUT


AS
BEGIN
SET NOCOUNT ON;

UPDATE   UIUEMS_CC_RegistrationWorksheet
set IsMandatory = 0 
where StudentId = @StudentId;


--STEP 1 START : Open Course(Auto open) by locality defination Setup, By priority and GPA range 
--
-- Load course which course status are not in 'Passed', 'Running', 'Incomplete', 'Waiver', 'Transfer'

DECLARE 
@s4_id int,
@s4_CourseId int, 
@s4_VersionId int,
@s4_SequenceNo int,
@s4_Credit int,
@s4_CreditCount int,
@s4_Priority int,
@s4_Result int,
@s4_Node_CourseID int,
@s4_ProgramId int,
@s4_IdForAssocCourse int,
@s4_AssocCourseID int, 
@s4_AssocVersionID int,
@s4_TempId int,
@s4_PriorityCheck int,
@s4_NodeID int,


--Course Status
@crsStatusPassedT int,
@crsStatusPassedNt int,
@crsStatusIncomplete int,
@crsStatusFail int,
@crsStatusRunning int,
@crsStatusX int,
@crsStatusWaiver int,
@crsStatusTransfer int;

DECLARE @S4TempTable TABLE(
							Id    int,
							CourseId int, 
							VersionId int,
							[Priority] int
						  )

 set @crsStatusPassedT  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pt')
 set @crsStatusPassedNt = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pn') 
 set @crsStatusIncomplete  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='I')
 set @crsStatusFail = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='F')
 set @crsStatusRunning  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='R')
 set @crsStatusX = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='X')
 set @crsStatusWaiver  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Wv')
 set @crsStatusTransfer = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Tr')


set @s4_CreditCount = 1;
set @s4_PriorityCheck = 0;

DECLARE  @Cursor_AutoOpenCourseByLocality CURSOR
	SET @Cursor_AutoOpenCourseByLocality = CURSOR FAST_FORWARD  FOR
		SELECT 
			id ,
			CourseId , 
			VersionId ,
			[Priority] ,
			Node_CourseID,
			ProgramId,
			Credits,
			NodeID
		FROM dbo.[UIUEMS_CC_RegistrationWorksheet]
		WHERE	StudentID = @StudentId and 
				IsOfferedCourse = 1 and
				IsAutoOpen = 1 and
				(CourseStatusId  not in(@crsStatusPassedT,
										@crsStatusPassedNt,
										@crsStatusIncomplete,																 
										@crsStatusRunning,
										@crsStatusX,
										@crsStatusWaiver,
										@crsStatusTransfer,
										-1) or 
										CourseStatusId is null) 
				ORDER BY [Priority]
		
		OPEN @Cursor_AutoOpenCourseByLocality
		FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
		INTO    @s4_id ,
				@s4_CourseId , 
				@s4_VersionId ,
				@s4_Priority,
				@s4_Node_CourseID,
				@s4_ProgramId,
				@s4_Credit,
				@s4_NodeID
		
		WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
				if((@s4_CreditCount <= @CrOpenLimit) OR (@s4_PriorityCheck = @s4_Priority)) -- # eleminate course which are out of range.	
					BEGIN
							if((@s4_NodeID is null) or (@s4_NodeID = 0))
							BEGIN
											SET @s4_Result =  dbo.CheckPrerequisitForNodeCoures(@s4_Node_CourseID, @s4_ProgramId, @StudentId); --Need Change					
											IF(@s4_Result = 1) -- # eleminate course which prerequisit are not done. 1 means complete.	
											BEGIN
												
												INSERT INTO @S4TempTable 
												(Id , CourseId , VersionId, [Priority]) VALUES
												(@s4_id , @s4_CourseId , @s4_VersionId,	@s4_Priority)
							
												SELECT @s4_AssocCourseID = AssocCourseID , @s4_AssocVersionID =AssocVersionID
												FROM UIUEMS_CC_Course WHERE CourseID = @s4_CourseId and   VersionID = @s4_VersionId
							
												IF ( @s4_AssocCourseID is not null)
												BEGIN

													Select @s4_IdForAssocCourse = id  from UIUEMS_CC_RegistrationWorksheet 
																					  where CourseID=@s4_AssocCourseID and 
																							VersionID=@s4_AssocVersionID and
																							StudentID= @StudentId and 
																							(CourseStatusId not in(@crsStatusPassedT, @crsStatusPassedNt,@crsStatusIncomplete,@crsStatusRunning,@crsStatusWaiver,@crsStatusTransfer) or 
																							CourseStatusId is null)

													INSERT INTO @S4TempTable 
													(Id , CourseId , VersionId, [Priority]) VALUES
													(@s4_IdForAssocCourse , @s4_AssocCourseID , @s4_AssocVersionID,	@s4_Priority)
												END
							
											END
									END
							--//#
							SET @s4_CreditCount = @s4_CreditCount + @s4_Credit;		
						
											
					END
				FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
				INTO    @s4_id ,
						@s4_CourseId , 
						@s4_VersionId ,
						@s4_Priority,
						@s4_Node_CourseID,
						@s4_ProgramId,
						@s4_Credit,
						@s4_NodeID
				
				END
							
	CLOSE @Cursor_AutoOpenCourseByLocality
	DEALLOCATE @Cursor_AutoOpenCourseByLocality

	--#	 Update worksheet	
	DECLARE @CourseID int, @VersionID int;


	DECLARE  @Cursor_TempTable CURSOR
	SET @Cursor_TempTable = CURSOR  FAST_FORWARD FOR
		SELECT 
			id , CourseID, VersionID
		FROM @S4TempTable
				
		OPEN @Cursor_TempTable
		FETCH NEXT FROM @Cursor_TempTable 
		INTO    @s4_TempId , @CourseID, @VersionID
				
				WHILE (@@FETCH_STATUS <> -1)
				BEGIN

				select @CourseID, @VersionID, @ProgramID, @TreeMasterID;
				
					UPDATE   [UIUEMS_CC_RegistrationWorksheet]
					   SET 	IsMandatory = 'True'--, IsAutoAssign = 'True'
					 WHERE (id =  @s4_TempId)

					  EXEC UIUEMS_CC_OfferedCourseIncreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 
				
					FETCH NEXT FROM @Cursor_TempTable 
					INTO   @s4_TempId , @CourseID, @VersionID
				END
				
	CLOSE @Cursor_TempTable
	DEALLOCATE @Cursor_TempTable
				
	--# END Update	
	
--STEP 4 END
---------------------------------------------------------------------- 

--Return result
-- error checking not complete

SET @ReturnValue = 1;
--return @ReturnValue;
--
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetAutoOpen]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetAutoOpen] 
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int,
@CrOpenLimit numeric(18,2),
@ReturnValue int OUTPUT


AS
BEGIN --1
	SET NOCOUNT ON;

	UPDATE   UIUEMS_CC_RegistrationWorksheet
	set IsAutoOpen = 0 
	where StudentId = @StudentId;


	--STEP 1 START : Open Course(Auto open) by locality defination Setup, By priority and GPA range 
	--
	-- Load course which course status are not in 'Passed', 'Running', 'Incomplete', 'Waiver', 'Transfer'

	DECLARE 
	@s4_id int,
	@s4_CourseId int, 
	@s4_VersionId int,
	@s4_SequenceNo int,
	@s4_Credit int,
	@s4_CreditCount int,
	@s4_Priority int,
	@s4_Result int,
	@s4_Node_CourseID int,
	@s4_ProgramId int,
	@s4_IdForAssocCourse int,
	@s4_AssocCourseID int, 
	@s4_AssocVersionID int,
	@s4_PriorityCheck int,
	@s4_Sequence int,
	@s4_ResultEquiChk int,

	--Course Status
	@crsStatusPassedT int,
	@crsStatusPassedNt int,
	@crsStatusIncomplete int,
	@crsStatusFail int,
	@crsStatusRunning int,
	@crsStatusX int,
	@crsStatusWaiver int,
	@crsStatusTransfer int;

	DECLARE @S4TempTable TABLE(
	Id    int,
	CourseId int, 
	VersionId int,
	[Priority] int,
	Sequence int
	)


	 set @crsStatusPassedT  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pt')
	 set @crsStatusPassedNt = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pn') 
	 set @crsStatusIncomplete  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='I')
	 set @crsStatusFail = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='F')
	 set @crsStatusRunning  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='R')
	 set @crsStatusX = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='X')
	 set @crsStatusWaiver  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Wv')
	 set @crsStatusTransfer = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Tr')


	set @s4_Sequence = 1;
	set @s4_CreditCount = 1;
	set @s4_PriorityCheck = 0;

	DECLARE  @Cursor_AutoOpenCourseByLocality CURSOR
	 SET @Cursor_AutoOpenCourseByLocality = CURSOR FAST_FORWARD  FOR
	  SELECT 
	   id ,
	   CourseId , 
	   VersionId ,
	   [Priority] ,
	   Node_CourseID,
	   ProgramId,
	   Credits
	  FROM dbo.[UIUEMS_CC_RegistrationWorksheet]
	  WHERE StudentID = @StudentId and 
		IsOfferedCourse = 1 and
		(CourseStatusId  not in(@crsStatusPassedT,
			  @crsStatusPassedNt,
			  @crsStatusIncomplete,                 
			  @crsStatusRunning,
			  @crsStatusX,
			  @crsStatusWaiver,
			  @crsStatusTransfer,
			  -1) or 
		CourseStatusId is null) 
		ORDER BY [Priority]
  
	  OPEN @Cursor_AutoOpenCourseByLocality
	  FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
	  INTO    @s4_id ,
		@s4_CourseId , 
		@s4_VersionId ,
		@s4_Priority,
		@s4_Node_CourseID,
		@s4_ProgramId,
		@s4_Credit
  
	  WHILE (@@FETCH_STATUS <> -1)
		BEGIN --2
    
		if((@s4_CreditCount <= @CrOpenLimit) OR (@s4_PriorityCheck = @s4_Priority)) -- # eleminate course which are out of range. 
		 BEGIN --3

			   SET @s4_Result =  dbo.CheckPrerequisitForNodeCoures(@s4_Node_CourseID, @s4_ProgramId, @StudentId); --Need Change     
			   IF(@s4_Result = 1) -- # eleminate course which prerequisit are not done. 1 means complete. 
			   BEGIN --4

					SET @s4_ResultEquiChk = dbo.CheckEquivalentCourse( @s4_CourseId , @s4_VersionId, @StudentId)
					IF(@s4_ResultEquiChk < 1)
					BEGIN --5

							INSERT INTO @S4TempTable 
							(Id , CourseId , VersionId, [Priority], Sequence) VALUES
							(@s4_id , @s4_CourseId , @s4_VersionId, @s4_Priority, @s4_Sequence)
       
							SELECT @s4_AssocCourseID = AssocCourseID , @s4_AssocVersionID = AssocVersionID
							FROM UIUEMS_CC_Course WHERE CourseID = @s4_CourseId and   VersionID = @s4_VersionId

				
       
							IF ( @s4_AssocCourseID is not null)
							BEGIN --6

								Select @s4_IdForAssocCourse = id from UIUEMS_CC_RegistrationWorksheet 
															  where CourseID=@s4_AssocCourseID and 
																	VersionID=@s4_AssocVersionID and
																	StudentID= @StudentId and 
																	(CourseStatusId not in(3,6,7,10,11) or CourseStatusId is null)
								INSERT INTO @S4TempTable 
								(Id , CourseId , VersionId, [Priority], Sequence) VALUES
								(@s4_IdForAssocCourse , @s4_AssocCourseID , @s4_AssocVersionID, @s4_Priority, @s4_Sequence)
							END --6

							IF(  @s4_PriorityCheck != @s4_Priority )
							BEGIN --7
								SET @s4_CreditCount = @s4_CreditCount + @s4_Credit;
								SET @s4_PriorityCheck = @s4_Priority;

								SET @s4_Sequence = @s4_Sequence + 1;
							END --7
					 END --5
			   END --4
		 END --3
		FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
		INTO    @s4_id ,
		  @s4_CourseId , 
		  @s4_VersionId ,
		  @s4_Priority,
		  @s4_Node_CourseID,
		  @s4_ProgramId,
		  @s4_Credit 
    
		END --2
    

    
	 --#  Update worksheet 

	 DECLARE @s4_TempId INT, @s4_TempSequence INT, @s4_TempCourseId INT, @s4_TempVersionId INT;

 

	 DECLARE  @Cursor_TempTable CURSOR
	 SET @Cursor_TempTable = CURSOR  FAST_FORWARD FOR
	  SELECT 
	   id , Sequence, CourseId, VersionId
	  FROM @S4TempTable
    
	  OPEN @Cursor_TempTable
	  FETCH NEXT FROM @Cursor_TempTable 
	  INTO    @s4_TempId , @s4_TempSequence, @s4_TempCourseId  , @s4_TempVersionId
    
		WHILE (@@FETCH_STATUS <> -1)
		BEGIN --8    
			UPDATE  [UIUEMS_CC_RegistrationWorksheet]
			SET  IsAutoOpen = 'True', SequenceNo = @s4_TempSequence
			WHERE (id =  @s4_TempId)
    
			FETCH NEXT FROM @Cursor_TempTable 
			INTO     @s4_TempId , @s4_TempSequence, @s4_TempCourseId  , @s4_TempVersionId
		END --8
    
	 CLOSE @Cursor_TempTable
	 DEALLOCATE @Cursor_TempTable
    
	 --# END Update  
    
	 CLOSE @Cursor_AutoOpenCourseByLocality
	 DEALLOCATE @Cursor_AutoOpenCourseByLocality
 
	--STEP 4 END
	----------------------------------------------------------------------
	----------------------------------------------------------------------

	--Return result
	-- error checking not complete

	SET @ReturnValue = 1;
	--return @ReturnValue;
--
END --1


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetAutoPreRegistration]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Batch submitted through debugger: SQLQuery6.sql|7|0|C:\Users\EMON\AppData\Local\Temp\~vsF800.sql

CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetAutoPreRegistration]
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int,
@CrOpenLimit numeric(18,2),
@ReturnValue int OUTPUT


AS
BEGIN
SET NOCOUNT ON;

UPDATE   UIUEMS_CC_RegistrationWorksheet
set IsAutoAssign = 0 
where StudentId = @StudentId;


--STEP 1 START : Open Course(Auto open) by locality defination Setup, By priority and GPA range 
--
-- Load course which course status are not in 'Passed', 'Running', 'Incomplete', 'Waiver', 'Transfer'

DECLARE 
@s4_id int,
@s4_CourseId int, 
@s4_VersionId int,
@s4_SequenceNo int,
@s4_Credit int,
@s4_CreditCount int,
@s4_Priority int,
@s4_Result int,
@s4_Node_CourseID int,
@s4_ProgramId int,
@s4_IdForAssocCourse int,
@s4_AssocCourseID int, 
@s4_AssocVersionID int,
@s4_TempId int,
@s4_PriorityCheck int,
@s4_NodeID int,

--Course Status
@crsStatusPassedT int,
@crsStatusPassedNt int,
@crsStatusIncomplete int,
@crsStatusFail int,
@crsStatusRunning int,
@crsStatusX int,
@crsStatusWaiver int,
@crsStatusTransfer int;

DECLARE @S4TempTable TABLE(
							Id    int,
							CourseId int, 
							VersionId int,
							[Priority] int
						  )

 set @crsStatusPassedT  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pt')
 set @crsStatusPassedNt = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pn') 
 set @crsStatusIncomplete  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='I')
 set @crsStatusFail = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='F')
 set @crsStatusRunning  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='R')
 set @crsStatusX = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='X')
 set @crsStatusWaiver  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Wv')
 set @crsStatusTransfer = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Tr')


set @s4_CreditCount = 1;
set @s4_PriorityCheck = 0;

DECLARE  @Cursor_AutoOpenCourseByLocality CURSOR
	SET @Cursor_AutoOpenCourseByLocality = CURSOR FAST_FORWARD  FOR
		SELECT 
			id ,
			CourseId , 
			VersionId ,
			[Priority] ,
			Node_CourseID,
			ProgramId,
			Credits,
			NodeID
		FROM dbo.[UIUEMS_CC_RegistrationWorksheet]
		WHERE	StudentID = @StudentId and 
				IsOfferedCourse = 1 and
				IsAutoOpen = 1 and
				(CourseStatusId  not in(@crsStatusPassedT,
										@crsStatusPassedNt,
										@crsStatusIncomplete,																 
										@crsStatusRunning,
										@crsStatusX,
										@crsStatusWaiver,
										@crsStatusTransfer,
										-1) or 
										CourseStatusId is null) 
				ORDER BY [Priority]
		
		OPEN @Cursor_AutoOpenCourseByLocality
		FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
		INTO    @s4_id ,
				@s4_CourseId , 
				@s4_VersionId ,
				@s4_Priority,
				@s4_Node_CourseID,
				@s4_ProgramId,
				@s4_Credit,
				@s4_NodeID
		
		WHILE (@@FETCH_STATUS <> -1)
				BEGIN
				
				if((@s4_CreditCount <= @CrOpenLimit)) -- # eleminate course which are out of range.	
					BEGIN
							if((@s4_NodeID is null) or (@s4_NodeID = 0))
							BEGIN
									SET @s4_Result =  dbo.CheckPrerequisitForNodeCoures(@s4_Node_CourseID, @s4_ProgramId, @StudentId); --Need Change					
									IF(@s4_Result = 1) -- # eleminate course which prerequisit are not done. 1 means complete.	
									BEGIN
												
										INSERT INTO @S4TempTable 
										(Id , CourseId , VersionId, [Priority]) VALUES
										(@s4_id , @s4_CourseId , @s4_VersionId,	@s4_Priority)
							
										SELECT @s4_AssocCourseID = AssocCourseID , @s4_AssocVersionID =AssocVersionID
										FROM UIUEMS_CC_Course WHERE CourseID = @s4_CourseId and   VersionID = @s4_VersionId
							
										IF ( @s4_AssocCourseID is not null)
										BEGIN

											Select @s4_IdForAssocCourse = id  from UIUEMS_CC_RegistrationWorksheet 
																				where CourseID=@s4_AssocCourseID and 
																					VersionID=@s4_AssocVersionID and
																					StudentID= @StudentId and 
																					(CourseStatusId not in(@crsStatusPassedT, @crsStatusPassedNt,@crsStatusIncomplete,@crsStatusRunning,@crsStatusWaiver,@crsStatusTransfer) or 
																					CourseStatusId is null)

											INSERT INTO @S4TempTable 
											(Id , CourseId , VersionId, [Priority]) VALUES
											(@s4_IdForAssocCourse , @s4_AssocCourseID , @s4_AssocVersionID,	@s4_Priority)
										END
							
									END	
							END
							--//#
							SET @s4_CreditCount = @s4_CreditCount + @s4_Credit;	

							 
					END
				FETCH NEXT FROM @Cursor_AutoOpenCourseByLocality 
				INTO    @s4_id ,
						@s4_CourseId , 
						@s4_VersionId ,
						@s4_Priority,
						@s4_Node_CourseID,
						@s4_ProgramId,
						@s4_Credit,
						@s4_NodeID	
				
				END
							
	CLOSE @Cursor_AutoOpenCourseByLocality
	DEALLOCATE @Cursor_AutoOpenCourseByLocality

	--#	 Update worksheet	
	DECLARE @CourseID int, @VersionID int;


	DECLARE  @Cursor_TempTable CURSOR
	SET @Cursor_TempTable = CURSOR  FAST_FORWARD FOR
		SELECT 
			id , CourseID, VersionID
		FROM @S4TempTable
				
		OPEN @Cursor_TempTable
		FETCH NEXT FROM @Cursor_TempTable 
		INTO    @s4_TempId , @CourseID, @VersionID
				
				WHILE (@@FETCH_STATUS <> -1)
				BEGIN 
				
					UPDATE   [UIUEMS_CC_RegistrationWorksheet]
					   SET 	IsAutoAssign = 'True'
					 WHERE (id =  @s4_TempId)

					  EXEC UIUEMS_CC_OfferedCourseIncreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 
				
					FETCH NEXT FROM @Cursor_TempTable 
					INTO   @s4_TempId , @CourseID, @VersionID
				END
				
	CLOSE @Cursor_TempTable
	DEALLOCATE @Cursor_TempTable
				
	--# END Update	
	
--STEP 4 END
---------------------------------------------------------------------- 

--Return result
-- error checking not complete

SET @ReturnValue = 1;
--return @ReturnValue;
--
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetCountMandatoryCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetCountMandatoryCourse] 
	
	@Count int output,
	@ProgramID int = null,
	@CourseID int = null,
	@VersionID int = null,
	@TreeMasterID int = null
	
AS
BEGIN
	
	SET NOCOUNT ON;

   set @Count = (select count(*) from UIUEMS_CC_RegistrationWorksheet 
					where ProgramID=@ProgramID and CourseID=@CourseID and VersionID=@VersionID and TreeMasterID=@TreeMasterID and IsMandatory=1)
   return @Count;
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetCountOpenCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetCountOpenCourse] 
	
	@Count int output,
	@ProgramID int = null,
	@CourseID int = null,
	@VersionID int = null,
	@TreeMasterID int = null
	
AS
BEGIN
	
	SET NOCOUNT ON;

   set @Count = (select count(*) from UIUEMS_CC_RegistrationWorksheet 
					where ProgramID=@ProgramID and CourseID=@CourseID and VersionID=@VersionID and TreeMasterID=@TreeMasterID and IsAutoOpen=1)
   return @Count;
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetCountTakenCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetCountTakenCourse] 
	
	@Count int output,
	@ProgramID int = null,
	@CourseID int = null,
	@VersionID int = null,
	@TreeMasterID int = null
	
AS
BEGIN
	
	SET NOCOUNT ON;

   set @Count = (select count(*) from UIUEMS_CC_RegistrationWorksheet 
					where ProgramID=@ProgramID and CourseID=@CourseID and VersionID=@VersionID and TreeMasterID=@TreeMasterID and IsAutoAssign=1)
   return @Count;
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetCourseRegistrationForStudent]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetCourseRegistrationForStudent]
@StudentId int = null
AS
BEGIN

  UPDATE UIUEMS_CC_RegistrationWorksheet SET IsRegistered=1 WHERE id in ( SELECT ID
																		  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
																		  WHERE StudentID = @StudentId and ISNULL( SectionName ,'') != '' )
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetDelete]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetDelete]
(
@ID int  = NULL)

AS
BEGIN
SET NOCOUNT ON;

DELETE FROM [dbo].[uiuems_cc_RegistrationWorksheet]   
 WHERE ID =@ID
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGeneratePerStudent]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGeneratePerStudent] 
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int, 
@ReturnValue int OUTPUT


AS
BEGIN
SET NOCOUNT ON;

DELETE FROM UIUEMS_CC_RegistrationWorksheet WHERE StudentId = @StudentId;

DECLARE @Pre_Major_Node int,@Post_Major_Node int,@Post_MajorNode_Level int,@IsMajorNode bit,@IsMinorNode bit,@Major1Node int, @RegistrationSession int,
--* For CURSOR TreeCalendarDetail c1
@c1_TreeCalendarDetailID int,	@c1_TreeCalendarMasterID int,
--* For CURSOR Cal_Course_Prog_Node c2
@c2_CalCorProgNodeID int, @c2_TreeCalendarDetailID int,	@c2_OfferedByProgramID int,	@c2_CourseID int, @c2_VersionID int, @c2_Node_CourseID int,	@c2_NodeID int,	@c2_NodeLinkName varchar(100),	@c2_Priority int,	@c2_Credits decimal(18,2),	@c2_IsMajorRelated bit, @c2_IsMinorRelated bit,	@C2_PostMajorLevel int,
-- 
@c3_CourseID int ,	@c3_VersionID int,	@c3_Node_CourseID int;

DECLARE @tblMinorNodeList TABLE(ID INT IDENTITY(1,1), ParentNode INT, ChildNode INT );
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 1 START : Generate course list from tree with priority of calender.

DECLARE		@Cursor_TreeCalendarDetail CURSOR, @Cursor_Cal_Course_Prog_Node CURSOR, @Cursor_Node CURSOR, @crs  CURSOR

set @RegistrationSession = (select AcademicCalenderID from UIUEMS_CC_AcademicCalender where IsActiveRegistration=1);

	--* CURSOR TreeCalendarDetail
	SET @Cursor_TreeCalendarDetail = CURSOR FAST_FORWARD FOR
		SELECT TreeCalendarDetailID,TreeCalendarMasterID 
		FROM dbo.UIUEMS_CC_TreeCalendarDetail 
		WHERE TreeCalendarMasterID = @TreeCalendarMasterID and TreeMasterID = @TreeMasterID

	OPEN @Cursor_TreeCalendarDetail
		FETCH NEXT FROM @Cursor_TreeCalendarDetail INTO @c1_TreeCalendarDetailID, @c1_TreeCalendarMasterID  
				
		WHILE @@FETCH_STATUS = 0
		BEGIN -- [1]
			    	--* CURSOR Cal_Course_Prog_Node			
					SET @Cursor_Cal_Course_Prog_Node = CURSOR FAST_FORWARD  FOR
					SELECT CalCorProgNodeID, TreeCalendarDetailID, OfferedByProgramID, CourseID, VersionID, Node_CourseID, NodeID, NodeLinkName,  [Priority],  Credits, IsMajorRelated, IsMinorRelated, PostMajorLevel
					FROM dbo.UIUEMS_CC_Cal_Course_Prog_Node
					WHERE TreeCalendarDetailID  = @c1_TreeCalendarDetailID			
					
					 OPEN @Cursor_Cal_Course_Prog_Node
						FETCH NEXT FROM @Cursor_Cal_Course_Prog_Node INTO
									@c2_CalCorProgNodeID, @c2_TreeCalendarDetailID, @c2_OfferedByProgramID,@c2_CourseID, @c2_VersionID , @c2_Node_CourseID , @c2_NodeID , @c2_NodeLinkName ,@c2_Priority , @c2_Credits , @c2_IsMajorRelated, @c2_IsMinorRelated, @C2_PostMajorLevel  						
						
						WHILE @@FETCH_STATUS = 0
						BEGIN -- [2]

							IF(@c2_CourseID IS NULL OR @c2_CourseID = 0) -- this if-condition Means Node Is available.
							BEGIN -- [3] 

								-- set  Major Node related information.
								SELECT @Pre_Major_Node = @c2_NodeID,  @IsMajorNode = @c2_IsMajorRelated, @IsMinorNode = @c2_IsMinorRelated, @Post_MajorNode_Level = @C2_PostMajorLevel
							 
								-- set Student Major Node.
								SELECT @Major1Node = (SELECT Major1NodeID FROM UIUEMS_ER_Student WHERE StudentID = @StudentId)							 

								--#@# Condition 1 (Student major define & major declare)
								IF((@Major1Node IS NOT NULL OR @Major1Node > 0) AND @IsMajorNode = 1 )
								BEGIN -- [4]

									SELECT @Post_Major_Node =  dbo.GetNextLevelNodeFromMajorNode(@Major1Node , @Post_MajorNode_Level);
									--SELECT   dbo.GetNextLevelNodeFromMajorNode(95 , 2);
									
										-- Get all course from Post-Major-Level-Node and insert
										EXEC AllCourseByNodeCursorParam @crs OUTPUT, @Post_Major_Node
											 
												FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
												WHILE (@@FETCH_STATUS <> -1)
												BEGIN -- [5]												
														-- INSERT COURSE FROM NODE																			
														INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
																	(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
																VALUES
																	(@RegistrationSession, @StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@Post_Major_Node ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated, @C2_PostMajorLevel) 
															 
													FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
												END; -- [5]
																		 
												CLOSE @crs;
												DEALLOCATE @crs;
								END -- [4]					

							--#@# Condition 2 (Student major define & major node not declare)
								IF((@Major1Node IS NOT NULL OR @Major1Node > 0)  AND (@IsMajorNode = 0 OR @IsMajorNode IS NULL))
									BEGIN -- [6]
										IF(@IsMinorNode = 1) -- If Minor node is declare
											BEGIN -- [7]
												-- Load Minor Nodes
												delete from @tblMinorNodeList
												INSERT INTO	@tblMinorNodeList SELECT * FROM   dbo.GetMinorNodeListForStudent(@Pre_Major_Node, @StudentId, @Post_MajorNode_Level);
												--SELECT * FROM   dbo.GetMinorNodeListForStudent(31, 3081, 1);

												DECLARE @i INT, @Node INT;

												SET @i = 1;

													WHILE @i <= (SELECT max(ID) FROM @tblMinorNodeList )
													BEGIN -- [8]
														SELECT  @Node = ChildNode FROM @tblMinorNodeList WHERE ID = @i;														
														 
														EXEC AllCourseByNodeCursorParam @crs OUTPUT, @Node

														FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
														WHILE (@@FETCH_STATUS <> -1)
														BEGIN -- [9]												
															-- INSERT COURSE FROM NODE																			
															INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
																		(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
																	VALUES
																		(@RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@Node ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated,@C2_PostMajorLevel) 
															 
															FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
														END; -- [9]
																	 
														CLOSE @crs;
														DEALLOCATE @crs;

														SET @i = @i + 1;
													END -- [8]
											END -- [7]
											ELSE
												BEGIN -- [10]
													--Get course list by Pre-Major-Node 
													EXEC AllCourseByNodeCursorParam @crs OUTPUT, @Pre_Major_Node

													FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
													WHILE (@@FETCH_STATUS <> -1)
													BEGIN -- [11]												
														-- INSERT COURSE FROM NODE																			
														INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
																	(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
																VALUES
																	(@RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@Pre_Major_Node ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated,@C2_PostMajorLevel) 
															 
														FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
													END; -- [11]
															 
													CLOSE @crs;
													DEALLOCATE @crs;
												END -- [10]
									END -- [6]
								

							--#@# Condition 3
								IF((@Major1Node IS NULL OR @Major1Node = 0 )AND (@IsMajorNode = 0 OR @IsMajorNode IS NULL))
								BEGIN -- [12]
									IF(@IsMinorNode = 0 OR @IsMinorNode IS NULL) -- If Minor node is declare
									BEGIN -- [13]
										EXEC AllCourseByNodeCursorParam @crs OUTPUT, @Pre_Major_Node

										FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
										WHILE (@@FETCH_STATUS <> -1)
										BEGIN -- [14]												
											-- INSERT COURSE FROM NODE																			
											INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
														(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
														VALUES
														( @RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@Pre_Major_Node ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated,@C2_PostMajorLevel) 
															 
											FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
										END; -- [14]
																												 
										CLOSE @crs;
										DEALLOCATE @crs;
									END -- [13]
								END -- [12]
							END -- [3]
								ELSE
									BEGIN -- [15]
										INSERT into [dbo].[UIUEMS_CC_RegistrationWorksheet]
													(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID] ,[TreeMasterID] ,[CourseID] ,[VersionID] ,[Credits] ,[Node_CourseID] ,[NodeID] ,[NodeLinkName] ,[Priority] ,[IsMajorRelated] ,[ProgramID] ,[DeptID] ,[AcademicCalenderID] ,[IsAutoOpen] ,[IsAutoAssign],[Isrequisitioned] ,[IsMandatory] ,[IsManualOpen] ,[CreatedDate] ,[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
												VALUES
													( @RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID ,@TreeMasterID ,@c2_CourseID ,@c2_VersionID ,@c2_Credits ,@c2_Node_CourseID  ,@c2_NodeID ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated  ,@c2_OfferedByProgramID ,@DepartmentID ,@AcademicCalenderID  ,'False' ,'False'  ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated,@C2_PostMajorLevel) 
									END -- [15]
								
								FETCH NEXT FROM @Cursor_Cal_Course_Prog_Node INTO
										@c2_CalCorProgNodeID , @c2_TreeCalendarDetailID  , @c2_OfferedByProgramID , @c2_CourseID  , @c2_VersionID , @c2_Node_CourseID , @c2_NodeID , @c2_NodeLinkName , @c2_Priority , @c2_Credits , @c2_IsMajorRelated, @c2_IsMinorRelated, @C2_PostMajorLevel   		
				END -- [2]							
						 
					--# CURSOR Cal_Course_Prog_Node
					FETCH NEXT FROM @Cursor_TreeCalendarDetail INTO @c1_TreeCalendarDetailID, @c1_TreeCalendarMasterID 
		END -- [1]
		--# CURSOR TreeCalendarDetail
	
	CLOSE @Cursor_TreeCalendarDetail
	DEALLOCATE @Cursor_TreeCalendarDetail
	
--STEP 1 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 5 START [Update Offered Course status]


UPDATE   UIUEMS_CC_RegistrationWorksheet
SET 	IsOfferedCourse = Oc.isActive
		FROM	UIUEMS_CC_RegistrationWorksheet AS r inner join  UIUEMS_CC_OfferedCourse AS Oc ON Oc.CourseID=r.CourseID and Oc.VersionID=r.VersionID		
		WHERE	r.StudentID=@StudentID and  
				Oc.ProgramID = @ProgramID and 
				Oc.AcademicCalenderID=(SELECT AcademicCalenderID FROM UIUEMS_CC_AcademicCalender WHERE IsActiveRegistration=1)



--STEP 5 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 2 START [update Student Result History]

declare 
@s2_StudentId int,  
@s2_CourseId int, 
@s2_VersionId int, 
@s2_RetakeNo int,  
@s2_ObtainedGPA numeric(18,2), 
@s2_ObtainedGrade varchar(150), 
@s2_CourseStatusID int,
@s2_CourseResultAcaCalID int,

@s2r_Priority int,
@s2r_Id int,
@s2r_StudentID int,
@s2r_CourseId int,
@s2r_VersionId int,

@crsStatusPassedT int,
@crsStatusPassedNt int, 

@lowestPriorityCoursePk INT, 
@courseCount int,
@lowestPriority int;


 set @crsStatusPassedT  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pt')
 set @crsStatusPassedNt = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pn')

	-- load students all course from course history 
	DECLARE  @Cursor_Student_Result_History CURSOR 
	SET @Cursor_Student_Result_History = CURSOR FAST_FORWARD  FOR
		SELECT    
			StudentId,  
			CourseId, 
			VersionId, 
			RetakeNo,  
			ObtainedGPA, 
			ObtainedGrade, 
			CourseStatusID,
			AcaCalID 
		FROM         UIUEMS_CC_Student_CourseHistory
		WHERE     (StudentID = @StudentId)  and (IsConsiderGPA = 1 or CourseStatusID = 7 or CourseStatusID = 10 or CourseStatusID = 11 or CourseStatusID = 9)
		
		OPEN @Cursor_Student_Result_History
		fetch NEXT from @Cursor_Student_Result_History into
					@s2_StudentId,  
					@s2_CourseId, 
					@s2_VersionId, 
					@s2_RetakeNo,  
					@s2_ObtainedGPA, 
					@s2_ObtainedGrade, 
					@s2_CourseStatusID,
					@s2_CourseResultAcaCalID  
				 WHILE (@@FETCH_STATUS <> -1)
					BEGIN 

					SET @courseCount = (SELECT count(*) FROM UIUEMS_CC_RegistrationWorksheet 
										WHERE CourseId= @s2_CourseId and 
											  VersionId = @s2_VersionId and 
											  StudentID = @s2_StudentId  )

					IF(@courseCount = 1)
						BEGIN
							UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
							SET
							 [RetakeNo] = @s2_RetakeNo
							,[ObtainedGPA] = @s2_ObtainedGPA
							,[ObtainedGrade] =   @s2_ObtainedGrade   
							,[CourseStatusId] = @s2_CourseStatusID
							,[CourseResultAcaCalID] = @s2_CourseResultAcaCalID
							,[IsCompleted] =	CASE @s2_CourseStatusID
													WHEN @crsStatusPassedT  THEN 1
													WHEN @crsStatusPassedNt THEN 1
													ELSE 0 	END
							WHERE CourseID= @s2_CourseId and 
								  VersionID = @s2_VersionId and 
								  StudentID = @s2_StudentId 
						END
					ELSE
						BEGIN
							-- set Lowest priority
								set @lowestPriority = (select MIN(priority) from UIUEMS_CC_RegistrationWorksheet 
												where CourseId= @s2_CourseId and 
													  VersionId = @s2_VersionId and 
													  StudentID = @s2_StudentId and 
													  CourseStatusId  is null )
							-- #

							-- update coursestatus for lowest priority course
								UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
								SET
								 [RetakeNo] = @s2_RetakeNo
								,[ObtainedGPA] = @s2_ObtainedGPA
								,[ObtainedGrade] =   @s2_ObtainedGrade   
								,[CourseStatusId] = @s2_CourseStatusID
								,[CourseResultAcaCalID] = @s2_CourseResultAcaCalID
								,[IsCompleted] =	CASE @s2_CourseStatusID
														WHEN @crsStatusPassedT  THEN 1
														WHEN @crsStatusPassedNt THEN 1
														ELSE 0 	END
								WHERE CourseID= @s2_CourseId and 
								VersionID = @s2_VersionId and 
								Priority = @lowestPriority and 
								StudentID = @s2_StudentId  
							-- #

							-- set lowest priority course PK
								SET  @lowestPriorityCoursePk  = (SELECT ID FROM UIUEMS_CC_RegistrationWorksheet 
																WHERE CourseID= @s2_CourseId and 
																VersionID = @s2_VersionId and 
																Priority = @lowestPriority and 
																StudentID = @s2_StudentId )
							-- #

							-- Update all course in lowest priority with -1 except the acting course
								UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
								SET							   
								[CourseStatusId] = -1							
								WHERE 									 
									Priority = @lowestPriority and 
									ID != @lowestPriorityCoursePk and 
									StudentID = @s2_StudentId
							--#

							-- Update other matched course with -1
								UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
								SET							   
								[CourseStatusId] = -1							
								WHERE 
									CourseID= @s2_CourseId and 
									VersionID = @s2_VersionId and 								
									ID != @lowestPriorityCoursePk and 
									StudentID = @s2_StudentId 
							--#
						END
						
					fetch NEXT from @Cursor_Student_Result_History into
					@s2_StudentId,  
					@s2_CourseId, 
					@s2_VersionId, 
					@s2_RetakeNo,  
					@s2_ObtainedGPA, 
					@s2_ObtainedGrade, 
					@s2_CourseStatusID,
					@s2_CourseResultAcaCalID 
				END
	CLOSE @Cursor_Student_Result_History
	DEALLOCATE @Cursor_Student_Result_History


--STEP 2 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 3 START [Update course Name and title ]


UPDATE  [UIUEMS_CC_RegistrationWorksheet]
SET 	FormalCode = c.FormalCode,  
		VersionCode = c.VersionCode, 
		CourseTitle	= c.Title , 
		Credits = c.Credits,
		IsCreditCourse = c.IsCreditCourse 
		FROM UIUEMS_CC_RegistrationWorksheet AS r INNER JOIN UIUEMS_CC_Course AS c ON r.CourseID= c.CourseID and r.VersionID = c.VersionID

WHERE r.StudentID=@StudentID


--STEP 3 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 4 START [Update CalendarDetailName ]

Update	UIUEMS_CC_RegistrationWorksheet 
	Set UIUEMS_CC_RegistrationWorksheet.CalendarDetailName = UIUEMS_CC_CalenderUnitDistribution.Name
	From UIUEMS_CC_RegistrationWorksheet, UIUEMS_CC_TreeCalendarDetail, UIUEMS_CC_CalenderUnitDistribution
	Where UIUEMS_CC_RegistrationWorksheet.TreeCalendarDetailID =  UIUEMS_CC_TreeCalendarDetail.TreeCalendarDetailID
		AND  UIUEMS_CC_TreeCalendarDetail.CalendarDetailID = UIUEMS_CC_CalenderUnitDistribution.CalenderUnitDistributionID

--STEP 4 END
----------------------------------------------------------------------
-- Update Pre Course History



DECLARE @NonCreditCourse TABLE(
pk int identity (1,1),
ID    int,
CourseId int, 
VersionId int,
IsCreditCourse bit 
)

INSERT INTO  @NonCreditCourse 
(ID ,CourseId , VersionId , IsCreditCourse) 
(SELECT id, CourseID,VersionID, IsCreditCourse FROM UIUEMS_CC_RegistrationWorksheet WHERE IsCreditCourse=0  and StudentID=@StudentId)


DECLARE @index INT, @courseId INT , @versionId INT , @id INT;
SET @index = 1;

WHILE @index <= (SELECT max(pk) FROM @NonCreditCourse )

BEGIN 

	SELECT @courseId = courseId , @versionId = versionId , @id = ID FROM @NonCreditCourse WHERE pk = @index;
	IF not EXISTS( SELECT * FROM UIUEMS_CC_StudentPreCourse 
			 WHERE PreCourseId = @courseId and PreVersionId = @versionId)
	BEGIN
			delete from UIUEMS_CC_RegistrationWorksheet where CourseID = @courseId and VersionID = @versionId
	END

	SET @index = @index + 1;
END

----------------------------------------------------------------------


--Return result
-- error checking not complete

set @ReturnValue = 1;
--return @ReturnValue;
--
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGeneratePerStudentBBA]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGeneratePerStudentBBA] 
@StudentId int,
@TreeCalendarMasterID int,
@TreeMasterID int,
@AcademicCalenderID int,
@ProgramID int,
@DepartmentID int, 
@ReturnValue int OUTPUT


AS
BEGIN
SET NOCOUNT ON;

DELETE FROM UIUEMS_CC_RegistrationWorksheet WHERE StudentId = @StudentId;

DECLARE @Pre_Major_Node int,@Post_Major_Node int,@Post_MajorNode_Level int,@IsMajorNode bit,@IsMinorNode bit,@Major1Node int, @RegistrationSession int, @ParentMajorNode int,
--* For CURSOR TreeCalendarDetail c1
@c1_TreeCalendarDetailID int,	@c1_TreeCalendarMasterID int,
--* For CURSOR Cal_Course_Prog_Node c2
@c2_CalCorProgNodeID int, @c2_TreeCalendarDetailID int,	@c2_OfferedByProgramID int,	@c2_CourseID int, @c2_VersionID int, @c2_Node_CourseID int,	@c2_NodeID int,	@c2_NodeLinkName varchar(100),	@c2_Priority int,	@c2_Credits decimal(18,2),	@c2_IsMajorRelated bit, @c2_IsMinorRelated bit,	@C2_PostMajorLevel int,
-- 
@c3_CourseID int ,	@c3_VersionID int,	@c3_Node_CourseID int;

DECLARE @tblMinorNodeList			TABLE	(ID INT IDENTITY(1,1), ParentNode INT, ChildNode INT);
DECLARE @tblStudentMajorCourseList	TABLE	(ID INT IDENTITY(1,1), CourseID int, VersionID int,	NodeCourseID int);
DECLARE @tblVirtualCourseList		TABLE	(ID INT IDENTITY(1,1), CourseID int, VersionID int,	NodeCourseID int);
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 1 START : Generate course list from tree with priority of calender.

DECLARE		@Cursor_TreeCalendarDetail CURSOR, @Cursor_Cal_Course_Prog_Node CURSOR, @Cursor_Node CURSOR, @crs  CURSOR


set @RegistrationSession = (select AcademicCalenderID from UIUEMS_CC_AcademicCalender where IsActiveRegistration=1);

	--* CURSOR TreeCalendarDetail
	SET @Cursor_TreeCalendarDetail = CURSOR FAST_FORWARD FOR
		SELECT TreeCalendarDetailID,TreeCalendarMasterID 
		FROM dbo.UIUEMS_CC_TreeCalendarDetail 
		WHERE TreeCalendarMasterID = @TreeCalendarMasterID and TreeMasterID = @TreeMasterID

	OPEN @Cursor_TreeCalendarDetail
		FETCH NEXT FROM @Cursor_TreeCalendarDetail INTO @c1_TreeCalendarDetailID, @c1_TreeCalendarMasterID  
				
		WHILE @@FETCH_STATUS = 0
		BEGIN -- [1]
			    	--* CURSOR Cal_Course_Prog_Node			
					SET @Cursor_Cal_Course_Prog_Node = CURSOR FAST_FORWARD  FOR
					SELECT CalCorProgNodeID, TreeCalendarDetailID, OfferedByProgramID, CourseID, VersionID, Node_CourseID, NodeID, NodeLinkName,  [Priority],  Credits, IsMajorRelated, IsMinorRelated, PostMajorLevel
					FROM dbo.UIUEMS_CC_Cal_Course_Prog_Node
					WHERE TreeCalendarDetailID  = @c1_TreeCalendarDetailID			
					
					 OPEN @Cursor_Cal_Course_Prog_Node
						FETCH NEXT FROM @Cursor_Cal_Course_Prog_Node INTO
									@c2_CalCorProgNodeID, @c2_TreeCalendarDetailID, @c2_OfferedByProgramID,@c2_CourseID, @c2_VersionID , @c2_Node_CourseID , @c2_NodeID , @c2_NodeLinkName ,@c2_Priority , @c2_Credits , @c2_IsMajorRelated, @c2_IsMinorRelated, @C2_PostMajorLevel  						
						
						WHILE @@FETCH_STATUS = 0
						BEGIN -- [2]

							IF(@c2_CourseID IS NULL OR @c2_CourseID = 0) -- this if-condition Means Node Is available.
							BEGIN -- [3]

								--#@# Condition for major node is true
								IF(@c2_IsMajorRelated = 1)
								BEGIN -- [4]

									-- set Student Major Node.
									SELECT @Major1Node = (SELECT Major1NodeID FROM UIUEMS_ER_Student WHERE StudentID = @StudentId)										
									if(@Major1Node is not null or @Major1Node != 0)	
									BEGIN -- [5]
											-- Get all course from student-major-Node and insert
											EXEC AllCourseByNodeCursorParam @crs OUTPUT, @Major1Node
											delete from @tblStudentMajorCourseList
											 
												FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
												WHILE (@@FETCH_STATUS <> -1)
												BEGIN -- [6]												
														-- INSERT COURSE FROM NODE																			
														INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
																	(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
																VALUES
																	(@RegistrationSession, @StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@Major1Node ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated, @C2_PostMajorLevel) 

														--Insert temp value in @tblStudentMajorCourseList table
														INSERT INTO @tblStudentMajorCourseList 
																	(CourseID  , VersionID  ,	NodeCourseID  )
																	VALUES (@c3_CourseID, @c3_VersionID, @c3_Node_CourseID )
															 
													FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
												END -- [6]
																		 
													CLOSE @crs;
													DEALLOCATE @crs;
									END -- [5]
								END -- [4]					

							--#@# Condition Is virtual node
								Else IF((select IsVirtual from UIUEMS_CC_Node where NodeID = @c2_NodeID) = 1)
									BEGIN -- [7]
									
												--Load student major course
												SELECT @Major1Node = (SELECT Major1NodeID FROM UIUEMS_ER_Student WHERE StudentID = @StudentId)										
												IF(@Major1Node is not null or @Major1Node != 0)	
													BEGIN -- [5]
															-- Get all course from student-major-Node and insert
															EXEC AllCourseByNodeCursorParam @crs OUTPUT, @Major1Node
															delete from @tblStudentMajorCourseList
											 
																FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
																WHILE (@@FETCH_STATUS <> -1)
																BEGIN -- [6]												
																	
																		--Insert temp value in @tblStudentMajorCourseList table
																		INSERT INTO @tblStudentMajorCourseList 
																					(CourseID  , VersionID  ,	NodeCourseID  )
																					VALUES (@c3_CourseID, @c3_VersionID, @c3_Node_CourseID )
															 
																	FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
																END -- [6]
																		 
																	CLOSE @crs;
																	DEALLOCATE @crs;
													END -- [5]
												ELSE
												BEGIN
													select @ParentMajorNode = (select distinct( ParentNodeID) from UIUEMS_CC_TreeDetail as td INNER JOIN UIUEMS_CC_Node as n 
																			on n.NodeID= td.ChildNodeID where n.IsMajor = 1 AND td.TreeMasterID = @TreeMasterID)
													IF(@ParentMajorNode is not null or @ParentMajorNode != 0)	
													BEGIN -- [5]
															-- Get all course from student-major-Node and insert
															EXEC AllCourseByNodeCursorParam @crs OUTPUT, @ParentMajorNode
															delete from @tblStudentMajorCourseList
											 
																FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
																WHILE (@@FETCH_STATUS <> -1)
																BEGIN -- [6]												
																	
																		--Insert temp value in @tblStudentMajorCourseList table
																		INSERT INTO @tblStudentMajorCourseList 
																					(CourseID  , VersionID  ,	NodeCourseID  )
																					VALUES (@c3_CourseID, @c3_VersionID, @c3_Node_CourseID )
															 
																	FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
																END -- [6]
																		 
																	CLOSE @crs;
																	DEALLOCATE @crs;
													END -- [5]
																				
												END


												EXEC AllCourseByNodeCursorParam @crs OUTPUT, @c2_NodeID
												delete from @tblVirtualCourseList
											 
												FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
												WHILE (@@FETCH_STATUS <> -1)
												BEGIN -- [9]												
														
														--Insert temp value in @tblStudentMajorCourseList table
														IF NOT EXISTS( select * from @tblVirtualCourseList where CourseID = @c3_CourseID AND VersionID = @c3_VersionID )
														BEGIN
															INSERT INTO @tblVirtualCourseList (CourseID  , VersionID  ,	NodeCourseID  )
																		VALUES (@c3_CourseID, @c3_VersionID, @c3_Node_CourseID )
														END	 
													FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
												END -- [9]
																		 
												CLOSE @crs;
												DEALLOCATE @crs;


												SET @crs = CURSOR FAST_FORWARD FOR
												SELECT vn.CourseID, vn.VersionID, vn.NodeCourseID FROM @tblVirtualCourseList AS vn 
												LEFT JOIN @tblStudentMajorCourseList AS mn ON vn.CourseID= mn.CourseID and vn.VersionID=mn.VersionID
												WHERE  mn.CourseID is null and mn.VersionID is null
												--SELECT CourseID, VersionID, NodeCourseID
												--FROM @tblVirtualCourseList 
												--WHERE NodeCourseID not in(select NodeCourseID from @tblStudentMajorCourseList)

												OPEN @crs
												FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
												WHILE (@@FETCH_STATUS <> -1)
												BEGIN -- [10]												
														-- INSERT COURSE FROM NODE																			
														INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
																	(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
																VALUES
																	( @RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@c2_NodeID ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated, @C2_PostMajorLevel) 
																																 
													FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
												END -- [10]
												CLOSE @crs;
												DEALLOCATE @crs;

											--END -- [8]											
								    END -- [7]
									ELSE
									BEGIN -- [11]
										--Get course list by Pre-Major-Node 
										EXEC AllCourseByNodeCursorParam @crs OUTPUT, @c2_NodeID

										FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID 
										WHILE (@@FETCH_STATUS <> -1)
										BEGIN -- [12]												
											-- INSERT COURSE FROM NODE																			
											INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
														(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID]	,[TreeMasterID],[CourseID],[VersionID],[Credits],[Node_CourseID],[NodeID],[NodeLinkName],[Priority],[IsMajorRelated],[ProgramID],[DeptID],[AcademicCalenderID],[IsAutoOpen],[IsAutoAssign],[Isrequisitioned],[IsMandatory],[IsManualOpen],[CreatedDate],[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
													VALUES
														( @RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID,@TreeMasterID,@c3_CourseID ,@c3_VersionID  ,@c2_Credits ,@c3_Node_CourseID ,@c2_NodeID ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated ,@c2_OfferedByProgramID  ,@DepartmentID ,@AcademicCalenderID ,'False' ,'False' ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated,@C2_PostMajorLevel) 
															 
											FETCH NEXT FROM @crs INTO @c3_CourseID, @c3_VersionID, @c3_Node_CourseID
										END -- [12]
															 
										CLOSE @crs;
										DEALLOCATE @crs;
									END -- [11]
							END -- [3]
								ELSE
									BEGIN -- [13]
										INSERT into [dbo].[UIUEMS_CC_RegistrationWorksheet]
													(OriginalCalID, [StudentID] ,[CalCourseProgNodeID] ,[TreeCalendarDetailID] ,[TreeCalendarMasterID] ,[TreeMasterID] ,[CourseID] ,[VersionID] ,[Credits] ,[Node_CourseID] ,[NodeID] ,[NodeLinkName] ,[Priority] ,[IsMajorRelated] ,[ProgramID] ,[DeptID] ,[AcademicCalenderID] ,[IsAutoOpen] ,[IsAutoAssign],[Isrequisitioned] ,[IsMandatory] ,[IsManualOpen] ,[CreatedDate] ,[ModifiedDate],[IsMinorRelated],[PostMajorNodeLevel])
												VALUES
													( @RegistrationSession,@StudentId, @c2_CalCorProgNodeID ,@c2_TreeCalendarDetailID ,@c1_TreeCalendarMasterID ,@TreeMasterID ,@c2_CourseID ,@c2_VersionID ,@c2_Credits ,@c2_Node_CourseID  ,@c2_NodeID ,@c2_NodeLinkName ,@c2_Priority ,@c2_IsMajorRelated  ,@c2_OfferedByProgramID ,@DepartmentID ,@AcademicCalenderID  ,'False' ,'False'  ,'False' ,'False' ,'False' ,GETDATE() ,GETDATE(),@c2_IsMinorRelated,@C2_PostMajorLevel) 
									END -- [13]
								
								FETCH NEXT FROM @Cursor_Cal_Course_Prog_Node INTO
										@c2_CalCorProgNodeID , @c2_TreeCalendarDetailID  , @c2_OfferedByProgramID , @c2_CourseID  , @c2_VersionID , @c2_Node_CourseID , @c2_NodeID , @c2_NodeLinkName , @c2_Priority , @c2_Credits , @c2_IsMajorRelated, @c2_IsMinorRelated, @C2_PostMajorLevel   		
				    END -- [2]							
						 
					--# CURSOR Cal_Course_Prog_Node
					FETCH NEXT FROM @Cursor_TreeCalendarDetail INTO @c1_TreeCalendarDetailID, @c1_TreeCalendarMasterID 
		END -- [1]
		--# CURSOR TreeCalendarDetail
	
	CLOSE @Cursor_TreeCalendarDetail
	DEALLOCATE @Cursor_TreeCalendarDetail
	
--STEP 1 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 5 START [Update Offered Course status]


UPDATE   UIUEMS_CC_RegistrationWorksheet
SET 	IsOfferedCourse = Oc.isActive
		FROM	UIUEMS_CC_RegistrationWorksheet AS r inner join  UIUEMS_CC_OfferedCourse AS Oc ON Oc.CourseID=r.CourseID and Oc.VersionID=r.VersionID		
		WHERE	r.StudentID=@StudentID and  
				Oc.ProgramID = @ProgramID and 
				Oc.AcademicCalenderID=(SELECT AcademicCalenderID FROM UIUEMS_CC_AcademicCalender WHERE IsActiveRegistration=1)

--STEP 5 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 2 START [update Student Result History]

declare 
@s2_StudentId int,  
@s2_CourseId int, 
@s2_VersionId int, 
@s2_RetakeNo int,  
@s2_ObtainedGPA numeric(18,2), 
@s2_ObtainedGrade varchar(150), 
@s2_CourseStatusID int,
@s2_CourseResultAcaCalID int,

@s2r_Priority int,
@s2r_Id int,
@s2r_StudentID int,
@s2r_CourseId int,
@s2r_VersionId int,

@crsStatusPassedT int,
@crsStatusPassedNt int, 

@lowestPriorityCoursePk INT, 
@courseCount int,
@lowestPriority int;


 set @crsStatusPassedT  = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pt')
 set @crsStatusPassedNt = (select CourseStatusID from UIUEMS_ER_CourseStatus where Code='Pn')

	-- load students all course from course history 
	DECLARE  @Cursor_Student_Result_History CURSOR 
	SET @Cursor_Student_Result_History = CURSOR FAST_FORWARD  FOR
		SELECT    
			StudentId,  
			CourseId, 
			VersionId, 
			RetakeNo,  
			ObtainedGPA, 
			ObtainedGrade, 
			CourseStatusID,
			AcaCalID 
		FROM         UIUEMS_CC_Student_CourseHistory
		WHERE     (StudentID = @StudentId)  and (IsConsiderGPA = 1 or CourseStatusID = 7 or CourseStatusID = 10 or CourseStatusID = 11 or CourseStatusID = 9)
		
		OPEN @Cursor_Student_Result_History
		fetch NEXT from @Cursor_Student_Result_History into
					@s2_StudentId,  
					@s2_CourseId, 
					@s2_VersionId, 
					@s2_RetakeNo,  
					@s2_ObtainedGPA, 
					@s2_ObtainedGrade, 
					@s2_CourseStatusID,
					@s2_CourseResultAcaCalID  
				 WHILE (@@FETCH_STATUS <> -1)
					BEGIN 

					SET @courseCount = (SELECT count(*) FROM UIUEMS_CC_RegistrationWorksheet 
										WHERE CourseId= @s2_CourseId and 
											  VersionId = @s2_VersionId and 
											  StudentID = @s2_StudentId  )

					IF(@courseCount = 1)
						BEGIN
							UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
							SET
							 [RetakeNo] = @s2_RetakeNo
							,[ObtainedGPA] = @s2_ObtainedGPA
							,[ObtainedGrade] =   @s2_ObtainedGrade   
							,[CourseStatusId] = @s2_CourseStatusID
							,[CourseResultAcaCalID] = @s2_CourseResultAcaCalID
							,[IsCompleted] =	CASE @s2_CourseStatusID
													WHEN @crsStatusPassedT  THEN 1
													WHEN @crsStatusPassedNt THEN 1
													ELSE 0 	END
							WHERE CourseID= @s2_CourseId and 
								  VersionID = @s2_VersionId and 
								  StudentID = @s2_StudentId 
						END
					ELSE
						BEGIN
							-- set Lowest priority
								set @lowestPriority = (select MIN(priority) from UIUEMS_CC_RegistrationWorksheet 
												where CourseId= @s2_CourseId and 
													  VersionId = @s2_VersionId and 
													  StudentID = @s2_StudentId and 
													  CourseStatusId  is null )
							-- #

							-- update coursestatus for lowest priority course
								UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
								SET
								 [RetakeNo] = @s2_RetakeNo
								,[ObtainedGPA] = @s2_ObtainedGPA
								,[ObtainedGrade] =   @s2_ObtainedGrade   
								,[CourseStatusId] = @s2_CourseStatusID
								,[CourseResultAcaCalID] = @s2_CourseResultAcaCalID
								,[IsCompleted] =	CASE @s2_CourseStatusID
														WHEN @crsStatusPassedT  THEN 1
														WHEN @crsStatusPassedNt THEN 1
														ELSE 0 	END
								WHERE CourseID= @s2_CourseId and 
								VersionID = @s2_VersionId and 
								Priority = @lowestPriority and 
								StudentID = @s2_StudentId  
							-- #

							-- set lowest priority course PK
								SET  @lowestPriorityCoursePk  = (SELECT ID FROM UIUEMS_CC_RegistrationWorksheet 
																WHERE CourseID= @s2_CourseId and 
																VersionID = @s2_VersionId and 
																Priority = @lowestPriority and 
																StudentID = @s2_StudentId )
							-- #

							-- Update all course in lowest priority with -1 except the acting course
								UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
								SET							   
								[CourseStatusId] = -1							
								WHERE 									 
									Priority = @lowestPriority and 
									ID != @lowestPriorityCoursePk and 
									StudentID = @s2_StudentId
							--#

							-- Update other matched course with -1
								UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
								SET							   
								[CourseStatusId] = -1							
								WHERE 
									CourseID= @s2_CourseId and 
									VersionID = @s2_VersionId and 								
									ID != @lowestPriorityCoursePk and 
									StudentID = @s2_StudentId 
							--#
						END
						
					fetch NEXT from @Cursor_Student_Result_History into
					@s2_StudentId,  
					@s2_CourseId, 
					@s2_VersionId, 
					@s2_RetakeNo,  
					@s2_ObtainedGPA, 
					@s2_ObtainedGrade, 
					@s2_CourseStatusID,
					@s2_CourseResultAcaCalID 
				END
	CLOSE @Cursor_Student_Result_History
	DEALLOCATE @Cursor_Student_Result_History


--STEP 2 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 3 START [Update course Name and title ]


UPDATE  [UIUEMS_CC_RegistrationWorksheet]
SET 	FormalCode = c.FormalCode,  
		VersionCode = c.VersionCode, 
		CourseTitle	= c.Title , 
		Credits = c.Credits,
		IsCreditCourse = c.IsCreditCourse 
		FROM UIUEMS_CC_RegistrationWorksheet AS r INNER JOIN UIUEMS_CC_Course AS c ON r.CourseID= c.CourseID and r.VersionID = c.VersionID

WHERE r.StudentID=@StudentID


--STEP 3 END
----------------------------------------------------------------------
----------------------------------------------------------------------
--STEP 4 START [Update CalendarDetailName ]

Update	UIUEMS_CC_RegistrationWorksheet 
	Set UIUEMS_CC_RegistrationWorksheet.CalendarDetailName = UIUEMS_CC_CalenderUnitDistribution.Name
	From UIUEMS_CC_RegistrationWorksheet, UIUEMS_CC_TreeCalendarDetail, UIUEMS_CC_CalenderUnitDistribution
	Where UIUEMS_CC_RegistrationWorksheet.TreeCalendarDetailID =  UIUEMS_CC_TreeCalendarDetail.TreeCalendarDetailID
		AND  UIUEMS_CC_TreeCalendarDetail.CalendarDetailID = UIUEMS_CC_CalenderUnitDistribution.CalenderUnitDistributionID

--STEP 4 END
----------------------------------------------------------------------
-- Update Pre Course History



DECLARE @NonCreditCourse TABLE(
pk int identity (1,1),
ID    int,
CourseId int, 
VersionId int,
IsCreditCourse bit 
)

INSERT INTO  @NonCreditCourse 
(ID ,CourseId , VersionId , IsCreditCourse) 
(SELECT id, CourseID,VersionID, IsCreditCourse FROM UIUEMS_CC_RegistrationWorksheet WHERE IsCreditCourse=0  and StudentID=@StudentId)


DECLARE @index INT, @courseId INT , @versionId INT , @id INT;
SET @index = 1;

WHILE @index <= (SELECT max(pk) FROM @NonCreditCourse )

BEGIN 

	SELECT @courseId = courseId , @versionId = versionId , @id = ID FROM @NonCreditCourse WHERE pk = @index;
	IF not EXISTS( SELECT * FROM UIUEMS_CC_StudentPreCourse 
			 WHERE PreCourseId = @courseId and PreVersionId = @versionId)
	BEGIN
			delete from UIUEMS_CC_RegistrationWorksheet where CourseID = @courseId and VersionID = @versionId
	END

	SET @index = @index + 1;
END

----------------------------------------------------------------------


--Return result
-- error checking not complete

set @ReturnValue = 1;
--return @ReturnValue;
--
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAll]
as
Begin
SELECT *
  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  end





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllAutoAssignCourseByStudentID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllAutoAssignCourseByStudentID]
@StudentID int = null
AS
BEGIN
SELECT *
  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  WHERE StudentID = @StudentID and IsAutoAssign = 1
  END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByAcaCalIdProgramId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByAcaCalIdProgramId]
(
	@acaCalId Int = NULL,
	@programId Int = NULL
)

As
Begin
	Select *   From [dbo].[UIUEMS_CC_RegistrationWorksheet]
	Where ProgramID = @programId and AcademicCalenderID = @acaCalId; 
End


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByAcaProgCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByAcaProgCourse]
(
	@acaCalId Int = NULL,
	@programId Int = NULL,
	@courseList nvarchar(max) = NULL
)

As
Begin
	Declare @query nvarchar(1000);
	Set @query = 'Select Distinct a.* from UIUEMS_CC_RegistrationWorksheet a, UIUEMS_CC_RegistrationWorksheet b Where a.IsAutoAssign = 1 and b.IsAutoAssign = 1 and a.StudentID = b.StudentID and a.OriginalCalID =' + Cast(@acaCalId as nvarchar(50))  + ' and a.ProgramID = ' + Cast(@programId as nvarchar(50)) + ' and b.OriginalCalID = ' + Cast(@acaCalId as nvarchar(50)) + ' and b.ProgramID = ' + Cast(@programId as nvarchar(50)) + ' and ';
	Set @query = @query + @courseList;
	print(@query);
	Exec sp_executesql @query;
End






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByProgramId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByProgramId]
@ProgramId int = null
AS
BEGIN
SELECT *   FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  WHERE ProgramID = @ProgramId 
  END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByStdProgCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByStdProgCal]
(
	@StudentID Int = NULL,
	@ProgramID Int = NULL,
	@AcademicCalenderID Int = NULL
)

As
Begin
	If @StudentID != 0 and @ProgramID != 0 and @AcademicCalenderID != 0
	Begin
		Select r.* , s.Roll, (p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName) as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and s.Roll = @StudentID and r.ProgramID = @ProgramID and r.AcademicCalenderID = @AcademicCalenderID;
	End
	Else If @StudentID != 0 and @ProgramID != 0 and @AcademicCalenderID = 0
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and s.Roll = @StudentID and r.ProgramID = @ProgramID;
	End
	Else If @StudentID != 0 and @ProgramID = 0 and @AcademicCalenderID != 0
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and s.Roll = @StudentID and r.AcademicCalenderID = @AcademicCalenderID;
	End
	Else If @StudentID != 0 and @ProgramID = 0 and @AcademicCalenderID = 0
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and s.Roll = @StudentID;
	End
	Else If @StudentID = 0 and @ProgramID != 0 and @AcademicCalenderID != 0
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and s.Roll = @StudentID and r.ProgramID = @ProgramID and r.AcademicCalenderID = @AcademicCalenderID;
	End
	Else If @StudentID = 0 and @ProgramID != 0 and @AcademicCalenderID = 0
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and s.Roll = @StudentID and r.ProgramID = @ProgramID;
	End
	Else If @StudentID = 0 and @ProgramID = 0 and @AcademicCalenderID != 0
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID and  r.AcademicCalenderID = @AcademicCalenderID;
	End
	Else
	Begin
		Select r.* , s.Roll, p.FirstName + ' ' + p.MiddleName + ' ' + p.LastName as FullName
		From UIUEMS_CC_RegistrationWorksheet r, UIUEMS_ER_Student s, UIUEMS_ER_Person p
		Where r.StudentID = s.StudentID and s.PersonID = p.PersonID;
	End
End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByStudentID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllByStudentID]
@StudentID int = null
AS
BEGIN
SELECT *
  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  WHERE StudentID = @StudentID 
  END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllOpenCourseByStudentID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllOpenCourseByStudentID]
@StudentID int = null
AS
BEGIN
SELECT *
  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  WHERE StudentID = @StudentID and IsAutoOpen = 1
  END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllRegistrationSessionDataByStudentID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetAllRegistrationSessionDataByStudentID]
@StudentID int = null
AS
BEGIN
SELECT *
  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  WHERE StudentID = @StudentID and OriginalCalID = (select AcademicCalenderID from UIUEMS_CC_AcademicCalender where IsActiveRegistration=1)
  END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetById]
@ID int = null
AS
BEGIN
  SELECT *
  FROM [dbo].[UIUEMS_CC_RegistrationWorksheet]
  WHERE ID = @ID
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetPreAdvisingByProgCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetPreAdvisingByProgCal]
(
	@ProgramID Int = NULL,
	@AcademicCalenderID Int = NULL
)

As
Begin
	If @ProgramID != 0 and @AcademicCalenderID != 0
	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsAutoAssign='True' and r.ProgramID=@ProgramID  and r.AcademicCalenderID = @AcademicCalenderID
		) as a
	where Rn=1;
	Else if @ProgramID!=0 and @AcademicCalenderID=0
	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsAutoAssign='True' and r.ProgramID=@ProgramID
		) as a
	where Rn=1;
	Else if @ProgramID=0 and @AcademicCalenderID!=0
	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsAutoAssign='True' and r.AcademicCalenderID=@AcademicCalenderID
		) as a
	where Rn=1;
	Else
	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsAutoAssign='True'
		) as a
	where Rn=1;

End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetPreRegisteredByCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetPreRegisteredByCourse]
(
	@CourseID Int = NULL,
	@VersionID Int = NULL
)

As
Begin
	If @CourseID != 0 and @VersionID != 0
	Begin

	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.*
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsAutoAssign='True' and R.SectionName is not null and r.CourseID=@courseID and r.VersionID=@VersionID
		) as a
	where Rn=1;
	End
End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetPreRegisteredByProgCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetPreRegisteredByProgCal]
(
	@ProgramID Int = NULL,
	@AcademicCalenderID Int = NULL
)

As
Begin
	If @ProgramID != 0 and @AcademicCalenderID != 0
	Begin

	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsAutoAssign='True' and R.SectionName is not null and r.ProgramID = @ProgramID and r.AcademicCalenderID = @AcademicCalenderID
		) as a
	where Rn=1;
	End
End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetReqByCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetReqByCourse]
(
	@CourseID Int = NULL,
	@VersionID Int = NULL
)

As
Begin
	If @CourseID != 0 and @VersionID != 0
	Begin

	Select * from
		(
			Select Row_Number() Over (Partition by r.StudentID Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsRequisitioned='True' and  r.CourseID=@courseID and r.VersionID=@VersionID 
		) as a
	where Rn=1;
	End
End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetGetReqByProgCal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetGetReqByProgCal]
(
	@ProgramID Int = NULL,
	@AcademicCalenderID Int = NULL
)

As
Begin
	If @ProgramID != 0 and @AcademicCalenderID != 0
	Begin
		Select * from
		(
			Select Row_Number() Over (Partition by r.FormalCode Order By r.StudentID Desc) as Rn,
			r.* 
			From UIUEMS_CC_RegistrationWorksheet r
			Where r.IsRequisitioned='True' and r.ProgramID = @ProgramID and r.AcademicCalenderID = @AcademicCalenderID
		) as a
		where Rn=1;
	End
End





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetInsert]
(
@ID int output
,@StudentID int = NULL
,@CalCourseProgNodeID int = NULL
,@IsCompleted bit = NULL
,@OriginalCalID int = NULL
,@IsAutoAssign bit = NULL
,@IsAutoOpen bit = NULL
,@Isrequisitioned bit = NULL
,@IsMandatory bit = NULL
,@IsManualOpen bit = NULL
,@TreeCalendarDetailID int = NULL
,@TreeCalendarMasterID int = NULL
,@TreeMasterID int = NULL
,@CalendarMasterName varchar(250) = NULL
,@CalendarDetailName varchar(250) = NULL
,@CourseID int = NULL
,@VersionID int = NULL
,@Credits decimal(18,2) = NULL
,@Node_CourseID int = NULL
,@NodeID int = NULL
,@IsMinorRelated bit = NULL
,@IsMajorRelated bit = NULL
,@NodeLinkName varchar(250) = NULL
,@FormalCode varchar(250) = NULL
,@VersionCode varchar(250) = NULL
,@CourseTitle varchar(250) = NULL
,@AcaCal_SectionID int = NULL
,@SectionName varchar(200) = NULL
,@ProgramID int = NULL
,@DeptID int = NULL
,@Priority int = NULL
,@RetakeNo int = NULL
,@ObtainedGPA numeric(18,2) = NULL
,@ObtainedGrade varchar(150) = NULL
,@AcademicCalenderID int = NULL
,@AcaCalYear int = NULL
,@BatchCode varchar(50) = NULL
,@AcaCalTypeName varchar(50) = NULL
,@CalCrsProgNdCredits decimal(18,2) = NULL
,@CalCrsProgNdIsMajorRelated bit = NULL
,@IsMultipleACUSpan bit = NULL
,@CompletedCredit numeric(18,2) = NULL
,@CourseCredit numeric(18,2) = NULL
,@CreatedBy int = NULL
,@CreatedDate datetime = NULL
,@ModifiedBy int = NULL
,@ModifiedDate datetime = NULL
,@CourseStatusId int = NULL
,@IsRegistered bit = NULL
,@IsAdd bit = NULL
,@ConflictedCourse  nvarchar(1000) = null
,@SequenceNo INT = null
,@IsOfferedCourse BIT
,@CourseResultAcaCalID  INT
,@PostMajorNodeLevel INT
,@IsCreditCourse BIT )

AS
BEGIN
SET NOCOUNT OFF;


INSERT INTO [dbo].[UIUEMS_CC_RegistrationWorksheet]
           ([StudentID]
           ,[CalCourseProgNodeID]
           ,[IsCompleted]
           ,[OriginalCalID]
           ,[IsAutoAssign]
           ,[IsAutoOpen]
           ,[IsRequisitioned]
           ,[IsMandatory]
           ,[IsManualOpen]
           ,[TreeCalendarDetailID]
           ,[TreeCalendarMasterID]
           ,[TreeMasterID]
           ,[CalendarMasterName]
           ,[CalendarDetailName]
           ,[CourseID]
           ,[VersionID]
           ,[Credits]
           ,[Node_CourseID]
           ,[NodeID]
           ,[IsMinorRelated]
           ,[IsMajorRelated]
           ,[NodeLinkName]
           ,[FormalCode]
           ,[VersionCode]
           ,[CourseTitle]
           ,[AcaCal_SectionID]
           ,[SectionName]
           ,[ProgramID]
           ,[DeptID]
           ,[Priority]
           ,[RetakeNo]
           ,[ObtainedGPA]
           ,[ObtainedGrade]
           ,[AcademicCalenderID]
           ,[AcaCalYear]
           ,[BatchCode]
           ,[AcaCalTypeName]
           ,[CalCrsProgNdCredits]
           ,[CalCrsProgNdIsMajorRelated]
           ,[IsMultipleACUSpan]
           ,[CompletedCredit]
           ,[CourseCredit]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CourseStatusId]
           ,[IsRegistered]
           ,[IsAdd]
           ,[ConflictedCourse]
           ,[SequenceNo]
           ,[IsOfferedCourse]
           ,[CourseResultAcaCalID]
           ,[PostMajorNodeLevel]
           ,[IsCreditCourse])
     VALUES
           (@StudentID 
           ,@CalCourseProgNodeID 
           ,@IsCompleted 
           ,@OriginalCalID 
           ,@IsAutoAssign 
           ,@IsAutoOpen 
           ,@IsRequisitioned 
           ,@IsMandatory 
           ,@IsManualOpen 
           ,@TreeCalendarDetailID 
           ,@TreeCalendarMasterID 
           ,@TreeMasterID 
           ,@CalendarMasterName 
           ,@CalendarDetailName 
           ,@CourseID 
           ,@VersionID 
           ,@Credits 
           ,@Node_CourseID 
           ,@NodeID 
           ,@IsMinorRelated 
           ,@IsMajorRelated 
           ,@NodeLinkName 
           ,@FormalCode 
           ,@VersionCode 
           ,@CourseTitle 
           ,@AcaCal_SectionID 
           ,@SectionName 
           ,@ProgramID 
           ,@DeptID 
           ,@Priority 
           ,@RetakeNo 
           ,@ObtainedGPA 
           ,@ObtainedGrade 
           ,@AcademicCalenderID 
           ,@AcaCalYear 
           ,@BatchCode 
           ,@AcaCalTypeName 
           ,@CalCrsProgNdCredits 
           ,@CalCrsProgNdIsMajorRelated 
           ,@IsMultipleACUSpan 
           ,@CompletedCredit 
           ,@CourseCredit 
           ,@CreatedBy 
           ,@CreatedDate 
           ,@ModifiedBy 
           ,@ModifiedDate 
           ,@CourseStatusId 
           ,@IsRegistered 
           ,@IsAdd 
           ,@ConflictedCourse 
           ,@SequenceNo 
           ,@IsOfferedCourse 
           ,@CourseResultAcaCalID 
           ,@PostMajorNodeLevel 
           ,@IsCreditCourse )
      
 SET @ID = SCOPE_IDENTITY()

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetRegisterCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetRegisterCourse] 

@ID int,
@CreatedBy int,
@CourseStatusID int

AS
BEGIN
SET NOCOUNT ON;

DECLARE @RowCount int

--IF EXISTS(select * from UIUEMS_CC_Student_CourseHistory where StudentID = @StudentID and
--														      AcaCalSectionID = @AcaCalSectionID )
--BEGIN
--	DELETE FROM UIUEMS_CC_Student_CourseHistory   WHERE StudentID= @StuId
--END

INSERT INTO UIUEMS_CC_Student_CourseHistory
           (StudentID
           ,CalCourseProgNodeID
           ,AcaCalSectionID
           ,RetakeNo
--		   ,ObtainedTotalMarks
           ,ObtainedGPA
           ,ObtainedGrade
		   ,CourseStatusID
		   ,CourseStatusDate
           ,AcaCalID
           ,CourseID
           ,VersionID
           ,Node_CourseID
           ,NodeID
           ,IsMultipleACUSpan
           ,CourseCredit
           ,CompletedCredit		   
           ,CreatedBy
           ,CreatedDate
           )     
    (SELECT 
		StudentID
		,CalCourseProgNodeID
		,AcaCal_SectionID
		,RetakeNo
		,ObtainedGPA
		,ObtainedGrade
		,@CourseStatusID
		,getdate()
		,AcademicCalenderID
		,CourseID
		,VersionID
		,Node_CourseID
		,NodeID
		,IsMultipleACUSpan
		,CourseCredit
		,CompletedCredit		
		,@CreatedBy
		,getdate()
	FROM UIUEMS_CC_RegistrationWorksheet 
	where ID = @ID )
	
	UPDATE UIUEMS_CC_RegistrationWorksheet set CourseStatusId = @CourseStatusId where ID = @ID
		
		--and AcademicCalenderID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1))

SELECT @RowCount = @@ROWCOUNT
SELECT @RowCount AS 'count'

END






















GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetUpdate]
(
@ID int  = NULL
,@StudentID int = NULL
,@CalCourseProgNodeID int = NULL
,@IsCompleted bit = NULL
,@OriginalCalID int = NULL
,@IsAutoAssign bit = NULL
,@IsAutoOpen bit = NULL
,@Isrequisitioned bit = NULL
,@IsMandatory bit = NULL
,@IsManualOpen bit = NULL
,@TreeCalendarDetailID int = NULL
,@TreeCalendarMasterID int = NULL
,@TreeMasterID int = NULL
,@CalendarMasterName varchar(250) = NULL
,@CalendarDetailName varchar(250) = NULL
,@CourseID int = NULL
,@VersionID int = NULL
,@Credits decimal(18,2) = NULL
,@Node_CourseID int = NULL
,@NodeID int = NULL
,@IsMajorRelated bit = NULL
,@NodeLinkName varchar(250) = NULL
,@FormalCode varchar(250) = NULL
,@VersionCode varchar(250) = NULL
,@CourseTitle varchar(250) = NULL
,@AcaCal_SectionID int = NULL
,@SectionName varchar(200) = NULL
,@ProgramID int = NULL
,@DeptID int = NULL
,@Priority int = NULL
,@RetakeNo int = NULL
,@ObtainedGPA numeric(18,2) = NULL
,@ObtainedGrade varchar(150) = NULL
,@AcademicCalenderID int = NULL
,@AcaCalYear int = NULL
,@BatchCode varchar(50) = NULL
,@AcaCalTypeName varchar(50) = NULL
,@CalCrsProgNdCredits decimal(18,2) = NULL
,@CalCrsProgNdIsMajorRelated bit = NULL
,@IsMultipleACUSpan bit = NULL
,@CompletedCredit numeric(18,2) = NULL
,@CourseCredit numeric(18,2) = NULL
,@CreatedBy int = NULL
,@CreatedDate datetime = NULL
,@ModifiedBy int = NULL
,@ModifiedDate datetime = NULL
,@CourseStatusId int = NULL
,@IsRegistered bit = NULL
,@IsAdd bit = NULL
,@ConflictedCourse  nvarchar(1000) = null
,@SequenceNo INT = null
,@CourseResultAcaCalID int
,@IsOfferedCourse bit
,@PostMajorNodeLevel int
,@IsCreditCourse bit)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [dbo].[UIUEMS_CC_RegistrationWorksheet]
   SET [StudentID] = @StudentID
      ,[CalCourseProgNodeID] = @CalCourseProgNodeID
      ,[IsCompleted] = @IsCompleted
      ,[OriginalCalID] = @OriginalCalID
      ,[IsAutoAssign] = @IsAutoAssign
      ,[IsAutoOpen] = @IsAutoOpen
      ,[Isrequisitioned] = @Isrequisitioned
      ,[IsMandatory] = @IsMandatory
      ,[IsManualOpen] = @IsManualOpen
      ,[TreeCalendarDetailID] = @TreeCalendarDetailID
      ,[TreeCalendarMasterID] = @TreeCalendarMasterID
      ,[TreeMasterID] = @TreeMasterID
      ,[CalendarMasterName] = @CalendarMasterName
      ,[CalendarDetailName] = @CalendarDetailName
      ,[CourseID] = @CourseID
      ,[VersionID] = @VersionID
      ,[Credits] = @Credits
      ,[Node_CourseID] = @Node_CourseID
      ,[NodeID] = @NodeID
      ,[IsMajorRelated] = @IsMajorRelated
      ,[NodeLinkName] = @NodeLinkName
      ,[FormalCode] = @FormalCode
      ,[VersionCode] = @VersionCode
      ,[CourseTitle] = @CourseTitle
      ,[AcaCal_SectionID] = @AcaCal_SectionID
      ,[SectionName] = @SectionName
      ,[ProgramID] = @ProgramID
      ,[DeptID] = @DeptID
      ,[Priority] = @Priority
      ,[RetakeNo] = @RetakeNo
      ,[ObtainedGPA] = @ObtainedGPA
      ,[ObtainedGrade] = @ObtainedGrade
      ,[AcademicCalenderID] = @AcademicCalenderID
      ,[AcaCalYear] = @AcaCalYear
      ,[BatchCode] = @BatchCode
      ,[AcaCalTypeName] = @AcaCalTypeName
      ,[CalCrsProgNdCredits] = @CalCrsProgNdCredits
      ,[CalCrsProgNdIsMajorRelated] = @CalCrsProgNdIsMajorRelated
      ,[IsMultipleACUSpan] = @IsMultipleACUSpan
      ,[CompletedCredit] = @CompletedCredit
      ,[CourseCredit] = @CourseCredit
      ,[CreatedBy] = @CreatedBy
      ,[CreatedDate] = @CreatedDate
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedDate] = @ModifiedDate
      ,[CourseStatusId] = @CourseStatusId
      ,[IsRegistered] = @IsRegistered
      ,[IsAdd] = @IsAdd
      ,[ConflictedCourse] = @ConflictedCourse
	  ,SequenceNo = @SequenceNo
		,CourseResultAcaCalID = @CourseResultAcaCalID
		,IsOfferedCourse = @IsOfferedCourse 
		,PostMajorNodeLevel = @PostMajorNodeLevel
		,IsCreditCourse = @IsCreditCourse 
      
 WHERE ID =@ID



  end





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourse]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourse]
(
@ID int  = NULL
,@StudentID int = NULL
,@CalCourseProgNodeID int = NULL
,@IsCompleted bit = NULL
,@OriginalCalID int = NULL
,@IsAutoAssign bit = NULL
,@IsAutoOpen bit = NULL
,@Isrequisitioned bit = NULL
,@IsMandatory bit = NULL
,@IsManualOpen bit = NULL
,@TreeCalendarDetailID int = NULL
,@TreeCalendarMasterID int = NULL
,@TreeMasterID int = NULL
,@CalendarMasterName varchar(250) = NULL
,@CalendarDetailName varchar(250) = NULL
,@CourseID int = NULL
,@VersionID int = NULL
,@Credits decimal(18,2) = NULL
,@Node_CourseID int = NULL
,@NodeID int = NULL
,@IsMajorRelated bit = NULL
,@NodeLinkName varchar(250) = NULL
,@FormalCode varchar(250) = NULL
,@VersionCode varchar(250) = NULL
,@CourseTitle varchar(250) = NULL
,@AcaCal_SectionID int = NULL
,@SectionName varchar(20) = NULL
,@ProgramID int = NULL
,@DeptID int = NULL
,@Priority int = NULL
,@RetakeNo int = NULL
,@ObtainedGPA numeric(18,2) = NULL
,@ObtainedGrade varchar(150) = NULL
,@AcademicCalenderID int = NULL
,@AcaCalYear int = NULL
,@BatchCode varchar(50) = NULL
,@AcaCalTypeName varchar(50) = NULL
,@CalCrsProgNdCredits decimal(18,2) = NULL
,@CalCrsProgNdIsMajorRelated bit = NULL
,@IsMultipleACUSpan bit = NULL
,@CompletedCredit numeric(18,2) = NULL
,@CourseCredit numeric(18,2) = NULL
,@CreatedBy int = NULL
,@CreatedDate datetime = NULL
,@ModifiedBy int = NULL
,@ModifiedDate datetime = NULL
,@CourseStatusId int = NULL
,@IsRegistered bit = NULL
,@IsAdd bit = NULL
,@ConflictedCourse  nvarchar(1000) = null
,@SequenceNo INT = null
,@CourseResultAcaCalID int)

AS
BEGIN
SET NOCOUNT OFF;

-- IF (@IsAutoAssign = 1)
--	 BEGIN
--		SET @CourseStatusId = -2;
--	 END
--ELSE
--	BEGIN
--		SET @CourseStatusId = NULL;
--	 END
SET @CourseStatusId = 0;
update UIUEMS_CC_RegistrationWorksheet 
  set CourseStatusId=0
  where CourseStatusId is null
UPDATE [dbo].[UIUEMS_CC_RegistrationWorksheet]
   SET [StudentID] = @StudentID
      ,[CalCourseProgNodeID] = @CalCourseProgNodeID
      ,[IsCompleted] = @IsCompleted
      ,[OriginalCalID] = @OriginalCalID
      ,[IsAutoAssign] = @IsAutoAssign
      ,[IsAutoOpen] = @IsAutoOpen
      ,[Isrequisitioned] = @Isrequisitioned
      ,[IsMandatory] = @IsMandatory
      ,[IsManualOpen] = @IsManualOpen
      ,[TreeCalendarDetailID] = @TreeCalendarDetailID
      ,[TreeCalendarMasterID] = @TreeCalendarMasterID
      ,[TreeMasterID] = @TreeMasterID
      ,[CalendarMasterName] = @CalendarMasterName
      ,[CalendarDetailName] = @CalendarDetailName
      ,[CourseID] = @CourseID
      ,[VersionID] = @VersionID
      ,[Credits] = @Credits
      ,[Node_CourseID] = @Node_CourseID
      ,[NodeID] = @NodeID
      ,[IsMajorRelated] = @IsMajorRelated
      ,[NodeLinkName] = @NodeLinkName
      ,[FormalCode] = @FormalCode
      ,[VersionCode] = @VersionCode
      ,[CourseTitle] = @CourseTitle
      ,[AcaCal_SectionID] = @AcaCal_SectionID
      ,[SectionName] = @SectionName
      ,[ProgramID] = @ProgramID
      ,[DeptID] = @DeptID
      ,[Priority] = @Priority
      ,[RetakeNo] = @RetakeNo
      ,[ObtainedGPA] = @ObtainedGPA
      ,[ObtainedGrade] = @ObtainedGrade
      ,[AcademicCalenderID] = @AcademicCalenderID
      ,[AcaCalYear] = @AcaCalYear
      ,[BatchCode] = @BatchCode
      ,[AcaCalTypeName] = @AcaCalTypeName
      ,[CalCrsProgNdCredits] = @CalCrsProgNdCredits
      ,[CalCrsProgNdIsMajorRelated] = @CalCrsProgNdIsMajorRelated
      ,[IsMultipleACUSpan] = @IsMultipleACUSpan
      ,[CompletedCredit] = @CompletedCredit
      ,[CourseCredit] = @CourseCredit
      ,[CreatedBy] = @CreatedBy
      ,[CreatedDate] = @CreatedDate
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedDate] = @ModifiedDate
      ,[CourseStatusId] = @CourseStatusId
      ,[IsRegistered] = @IsRegistered
      ,[IsAdd] = @IsAdd
      ,[ConflictedCourse] = @ConflictedCourse
   ,SequenceNo = @SequenceNo
   ,CourseResultAcaCalID = @CourseResultAcaCalID
      
 WHERE ID =@ID


	 -- if [IsAutoAssign] is TRUE
	 if (@IsAutoAssign = 1)
	 begin

	 -- Update OfferedCourse table, Increase 1 for Occupied 
	 EXEC UIUEMS_CC_OfferedCourseIncreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 


	 -- [IsAutoOpen] <-- 0 and [CourseStatusId] <-- -2
	 -- for similar [NodeID] and [Priority] of current @ID

		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  [CourseStatusId] = 0 , IsAutoOpen = 0       
		  WHERE           
		  Priority = @Priority and 
		  NodeID = @NodeID  and
		  ID != @ID and 
		  StudentID = @StudentID and 
		  CourseStatusId = 0 and 
		--  CourseStatusId != -1 and 
		  IsOfferedCourse=1


		 -- [IsAutoOpen] <-- 0 and [CourseStatusId] <-- -2
		 -- for similar [CourseID] and [VersionCode] of current @ID
		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  IsAutoOpen = 0        
		  WHERE 
		  CourseID= @CourseID and 
		  VersionID = @VersionID and         
		  ID != @ID and 
		  StudentID = @StudentID and 
		  CourseStatusId = 0 and
		  IsAutoOpen = 1 and 
		  --CourseStatusId != -1 and 
		  IsOfferedCourse=1
	 end

	 -- if [IsAutoAssign] is FALSE
	 if (@IsAutoAssign = 0)
	 begin

	  -- Update OfferedCourse table, Decrease 1 for Occupied 
	 EXEC UIUEMS_CC_OfferedCourseDecreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 

	 -- [IsAutoOpen] <-- 1 and [CourseStatusId] <-- null
	 -- for similar [NodeID] and [Priority] of current @ID


		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  CourseStatusId = 0 , IsAutoOpen = 1       
		  WHERE           
		  Priority = @Priority and 
		  NodeID = @NodeID  and
		  ID != @ID and 
		  StudentID = @StudentID and 
		  IsOfferedCourse=1 and
		  CourseStatusId !=-1 and
		  FormalCode not in -- also check that this course is not autoopened under any other semester
			(select FormalCode from  UIUEMS_CC_RegistrationWorksheet where StudentID=@StudentID and NodeID =@NodeID and IsAutoOpen=1)

		 -- [IsAutoOpen] <-- 1 and [CourseStatusId] <-- null
		 -- for similar [CourseID] and [VersionCode] of current @ID
		  --UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  --SET          
		  --CourseStatusId = NULL , IsAutoOpen = 1        
		  --WHERE 
		  --CourseID= @CourseID and 
		  --VersionID = @VersionID and         
		  --ID != @ID and 
		  --StudentID = @StudentID  and 
		  --CourseStatusId = -3 and 
		  --IsOfferedCourse=1
	 end
  end


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourse_NOT_IN_USED]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourse_NOT_IN_USED]
(
@ID int  = NULL
,@StudentID int = NULL
,@CalCourseProgNodeID int = NULL
,@IsCompleted bit = NULL
,@OriginalCalID int = NULL
,@IsAutoAssign bit = NULL
,@IsAutoOpen bit = NULL
,@Isrequisitioned bit = NULL
,@IsMandatory bit = NULL
,@IsManualOpen bit = NULL
,@TreeCalendarDetailID int = NULL
,@TreeCalendarMasterID int = NULL
,@TreeMasterID int = NULL
,@CalendarMasterName varchar(250) = NULL
,@CalendarDetailName varchar(250) = NULL
,@CourseID int = NULL
,@VersionID int = NULL
,@Credits decimal(18,2) = NULL
,@Node_CourseID int = NULL
,@NodeID int = NULL
,@IsMajorRelated bit = NULL
,@NodeLinkName varchar(250) = NULL
,@FormalCode varchar(250) = NULL
,@VersionCode varchar(250) = NULL
,@CourseTitle varchar(250) = NULL
,@AcaCal_SectionID int = NULL
,@SectionName varchar(20) = NULL
,@ProgramID int = NULL
,@DeptID int = NULL
,@Priority int = NULL
,@RetakeNo int = NULL
,@ObtainedGPA numeric(18,2) = NULL
,@ObtainedGrade varchar(150) = NULL
,@AcademicCalenderID int = NULL
,@AcaCalYear int = NULL
,@BatchCode varchar(50) = NULL
,@AcaCalTypeName varchar(50) = NULL
,@CalCrsProgNdCredits decimal(18,2) = NULL
,@CalCrsProgNdIsMajorRelated bit = NULL
,@IsMultipleACUSpan bit = NULL
,@CompletedCredit numeric(18,2) = NULL
,@CourseCredit numeric(18,2) = NULL
,@CreatedBy int = NULL
,@CreatedDate datetime = NULL
,@ModifiedBy int = NULL
,@ModifiedDate datetime = NULL
,@CourseStatusId int = NULL
,@IsRegistered bit = NULL
,@IsAdd bit = NULL
,@ConflictedCourse  nvarchar(1000) = null
,@SequenceNo INT = null)

AS
BEGIN
SET NOCOUNT OFF;

 IF (@IsAutoAssign = 1)
	 BEGIN
		SET @CourseStatusId = -2;
	 END
ELSE
	BEGIN
		SET @CourseStatusId = NULL;
	 END

UPDATE [dbo].[UIUEMS_CC_RegistrationWorksheet]
   SET [StudentID] = @StudentID
      ,[CalCourseProgNodeID] = @CalCourseProgNodeID
      ,[IsCompleted] = @IsCompleted
      ,[OriginalCalID] = @OriginalCalID
      ,[IsAutoAssign] = @IsAutoAssign
      ,[IsAutoOpen] = @IsAutoOpen
      ,[Isrequisitioned] = @Isrequisitioned
      ,[IsMandatory] = @IsMandatory
      ,[IsManualOpen] = @IsManualOpen
      ,[TreeCalendarDetailID] = @TreeCalendarDetailID
      ,[TreeCalendarMasterID] = @TreeCalendarMasterID
      ,[TreeMasterID] = @TreeMasterID
      ,[CalendarMasterName] = @CalendarMasterName
      ,[CalendarDetailName] = @CalendarDetailName
      ,[CourseID] = @CourseID
      ,[VersionID] = @VersionID
      ,[Credits] = @Credits
      ,[Node_CourseID] = @Node_CourseID
      ,[NodeID] = @NodeID
      ,[IsMajorRelated] = @IsMajorRelated
      ,[NodeLinkName] = @NodeLinkName
      ,[FormalCode] = @FormalCode
      ,[VersionCode] = @VersionCode
      ,[CourseTitle] = @CourseTitle
      ,[AcaCal_SectionID] = @AcaCal_SectionID
      ,[SectionName] = @SectionName
      ,[ProgramID] = @ProgramID
      ,[DeptID] = @DeptID
      ,[Priority] = @Priority
      ,[RetakeNo] = @RetakeNo
      ,[ObtainedGPA] = @ObtainedGPA
      ,[ObtainedGrade] = @ObtainedGrade
      ,[AcademicCalenderID] = @AcademicCalenderID
      ,[AcaCalYear] = @AcaCalYear
      ,[BatchCode] = @BatchCode
      ,[AcaCalTypeName] = @AcaCalTypeName
      ,[CalCrsProgNdCredits] = @CalCrsProgNdCredits
      ,[CalCrsProgNdIsMajorRelated] = @CalCrsProgNdIsMajorRelated
      ,[IsMultipleACUSpan] = @IsMultipleACUSpan
      ,[CompletedCredit] = @CompletedCredit
      ,[CourseCredit] = @CourseCredit
      ,[CreatedBy] = @CreatedBy
      ,[CreatedDate] = @CreatedDate
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedDate] = @ModifiedDate
      ,[CourseStatusId] = @CourseStatusId
      ,[IsRegistered] = @IsRegistered
      ,[IsAdd] = @IsAdd
      ,[ConflictedCourse] = @ConflictedCourse
   ,SequenceNo = @SequenceNo
      
 WHERE ID =@ID


	 -- if [IsAutoAssign] is TRUE
	 if (@IsAutoAssign = 1)
	 begin

	 -- Update OfferedCourse table, Increase 1 for Occupied 
	 EXEC UIUEMS_CC_OfferedCourseIncreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 


	 -- [IsAutoOpen] <-- 0 and [CourseStatusId] <-- -2
	 -- for similar [NodeID] and [Priority] of current @ID

		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  [CourseStatusId] = -2 , IsAutoOpen = 0       
		  WHERE           
		  Priority = @Priority and 
		  NodeID = @NodeID  and
		  ID != @ID and 
		  StudentID = @StudentID and 
		  CourseStatusId IS NULL and 
		  IsOfferedCourse=1


		 -- [IsAutoOpen] <-- 0 and [CourseStatusId] <-- -2
		 -- for similar [CourseID] and [VersionCode] of current @ID
		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  [CourseStatusId] = -3 , IsAutoOpen = 0        
		  WHERE 
		  CourseID= @CourseID and 
		  VersionID = @VersionID and         
		  ID != @ID and 
		  StudentID = @StudentID and 
		  CourseStatusId IS NULL and 
		  IsOfferedCourse=1
	 end

	 -- if [IsAutoAssign] is FALSE
	 if (@IsAutoAssign = 0)
	 begin

	  -- Update OfferedCourse table, Decrease 1 for Occupied 
	 EXEC UIUEMS_CC_OfferedCourseDecreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 

	 -- [IsAutoOpen] <-- 1 and [CourseStatusId] <-- null
	 -- for similar [NodeID] and [Priority] of current @ID


		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  CourseStatusId = NULL , IsAutoOpen = 1       
		  WHERE           
		  Priority = @Priority and 
		  NodeID = @NodeID  and
		  ID != @ID and 
		  StudentID = @StudentID and 
		  CourseStatusId = -2 and 
		  IsOfferedCourse=1


		 -- [IsAutoOpen] <-- 1 and [CourseStatusId] <-- null
		 -- for similar [CourseID] and [VersionCode] of current @ID
		  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
		  SET          
		  CourseStatusId = NULL , IsAutoOpen = 1        
		  WHERE 
		  CourseID= @CourseID and 
		  VersionID = @VersionID and         
		  ID != @ID and 
		  StudentID = @StudentID  and 
		  CourseStatusId = -3 and 
		  IsOfferedCourse=1
	 end
  end


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourseNew]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourseNew]
(
@ID int  = NULL
,@StudentID int = NULL
,@CalCourseProgNodeID int = NULL
,@IsCompleted bit = NULL
,@OriginalCalID int = NULL
,@IsAutoAssign bit = NULL
,@IsAutoOpen bit = NULL
,@Isrequisitioned bit = NULL
,@IsMandatory bit = NULL
,@IsManualOpen bit = NULL
,@TreeCalendarDetailID int = NULL
,@TreeCalendarMasterID int = NULL
,@TreeMasterID int = NULL
,@CalendarMasterName varchar(250) = NULL
,@CalendarDetailName varchar(250) = NULL
,@CourseID int = NULL
,@VersionID int = NULL
,@Credits decimal(18,2) = NULL
,@Node_CourseID int = NULL
,@NodeID int = NULL
,@IsMajorRelated bit = NULL
,@NodeLinkName varchar(250) = NULL
,@FormalCode varchar(250) = NULL
,@VersionCode varchar(250) = NULL
,@CourseTitle varchar(250) = NULL
,@AcaCal_SectionID int = NULL
,@SectionName varchar(200) = NULL
,@ProgramID int = NULL
,@DeptID int = NULL
,@Priority int = NULL
,@RetakeNo int = NULL
,@ObtainedGPA numeric(18,2) = NULL
,@ObtainedGrade varchar(150) = NULL
,@AcademicCalenderID int = NULL
,@AcaCalYear int = NULL
,@BatchCode varchar(50) = NULL
,@AcaCalTypeName varchar(50) = NULL
,@CalCrsProgNdCredits decimal(18,2) = NULL
,@CalCrsProgNdIsMajorRelated bit = NULL
,@IsMultipleACUSpan bit = NULL
,@CompletedCredit numeric(18,2) = NULL
,@CourseCredit numeric(18,2) = NULL
,@CreatedBy int = NULL
,@CreatedDate datetime = NULL
,@ModifiedBy int = NULL
,@ModifiedDate datetime = NULL
,@CourseStatusId int = NULL
,@IsRegistered bit = NULL
,@IsAdd bit = NULL
,@ConflictedCourse  nvarchar(1000) = null
,@SequenceNo INT = null
,@CourseResultAcaCalID int
,@IsOfferedCourse bit
,@PostMajorNodeLevel int
,@IsCreditCourse bit)

AS
BEGIN --1
SET NOCOUNT OFF;

UPDATE [dbo].[UIUEMS_CC_RegistrationWorksheet]
   SET [StudentID] = @StudentID
      ,[CalCourseProgNodeID] = @CalCourseProgNodeID
      ,[IsCompleted] = @IsCompleted
      ,[OriginalCalID] = @OriginalCalID
      ,[IsAutoAssign] = @IsAutoAssign
      ,[IsAutoOpen] = @IsAutoOpen
      ,[Isrequisitioned] = @Isrequisitioned
      ,[IsMandatory] = @IsMandatory
      ,[IsManualOpen] = @IsManualOpen
      ,[TreeCalendarDetailID] = @TreeCalendarDetailID
      ,[TreeCalendarMasterID] = @TreeCalendarMasterID
      ,[TreeMasterID] = @TreeMasterID
      ,[CalendarMasterName] = @CalendarMasterName
      ,[CalendarDetailName] = @CalendarDetailName
      ,[CourseID] = @CourseID
      ,[VersionID] = @VersionID
      ,[Credits] = @Credits
      ,[Node_CourseID] = @Node_CourseID
      ,[NodeID] = @NodeID
      ,[IsMajorRelated] = @IsMajorRelated
      ,[NodeLinkName] = @NodeLinkName
      ,[FormalCode] = @FormalCode
      ,[VersionCode] = @VersionCode
      ,[CourseTitle] = @CourseTitle
      ,[AcaCal_SectionID] = @AcaCal_SectionID
      ,[SectionName] = @SectionName
      ,[ProgramID] = @ProgramID
      ,[DeptID] = @DeptID
      ,[Priority] = @Priority
      ,[RetakeNo] = @RetakeNo
      ,[ObtainedGPA] = @ObtainedGPA
      ,[ObtainedGrade] = @ObtainedGrade
      ,[AcademicCalenderID] = @AcademicCalenderID
      ,[AcaCalYear] = @AcaCalYear
      ,[BatchCode] = @BatchCode
      ,[AcaCalTypeName] = @AcaCalTypeName
      ,[CalCrsProgNdCredits] = @CalCrsProgNdCredits
      ,[CalCrsProgNdIsMajorRelated] = @CalCrsProgNdIsMajorRelated
      ,[IsMultipleACUSpan] = @IsMultipleACUSpan
      ,[CompletedCredit] = @CompletedCredit
      ,[CourseCredit] = @CourseCredit
      ,[CreatedBy] = @CreatedBy
      ,[CreatedDate] = @CreatedDate
      ,[ModifiedBy] = @ModifiedBy
      ,[ModifiedDate] = @ModifiedDate
      ,[CourseStatusId] = @CourseStatusId
      ,[IsRegistered] = @IsRegistered
      ,[IsAdd] = @IsAdd
      ,[ConflictedCourse] = @ConflictedCourse
   ,SequenceNo = @SequenceNo
   ,CourseResultAcaCalID = @CourseResultAcaCalID 
   ,IsOfferedCourse = @IsOfferedCourse 
   ,PostMajorNodeLevel = @PostMajorNodeLevel
   ,IsCreditCourse = @IsCreditCourse    


 WHERE ID =@ID


	 -- if [IsAutoAssign] is TRUE
	 if (@IsAutoAssign = 1)
	 BEGIN --4

		 -- Update OfferedCourse table, Increase 1 for Occupied 
		 EXEC UIUEMS_CC_OfferedCourseIncreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 


		 -- [IsAutoOpen] <-- 0 and [CourseStatusId] <-- -2
		 -- for similar [NodeID] and [Priority] of current @ID

			  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
			  SET          
			  IsAutoOpen = 0       
			  WHERE           
			  Priority = @Priority and 
			 -- NodeID = @NodeID  and
			  ID != @ID and 
			  StudentID = @StudentID and 
			  ISNULL( CourseStatusId,0) = 0 and 			   
			  IsOfferedCourse = 1 
	 END --4



	 -- if [IsAutoAssign] is FALSE
	 IF (@IsAutoAssign = 0)
	 BEGIN --5

		  -- Update OfferedCourse table, Decrease 1 for Occupied 
		 EXEC UIUEMS_CC_OfferedCourseDecreaseOccupied @CourseID, @VersionID, @ProgramID, @TreeMasterID; 

		 -- [IsAutoOpen] <-- 1 and [CourseStatusId] <-- 0
		 -- for similar [NodeID] and [Priority] of current @ID

			  UPDATE  [dbo].[UIUEMS_CC_RegistrationWorksheet]
			  SET          
			   IsAutoOpen = 1       
			  WHERE           
			  Priority = @Priority and 
			 -- NodeID = @NodeID  and
			  ID != @ID and 
			  StudentID = @StudentID and 
			  IsOfferedCourse = 1 and
			   ISNULL( CourseStatusId,0) = 0 
	 END --5

  END --1


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourseRetake]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RegistrationWorksheetUpdateForAssignCourseRetake]
(
@ID int  = NULL
,@StudentID int = NULL
,@CalCourseProgNodeID int = NULL
,@IsCompleted bit = NULL
,@OriginalCalID int = NULL
,@IsAutoAssign bit = NULL
,@IsAutoOpen bit = NULL
,@Isrequisitioned bit = NULL
,@IsMandatory bit = NULL
,@IsManualOpen bit = NULL
,@TreeCalendarDetailID int = NULL
,@TreeCalendarMasterID int = NULL
,@TreeMasterID int = NULL
,@CalendarMasterName varchar(250) = NULL
,@CalendarDetailName varchar(250) = NULL
,@CourseID int = NULL
,@VersionID int = NULL
,@Credits decimal(18,2) = NULL
,@Node_CourseID int = NULL
,@NodeID int = NULL
,@IsMajorRelated bit = NULL
,@NodeLinkName varchar(250) = NULL
,@FormalCode varchar(250) = NULL
,@VersionCode varchar(250) = NULL
,@CourseTitle varchar(250) = NULL
,@AcaCal_SectionID int = NULL
,@SectionName varchar(200) = NULL
,@ProgramID int = NULL
,@DeptID int = NULL
,@Priority int = NULL
,@RetakeNo int = NULL
,@ObtainedGPA numeric(18,2) = NULL
,@ObtainedGrade varchar(150) = NULL
,@AcademicCalenderID int = NULL
,@AcaCalYear int = NULL
,@BatchCode varchar(50) = NULL
,@AcaCalTypeName varchar(50) = NULL
,@CalCrsProgNdCredits decimal(18,2) = NULL
,@CalCrsProgNdIsMajorRelated bit = NULL
,@IsMultipleACUSpan bit = NULL
,@CompletedCredit numeric(18,2) = NULL
,@CourseCredit numeric(18,2) = NULL
,@CreatedBy int = NULL
,@CreatedDate datetime = NULL
,@ModifiedBy int = NULL
,@ModifiedDate datetime = NULL
,@CourseStatusId int = NULL
,@IsRegistered bit = NULL
,@IsAdd bit = NULL
,@ConflictedCourse  nvarchar(1000) = null
,@SequenceNo INT = null
,@CourseResultAcaCalID int
,@IsOfferedCourse bit
,@PostMajorNodeLevel int
,@IsCreditCourse bit)

AS
BEGIN
	SET NOCOUNT OFF;
 
	UPDATE [dbo].[UIUEMS_CC_RegistrationWorksheet]
	SET [StudentID] = @StudentID
	,[CalCourseProgNodeID] = @CalCourseProgNodeID
	,[IsCompleted] = @IsCompleted
	,[OriginalCalID] = @OriginalCalID
	,[IsAutoAssign] = @IsAutoAssign
	,[IsAutoOpen] = @IsAutoOpen
	,[Isrequisitioned] = @Isrequisitioned
	,[IsMandatory] = @IsMandatory
	,[IsManualOpen] = @IsManualOpen
	,[TreeCalendarDetailID] = @TreeCalendarDetailID
	,[TreeCalendarMasterID] = @TreeCalendarMasterID
	,[TreeMasterID] = @TreeMasterID
	,[CalendarMasterName] = @CalendarMasterName
	,[CalendarDetailName] = @CalendarDetailName
	,[CourseID] = @CourseID
	,[VersionID] = @VersionID
	,[Credits] = @Credits
	,[Node_CourseID] = @Node_CourseID
	,[NodeID] = @NodeID
	,[IsMajorRelated] = @IsMajorRelated
	,[NodeLinkName] = @NodeLinkName
	,[FormalCode] = @FormalCode
	,[VersionCode] = @VersionCode
	,[CourseTitle] = @CourseTitle
	,[AcaCal_SectionID] = @AcaCal_SectionID
	,[SectionName] = @SectionName
	,[ProgramID] = @ProgramID
	,[DeptID] = @DeptID
	,[Priority] = @Priority
	,[RetakeNo] = @RetakeNo
	,[ObtainedGPA] = @ObtainedGPA
	,[ObtainedGrade] = @ObtainedGrade
	,[AcademicCalenderID] = @AcademicCalenderID
	,[AcaCalYear] = @AcaCalYear
	,[BatchCode] = @BatchCode
	,[AcaCalTypeName] = @AcaCalTypeName
	,[CalCrsProgNdCredits] = @CalCrsProgNdCredits
	,[CalCrsProgNdIsMajorRelated] = @CalCrsProgNdIsMajorRelated
	,[IsMultipleACUSpan] = @IsMultipleACUSpan
	,[CompletedCredit] = @CompletedCredit
	,[CourseCredit] = @CourseCredit
	,[CreatedBy] = @CreatedBy
	,[CreatedDate] = @CreatedDate
	,[ModifiedBy] = @ModifiedBy
	,[ModifiedDate] = @ModifiedDate
	,[CourseStatusId] = @CourseStatusId
	,[IsRegistered] = @IsRegistered
	,[IsAdd] = @IsAdd
	,[ConflictedCourse] = @ConflictedCourse
	,SequenceNo = @SequenceNo
	,CourseResultAcaCalID = @CourseResultAcaCalID
	,IsOfferedCourse = @IsOfferedCourse 
   ,PostMajorNodeLevel = @PostMajorNodeLevel
   ,IsCreditCourse = @IsCreditCourse    

      
	WHERE ID =@ID
	
  END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomInformation_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomInformation_Insert]
(
	@RoomInfoID int Output,
	@RoomNumber varchar(100) = NULL,
	@RoomName varchar(50) = NULL,
	@RoomFloorNo nvarchar(250) = NULL,
	@RoomTypeID int = NULL,
	@Capacity int = NULL,
	@Campus nvarchar(250) = NULL,
	@AddressID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_CC_RoomInformation]
(
	[RoomNumber],
	[RoomName],
	[RoomFloorNo],
	[RoomTypeID],
	[Capacity],
	[Campus],
	[AddressID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@RoomNumber,
	@RoomName,
	@RoomFloorNo,
	@RoomTypeID,
	@Capacity,
	@Campus,
	@AddressID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @RoomInfoID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomInformationGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomInformationGetAll]

As
Begin
	Select * From UIUEMS_CC_RoomInformation;
End



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomInformationGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
--@Sajib
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomInformationGetById]
(
@RoomInfoID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT
[RoomInfoID],
[RoomNumber],
[RoomName],
[RoomTypeID],
[Capacity],
[AddressID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_RoomInformation
WHERE     (RoomInfoID = @RoomInfoID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_RoomTypeDeleteById]
(
@RoomTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_RoomType]
WHERE RoomTypeID = @RoomTypeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_RoomType


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomTypeGetById]
(
@RoomTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_RoomType
WHERE     (RoomTypeID = @RoomTypeID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomTypeInsert] 
(
	@RoomTypeID int  OUTPUT,
	@TypeName varchar(50)  = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_RoomType]
(
	[RoomTypeID],
	[TypeName],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@RoomTypeID,
	@TypeName,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @RoomTypeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_RoomTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_RoomTypeUpdate]
(
	@RoomTypeID int  = NULL,
	@TypeName varchar(50)  = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_RoomType]
   SET

	[TypeName]	=	@TypeName,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE RoomTypeID = @RoomTypeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_SchoolDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_SchoolDeleteById]
(
@SchoolID int = null
)

AS
BEGIN
SET NOCOUNT ON;

DELETE FROM [UIUEMS_CC_School]
WHERE SchoolID = @SchoolID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_SchoolGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_SchoolGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[SchoolID],
[Name],
[Code],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_School


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_SchoolGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_SchoolGetById]
(
@SchoolID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[SchoolID],
[Name],
[Code],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]


FROM       UIUEMS_CC_School
WHERE     (SchoolID = @SchoolID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_SchoolInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_SchoolInsert] 
(
@SchoolID int   OUTPUT,
@Name varchar(100) = NULL,
@Code varchar(50) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_School]
(
[SchoolID],
[Name],
[Code],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@SchoolID,
@Name,
@Code,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @SchoolID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_SchoolUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_SchoolUpdate]
(
@SchoolID int   = NULL,
@Name varchar(100) = NULL,
@Code varchar(50) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_School]
   SET

[Name]	=	@Name,
[Code]	=	@Code,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE SchoolID = @SchoolID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CalCourseProgNodeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_Student_CalCourseProgNodeDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Student_CalCourseProgNode]
WHERE ID = @ID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CalCourseProgNodeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CalCourseProgNodeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Student_CalCourseProgNode


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CalCourseProgNodeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CalCourseProgNodeGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Student_CalCourseProgNode
WHERE     (ID = @ID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CalCourseProgNodeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CalCourseProgNodeInsert] 
(
	@ID int  OUTPUT,
	@StudentID int  = NULL,
	@CalCourseProgNodeID int  = NULL,
	@IsCompleted bit  = NULL,
	@OriginalCalID int = NULL,
	@IsAutoAssign bit  = NULL,
	@IsAutoOpen bit  = NULL,
	@Isrequisitioned bit  = NULL,
	@IsMandatory bit  = NULL,
	@IsManualOpen bit  = NULL,
	@TreeCalendarDetailID int = NULL,
	@TreeCalendarMasterID int = NULL,
	@TreeMasterID int = NULL,
	@CalendarMasterName varchar(250) = NULL,
	@CalendarDetailName varchar(250) = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Credits decimal(18, 2) = NULL,
	@Node_CourseID int = NULL,
	@NodeID int = NULL,
	@IsMajorRelated bit = NULL,
	@NodeLinkName varchar(250) = NULL,
	@FormalCode varchar(250) = NULL,
	@VersionCode varchar(250) = NULL,
	@CourseTitle varchar(250) = NULL,
	@AcaCal_SectionID int = NULL,
	@SectionName varchar(20) = NULL,
	@ProgramID int = NULL,
	@DeptID int = NULL,
	@Priority int = NULL,
	@RetakeNo int = NULL,
	@ObtainedGPA numeric(18, 2) = NULL,
	@ObtainedGrade varchar(150) = NULL,
	@AcademicCalenderID int = NULL,
	@AcaCalYear int = NULL,
	@BatchCode varchar(50) = NULL,
	@AcaCalTypeName varchar(50) = NULL,
	@CalCrsProgNdCredits decimal(18, 2) = NULL,
	@CalCrsProgNdIsMajorRelated bit = NULL,
	@IsMultipleACUSpan bit = NULL,
	@CompletedCredit numeric(18, 2) = NULL,
	@CourseCredit numeric(18, 2) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Student_CalCourseProgNode]
(
	[ID],
	[StudentID],
	[CalCourseProgNodeID],
	[IsCompleted],
	[OriginalCalID],
	[IsAutoAssign],
	[IsAutoOpen],
	[Isrequisitioned],
	[IsMandatory],
	[IsManualOpen],
	[TreeCalendarDetailID],
	[TreeCalendarMasterID],
	[TreeMasterID],
	[CalendarMasterName],
	[CalendarDetailName],
	[CourseID],
	[VersionID],
	[Credits],
	[Node_CourseID],
	[NodeID],
	[IsMajorRelated],
	[NodeLinkName],
	[FormalCode],
	[VersionCode],
	[CourseTitle],
	[AcaCal_SectionID],
	[SectionName],
	[ProgramID],
	[DeptID],
	[Priority],
	[RetakeNo],
	[ObtainedGPA],
	[ObtainedGrade],
	[AcademicCalenderID],
	[AcaCalYear],
	[BatchCode],
	[AcaCalTypeName],
	[CalCrsProgNdCredits],
	[CalCrsProgNdIsMajorRelated],
	[IsMultipleACUSpan],
	[CompletedCredit],
	[CourseCredit],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@ID,
	@StudentID,
	@CalCourseProgNodeID,
	@IsCompleted,
	@OriginalCalID,
	@IsAutoAssign,
	@IsAutoOpen,
	@Isrequisitioned,
	@IsMandatory,
	@IsManualOpen,
	@TreeCalendarDetailID,
	@TreeCalendarMasterID,
	@TreeMasterID,
	@CalendarMasterName,
	@CalendarDetailName,
	@CourseID,
	@VersionID,
	@Credits,
	@Node_CourseID,
	@NodeID,
	@IsMajorRelated,
	@NodeLinkName,
	@FormalCode,
	@VersionCode,
	@CourseTitle,
	@AcaCal_SectionID,
	@SectionName,
	@ProgramID,
	@DeptID,
	@Priority,
	@RetakeNo,
	@ObtainedGPA,
	@ObtainedGrade,
	@AcademicCalenderID,
	@AcaCalYear,
	@BatchCode,
	@AcaCalTypeName,
	@CalCrsProgNdCredits,
	@CalCrsProgNdIsMajorRelated,
	@IsMultipleACUSpan,
	@CompletedCredit,
	@CourseCredit,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CalCourseProgNodeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CalCourseProgNodeUpdate]
(
	@ID int  = NULL,
	@StudentID int  = NULL,
	@CalCourseProgNodeID int  = NULL,
	@IsCompleted bit  = NULL,
	@OriginalCalID int = NULL,
	@IsAutoAssign bit  = NULL,
	@IsAutoOpen bit  = NULL,
	@Isrequisitioned bit  = NULL,
	@IsMandatory bit  = NULL,
	@IsManualOpen bit  = NULL,
	@TreeCalendarDetailID int = NULL,
	@TreeCalendarMasterID int = NULL,
	@TreeMasterID int = NULL,
	@CalendarMasterName varchar(250) = NULL,
	@CalendarDetailName varchar(250) = NULL,
	@CourseID int = NULL,
	@VersionID int = NULL,
	@Credits decimal(18, 2) = NULL,
	@Node_CourseID int = NULL,
	@NodeID int = NULL,
	@IsMajorRelated bit = NULL,
	@NodeLinkName varchar(250) = NULL,
	@FormalCode varchar(250) = NULL,
	@VersionCode varchar(250) = NULL,
	@CourseTitle varchar(250) = NULL,
	@AcaCal_SectionID int = NULL,
	@SectionName varchar(20) = NULL,
	@ProgramID int = NULL,
	@DeptID int = NULL,
	@Priority int = NULL,
	@RetakeNo int = NULL,
	@ObtainedGPA numeric(18, 2) = NULL,
	@ObtainedGrade varchar(150) = NULL,
	@AcademicCalenderID int = NULL,
	@AcaCalYear int = NULL,
	@BatchCode varchar(50) = NULL,
	@AcaCalTypeName varchar(50) = NULL,
	@CalCrsProgNdCredits decimal(18, 2) = NULL,
	@CalCrsProgNdIsMajorRelated bit = NULL,
	@IsMultipleACUSpan bit = NULL,
	@CompletedCredit numeric(18, 2) = NULL,
	@CourseCredit numeric(18, 2) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_Student_CalCourseProgNode]
   SET


	[StudentID]	=	@StudentID,
	[CalCourseProgNodeID]	=	@CalCourseProgNodeID,
	[IsCompleted]	=	@IsCompleted,
	[OriginalCalID]	=	@OriginalCalID,
	[IsAutoAssign]	=	@IsAutoAssign,
	[IsAutoOpen]	=	@IsAutoOpen,
	[Isrequisitioned]	=	@Isrequisitioned,
	[IsMandatory]	=	@IsMandatory,
	[IsManualOpen]	=	@IsManualOpen,
	[TreeCalendarDetailID]	=	@TreeCalendarDetailID,
	[TreeCalendarMasterID]	=	@TreeCalendarMasterID,
	[TreeMasterID]	=	@TreeMasterID,
	[CalendarMasterName]	=	@CalendarMasterName,
	[CalendarDetailName]	=	@CalendarDetailName,
	[CourseID]	=	@CourseID,
	[VersionID]	=	@VersionID,
	[Credits]	=	@Credits,
	[Node_CourseID]	=	@Node_CourseID,
	[NodeID]	=	@NodeID,
	[IsMajorRelated]	=	@IsMajorRelated,
	[NodeLinkName]	=	@NodeLinkName,
	[FormalCode]	=	@FormalCode,
	[VersionCode]	=	@VersionCode,
	[CourseTitle]	=	@CourseTitle,
	[AcaCal_SectionID]	=	@AcaCal_SectionID,
	[SectionName]	=	@SectionName,
	[ProgramID]	=	@ProgramID,
	[DeptID]	=	@DeptID,
	[Priority]	=	@Priority,
	[RetakeNo]	=	@RetakeNo,
	[ObtainedGPA]	=	@ObtainedGPA,
	[ObtainedGrade]	=	@ObtainedGrade,
	[AcademicCalenderID]	=	@AcademicCalenderID,
	[AcaCalYear]	=	@AcaCalYear,
	[BatchCode]	=	@BatchCode,
	[AcaCalTypeName]	=	@AcaCalTypeName,
	[CalCrsProgNdCredits]	=	@CalCrsProgNdCredits,
	[CalCrsProgNdIsMajorRelated]	=	@CalCrsProgNdIsMajorRelated,
	[IsMultipleACUSpan]	=	@IsMultipleACUSpan,
	[CompletedCredit]	=	@CompletedCredit,
	[CourseCredit]	=	@CourseCredit,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE ID = @ID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_Student_CourseHistoryDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_Student_CourseHistory]
WHERE ID = @ID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<Sajib, Ahmed>								--
--								Create date:	< 2014-02-02 >								--
--								Description:	<Softwar Eng.>								--
--								Action:			<Remove All Column>							--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetAll]


AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_CC_Student_CourseHistory;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByAcaCalId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByAcaCalId]

@AcaCalId int = null

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_CC_Student_CourseHistory

where AcaCalID = @AcaCalId


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByAcaCalSectionId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByAcaCalSectionId]
(
@AcaCalSectionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM       UIUEMS_CC_Student_CourseHistory
WHERE     (AcaCalSectionID = @AcaCalSectionID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByStudentId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByStudentId]

@StudentID int = null

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_CC_Student_CourseHistory

where StudentID = @StudentID


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByStudentIdAcaCalId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<Sajib, Ahmed>								--
--								Create date:	< 2014-02-02 >								--
--								Description:	<Softwar Eng.>								--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByStudentIdAcaCalId]
(
	@StudentID int = null,
	@AcaCalId int = null
)
AS
BEGIN
SET NOCOUNT ON;

Select *
From UIUEMS_CC_Student_CourseHistory
Where StudentID = @StudentID and AcaCalID = @AcaCalId;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByStudentIdCourseIdVersionId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetAllByStudentIdCourseIdVersionId]
(
@StudentID int = null,
@CourseID int = null,
@VersionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Student_CourseHistory
WHERE     (StudentID = @StudentID AND CourseID = @CourseID AND VersionID = @VersionID)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Student_CourseHistory
WHERE     (ID = @ID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetByStudentIdCourseIdVersionIdAcaCalSecId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-09-21 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetByStudentIdCourseIdVersionIdAcaCalSecId]
(
	@StudentID int = null,
	@CourseID int = null,
	@VersionID int = null,
	@AcaCalISecID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Student_CourseHistory
WHERE     (StudentID = @StudentID AND CourseID = @CourseID AND VersionID = @VersionID AND AcaCalSectionID = @AcaCalISecID)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryGetByStudentIdCourseIdVersionIdConsiderGPA]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-09-25 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryGetByStudentIdCourseIdVersionIdConsiderGPA]
(
@StudentID int = null,
@CourseID int = null,
@VersionID int = null,
@IsConsiderGPA bit = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_Student_CourseHistory
WHERE     (StudentID = @StudentID AND CourseID = @CourseID AND VersionID = @VersionID AND IsConsiderGPA = @IsConsiderGPA)

END







GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<Sajib, Ahmed>								--
--								Create date:	< 2014-02-02 >								--
--								Description:	<Softwar Eng.>								--
--								Action:			<Add Remark Column>							--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryInsert] 
(
@ID int   OUTPUT,
@StudentID int  = NULL,
@CalCourseProgNodeID int = NULL,
@AcaCalSectionID int = NULL,
@RetakeNo int = NULL,
@ObtainedTotalMarks numeric(18, 2) = NULL,
@ObtainedGPA numeric(18, 2) = NULL,
@ObtainedGrade varchar(2) = NULL,
@GradeId int = NULL,
@CourseStatusID int = NULL,
@CourseStatusDate datetime = NULL,
@AcaCalID int = NULL,
@CourseID int = NULL,
@VersionID int = NULL,
@CourseCredit numeric(18, 2) = NULL,
@CompletedCredit numeric(18, 2) = NULL,
@Node_CourseID int = NULL,
@NodeID int = NULL,
@IsMultipleACUSpan bit = NULL,
@IsConsiderGPA bit = NULL,
@CourseWavTransfrID int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@Remark nvarchar(max) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_Student_CourseHistory]
(
[StudentID],
[CalCourseProgNodeID],
[AcaCalSectionID],
[RetakeNo],
[ObtainedTotalMarks],
[ObtainedGPA],
[ObtainedGrade],
[GradeId],
[CourseStatusID],
[CourseStatusDate],
[AcaCalID],
[CourseID],
[VersionID],
[CourseCredit],
[CompletedCredit],
[Node_CourseID],
[NodeID],
[IsMultipleACUSpan],
[IsConsiderGPA],
[CourseWavTransfrID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[Remark]
)
 VALUES
(
@StudentID,
@CalCourseProgNodeID,
@AcaCalSectionID,
@RetakeNo,
@ObtainedTotalMarks,
@ObtainedGPA,
@ObtainedGrade,
@GradeId,
@CourseStatusID,
@CourseStatusDate,
@AcaCalID,
@CourseID,
@VersionID,
@CourseCredit,
@CompletedCredit,
@Node_CourseID,
@NodeID,
@IsMultipleACUSpan,
@IsConsiderGPA,
@CourseWavTransfrID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate,
@Remark
)
           
SET @ID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_Student_CourseHistoryUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<Sajib, Ahmed>								--
--								Create date:	< 2014-02-02 >								--
--								Description:	<Softwar Eng.>								--
--								Action:			<Add Remark Column>							--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_CC_Student_CourseHistoryUpdate]
(
@ID int   = NULL,
@StudentID int  = NULL,
@CalCourseProgNodeID int = NULL,
@AcaCalSectionID int = NULL,
@RetakeNo int = NULL,
@ObtainedTotalMarks numeric(18, 2) = NULL,
@ObtainedGPA numeric(18, 2) = NULL,
@ObtainedGrade varchar(2) = NULL,
@GradeId int = NULL,
@CourseStatusID int = NULL,
@CourseStatusDate datetime = NULL,
@AcaCalID int = NULL,
@CourseID int = NULL,
@VersionID int = NULL,
@CourseCredit numeric(18, 2) = NULL,
@CompletedCredit numeric(18, 2) = NULL,
@Node_CourseID int = NULL,
@NodeID int = NULL,
@IsMultipleACUSpan bit = NULL,
@IsConsiderGPA bit = NULL,
@CourseWavTransfrID int = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@Remark nvarchar(max) = NULL
)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_CC_Student_CourseHistory]
   SET

[StudentID]	=	@StudentID,
[CalCourseProgNodeID]	=	@CalCourseProgNodeID,
[AcaCalSectionID]	=	@AcaCalSectionID,
[RetakeNo]	=	@RetakeNo,
[ObtainedTotalMarks]	=	@ObtainedTotalMarks,
[ObtainedGPA]	=	@ObtainedGPA,
[ObtainedGrade]	=	@ObtainedGrade,
[GradeId]	=	@GradeId,
[CourseStatusID]	=	@CourseStatusID,
[CourseStatusDate]	=	@CourseStatusDate,
[AcaCalID]	=	@AcaCalID,
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[CourseCredit]	=	@CourseCredit,
[CompletedCredit]	=	@CompletedCredit,
[Node_CourseID]	=	@Node_CourseID,
[NodeID]	=	@NodeID,
[IsMultipleACUSpan]	=	@IsMultipleACUSpan,
[IsConsiderGPA]	=	@IsConsiderGPA,
[CourseWavTransfrID] = @CourseWavTransfrID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[Remark] = @Remark

WHERE ID = @ID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentPreCourseDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_StudentPreCourseDeleteById]
(
@StudentPreCourseId int = null
)

AS
BEGIN
SET NOCOUNT ON;

DELETE FROM [UIUEMS_CC_StudentPreCourse]
WHERE StudentPreCourseId = @StudentPreCourseId

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentPreCourseGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_StudentPreCourseGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       [UIUEMS_CC_StudentPreCourse]


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentPreCourseGetAllByParameter]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_StudentPreCourseGetAllByParameter]
(
	@Action int = NULL,
	@BatchCode nvarchar(3) = NULL,
	@ProgramCode nvarchar(3) = NULL,
	@PreMandatoryCourse nvarchar(max) = NULL,
	@PreCourseId int = NULL,
	@PreVersionId int = NULL,
	@MainCourseId int = NULL,
	@MainVersionId int = NULL
)
AS
BEGIN
SET NOCOUNT ON;

If @Action = 0
Begin
	Select a.* From UIUEMS_CC_StudentPreCourse a, UIUEMS_ER_Student b
	Where a.StudentId = b.StudentID and SUBSTRING(b.Roll, 4, 3) = @BatchCode and SUBSTRING(b.Roll, 1, 3) = @ProgramCode
	and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.MainCourseId = @MainCourseId and a.MainVersionId = @MainVersionId;
End
Else If @Action = 1 and @PreMandatoryCourse = 'English'
Begin
	Insert Into UIUEMS_CC_StudentPreCourse
	Select b.StudentID, @PreCourseId, @PreVersionId, 0, @MainCourseId, @MainVersionId, 0, 0, 'Transfer', 0, 0, NULL, NULL, -1, GETDATE(), NULL, NULL
	From UIUEMS_ER_Student b, [Admission.2.0.0].[dbo].[Candidate] c
	Where b.Roll = c.StudentID and SUBSTRING(c.StudentID, 4, 3) = @BatchCode and SUBSTRING(c.StudentID, 1, 3) = @ProgramCode and c.IsPreEnglish = 1
	and NOT EXISTS (Select a.StudentId From UIUEMS_CC_StudentPreCourse a Where a.StudentId = b.StudentID and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.MainCourseId = @MainCourseId and a.MainVersionId = @MainVersionId);

	Select a.* From UIUEMS_CC_StudentPreCourse a, UIUEMS_ER_Student b
	Where a.StudentId = b.StudentID and SUBSTRING(b.Roll, 4, 3) = @BatchCode and SUBSTRING(b.Roll, 1, 3) = @ProgramCode
	and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.MainCourseId = @MainCourseId and a.MainVersionId = @MainVersionId;
End
Else If @Action = 1 and @PreMandatoryCourse = 'Math'
Begin
	Insert Into UIUEMS_CC_StudentPreCourse
	Select b.StudentID, @PreCourseId, @PreVersionId, 0, @MainCourseId, @MainVersionId, 0, 0, 'Transfer', 0, 0, NULL, NULL, -1, GETDATE(), NULL, NULL
	From UIUEMS_ER_Student b, [Admission.2.0.0].[dbo].[Candidate] c
	Where b.Roll = c.StudentID and SUBSTRING(c.StudentID, 4, 3) = @BatchCode and SUBSTRING(c.StudentID, 1, 3) = @ProgramCode and c.IsPremath = 1
	and NOT EXISTS (Select a.StudentId From UIUEMS_CC_StudentPreCourse a Where a.StudentId = b.StudentID and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.MainCourseId = @MainCourseId and a.MainVersionId = @MainVersionId);

	Select a.* From UIUEMS_CC_StudentPreCourse a, UIUEMS_ER_Student b
	Where a.StudentId = b.StudentID and SUBSTRING(b.Roll, 4, 3) = @BatchCode and SUBSTRING(b.Roll, 1, 3) = @ProgramCode
	and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.PreCourseId = @PreCourseId and a.PreVersionId = @PreVersionId and a.MainCourseId = @MainCourseId and a.MainVersionId = @MainVersionId;
End

END

GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentPreCourseGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_StudentPreCourseGetById]
(
@StudentPreCourseId int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       [UIUEMS_CC_StudentPreCourse]
WHERE     (StudentPreCourseId = @StudentPreCourseId)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentPreCourseInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_StudentPreCourseInsert] 
(
@StudentPreCourseId int OUTPUT,
@StudentId int  = NULL,
@PreCourseId int  = NULL,
@PreVersionId int  = NULL,
@PreNodeCourseId int = NULL,
@MainCourseId int = NULL,
@MainVersionId int = NULL,
@ManiNodeCourseId int = NULL,
@IsBundle bit = NULL,
@Remarks nvarchar(max) = NULL,
@IsBool bit = NULL,
@Number int = NULL,
@Attribute1 nvarchar(500) = NULL,
@Attribute2 nvarchar(max) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_StudentPreCourse]
(

[StudentId],
[PreCourseId],
[PreVersionId],
[PreNodeCourseId],
[MainCourseId],
[MainVersionId],
[ManiNodeCourseId],
[IsBundle],
[Remarks],
[IsBool],
[Number],
[Attribute1],
[Attribute2],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(

@StudentId,
@PreCourseId,
@PreVersionId,
@PreNodeCourseId,
@MainCourseId,
@MainVersionId,
@ManiNodeCourseId,
@IsBundle,
@Remarks,
@IsBool,
@Number,
@Attribute1,
@Attribute2,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @StudentPreCourseId = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentPreCourseUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_StudentPreCourseUpdate] 

(
@StudentPreCourseId int  = NULL,
@StudentId int  = NULL,
@PreCourseId int  = NULL,
@PreVersionId int  = NULL,
@PreNodeCourseId int = NULL,
@MainCourseId int = NULL,
@MainVersionId int = NULL,
@ManiNodeCourseId int = NULL,
@IsBundle bit = NULL,
@Remarks nvarchar(max) = NULL,
@IsBool bit = NULL,
@Number int = NULL,
@Attribute1 nvarchar(500) = NULL,
@Attribute2 nvarchar(max) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)
AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_StudentPreCourse]
   SET

[StudentId]	=	@StudentId,
[PreCourseId]	=	@PreCourseId,
[PreVersionId]	=	@PreVersionId,
[PreNodeCourseId]	=	@PreNodeCourseId,
[MainCourseId]	=	@MainCourseId,
[MainVersionId]	=	@MainVersionId,
[ManiNodeCourseId]	=	@ManiNodeCourseId,
[IsBundle]	=	@IsBundle,
[Remarks]	=	@Remarks,
[IsBool]	=	@IsBool,
[Number]	=	@Number,
[Attribute1]	=	@Attribute1,
[Attribute2]	=	@Attribute2,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE StudentPreCourseId = @StudentPreCourseId
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentProbrationListGenerateANDInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_StudentProbrationListGenerateANDInsert]
(
	--@OutputValue Int Output,
	@FromAcaCalId Int = NULL,
	@ToAcaCalId Int = NULL,
	@FromRange numeric(18, 2) = NULL,
	@ToRange numeric(18, 2) = NULL,
	@ProgramId Int,
	@FromSemester Int = NULL,
	@ToSemester Int = NULL
)
As

Begin
	SET NOCOUNT ON;
	Declare @programCode Nvarchar(3); 
	Select @programCode = Code From UIUEMS_CC_Program Where ProgramID = @ProgramId;

	Delete UIUEMS_CC_StudentProbrationList Where SUBSTRING(Roll, 1, 3) = @programCode;

	Declare ProbitionInfo Cursor For
	Select Distinct b.StudentID, b.PersonID, b.Roll	From UIUEMS_CC_Student_CourseHistory a, UIUEMS_ER_Student b
	Where a.StudentID = b.StudentID and (a.AcaCalID between @FromAcaCalId and @ToAcaCalId) and SUBSTRING(b.Roll, 1, 3) = @programCode;

	Declare @StudentId Int, @PersonId Int, @Name Nvarchar(Max), @Roll Nvarchar(Max), @AcaCalId Int, @AcaCalCode nvarchar(3), @GPA numeric(18, 2), @CGPA numeric(18, 2), @ProvitionCount Int; Set @ProvitionCount = 1;
	Open ProbitionInfo
	Fetch Next From ProbitionInfo Into @StudentId, @PersonId, @Roll;
	While @@FETCH_STATUS = 0
	Begin
		Set @AcaCalId = NULL; Set @GPA = null; Set @CGPA = NULL;
		Select @AcaCalId = StdAcademicCalenderID, @GPA = GPA, @CGPA = CGPA From UIUEMS_ER_Student_ACUDetail c
		Where c.StudentID = @StudentId and (c.CGPA Between @FromRange And @ToRange) 
		and c.StdAcademicCalenderID = (Select Max(cc.StdAcademicCalenderID) From UIUEMS_ER_Student_ACUDetail cc Where cc.StudentID = c.StudentID);
		
		If @AcaCalId Is Null
		Begin
			--Print('If');
			Fetch Next From ProbitionInfo Into @StudentId, @PersonId, @Roll;
			Continue;
		End
		--Else
		--Begin
		--	Print('Else');
		--End
		--Print('Check');

		Select @Name = FirstName From UIUEMS_ER_Person Where PersonID = @PersonId;
		If @AcaCalId Between @FromSemester and @ToSemester
		Begin
			Select @AcaCalCode = BatchCode From UIUEMS_CC_AcademicCalender Where AcademicCalenderID = @AcaCalId;
			Insert Into UIUEMS_CC_StudentProbrationList Values(@StudentId, @PersonId, @Name, @Roll, @AcaCalId, @AcaCalCode, @GPA, @CGPA, @ProvitionCount, GETDATE());
		End

		Declare PerStudentAcaCalList Cursor For
		Select StdAcademicCalenderID, GPA, CGPA From UIUEMS_ER_Student_ACUDetail Where StudentID = @StudentId Order By StdAcademicCalenderID Desc;
		Declare @tempAcaCalId Int, @tempGPA numeric(18, 2), @tempCGPA numeric(18, 2);
		Open PerStudentAcaCalList
		Fetch Next From PerStudentAcaCalList Into @tempAcaCalId, @tempGPA, @tempCGPA;
		Set @ProvitionCount = 1;
		While @@FETCH_STATUS = 0
		Begin
			Declare @semesterCGPA numeric(18, 2), @semesterGPA numeric(18, 2), @semesterAcaCalId Int; Set @semesterCGPA = NULL; Set @semesterGPA = NULL; Set @semesterAcaCalId = NULL;
			Select @semesterCGPA = CGPA, @semesterGPA = GPA, @semesterAcaCalId = StdAcademicCalenderID From UIUEMS_ER_Student_ACUDetail 
			Where StudentID = @StudentId and StdAcademicCalenderID = (Select Max(StdAcademicCalenderID) From UIUEMS_ER_Student_ACUDetail ac Where ac.StdAcademicCalenderID < @tempAcaCalId and ac.StudentID = @StudentId);

			----print(@semesterCGPA);
			If (@semesterCGPA Between @FromRange And @ToRange) and @semesterCGPA Is Not NULL
			Begin
				Set @ProvitionCount = @ProvitionCount + 1;
				----print(@semesterAcaCalId);
				If @semesterAcaCalId Between @FromSemester and @ToSemester
				Begin
					Select @AcaCalCode = BatchCode From UIUEMS_CC_AcademicCalender Where AcademicCalenderID = @semesterAcaCalId;
					Insert Into UIUEMS_CC_StudentProbrationList Values(@StudentId, @PersonId, @Name, @Roll, @semesterAcaCalId, @AcaCalCode, @semesterGPA, @semesterCGPA, @ProvitionCount, GETDATE());
				End
			End
			Else
			Begin
				Update UIUEMS_CC_StudentProbrationList Set ProbationCount = @ProvitionCount Where StudentId = @StudentId;
				Break;
			End

			Set @tempAcaCalId = NULL; Set @tempGPA = NULL; Set @tempCGPA = NULL;
			Fetch Next From PerStudentAcaCalList Into @tempAcaCalId, @tempGPA, @tempCGPA;
		End
		Close PerStudentAcaCalList
		Deallocate PerStudentAcaCalList
		--Insert Into temp_Probation Values(@StudentId, @PersonId, @Name, @Roll, @AcaCalId, @GPA, @CGPA, @ProvitionCount);
		Set @Name = ''; Set @Roll = ''; Set @AcaCalId = 0; Set @GPA = 0; Set @CGPA = 0; Set @ProvitionCount = 0;
		Fetch Next From ProbitionInfo Into @StudentId, @PersonId, @Roll;
	End
	Close ProbitionInfo
	Deallocate ProbitionInfo
	Select * From UIUEMS_CC_StudentProbrationList Where SUBSTRING(Roll, 1, 3) = @programCode;
	--Set @OutputValue = 1;
End
GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_StudentProbrationListGetAllByProgramOrder]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create PROCEDURE [dbo].[UIUEMS_CC_StudentProbrationListGetAllByProgramOrder]
(
	@ProgramId Int = NULL,
	@OrderType Nvarchar(Max) = NULL
)
AS
BEGIN
SET NOCOUNT ON;

	Declare @ProgramCode Nvarchar(Max);
	Select @ProgramCode = Code From UIUEMS_CC_Program Where ProgramID = @ProgramId;

	If @OrderType = 'Probation[Ascending]'
	Begin
		Select * From UIUEMS_CC_StudentProbrationList Where SUBSTRING(Roll, 1, 3) = @ProgramCode Order By ProbationCount;
	End
	Else If @OrderType = 'Probation[Descending]'
	Begin
		Select * From UIUEMS_CC_StudentProbrationList Where SUBSTRING(Roll, 1, 3) = @ProgramCode Order By ProbationCount DESC;
	End
	Else If @OrderType = 'Roll[Ascending]'
	Begin
		Select * From UIUEMS_CC_StudentProbrationList Where SUBSTRING(Roll, 1, 3) = @ProgramCode Order By Roll;
	End
	Else If @OrderType = 'Roll[Descending]'
	Begin
		Select * From UIUEMS_CC_StudentProbrationList Where SUBSTRING(Roll, 1, 3) = @ProgramCode Order By Roll DESC;
	End

END
GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TimeSlotPlanDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_TimeSlotPlanDeleteById]
@TimeSlotPlanID int = null
AS
BEGIN
	
	SET NOCOUNT ON;

   Delete from [dbo].[UIUEMS_CC_TimeSlotPlan] 
           WHERE TimeSlotPlanID = @TimeSlotPlanID  
   
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TimeSlotPlanGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_TimeSlotPlanGetAll]

AS
BEGIN
	
	SET NOCOUNT ON;

   Select * from [dbo].[UIUEMS_CC_TimeSlotPlan] 
            
   
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TimeSlotPlanGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_TimeSlotPlanGetById]
@TimeSlotPlanID int = null
AS
BEGIN
	
	SET NOCOUNT ON;

   Select * from [dbo].[UIUEMS_CC_TimeSlotPlan] 
           WHERE TimeSlotPlanID = @TimeSlotPlanID  
   
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TimeSlotPlanInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_TimeSlotPlanInsert]
@TimeSlotPlanID int output,
@StartHour int = null,
@StartMin int = null,
@StartAMPM int = null,
@EndHour int = null,
@EndMin int = null,
@EndAMPM int = null,
@Type int = null,
@CreatedBy int = null,
@CreatedDate datetime = null,
@ModifiedBy int = null,
@ModifiedDate datetime = null
AS
BEGIN
	
	SET NOCOUNT ON;

   INSERT INTO [dbo].[UIUEMS_CC_TimeSlotPlan]
           ([StartHour]
           ,[StartMin]
           ,[StartAMPM]
           ,[EndHour]
           ,[EndMin]
           ,[EndAMPM]
           ,[Type]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
     VALUES
           (@StartHour
           ,@StartMin
           ,@StartAMPM
           ,@EndHour 
           ,@EndMin 
           ,@EndAMPM 
           ,@Type
           ,@CreatedBy 
           ,@CreatedDate 
           ,@ModifiedBy 
           ,@ModifiedDate )
           
           SET @TimeSlotPlanID = SCOPE_IDENTITY();
   
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TimeSlotPlanUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_CC_TimeSlotPlanUpdate]
@TimeSlotPlanID int = null,
@StartHour int = null,
@StartMin int = null,
@StartAMPM int = null,
@EndHour int = null,
@EndMin int = null,
@EndAMPM int = null,
@Type int = null,
@CreatedBy int = null,
@CreatedDate datetime = null,
@ModifiedBy int = null,
@ModifiedDate datetime = null
AS
BEGIN
	
	SET NOCOUNT ON;

   UPDATE [dbo].[UIUEMS_CC_TimeSlotPlan] SET
           [StartHour] = @StartHour
           ,[StartMin] = @StartMin
           ,[StartAMPM] = @StartAMPM
           ,[EndHour] = @EndHour
           ,[EndMin] = @EndMin
           ,[EndAMPM] = @EndAMPM
           ,[Type] = @Type
           ,[CreatedBy] = @CreatedBy
           ,[CreatedDate] = @CreatedDate
           ,[ModifiedBy] = @ModifiedBy
           ,[ModifiedDate] = @ModifiedDate    
           
           WHERE TimeSlotPlanID = @TimeSlotPlanID  
   
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeCalendarDetailDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_TreeCalendarDetailDeleteById]
(
@TreeCalendarDetailID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_TreeCalendarDetail]
WHERE TreeCalendarDetailID = @TreeCalendarDetailID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeCalendarDetailGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeCalendarDetailGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_TreeCalendarDetail


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeCalendarDetailGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeCalendarDetailGetById]
(
@TreeCalendarDetailID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_TreeCalendarDetail
WHERE     (TreeCalendarDetailID = @TreeCalendarDetailID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeCalendarDetailInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeCalendarDetailInsert] 
(
	@TreeCalendarDetailID int  OUTPUT,
	@TreeCalendarMasterID int  = NULL,
	@TreeMasterID int  = NULL,
	@CalendarMasterID int  = NULL,
	@CalendarDetailID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_TreeCalendarDetail]
(
	[TreeCalendarDetailID],
	[TreeCalendarMasterID],
	[TreeMasterID],
	[CalendarMasterID],
	[CalendarDetailID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@TreeCalendarDetailID,
	@TreeCalendarMasterID,
	@TreeMasterID,
	@CalendarMasterID,
	@CalendarDetailID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @TreeCalendarDetailID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeCalendarDetailUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeCalendarDetailUpdate]
(
	@TreeCalendarDetailID int  = NULL,
	@TreeCalendarMasterID int  = NULL,
	@TreeMasterID int  = NULL,
	@CalendarMasterID int  = NULL,
	@CalendarDetailID int  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_TreeCalendarDetail]
   SET

	[TreeCalendarMasterID]	=	@TreeCalendarMasterID,
	[TreeMasterID]	=	@TreeMasterID,
	[CalendarMasterID]	=	@CalendarMasterID,
	[CalendarDetailID]	=	@CalendarDetailID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE TreeCalendarDetailID = @TreeCalendarDetailID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeDetailDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_TreeDetailDeleteById]
(
@TreeDetailID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_TreeDetail]
WHERE TreeDetailID = @TreeDetailID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeDetailGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeDetailGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeDetailID],
[TreeMasterID],
[ParentNodeID],
[ChildNodeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_TreeDetail


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeDetailGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeDetailGetById]
(
@TreeDetailID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

[TreeDetailID],
[TreeMasterID],
[ParentNodeID],
[ChildNodeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

FROM       UIUEMS_CC_TreeDetail
WHERE     (TreeDetailID = @TreeDetailID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeDetailInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeDetailInsert] 
(
@TreeDetailID int   OUTPUT,
@TreeMasterID int  = NULL,
@ParentNodeID int = NULL,
@ChildNodeID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_TreeDetail]
(
[TreeDetailID],
[TreeMasterID],
[ParentNodeID],
[ChildNodeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@TreeDetailID,
@TreeMasterID,
@ParentNodeID,
@ChildNodeID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @TreeDetailID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_TreeDetailUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_TreeDetailUpdate]
(
@TreeDetailID int   = NULL,
@TreeMasterID int  = NULL,
@ParentNodeID int = NULL,
@ChildNodeID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_TreeDetail]
   SET

[TreeMasterID]	=	@TreeMasterID,
[ParentNodeID]	=	@ParentNodeID,
[ChildNodeID]	=	@ChildNodeID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE TreeDetailID = @TreeDetailID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_VNodeSetDeleteById]
(
@VNodeSetID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_VNodeSet]
WHERE VNodeSetID = @VNodeSetID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_VNodeSet


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetGetById]
(
@VNodeSetID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_VNodeSet
WHERE     (VNodeSetID = @VNodeSetID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetInsert] 
(
	@VNodeSetID int  OUTPUT,
	@VNodeSetMasterID int  = NULL,
	@NodeID int  = NULL,
	@SetNo int  = NULL,
	@OperandNodeID int = NULL,
	@OperandCourseID int = NULL,
	@OperandVersionID int = NULL,
	@NodeCourseID int = NULL,
	@OperatorID int  = NULL,
	@WildCard varchar(50) = NULL,
	@IsStudntSpec bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL


)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_VNodeSet]
(
	[VNodeSetID],
	[VNodeSetMasterID],
	[NodeID],
	[SetNo],
	[OperandNodeID],
	[OperandCourseID],
	[OperandVersionID],
	[NodeCourseID],
	[OperatorID],
	[WildCard],
	[IsStudntSpec],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@VNodeSetID,
	@VNodeSetMasterID,
	@NodeID,
	@SetNo,
	@OperandNodeID,
	@OperandCourseID,
	@OperandVersionID,
	@NodeCourseID,
	@OperatorID,
	@WildCard,
	@IsStudntSpec,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @VNodeSetID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_CC_VNodeSetMasterDeleteById]
(
@VNodeSetMasterID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_CC_VNodeSetMaster]
WHERE VNodeSetMasterID = @VNodeSetMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_VNodeSetMaster


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetMasterGetById]
(
@VNodeSetMasterID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_CC_VNodeSetMaster
WHERE     (VNodeSetMasterID = @VNodeSetMasterID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetMasterInsert] 
(
	@VNodeSetMasterID int  OUTPUT,
	@SetNo int  = NULL,
	@NodeID int  = NULL,
	@RequiredUnits money = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_CC_VNodeSetMaster]
(
	[VNodeSetMasterID],
	[SetNo],
	[NodeID],
	[RequiredUnits],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@VNodeSetMasterID,
	@SetNo,
	@NodeID,
	@RequiredUnits,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @VNodeSetMasterID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetMasterUpdate]
(
	@VNodeSetMasterID int  = NULL,
	@SetNo int  = NULL,
	@NodeID int  = NULL,
	@RequiredUnits money = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_VNodeSetMaster]
   SET

	[SetNo]	=	@SetNo,
	[NodeID]	=	@NodeID,
	[RequiredUnits]	=	@RequiredUnits,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE VNodeSetMasterID = @VNodeSetMasterID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_CC_VNodeSetUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_CC_VNodeSetUpdate]
(
	@VNodeSetID int  = NULL,
	@VNodeSetMasterID int  = NULL,
	@NodeID int  = NULL,
	@SetNo int  = NULL,
	@OperandNodeID int = NULL,
	@OperandCourseID int = NULL,
	@OperandVersionID int = NULL,
	@NodeCourseID int = NULL,
	@OperatorID int  = NULL,
	@WildCard varchar(50) = NULL,
	@IsStudntSpec bit  = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_CC_VNodeSet]
   SET


	[VNodeSetMasterID]	=	@VNodeSetMasterID,
	[NodeID]	=	@NodeID,
	[SetNo]	=	@SetNo,
	[OperandNodeID]	=	@OperandNodeID,
	[OperandCourseID]	=	@OperandCourseID,
	[OperandVersionID]	=	@OperandVersionID,
	[NodeCourseID]	=	@NodeCourseID,
	[OperatorID]	=	@OperatorID,
	[WildCard]	=	@WildCard,
	[IsStudntSpec]	=	@IsStudntSpec,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE VNodeSetID = @VNodeSetID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AddressDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_AddressDeleteById]
(
@AddressID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Address]
WHERE AddressID = @AddressID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AddressGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AddressGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Address


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AddressGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AddressGetById]
(
@AddressID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Address
WHERE     (AddressID = @AddressID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AddressInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AddressInsert] 
(
	@AddressID int  OUTPUT,
	@AddressTypeID int  = NULL,
	@AddressLine1 varchar(500) = NULL,
	@AddressLine2 varchar(500) = NULL,
	@Road varchar(500) = NULL,
	@House varchar(500) = NULL,
	@VillageOrArea varchar(500) = NULL,
	@Country varchar(100) = NULL,
	@Division int = NULL,
	@District int = NULL,
	@Upozilla varchar(100) = NULL,
	@Thana varchar(100) = NULL,
	@PostCode varchar(100) = NULL,
	@Remarks varchar(100) = NULL,
	@CellPhone varchar(50) = NULL,
	@LandPhone varchar(50) = NULL,
	@Fax varchar(100) = NULL,
	@EmailAddress varchar(500) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Address]
(
	[AddressID],
	[AddressTypeID],
	[AddressLine1],
	[AddressLine2],
	[Road],
	[House],
	[VillageOrArea],
	[Country],
	[Division],
	[District],
	[Upozilla],
	[Thana],
	[PostCode],
	[Remarks],
	[CellPhone],
	[LandPhone],
	[Fax],
	[EmailAddress],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@AddressID,
	@AddressTypeID,
	@AddressLine1,
	@AddressLine2,
	@Road,
	@House,
	@VillageOrArea,
	@Country,
	@Division,
	@District,
	@Upozilla,
	@Thana,
	@PostCode,
	@Remarks,
	@CellPhone,
	@LandPhone,
	@Fax,
	@EmailAddress,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @AddressID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AddressUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AddressUpdate]
(
	@AddressID int  = NULL,
	@AddressTypeID int  = NULL,
	@AddressLine1 varchar(500) = NULL,
	@AddressLine2 varchar(500) = NULL,
	@Road varchar(500) = NULL,
	@House varchar(500) = NULL,
	@VillageOrArea varchar(500) = NULL,
	@Country varchar(100) = NULL,
	@Division int = NULL,
	@District int = NULL,
	@Upozilla varchar(100) = NULL,
	@Thana varchar(100) = NULL,
	@PostCode varchar(100) = NULL,
	@Remarks varchar(100) = NULL,
	@CellPhone varchar(50) = NULL,
	@LandPhone varchar(50) = NULL,
	@Fax varchar(100) = NULL,
	@EmailAddress varchar(500) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Address]
   SET


	[AddressTypeID]	=	@AddressTypeID,
	[AddressLine1]	=	@AddressLine1,
	[AddressLine2]	=	@AddressLine2,
	[Road]	=	@Road,
	[House]	=	@House,
	[VillageOrArea]	=	@VillageOrArea,
	[Country]	=	@Country,
	[Division]	=	@Division,
	[District]	=	@District,
	[Upozilla]	=	@Upozilla,
	[Thana]	=	@Thana,
	[PostCode]	=	@PostCode,
	[Remarks]	=	@Remarks,
	[CellPhone]	=	@CellPhone,
	[LandPhone]	=	@LandPhone,
	[Fax]	=	@Fax,
	[EmailAddress]	=	@EmailAddress,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE AddressID = @AddressID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Admission_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_ER_Admission_Insert]
(
	@AdmissionID int Output,
	@StudentID int = NULL,
	@AdmissionCalenderID int = NULL,
	@Remarks varchar(500) = NULL,
	@IsRule bit = NULL,
	@IsLastAdmission bit = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_Admission]
(
	[StudentID],
	[AdmissionCalenderID],
	[Remarks],
	[IsRule],
	[IsLastAdmission],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@StudentID,
	@AdmissionCalenderID,
	@Remarks,
	@IsRule,
	@IsLastAdmission,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @AdmissionID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AdmissionDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_AdmissionDeleteById]
(
@AdmissionID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Admission]
WHERE AdmissionID = @AdmissionID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AdmissionGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AdmissionGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Admission


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AdmissionGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AdmissionGetById]
(
@AdmissionID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Admission
WHERE     (AdmissionID = @AdmissionID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AdmissionGetByStudentID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AdmissionGetByStudentID]
(
@studentID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Admission
WHERE     (StudentID = @studentID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AdmissionInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AdmissionInsert] 
(
	@AdmissionID			int output,
	@StudentID				int = NULL,
	@PersonID				int = NULL,
	@AdmissionCalenderID	int = NULL,
	@Remarks				varchar(500) = NULL,
	@IsRule					bit = NULL,
	@IsLastAdmission		bit = NULL,
	@CreatedBy				int = NULL,
	@CreatedDate			datetime = NULL,
	@ModifiedBy				int = NULL,
	@ModifiedDate			datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS.2.0.0].[dbo].[UIUEMS_ER_Admission]
(
	[StudentID],
	[PersonID],
	[AdmissionCalenderID],
	[Remarks],
	[IsRule],
	[IsLastAdmission],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@StudentID,
	@PersonID,
	@AdmissionCalenderID,
	@Remarks,
	@IsRule,
	@IsLastAdmission,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)
           
SET @AdmissionID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_AdmissionUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_AdmissionUpdate]
(
	@AdmissionID int  = NULL,
	@StudentID int = NULL,
	@AdmissionCalenderID int = NULL,
	@Remarks varchar(500) = NULL,
	@IsRule bit = NULL,
	@IsLastAdmission bit = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Admission]
   SET


	[StudentID]	=	@StudentID,
	[AdmissionCalenderID]	=	@AdmissionCalenderID,
	[Remarks]	=	@Remarks,
	[IsRule]	=	@IsRule,
	[IsLastAdmission]	=	@IsLastAdmission,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE AdmissionID = @AdmissionID
           
END
	



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseStatusDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_CourseStatusDeleteById]
(
@CourseStatusID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_CourseStatus]
WHERE CourseStatusID = @CourseStatusID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseStatusGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseStatusGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_CourseStatus


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseStatusGetByCode]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseStatusGetByCode]
(
@Code nvarchar(50) = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_CourseStatus
WHERE     (Code = @Code)

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseStatusGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseStatusGetById]
(
@CourseStatusID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_CourseStatus
WHERE     (CourseStatusID = @CourseStatusID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseStatusInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseStatusInsert] 
(
	@CourseStatusID int  OUTPUT,
	@Code varchar(2)  = NULL,
	@Description varchar(50)  = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_CourseStatus]
(
	[CourseStatusID],
	[Code],
	[Description],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@CourseStatusID,
	@Code,
	@Description,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @CourseStatusID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseStatusUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseStatusUpdate]
(
	@CourseStatusID int  = NULL,
	@Code varchar(2)  = NULL,
	@Description varchar(50)  = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_CourseStatus]
   SET


	[Code]	=	@Code,
	[Description]	=	@Description,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE CourseStatusID = @CourseStatusID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfr_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-21 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfr_Insert]
(
	@CourseWavTransfrID int Output,
	@StudentID int = NULL,
	@UniversityName varchar(50) = NULL,
	@FromDate datetime = NULL,
	@ToDate datetime = NULL,
	@DivisionType int = NULL,
	@CourseStatusID int = NULL,
	@Remarks nvarchar(MAX) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_CourseWavTransfr]
(
	[StudentID],
	[UniversityName],
	[FromDate],
	[ToDate],
	[DivisionType],
	[CourseStatusID],
	[Remarks],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@StudentID,
	@UniversityName,
	@FromDate,
	@ToDate,
	@DivisionType,
	@CourseStatusID,
	@Remarks,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @CourseWavTransfrID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_CourseWavTransfrDeleteById]
(
@CourseWavTransfrID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_CourseWavTransfr]
WHERE CourseWavTransfrID = @CourseWavTransfrID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrDetailDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_CourseWavTransfrDetailDeleteById]
(
@CourseWavTransfrDetailID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_CourseWavTransfrDetail]
WHERE CourseWavTransfrDetailID = @CourseWavTransfrDetailID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrDetailGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrDetailGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_CourseWavTransfrDetail


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrDetailGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrDetailGetById]
(
@CourseWavTransfrDetailID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_CourseWavTransfrDetail
WHERE     (CourseWavTransfrDetailID = @CourseWavTransfrDetailID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrDetailInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrDetailInsert] 
(
@CourseWavTransfrDetailID int  OUTPUT,
@CourseWavTransfrMasterID int  = NULL,
@OwnerNodeCourseID int  = NULL,
@AgainstCourseInfo varchar(50) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_CourseWavTransfrDetail]
(
[CourseWavTransfrDetailID],
[CourseWavTransfrMasterID],
[OwnerNodeCourseID],
[AgainstCourseInfo],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@CourseWavTransfrDetailID,
@CourseWavTransfrMasterID,
@OwnerNodeCourseID,
@AgainstCourseInfo,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @CourseWavTransfrDetailID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrDetailUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrDetailUpdate]
(
@CourseWavTransfrDetailID int  = NULL,
@CourseWavTransfrMasterID int  = NULL,
@OwnerNodeCourseID int  = NULL,
@AgainstCourseInfo varchar(50) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_CourseWavTransfrDetail]
   SET
[CourseWavTransfrMasterID]	=	@CourseWavTransfrMasterID,
[OwnerNodeCourseID]	=	@OwnerNodeCourseID,
[AgainstCourseInfo]	=	@AgainstCourseInfo,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE CourseWavTransfrDetailID = @CourseWavTransfrDetailID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_ER_CourseWavTransfr


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrGetById]
(
@CourseWavTransfrID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_ER_CourseWavTransfr
WHERE     (CourseWavTransfrID = @CourseWavTransfrID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrInsert] 
(
@CourseWavTransfrID int  OUTPUT,
@StudentID int  = NULL,
@UniversityName varchar(50) = NULL,
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@DivisionType int = NULL,
@CourseStatusID int = NULL,
@Remarks nvarchar(max) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_CourseWavTransfr]
(
[CourseWavTransfrID],
[StudentID],
[UniversityName],
[FromDate],
[ToDate],
[DivisionType],
[CourseStatusID],
[Remarks],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@CourseWavTransfrID,
@StudentID,
@UniversityName,
@FromDate,
@ToDate,
@DivisionType,
@CourseStatusID,
@Remarks,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @CourseWavTransfrID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrMasterDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_CourseWavTransfrMasterDeleteById]
(
@CourseWavTransfrMasterID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_CourseWavTransfrMaster]
WHERE CourseWavTransfrMasterID = @CourseWavTransfrMasterID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrMasterGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrMasterGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_ER_CourseWavTransfrMaster


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrMasterGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrMasterGetById]
(
@CourseWavTransfrMasterID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_CourseWavTransfrMaster
WHERE     (CourseWavTransfrMasterID = @CourseWavTransfrMasterID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrMasterInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrMasterInsert] 
(
@CourseWavTransfrMasterID int  OUTPUT,
@StudentID int  = NULL,
@UniversityName varchar(50) = NULL,
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@DivisionType int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_CourseWavTransfrMaster]
(
[CourseWavTransfrMasterID],
[StudentID],
[UniversityName],
[FromDate],
[ToDate],
[DivisionType],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@CourseWavTransfrMasterID,
@StudentID,
@UniversityName,
@FromDate,
@ToDate,
@DivisionType,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @CourseWavTransfrMasterID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrMasterUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrMasterUpdate]
(
@CourseWavTransfrMasterID int  = NULL,
@StudentID int  = NULL,
@UniversityName varchar(50) = NULL,
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@DivisionType int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_CourseWavTransfrMaster]
   SET
[StudentID]	=	@StudentID,
[UniversityName]	=	@UniversityName,
[FromDate]	=	@FromDate,
[ToDate]	=	@ToDate,
[DivisionType]	=	@DivisionType,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE CourseWavTransfrMasterID = @CourseWavTransfrMasterID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_CourseWavTransfrUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_CourseWavTransfrUpdate]
(
@CourseWavTransfrID int  = NULL,
@StudentID int  = NULL,
@UniversityName varchar(50) = NULL,
@FromDate datetime = NULL,
@ToDate datetime = NULL,
@DivisionType int = NULL,
@CourseStatusID int = NULL,
@Remarks nvarchar(max) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_CourseWavTransfr]
   SET
[StudentID]	=	@StudentID,
[UniversityName]	=	@UniversityName,
[FromDate]	=	@FromDate,
[ToDate]	=	@ToDate,
[DivisionType]	=	@DivisionType,
[CourseStatusID]	=	@CourseStatusID,
[Remarks]	=	@Remarks,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE CourseWavTransfrID = @CourseWavTransfrID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Person_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_ER_Person_Insert]
(
	@PersonID int Output,
	@Prefix int = NULL,
	@FirstName varchar(max) = NULL,
	@MiddleName varchar(100) = NULL,
	@LastName varchar(100) = NULL,
	@NickOrOtherName varchar(100) = NULL,
	@DOB datetime = NULL,
	@Gender varchar(100) = NULL,
	@MatrialStatus varchar(100) = NULL,
	@BloodGroup varchar(100) = NULL,
	@Religion nvarchar(500) = NULL,
	@FatherName varchar(MAX) = NULL,
	@FatherProfession varchar(250) = NULL,
	@MotherName varchar(MAX) = NULL,
	@MotherProfession varchar(250) = NULL,
	@Nationality nvarchar(500) = NULL,
	@PhotoPath varchar(500) = NULL,
	@IsActive bit = NULL,
	@IsDeleted bit = NULL,
	@Remarks nvarchar(500) = NULL,
	@Phone varchar(50) = NULL,
	@Email nvarchar(250) = NULL,
	@CandidateID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@TypeId int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_Person]
(
	[Prefix],
	[FirstName],
	[MiddleName],
	[LastName],
	[NickOrOtherName],
	[DOB],
	[Gender],
	[MatrialStatus],
	[BloodGroup],
	[Religion],
	[Nationality],
	[FatherName],
	[FatherProfession],
	[MotherName],
	[MotherProfession],
	[PhotoPath],
	[IsActive],
	[IsDeleted],
	[Remarks],
	[Phone],
	[Email],
	[CandidateID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[TypeId]
)
 VALUES
(
	@Prefix,
	@FirstName,
	@MiddleName,
	@LastName,
	@NickOrOtherName,
	@DOB,
	@Gender,
	@MatrialStatus,
	@BloodGroup,
	@Religion,
	@Nationality,
	@FatherName,
	@FatherProfession,
	@MotherName,
	@MotherProfession,
	@PhotoPath,
	@IsActive,
	@IsDeleted,
	@Remarks,
	@Phone,
	@Email,
	@CandidateID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@TypeId
)           
SET @PersonID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_ER_PersonBlockDeleteById]
(
	@PersonBlockId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_PersonBlock]
WHERE PersonBlockId = @PersonBlockId

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockDeleteByPersonId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE  [dbo].[UIUEMS_ER_PersonBlockDeleteByPersonId]
(
	@PersonId int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_PersonBlock]
WHERE PersonId = @PersonId

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonBlockGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_PersonBlock


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonBlockGetById]
(
	@PersonBlockId int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_PersonBlock
WHERE     (PersonBlockId = @PersonBlockId)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockGetByPersonID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonBlockGetByPersonID]
(
	@PersonID int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_PersonBlock
WHERE     (PersonId = @PersonID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonBlockInsert]
(
	@PersonBlockId int OUTPUT,
	@PersonId int = NULL,
	@StartDateAndTime datetime = NULL,
	@EndDateAndTime datetime = NULL,
	@Remarks nvarchar(500) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

IF NOT EXISTS(SELECT * FROM UIUEMS_ER_PersonBlock where PersonId = @PersonId)
	BEGIN
		Insert Into [dbo].[UIUEMS_ER_PersonBlock]
		(
			[PersonId],
			[StartDateAndTime],
			[EndDateAndTime],
			[Remarks],
			[CreatedBy],
			[CreatedDate],
			[ModifiedBy],
			[ModifiedDate]
		)
		 VALUES
		(
			@PersonId,
			@StartDateAndTime,
			@EndDateAndTime,
			@Remarks,
			@CreatedBy,
			@CreatedDate,
			@ModifiedBy,
			@ModifiedDate
		)           
		SET @PersonBlockId = SCOPE_IDENTITY()
	END
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonBlockUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonBlockUpdate]
(
	@PersonBlockId int = NULL,
	@PersonId int = NULL,
	@StartDateAndTime datetime = NULL,
	@EndDateAndTime datetime = NULL,
	@Remarks nvarchar(500) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_PersonBlock]
   SET

	[PersonId]=@PersonId,
	[StartDateAndTime]=@StartDateAndTime,
	[EndDateAndTime]=@EndDateAndTime,
	[Remarks]=@Remarks,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE PersonBlockId = @PersonBlockId
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_PersonDeleteById]
(
@PersonID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Person]
WHERE PersonID = @PersonID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_ER_Person


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonGetById]
(
@PersonID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Person
WHERE     (PersonID = @PersonID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonGetByUserID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonGetByUserID]
(
	@UserID int = null
)

AS
BEGIN
SET NOCOUNT ON;

Select p.* From UIUEMS_ER_Person p, UIUEMS_AD_UserInPerson up Where p.PersonID = up.PersonID and up.User_ID = @UserID;

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-20 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonInsert]
(
	@PersonID int Output,
	@Prefix int = NULL,
	@FirstName varchar(max) = NULL,
	@MiddleName varchar(100) = NULL,
	@LastName varchar(100) = NULL,
	@NickOrOtherName varchar(100) = NULL,
	@DOB datetime = NULL,
	@Gender varchar(100) = NULL,
	@MatrialStatus varchar(100) = NULL,
	@BloodGroup varchar(100) = NULL,
	@Religion nvarchar(500) = NULL,
	@FatherName varchar(MAX) = NULL,
	@FatherProfession varchar(250) = NULL,
	@MotherName varchar(MAX) = NULL,
	@MotherProfession varchar(250) = NULL,
	@Nationality nvarchar(500) = NULL,
	@PhotoPath varchar(500) = NULL,
	@IsActive bit = NULL,
	@IsDeleted bit = NULL,
	@Remarks nvarchar(500) = NULL,
	@Phone varchar(50) = NULL,
	@Email nvarchar(250) = NULL,
	@CandidateID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@TypeId int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_Person]
(
	[Prefix],
	[FirstName],
	[MiddleName],
	[LastName],
	[NickOrOtherName],
	[DOB],
	[Gender],
	[MatrialStatus],
	[BloodGroup],
	[Religion],
	[Nationality],
	[FatherName],
	[FatherProfession],
	[MotherName],
	[MotherProfession],
	[PhotoPath],
	[IsActive],
	[IsDeleted],
	[Remarks],
	[Phone],
	[Email],
	[CandidateID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[TypeId]
)
 VALUES
(
	@Prefix,
	@FirstName,
	@MiddleName,
	@LastName,
	@NickOrOtherName,
	@DOB,
	@Gender,
	@MatrialStatus,
	@BloodGroup,
	@Religion,
	@Nationality,
	@FatherName,
	@FatherProfession,
	@MotherName,
	@MotherProfession,
	@PhotoPath,
	@IsActive,
	@IsDeleted,
	@Remarks,
	@Phone,
	@Email,
	@CandidateID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@TypeId
)           
SET @PersonID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_PersonUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_PersonUpdate]
(
	@PersonID int  = NULL,
	@Prefix int = NULL,
	@FirstName varchar(max) = NULL,
	@MiddleName varchar(100) = NULL,
	@LastName varchar(100) = NULL,
	@NickOrOtherName varchar(100) = NULL,
	@DOB datetime = NULL,
	@Gender varchar(100) = NULL,
	@MatrialStatus varchar(100) = NULL,
	@BloodGroup varchar(100) = NULL,
	@Religion nvarchar(500) = NULL,
	@FatherName varchar(MAX) = NULL,
	@FatherProfession varchar(250) = NULL,
	@MotherName varchar(MAX) = NULL,
	@MotherProfession varchar(250) = NULL,
	@Nationality nvarchar(500) = NULL,
	@PhotoPath varchar(500) = NULL,
	@IsActive bit = NULL,
	@IsDeleted bit = NULL,
	@Remarks nvarchar(500) = NULL,
	@Phone varchar(50) = NULL,
	@Email nvarchar(250) = NULL,
	@CandidateID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@TypeId int = NULL

)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_ER_Person]
   SET

	[Prefix] = @Prefix,
	[FirstName] = @FirstName,
	[MiddleName] = @MiddleName,
	[LastName] = @LastName,
	[NickOrOtherName] = @NickOrOtherName,
	[DOB] = @DOB,
	[Gender] = @Gender,
	[MatrialStatus] = @MatrialStatus,
	[BloodGroup] = @BloodGroup,
	[Religion] = @Religion,
	[Nationality] = @Nationality,
	[FatherName] = @FatherName,
	[FatherProfession] = @FatherProfession,
	[MotherName] = @MotherName,
	[MotherProfession] = @MotherProfession,
	[PhotoPath] = @PhotoPath,
	[IsActive] = @IsActive,
	[IsDeleted] = @IsDeleted,
	[Remarks] = @Remarks,
	[Phone] = @Phone,
	[Email] = @Email,
	[CandidateID] = @CandidateID,
	[CreatedBy] = @CreatedBy,
	[CreatedDate] = @CreatedDate,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate,
	[TypeId] = @TypeId

WHERE PersonID = @PersonID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_pickStudentAndShow]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_pickStudentAndShow] 
	-- Add the parameters for the stored procedure here
	@roll nvarchar(15)=NULL

AS
BEGIN
	
	SET NOCOUNT ON;

Select s.StudentID,s.Roll,p.FirstName
From UIUEMS_ER_Student s inner join UIUEMS_ER_Person p
on s.PersonID=p.PersonID
where s.Roll LIKE + @roll + '%' 


END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SchemeSetupDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-15		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE  [dbo].[UIUEMS_ER_SchemeSetupDeleteById]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_SchemeSetup]
WHERE Id = @Id

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SchemeSetupGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-15		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_SchemeSetupGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_SchemeSetup


END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SchemeSetupGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-15		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_SchemeSetupGetById]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_SchemeSetup
WHERE     (Id = @Id)

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SchemeSetupInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-15		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_SchemeSetupInsert]
(
	@Id int OUTPUT,
	@SchemeName nvarchar(max) = NULL,
	@FromBatch int = NULL,
	@ToBatch int = NULL,
	@Percentage100 nvarchar(max) = NULL,
	@Percentage50 nvarchar(max) = NULL,
	@Percentage25 nvarchar(max) = NULL,
	@Attribute1 nvarchar(MAX) = NULL,
	@Attribute2 nvarchar(MAX) = NULL,
	@Attribute3 nvarchar(MAX) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_SchemeSetup]
(
	[SchemeName],
	[FromBatch],
	[ToBatch],
	[Percentage100],
	[Percentage50],
	[Percentage25],
	[Attribute1],
	[Attribute2],
	[Attribute3],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@SchemeName,
	@FromBatch,
	@ToBatch,
	@Percentage100,
	@Percentage50,
	@Percentage25,
	@Attribute1,
	@Attribute2,
	@Attribute3,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @Id = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SchemeSetupUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-15		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_SchemeSetupUpdate]
(
	@Id int = NULL,
	@SchemeName nvarchar(max) = NULL,
	@FromBatch int = NULL,
	@ToBatch int = NULL,
	@Percentage100 nvarchar(max) = NULL,
	@Percentage50 nvarchar(max) = NULL,
	@Percentage25 nvarchar(max) = NULL,
	@Attribute1 nvarchar(MAX) = NULL,
	@Attribute2 nvarchar(MAX) = NULL,
	@Attribute3 nvarchar(MAX) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_SchemeSetup]
   SET
	
	[SchemeName]=@SchemeName,
	[FromBatch]=@FromBatch,
	[ToBatch]=@ToBatch,
	[Percentage100]=@Percentage100,
	[Percentage50]=@Percentage50,
	[Percentage25]=@Percentage25,
	[Attribute1]=@Attribute1,
	[Attribute2]=@Attribute2,
	[Attribute3]=@Attribute3,
	[CreatedBy]=@CreatedBy,
	[CreatedDate]=@CreatedDate,
	[ModifiedBy]=@ModifiedBy,
	[ModifiedDate]=@ModifiedDate

WHERE Id = @Id
           
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE  [dbo].[UIUEMS_ER_ScholarshipListDeleteById]
(
	@Id int=NULL
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_ScholarshipList]
WHERE Id = @Id;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListGetAll]

AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_ER_ScholarshipList;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListGetAllByAcaCalProg]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListGetAllByAcaCalProg]
(
	@AcaCalId int = NULL,
	@ProgramCode nvarchar(3) = NULL
)
AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_ER_ScholarshipList Where AcaCalId = @AcaCalId and SUBSTRING(Roll, 1, 3) = @ProgramCode;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListGetAllByAcaCalProgBatch]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListGetAllByAcaCalProgBatch]
(
	@AcaCalId int = NULL,
	@ProgramCode nvarchar(3) = NULL,
	@FromBatch nvarchar(3) = NULL,
	@ToBatch nvarchar(3) = NULL
)
AS
BEGIN
SET NOCOUNT ON;
Declare @nextAcaCalID Int; Set @nextAcaCalID = 0;
Select @nextAcaCalID = min(AcademicCalenderID) From UIUEMS_CC_AcademicCalender Where AcademicCalenderID > @AcaCalId;

Delete UIUEMS_ER_ScholarshipList Where AcaCalId = @AcaCalId and SUBSTRING(Roll, 1, 3) = @ProgramCode and Convert(int, SUBSTRING(Roll, 4, 3)) Between Convert(int, @FromBatch) and Convert(int, @ToBatch);

Insert Into UIUEMS_ER_ScholarshipList
Select StdAcademicCalenderID, a.StudentID, s.Roll, p.FirstName, a.GPA, a.Credit , 
(Select Case When a.GPA = 0 OR SUM(h.CourseCredit) Is NULL then 0 else SUM(h.CourseCredit) end From UIUEMS_CC_Student_CourseHistory h Where h.StudentID = s.StudentID and h.AcaCalID = @AcaCalId and (h.GradeId is not null and h.ObtainedGrade != 'F' and h.CourseStatusID != 14)) as PassCredit,
(Select Case When SUM(h1.CourseCredit) > 0 Then SUM(h1.CourseCredit) Else 0 End From UIUEMS_CC_Student_CourseHistory h1 Where h1.StudentID = s.StudentID and h1.AcaCalID = @nextAcaCalID) as RegisterCredit,
null, null, null, null, null, 99, GETDATE(), null, null
From UIUEMS_ER_Student_ACUDetail a, UIUEMS_ER_Student s, UIUEMS_ER_Person p
Where a.StudentID = s.StudentID and s.PersonID = p.PersonID and StdAcademicCalenderID = @AcaCalId and SUBSTRING(s.Roll, 1, 3) = @ProgramCode and Convert(int, SUBSTRING(s.Roll, 4, 3)) Between Convert(int, @FromBatch) and Convert(int, @ToBatch)
Order by a.GPA Desc, SUBSTRING(s.Roll, 4, 9) Asc;

Select * From UIUEMS_ER_ScholarshipList Where AcaCalId = @AcaCalId and SUBSTRING(Roll, 1, 3) = @ProgramCode and Convert(int, SUBSTRING(Roll, 4, 3)) Between Convert(int, @FromBatch) and Convert(int, @ToBatch);

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListGetAllByParameter]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListGetAllByParameter]
(
	@AcaCalId int = NULL,
	@ProgramCode nvarchar(3) = NULL,
	@FromBatch nvarchar(3) = NULL,
	@ToBatch nvarchar(3) = NULL
)
AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_ER_ScholarshipList Where AcaCalId = @AcaCalId and SUBSTRING(Roll, 1, 3) = @ProgramCode and Convert(int, SUBSTRING(Roll, 4, 3)) Between Convert(int, @FromBatch) and Convert(int, @ToBatch);

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListGetById]
(
	@Id int=NULL
)
AS
BEGIN
SET NOCOUNT ON;

SELECT * FROM UIUEMS_ER_ScholarshipList WHERE  Id = @Id;

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListInsert]
(
	@Id int OUTPUT,
	@AcaCalId int = NULL,
	@StudentId int = NULL,
	@Roll nvarchar(10) = NULL,
	@Name nvarchar(max) = NULL,
	@GPA numeric(18,2) = NULL,
	@Credit numeric(3,1) = NULL,
	@PassCredit numeric(3,1) = NULL,
	@RegisterCredit numeric(3,1) = NULL,
	@CalculateScholarship nvarchar(10) = NULL,
	@ManualScholarship nvarchar(10) = NULL,
	@Attribute1 nvarchar(max) = NULL,
	@Attribute2 nvarchar(max) = NULL,
	@Attribute3 nvarchar(max) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_ScholarshipList]
(
	[AcaCalId],
	[StudentId],
	[Roll],
	[Name],
	[GPA],
	[Credit],
	[PassCredit],
	[RegisterCredit],
	[CalculateScholarship],
	[ManualScholarship],
	[Attribute1],
	[Attribute2],
	[Attribute3],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]
)
 VALUES
(
	@AcaCalId,
	@StudentId,
	@Roll,
	@Name,
	@GPA,
	@Credit,
	@PassCredit,
	@RegisterCredit,
	@CalculateScholarship,
	@ManualScholarship,
	@Attribute1,
	@Attribute2,
	@Attribute3,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate
)           
SET @Id = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ScholarshipListUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<	Md. Sajib Ahmed	>						--
--								Create date:	<	2014-02-11		>						--
--								Description:	<	Software Eng.	>						--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_ScholarshipListUpdate]
(
	@Id int = NULL,
	@AcaCalId int = NULL,
	@StudentId int = NULL,
	@Roll nvarchar(10) = NULL,
	@Name nvarchar(max) = NULL,
	@GPA numeric(18,2) = NULL,
	@Credit numeric(3,1) = NULL,
	@PassCredit numeric(3,1) = NULL,
	@RegisterCredit numeric(3,1) = NULL,
	@CalculateScholarship nvarchar(10) = NULL,
	@ManualScholarship nvarchar(10) = NULL,
	@Attribute1 nvarchar(10) = NULL,
	@Attribute2 nvarchar(10) = NULL,
	@Attribute3 nvarchar(10) = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT OFF;

UPDATE [UIUEMS_ER_ScholarshipList]
	SET

	[AcaCalId] = @AcaCalId,
	[StudentId] = @StudentId,
	[Roll] = @Roll,
	[Name] = @Name,
	[GPA] = @GPA,
	[Credit] = @Credit,
	[PassCredit] = @PassCredit,
	[RegisterCredit] = @RegisterCredit,
	[CalculateScholarship] = @CalculateScholarship,
	[ManualScholarship] = @ManualScholarship,
	[Attribute1] = @Attribute1,
	[Attribute2] = @Attribute2,
	[Attribute3] = @Attribute3,
	[CreatedBy] = @CreatedBy,
	[CreatedDate] = @CreatedDate,
	[ModifiedBy] = @ModifiedBy,
	[ModifiedDate] = @ModifiedDate

WHERE Id = @Id;
           
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SkillTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_SkillTypeDeleteById]
(
@SkillTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_SkillType]
WHERE SkillTypeID = @SkillTypeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SkillTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_SkillTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_SkillType


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SkillTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_SkillTypeGetById]
(
@SkillTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_SkillType
WHERE     (SkillTypeID = @SkillTypeID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SkillTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_SkillTypeInsert] 
(
@SkillTypeID int  OUTPUT,
@TypeDescription varchar(200)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_SkillType]
(
[SkillTypeID],
[TypeDescription]

)
 VALUES
(
@SkillTypeID,
@TypeDescription

)
           
SET @SkillTypeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_SkillTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_SkillTypeUpdate]
(
@SkillTypeID int  = NULL,
@TypeDescription varchar(200)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_SkillType]
   SET

[TypeDescription]	=	@TypeDescription


WHERE SkillTypeID = @SkillTypeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StatusTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_StatusTypeDeleteById]
(
@StatusTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_StatusType]
WHERE StatusTypeID = @StatusTypeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StatusTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StatusTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_StatusType


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StatusTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StatusTypeGetById]
(
@StatusTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_StatusType
WHERE     (StatusTypeID = @StatusTypeID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StatusTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StatusTypeInsert] 
(
@StatusTypeID int  OUTPUT,
@TypeDescription varchar(200)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_StatusType]
(
[StatusTypeID],
[TypeDescription]

)
 VALUES
(
@StatusTypeID,
@TypeDescription

)
           
SET @StatusTypeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StatusTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StatusTypeUpdate]
(
@StatusTypeID int  = NULL,
@TypeDescription varchar(200)  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_StatusType]
   SET

[TypeDescription]	=	@TypeDescription


WHERE StatusTypeID = @StatusTypeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Std_AcademicCalenderDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_Std_AcademicCalenderDeleteById]
(
@StdAcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Std_AcademicCalender]
WHERE StdAcademicCalenderID = @StdAcademicCalenderID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Std_AcademicCalenderGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Std_AcademicCalenderGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_ER_Std_AcademicCalender


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Std_AcademicCalenderGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Std_AcademicCalenderGetById]
(
@StdAcademicCalenderID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Std_AcademicCalender
WHERE     (StdAcademicCalenderID = @StdAcademicCalenderID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Std_AcademicCalenderInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Std_AcademicCalenderInsert] 
(
@StdAcademicCalenderID int  OUTPUT,
@StudentID int  = NULL,
@AcademicCalenderID int  = NULL,
@Description varchar(200) = NULL,
@RegStatusType bit = NULL,
@CGPA numeric(18, 2) = NULL,
@GPA numeric(18, 2) = NULL,
@TotalCreditsPerCalender numeric(18, 2) = NULL,
@TotalCreditsComleted numeric(18, 2) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Std_AcademicCalender]
(
[StdAcademicCalenderID],
[StudentID],
[AcademicCalenderID],
[Description],
[RegStatusType],
[CGPA],
[GPA],
[TotalCreditsPerCalender],
[TotalCreditsComleted],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@StdAcademicCalenderID,
@StudentID,
@AcademicCalenderID,
@Description,
@RegStatusType,
@CGPA,
@GPA,
@TotalCreditsPerCalender,
@TotalCreditsComleted,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @StdAcademicCalenderID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Std_AcademicCalenderUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Std_AcademicCalenderUpdate]
(
@StdAcademicCalenderID int  = NULL,
@StudentID int  = NULL,
@AcademicCalenderID int  = NULL,
@Description varchar(200) = NULL,
@RegStatusType bit = NULL,
@CGPA numeric(18, 2) = NULL,
@GPA numeric(18, 2) = NULL,
@TotalCreditsPerCalender numeric(18, 2) = NULL,
@TotalCreditsComleted numeric(18, 2) = NULL,
@CreatedBy int = NULL,
@CreatedDate datetime = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Std_AcademicCalender]
   SET
[StudentID]	=	@StudentID,
[AcademicCalenderID]	=	@AcademicCalenderID,
[Description]	=	@Description,
[RegStatusType]	=	@RegStatusType,
[CGPA]	=	@CGPA,
[GPA]	=	@GPA,
[TotalCreditsPerCalender]	=	@TotalCreditsPerCalender,
[TotalCreditsComleted]	=	@TotalCreditsComleted,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE StdAcademicCalenderID = @StdAcademicCalenderID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StdEducationInfoDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_StdEducationInfoDeleteById]
(
@StdEducationInfoID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_StdEducationInfo]
WHERE StdEducationInfoID = @StdEducationInfoID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StdEducationInfoGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StdEducationInfoGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_StdEducationInfo


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StdEducationInfoGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StdEducationInfoGetById]
(
@StdEducationInfoID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_StdEducationInfo
WHERE     (StdEducationInfoID = @StdEducationInfoID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StdEducationInfoInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StdEducationInfoInsert] 
(
@StdEducationInfoID int  OUTPUT,
@DregreeName varchar(100)  = NULL,
@GroupName varchar(100) = NULL,
@InstitutionName varchar(200) = NULL,
@TotalMarks decimal(4, 2) = NULL,
@ObtainedMarks decimal(4, 2) = NULL,
@Division varchar(50) = NULL,
@TotalCGPA decimal(2, 2) = NULL,
@ObtainedCGPA decimal(2, 2) = NULL,
@StudentID int = NULL,
@AddressID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_StdEducationInfo]
(
[StdEducationInfoID],
[DregreeName],
[GroupName],
[InstitutionName],
[TotalMarks],
[ObtainedMarks],
[Division],
[TotalCGPA],
[ObtainedCGPA],
[StudentID],
[AddressID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@StdEducationInfoID,
@DregreeName,
@GroupName,
@InstitutionName,
@TotalMarks,
@ObtainedMarks,
@Division,
@TotalCGPA,
@ObtainedCGPA,
@StudentID,
@AddressID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @StdEducationInfoID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StdEducationInfoUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StdEducationInfoUpdate]
(
@StdEducationInfoID int  = NULL,
@DregreeName varchar(100)  = NULL,
@GroupName varchar(100) = NULL,
@InstitutionName varchar(200) = NULL,
@TotalMarks decimal(4, 2) = NULL,
@ObtainedMarks decimal(4, 2) = NULL,
@Division varchar(50) = NULL,
@TotalCGPA decimal(2, 2) = NULL,
@ObtainedCGPA decimal(2, 2) = NULL,
@StudentID int = NULL,
@AddressID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_StdEducationInfo]
   SET
[DregreeName]	=	@DregreeName,
[GroupName]	=	@GroupName,
[InstitutionName]	=	@InstitutionName,
[TotalMarks]	=	@TotalMarks,
[ObtainedMarks]	=	@ObtainedMarks,
[Division]	=	@Division,
[TotalCGPA]	=	@TotalCGPA,
[ObtainedCGPA]	=	@ObtainedCGPA,
[StudentID]	=	@StudentID,
[AddressID]	=	@AddressID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE StdEducationInfoID = @StdEducationInfoID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetail_Calculate_GPAandCGPA]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<Sajib, Ahmed>								--
--								Create date:	< 2014-02-13 >								--
--								Description:	<Software Eng.>								--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetail_Calculate_GPAandCGPA]
(
	@ProcessResult int Output,
	@Roll nvarchar(15) = NULL
)
AS
BEGIN
	Declare @StudentId Int, @Counter Int; Set @StudentId = 0; Set @Counter = 0;
	Select @StudentId = StudentID From UIUEMS_ER_Student Where Roll = @Roll;

	If @StudentId != 0
	Begin
		Declare @minAcaCalID Int, @maxAcaCalID Int;
		Select @minAcaCalID = Min(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @StudentId;
		Select @maxAcaCalID = Max(AcaCalID) From UIUEMS_CC_Student_CourseHistory Where StudentID = @StudentId;
		
		Declare @i Int;
		Set @i = @minAcaCalID;
		While @i <= @maxAcaCalID
		Begin
			Declare @flagFoundSemester Int;
			Set @flagFoundSemester = 0;
			Select @flagFoundSemester = Count(*) From UIUEMS_CC_Student_CourseHistory Where StudentID = @StudentId And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
			
			If @flagFoundSemester > 0
			Begin
				Declare @tempCredit Numeric(4,1), @tempCreditTemp numeric(3,1), @tempGPAPlusCredit Numeric(14,2), @tempGPA Numeric(18,2), @TempCGPA Numeric(18,2);
				Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCreditTemp = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @StudentId And AcaCalID = @i And ObtainedGPA Is Not Null and CourseCredit is not null;
				--IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
				Set @tempGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCreditTemp);
				
				--Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @tempStudentID And AcaCalID < (@i + 1) And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null and CourseCredit is not null;
				--Set @TempCGPA = Convert(numeric(18,2),@tempGPAPlusCredit / @tempCredit);
				Declare @Trimester Int, @StdId Int;
				Set @Trimester = @i; Set @StdId = @StudentId;
				set @tempCGPA = (select [dbo].[CGPACalculation](@Trimester, @StdId));
				
				Declare @FlagFound Int;
				Set @FlagFound = 0;
				Select @FlagFound = Count(*) From UIUEMS_ER_Student_ACUDetail Where StdAcademicCalenderID = @i and StudentID = @StudentId;
				
				If @FlagFound = 0
				Begin
					Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
					Values (@i, @StudentId, @tempCreditTemp, @TempCGPA, @tempGPA, 1, GetDate());
					Set @Counter = @Counter + 1;
				End
				Else If @FlagFound > 0
				Begin
					Update UIUEMS_ER_Student_ACUDetail Set CGPA = @TempCGPA, GPA = @tempGPA Where StdAcademicCalenderID = @i and StudentID = @StudentId;
					Set @Counter = @Counter + 1;
				End
				--Set @tempCredit = 1; Set @TempCGPA = 0; Set @tempGPA = 0;
			End
			
			Set @i += 1;
		End
		Set @ProcessResult = @Counter;
	End
	Else
	Begin
		Set @ProcessResult = 0;
	End
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_Student_ACUDetailDeleteById]
(
@StdACUDetailID int = null
)

AS
BEGIN
SET NOCOUNT ON;

DELETE FROM [UIUEMS_ER_Student_ACUDetail]
WHERE StdACUDetailID = @StdACUDetailID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetailGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_ACUDetail


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetailGetById]
(
@StdACUDetailID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_ACUDetail
WHERE     (StdACUDetailID = @StdACUDetailID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailGetLatestCGPAByStudentId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--								Author:			<Sajib, Ahmed>								--
--								Create date:	< 2014-02-02 >								--
--								Description:	<Softwar Eng.>								--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetailGetLatestCGPAByStudentId]
(
	@StudentId int = null
)

AS
BEGIN
SET NOCOUNT ON;

Select * 
From UIUEMS_ER_Student_ACUDetail 
Where StudentID = @StudentId and StdAcademicCalenderID = (Select MAX(StdAcademicCalenderID) From UIUEMS_ER_Student_ACUDetail Where StudentID = @StudentId);

END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetailInsert] 
(
@StdACUDetailID int   OUTPUT,
@StdAcademicCalenderID int  = NULL,
@StudentID int  = NULL,
@StatusTypeID int = NULL,
@SchSetUpID int = NULL,
@Credit numeric(3, 1) = NULL,
@CGPA money = NULL,
@GPA money = NULL,
@Description varchar(200) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Student_ACUDetail]
(
[StdACUDetailID],
[StdAcademicCalenderID],
[StudentID],
[StatusTypeID],
[SchSetUpID],
[Credit],
[CGPA],
[GPA],
[Description],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@StdACUDetailID,
@StdAcademicCalenderID,
@StudentID,
@StatusTypeID,
@SchSetUpID,
@Credit,
@CGPA,
@GPA,
@Description,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate
)
           
SET @StdACUDetailID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetailUpdate] 
(
@StdACUDetailID int   = NULL,
@StdAcademicCalenderID int  = NULL,
@StudentID int  = NULL,
@StatusTypeID int = NULL,
@SchSetUpID int = NULL,
@Credit numeric(3, 1) = NULL,
@CGPA money = NULL,
@GPA money = NULL,
@Description varchar(200) = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL
)

AS
BEGIN
SET NOCOUNT off;

UPDATE [UIUEMS_ER_Student_ACUDetail]
   SET


[StdAcademicCalenderID]	=	@StdAcademicCalenderID,
[StudentID]	=	@StudentID,
[StatusTypeID]	=	@StatusTypeID,
[SchSetUpID]	=	@SchSetUpID,
[Credit]	=	@Credit,
[CGPA]	=	@CGPA,
[GPA]	=	@GPA,
[Description]	=	@Description,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE StdACUDetailID = @StdACUDetailID
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_ACUDetailUpdateByAcaCalRoll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_ACUDetailUpdateByAcaCalRoll]
(
@result int output,
@studentId int = null,
@acaCalId int = null
)
AS
BEGIN
	Declare @tempCredit Numeric(3,1), @tempGPAPlusCredit Money, @tempGPA Money, @TempCGPA Money;
	Declare @previousCredit Numeric(3,1), @previousCGPA Money;
	
	Select @tempGPAPlusCredit = Sum(ObtainedGPA * CourseCredit), @tempCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @studentId And AcaCalID = @acaCalId And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
	
	Select @previousCredit = Sum(CourseCredit) From UIUEMS_CC_Student_CourseHistory Where StudentID = @studentId And AcaCalID < @acaCalId And IsConsiderGPA = 'True' And ObtainedGPA Is Not Null;
	Select @previousCGPA = CGPA From UIUEMS_ER_Student_ACUDetail Where StudentID = @studentId And StdAcademicCalenderID = @acaCalId;
	
	Set @tempGPA = @tempGPAPlusCredit / @tempCredit;
	Set @TempCGPA = (@previousCGPA*@previousCredit + @tempGPAPlusCredit) / (@tempCredit + @previousCredit);
	
	Declare @StdACUDetailID int;
	Set @StdACUDetailID = 0;
	Select @StdACUDetailID = StdACUDetailID From UIUEMS_ER_Student_ACUDetail Where StdACUDetailID = @studentId and StdAcademicCalenderID = @acaCalId;
	
	If @StdACUDetailID = 0
	Begin
		Insert Into UIUEMS_ER_Student_ACUDetail (StdAcademicCalenderID, StudentID, Credit, CGPA, GPA, CreatedBy, CreatedDate)
		Values (@acaCalId, @studentId, @tempCredit, @TempCGPA, @tempGPA, 1, GetDate());
	End
	else if @StdACUDetailID > 0
	Begin
		Update UIUEMS_ER_Student_ACUDetail Set Credit = @tempCredit, CGPA = @TempCGPA, GPA = @tempGPA Where StdACUDetailID = @StdACUDetailID;
		Set @StdACUDetailID = 0;
	End
	SET @result = 1;
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_CourseDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_Student_CourseDeleteById]
(
@Student_CourseID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Student_Course]
WHERE Student_CourseID = @Student_CourseID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_CourseGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_CourseGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_Course


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_CourseGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_CourseGetById]
(
@Student_CourseID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_Course
WHERE     (Student_CourseID = @Student_CourseID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_CourseInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_CourseInsert] 
(
@Student_CourseID int  OUTPUT,
@StudentID int  = NULL,
@StdAcademicCalenderID int  = NULL,
@DscntSetUpID int = NULL,
@RetakeNo int = NULL,
@Node_CourseID int  = NULL,
@CourseID int = NULL,
@VersionID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Student_Course]
(
[Student_CourseID],
[StudentID],
[StdAcademicCalenderID],
[DscntSetUpID],
[RetakeNo],
[Node_CourseID],
[CourseID],
[VersionID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@Student_CourseID,
@StudentID,
@StdAcademicCalenderID,
@DscntSetUpID,
@RetakeNo,
@Node_CourseID,
@CourseID,
@VersionID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @Student_CourseID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_CourseUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_CourseUpdate]
(
@Student_CourseID int  = NULL,
@StudentID int  = NULL,
@StdAcademicCalenderID int  = NULL,
@DscntSetUpID int = NULL,
@RetakeNo int = NULL,
@Node_CourseID int  = NULL,
@CourseID int = NULL,
@VersionID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Student_Course]
   SET
[StudentID]	=	@StudentID,
[StdAcademicCalenderID]	=	@StdAcademicCalenderID,
[DscntSetUpID]	=	@DscntSetUpID,
[RetakeNo]	=	@RetakeNo,
[Node_CourseID]	=	@Node_CourseID,
[CourseID]	=	@CourseID,
[VersionID]	=	@VersionID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE Student_CourseID = @Student_CourseID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_Insert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author		<Sajib, Ahmed>
-- Create date	< 2013-05-21 >
-- Description	<Softwar Engr>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_Insert]
(
	@StudentID int Output,
	@Roll nvarchar(15) = NULL,
	@ProgramID int = NULL,
	@TotalDue money = NULL,
	@TotalPaid money = NULL,
	@Balance money = NULL,
	@TuitionSetUpID int = NULL,
	@WaiverSetUpID int = NULL,
	@DiscountSetUpID int = NULL,
	@RelationTypeID int = NULL,
	@RelativeID int = NULL,
	@TreeMasterID int = NULL,
	@Major1NodeID int = NULL,
	@Major2NodeID int = NULL,
	@Major3NodeID int = NULL,
	@Minor1NodeID int = NULL,
	@Minor2NodeID int = NULL,
	@Minor3NodeID int = NULL,
	@CreatedBy int = NULL,
	@CreatedDate datetime = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@PersonID int = NULL,
	@PaymentSlNo nvarchar(50) = NULL,
	@IsActive bit = NULL,
	@IsDeleted bit = NULL,
	@IsDiploma bit = NULL,
	@Remarks varchar(500) = NULL,
	@AccountHeadsID int = NULL,
	@CandidateId int = NULL,
	@IsProvisionalAdmission bit = NULL,
	@ValidUptoProAdmissionDate datetime = NULL,
	@Pre_English bit = NULL,
	@Pre_Math bit = NULL,
	@History nvarchar(MAX) = NULL,
	@Attribute1 nvarchar(500) = NULL,
	@Attribute2 nvarchar(500) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

Insert Into [dbo].[UIUEMS_ER_Student]
(
	[Roll],
	[ProgramID],
	[TotalDue],
	[TotalPaid],
	[Balance],
	[TuitionSetUpID],
	[WaiverSetUpID],
	[DiscountSetUpID],
	[RelationTypeID],
	[RelativeID],
	[TreeMasterID],
	[Major1NodeID],
	[Major2NodeID],
	[Major3NodeID],
	[Minor1NodeID],
	[Minor2NodeID],
	[Minor3NodeID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[PersonID],
	[PaymentSlNo],
	[IsActive],
	[IsDeleted],
	[IsDiploma],
	[Remarks],
	[AccountHeadsID],
	[CandidateId],
	[IsProvisionalAdmission],
	[ValidUptoProAdmissionDate],
	[Pre_English],
	[Pre_Math],
	[History],
	[Attribute1],
	[Attribute2]
)
 VALUES
(
	@Roll,
	@ProgramID,
	@TotalDue,
	@TotalPaid,
	@Balance,
	@TuitionSetUpID,
	@WaiverSetUpID,
	@DiscountSetUpID,
	@RelationTypeID,
	@RelativeID,
	@TreeMasterID,
	@Major1NodeID,
	@Major2NodeID,
	@Major3NodeID,
	@Minor1NodeID,
	@Minor2NodeID,
	@Minor3NodeID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@PersonID,
	@PaymentSlNo,
	@IsActive,
	@IsDeleted,
	@IsDiploma,
	@Remarks,
	@AccountHeadsID,
	@CandidateId,
	@IsProvisionalAdmission,
	@ValidUptoProAdmissionDate,
	@Pre_English,
	@Pre_Math,
	@History,
	@Attribute1,
	@Attribute2
)           
SET @StudentID = SCOPE_IDENTITY()
END


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_OldDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_Student_OldDeleteById]
(
@StudentID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Student_Old]
WHERE StudentID = @StudentID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_OldGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_OldGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*
FROM       UIUEMS_ER_Student_Old


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_OldGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_OldGetById]
(
@StudentID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_Old
WHERE     (StudentID = @StudentID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_OldInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_OldInsert] 
(
@StudentID int  OUTPUT,
@Roll nvarchar(15)  = NULL,
@Prefix int = NULL,
@FirstName varchar(100) = NULL,
@MiddleName varchar(100) = NULL,
@LastName varchar(100) = NULL,
@NickOrOtherName varchar(100) = NULL,
@DOB datetime = NULL,
@Gender int = NULL,
@MatrialStatus int = NULL,
@BloodGroup int = NULL,
@ReligionID int = NULL,
@NationalityID int = NULL,
@PhotoPath varchar(500) = NULL,
@ProgramID int = NULL,
@TotalDue money = NULL,
@TotalPaid money = NULL,
@Balance money = NULL,
@TuitionSetUpID int = NULL,
@WaiverSetUpID int = NULL,
@DiscountSetUpID int = NULL,
@RelationTypeID int = NULL,
@RelativeID int = NULL,
@TreeMasterID int = NULL,
@Major1NodeID int = NULL,
@Major2NodeID int = NULL,
@Major3NodeID int = NULL,
@Minor1NodeID int = NULL,
@Minor2NodeID int = NULL,
@Minor3NodeID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Student_Old]
(
[StudentID],
[Roll],
[Prefix],
[FirstName],
[MiddleName],
[LastName],
[NickOrOtherName],
[DOB],
[Gender],
[MatrialStatus],
[BloodGroup],
[ReligionID],
[NationalityID],
[PhotoPath],
[ProgramID],
[TotalDue],
[TotalPaid],
[Balance],
[TuitionSetUpID],
[WaiverSetUpID],
[DiscountSetUpID],
[RelationTypeID],
[RelativeID],
[TreeMasterID],
[Major1NodeID],
[Major2NodeID],
[Major3NodeID],
[Minor1NodeID],
[Minor2NodeID],
[Minor3NodeID],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@StudentID,
@Roll,
@Prefix,
@FirstName,
@MiddleName,
@LastName,
@NickOrOtherName,
@DOB,
@Gender,
@MatrialStatus,
@BloodGroup,
@ReligionID,
@NationalityID,
@PhotoPath,
@ProgramID,
@TotalDue,
@TotalPaid,
@Balance,
@TuitionSetUpID,
@WaiverSetUpID,
@DiscountSetUpID,
@RelationTypeID,
@RelativeID,
@TreeMasterID,
@Major1NodeID,
@Major2NodeID,
@Major3NodeID,
@Minor1NodeID,
@Minor2NodeID,
@Minor3NodeID,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @StudentID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_OldUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_OldUpdate]
(
@StudentID int  = NULL,
@Roll nvarchar(15)  = NULL,
@Prefix int = NULL,
@FirstName varchar(100) = NULL,
@MiddleName varchar(100) = NULL,
@LastName varchar(100) = NULL,
@NickOrOtherName varchar(100) = NULL,
@DOB datetime = NULL,
@Gender int = NULL,
@MatrialStatus int = NULL,
@BloodGroup int = NULL,
@ReligionID int = NULL,
@NationalityID int = NULL,
@PhotoPath varchar(500) = NULL,
@ProgramID int = NULL,
@TotalDue money = NULL,
@TotalPaid money = NULL,
@Balance money = NULL,
@TuitionSetUpID int = NULL,
@WaiverSetUpID int = NULL,
@DiscountSetUpID int = NULL,
@RelationTypeID int = NULL,
@RelativeID int = NULL,
@TreeMasterID int = NULL,
@Major1NodeID int = NULL,
@Major2NodeID int = NULL,
@Major3NodeID int = NULL,
@Minor1NodeID int = NULL,
@Minor2NodeID int = NULL,
@Minor3NodeID int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Student_Old]
   SET
[Roll]	=	@Roll,
[Prefix]	=	@Prefix,
[FirstName]	=	@FirstName,
[MiddleName]	=	@MiddleName,
[LastName]	=	@LastName,
[NickOrOtherName]	=	@NickOrOtherName,
[DOB]	=	@DOB,
[Gender]	=	@Gender,
[MatrialStatus]	=	@MatrialStatus,
[BloodGroup]	=	@BloodGroup,
[ReligionID]	=	@ReligionID,
[NationalityID]	=	@NationalityID,
[PhotoPath]	=	@PhotoPath,
[ProgramID]	=	@ProgramID,
[TotalDue]	=	@TotalDue,
[TotalPaid]	=	@TotalPaid,
[Balance]	=	@Balance,
[TuitionSetUpID]	=	@TuitionSetUpID,
[WaiverSetUpID]	=	@WaiverSetUpID,
[DiscountSetUpID]	=	@DiscountSetUpID,
[RelationTypeID]	=	@RelationTypeID,
[RelativeID]	=	@RelativeID,
[TreeMasterID]	=	@TreeMasterID,
[Major1NodeID]	=	@Major1NodeID,
[Major2NodeID]	=	@Major2NodeID,
[Major3NodeID]	=	@Major3NodeID,
[Minor1NodeID]	=	@Minor1NodeID,
[Minor2NodeID]	=	@Minor2NodeID,
[Minor3NodeID]	=	@Minor3NodeID,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE StudentID = @StudentID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_SkillTypeDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_Student_SkillTypeDeleteById]
(
@Std_SkillTypeID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Student_SkillType]
WHERE Std_SkillTypeID = @Std_SkillTypeID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_SkillTypeGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_SkillTypeGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_SkillType


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_SkillTypeGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_SkillTypeGetById]
(
@Std_SkillTypeID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Student_SkillType
WHERE     (Std_SkillTypeID = @Std_SkillTypeID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_SkillTypeInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_SkillTypeInsert] 
(
	@Std_SkillTypeID int  OUTPUT,
	@SkillTypeID int  = NULL,
	@StudentID int  = NULL,
	@Description varchar(200) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Student_SkillType]
(
	[Std_SkillTypeID],
	[SkillTypeID],
	[StudentID],
	[Description],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@Std_SkillTypeID,
	@SkillTypeID,
	@StudentID,
	@Description,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @Std_SkillTypeID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_Student_SkillTypeUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_Student_SkillTypeUpdate]
(
	@Std_SkillTypeID int  = NULL,
	@SkillTypeID int  = NULL,
	@StudentID int  = NULL,
	@Description varchar(200) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Student_SkillType]
   SET


	[SkillTypeID]	=	@SkillTypeID,
	[StudentID]	=	@StudentID,
	[Description]	=	@Description,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE Std_SkillTypeID = @Std_SkillTypeID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentBlockCountByProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentBlockCountByProgram]

AS
BEGIN
SET NOCOUNT ON;

SELECT COUNT( s.StudentID) AS StudentCount, p.ProgramID, p.ShortName, p.DetailName , p.Code FROM UIUEMS_ER_Student AS s 
INNER JOIN UIUEMS_ER_PersonBlock AS pb ON pb.PersonId=s.PersonID
RIGHT OUTER JOIN UIUEMS_CC_Program AS p ON p.ProgramID= s.ProgramID
GROUP BY p.ProgramID, p.ShortName, p.DetailName, p.Code
ORDER BY p.ProgramID

END

GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentBlockDeleteAllByProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentBlockDeleteAllByProgram]
@ProgramID int = NULL
AS

BEGIN
SET NOCOUNT ON;

DELETE  FROM UIUEMS_ER_PersonBlock WHERE PersonBlockId IN (
SELECT pb.PersonBlockId FROM UIUEMS_ER_Student AS s 
INNER JOIN UIUEMS_ER_PersonBlock AS pb ON pb.PersonId=s.PersonID
WHERE s.ProgramID = @ProgramID)

END

GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_StudentDeleteById]
(
@StudentID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Student]
WHERE StudentID = @StudentID

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     
*

FROM       UIUEMS_ER_Student


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAllActiveInactiveWithRegistrationStatus]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetAllActiveInactiveWithRegistrationStatus]
(
	@ProgramID  int  = 0,
	@BatchID int = 0,
	@AcaCalID int= 0,
	@StudentRoll nvarchar(50) = ''
)
AS
  Declare @query nvarchar(500)

  declare @F_W int, @F_And int;
  set @F_W=0;
  set @F_And=0; 
  
SET @query = 'select s.* from UIUEMS_ER_Student as s inner join    UIUEMS_ER_Admission as a on s.StudentID = a.StudentID
   full outer join (select distinct studentid, AcaCalID from UIUEMS_CC_Student_CourseHistory) as ch on ch.StudentID = s.StudentID'

if(@ProgramID > 0)
BEGIN
	if(@F_W = 0)
		BEGIN
			set @query = @query + ' WHERE s.ProgramID = ' + Cast(@ProgramID as nvarchar(50))

			set @F_W = 1
		END
	else if(@F_W = 1)
		BEGIN
			set @query = @query + ' AND s.ProgramID = ' + Cast(@ProgramID as nvarchar(50))
		END
END

if(@BatchID > 0)
BEGIN
	if(@F_W = 0)
		BEGIN
			set @query = @query + ' WHERE a.AdmissionCalenderID = ' + Cast(@BatchID as nvarchar(50))

			set @F_W = 1
		END
	else if(@F_W = 1)
		BEGIN
			set @query = @query + ' AND a.AdmissionCalenderID = ' + Cast(@BatchID as nvarchar(50))
		END
END

if(@AcaCalID > 0)
BEGIN
	if(@F_W = 0)
		BEGIN
			set @query = @query + ' WHERE ch.AcaCalID = ' + Cast(@AcaCalID as nvarchar(50))

			set @F_W = 1
		END
	else if(@F_W = 1)
		BEGIN
			set @query = @query + ' AND ch.AcaCalID = ' + Cast(@AcaCalID as nvarchar(50))
		END
END

if(@StudentRoll != '')
BEGIN
	if(@F_W = 0)
		BEGIN
			set @query = @query + ' WHERE s.Roll like %' + Cast(@StudentRoll as nvarchar(50)) + '% '

			set @F_W = 1
		END
	else if(@F_W = 1)
		BEGIN
			set @query = @query + ' AND s.Roll like %' + Cast(@StudentRoll as nvarchar(50)) + '% '
		END
END

--print @query
Exec sp_executesql @query


GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAllByProbationStatus]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[UIUEMS_ER_StudentGetAllByProbationStatus]

@ProgramID int = null,
@BatchID int = null,
@MinProbation int = null,
@MaxProbation int = null

AS
BEGIN
SET NOCOUNT ON;

IF(@BatchID = 0)
BEGIN
set @BatchID = null;
END

select s.StudentID,s.Roll,  p.FirstName as Name, s.ProgramID,a.AdmissionCalenderID,ac.BatchCode,
case  when pb.PersonId is null then 0 else 1 end as isBlock, pb.Remarks, g.GPA, g.CGPA, ISNULL( sp.ProbationCount,0) as ProbationCount
from UIUEMS_ER_Student as s
left outer join UIUEMS_ER_Person as p on s.PersonID=p.PersonID
left outer join UIUEMS_ER_Admission as a on s.StudentID=a.StudentID
left outer join UIUEMS_CC_AcademicCalender as ac on a.AdmissionCalenderID=ac.AcademicCalenderID
left outer join UIUEMS_ER_PersonBlock as pb on s.PersonID= pb.PersonId
left outer join  (SELECT  pl1.* FROM   UIUEMS_CC_StudentProbrationList as pl1
				  INNER JOIN (select StudentId, max(AcaCalCode) as AcaCalCode from UIUEMS_CC_StudentProbrationList group by StudentId )  pl2
				  on pl1.StudentId=pl2.StudentId  and pl1.AcaCalCode=pl2.AcaCalCode ) as sp on s.StudentID = sp.StudentId
left outer join ( SELECT   ad1.* FROM   UIUEMS_ER_Student_ACUDetail as ad1
				INNER JOIN (select StudentId, max(StdAcademicCalenderID) as AcaCalCode from UIUEMS_ER_Student_ACUDetail group by StudentId )  ad2
				on ad1.StudentId=ad2.StudentId  and ad1.StdAcademicCalenderID=ad2.AcaCalCode) as g on g.StudentID=s.StudentID

where s.ProgramID = @ProgramID
and (a.AdmissionCalenderID = @BatchID or @BatchID is null)
and (ISNULL( sp.ProbationCount,0) >= @MinProbation or @MinProbation is null)
and (ISNULL( sp.ProbationCount,0) <= @MaxProbation or @MaxProbation is null) 

order by s.Roll

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAllByProgAdminCalID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetAllByProgAdminCalID]
(
@ProgramID int = NULL,
@AcademicCalenderID int = NULL
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

s.*

FROM	UIUEMS_ER_Student s, UIUEMS_ER_Admission a
WHERE	a.AdmissionCalenderID = @AcademicCalenderID and s.StudentID = a.StudentID and s.ProgramID = @ProgramID


END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAllFromRegWorksheetByProgramAndBatch]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetAllFromRegWorksheetByProgramAndBatch]
(
@ProgramID int = NULL,
@AcademicCalenderID int = NULL
)

AS
BEGIN
SET NOCOUNT ON;


SELECT
s.*
FROM	UIUEMS_ER_Student as s 
INNER JOIN UIUEMS_ER_Admission as a on	a.AdmissionCalenderID = @AcademicCalenderID AND 
									s.StudentID = a.StudentID AND 
									s.ProgramID = @ProgramID
INNER JOIN (select  distinct(StudentID) from UIUEMS_CC_RegistrationWorksheet 
			where AcademicCalenderID = @AcademicCalenderID ) as rw  on s.StudentID = rw.StudentID 

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAllFromRegWorksheetByStudentRoll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetAllFromRegWorksheetByStudentRoll]
(
@StudentRoll nvarchar(100) = NULL 
)

AS
BEGIN
SET NOCOUNT ON;


SELECT
s.*
FROM	UIUEMS_ER_Student as s 
INNER JOIN UIUEMS_ER_Admission as a on	s.StudentID = a.StudentID  
INNER JOIN (select  distinct(StudentID) from UIUEMS_CC_RegistrationWorksheet ) as rw  on s.StudentID = rw.StudentID 
where s.Roll like @StudentRoll+'%'

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetAllRegisteredStudentBySession]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetAllRegisteredStudentBySession]

@SessionId int = null

AS
BEGIN
SET NOCOUNT ON;

select * from UIUEMS_ER_Student as s inner join 
(select distinct StudentID, AcaCalID from UIUEMS_CC_Student_CourseHistory ) as ch on s.StudentID = ch.StudentID
where ch.AcaCalID = @SessionId 

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetById]
(
@StudentID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*
FROM       UIUEMS_ER_Student
WHERE     (StudentID = @StudentID)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetByPersonID]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetByPersonID]
(
	@PersonID int = null
)

AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_ER_Student Where PersonID = @PersonID;

END






GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetByProgramOrBatchOrRoll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetByProgramOrBatchOrRoll]
(
@pid int = null, 
@acid int  = null, 
@roll nvarchar(10)  = null
)

AS
BEGIN
SET NOCOUNT ON;

Declare @query nvarchar(1000),  @flag int;

set @query = ' select s.* from UIUEMS_ER_Student as s inner join UIUEMS_ER_Admission as sa on s.StudentID= sa.StudentID ';
set @flag = 0;

if(@pid != 0 and @flag = 0)
BEGIN
	set @query +=  ' Where s.ProgramID = ' + Cast(@pid as nvarchar(100))  ;
	set @flag = 1;
END

if(@acid != 0 and @flag = 0)
BEGIN
	set @query +=  ' Where sa.AdmissionCalenderID = ' + Cast(@acid as nvarchar(100))  ;
	set @flag = 1;
END

else if(@acid != 0 and @flag = 1)
BEGIN
	set @query +=  ' and sa.AdmissionCalenderID  = ' + Cast(@acid as nvarchar(100)) ;
	set @flag = 1;
END

if(@roll != '' and @flag = 0)
BEGIN
	set @query +=  ' Where Roll like  ''%' + Cast(@roll as nvarchar(100)) + '%''' ;
	set @flag = 1;
END

else if(@roll != '' and @flag = 1)
BEGIN
	set @query +=  ' and Roll like  ''%' + Cast(@roll as nvarchar(100)) + '%''' ;
	set @flag = 1;
END

--print @query
Exec sp_executesql @query

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetByProgramOrBatchOrRollRange]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetByProgramOrBatchOrRollRange]
(
@pid int = null, 
@acid int  = null, 
@rollFrom nvarchar(10)  = null,
@rollTo nvarchar(10)  = null
)

AS
BEGIN
SET NOCOUNT ON;

Declare @query nvarchar(MAX),  @flag int;

set @query = ' select s.* from UIUEMS_ER_Student as s inner join UIUEMS_ER_Admission as sa on s.StudentID= sa.StudentID ';
set @flag = 0;

if(@pid != 0 and @flag = 0)
BEGIN
	set @query +=  ' Where s.ProgramID = ' + Cast(@pid as nvarchar(100))  ;
	set @flag = 1;
END

if(@acid != 0 and @flag = 0)
BEGIN
	set @query +=  ' Where sa.AdmissionCalenderID = ' + Cast(@acid as nvarchar(100))  ;
	set @flag = 1;
END

else if(@acid != 0 and @flag = 1)
BEGIN
	set @query +=  ' and sa.AdmissionCalenderID  = ' + Cast(@acid as nvarchar(100)) ;
	set @flag = 1;
END

if(@rollFrom != '' and @flag = 0)
BEGIN
	set @query +=  ' Where Roll BETWEEN  ' + Cast(@rollFrom as nvarchar(100)) + ' AND ' + Cast(@rollTo as nvarchar(100)) ;
	set @flag = 1;
END

else if(@rollFrom != '' and @flag = 1)
BEGIN
	set @query +=  ' and Roll BETWEEN  ' + Cast(@rollFrom as nvarchar(100)) + ' AND ' + Cast(@rollTo as nvarchar(100)) ;
	set @flag = 1;
END

print @query
--Exec sp_executesql @query

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentGetByRoll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentGetByRoll]
(
@Roll nvarchar(100) = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     
*
FROM       UIUEMS_ER_Student
WHERE     (Roll = @Roll)

END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentInActiveCountByProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[UIUEMS_ER_StudentInActiveCountByProgram]

AS
BEGIN
SET NOCOUNT ON;

select count(s.StudentID) StudentCount, p.ProgramID, p.ShortName,p.DetailName,p.Code from UIUEMS_ER_Student as s
inner join UIUEMS_CC_Program as p on s.ProgramID = p.ProgramID
where s.IsActive=0
group by p.ProgramID ,p.ShortName,p.DetailName,p.Code
order by p.ProgramID

END

GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--		Author:			<	Md. Sajib Ahmed	>												--
--		Create date:	<	2014-02-05		>												--
--		Description:	<	Software Eng.	>												--
--		Modify(Add):	<	IsCompleted, CompletedAcaCalId, TranscriptSerial	>			--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentInsert] 
(
	@StudentID                  int output,
	@Roll                       nvarchar(15) = NULL,	
	@ProgramID                  int = NULL,
	@TotalDue                   money = NULL,
	@TotalPaid                  money = NULL,
	@Balance                    money = NULL,
	@TuitionSetUpID             int = NULL,
	@WaiverSetUpID              int = NULL,
	@DiscountSetUpID            int = NULL,
	@RelationTypeID             int = NULL,
	@RelativeID                 int = NULL,
	@TreeMasterID               int = NULL,
	@Major1NodeID               int = NULL,
	@Major2NodeID               int = NULL,
	@Major3NodeID               int = NULL,
	@Minor1NodeID               int = NULL,
	@Minor2NodeID               int = NULL,
	@Minor3NodeID               int = NULL,
	@CreatedBy                  int = NULL,
	@CreatedDate                datetime = NULL,
	@ModifiedBy                 int = NULL,
	@ModifiedDate               datetime = NULL,
	@PersonID                   int = NULL,
	@PaymentSlNo                nvarchar(50) = NULL,
	@IsActive                   bit = NULL,
	@IsDeleted                  bit = NULL,
	@IsDiploma                  bit = NULL,
	@Remarks                    varchar(500) = NULL,
	@AccountHeadsID             int = NULL,
	@CandidateId                int = NULL,
	@IsProvisionalAdmission     bit = NULL,
	@ValidUptoProAdmissionDate  datetime = NULL,
	@Pre_English                bit = NULL,
	@Pre_Math					bit = NULL,
	@History					nvarchar(MAX) = NULL,
	@Attribute1					nvarchar(500) = NULL,
	@Attribute2					nvarchar(500) = NULL,
	@TreeCalendarMasterID		int = NULL,
	@IsCompleted				bit = NULL,
	@CompletedAcaCalId			int = NULL,
	@TranscriptSerial			nvarchar(12) = NULL
)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS.2.0.0].[dbo].[UIUEMS_ER_Student]
(
	[Roll],	
	[ProgramID],
	[TotalDue],
	[TotalPaid],
	[Balance],
	[TuitionSetUpID],
	[WaiverSetUpID],
	[DiscountSetUpID],
	[RelationTypeID],
	[RelativeID],
	[TreeMasterID],
	[Major1NodeID],
	[Major2NodeID],
	[Major3NodeID],
	[Minor1NodeID],
	[Minor2NodeID],
	[Minor3NodeID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate],
	[PersonID],
	[PaymentSlNo],
	[IsActive],
	[IsDeleted],
	[IsDiploma],
	[Remarks],
	[AccountHeadsID],
	[CandidateId],
	[IsProvisionalAdmission],
	[ValidUptoProAdmissionDate],
	[Pre_English],
	[Pre_Math],
	[History],
	[Attribute1],
	[Attribute2],
	[TreeCalendarMasterID],
	[IsCompleted],
	[CompletedAcaCalId],
	[TranscriptSerial]
)
 VALUES
(
	@Roll,	
	@ProgramID,
	@TotalDue,
	@TotalPaid,
	@Balance,
	@TuitionSetUpID,
	@WaiverSetUpID,
	@DiscountSetUpID,
	@RelationTypeID,
	@RelativeID,
	@TreeMasterID,
	@Major1NodeID,
	@Major2NodeID,
	@Major3NodeID,
	@Minor1NodeID,
	@Minor2NodeID,
	@Minor3NodeID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate,
	@PersonID,
	@PaymentSlNo,
	@IsActive,
	@IsDeleted,
	@IsDiploma,
	@Remarks,
	@AccountHeadsID,
	@CandidateId,
	@IsProvisionalAdmission,
	@ValidUptoProAdmissionDate,
	@Pre_English,
	@Pre_Math,
	@History,
	@Attribute1,
	@Attribute2,
	@TreeCalendarMasterID,
	@IsCompleted,
	@CompletedAcaCalId,
	@TranscriptSerial
)
           
SET @StudentID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_StudentUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ========================================================================================	--
--		Author:			<	Md. Sajib Ahmed	>												--
--		Create date:	<	2014-02-05		>												--
--		Description:	<	Software Eng.	>												--
--		Modify(Add):	<	IsCompleted, CompletedAcaCalId, TranscriptSerial	>			--
-- ========================================================================================	--
CREATE PROCEDURE [dbo].[UIUEMS_ER_StudentUpdate]
(
	@StudentID int  = NULL,
	@Roll nvarchar(15)  = NULL,
	@ProgramID int = NULL,
	@TotalDue money = NULL,
	@TotalPaid money = NULL,
	@Balance money = NULL,
	@TuitionSetUpID int = NULL,
	@WaiverSetUpID int = NULL,
	@DiscountSetUpID int = NULL,
	@RelationTypeID int = NULL,
	@RelativeID int = NULL,
	@TreeMasterID int = NULL,
	@Major1NodeID int = NULL,
	@Major2NodeID int = NULL,
	@Major3NodeID int = NULL,
	@Minor1NodeID int = NULL,
	@Minor2NodeID int = NULL,
	@Minor3NodeID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL,
	@PersonID int = NULL,
	@PaymentSlNo nvarchar(50) = NULL,
	@IsActive bit = NULL,
	@IsDeleted bit = NULL,
	@IsDiploma bit = NULL,
	@Remarks varchar(500) = NULL,
	@AccountHeadsID int = NULL,
	@CandidateId int = NULL,
	@IsProvisionalAdmission bit = NULL,
	@ValidUptoProAdmissionDate datetime = NULL,
	@Pre_English bit = NULL,
	@Pre_Math bit = NULL,
	@History nvarchar(MAX) = NULL,
	@Attribute1 nvarchar(500) = NULL,
	@Attribute2 nvarchar(500) = NULL,
	@TreeCalendarMasterID int = NULL,
	@IsCompleted bit = NULL,
	@CompletedAcaCalId int = NULL,
	@TranscriptSerial nvarchar(12) = NULL
)

AS
BEGIN
SET NOCOUNT OFF;
If @ValidUptoProAdmissionDate is null
	set @ValidUptoProAdmissionDate = '';
UPDATE [UIUEMS_ER_Student]
   SET
	[Roll]	=	@Roll,
	[ProgramID]	=	@ProgramID,
	[TotalDue]	=	@TotalDue,
	[TotalPaid]	=	@TotalPaid,
	[Balance]	=	@Balance,
	[TuitionSetUpID]	=	@TuitionSetUpID,
	[WaiverSetUpID]	=	@WaiverSetUpID,
	[DiscountSetUpID]	=	@DiscountSetUpID,
	[RelationTypeID]	=	@RelationTypeID,
	[RelativeID]	=	@RelativeID,
	[TreeMasterID]	=	@TreeMasterID,
	[Major1NodeID]	=	@Major1NodeID,
	[Major2NodeID]	=	@Major2NodeID,
	[Major3NodeID]	=	@Major3NodeID,
	[Minor1NodeID]	=	@Minor1NodeID,
	[Minor2NodeID]	=	@Minor2NodeID,
	[Minor3NodeID]	=	@Minor3NodeID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate,
	[PersonID]	=	@PersonID,
	[PaymentSlNo]	=	@PaymentSlNo,
	[IsActive]	=	@IsActive,
	[IsDeleted]	=	@IsDeleted,
	[IsDiploma]	=	@IsDiploma,
	[Remarks]	=	@Remarks,
	[AccountHeadsID]	=	@AccountHeadsID,
	[CandidateId]	=	@CandidateId,
	[IsProvisionalAdmission]	=	@IsProvisionalAdmission,
	[ValidUptoProAdmissionDate]	=	@ValidUptoProAdmissionDate,
	[Pre_English]	=	@Pre_English,
	[Pre_Math]	=	@Pre_Math,
	[History] = @History,
	[Attribute1] = @Attribute1,
	[Attribute2] = @Attribute2,
	[TreeCalendarMasterID] = @TreeCalendarMasterID,
	IsCompleted = @IsCompleted,
	[CompletedAcaCalId] = @CompletedAcaCalId,
	[TranscriptSerial]  = @TranscriptSerial

WHERE StudentID = @StudentID
           
END





GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_ValueDeleteById]
(
@ValueID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_Value]
WHERE ValueID = @ValueID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Value


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueGetById]
(
@ValueID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_Value
WHERE     (ValueID = @ValueID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueGetByValueSetId]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Sajib, Ahmed>
-- Create date: < 2013-11-11 >
-- Description:	<Softwar Eng.>
-- =============================================
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueGetByValueSetId]
(
	@ValueSetID int = null
)

AS
BEGIN
SET NOCOUNT ON;

Select * From UIUEMS_ER_Value Where ValueSetID = @ValueSetID;

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueInsert] 
(
@ValueID int   OUTPUT,
@ValueName nvarchar(100)  = NULL,
@ValueCode nvarchar(50) = NULL,
@ValueSetID int = NULL,
@ParentValueID int = NULL,
@Remarks nvarchar(1000) = NULL,
@Attribute1 nvarchar(150) = NULL,
@Attribute2 nvarchar(150) = NULL,
@Attribute3 nvarchar(150) = NULL,
@CreatedBy nvarchar(50)  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy nvarchar(50)  = NULL,
@ModifiedDate datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_Value]
(
[ValueID],
[ValueName],
[ValueCode],
[ValueSetID],
[ParentValueID],
[Remarks],
[Attribute1],
[Attribute2],
[Attribute3],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@ValueID,
@ValueName,
@ValueCode,
@ValueSetID,
@ParentValueID,
@Remarks,
@Attribute1,
@Attribute2,
@Attribute3,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @ValueID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueSetDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_ER_ValueSetDeleteById]
(
@ValueSetID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_ER_ValueSet]
WHERE ValueSetID = @ValueSetID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueSetGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueSetGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_ValueSet


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueSetGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueSetGetById]
(
@ValueSetID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_ER_ValueSet
WHERE     (ValueSetID = @ValueSetID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueSetInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueSetInsert] 
(
@ValueSetID int   OUTPUT,
@ValueSetName nvarchar(100)  = NULL,
@ValueSetCode nvarchar(50) = NULL,
@Remarks nvarchar(1000) = NULL,
@Attribute1 nvarchar(150) = NULL,
@Attribute2 nvarchar(150) = NULL,
@CreatedBy nvarchar(50)  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy nvarchar(50)  = NULL,
@ModifiedDate datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_ER_ValueSet]
(
[ValueSetID],
[ValueSetName],
[ValueSetCode],
[Remarks],
[Attribute1],
[Attribute2],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate]

)
 VALUES
(
@ValueSetID,
@ValueSetName,
@ValueSetCode,
@Remarks,
@Attribute1,
@Attribute2,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate

)
           
SET @ValueSetID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueSetUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueSetUpdate]
(
@ValueSetID int   = NULL,
@ValueSetName nvarchar(100)  = NULL,
@ValueSetCode nvarchar(50) = NULL,
@Remarks nvarchar(1000) = NULL,
@Attribute1 nvarchar(150) = NULL,
@Attribute2 nvarchar(150) = NULL,
@CreatedBy nvarchar(50)  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy nvarchar(50)  = NULL,
@ModifiedDate datetime  = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_ValueSet]
   SET

[ValueSetName]	=	@ValueSetName,
[ValueSetCode]	=	@ValueSetCode,
[Remarks]	=	@Remarks,
[Attribute1]	=	@Attribute1,
[Attribute2]	=	@Attribute2,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate


WHERE ValueSetID = @ValueSetID
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_ER_ValueUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_ER_ValueUpdate]
(
@ValueID int   = NULL,
@ValueName nvarchar(100)  = NULL,
@ValueCode nvarchar(50) = NULL,
@ValueSetID int = NULL,
@ParentValueID int = NULL,
@Remarks nvarchar(1000) = NULL,
@Attribute1 nvarchar(150) = NULL,
@Attribute2 nvarchar(150) = NULL,
@Attribute3 nvarchar(150) = NULL,
@CreatedBy nvarchar(50)  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy nvarchar(50)  = NULL,
@ModifiedDate datetime  = NULL
)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_ER_Value]
   SET

[ValueName]	=	@ValueName,
[ValueCode]	=	@ValueCode,
[ValueSetID]	=	@ValueSetID,
[ParentValueID]	=	@ParentValueID,
[Remarks]	=	@Remarks,
[Attribute1]	=	@Attribute1,
[Attribute2]	=	@Attribute2,
[Attribute3]	=	@Attribute3,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate

WHERE ValueID = @ValueID
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_RG_DeptRegSetUpDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_RG_DeptRegSetUpDeleteById]
(
@DeptRegSetUpID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_RG_DeptRegSetUp]
WHERE DeptRegSetUpID = @DeptRegSetUpID

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_RG_DeptRegSetUpGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_RG_DeptRegSetUpGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_RG_DeptRegSetUp


END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_RG_DeptRegSetUpGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_RG_DeptRegSetUpGetById]
(
@DeptRegSetUpID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_RG_DeptRegSetUp
WHERE     (DeptRegSetUpID = @DeptRegSetUpID)

END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_RG_DeptRegSetUpInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_RG_DeptRegSetUpInsert] 
(
@DeptRegSetUpID int  OUTPUT,
@ProgramID int  = NULL,
@LocalCGPA1 money = NULL,
@LocalCredit1 money = NULL,
@LocalCGPA2 money = NULL,
@LocalCredit2 money = NULL,
@LocalCGPA3 money = NULL,
@LocalCredit3 money = NULL,
@ManCGPA1 money = NULL,
@ManCredit1 money = NULL,
@ManRetakeGradeLimit1 nvarchar(5) = NULL,
@ManCGPA2 money = NULL,
@ManCredit2 money = NULL,
@ManRetakeGradeLimit2 nvarchar(5) = NULL,
@ManCGPA3 money = NULL,
@ManCredit3 money = NULL,
@ManRetakeGradeLimit3 nvarchar(5) = NULL,
@MaxCGPA1 money = NULL,
@MaxCredit1 money = NULL,
@MaxCGPA2 money = NULL,
@MaxCredit2 money = NULL,
@MaxCGPA3 money = NULL,
@MaxCredit3 money = NULL,
@ProjectCGPA money = NULL,
@ProjectCredit money = NULL,
@ThesisCGPA money = NULL,
@ThesisCredit money = NULL,
@MajorCGPA money = NULL,
@MajorCredit money = NULL,
@ProbationLock int = NULL,
@CourseRetakeLimit int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@AutoPreRegCGPA1 money = NULL,
@AutoPreRegCredit1 money = NULL,
@AutoPreRegCGPA2 money = NULL,
@AutoPreRegCredit2 money = NULL,
@AutoPreRegCGPA3 money = NULL,
@AutoPreRegCredit3 money = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_RG_DeptRegSetUp]
(
[DeptRegSetUpID],
[ProgramID],
[LocalCGPA1],
[LocalCredit1],
[LocalCGPA2],
[LocalCredit2],
[LocalCGPA3],
[LocalCredit3],
[ManCGPA1],
[ManCredit1],
[ManRetakeGradeLimit1],
[ManCGPA2],
[ManCredit2],
[ManRetakeGradeLimit2],
[ManCGPA3],
[ManCredit3],
[ManRetakeGradeLimit3],
[MaxCGPA1],
[MaxCredit1],
[MaxCGPA2],
[MaxCredit2],
[MaxCGPA3],
[MaxCredit3],
[ProjectCGPA],
[ProjectCredit],
[ThesisCGPA],
[ThesisCredit],
[MajorCGPA],
[MajorCredit],
[ProbationLock],
[CourseRetakeLimit],
[CreatedBy],
[CreatedDate],
[ModifiedBy],
[ModifiedDate],
[AutoPreRegCGPA1],
[AutoPreRegCredit1],
[AutoPreRegCGPA2],
[AutoPreRegCredit2],
[AutoPreRegCGPA3],
[AutoPreRegCredit3]

)
 VALUES
(
@DeptRegSetUpID,
@ProgramID,
@LocalCGPA1,
@LocalCredit1,
@LocalCGPA2,
@LocalCredit2,
@LocalCGPA3,
@LocalCredit3,
@ManCGPA1,
@ManCredit1,
@ManRetakeGradeLimit1,
@ManCGPA2,
@ManCredit2,
@ManRetakeGradeLimit2,
@ManCGPA3,
@ManCredit3,
@ManRetakeGradeLimit3,
@MaxCGPA1,
@MaxCredit1,
@MaxCGPA2,
@MaxCredit2,
@MaxCGPA3,
@MaxCredit3,
@ProjectCGPA,
@ProjectCredit,
@ThesisCGPA,
@ThesisCredit,
@MajorCGPA,
@MajorCredit,
@ProbationLock,
@CourseRetakeLimit,
@CreatedBy,
@CreatedDate,
@ModifiedBy,
@ModifiedDate,
@AutoPreRegCGPA1,
@AutoPreRegCredit1,
@AutoPreRegCGPA2,
@AutoPreRegCredit2,
@AutoPreRegCGPA3,
@AutoPreRegCredit3

)
           
SET @DeptRegSetUpID = SCOPE_IDENTITY()
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_RG_DeptRegSetUpUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_RG_DeptRegSetUpUpdate]
(
@DeptRegSetUpID int  = NULL,
@ProgramID int  = NULL,
@LocalCGPA1 money = NULL,
@LocalCredit1 money = NULL,
@LocalCGPA2 money = NULL,
@LocalCredit2 money = NULL,
@LocalCGPA3 money = NULL,
@LocalCredit3 money = NULL,
@ManCGPA1 money = NULL,
@ManCredit1 money = NULL,
@ManRetakeGradeLimit1 nvarchar(5) = NULL,
@ManCGPA2 money = NULL,
@ManCredit2 money = NULL,
@ManRetakeGradeLimit2 nvarchar(5) = NULL,
@ManCGPA3 money = NULL,
@ManCredit3 money = NULL,
@ManRetakeGradeLimit3 nvarchar(5) = NULL,
@MaxCGPA1 money = NULL,
@MaxCredit1 money = NULL,
@MaxCGPA2 money = NULL,
@MaxCredit2 money = NULL,
@MaxCGPA3 money = NULL,
@MaxCredit3 money = NULL,
@ProjectCGPA money = NULL,
@ProjectCredit money = NULL,
@ThesisCGPA money = NULL,
@ThesisCredit money = NULL,
@MajorCGPA money = NULL,
@MajorCredit money = NULL,
@ProbationLock int = NULL,
@CourseRetakeLimit int = NULL,
@CreatedBy int  = NULL,
@CreatedDate datetime  = NULL,
@ModifiedBy int = NULL,
@ModifiedDate datetime = NULL,
@AutoPreRegCGPA1 money = NULL,
@AutoPreRegCredit1 money = NULL,
@AutoPreRegCGPA2 money = NULL,
@AutoPreRegCredit2 money = NULL,
@AutoPreRegCGPA3 money = NULL,
@AutoPreRegCredit3 money = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_RG_DeptRegSetUp]
   SET

[ProgramID]	=	@ProgramID,
[LocalCGPA1]	=	@LocalCGPA1,
[LocalCredit1]	=	@LocalCredit1,
[LocalCGPA2]	=	@LocalCGPA2,
[LocalCredit2]	=	@LocalCredit2,
[LocalCGPA3]	=	@LocalCGPA3,
[LocalCredit3]	=	@LocalCredit3,
[ManCGPA1]	=	@ManCGPA1,
[ManCredit1]	=	@ManCredit1,
[ManRetakeGradeLimit1]	=	@ManRetakeGradeLimit1,
[ManCGPA2]	=	@ManCGPA2,
[ManCredit2]	=	@ManCredit2,
[ManRetakeGradeLimit2]	=	@ManRetakeGradeLimit2,
[ManCGPA3]	=	@ManCGPA3,
[ManCredit3]	=	@ManCredit3,
[ManRetakeGradeLimit3]	=	@ManRetakeGradeLimit3,
[MaxCGPA1]	=	@MaxCGPA1,
[MaxCredit1]	=	@MaxCredit1,
[MaxCGPA2]	=	@MaxCGPA2,
[MaxCredit2]	=	@MaxCredit2,
[MaxCGPA3]	=	@MaxCGPA3,
[MaxCredit3]	=	@MaxCredit3,
[ProjectCGPA]	=	@ProjectCGPA,
[ProjectCredit]	=	@ProjectCredit,
[ThesisCGPA]	=	@ThesisCGPA,
[ThesisCredit]	=	@ThesisCredit,
[MajorCGPA]	=	@MajorCGPA,
[MajorCredit]	=	@MajorCredit,
[ProbationLock]	=	@ProbationLock,
[CourseRetakeLimit]	=	@CourseRetakeLimit,
[CreatedBy]	=	@CreatedBy,
[CreatedDate]	=	@CreatedDate,
[ModifiedBy]	=	@ModifiedBy,
[ModifiedDate]	=	@ModifiedDate,
[AutoPreRegCGPA1]	=	@AutoPreRegCGPA1,
[AutoPreRegCredit1]	=	@AutoPreRegCredit1,
[AutoPreRegCGPA2]	=	@AutoPreRegCGPA2,
[AutoPreRegCredit2]	=	@AutoPreRegCredit2,
[AutoPreRegCGPA3]	=	@AutoPreRegCGPA3,
[AutoPreRegCredit3]	=	@AutoPreRegCredit3


WHERE DeptRegSetUpID = @DeptRegSetUpID
           
END



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdAdmissionDiscountDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_SM_StdAdmissionDiscountDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_SM_StdAdmissionDiscount]
WHERE ID = @ID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdAdmissionDiscountGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdAdmissionDiscountGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_SM_StdAdmissionDiscount


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdAdmissionDiscountGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdAdmissionDiscountGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_SM_StdAdmissionDiscount
WHERE     (ID = @ID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdAdmissionDiscountInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdAdmissionDiscountInsert] 
(
	@ID int  OUTPUT,
	@AdmissionID int  = NULL,
	@TypeDefID int  = NULL,
	@TypePercentage decimal(18, 0) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_SM_StdAdmissionDiscount]
(
	[ID],
	[AdmissionID],
	[TypeDefID],
	[TypePercentage],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@ID,
	@AdmissionID,
	@TypeDefID,
	@TypePercentage,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ID = SCOPE_IDENTITY()
END
	



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdAdmissionDiscountUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdAdmissionDiscountUpdate]
(
	@ID int  = NULL,
	@AdmissionID int  = NULL,
	@TypeDefID int  = NULL,
	@TypePercentage decimal(18, 0) = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_SM_StdAdmissionDiscount]
   SET

	[AdmissionID]	=	@AdmissionID,
	[TypeDefID]	=	@TypeDefID,
	[TypePercentage]	=	@TypePercentage,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE ID = @ID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountCurrentDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_SM_StdDiscountCurrentDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_SM_StdDiscountCurrent]
WHERE ID = @ID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountCurrentGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountCurrentGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_SM_StdDiscountCurrent


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountCurrentGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountCurrentGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_SM_StdDiscountCurrent
WHERE     (ID = @ID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountCurrentInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountCurrentInsert] 
(
	@ID int  OUTPUT,
	@AdmissionID int  = NULL,
	@TypeDefID int  = NULL,
	@TypePercentage decimal(18, 2)  = NULL,
	@EffectiveAcaCalID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_SM_StdDiscountCurrent]
(
	[ID],
	[AdmissionID],
	[TypeDefID],
	[TypePercentage],
	[EffectiveAcaCalID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@ID,
	@AdmissionID,
	@TypeDefID,
	@TypePercentage,
	@EffectiveAcaCalID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountCurrentUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountCurrentUpdate]
(
		@ID int  = NULL,
		@AdmissionID int  = NULL,
		@TypeDefID int  = NULL,
		@TypePercentage decimal(18, 2)  = NULL,
		@EffectiveAcaCalID int = NULL,
		@CreatedBy int  = NULL,
		@CreatedDate datetime  = NULL,
		@ModifiedBy int = NULL,
		@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_SM_StdDiscountCurrent]
   SET


	[AdmissionID]	=	@AdmissionID,
	[TypeDefID]	=	@TypeDefID,
	[TypePercentage]	=	@TypePercentage,
	[EffectiveAcaCalID]	=	@EffectiveAcaCalID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE ID = @ID
           
END
	



GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountHistoryDeleteById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE  [dbo].[UIUEMS_SM_StdDiscountHistoryDeleteById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT OFF;

DELETE FROM [UIUEMS_SM_StdDiscountHistory]
WHERE ID = @ID

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountHistoryGetAll]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountHistoryGetAll]


AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_SM_StdDiscountHistory


END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountHistoryGetById]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountHistoryGetById]
(
@ID int = null
)

AS
BEGIN
SET NOCOUNT ON;

SELECT     

*

FROM       UIUEMS_SM_StdDiscountHistory
WHERE     (ID = @ID)

END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountHistoryInsert]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountHistoryInsert] 
(
	@ID int  OUTPUT,
	@AdmissionID int  = NULL,
	@TypeDefID int  = NULL,
	@TypePercentage decimal(18, 2) = NULL,
	@EffectiveAcaCalID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

INSERT INTO [UIUEMS_SM_StdDiscountHistory]
(
	[ID],
	[AdmissionID],
	[TypeDefID],
	[TypePercentage],
	[EffectiveAcaCalID],
	[CreatedBy],
	[CreatedDate],
	[ModifiedBy],
	[ModifiedDate]

)
 VALUES
(
	@ID,
	@AdmissionID,
	@TypeDefID,
	@TypePercentage,
	@EffectiveAcaCalID,
	@CreatedBy,
	@CreatedDate,
	@ModifiedBy,
	@ModifiedDate

)
           
SET @ID = SCOPE_IDENTITY()
END




GO
/****** Object:  StoredProcedure [dbo].[UIUEMS_SM_StdDiscountHistoryUpdate]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[UIUEMS_SM_StdDiscountHistoryUpdate]
(
	@ID int  = NULL,
	@AdmissionID int  = NULL,
	@TypeDefID int  = NULL,
	@TypePercentage decimal(18, 2) = NULL,
	@EffectiveAcaCalID int = NULL,
	@CreatedBy int  = NULL,
	@CreatedDate datetime  = NULL,
	@ModifiedBy int = NULL,
	@ModifiedDate datetime = NULL

)

AS
BEGIN
SET NOCOUNT ON;

UPDATE [UIUEMS_SM_StdDiscountHistory]
   SET


	[AdmissionID]	=	@AdmissionID,
	[TypeDefID]	=	@TypeDefID,
	[TypePercentage]	=	@TypePercentage,
	[EffectiveAcaCalID]	=	@EffectiveAcaCalID,
	[CreatedBy]	=	@CreatedBy,
	[CreatedDate]	=	@CreatedDate,
	[ModifiedBy]	=	@ModifiedBy,
	[ModifiedDate]	=	@ModifiedDate


WHERE ID = @ID
           
END




GO
/****** Object:  StoredProcedure [dbo].[UpdateInActiveToActiveByProgram]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create PROCEDURE [dbo].[UpdateInActiveToActiveByProgram]
@ProgramID int = NULL
AS

BEGIN
SET NOCOUNT ON;

UPDATE UIUEMS_ER_Student SET IsActive = 1 WHERE StudentID 
IN (SELECT studentid FROM UIUEMS_ER_Student WHERE IsActive = 0 AND ProgramID= @ProgramID)

END

GO
/****** Object:  StoredProcedure [dbo].[usp_FinalGradeSubmission]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
CREATE PROCEDURE [dbo].[usp_FinalGradeSubmission] 	
	@AcademicCalenderID int,
	@CourseID int,
	@VersionID int,
	@StudentID int,
	@AcaCal_SectionID int
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @GradeSheetId int,
			@ObtainMarks numeric(18,2),
			@GradeId int,
			@SectionCount int;	
	-- ============================================= 
	SELECT  @GradeSheetId = GradeSheetId ,  @ObtainMarks = ObtainMarks, @GradeId = GradeId
	FROM    UIUEMS_CC_GradeSheet
	WHERE	AcademicCalenderID = @AcademicCalenderID and 
			CourseID = @CourseID and 
			VersionID = @VersionID and 
			StudentID = @StudentID and 
			AcaCal_SectionID = @AcaCal_SectionID
--	select @GradeSheetId,@ObtainMarks,@GradeId;
	-- =============================================
	UPDATE UIUEMS_CC_GradeSheet
	SET IsConflictWithRetake = 0      
	WHERE GradeSheetId = @GradeSheetId
	-- =============================================
	 UPDATE UIUEMS_CC_Student_CourseHistory
	 SET ObtainedTotalMarks = @ObtainMarks
	     ,GradeId = @GradeId    
	 WHERE AcaCalID = @AcademicCalenderID and 
			CourseID = @CourseID and 
			VersionID = @VersionID and 
			StudentID = @StudentID and 
			AcaCalSectionID = @AcaCal_SectionID
	-- =============================================
	set @SectionCount = (select (count(AcaCalSectionID))
	from UIUEMS_CC_Student_CourseHistory
	where StudentID = @StudentID and 
		  CourseID = @CourseID and 
		  VersionID = @VersionID and 
		  AcaCalSectionID != @AcaCal_SectionID)
--	select @SectionCount 
	-- =============================================   
	if(@SectionCount > 0)
		BEGIN
			UPDATE UIUEMS_CC_GradeSheet
			SET IsConflictWithRetake = 1      
			WHERE GradeSheetId = @GradeSheetId
		END
	-- =============================================
select 1	 
END





GO
/****** Object:  StoredProcedure [dbo].[usp_FirstBillGeneration]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Ashraf>
-- Create date: <27 Dec, 2011>
-- Description:	<First bill Generation>
-- =============================================
CREATE PROCEDURE [dbo].[usp_FirstBillGeneration]

@StdID int,
--@progID int,
@CreatorID int
	
AS
BEGIN
	TRUNCATE TABLE Temp_First_Bill_Generation 

---------------------------------------------------------------------------------------------------------
-- insert basic student info into Temp_First_Bill_Generation
--
insert into Temp_First_Bill_Generation
	(StudentID, GradeId, StdAccHeadID,	PcaAccHeadId , AcaCalSectionID, RetakeNo, CourseStatusID, AcaCalID, 
	CourseID,VersionID, Node_CourseID, NodeID, IsMultipleACUSpan)

SELECT	StudentID, GradeId,
		(select AccountsID from UIUEMS_AC_AccountHeads 
				where [Name] = (select roll from dbo.UIUEMS_ER_Student 
						where StudentID = @StdID)) AS StdAccHeadID,
		PcaAccHeadId = 137, --// Hard coded 'Per Credit Amount'
		AcaCalSectionID, isnull(RetakeNo,0) AS RetakeNo,CourseStatusID, AcaCalID, CourseID, 
		VersionID, Node_CourseID, NodeID, IsMultipleACUSpan		
FROM    UIUEMS_CC_Student_CourseHistory 
WHERE	(StudentID = @StdID) 
		and AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)
---------------------------------------------------------------------------------------------------------
--Global Data
declare 
@AcaCalID int,
@RetakeNo int,
@BillStartFromRetakeNo int, 
@CourseID int, 
@VersionID int, 
@GradeId int, 
@ProgramID int, 
@AdmissionCalenderID int,
@IsCreditCourse bit ,
@Credits int,
@RetakeDiscountPercentage decimal(18,2) ,
@PCA decimal(18,2) ,
@Fee decimal(18,2),

@count int,
@StdAccHeadID int, 
@PcaAccHeadId int,
@BillStartFromRetake int,
@v_SLNO int,
@Definition nvarchar(250);

---------------------------------------------------------------------------------------------------------
-- update corresponding "IsCreditCourse" and "Credits" value
--
declare		@Cursor_Cr CURSOR

set @Cursor_Cr = CURSOR FOR
	select  c.IsCreditCourse ,c.Credits, tmp.CourseID, tmp.VersionID
	from Temp_First_Bill_Generation as tmp
	INNER JOIN UIUEMS_CC_Course AS c ON c.CourseID = tmp.CourseID AND c.VersionID = tmp.VersionID

OPEN @Cursor_Cr
		fetch NEXT from @Cursor_Cr into
				@IsCreditCourse  ,
				@Credits , 
				@CourseID , 
				@VersionID 
	while @@FETCH_STATUS = 0
	BEGIN 

				UPDATE Temp_First_Bill_Generation
				   SET 
					  IsCreditCourse = @IsCreditCourse
					  ,Credits = @Credits
				      
				 WHERE CourseID = @CourseID and
					 VersionID = @VersionID

				fetch NEXT from @Cursor_Cr into
								@IsCreditCourse  ,
								@Credits , 
								@CourseID , 
								@VersionID 
	END
CLOSE @Cursor_Cr
DEALLOCATE @Cursor_Cr

---------------------------------------------------------------------------------------------------------
-- update corresponding "AdmissionCalenderID" 
--
	UPDATE Temp_First_Bill_Generation 
	SET 
		AdmissionCalenderID = (select  AdmissionCalenderID from  UIUEMS_ER_Admission as a where a.StudentID = @StdID)					  
		WHERE StudentID = @StdID
---------------------------------------------------------------------------------------------------------
-- update corresponding Student's "ProgramID "
--
UPDATE Temp_First_Bill_Generation 
	SET 
		ProgramID = (select  ProgramID from  UIUEMS_ER_Student where StudentID = @StdID)					  
		WHERE StudentID = @StdID
---------------------------------------------------------------------------------------------------------
-- update corresponding Student's "PCA" and "Fee"
--
declare		@Cursor_Fee CURSOR

set @Cursor_fee = CURSOR FOR
	select  fs.Amount 'PCA' , isnull(fs.Amount,0) * tmp.Credits as Fee, tmp.CourseID, tmp.VersionID
	from Temp_First_Bill_Generation as tmp
	INNER JOIN UIUEMS_BL_FeeSetup AS fs 
		ON	fs.ProgramID = tmp.ProgramID AND 
			fs.AcaCalID = tmp.AdmissionCalenderID AND 
			fs.typeDefId = 14 --Hard code for 'PCA'

OPEN @Cursor_fee
		fetch NEXT from @Cursor_fee into
				@PCA  ,
				@Fee , 
				@CourseID , 
				@VersionID 
		while @@FETCH_STATUS = 0
		BEGIN
				UPDATE Temp_First_Bill_Generation
				   SET 
					   PCA = @PCA
					  ,Fee = @Fee
				      
				 WHERE CourseID = @CourseID and
					 VersionID = @VersionID

				fetch NEXT from @Cursor_fee into
								@PCA  ,
								@Fee , 
								@CourseID , 
								@VersionID 
	END
CLOSE @Cursor_Fee
DEALLOCATE @Cursor_Fee
---------------------------------------------------------------------------------------------------------
-- update corresponding "RetakeNo" 
--
declare	@Cursor_Re CURSOR

set @Cursor_Re = CURSOR FOR
	select  CourseID, VersionID
	from Temp_First_Bill_Generation 
	where StudentId = @StdID

OPEN @Cursor_Re
		fetch NEXT from @Cursor_Re into				
				@CourseID , 
				@VersionID 
	while @@FETCH_STATUS = 0
	BEGIN 

				UPDATE Temp_First_Bill_Generation
				   SET 
					  RetakeNo = (select isnull(max(RetakeNo),0)'RetakeNo' from UIUEMS_CC_Student_CourseHistory where studentId = @StdID and CourseID = @CourseID and VersionID = @VersionID)

				 WHERE CourseID = @CourseID and
					 VersionID = @VersionID

				fetch NEXT from @Cursor_Re into								
								@CourseID , 
								@VersionID 
	END
CLOSE @Cursor_Re
DEALLOCATE @Cursor_Re
---------------------------------------------------------------------------------------------------------
-- update corresponding "RetakeDiscountPercentage" and "Fee"
--
declare	@Cursor_ReDis CURSOR

set @Cursor_ReDis = CURSOR FOR
	select  GradeId , RetakeNo, ProgramID, AdmissionCalenderID,courseId,versionId,fee
	from Temp_First_Bill_Generation 
	where StudentId = @StdID

OPEN @Cursor_ReDis
		fetch NEXT from @Cursor_ReDis into				
				@GradeId, 
				@RetakeNo,
				@ProgramID,
				@AdmissionCalenderID,
				@CourseID, 
				@VersionID,
				@Fee
 
	while @@FETCH_STATUS = 0
	BEGIN 

		if(@RetakeNo > 0)
		begin
			set @RetakeDiscountPercentage = (select RetakeDiscount from  UIUEMS_CC_GradeDetails where ProgramID = @ProgramID and AcaCalId = @AdmissionCalenderID and GradeId = @GradeId)
			UPDATE Temp_First_Bill_Generation
						   SET 
							  RetakeDiscountPercentage = @RetakeDiscountPercentage,
							  fee = ((100 - @RetakeDiscountPercentage) / 100) * @Fee
						 WHERE  StudentId = @StdID and
								GradeId = @GradeId and
								CourseId = @CourseID  and 
								VersionId = @VersionID
		end				

	fetch NEXT from @Cursor_ReDis into								
		@GradeId, 
		@RetakeNo,
		@ProgramID,
		@AdmissionCalenderID,
		@CourseID, 
		@VersionID,
		@Fee 
	END
CLOSE @Cursor_ReDis
DEALLOCATE @Cursor_ReDis
				
END
---------------------------------------------------------------------------------------------------------
-- update corresponding "BillStartFromRetake" 
--
declare	@Cursor_BSFR CURSOR

set @Cursor_BSFR = CURSOR FOR
	select  AcaCalID , ProgramID, CourseID, VersionID ,BillStartFromRetakeNo
	from UIUEMS_BL_IsCourseBillable 
	--where StudentId = 33

OPEN @Cursor_BSFR
		fetch NEXT from @Cursor_BSFR into				
				@AcaCalID, 
				@ProgramID,
				@CourseID,
				@VersionID,
				@BillStartFromRetakeNo
 
	while @@FETCH_STATUS = 0
	BEGIN 		
		UPDATE Temp_First_Bill_Generation
		SET 
			BillStartFromRetake = @BillStartFromRetakeNo

			WHERE  AdmissionCalenderID = @AcaCalID AND 
					ProgramID = @ProgramID AND
								CourseID = @CourseID AND
								VersionID = @VersionID					

	fetch NEXT from @Cursor_BSFR into								
		@AcaCalID, 
		@ProgramID,
		@CourseID,
		@VersionID,
		@BillStartFromRetakeNo
	END
CLOSE @Cursor_BSFR
DEALLOCATE @Cursor_BSFR

--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
-- BILL Posting Semester Fee
--


set @Definition = 'System'
set	@AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)

--select t1.* 
--from (select slno from UIUEMS_AC_Voucher where DrAccountHeadsID = (select ah.AccountsID   from UIUEMS_ER_Student as s 
--		inner join UIUEMS_AC_AccountHeads as ah on ah.Name = s.roll where studentId=@StdID)) as t1 
--		inner join (select slno from UIUEMS_AC_Voucher where CrAccountHeadsID = 135) as t2 on t1.slno = t2.slno 

select @count = count(*) 
from UIUEMS_AC_Voucher as v
where v.slno = (select t1.* 
		from (select slno 
			from UIUEMS_AC_Voucher where DrAccountHeadsID = (select ah.AccountsID   from UIUEMS_ER_Student as s 
				inner join UIUEMS_AC_AccountHeads as ah on ah.Name = s.roll where studentId=33)) as t1 
				inner join (select slno from UIUEMS_AC_Voucher where CrAccountHeadsID = 135) as t2 on t1.slno = t2.slno) -- 135 Library and laboratory fee Hard code
	and AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)

--select @count

--select @count
if(@count < 1)
begin
	select    @Fee = amount,  @StdAccHeadID = StdAccHeadId 
		from Temp_First_Bill_Generation as tmp
		INNER JOIN UIUEMS_BL_FeeSetup AS fs 
			ON	fs.ProgramID = tmp.ProgramID AND 
				fs.AcaCalID = tmp.AdmissionCalenderID AND 
				fs.typeDefId = 21 --Hard code for 'PCA'
	where tmp.studentId = 33 group by amount,  StdAccHeadId 

	-- Library and laboratory fee (135)
							set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

							EXEC CreateBillVoucher									 
								'Bill'
								,@v_SLNO
								--,@Condition_AcaCalID
								,@StdAccHeadID--DrAccountHeadsID
								,null--CrAccountHeadsID							
								,@Fee--<Amount, money,> 
								,'System'
								,null
								,null
								,@Definition
								,@CreatorID
								,@AcaCalID

							EXEC CreateBillVoucher
								'Bill'
								,@v_SLNO
								--,@Condition_AcaCalID
								,null--DrAccountHeadsID
								,135--CrAccountHeadsID	[hard coded] 						
								,@Fee--<Amount, money,>
								,'System'
								,null
								,null
								,@Definition
								,@CreatorID
								,@AcaCalID

end



--select t1.* 
--from (select slno from UIUEMS_AC_Voucher where DrAccountHeadsID = (select ah.AccountsID   from UIUEMS_ER_Student as s 
--		inner join UIUEMS_AC_AccountHeads as ah on ah.Name = s.roll where studentId=@StdID)) as t1 
--		inner join (select slno from UIUEMS_AC_Voucher where CrAccountHeadsID = 136) as t2 on t1.slno = t2.slno

select @count = count(*) 
from UIUEMS_AC_Voucher as v
where v.slno = (select t1.* 
	from (select slno 
			from UIUEMS_AC_Voucher where DrAccountHeadsID = (select ah.AccountsID   from UIUEMS_ER_Student as s 
				inner join UIUEMS_AC_AccountHeads as ah on ah.Name = s.roll where studentId=33)) as t1 
				inner join (select slno from UIUEMS_AC_Voucher where CrAccountHeadsID = 136) as t2 on t1.slno = t2.slno) --136 Extra curricular activities {Hard coded}
	and AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)

--select @count

--select @count
if(@count < 1)
begin
	select    @Fee = amount,  @StdAccHeadID = StdAccHeadId 
		from Temp_First_Bill_Generation as tmp
		INNER JOIN UIUEMS_BL_FeeSetup AS fs 
			ON	fs.ProgramID = tmp.ProgramID AND 
				fs.AcaCalID = tmp.AdmissionCalenderID AND 
				fs.typeDefId = 22 --Hard code for 'PCA'
	where tmp.studentId = 33 group by amount,  StdAccHeadId 

	-- Extra curricular activities (136)
							set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

							EXEC CreateBillVoucher									 
								'Bill'
								,@v_SLNO
								--,@Condition_AcaCalID
								,@StdAccHeadID --DrAccountHeadsID
								,null --CrAccountHeadsID							
								,@Fee --<Amount, money,> 
								,'System'
								,null 
								,null
								,@Definition
								,@CreatorID
								,@AcaCalID

							EXEC CreateBillVoucher
								'Bill'
								,@v_SLNO
								--,@Condition_AcaCalID
								,null--DrAccountHeadsID
								,136--CrAccountHeadsID	[Hard coded]						
								,@Fee--<Amount, money,>
								,'System'
								,null 
								,null
								,@Definition
								,@CreatorID
								,@AcaCalID
end

--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
-- BILL Posting Course Fee
--

--@CreatorID int,
--@AcaCalID int,
--@RetakeNo int,
--@BillStartFromRetakeNo int, 
--@CourseID int, 
--@VersionID int, 
--@GradeId int, 
--@ProgramID int, 
--@AdmissionCalenderID int,
--@IsCreditCourse bit ,
--@Credits int,
--@RetakeDiscountPercentage decimal(18,2) ,
--@PCA decimal(18,2) ,
--@Fee decimal(18,2);

set @Definition = 'System'
set	@AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)

declare @Bill_Cursor Cursor;


--select t1.* 
--from (select slno from UIUEMS_AC_Voucher where DrAccountHeadsID = (select ah.AccountsID   from UIUEMS_ER_Student as s 
--		inner join UIUEMS_AC_AccountHeads as ah on ah.Name = s.roll where studentId=@StdID)) as t1 
--		inner join (select slno from UIUEMS_AC_Voucher where CrAccountHeadsID = 137) as t2 on t1.slno = t2.slno 



	set @Bill_Cursor = cursor for

	SELECT     StdAccHeadID, PcaAccHeadId, RetakeDiscountPercentage, RetakeNo, isnull(BillStartFromRetake,0), 
			   AcaCalID, CourseID, VersionID, Credits, ProgramID, AdmissionCalenderID, Fee
	FROM       Temp_First_Bill_Generation

	OPEN @Bill_Cursor
			fetch NEXT from @Bill_Cursor into				
					@StdAccHeadID , 
					@PcaAccHeadId , 
					@RetakeDiscountPercentage , 
					@RetakeNo , 
					@BillStartFromRetake ,
					@AcaCalID ,
					@CourseID , 
					@VersionID , 
					@Credits , 
					@ProgramID , 
					@AdmissionCalenderID , 
					@Fee
	 
		while @@FETCH_STATUS = 0
		BEGIN 		


select @count = count(*) 
from UIUEMS_AC_Voucher as v
where v.slno = (select t1.* 
from (select slno 
			from UIUEMS_AC_Voucher where DrAccountHeadsID = (select ah.AccountsID   from UIUEMS_ER_Student as s 
				inner join UIUEMS_AC_AccountHeads as ah on ah.Name = s.roll where studentId=33)) as t1 
				inner join (select slno from UIUEMS_AC_Voucher where CrAccountHeadsID = 137 and courseId = @CourseID and versionId = @VersionID) as t2 on t1.slno = t2.slno) -- 137 Per Credit Amount Hard code

and AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)

--select @count

--select @count
if(@count < 1)
begin





								if(@BillStartFromRetake = 0)
									begin
							--				select @StdAccHeadID , 
							--						@PcaAccHeadId , 
							--						@RetakeDiscountPercentage , 
							--						@RetakeNo , 
							--						@BillStartFromRetake ,
							--						@AcaCalID ,
							--						@CourseID , 
							--						@VersionID , 
							--						@Credits , 
							--						@ProgramID , 
							--						@AdmissionCalenderID , 
							--						@Fee

													set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

													EXEC CreateBillVoucher									 
														'Bill'
														,@v_SLNO
														--,@Condition_AcaCalID
														,@StdAccHeadID--DrAccountHeadsID
														,null--CrAccountHeadsID							
														,@Fee--<Amount, money,> 
														,'System'
														,@CourseID 
														,@VersionID
														,@Definition
														,@CreatorID
														,@AcaCalID

													EXEC CreateBillVoucher
														'Bill'
														,@v_SLNO
														--,@Condition_AcaCalID
														,null--DrAccountHeadsID
														,@PcaAccHeadId--CrAccountHeadsID							
														,@Fee--<Amount, money,>
														,'System'
														,@CourseID 
														,@VersionID
														,@Definition
														,@CreatorID
														,@AcaCalID
									end
								else if(@RetakeNo >= @BillStartFromRetake)
									begin
							--					 select @StdAccHeadID , 
							--							@PcaAccHeadId , 
							--							@RetakeDiscountPercentage , 
							--							@RetakeNo , 
							--							@BillStartFromRetake ,
							--							@AcaCalID ,
							--							@CourseID , 
							--							@VersionID , 
							--							@Credits , 
							--							@ProgramID , 
							--							@AdmissionCalenderID , 
							--							@Fee

													set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

													EXEC CreateBillVoucher									 
														'Bill'
														,@v_SLNO
														--,@Condition_AcaCalID
														,@StdAccHeadID--DrAccountHeadsID
														,null--CrAccountHeadsID							
														,@Fee--<Amount, money,> 
														,'System'
														,@CourseID 
														,@VersionID
														,@Definition
														,@CreatorID
														,@AcaCalID

													EXEC CreateBillVoucher
														'Bill'
														,@v_SLNO
														--,@Condition_AcaCalID
														,null--DrAccountHeadsID
														,@PcaAccHeadId--CrAccountHeadsID							
														,@Fee--<Amount, money,>
														,'System'
														,@CourseID 
														,@VersionID
														,@Definition
														,@CreatorID
														,@AcaCalID
									end		

end

			fetch NEXT from @Bill_Cursor into								
						@StdAccHeadID , 
						@PcaAccHeadId , 
						@RetakeDiscountPercentage , 
						@RetakeNo , 
						@BillStartFromRetake ,
						@AcaCalID ,
						@CourseID , 
						@VersionID , 
						@Credits , 
						@ProgramID , 
						@AdmissionCalenderID , 
						@Fee
		END


	CLOSE @Bill_Cursor
	DEALLOCATE @Bill_Cursor
--------end

--@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@





GO
/****** Object:  StoredProcedure [dbo].[usp_Prepare_Student_Discount_Worksheet]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author: <Ashraf>
-- Create date: <1,12,2011>
-- Description: <prepare bill worksheet for per student>
-- =============================================

CREATE PROCEDURE [dbo].[usp_Prepare_Student_Discount_Worksheet] 

@StuId int,
@BatchId int,
@ProgramId int

AS
BEGIN

IF EXISTS
	(
		select * from UIUEMS_BL_StudentDiscountWorkSheet 
		where (StudentID = @StuId or  @StuId is null)
		and (ProgramID = @ProgramId  or @ProgramId is null)
		and (AdmissionCalId = @BatchId or @BatchId is null)
	)
BEGIN
	DELETE FROM UIUEMS_BL_StudentDiscountWorkSheet 
		where (StudentID = @StuId or  @StuId is null)
		and (ProgramID = @ProgramId  or @ProgramId is null)
		and (AdmissionCalId = @BatchId or @BatchId is null)
END
	-- declare common field

DECLARE @RowCount int
set @RowCount = 0;

	declare 
			@cm_StudentID int,
			@cm_ProgramID int,
			@cm_AcaCalID int, 
			@cm_AdmissionCalenderID int,			
			@cm_TotalCrRegInPreviousSession numeric(18,2),
			@cm_GPAinPreviousSession numeric(18,2),
			@cm_CGPAupToPreviousSession numeric(18,2),
			@cm_TotalCrRegInCurrentSession numeric(18,2),
			@cm_Remarks varchar(max);

	set @cm_Remarks = 'System input';
	set @cm_TotalCrRegInPreviousSession = 0;
	set @cm_GPAinPreviousSession = 0;
	set @cm_CGPAupToPreviousSession = 0;
	set @cm_TotalCrRegInCurrentSession = 0;

	declare 
			@Cursor_cm CURSOR

	-- declare discount field
	declare 
			@dc_TypeDefID int, 
			@dc_TypePercentage decimal(18,2);
	declare 
			@Cursor_dc CURSOR
	
	-- fill common cursor
	set @Cursor_cm = CURSOR For 

--need change, 
		SELECT     
		 distinct(sch.StudentID), -- need to ignore 'distinct', add joining with UIUEMS_ER_STD_AcademicCalender 
			p.ProgramID	,	
			sch.AcaCalID,
			adm.AdmissionCalenderID,
			0 'TotalCrRegInPreviousSession',
			0 'GPAinPreviousSession',
			0 'CGPAupToPreviousSession',
			0 'TotalCrRegInCurrentSession'			

		FROM	UIUEMS_CC_Student_CourseHistory as sch 
		left outer join UIUEMS_ER_Student as s on s.StudentID = sch.StudentID
		left outer join UIUEMS_CC_Program as p on p.ProgramID = s.ProgramID		
		left outer join UIUEMS_ER_Admission as adm on adm.StudentID = sch.StudentID

		where (sch.StudentID = @StuId or  @StuId is null)
			and (p.ProgramID = @ProgramId  or @ProgramId is null)
			and (sch.AcaCalID = (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1) )
	
	--Start Batch execution.
		---------------------SELECT @RowCount = @@CURSOR_ROWS
		OPEN @Cursor_cm
				fetch NEXT from @Cursor_cm into 
												@cm_StudentID ,
												@cm_ProgramID ,
												@cm_AcaCalID , 
												@cm_AdmissionCalenderID ,			
												@cm_TotalCrRegInPreviousSession ,
												@cm_GPAinPreviousSession ,
												@cm_CGPAupToPreviousSession ,
												@cm_TotalCrRegInCurrentSession 
																								
				SELECT @RowCount = @@CURSOR_ROWS						
				while @@FETCH_STATUS = 0
				BEGIN
					-- discount cursor block
						set @Cursor_dc = CURSOR For
								SELECT	
									sdc.TypeDefID, 
									sdc.TypePercentage
								FROM	UIUEMS_SM_StdDiscountCurrent AS sdc 
										LEFT OUTER JOIN UIUEMS_ER_Admission AS adm ON adm.AdmissionID = sdc.AdmissionID
										WHERE     (adm.StudentID = @cm_StudentID)			

						OPEN @Cursor_dc 
								fetch NEXT from @Cursor_dc into  
															@dc_TypeDefID, 
															@dc_TypePercentage 

								while @@FETCH_STATUS = 0	
								BEGIN									
										INSERT INTO UIUEMS_BL_StudentDiscountWorkSheet
											   (
												    StudentID
												   ,ProgramID
												   ,AcaCalID
												   ,AdmissionCalId
												   ,TotalCrRegInPreviousSession
												   ,GPAinPreviousSession
												   ,CGPAupToPreviousSession
												   ,TotalCrRegInCurrentSession
												   ,DiscountTypeId
												   ,DiscountPercentage
												   ,Remarks
												   ,CreatedBy
												   ,CreatedDate
											   )
										 VALUES
											   (
												    @cm_StudentID
												   ,@cm_ProgramID
												   ,@cm_AcaCalID
												   ,@cm_AdmissionCalenderID 
												   ,@cm_TotalCrRegInPreviousSession
												   ,@cm_GPAinPreviousSession
												   ,@cm_CGPAupToPreviousSession
												   ,@cm_TotalCrRegInCurrentSession
												   ,@dc_TypeDefID		--DiscountTypeId
												   ,@dc_TypePercentage	--DiscountPercentage
												   ,@cm_Remarks			--remarks
												   ,-2
												   ,getdate()
												)

									FETCH NEXT from @Cursor_dc into  
																 @dc_TypeDefID, 
																 @dc_TypePercentage
								END
							CLOSE @Cursor_dc
							DEALLOCATE @Cursor_dc
					--discount cursor block END

					--***********************************************************************************************************************************
					
					FETCH NEXT from @Cursor_cm into							
												@cm_StudentID ,
												@cm_ProgramID ,
												@cm_AcaCalID , 
												@cm_AdmissionCalenderID ,			
												@cm_TotalCrRegInPreviousSession ,
												@cm_GPAinPreviousSession ,
												@cm_CGPAupToPreviousSession ,
												@cm_TotalCrRegInCurrentSession 

				END

	CLOSE @Cursor_cm
	DEALLOCATE @Cursor_cm

SELECT @RowCount AS 'count'
--End Batch execution.

END
 













GO
/****** Object:  StoredProcedure [dbo].[usp_RegistrationBillFinal]    Script Date: 3/22/2014 4:34:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp_RegistrationBillFinal]

@StdID int,
@progID int,
@CreatorID int

AS
BEGIN

	declare	
	@v_stdAccHeadID int,
	@Definition nvarchar(250),	
	@v_SLNO int,
	@AccountsID int;

	set @v_stdAccHeadID = (select AccountsID 
							from UIUEMS_AC_AccountHeads 
							where [Name] = (select roll from dbo.UIUEMS_ER_Student where StudentID = @StdID))
	set @AccountsID = 137 --// Hard coded 'Per Credit Amount'
	set @Definition = 'System'

	declare	
	@Data_StudentID int, 
	@Data_AcaCalSectionID int,
	@Data_RetakeNo int, 
	@Data_CourseStatusID int, 
	@Data_AcaCalID int, 
	@Data_CourseID int,
	@Data_VersionID int, 
	@Data_Node_CourseID int, 
	@Data_NodeID int, 
	@Data_IsMultipleACUSpan bit,
	@Data_IsCreditCourse bit, 
	@Data_Credits numeric(18,2),
	@Data_Fee decimal(18,2);

	declare		@Cursor_Data CURSOR

	declare
	@Condition_AcaCalID int, 
	@Condition_ProgramID int , 
	@Condition_BillStartFromRetakeNo int, 
	@Condition_IsCreditCourse bit

	declare		@Cursor_Condition CURSOR

	set @Cursor_Data = CURSOR FOR

		SELECT		ch.StudentID, ch.AcaCalSectionID, isnull(ch.RetakeNo,0) AS RetakeNo, ch.CourseStatusID, ch.AcaCalID, ch.CourseID, 
					ch.VersionID, ch.Node_CourseID, ch.NodeID, ch.IsMultipleACUSpan,
					c.IsCreditCourse, c.Credits,
					isnull(bw.Fee,0) * c.Credits as Fee

		FROM		UIUEMS_CC_Student_CourseHistory AS ch 
					inner join 	dbo.UIUEMS_CC_Course AS c ON c.CourseID = ch.CourseID and c.VersionID = ch.VersionID
					inner join dbo.UIUEMS_BL_Std_Crs_Bill_Worksheet AS bw ON bw.StudentID = ch.StudentID 
																			and bw.CourseID = ch.CourseID 
																			and bw.VersionID = ch.VersionID 

		WHERE		(ch.StudentID = @StdID and Fee != 0)

		OPEN @Cursor_Data
		fetch NEXT from @Cursor_Data into
					@Data_StudentID , 
					@Data_AcaCalSectionID ,
					@Data_RetakeNo , 
					@Data_CourseStatusID , 
					@Data_AcaCalID , 
					@Data_CourseID ,
					@Data_VersionID , 
					@Data_Node_CourseID , 
					@Data_NodeID , 
					@Data_IsMultipleACUSpan ,
					@Data_IsCreditCourse , 
					@Data_Credits ,
					@Data_Fee 

			while @@FETCH_STATUS = 0
			BEGIN

			select  @Data_StudentID , 
					@Data_AcaCalSectionID ,
					@Data_RetakeNo , 
					@Data_CourseStatusID , 
					@Data_AcaCalID , 
					@Data_CourseID ,
					@Data_VersionID , 
					@Data_Node_CourseID , 
					@Data_NodeID , 
					@Data_IsMultipleACUSpan ,
					@Data_IsCreditCourse , 
					@Data_Credits ,
					@Data_Fee 

	--
		set @Cursor_Condition = CURSOR FOR
			SELECT     icb.AcaCalID, icb.ProgramID, icb.BillStartFromRetakeNo, icb.IsCreditCourse

			FROM         UIUEMS_BL_IsCourseBillable AS icb

			where	icb.AcaCalID= (select AcademicCalenderID from dbo.UIUEMS_CC_AcademicCalender where IsCurrent = 1)
					AND icb.ProgramID=@progID

		OPEN @Cursor_Condition
			fetch NEXT from @Cursor_Condition into
				@Condition_AcaCalID , 
				@Condition_ProgramID  , 
				@Condition_BillStartFromRetakeNo , 
				@Condition_IsCreditCourse 

		while @@FETCH_STATUS = 0
			BEGIN			

		if(@Data_IsCreditCourse = 1)
		begin
			if(@Condition_IsCreditCourse = 1)
			begin

				if(@Data_RetakeNo >= @Condition_BillStartFromRetakeNo)
				begin
	--				select
	--					@Condition_AcaCalID , 
	--					@Condition_ProgramID  , 
	--					@Condition_BillStartFromRetakeNo , 
	--					@Condition_IsCreditCourse 
						
						set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

						EXEC dbo.CreateBillVoucher									 
							'Bill'
							,@v_SLNO
							--,@Condition_AcaCalID
							,null--DrAccountHeadsID
							,@v_stdAccHeadID							
							,@Data_Fee--<Amount, money,> 
							,'System'
							,@Data_CourseID 
							,@Data_VersionID
							,@Definition
							,@CreatorID

						EXEC dbo.CreateBillVoucher
							'Bill'
							,@v_SLNO
							--,@Condition_AcaCalID
							,@AccountsID
							,null--CrAccountHeadsID							
							,@Data_Fee--<Amount, money,>
							,'System'
							,@Data_CourseID 
							,@Data_VersionID
							,@Definition
							,@CreatorID

				end
			end
		end

		else if(@Data_IsCreditCourse = 0)
		begin
			if(@Condition_IsCreditCourse = 0)
			begin

				if(@Data_RetakeNo >= @Condition_BillStartFromRetakeNo)
				begin
	--				select
	--					@Condition_AcaCalID , 
	--					@Condition_ProgramID  , 
	--					@Condition_BillStartFromRetakeNo , 
	--					@Condition_IsCreditCourse 

						set @v_SLNO = (select max(SLNO) from UIUEMS_AC_Voucher) + 1

						EXEC dbo.CreateBillVoucher									 
							'Bill'
							,@v_SLNO
							,@Condition_AcaCalID
							,@v_stdAccHeadID
							,null--<CrAccountHeadsID, int,>
							,@Data_Fee--<Amount, money,> 
							,'System'
							,@Data_CourseID 
							,@Data_VersionID
							,@Definition
							,@CreatorID

						EXEC dbo.CreateBillVoucher
							'Bill'
							,@v_SLNO
							,@Condition_AcaCalID
							,null--DrAccountHeadsID, int,>
							,@AccountsID
							,@Data_Fee--<Amount, money,>
							,'System'
							,@Data_CourseID 
							,@Data_VersionID
							,@Definition
							,@CreatorID
				end
			end
		end		

				fetch NEXT from @Cursor_Condition into
					@Condition_AcaCalID , 
					@Condition_ProgramID  , 
					@Condition_BillStartFromRetakeNo , 
					@Condition_IsCreditCourse 
			END

			CLOSE @Cursor_Condition
			DEALLOCATE @Cursor_Condition
	--
				fetch NEXT from @Cursor_Data into
							@Data_StudentID , 
							@Data_AcaCalSectionID ,
							@Data_RetakeNo , 
							@Data_CourseStatusID , 
							@Data_AcaCalID , 
							@Data_CourseID ,
							@Data_VersionID , 
							@Data_Node_CourseID , 
							@Data_NodeID , 
							@Data_IsMultipleACUSpan ,
							@Data_IsCreditCourse , 
							@Data_Credits ,
							@Data_Fee
			END

			CLOSE @Cursor_data
			DEALLOCATE @Cursor_data
END





GO
