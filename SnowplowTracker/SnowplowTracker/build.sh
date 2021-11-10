#!/bin/sh
#define absolute path to game project and snowplow plugin folder
#build release dll and copy .dll file to Demo, Tests and wokawoka project
#you can call this script with command " sh build.sh "Release" "ABSOLUTE_PATH_TO_DLL_IN_UNITY_PROJCT" "
unity_project_plugin_path="/home/djol/Projects/wokawoka/Assets/Plugins/SnowplowAnalytics/SnowplowTracker.dll"
if [ ! -z "$2" ]
then
    unity_project_plugin_path=$2
fi
dotnet build SnowplowTracker.csproj --configuration $1 --framework netstandard2.0
cp bin/$1/netstandard2.0/SnowplowTracker.dll ../../SnowplowTracker.Demo/Assets/Plugins/SnowplowTracker/SnowplowTracker.dll
cp bin/$1/netstandard2.0/SnowplowTracker.dll ../../SnowplowTracker.Tests/Assets/Plugins/SnowplowTracker/SnowplowTracker.dll
cp bin/$1/netstandard2.0/SnowplowTracker.dll $unity_project_plugin_path