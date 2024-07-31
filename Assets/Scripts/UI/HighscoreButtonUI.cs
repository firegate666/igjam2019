using System;
using UnityEngine;

public class HighscoreButtonUI : MonoBehaviour
{
    public static event Action OnShowLeaderBoard = delegate {};

    public void ShowLeaderboard()
    {
        OnShowLeaderBoard();
    }
}
