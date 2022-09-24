/****** Object:  StoredProcedure [dbo].[sp_CalcularMatriz]    Script Date: 10/09/2019 08:33:59 ******/
DROP PROCEDURE [dbo].[sp_CalcularMatriz]
GO

/****** Object:  StoredProcedure [dbo].[sp_CalcularMatriz]    Script Date: 10/09/2019 08:33:59 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Batch submitted through debugger: SQLQuery44.sql|7|0|C:\Users\daniel.souza\AppData\Local\Temp\~vsA7C9.sql
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[sp_CalcularMatriz]
	@versaoId bigint
AS
BEGIN
--exec sp_start_job 'EXEC_SSIS_PACKAGE'
	DECLARE @p_formulaX NVARCHAR(MAX) = null --N'({P1} * 0.25) + ({P2} * 0.25) + ({P3} * 0.25) + ({P4} * 0.25)'
	DECLARE @p_formulaY NVARCHAR(MAX) = null --N'({P5} * 0.30) + ({P6} * 0.30) + ({P7} * 0.30) + ({P8} * 0.10)'
	DECLARE @formulaX NVARCHAR(MAX) = null
	DECLARE @formulaY NVARCHAR(MAX) = null
	DECLARE @categoriaId bigint = null
	DECLARE @sql VARCHAR(MAX) = null
	DECLARE @valorX float = null
	DECLARE @valorY float = null
	DECLARE @estimativaGastoMensal float = NULL
	DECLARE @quadranteId int = NULL
	DECLARE @quadrante varchar(max) = NULL
	
	SELECT @p_formulaX = [FormulaEixoX], @p_formulaY = [FormulaEixoY] FROM [dbo].[VersaoMec] WHERE Id = @versaoId

	DELETE AvaliacaoCategoria where VersaoMecId = @versaoId

	DECLARE CUR CURSOR 
		LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
		SELECT Categoria.Id FROM Categoria 
		INNER JOIN VersaoMec ON Categoria.VersaoMecId = VersaoMec.Id
		WHERE VersaoMec.DataEncerramento IS NULL
	OPEN CUR
		FETCH NEXT FROM CUR INTO @categoriaId
	
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		select @formulaX = dbo.fn_ConstruirFormula(@p_formulaX,@categoriaId)
		select @formulaY = dbo.fn_ConstruirFormula(@p_formulaY,@categoriaId)

		SELECT @estimativaGastoMensal = ROUND(SUM(vlr_liquido_contratado/vlr_meses_contrato),0)
		FROM 
		[dbo].[Pd_Stg1_P1_Peso_Valor_Compra]
		WHERE cod_categoria is not null and cod_categoria = (SELECT cat.Codigo FROM Categoria cat WHERE cat.Id = @categoriaId)
		GROUP BY cod_categoria
		
		set @quadrante = 
		N'CASE  WHEN '+@formulaX+' > 5 AND '+@formulaY+' > 5 THEN 1 
			WHEN '+@formulaX+' > 5 AND '+@formulaY+' < 5 THEN 2
			WHEN '+@formulaX+' < 5 AND '+@formulaY+' > 5 THEN 4
			WHEN '+@formulaX+' < 5 AND '+@formulaY+' < 5 THEN 3
			ELSE 5
		  END'
		
		set @sql = '
			INSERT INTO AvaliacaoCategoria(DataCriacao,DataModificacao,CategoriaId,EixoX,EixoY,QuadranteId,EstimativaGastoMensal,GastoTotal,RiscoOperativo,RiscoQualidade, VersaoMecId)
			VALUES
			(GETDATE(),GETDATE(),'+ convert(varchar, @categoriaId) +','+ @formulaX +','+ @formulaY +','+ @quadrante +','+convert(varchar, @estimativaGastoMensal)+',0,0,0, '+convert(varchar,@versaoId)+')
		'
		exec(@sql)

		FETCH NEXT FROM CUR INTO @categoriaId
	END
	CLOSE CUR
	DEALLOCATE CUR
END

GO


