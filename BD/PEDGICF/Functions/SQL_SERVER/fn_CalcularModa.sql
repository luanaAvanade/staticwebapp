DROP FUNCTION IF EXISTS fn_CalcularModa
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
CREATE FUNCTION fn_CalcularModa
(
	--@media int,
	@categoriaId bigint,
	@pergundaId bigint
)
RETURNS int
AS
BEGIN
	DECLARE @MODAS TABLE (
		VALOR INT
	)
	
	DECLARE @MODA INT
    --DECLARE @TESTE INT
	INSERT INTO @MODAS (VALOR)
	(
		SELECT COUNT(nota) AS MODA 
		FROM [dbo].[Resposta] A 
		INNER JOIN [dbo].[Categoria]B ON (B.ID = A.CategoriaId)
		WHERE PerguntaId = @pergundaId AND CategoriaId = @categoriaId
		GROUP BY b.codigo, nota
		--ORDER BY 1 DESC
	)

	DECLARE @MEDIA FLOAT
	SELECT @MEDIA = dbo.fn_CalcularMedia(@categoriaId, @pergundaId)

	SELECT @MODA = MAX(MODAS2.VALOR) FROM
	(SELECT MODAS.VALOR, ABS(MODAS.VALOR - @media) DIF 
	FROM @MODAS MODAS
	) MODAS2
	WHERE MODAS2.DIF =
	(SELECT MIN(ABS(MODAS3.VALOR - @media))
	FROM @MODAS MODAS3)

	RETURN @MODA
END
GO