using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string FN_CalcularMedia =
        @"
            IF EXISTS (SELECT * FROM sysobjects WHERE id = object_id(N'fn_CalcularMedia') AND xtype IN (N'FN', N'IF', N'TF'))
            BEGIN
                DROP FUNCTION fn_CalcularMedia
            END

            /****** Object:  UserDefinedFunction [dbo].[fn_CalcularMedia]    Script Date: 2019-12-02 5:54:06 PM ******/
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
	            INNER JOIN [dbo].[VersaoMec]C ON (C.Id = B.VersaoMecId AND C.DataEncerramento IS NULL)
	            WHERE PERGUNTAID = @pergundaId AND CategoriaId = @categoriaId
	            GROUP BY B.CODIGO

	            RETURN @Media
            END
            GO
        ";
    }
}
