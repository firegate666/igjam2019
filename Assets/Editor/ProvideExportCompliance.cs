#if UNITY_IOS
using UnityEngine;
using UnityEditor.Callbacks;
using UnityEditor;
using UnityEditor.iOS.Xcode;
using System.IO;
 
public class ProvideExportCompliance
{
    [PostProcessBuild]
    public static void ChangeXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        // Performs any post build processes that we need done
        if( buildTarget == BuildTarget.iOS )
        {
            // PList modifications
            {
                // Get plist
                string plistPath = pathToBuiltProject + "/Info.plist";
                var plist = new PlistDocument();
                plist.ReadFromString(File.ReadAllText(plistPath));
 
                // Get root
                var rootDict = plist.root;
 
                // Add export compliance for TestFlight builds
                var buildKeyExportCompliance = "ITSAppUsesNonExemptEncryption";
                rootDict.SetString( buildKeyExportCompliance , "false" );
               
                // Write to file
                File.WriteAllText( plistPath , plist.WriteToString() );
            }
        }
    }
}
#endif
