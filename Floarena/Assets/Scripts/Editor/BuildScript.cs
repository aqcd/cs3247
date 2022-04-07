using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildScript {
    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildWindowsServer();
        BuildLinuxServer();
        BuildWindowsClient();
        BuildAndroidClient();
    }

    [MenuItem("Build/Build Server (Windows)")]
    public static void BuildWindowsServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainPage.unity", "Assets/Scenes/MapWithPlayer.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/Server/FlorarenaServer.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building Server (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Server (Windows).");
    }

    [MenuItem("Build/Build Server (Linux)")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainPage.unity", "Assets/Scenes/MapWithPlayer.unity" };
        buildPlayerOptions.locationPathName = "Builds/Linux/Server/FlorarenaServer.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building Server (Linux)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Server (Linux).");
    }


    [MenuItem("Build/Build Client (Windows)")]
    public static void BuildWindowsClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainPage.unity", "Assets/Scenes/MapWithPlayer.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/Client/Florarena.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.Development;

        Console.WriteLine("Building Client (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Client (Windows).");
    }

    [MenuItem("Build/Build Client (Android)")]
    public static void BuildAndroidClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainPage.unity", "Assets/Scenes/MapWithPlayer.unity" };
        buildPlayerOptions.locationPathName = "Builds/Android/Client/Florarena.apk";
        buildPlayerOptions.target = BuildTarget.Android;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.Development;

        Console.WriteLine("Building Client (Android)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Client (Android).");
    }
}