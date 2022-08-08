dotnet restore

dotnet build --configuration Debug
dotnet build --configuration Release

dotnet test -c Debug .\test\TauCode.Mq.EasyNetQ.Tests\TauCode.Mq.EasyNetQ.Tests.csproj
dotnet test -c Release .\test\TauCode.Mq.EasyNetQ.Tests\TauCode.Mq.EasyNetQ.Tests.csproj

nuget pack nuget\TauCode.Mq.EasyNetQ.nuspec
