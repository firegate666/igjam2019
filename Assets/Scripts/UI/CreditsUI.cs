using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsUI : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetButtonDown("Xbox1Drop"))
        {
            gameObject.SetActive(false);
        }
    }
}
