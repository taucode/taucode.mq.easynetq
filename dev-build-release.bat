dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\tests\TauCode.Mq.EasyNetQ.Tests\TauCode.Mq.EasyNetQ.Tests.csproj
dotnet test -c Release .\tests\TauCode.Mq.EasyNetQ.Tests\TauCode.Mq.EasyNetQ.Tests.csproj

nuget pack nuget\TauCode.Mq.EasyNetQ.nuspec
