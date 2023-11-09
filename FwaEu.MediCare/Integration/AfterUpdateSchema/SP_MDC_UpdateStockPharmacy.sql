ALTER PROCEDURE [dbo].[SP_MDC_UpdateStockPharmacy]
	@StockId as int,
	@Quantity as int,
	@UserLogin as nvarchar(255),
	@UserIp as varchar(15),
	@PatientId as int
AS
BEGIN
	declare @IDPHARMACIST int
	declare @ActionDescription varchar(999)
	select @IDPHARMACIST=PHA_KIDPHARMACIST from PRESCRIPTION.PHARMACIST where PHA_VISA=@UserLogin


	--etape 1, update le stock
	declare @OldQuantity int
	select @OldQuantity = s.SRV_QUANTITY from STOCK.DRUGSTOCKEMS s where s.SRV_KIDRESERVE = @StockId

	UPDATE [STOCK].[DRUGSTOCKEMS]
	   SET [SRV_QUANTITY] = @OldQuantity - @Quantity
		  ,[SRV_MODIFICATIONDATE] = getdate()
		WHERE SRV_KIDRESERVE = @StockId

	--etape 2, journalier la création
	set @ActionDescription = concat('Mise à jour de stock de la pharmacie depuis l''appli mobile pour la réserve id ', @StockId, ', quantité ', @Quantity)
	exec [dbo].[SP_ADD_HISTORIC_V2]	@IDPHARMACIST, '[COMMANDEEMS]', '[COMMANDEEMS]', 0, @ActionDescription, @UserIp, 0
	
	
	declare @pharmacode int
	select @pharmacode =    [DRA_PHARMACODE]  FROM [GESTION_DEV].[PRESCRIPTION].[DRUGARTICLE]   where DRA_IDDRUG=
	(select  s.SRV_IDDRUG from  STOCK.DRUGSTOCKEMS s where s.SRV_KIDRESERVE = @StockId) order by [DRA_QUANTITY] desc

	declare @date datetime = getdate()
	exec [dbo].[SP_ADD_RESERVEUSED] @pharmacode, @PatientId , @Quantity , '', @IDPHARMACIST, 0, @date

END
