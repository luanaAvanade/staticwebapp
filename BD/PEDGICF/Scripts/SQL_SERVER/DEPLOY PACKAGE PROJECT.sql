EXECUTE AS LOGIN = 'DEVELOPER\wcorreia'

DECLARE @folderName nvarchar(500) = 'PED653'
DECLARE @projectName nvarchar(500) = 'PED653'
DECLARE @ProjectBinary AS VARBINARY(MAX)  
DECLARE @operation_id AS BIGINT
  
SET @ProjectBinary = (SELECT * FROM OPENROWSET(BULK 'D:\dtsx\PED653.ispac', SINGLE_BLOB) as BinaryData)  

EXEC [SSISDB].[catalog].[deploy_project]
@folder_name = @folderName, 
@project_name = @projectName, 
@Project_Stream = @ProjectBinary, 
@operation_id = @operation_id OUT  


--Server=192.168.46.87\sql2014;Database=PD653;User Id=U_PD653;Password=U_PD6533;MultipleActiveResultSets=true
/*
EXECUTE AS LOGIN = 'DEVELOPER\wcorreia'
EXEC [SSISDB].[catalog].[create_environment] 
@environment_name=N'Dev', 
@environment_description=N'', 
@folder_name=N'PED653'

EXECUTE AS LOGIN = 'DEVELOPER\wcorreia'
EXEC [SSISDB].[catalog].[create_environment_variable] 
@variable_name=N'FilesDir', 
@sensitive=False, 
@description=N'', 
@environment_name=N'Dev', 
@folder_name=@folderName, 
@value=N'd:\dstx', 
@data_type=N'String'
*/

DECLARE @connectionString sql_variant = N'Data Source=192.168.46.87\sql2014;User Id=U_PD653;Password=U_PD6533;Initial Catalog=PD653;Provider=SQLNCLI11.1;Persist Security Info=True;Auto Translate=False;'
DECLARE @fileDir nvarchar(500) = N'D:\dstx'
DECLARE @filePath nvarchar(500)
DECLARE @filePathParam sql_variant

--#region PKG_PD_CATEGORIAS

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_CATEGORIAS.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString
--#endregion

--#region Volume de Compras PKG_PD_P1_STG1_PESO_VALOR_COMPRA.dtsx 

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P1_FT_PESO_VALOR_COMPRA.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P1_STG1_PESO_VALOR_COMPRA.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

SET @filePath = FORMATMESSAGE('%s\Volume de Compras.xlsx',@fileDir) 
SET @filePathParam = CONVERT(sql_variant,@filePath) 

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.ARQ_VOLUME_COMPRA.ExcelFilePath', 
@object_name=N'PKG_PD_P1_STG1_PESO_VALOR_COMPRA.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @filePathParam

--#endregion


--============== PKG_PD_P4_FT_NOTAS.dtsx ==============

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P4_FT_NOTAS.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P4_STG1_CONTRATOS_PEDIDOS.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

SET @filePath = FORMATMESSAGE('%s\Contratos e Pedidos.xlsx',@fileDir) 
SET @filePathParam = CONVERT(sql_variant,@filePath) 

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.Arq_Contratos_Pedidos.ExcelFilePath', 
@object_name=N'PKG_PD_P4_STG1_CONTRATOS_PEDIDOS.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @filePathParam

--===================================================================

--============== PKG_PD_P4_STG1_LEADTIME.dtsx ==============

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P4_STG1_LEADTIME.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @connectionString

SET @filePath = FORMATMESSAGE('%s\LeadTime.xlsx',@fileDir) 
SET @filePathParam = CONVERT(sql_variant,@filePath) 

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.Arq_Leadtime.ExcelFilePath', 
@object_name=N'PKG_PD_P4_STG1_LEADTIME.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @connectionString

--===================================================================

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P4_STG1_PESO_FORNECEDOR.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P5_FT_NOTAS.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P5_STG_NUM_FORNECEDOR.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P6_FT_NOTAS.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PD_P6_STG1_NUM_FORNECEDOR.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value=@connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PROCESSA_P4.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @connectionString

EXEC [SSISDB].[catalog].[set_object_parameter_value] 
@object_type=30, 
@parameter_name=N'CM.U_PD653.ConnectionString', 
@object_name=N'PKG_PROCESSA_PERGUNTA_FORMULARIO.dtsx', 
@folder_name=@folderName, 
@project_name=@projectName, 
@value_type=V, 
@parameter_value= @connectionString


--exec [dbo].[CALL_SSIS_PACKAGE] 'DEVELOPER\wcorreia', NULL, @p_codigo_pergunta = 1
/*

EXECUTE AS LOGIN = 'DEVELOPER\wcorreia'

DECLARE @var sql_variant = N'C:\teste\'
EXEC [SSISDB].[catalog].[create_environment_variable] @variable_name=N'ARQ_VOLUME_COMPRA', @sensitive=False, @description=N'', @environment_name=N'Dev', @folder_name=@folderName, @value=@var, @data_type=N'String'
GO

GO

GO*/