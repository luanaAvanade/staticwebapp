using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string FN_CalcularModa =
        @"
            IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'fn_CalcularModa') AND xtype IN (N'FN', N'IF', N'TF'))
            BEGIN
                DROP FUNCTION fn_CalcularModa
            END
            
            GO
            -- =============================================
            -- Author:		<Author,,Name>
            -- Create date: <Create Date, ,>
            -- Description:	<Description, ,>
            -- =============================================
            CREATE FUNCTION [dbo].[fn_CalcularModa]
            (
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

	            INSERT INTO @MODAS (VALOR)
	            (
		            SELECT Nota
		            FROM [dbo].[Resposta] A 
		            WHERE PerguntaId = @pergundaId AND CategoriaId = @categoriaId
		            GROUP BY Nota HAVING COUNT(Nota) =
		            (SELECT TOP 1 COUNT(nota)
		            FROM [dbo].[Resposta] 
		            WHERE PerguntaId = @pergundaId AND CategoriaId = @categoriaId
		            GROUP BY nota
		            ORDER BY 1 DESC)
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
        ";
    }
}
