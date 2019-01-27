using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntermissionUI : MonoBehaviour
{
    public GameObject QuotePanel;
    public GameObject StarPanel;
    public GameObject AlienPanel;
    public TextMeshProUGUI ScorePanel;

    public void SetPlanetScore(int score)
    {
        ScorePanel.text = (score * 100).ToString();
    }
}
