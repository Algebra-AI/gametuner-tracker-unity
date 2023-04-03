#!/bin/sh
unity_editor_path="/home/djol/Unity/Hub/Editor/2021.3.18f1/Editor/Unity"
if [ ! -z "$1" ]
then
    unity_editor_path=$1
fi
$unity_editor_path -exportPackage Assets/Plugins/GametunerTracker ../Resources/GametunerTracker.unitypackage -projectPath SnowplowTracker.Demo -quit -batchmode
