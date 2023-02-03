dotnet restore

dotnet build TauCode.Mq.EasyNetQ.sln -c Debug
dotnet build TauCode.Mq.EasyNetQ.sln -c Release

dotnet test TauCode.Mq.EasyNetQ.sln -c Debug
dotnet test TauCode.Mq.EasyNetQ.sln -c Release

nuget pack nuget\TauCode.Mq.EasyNetQ.nuspec