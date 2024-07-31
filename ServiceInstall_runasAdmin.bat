@echo off
rem Memorizza il path completo del server
set CURR_SERVER=%cd%\bin\Release\net8.0\ErpToolkit.exe
rem Visualizza il server
echo Il server corrente è: %CURR_SERVER%
pause
rem carica il server come servizio windows
rem dotnet publish ErpToolkit.csproj -p:Configuration=Release
rem sc create ErpToolkitService binPath=%CURR_SERVER% start=delayed-auto
SC DELETE ErpToolkitService 
pause