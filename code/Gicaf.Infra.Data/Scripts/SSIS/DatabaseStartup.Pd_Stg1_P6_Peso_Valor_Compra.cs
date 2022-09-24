using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Stg1_P6_Peso_Valor_Compra =
        @"
            /****** Object:  Table [dbo].[Pd_Stg1_P6_Peso_Valor_Compra]    Script Date: 2019-12-02 8:34:20 PM ******/
            IF OBJECT_ID('dbo.Pd_Stg1_P6_Peso_Valor_Compra', 'U') IS NOT NULL 
	            DROP TABLE [dbo].[Pd_Stg1_P6_Peso_Valor_Compra]
            GO

            /****** Object:  Table [dbo].[Pd_Stg1_P6_Peso_Valor_Compra]    Script Date: 2019-12-02 8:40:21 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Stg1_P6_Peso_Valor_Compra](
	        [COD_CATEGORIA] [nvarchar](255) NULL,
	        [DSC_CATEGORIA] [nvarchar](255) NULL,
	        [TIPO] [nvarchar](255) NULL,
	        [NUM_FALTA_FALHA_INDICADOR] [float] NULL,
	        [NUM_ITENS_REMUN_REGULA] [float] NULL,
	        [NUM_LEGISLACAO_ESP] [float] NULL
            ) ON [PRIMARY]
            GO
        ";
    }
}
