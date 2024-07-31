using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditor.Build.Reporting;

[UsedImplicitly]
public static class Builder
{
    private const char CommandStartCharacter = '-';

    // GENERIC
    private const string ParameterOutputFolder = "-outputFolderName";
    private const string ParameterBuildNumber = "-buildNumber";
    private const string ParameterIl2CPP = "-il2cpp";

    // ANDROID
    private const string ParameterKeystorePath = "-keystorePath";
    private const string ParameterKeystorePassword = "-keystorePass";
    private const string ParameterKeystoreAliasPassword = "-keyaliasPass";
    private const string ParameterKeystoreAliasName = "-keyaliasName";

    // IOS
    private const string ProvisionProfileId = "-provisionUUID";
    private const string AppleTeamID = "-appleTeamID";

    [UsedImplicitly]
    public static void Build()
    {
        switch (EditorUserBuildSettings.activeBuildTarget)
        {
            case BuildTarget.Android:
                BuildAndroid();
                break;
            case BuildTarget.iOS:
                BuildiOS();
                break;
            default:
                BuildDefault();
                break;
        }
    }

    private static void BuildAndroid()
    {
        Dictionary<string, string> commandToValueDictionary = GetCommandLineArguments();
        commandToValueDictionary.TryGetValue(ParameterOutputFolder, out string buildPath);
        commandToValueDictionary.TryGetValue(ParameterKeystorePath, out string keystorePath);
        commandToValueDictionary.TryGetValue(ParameterKeystorePassword, out string keystorePassword);
        commandToValueDictionary.TryGetValue(ParameterKeystoreAliasName, out string keyAliasName);
        commandToValueDictionary.TryGetValue(ParameterKeystoreAliasPassword, out string keyAliasPassword);
        commandToValueDictionary.TryGetValue(ParameterBuildNumber, out string buildNumber);

        if (buildNumber == null)
        {
            throw new ArgumentException("Missing -buildNumber");
        }

        PlayerSettings.Android.bundleVersionCode = int.Parse(buildNumber);

        // setup build system
        EditorUserBuildSettings.buildAppBundle = true;
        EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
        EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;

        // setup keystore
        PlayerSettings.Android.useCustomKeystore = true;
        PlayerSettings.Android.keystoreName = keystorePath;
        PlayerSettings.Android.keystorePass = keystorePassword;
        PlayerSettings.Android.keyaliasName = keyAliasName;
        PlayerSettings.Android.keyaliasPass = keyAliasPassword;

        BuildPlayer(GetEnabledScenePaths(), buildPath, BuildTarget.Android, BuildOptions.None);
    }

    private static void BuildiOS()
    {
        //Parse command line arguments
        Dictionary<string, string> commandToValueDictionary = GetCommandLineArguments();
        commandToValueDictionary.TryGetValue(ParameterOutputFolder, out string buildPath);
        commandToValueDictionary.TryGetValue(ProvisionProfileId, out string provisionUuid);
        commandToValueDictionary.TryGetValue(AppleTeamID, out string appleTeamID);
        commandToValueDictionary.TryGetValue(ParameterBuildNumber, out string buildNumber);

        provisionUuid = "d45af528-e818-48dd-aa45-a231cc742d36";
        appleTeamID = "JLPC23AUSY";
        
        if (buildNumber == null)
        {
            throw new ArgumentException("Missing -buildNumber");
        }

        //Update iOS Manual provisioning profile to Developer or App Store
        PlayerSettings.iOS.appleEnableAutomaticSigning = true;
        //PlayerSettings.iOS.iOSManualProvisioningProfileID = provisionUuid;
        PlayerSettings.iOS.iOSManualProvisioningProfileType = ProvisioningProfileType.Distribution;
        PlayerSettings.iOS.appleDeveloperTeamID = appleTeamID;
        
        PlayerSettings.iOS.buildNumber = buildNumber;
        
        PlayerSettings.SetScriptingBackend (BuildTargetGroup.iOS, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetScriptingDefineSymbolsForGroup (BuildTargetGroup.iOS, new string[] {});

        BuildPlayer(GetEnabledScenePaths(), buildPath, BuildTarget.iOS, BuildOptions.None);
    }

    private static void BuildDefault()
    {
        Dictionary<string, string> commandToValueDictionary = GetCommandLineArguments();
        commandToValueDictionary.TryGetValue(ParameterOutputFolder, out string buildPath);
        commandToValueDictionary.TryGetValue(ParameterIl2CPP, out string il2CPPBuild);
        commandToValueDictionary.TryGetValue(ParameterBuildNumber, out string buildNumber);

        BuildTargetGroup buildTargetGroup = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
        PlayerSettings.bundleVersion = buildNumber;
        PlayerSettings.SetScriptingBackend(buildTargetGroup, il2CPPBuild is "1" ? ScriptingImplementation.IL2CPP : ScriptingImplementation.Mono2x);

        BuildPlayer(GetEnabledScenePaths(), buildPath, EditorUserBuildSettings.activeBuildTarget, BuildOptions.None);
    }

    private static string[] GetEnabledScenePaths()
    {
        return EditorBuildSettings.scenes.Select(e => e.path).ToArray();
    }

    private static Dictionary<string, string> GetCommandLineArguments()
    {
        Dictionary<string, string> commandToValueDictionary = new Dictionary<string, string>();
        string[] args = Environment.GetCommandLineArgs();
        for (int i = 0; i < args.Length; i++)
        {
            if (!args[i].StartsWith(CommandStartCharacter.ToString(), StringComparison.Ordinal))
            {
                continue;
            }

            string command = args[i];
            string value = string.Empty;
            if (i < args.Length - 1 && !args[i + 1].StartsWith(CommandStartCharacter.ToString(), StringComparison.Ordinal))
            {
                value = args[i + 1];
                i++;
            }
            if (!commandToValueDictionary.ContainsKey(command))
                commandToValueDictionary.Add(command, value);
        }
        return commandToValueDictionary;
    }

    private static void BuildPlayer(string[] scenes, string buildPath, BuildTarget buildTarget, BuildOptions buildOptions)
    {
        BuildReport report = BuildPipeline.BuildPlayer(scenes, buildPath, buildTarget, buildOptions);
        BuildSummary summary = report.summary;
        
        if (summary.result == BuildResult.Failed)
        {
            throw new Exception(summary.ToString());
        }
    }
}
