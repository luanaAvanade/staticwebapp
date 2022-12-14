IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'CALL_SSIS_PACKAGE')
BEGIN
    DROP PROCEDURE CALL_SSIS_PACKAGE
END

/****** Object:  StoredProcedure [dbo].[CALL_SSIS_PACKAGE]    Script Date: 06/09/2019 18:53:29 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Daniel Carvalho Souza
-- Create date: 19/07/2019
-- Description:	
-- =============================================
CREATE PROCEDURE [dbo].[CALL_SSIS_PACKAGE]
	@ssis_login nvarchar(128),
	@p_arquivo_proc_pergunta_id bigint = NULL,
	@p_codigo_pergunta bigint =	NULL
AS
BEGIN
	DECLARE @package_name nvarchar(128)
	declare @execution_status int = NULL
	declare @execution_message nvarchar(500) = NULL
	declare @status smallint = 2
	declare @origemDados int = NULL
	declare @error_number bigint = null 
	declare	@error_state smallint = null
	declare	@error_severity smallint = null 
	DECLARE @execution_id bigint
		
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
		SELECT @package_name = 
		CASE WHEN @origemDados = 1 THEN 'PKG_PROCESSA_PERGUNTA_FORMULARIO.dtsx' ELSE 
			CASE (@p_codigo_pergunta) 
				WHEN 1 THEN 'PKG_PROCESSA_P1.dtsx' 
				WHEN 4 THEN 'PKG_PROCESSA_P4.dtsx' 
				WHEN 5 THEN 'PKG_PROCESSA_P5.dtsx' 
				WHEN 6 THEN 'PKG_PROCESSA_P6.dtsx' 
			ELSE NULL 
			END 
		END

		IF(@package_name is not null)
		BEGIN
			EXEC @execution_status = [SSISDB].[dbo].[exec_mec_package] 	
				 @ssis_login = @ssis_login,	@package_name = @package_name, @codigo_pergunta = @p_codigo_pergunta
		
			IF(@execution_status = 7)
			BEGIN
				SET @status = 1
				
				DELETE Resultado 
				WHERE PerguntaId = (SELECT Id FROM Pergunta WHERE Codigo = @p_codigo_pergunta)

				INSERT INTO Resultado(DataCriacao, DataModificacao, Nota, Moda, Media, Mediana, PerguntaId, CategoriaId, ArquivoProcessamentoPerguntaId)
				SELECT GETDATE(), GETDATE(), np.NOTA, np.Moda, np.Media, np.MEDIANA, np.ID_NUM_PERGUNTA, cat.Id, @p_arquivo_proc_pergunta_id
				FROM [dbo].[Pd_Ft_Notas_Perguntas] np
				INNER JOIN Categoria cat on np.COD_CATEGORIA = cat.Codigo 
				WHERE np.ID_NUM_PERGUNTA = @p_codigo_pergunta
			END
			ELSE
			BEGIN
				SET @execution_message = 'ERRO PACOTE SSIS'
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
	--INSERT INTO [LOG](DESCRICAO) VALUES (FORMATMESSAGE('origem dados: %s | msg: %s | pacote: %s', convert(varchar,@origemDados), @execution_message, @package_name)) 
	
	IF(@execution_message IS NOT NULL)
	BEGIN
		RAISERROR (@execution_message, @error_severity, @error_state);
	END
END

--EXEC [CALL_SSIS_PACKAGE] 'DEVELOPER\WCORREIA', NULL, 1

--EXECUTE AS login = 'DEVELOPER\WCORREIA'
--	select * from [SSISDB].catalog.executions
--REVERT