ALTER PROCEDURE [dbo].[SP_MDC_UpdateProtection]
	@PrescriptionId as int,
	@StartDate as datetime,
	@StopDate as datetime,
	@PosologyExpression as nvarchar(100),
	@PosologyJSONDetails as nvarchar(4000),
	@UserLogin as nvarchar(255),
	@UserIp as varchar(15)
AS
BEGIN
	declare @IDPHARMACIST int
	declare @ActionDescription varchar(999)
	select @IDPHARMACIST=PHA_KIDPHARMACIST from PRESCRIPTION.PHARMACIST where PHA_VISA=@UserLogin

	--etape 1, arrêter la protection actuelle
	update p set p.PRE_STOPDATE=DATEADD(d, -1, @StartDate), p.PRE_MODIFICATIONDATE=getdate() from PRESCRIPTION.PRESCRIPTION p where p.PRE_KIDPRESCRIPTION=@PrescriptionId

	--etape 2, créer le schedule
	declare @CreationDate datetime 
	set @CreationDate = getdate()
	declare @ScheduleId int

	set @StartDate= DATEADD(d, 0, DATEDIFF(d, 0,@StartDate))
set @StopDate= DATEADD(d, 0, DATEDIFF(d, 0,@StopDate))

	-- @@PosologyJSONDetails h:q avec division par |
	-- exemple : [{"Hour":8,"Quantity":1},{"Hour":13,"Quantity":4}]

	INSERT INTO [PRESCRIPTION].[SCHEDULE]
           ([SCH_STARTDATE],[SCH_STOPDATE],[SCH_CREATIONDATE],[SCH_MODIFICATIONDATE],[SCH_H0],[SCH_H1],[SCH_H2],[SCH_H3],[SCH_H4],[SCH_H5],[SCH_H6]
           ,[SCH_H7],[SCH_H8],[SCH_H9],[SCH_H10],[SCH_H11],[SCH_H12],[SCH_H13],[SCH_H14],[SCH_H15],[SCH_H16],[SCH_H17],[SCH_H18],[SCH_H19],[SCH_H20]
           ,[SCH_H21],[SCH_H22],[SCH_H23],[SCH_DAY0],[SCH_DAY1],[SCH_DAY2],[SCH_DAY3],[SCH_DAY4],[SCH_DAY5],[SCH_DAY6],[SCH_DAY7],[SCH_DAY8]
           ,[SCH_DAY9],[SCH_DAY10],[SCH_DAY11],[SCH_DAY12],[SCH_DAY13],[SCH_DAY14],[SCH_DAY15],[SCH_DAY16],[SCH_DAY17],[SCH_DAY18],[SCH_DAY19]
           ,[SCH_DAY20],[SCH_DAY21],[SCH_DAY22],[SCH_DAY23],[SCH_DAY24],[SCH_DAY25],[SCH_DAY26],[SCH_DAY27],[SCH_DAY28],[SCH_DAY29],[SCH_DAY30]
           ,[SCH_POSOLOGY],[SCH_TYPEMODIFICATION],[SCH_DELETED])
	select
		@StartDate as [SCH_STARTDATE], @StopDate as [SCH_STOPDATE], @CreationDate as [SCH_CREATIONDATE], @CreationDate as [SCH_MODIFICATIONDATE],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=0),0) as [SCH_H0],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=1),0) as [SCH_H1],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=2),0) as [SCH_H2],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=3),0) as [SCH_H3],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=4),0) as [SCH_H4],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=5),0) as [SCH_H5],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=6),0) as [SCH_H6],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=7),0) as [SCH_H7],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=8),0) as [SCH_H8],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=9),0) as [SCH_H9],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=10),0) as [SCH_H10],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=11),0) as [SCH_H11],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=12),0) as [SCH_H12],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=13),0) as [SCH_H13],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=14),0) as [SCH_H14],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=15),0) as [SCH_H15],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=16),0) as [SCH_H16],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=17),0) as [SCH_H17],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=18),0) as [SCH_H18],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=19),0) as [SCH_H19],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=20),0) as [SCH_H20],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=21),0) as [SCH_H21],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=22),0) as [SCH_H22],
			isnull((SELECT JSON_VALUE(value,'$.Quantity') FROM OPENJSON(@PosologyJSONDetails) where JSON_VALUE(value,'$.Hour')=23),0) as [SCH_H23],
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 
			1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
			@PosologyExpression as [SCH_POSOLOGY], 
			3 as [SCH_TYPEMODIFICATION], 
			0 as [SCH_DELETED]

	-- etape 3, récupérer l'id de schedule créé
	select @ScheduleId=s.SCH_KIDSCHEDULE from [PRESCRIPTION].[SCHEDULE] s where s.SCH_CREATIONDATE=@CreationDate

	--etape 4, créer une nouvelle protection en copiant la précédente
	declare @NumPrescription int
	select @NumPrescription=max(p.[PRE_NUMPRESCRIPTION])+1 from PRESCRIPTION.PRESCRIPTION p -- ??? est-ce correct ?

	insert into PRESCRIPTION.PRESCRIPTION ([PRE_NUMPRESCRIPTION], [PRE_IDPATIENT],[PRE_IDDOCTOR],[PRE_IDPHARMACIST],[PRE_IDDRUGARTICLE_DOCTOR],
		[PRE_IDDRUGARTICLE_PHARMACIST],[PRE_IDSCHEDULE],[PRE_STARTDATE],[PRE_STOPDATE],[PRE_DELETED],[PRE_SPECIAL],[PRE_RESERVE],[PRE_DESCRIPTIONARTICLE_EMS],
		[PRE_MODIFICATIONDATE],[PRE_CREATIONDATE])
	select @NumPrescription, [PRE_IDPATIENT],[PRE_IDDOCTOR],[PRE_IDPHARMACIST],[PRE_IDDRUGARTICLE_DOCTOR],
		[PRE_IDDRUGARTICLE_PHARMACIST], @ScheduleId, @StartDate, @StopDate,0,[PRE_SPECIAL],[PRE_RESERVE],[PRE_DESCRIPTIONARTICLE_EMS],
		null, getdate()
	from PRESCRIPTION.PRESCRIPTION p 
	where p.PRE_KIDPRESCRIPTION=@PrescriptionId 

	-- etape 5 ? lancer le traitement de création de calendrier ?	

	--etape 6, journalier la mise à jour
	set @ActionDescription = concat('Mise à jour de la protection ', @PrescriptionId,' à partir de ', convert(nvarchar(20), @StopDate, 103),' depuis Medicare mobile')
	exec [dbo].[SP_ADD_HISTORIC_V2]	@IDPHARMACIST, '[PRESCRIPTION]', '[PRESCRIPTION]', 0, @ActionDescription, @UserIp, 0

		exec [dbo].SP_CREATE_CALENDAR_FROM_SCHEDULE 
END

