using System;
using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace CureShot.Editor
{
    public static class BuildPlayer
    {
        private static readonly string[] Scenes = new string[]
        {
            "Assets/Scenes/MenuSence.unity",
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/ShopScene.unity",
            "Assets/Scenes/Final/GamePlaySence.unity",
            "Assets/Scenes/DaySummary.unity",
            "Assets/Scenes/Ending.unity"
        };

        [MenuItem("Build/Build Windows (.EXE)", priority = 100)]
        public static void BuildWindowsFromMenu()
        {
            PerformBuild(isBatchMode: false);
        }

        public static void BuildWindows()
        {
            PerformBuild(isBatchMode: true);
        }

        private static void PerformBuild(bool isBatchMode)
        {
            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            string workspaceRoot = Path.GetDirectoryName(projectRoot);
            string outputDir = Path.Combine(workspaceRoot, "Builds", "Windows");
            string outputExe = Path.Combine(outputDir, "CureShot.exe");

            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            Debug.Log("<color=cyan>[BuildPlayer] Bắt đầu đóng gói Windows 64-bit EXE...</color>");
            Debug.Log($"[BuildPlayer] Output Path: {outputExe}");
            Debug.Log($"[BuildPlayer] Số Scene: {Scenes.Length}");

            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
            {
                scenes = Scenes,
                locationPathName = outputExe,
                target = BuildTarget.StandaloneWindows64,
                options = BuildOptions.None
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;

            if (summary.result == BuildResult.Succeeded)
            {
                double sizeMb = summary.totalSize / (1024.0 * 1024.0);
                string msg = $"[BuildPlayer] BUILD THÀNH CÔNG! Dung lượng: {sizeMb:F1} MB, Thời gian: {summary.totalTime.TotalSeconds:F1}s";
                Debug.Log($"<color=green>{msg}</color>");

                if (!isBatchMode)
                {
                    EditorUtility.DisplayDialog("Build CureShot Thành Công", $"{msg}\n\nThư mục: {outputDir}", "OK");
                    EditorUtility.RevealInFinder(outputExe);
                }
                else
                {
                    EditorApplication.Exit(0);
                }
            }
            else
            {
                string errorMsg = $"[BuildPlayer] BUILD THẤT BẠI! Lỗi: {summary.totalErrors}, Cảnh báo: {summary.totalWarnings}";
                Debug.LogError(errorMsg);

                if (!isBatchMode)
                {
                    EditorUtility.DisplayDialog("Build Thất Bại", errorMsg, "Đóng");
                }
                else
                {
                    EditorApplication.Exit(1);
                }
            }
        }
    }
}

