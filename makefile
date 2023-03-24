all:
	dotnet run
win64:
	dotnet publish -r win-x64 -c Release -p:PublishAot=false --sc
aotforme:
	dotnet publish -r linux-x64 -c Release -o ~/dev/Executables/todo-aot-x64/ -p:PublishSingleFile=false
