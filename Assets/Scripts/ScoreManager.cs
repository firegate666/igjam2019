using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{    
    public int Score;
    public int PlanetScore;

	private int highestScore;

    public int getTotalScore() => Score;
	public int getHighestScore() => highestScore;

    public void setTotalScore(int newScore) => Score = newScore;

    public void addTotalScore(int change) => Score += change;

    public int getPlanetScore() => PlanetScore;

    public void setPlanetScore(int newScore) => PlanetScore = newScore;

    public void addPlanetScore(int change) => PlanetScore += change;

	void Start()
	{
		DontDestroyOnLoad(gameObject);

		LoadPlayerProgress();
	}

	private void LoadPlayerProgress()
	{
		if (PlayerPrefs.HasKey("highestScore"))
		{
			highestScore = PlayerPrefs.GetInt("highestScore");
		}
		else
		{
			highestScore = 0;
		}
	}

	public void CheckHighscore()
	{
		if (Score > highestScore)
		{
			highestScore = Score;
			PlayerPrefs.SetInt("highestScore", highestScore);
		}
	}
}
