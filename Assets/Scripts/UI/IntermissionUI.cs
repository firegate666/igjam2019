using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntermissionUI : MonoBehaviour
{
    public GameObject QuotePanel;
    public GameObject StarPanel;
    public Image AlienPanel1;
    public Image AlienPanel2;
    public TextMeshProUGUI ScorePanel;

    private int _winnerCount = 0;

    public void SetPlanetScore(int score)
    {
        ScorePanel.text = (score * 100).ToString();
    }

    public void AddWinner(AlienContainer alien)
    {
        if (_winnerCount == 0)
        {
            AlienPanel1.sprite = alien.AlienWinnerImage.sprite;
            _winnerCount++;
            AlienPanel1.gameObject.SetActive(true);
        }
        else
        {
            AlienPanel2.sprite = alien.AlienWinnerImage.sprite;
            AlienPanel2.gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        _winnerCount = 0;
        AlienPanel1.gameObject.SetActive(false);
        AlienPanel2.gameObject.SetActive(false);
    }
}
