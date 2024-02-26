@echo off

rem Set the command-line arguments
set args=-res 20 -console -image -ll

rem Call the C# executable with the arguments
rem dotnet asd_2_wquf_apps.dll %args%
cd .\bin\Release\net6.0\
asd_2_wquf_apps.exe %args%
cd ..\..\..\