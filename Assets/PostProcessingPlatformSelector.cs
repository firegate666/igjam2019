using UnityEngine;
using UnityEngine.PostProcessing;

public class PostProcessingPlatformSelector : MonoBehaviour
{
    [SerializeField] private PostProcessingBehaviour postProcessingBehaviour;
    [SerializeField] private PostProcessingProfile profileDesktop;
    [SerializeField] private PostProcessingProfile profileMobile;
    
    void Awake()
    {
#if UNITY_ANDROID || UNITY_IOS
        postProcessingBehaviour.profile = profileMobile;
#else      
        postProcessingBehaviour.profile = profileDesktop;
#endif
    }

    private void OnValidate()
    {
        Awake();
    }
}
