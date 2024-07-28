cd C:\dhe_desktop\dev\wbs\ErpToolkit
rem dotnet publish ErpToolkit.csproj -p:Configuration=Release
rem sc create ErpToolkitService binPath="C:\dhe_desktop\dev\wbs\ErpToolkit\bin\Release\net8.0\TaskScheduler.exe" start=delayed-auto
SC DELETE ErpToolkitService 
pause