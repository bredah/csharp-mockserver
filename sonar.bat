@echo off
set /p token=<sonar.txt
REM dotnet tool install --global dotnet-sonarscanner
dotnet sonarscanner begin /k:"bredah_csharp-mockserver" /o:"bredah-github" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login=%token%  /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.cs.opencover.reportsPaths="${dir}/lcov.opencover.xml"
dotnet restore
dotnet build
dotnet test MockServer.Tests/MockServer.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet sonarscanner end /d:sonar.login=%token%