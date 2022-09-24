@ECHO OFF
SET caminhoInfra=code\EcoSis\Data
SET caminhoStartup=code\EcoSis\API


dotnet ef migrations add MigracaoInicial  --project %caminhoInfra% --startup-project %caminhoStartup%

:Fim
SET /p fim="Enter para finalizar"