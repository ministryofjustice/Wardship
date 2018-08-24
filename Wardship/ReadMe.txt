How to create the package

1) Update the TPLibrary.nuspec - new version, release notes, etc
2) Make sure the project compiles/runs as expected
3) Open a command line window (as an administrator) and go to the folder with the project (not the solution folder but the one inside)
4) Make sure NuGet is installed in your local machine
5) Type "nuget pack TPLibrary.csproj"
6) If it fails, you may have to run "nuget update -self" and then try step 5 again

The above will generate the package