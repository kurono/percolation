@echo off

rem Set the command-line arguments
set args=-res 20 -console -image -ll

rem Call the Java executable with the arguments
cd .\out\artifacts\asd_2_wquf_apps_jar
java -jar .\asd_2_wquf_apps.jar %args%
cd ..\..\..\
