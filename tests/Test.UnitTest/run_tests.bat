@echo off
setlocal

REM Carpeta del proyecto de tests (ajusta según sea Unit o Integration)
set TEST_PROJECT=Test.UnitTest.csproj

echo Borrando subcarpetas antiguas de TestResults...
if exist "%~dp0TestResults" (
    for /d %%D in ("%~dp0TestResults\*") do (
        echo Eliminando carpeta %%D
        rmdir /s /q "%%D"
    )
)

REM Ejecuta los tests con cobertura
echo Ejecutando tests con cobertura en %TEST_PROJECT% ...
REM dotnet test %TEST_PROJECT% --collect:"XPlat Code Coverage"
dotnet test %TEST_PROJECT% --collect:"XPlat Code Coverage" --logger "trx;LogFileName=TestResult.trx"

echo.
echo ======= Tests finalizados =======
echo El coverage se encuentra en la carpeta TestResults
echo.

REM Borrar carpetas temporales extra creadas por dotnet test
for /d %%D in ("%~dp0TestResults\David_DG*") do (
    echo Eliminando carpeta temporal %%D
    rmdir /s /q "%%D"
)

pause

