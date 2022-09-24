using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string SP_CalcularMatriz =
        @"
            IF EXISTS (SELECT  * FROM  sys.objects WHERE  object_id = OBJECT_ID(N'sp_CalcularMatriz') AND type IN (N'P', N'PC' ))
            BEGIN
	            DROP PROCEDURE sp_CalcularMatriz
            END
            GO

            /****** Object:  StoredProcedure [dbo].[sp_CalcularMatriz]    Script Date: 2019-12-02 5:23:01 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            -- =============================================
            -- Author:		Daniel Carvalho Souza
            -- Create date: <Create Date,,>
            -- Description:	<Description,,>
            -- =============================================
            CREATE PROCEDURE [dbo].[sp_CalcularMatriz]
	            @versaoId bigint
            AS
            BEGIN
	            DECLARE @p_formulaX NVARCHAR(MAX) = null 
	            DECLARE @p_formulaY NVARCHAR(MAX) = null 
	            DECLARE @formulaX NVARCHAR(MAX) = null
	            DECLARE @formulaY NVARCHAR(MAX) = null
	            DECLARE @categoriaId bigint = null
	            DECLARE @sql VARCHAR(MAX) = null
	            DECLARE @valorX float = null
	            DECLARE @valorY float = null
	            DECLARE @estimativaGastoMensal float = NULL
	            DECLARE @quadranteId int = NULL
	            DECLARE @quadrante varchar(max) = NULL
	
	            SELECT @p_formulaX = [FormulaEixoX], @p_formulaY = [FormulaEixoY] FROM [dbo].[VersaoMec] WHERE Id = @versaoId

	            DELETE AvaliacaoCategoria where VersaoMecId = @versaoId

	            DECLARE CUR CURSOR 
		            LOCAL STATIC READ_ONLY FORWARD_ONLY
	            FOR 
		            SELECT Categoria.Id FROM Categoria 
		            INNER JOIN VersaoMec ON Categoria.VersaoMecId = VersaoMec.Id
		            WHERE VersaoMec.DataEncerramento IS NULL
	            OPEN CUR
		            FETCH NEXT FROM CUR INTO @categoriaId
	
	            WHILE @@FETCH_STATUS = 0
	            BEGIN 
		            select @formulaX = dbo.fn_ConstruirFormula(@p_formulaX,@categoriaId)
		            select @formulaY = dbo.fn_ConstruirFormula(@p_formulaY,@categoriaId)

		            SELECT @estimativaGastoMensal = ROUND(sum(a.estimativa_vlr_mensal),0)
		            FROM 
	                [dbo].[Pd_Ft_Notas_Perguntas] a inner join [dbo].VersaoMec b on(b.Id = a.ID_VERSAO and b.DataEncerramento is null)
		            WHERE a.cod_categoria is not null and a.estimativa_vlr_mensal is not null
		            and a.cod_categoria = (SELECT cat.Codigo FROM Categoria cat WHERE cat.Id = @categoriaId)
		            GROUP BY a.cod_categoria
		
		            set @quadrante = 
		            N'CASE  WHEN '+@formulaX+' > 5 AND '+@formulaY+' > 5 THEN 1 
			            WHEN '+@formulaX+' > 5 AND '+@formulaY+' < 5 THEN 2
			            WHEN '+@formulaX+' < 5 AND '+@formulaY+' > 5 THEN 4
			            WHEN '+@formulaX+' < 5 AND '+@formulaY+' < 5 THEN 3
			            ELSE 5
		              END'
		
		            set @sql = '
			            INSERT INTO AvaliacaoCategoria(DataCriacao,DataModificacao,CategoriaId,EixoX,EixoY,QuadranteId,EstimativaGastoMensal,GastoTotal,RiscoOperativo,RiscoQualidade, VersaoMecId)
			            VALUES
			            (GETDATE(),GETDATE(),'+ convert(varchar, @categoriaId) +','+ @formulaX +','+ @formulaY +','+ @quadrante +','+convert(varchar, @estimativaGastoMensal)+',0,0,0, '+convert(varchar,@versaoId)+')
		            '
		            exec(@sql)

		            FETCH NEXT FROM CUR INTO @categoriaId
	            END
	            CLOSE CUR
	            DEALLOCATE CUR
            END
            GO
        ";
    }
}
