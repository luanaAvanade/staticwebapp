using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Stg1_P4_Peso_Fornecedor =
        @"
            /****** Object:  Table [dbo].[Pd_Stg1_P4_Peso_Fornecedor]    Script Date: 2019-12-02 8:34:20 PM ******/
            IF OBJECT_ID('dbo.Pd_Stg1_P4_Peso_Fornecedor', 'U') IS NOT NULL 
	            DROP TABLE [dbo].[Pd_Stg1_P4_Peso_Fornecedor]
            GO

            /****** Object:  Table [dbo].[Pd_Stg1_P4_Peso_Fornecedor]    Script Date: 2019-12-02 8:40:21 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Stg1_P4_Peso_Fornecedor](
	        [Dt_Entrada_MS] [datetime] NULL,
	        [Dt_Remessa] [datetime] NULL,
	        [Dt_Assinatura] [datetime] NULL,
	        [Num_Prazo_Mobilizacao] [float] NULL,
	        [Num_Doc_Compra] [nvarchar](255) NULL,
	        [Dt_Doc_Compra] [datetime] NULL,
	        [Num_Licitacao] [nvarchar](255) NULL,
	        [Dsc_Modalidade] [nvarchar](255) NULL,
	        [Dsc_Categoria] [nvarchar](255) NULL
            ) ON [PRIMARY]
            GO
        ";
    }
}
