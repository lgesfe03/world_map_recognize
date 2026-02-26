# world_map_recognize
app for learning world map recognize


Publish command
````
dotnet publish -c Release -r win-x64 --self-contained 

dotnet publish -c Release -r win-x64 --no-self-contained

dotnet publish -c Release -r win-x64 --self-contained -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true
````