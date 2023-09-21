#!/bin/sh
#define absolute path to game project and snowplow plugin folder
#build release dll and copy .dll file to Demo, Tests and wokawoka project
#you can call this script with command " sh build.sh "Release" "ABSOLUTE_PATH_TO_DLL_IN_UNITY_PROJCT" "
release_mode="Release"
if [ ! -z "$1" ]
then
    release_mode=$1
fi

dotnet build GametunerTracker.csproj --configuration $release_mode --framework netstandard2.0
cp bin/$release_mode/netstandard2.0/GametunerTracker.dll ../../SnowplowTracker.Demo/Library/PackageCache/com.algebraai.gametunertracker@8489a986bf//GametunerTracker.dll
cp bin/$release_mode/netstandard2.0/GametunerTracker.dll ../../SnowplowTracker.Tests/Assets/Plugins/GametunerTracker/GametunerTracker.dll
cp bin/$release_mode/netstandard2.0/GametunerTracker.dll ../../unity-package/GametunerTracker.dll