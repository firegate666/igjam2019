using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntermissionUI : MonoBehaviour
{
    public QuotePanelUI QuotePanel;
    public StarPanelUI StarPanel;
    public AlienScorePanelUI AlienPanel;
    public TextMeshProUGUI ScorePanel;


    public void SetPlanetScore(int score)
    {
		int adjustedScore = score * 100;

        ScorePanel.text = adjustedScore.ToString();

		int stars = GlobalConfig.GetStarsFromScore(adjustedScore);
		QuotePanel.SetStars(stars);
		StarPanel.SetStars(stars);
	}

    public void AddWinner(AlienContainer alien)
    {
		AlienPanel.AddWinner(alien);
    }

    private void OnDisable()
    {
		AlienPanel.Reset();
    }
}
