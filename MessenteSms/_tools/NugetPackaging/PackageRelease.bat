@ECHO OFF
..\..\..\.nuget\nuget.exe pack -Outputdirectory output -Build -Properties Configuration=Release ..\..\MessenteSms.csproj
pause