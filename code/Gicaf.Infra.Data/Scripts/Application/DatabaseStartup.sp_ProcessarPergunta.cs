using System;
using System.Collections.Generic;
using System.Text;

namespace Gicaf.Infra.Data.Scripts
{
    public partial class DatabaseStartup
    {
        public const string SP_ProcessaPergunta =
        @"
            /****** Object:  StoredProcedure [dbo].[sp_ProcessarPergunta]    Script Date: 2019-12-02 12:43:06 PM ******/
            IF EXISTS (SELECT  * FROM  sys.objects WHERE  object_id = OBJECT_ID(N'sp_ProcessarPergunta') AND type IN (N'P', N'PC' ))
            BEGIN
	            DROP PROCEDURE sp_ProcessarPergunta
            END
            GO

            /****** Object:  StoredProcedure [dbo].[sp_ProcessarPergunta]    Script Date: 2019-12-02 12:43:06 PM ******/
            SET ANSI_NULLS ON
            GO

            SET QUOTED_IDENTIFIER ON
            GO

            -- =============================================
            -- Author:		Daniel Carvalho Souza
            -- Create date: 19/07/2019
            -- Description:	
            -- =============================================
            CREATE PROCEDURE [dbo].[sp_ProcessarPergunta]
	            @p_arquivo_proc_pergunta_id bigint = NULL,
	            @p_codigo_pergunta bigint =	NULL,
	            @p_job_name nvarchar(128)
            AS
            BEGIN
	            --DECLARE @job_name nvarchar(128)
	            declare @execution_message nvarchar(500) = NULL
	            declare @status smallint = 2
	            declare @origemDados int = NULL
	            declare @error_number bigint = null 
	            declare	@error_state smallint = null
	            declare	@error_severity smallint = null 
	            DECLARE @execution_id bigint
	            DECLARE @versaoMecId bigint 
	
	            SELECT @versaoMecId = Id FROM VersaoMec WHERE DataEncerramento IS NULL
	            -- SET NOCOUNT ON added to prevent extra result sets from
	            -- interfering with SELECT statements.
	            SET NOCOUNT ON;
	
	            IF(@p_codigo_pergunta IS NULL)
	            BEGIN
		            SELECT @p_codigo_pergunta = per.Codigo, @origemDados = per.OrigemDados 
		            FROM ArquivoProcessamentoPergunta arqp 
		            INNER JOIN Pergunta per on arqp.PerguntaId = per.Id
		            WHERE arqp.Id = @p_arquivo_proc_pergunta_id
	            END
	            ELSE
	            BEGIN
		            SELECT @origemDados = per.OrigemDados 
		            FROM Pergunta per
		            WHERE per.Codigo = @p_codigo_pergunta
	            END

	            BEGIN TRY
		            IF(@p_job_name is not null)
		            BEGIN
			
			            IF(ISNULL(@origemDados,0) = 1)
			            BEGIN
				            IF OBJECT_ID('tempdb..##ParametroPergunta') is not null
				            BEGIN
					            DROP TABLE ##ParametroPergunta
				            END

				            CREATE TABLE ##ParametroPergunta
				            (NumeroPergunta INT)

				            INSERT INTO ##ParametroPergunta VALUES(@p_codigo_pergunta)
			            END

			            SET NOCOUNT ON
			            DECLARE @jobID UNIQUEIDENTIFIER,
					            @maxID INT,
					            @runStatus INT,
					            @rc INT

			            SELECT @jobID = job_id
			            FROM   msdb..sysjobs
			            WHERE name = @p_job_name

			            SELECT @maxID = MAX(instance_id)
			            FROM   msdb..sysjobhistory
			            WHERE job_id = @jobID
				               AND step_id = 0

			            SET  @maxID = COALESCE(@maxID, -1)

			            EXEC @rc = msdb..sp_start_job @job_name = @p_job_name

			            WHILE (SELECT MAX(instance_id) FROM msdb..sysjobhistory WHERE job_id = @jobID AND step_id = 0) = @maxID
				              WAITFOR DELAY '00:00:01'

			            SELECT @maxID = MAX(instance_id)
			            FROM   msdb..sysjobhistory
			            WHERE job_id = @jobID
				               AND step_id = 0

			            SELECT @runStatus = run_status
			            FROM   msdb..sysjobhistory
			            WHERE instance_id = @maxID

			            IF(@runStatus <> 1)
			            BEGIN
				            SET @execution_message = 'ERRO PACOTE SSIS'
			            END
                        ELSE
			            BEGIN
				            SET @status = 1
			            END
		            END
		            ELSE
		            BEGIN
			            SET @execution_message = 'PACOTE SSIS INVÁLIDO'
		            END
	            END TRY
	            BEGIN CATCH
		            SET @execution_message = ERROR_MESSAGE() 
		            SET @error_number = ERROR_NUMBER() 
		            SET @error_severity = ERROR_SEVERITY() 
	            END CATCH
	
	            IF(@p_arquivo_proc_pergunta_id IS NOT NULL)
	            BEGIN
		            UPDATE ArquivoProcessamentoPergunta 
		            SET [Status] = @status, 
			            [DataProcessamento] = GETDATE(),
			            [Mensagem] = @execution_message
		            WHERE Id = @p_arquivo_proc_pergunta_id
	            END
	
	            IF(@execution_message IS NOT NULL)
	            BEGIN
		            RAISERROR (@execution_message, @error_severity, @error_state);
	            END
            END
            GO
        ";
    }
}
