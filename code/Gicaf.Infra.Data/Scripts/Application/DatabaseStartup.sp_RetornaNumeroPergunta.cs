using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string SP_RetornaNumeroPergunta =
        @"
            IF EXISTS (SELECT  * FROM  sys.objects WHERE  object_id = OBJECT_ID(N'RetornaNumeroPergunta') AND type IN (N'P', N'PC' ))
            BEGIN
	            DROP PROCEDURE [dbo].[RetornaNumeroPergunta]
            END
            GO            

            /****** Object:  StoredProcedure [dbo].[RetornaNumeroPergunta]    Script Date: 2019-12-02 5:20:18 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            -- =============================================
            -- Author:		<Author,,Name>
            -- Create date: <Create Date,,>
            -- Description:	<Description,,>
            -- =============================================
            CREATE PROCEDURE [dbo].[RetornaNumeroPergunta]
            AS
            BEGIN
	            return select top 1 NumeroPergunta from ##ParametroPergunta;
            END
            GO
        ";
    }
}
