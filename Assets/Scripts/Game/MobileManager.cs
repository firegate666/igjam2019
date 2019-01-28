using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileManager : MonoBehaviour
{
	public GameObject[] MobileOnlyGameObjects;
	public bool DebugMode;

	private bool _validationFinished;

	// Start is called before the first frame update
	void Start()
    {
		bool enableObjects = DebugMode;
#if UNITY_IOS || UNITY_ANDROID
		enableObjects = true;
#endif
		ValidateObjects(enableObjects);
	}

	void ValidateObjects(bool enableObjects)
	{
		foreach (GameObject go in MobileOnlyGameObjects)
		{
			go.SetActive(enableObjects);
		}
	}
}
