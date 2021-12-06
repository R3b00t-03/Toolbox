
dotnet.exe build --configuration release
Compress-Archive -Path ".\Pinger\bin\Release\net6.0\*" ".\_BuildOutput\PingLogger_Win64x.zip" -Force
Compress-Archive -Path ".\DirectoryAccessCrawler\bin\Release\net6.0\*" ".\_BuildOutput\DirectoryAccessCrawler_Win64x.zip" -Force

Pause