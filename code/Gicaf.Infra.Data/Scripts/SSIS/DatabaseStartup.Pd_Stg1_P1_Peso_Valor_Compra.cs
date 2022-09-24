using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Stg1_P1_Peso_Valor_Compra =
        @"
            /****** Object:  Table [dbo].[Pd_Stg1_P1_Peso_Valor_Compra]    Script Date: 2019-12-02 8:34:20 PM ******/
            IF OBJECT_ID('dbo.Pd_Stg1_P1_Peso_Valor_Compra', 'U') IS NOT NULL 
	            DROP TABLE [dbo].[Pd_Stg1_P1_Peso_Valor_Compra]
            GO

            /****** Object:  Table [dbo].[Pd_Stg1_P1_Peso_Valor_Compra]    Script Date: 2019-12-02 8:34:20 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Stg1_P1_Peso_Valor_Compra](
	            [COD_CATEGORIA] [varchar](10) NULL,
	            [DSC_CATEGORIA] [nvarchar](255) NULL,
	            [NUM_DOC_COMPRA] [varchar](10) NULL,
	            [COD_FORNECEDOR] [varchar](6) NULL,
	            [VLR_LIQUIDO_CONTRATADO] [float] NULL,
	            [VLR_MESES_CONTRATO] [float] NULL,
	            [NUM_ANO_VIGENCIA] [int] NULL
            ) ON [PRIMARY]
            GO
        ";
    }
}
