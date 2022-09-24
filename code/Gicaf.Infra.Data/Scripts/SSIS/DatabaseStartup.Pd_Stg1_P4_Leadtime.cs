using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string Pd_Stg1_P4_Leadtime =
        @"
            /****** Object:  Table [dbo].[Pd_Stg1_P4_Leadtime]    Script Date: 2019-12-02 8:34:20 PM ******/
            IF OBJECT_ID('dbo.Pd_Stg1_P4_Leadtime', 'U') IS NOT NULL 
	            DROP TABLE [dbo].[Pd_Stg1_P4_Leadtime]
            GO

            /****** Object:  Table [dbo].[Pd_Stg1_P4_Leadtime]    Script Date: 2019-12-02 8:40:21 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            CREATE TABLE [dbo].[Pd_Stg1_P4_Leadtime](
	        [Cod_Grupo] [nvarchar](255) NULL,
	        [Cod_Material] [nvarchar](255) NULL,
	        [Dsc_Material] [nvarchar](255) NULL,
	        [Qtd_Leadtime] [nvarchar](255) NULL
            ) ON [PRIMARY]
            GO
        ";
    }
}
