using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Stg1_P4_Contratos_Pedidos =
        @"
            /****** Object:  Table [dbo].[Pd_Stg1_P4_Contratos_Pedidos]    Script Date: 2019-12-02 8:34:20 PM ******/
            IF OBJECT_ID('dbo.Pd_Stg1_P4_Contratos_Pedidos', 'U') IS NOT NULL 
	            DROP TABLE [dbo].[Pd_Stg1_P4_Contratos_Pedidos]
            GO

            /****** Object:  Table [dbo].[Pd_Stg1_P4_Contratos_Pedidos]    Script Date: 2019-12-02 8:40:21 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Stg1_P4_Contratos_Pedidos](
	            [Cod_Item] [nvarchar](255) NULL,
	            [Dsc_Item] [nvarchar](255) NULL,
	            [Num_Grupo_Mercadoria] [nvarchar](255) NULL,
	            [Data_Doc_Compra] [datetime] NULL,
	            [Num_Doc_Compra] [nvarchar](255) NULL
            ) ON [PRIMARY]
            GO
        ";
    }
}
