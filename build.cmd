@setlocal

dotnet restore
dotnet build --configuration Release
dotnet publish src\dotnetdoc.doc\dotnetdoc.doc.csproj --configuration Release --no-build --framework net45
dotnet publish src\dotnetdoc.doc\dotnetdoc.doc.csproj --configuration Release --no-build --framework net5-windows
dotnet pack src\dotnetdoc.doc\dotnetdoc.doc.csproj --configuration Release --no-build
