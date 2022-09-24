DROP PROCEDURE IF EXISTS sp_CalcularMatriz 
--select * from [dbo].AvaliacaoCategoria
-- ================================================
-- Template generated from Template Explorer using:
-- Create Procedure (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the procedure.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE sp_CalcularMatriz
	@formulaId bigint
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
	
	SELECT @p_formulaX = [FormulaEixoX] FROM [dbo].[FormulaCalculoMatriz] WHERE Id = @formulaId
	SELECT @p_formulaY = [FormulaEixoY] FROM [dbo].[FormulaCalculoMatriz] WHERE Id = @formulaId


	DELETE AvaliacaoCategoria

	DECLARE CUR CURSOR 
		LOCAL STATIC READ_ONLY FORWARD_ONLY
	FOR 
		SELECT Id FROM Categoria
	OPEN CUR
		FETCH NEXT FROM CUR INTO @categoriaId
	
	WHILE @@FETCH_STATUS = 0
	BEGIN 
		select @formulaX = dbo.fn_ConstruirFormula(@p_formulaX,@categoriaId)
		select @formulaY = dbo.fn_ConstruirFormula(@p_formulaY,@categoriaId)

		set @sql = '
			INSERT INTO AvaliacaoCategoria(DataCriacao,DataModificacao,CategoriaId,EixoX,EixoY,QuadranteId,EstimativaGastoMensal,GastoTotal,RiscoOperativo,RiscoQualidade)
			VALUES
			(GETDATE(),GETDATE(),'+ convert(varchar, @categoriaId) +','+ @formulaX +','+ @formulaY +',null,0,0,0,0)
		'
		exec(@sql)

		FETCH NEXT FROM CUR INTO @categoriaId
	END
	CLOSE CUR
	DEALLOCATE CUR

	select * from  AvaliacaoCategoria
END