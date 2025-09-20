@echo off
setlocal

REM Genera reporte HTML desde todos los coverage.cobertura.xml encontrados
echo Generando reporte de cobertura en carpeta Report...
reportgenerator -reports:"TestResults/**/coverage.cobertura.xml" -targetdir:"Report" -reporttypes:Html

echo.
echo ======= Reporte generado =======
echo Abre Report\index.html en tu navegador
echo.
pause
