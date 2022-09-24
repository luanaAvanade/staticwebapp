DROP FUNCTION IF EXISTS fn_CalcularMediana

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
CREATE FUNCTION fn_CalcularMediana
(
	--@media int,
	@categoriaId bigint,
	@pergundaId bigint
)
RETURNS int
AS
BEGIN
	DECLARE @mediana int
	
	SELECT @mediana =
	ROUND(PERCENTILE_CONT(0.5) WITHIN GROUP(ORDER BY NOTA)
	OVER (PARTITION BY B.CODIGO),0) 
	FROM [dbo].[Resposta] A 
	INNER JOIN [dbo].[Categoria]B ON (B.ID = A.CategoriaId)
	WHERE PerguntaId = @pergundaId AND CategoriaId = @categoriaId
	ORDER BY B.CODIGO

	RETURN @mediana
END
GO