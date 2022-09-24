@ECHO OFF
SET caminhoInfra=code\Gicaf.Infra.Data
SET caminhoStartup=code\Gicaf.API

:UltimaMigracaoCodigo
FOR /F "tokens=* USEBACKQ" %%F IN (`dotnet ef migrations list  --project %caminhoInfra% --startup-project %caminhoStartup%`) DO (
  SET MigracaoAnterior=%%F
)
ECHO Migração Anterior: %MigracaoAnterior%
IF "%MigracaoAnterior%" == "No migrations were found." (
  ECHO Sem Migração Anterior
  GOTO Fim
)

:CriarMigracao
IF "%1" == "" (
  SET /p Migracao="Nome da Migração (apenas uma breve descrição, data e hora UTC são calculados automaticamente):"
) ELSE (
  SET Migracao=%1
)
dotnet ef migrations add %Migracao%  --project %caminhoInfra% --startup-project %caminhoStartup%
ECHO.
ECHO Revise a migração (a migração não detecta renames, hora certa de corrigir manualmente é agora).
SET /p revise="Enter para continuar"
dotnet ef migrations script %MigracaoAnterior%  --project %caminhoInfra% --startup-project %caminhoStartup% --output scripts\%Migracao%.sql
FOR /F "tokens=2 delims='" %%G in ('FINDSTR /R /C:"^VALUES.*" scripts\%Migracao%.sql') DO SET "Nome=%%G"
IF NOT "%Nome%" == "" REN scripts\%Migracao%.sql %Nome%.sql

:Fim
SET /p fim="Enter para finalizar"