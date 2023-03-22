all:
	dotnet run
win64:
	dotnet publish -r win-x64 -c Release -o ~/dev/Executables/todo-latest-win64/ -p:PublishAot=false --sc
linux64:
	dotnet publish -r linux-x64 -c Release -o ~/dev/Executables/todo-aot-x64/ -p:PublishSingleFile=false
