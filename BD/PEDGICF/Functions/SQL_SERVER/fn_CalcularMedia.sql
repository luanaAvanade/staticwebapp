DROP FUNCTION IF EXISTS [dbo].[fn_CalcularMedia]
GO
/****** Object:  UserDefinedFunction [dbo].[fn_CalcularMedia]    Script Date: 22/08/2019 16:54:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[fn_CalcularMedia]
(
	--@media int,
	@categoriaId bigint,
	@pergundaId bigint
)
RETURNS int
AS
BEGIN
	DECLARE @Media INT
    
	SELECT @Media = ROUND(AVG(NOTA),0)
	FROM [dbo].[Resposta] A 
	INNER JOIN [dbo].[Categoria]B ON (B.ID = A.CategoriaId)
	WHERE PERGUNTAID = @pergundaId AND CategoriaId = @categoriaId
	GROUP BY B.CODIGO

	RETURN @Media
END
--SELECT DBO.fn_CalcularMedia(1,2)

--SELECT * FROM Resposta WHERE CategoriaId = 1 AND PerguntaId = 2