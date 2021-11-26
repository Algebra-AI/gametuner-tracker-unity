using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public static class BuildCustom
{
    [MenuItem("Tools/Build_WebGL")]
    public static void BuildWebGLBase()
    {
        //var outputdir = Path.Combine(Getoutputdirectory(), "wokawoka-web");
        try
        {
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
        }
        catch (Exception ex)
        {
            EditorUtility.DisplayDialog("Error", "An error occured: " + Environment.NewLine + Environment.NewLine + ex.ToString(), "OK");
        }
        /*
        try
        {
            // Set correct editor params
            EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.WebGL, BuildTarget.WebGL);
           
            //EditorUserBuildSettings.development = true;
            EditorUserBuildSettings.development = false;

            PlayerSettings.SplashScreen.showUnityLogo = false;
            PlayerSettings.SplashScreen.show = false;
            PlayerSettings.bundleVersion = Version;
            PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.WebGL, "NO_GPGS");

            if (System.IO.Directory.Exists("Assets/ResourcesBundle/Resources"))
            {
                // Copy over level files above 1000
                var sourceLevelManifestPath = "Assets/Resources/LevelsJSON/Android/levelsData.json";
                var sourceLevelManifestData = File.ReadAllText(sourceLevelManifestPath).Split(new[] { "level1000" }, StringSplitOptions.None)[1];
                var destLevelManifestPath = "Assets/Resources/LevelsJSON/WebGL/levelsData.json";
                var destLevelManifestData = File.ReadAllText(destLevelManifestPath).Split(new[] { "level1000" }, StringSplitOptions.None)[0];
                File.WriteAllText(destLevelManifestPath, destLevelManifestData + "level1000" + sourceLevelManifestData);

                var levelToCopy = 1;
                while (File.Exists("Assets/Resources/LevelsJSON/Android/level" + levelToCopy + ".json"))
                {
                    if (File.Exists("Assets/Resources/LevelsJSON/WebGL/level" + levelToCopy + ".json"))
                        File.Delete("Assets/Resources/LevelsJSON/WebGL/level" + levelToCopy + ".json");

                    File.Copy("Assets/Resources/LevelsJSON/Android/level" + levelToCopy + ".json", "Assets/Resources/LevelsJSON/WebGL/level" + levelToCopy + ".json");
                
                    levelToCopy++;
                }

                // Set background bundles
                var bgPath = "Assets/ResourcesBundle/Resources/Backgrounds";
                var bgFiles = Directory.GetFiles(bgPath);

                foreach (var bg in bgFiles)
                {
                    if (bg.EndsWith(".meta"))
                        continue;

                    var name = Path.GetFileNameWithoutExtension(bg);
                    var num = name.Substring(3);
                    AssetImporter.GetAtPath(bg).SetAssetBundleNameAndVariant("backgrounds/" + num, "");
                }

                // Set map part bundles
                var mpPath = "Assets/ResourcesBundle/Resources/MapParts/Segments";
                var mpFiles = Directory.GetFiles(mpPath);

                foreach (var mp in mpFiles)
                {
                    if (mp.EndsWith(".meta"))
                        continue;

                    var name = Path.GetFileNameWithoutExtension(mp);
                    var num = Convert.ToInt32(name.Substring(10));

                    if (num < 40)
                        continue;

                    AssetImporter.GetAtPath(mp).SetAssetBundleNameAndVariant("map/" + (num - 8), "");
                }

                // Set map image bundles
                var miPath = "Assets/ResourcesBundle/Resources/MapImages";
                var miFiles = Directory.GetFiles(miPath);

                foreach (var mi in miFiles)
                {
                    if (mi.EndsWith(".meta"))
                        continue;

                    var name = Path.GetFileNameWithoutExtension(mi);
                    var regExp = @"map_bkg_([0-9]+)_Path.*";
                    var match = Regex.Match(name, regExp);

                    if (!match.Success)
                        continue;

                    var num = Convert.ToInt32(match.Groups[1].Value);
                    if (num < 30)
                        continue;

                    AssetImporter.GetAtPath(mi).SetAssetBundleNameAndVariant("map/" + num , "");
                }

                // Build bundles
                var bundlesDir = Path.Combine(Path.Combine(outputdir, "WebGL"), "WebGL");
                if (!Directory.Exists(bundlesDir))
                    Directory.CreateDirectory(bundlesDir);
                BuildPipeline.BuildAssetBundles(bundlesDir, BuildAssetBundleOptions.None, EditorUserBuildSettings.activeBuildTarget);

                // Copy over extra bundles
                var videosDir = Path.Combine(bundlesDir, "Videos");
                FileUtil.CopyFileOrDirectory("Assets/ResourcesBundle/WebVideos", videosDir);

                // Lets refresh before moving to be safe
                AssetDatabase.Refresh(ImportAssetOptions.Default);

                // Move bundles directory
                FileUtil.DeleteFileOrDirectory("Assets/ResourcesBundle/Bundles");
                FileUtil.MoveFileOrDirectory("Assets/ResourcesBundle/Resources", "Assets/ResourcesBundle/Bundles");

                // Refresh assets once moved
                AssetDatabase.Refresh(ImportAssetOptions.Default);
            }

            // Set correct download url
            var settingsPath = "Assets/Scripts/GameSettings.cs";
            var settingsText = File.ReadAllText(settingsPath);
            settingsText = Regex.Replace(settingsText, 
                "\"https:\\/\\/wokawokagame\\.herokuapp\\.com\\/v([0123456789\\.,]+)\\/WebGL\\/\";", 
                "\"https://wokawokagame.herokuapp.com/v" + Version + "/WebGL/\";"
            );
            File.WriteAllText(settingsPath, settingsText);

            if (switching)
                return;

            EditorUserBuildSettings.SetBuildLocation(BuildTarget.WebGL, outputdir);

            // Create output dir
            if (!Directory.Exists(outputdir))
                Directory.CreateDirectory(outputdir);

            // Build player
            var playerOptions = new BuildPlayerOptions();
            playerOptions.locationPathName = outputdir;
            playerOptions.target = BuildTarget.WebGL;
            playerOptions.scenes = new[] {
                "Assets/Scene/SceneLoading.unity",
                "Assets/Scene/ScenePlay.unity",
                "Assets/Scene/SceneMap.unity",
                "Assets/Scene/SceneGame.unity",
                "Assets/Scene/SceneInGameLoading.unity",
                "Assets/Scene/SceneArena.unity",
                "Assets/Scene/SceneAroundTheWorld.unity",
                "Assets/Scene/ScenePachinko.unity"
            };
            var buildResult = BuildPipeline.BuildPlayer(playerOptions);
            if (buildResult.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
                throw new Exception("Error: " + buildResult.ToString());

            // Delete streaming assets
            Directory.Delete(Path.Combine(outputdir, "StreamingAssets"), true);

            // Show the generated files?
            if (IsCalledFromCmd)
                EditorApplication.Exit(0);
            else
                Process.Start(outputdir);
        }
        catch (Exception ex)
        {
            if (IsCalledFromCmd)
            {
                UnityEngine.Debug.LogError("Wokamated error: " + ex.ToString());
                EditorApplication.Exit(1);
            }
            else
                EditorUtility.DisplayDialog("Error", "An error occured: " + Environment.NewLine + Environment.NewLine + ex.ToString(), "OK");
        }
        */
    }
}
