using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string FN_CalcularMediana =
        @"
            IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'fn_CalcularMediana') AND xtype IN (N'FN', N'IF', N'TF'))
            BEGIN
                DROP FUNCTION fn_CalcularMediana
            END            

            /****** Object:  UserDefinedFunction [dbo].[fn_CalcularMediana]    Script Date: 2019-12-02 5:52:57 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            -- =============================================
            -- Author:		<Author,,Name>
            -- Create date: <Create Date, ,>
            -- Description:	<Description, ,>
            -- =============================================
            CREATE FUNCTION [dbo].[fn_CalcularMediana]
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
	            INNER JOIN [dbo].[VersaoMec]C ON (C.Id = B.VersaoMecId AND C.DataEncerramento IS NULL)
	            WHERE PerguntaId = @pergundaId AND CategoriaId = @categoriaId
	            ORDER BY B.CODIGO

	            RETURN @mediana
            END
            GO
        ";
    }
}
