using System;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEditor;

[Serializable]
public class ScreenshotConfiguration
{
#if UNITY_EDITOR    
    public string id;
    public BuildTarget platform;
    public int width;
    public int height;
#endif
}

public class ScreenshotUtility : MonoBehaviour
{
#if UNITY_EDITOR    
    [SerializeField] private string screenshotFolder = "Assets/Screenshots";
    [SerializeField] private string screenshotPrefix = "Screenshot_";
    [SerializeField] private KeyCode screenshotKey = KeyCode.S;

    [SerializeField] private ScreenshotConfiguration[] screenshotSizes = new[]
    {
        new ScreenshotConfiguration() { id = "iPhone 5.5 Portrait", platform = BuildTarget.iOS, width = 1242, height = 2208 }, 
        new ScreenshotConfiguration() { id = "iPhone 6.5 Portrait", platform = BuildTarget.iOS, width = 1242, height = 2688 }, 
        new ScreenshotConfiguration() { id = "iPad Pro Portrait", platform = BuildTarget.iOS, width = 2048, height = 2732 }, 
        new ScreenshotConfiguration() { id = "Android Portrait", platform = BuildTarget.Android, width = 1080, height = 1920 }, 
    };
    
    // The number of screenshots taken
    private int m_ImageCount = 0;

    // The key used to get/set the number of images
    private const string ImageCntKey = "IMAGE_CNT";

    /// <summary>
    /// Lets the screenshot utility persist through scenes.
    /// </summary>
    void Awake () 
    {
        // get image count from player prefs for indexing of filename
        m_ImageCount = PlayerPrefs.GetInt(ImageCntKey);
    }

    private void Start()
    {
        if (screenshotSizes.Length == 0)
        {
            Debug.LogWarning("No sizes set for screenshot utility");
        }

        if (!Directory.Exists(screenshotFolder))
        {
            Debug.LogWarning("Target directory for screenshots does not exist, creating it");
            Directory.CreateDirectory(screenshotFolder);
        }
    }

    void Update ()
    {
        // Checks for input
        if (Input.GetKeyDown(screenshotKey))
        {
            StartCoroutine(TakeScreenshots());
        }
    }

    private IEnumerator TakeScreenshots()
    {
        foreach (var size in screenshotSizes)
        {
            if (size.platform != EditorUserBuildSettings.activeBuildTarget)
            {
                continue;
            }
            
            yield return new WaitForEndOfFrame();
            var found = GameViewUtils.FindAndSetSize(size.width, size.height);

            if (found)
            {
                yield return new WaitForEndOfFrame();


                Texture2D texture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);

                // Read screen contents into the texture
                texture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
                texture.Apply();

                // Write to file
                byte[] bytes = texture.EncodeToPNG();

                PlayerPrefs.SetInt(ImageCntKey, ++m_ImageCount);
                var path = CreatePath(size.id, size.platform.ToString(), size.width, size.height);
                Debug.Log($"Capture screenshot {path}");
                File.WriteAllBytes(path, bytes);

                // Clean up the used texture
                Destroy(texture);
            }
            else
            {
                Debug.LogWarning($"Preferred size for screenshot not found, skipping {size.width}x{size.height}");
            }
        }
    }

    private string CreatePath(string id, string platform, int width, int height)
    {
        PlayerPrefs.SetInt(ImageCntKey, ++m_ImageCount);
        var lang = "en";
        var platformPath = $"{screenshotFolder}/{lang}/{platform}/{id}/";
        if (!Directory.Exists(platformPath))
        {
            Directory.CreateDirectory(platformPath);
        }

        return $"{platformPath}{screenshotPrefix}{m_ImageCount}.png";
    }
#endif    
}