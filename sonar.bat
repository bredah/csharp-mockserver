@echo off
set /p token=<sonar.txt
dotnet sonarscanner begin /k:"bredah:csharp-mockserver" /n:"CSharp MockServer" /v:"1.0.0" /o:"bredah" /d:sonar.host.url="https://sonarcloud.io" /d:sonar.login="%token%" /d:sonar.language="cs" /d:sonar.exclusions="**/bin/**/*,**/obj/**/*" /d:sonar.coverage.exclusions="Project.Tests/**,**/*Tests.cs" /d:sonar.cs.opencover.reportsPaths="%cd%\lcov.opencover.xml"
dotnet restore
dotnet build
dotnet test MockServer.Tests/MockServer.Tests.csproj /p:CollectCoverage=true /p:CoverletOutputFormat=\"opencover,lcov\" /p:CoverletOutput=../lcov
dotnet sonarscanner end /d:sonar.login="%token%"