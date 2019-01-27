using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Xbox1Drop"))
        {
            SoundManager.Instance.PlayMenuClick();
            GameManager.Instance.Restart();
        }
    }
}
