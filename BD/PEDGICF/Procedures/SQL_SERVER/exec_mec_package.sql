USE [SSISDB]
GO

IF EXISTS(SELECT 1 FROM sys.procedures WHERE Name = 'exec_mec_package')
BEGIN
    DROP PROCEDURE exec_mec_package
END

/****** Object:  StoredProcedure [dbo].[exec_mec_package]    Script Date: 10/09/2019 10:12:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[exec_mec_package]
	@ssis_login nvarchar(128),
	@package_name nvarchar(128),
	@codigo_pergunta bigint = NULL,
	@status int output 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXECUTE AS LOGIN = @ssis_login

	DECLARE @execution_id bigint

	EXEC [SSISDB].[catalog].[create_execution] 
	@folder_name= 'PED653',
	@project_name= 'PED653',
	@package_name= @package_name,
	@use32bitruntime=False,
	@reference_id=Null,
	@execution_id= @execution_id OUTPUT
	
	--Select @execution_id
	--DECLARE @var0 smallint = 0
	EXEC [SSISDB].[catalog].[set_execution_parameter_value] 
		@execution_id,
		@object_type=50,
		@parameter_name=N'LOGGING_LEVEL',
		@parameter_value= 2
	
	--DECLARE @var1 bit = 0
	EXEC [SSISDB].[catalog].[set_execution_parameter_value] 
		@execution_id,  
		@object_type=50, 
		@parameter_name=N'DUMP_ON_ERROR', 
		@parameter_value= 0

	--Aguardar Execução
	EXEC [SSISDB].[catalog].[set_execution_parameter_value] 
		@execution_id,  
		@object_type=50, 
		@parameter_name=N'SYNCHRONIZED', 
		@parameter_value= 1

	IF(@codigo_pergunta IS NOT NULL)
	BEGIN
		-- parâmetro pacote
		EXEC [SSISDB].[catalog].[set_execution_parameter_value] 
			@execution_id,  
			@object_type=30, -- 20 : parâmetro projeto
			@parameter_name=N'NUM_PERGUNTA', 
			@parameter_value= @codigo_pergunta
	END

	EXEC [SSISDB].[catalog].[start_execution] @execution_id
	SELECT @status = [STATUS] FROM [SSISDB].[catalog].executions WHERE execution_id = @execution_id

	REVERT
END
