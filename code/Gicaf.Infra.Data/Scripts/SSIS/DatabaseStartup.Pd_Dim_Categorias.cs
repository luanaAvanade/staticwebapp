using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Dim_Categorias =
        @"
            /****** Object:  Table [dbo].[Pd_Dim_Categorias]    Script Date: 2019-12-03 10:17:21 AM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Dim_Categorias](
	            [COD_GRUPO_CATEGORIA] [nvarchar](255) NOT NULL,
	            [DSC_GRUPO_CATEGORIA] [nvarchar](255) NULL,
	            [COD_CATEGORIA] [varchar](10) NULL,
	            [DSC_CATEGORIA] [nvarchar](255) NULL,
	            [TP_CATEGORIA] [varchar](10) NULL,
	            [ID_VERSAO] [bigint] NOT NULL,
             CONSTRAINT [PK_PD_Categoria] PRIMARY KEY CLUSTERED 
            (
	            [COD_GRUPO_CATEGORIA] ASC,
	            [ID_VERSAO] ASC
            )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
            ) ON [PRIMARY]
            GO
        ";
    }
}
