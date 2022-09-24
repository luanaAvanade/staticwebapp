DROP FUNCTION IF EXISTS fn_ConstruirFormula
-- ================================================
-- Template generated from Template Explorer using:
-- Create Scalar Function (New Menu).SQL
--
-- Use the Specify Values for Template Parameters 
-- command (Ctrl-Shift-M) to fill in the parameter 
-- values below.
--
-- This block of comments will not be included in
-- the definition of the function.
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION fn_ConstruirFormula
(
	@templateFormula varchar(max),
	@categoriaId bigint
)
RETURNS varchar(max)
AS
BEGIN
	DECLARE @end BIT = 0

	DECLARE @indexBegin int = 0
	DECLARE @indexEnd int = 0
	DECLARE @strReplace VARCHAR(MAX) = 0
	DECLARE @perguntaId varchar(10) = null
	--DECLARE @categoriaId bigint = null
	DECLARE @valor float = null

	WHILE(@end = 0)
	BEGIN
		set @indexBegin = CHARINDEX('{P',@templateFormula,0)
		set @indexEnd = CHARINDEX('}',@templateFormula,@indexBegin)
		
		IF(@indexBegin > 0 AND @indexEnd > 0)
		BEGIN
			set @strReplace = SUBSTRING(@templateFormula, @indexBegin, @indexEnd-@indexBegin+1)

			set @perguntaId = SUBSTRING(@strReplace,3,charindex('}', @strReplace)-3)

			select @valor = valor 
			from Resultado where perguntaId = @perguntaId AND categoriaId = @categoriaId

			set @templateFormula = REPLACE(@templateFormula, @strReplace, @valor)
		END
		ELSE
		BEGIN
			set @end = 1
		END
	END
	return @templateFormula
END
GO