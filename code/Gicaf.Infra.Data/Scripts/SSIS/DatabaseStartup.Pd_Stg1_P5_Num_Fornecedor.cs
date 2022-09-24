using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Stg1_P5_Num_Fornecedor =
        @"
            /****** Object:  Table [dbo].[Pd_Stg1_P5_Num_Fornecedor]    Script Date: 2019-12-02 8:34:20 PM ******/
            IF OBJECT_ID('dbo.Pd_Stg1_P5_Num_Fornecedor', 'U') IS NOT NULL 
	            DROP TABLE [dbo].[Pd_Stg1_P5_Num_Fornecedor]
            GO

            /****** Object:  Table [dbo].[Pd_Stg1_P5_Num_Fornecedor]    Script Date: 2019-12-02 8:40:21 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Stg1_P5_Num_Fornecedor](
	        [NUM_ANO] [float] NULL,
	        [NUM_LICITACAO] [nvarchar](255) NULL,
	        [COD_GRUPO_CATEGORIA] [float] NULL,
	        [COD_CATEGORIA] [nvarchar](255) NULL,
	        [DSC_CATEGORIA] [nvarchar](255) NULL,
	        [STATUS] [nvarchar](255) NULL,
	        [QTD_PROPONENTE] [float] NULL
            ) ON [PRIMARY]
            GO
        ";
    }
}
