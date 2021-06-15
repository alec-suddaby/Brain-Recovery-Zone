#if UNITY_IOS
using System;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using UnityEngine;

namespace Facebook.Audio
{
    public static class AddFrameworksToIOSProject
    {
        private const string simulatorFrameworkPath = "Audio360/Plugins/iOS-Simulator/";
        private const string deviceFrameworkPath = "Audio360/Plugins/iOS/";
        private const string frameworkName = "Audio360CSharp.framework";
        
        [PostProcessBuild]
        public static void OnPostprocessBuild(BuildTarget target, string pathToProject)
        {
            // open the project
            var project = new PBXProject();
            var pbxProjectPath = PBXProject.GetPBXProjectPath(pathToProject);
            project.ReadFromFile(pbxProjectPath);
            
            // get the path to the framework, depending on the iOS version (device or sim)
            var iosVersion = PlayerSettings.iOS.sdkVersion;
            string frameworkPath = "";
            switch (iosVersion)
            {
                case iOSSdkVersion.DeviceSDK:
                    Debug.Log("[fba] Packaging for device");
                    frameworkPath = Path.Combine(Application.dataPath, deviceFrameworkPath);
                    break;
                case iOSSdkVersion.SimulatorSDK:
                    Debug.Log("[fba] Packaging for simulator");
                    frameworkPath = Path.Combine(Application.dataPath, simulatorFrameworkPath);
                    break;
                default:
                    Debug.LogError("[fba] Unsupported iOS SDK target: " + iosVersion.ToString());
                    return;
            }

            // Add the framework to the project
            string frameworkGuid = project.AddFile(
                Path.Combine(frameworkPath, frameworkName), 
                "Frameworks/" + frameworkName,
                PBXSourceTree.Sdk);
            var defaultTarget = project.TargetGuidByName(PBXProject.GetUnityTargetName());
            project.AddFileToBuild(defaultTarget, frameworkGuid);
            project.UpdateBuildProperty(defaultTarget, "FRAMEWORK_SEARCH_PATHS", new string[]{frameworkPath}, null);
            
            // save the project
            project.WriteToFile(pbxProjectPath);
        }
    }
}
#endif
