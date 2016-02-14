::dnu restore src\Lifecycle.AspNet\project.json
dnu pack src\Lifecycle.AspNet\project.json --configuration Release --out artifacts\bin\Lifecycle.AspNet

set /p version="Version: "
echo artifacts\bin\Lifecycle.AspNet\Release\Lifecycle.AspNet.%version%.nupkg
nuget push artifacts\bin\Lifecycle.AspNet\Release\Lifecycle.AspNet.%version%.nupkg