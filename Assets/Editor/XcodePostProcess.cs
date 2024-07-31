using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using UnityEngine;

public class XcodePostProcess : IPostprocessBuildWithReport
{
    public int callbackOrder => 999;
    public void OnPostprocessBuild(BuildReport report)
    {
        Debug.Log("PostprocessBuild");
        OnPostprocessBuild(report.summary.platform, report.summary.outputPath);
    }
    
    public void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.iOS)
        {
            // Add Game Center capability. Required since Unity and Apple fucked everything up with Xcode 15.
            string projectPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
            PBXProject project = new PBXProject();
            project.ReadFromString(File.ReadAllText(projectPath));

            string mainTarget = project.GetUnityMainTargetGuid();
            
            // Get path to entitlement file (if one was created by Unity) using the Main Target. If the file doesn't exist, we make our own
            string entitlementPath = project.GetEntitlementFilePathForTarget(mainTarget) ?? $"{pathToBuiltProject}/Entitlements.entitlements";

            // Create a new entitlement file if one doesn't exist and populate it with the template
            if(!File.Exists(entitlementPath))
            {
                File.WriteAllText(entitlementPath, _entitlementsTemplate);
            }

            PlistDocument entitlementsDoc = new();
            entitlementsDoc.ReadFromString(File.ReadAllText(entitlementPath));

            // Add the Game Center capability to the root dict of the entitlements file
            PlistElementDict entitlementDict = entitlementsDoc.root;
            entitlementDict.SetBoolean("com.apple.developer.game-center", true);
            File.WriteAllText(entitlementPath, entitlementsDoc.WriteToString());
            
            project.AddFrameworkToProject(mainTarget, "GameKit.framework", false);
            project.AddBuildProperty(mainTarget,"CODE_SIGN_ENTITLEMENTS",entitlementPath);
            File.WriteAllText(projectPath, project.WriteToString());
        }
    }
    
    private static readonly string _entitlementsTemplate =
        @"<?xml version=""1.0"" encoding=""UTF-8""?>
<!DOCTYPE plist PUBLIC ""-//Apple//DTD PLIST 1.0//EN"" ""http://www.apple.com/DTDs/PropertyList-1.0.dtd"">
<plist version=""1.0"">
    <dict>
    </dict>
</plist>";  
}
