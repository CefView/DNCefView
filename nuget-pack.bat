set CONFIG=%1
nuget pack CCefView.nuspec -BasePath "out\%CONFIG%" -OutputFileNamesWithoutVersion
nuget add CCefView.nupkg -Source "%temp%\cefviewnugetfeed"