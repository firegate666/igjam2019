using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class SocialManager : MonoBehaviour
{
	private bool _authenticated;

	private void Start()
    {
        GameManager.OnGameOver += ReportCredits;
        GameManager.OnPlanetFinished += ReportPlanetScore;
        HighscoreButtonUI.OnShowLeaderBoard += OpenLeaderboard;

		var lastAuthenticate = PlayerPrefs.GetInt("AuthenticatedBefore", 0);
		if (lastAuthenticate == 1)
		{
			Authenticate(() =>
			{
				Debug.Log("Auto authenticate");
			});
		}
		else if (lastAuthenticate == 2)
		{
			Debug.Log("Last authenticate failed, no retry");
		}
		else
		{
			Authenticate(() =>
			{
				Debug.Log("Initial authentication");
			});
		}
	}

	/*private void HandleAchievements(int score, float playtime)
	{
		if (playtime >= 60 && score <= 1)
		{
			ReportAchievement(SocialIDs.ACHIEVEMENT_CHEATER);
		}

		if (score >= 10)
		{
			ReportAchievement(SocialIDs.ACHIEVEMENT_SCORE_10);
		}

		if (score == 99)
		{
			ReportAchievement(SocialIDs.ACHIEVEMENT_SO_CLOSE);
		}

		if (score >= 100)
		{
			ReportAchievement(SocialIDs.ACHIEVEMENT_CENTURY);
		}
	}*/

	private void OnDestroy()
	{
        GameManager.OnGameOver -= ReportCredits;
        GameManager.OnPlanetFinished -= ReportPlanetScore;
        HighscoreButtonUI.OnShowLeaderBoard -= OpenLeaderboard;

		//GameManager.GameOver -= HandleAchievements;
	}

	private void Authenticate(Action successCallback)
	{
		ActivatePlayGamesPlatform();
		
		Social.localUser.Authenticate(success =>
		{
			if (success)
			{
				Debug.Log("Authentication successful");
				string userInfo = "Username: " + Social.localUser.userName +
				                  "\nUser ID: " + Social.localUser.id +
				                  "\nIsUnderage: " + Social.localUser.underage;
				Debug.Log(userInfo);
	
				ActivateGameCenterPlatform();
				
				_authenticated = true;

				successCallback();
				
				PlayerPrefs.SetInt("AuthenticatedBefore", 1);
				
				CreateObjects();
			}
			else
			{
				PlayerPrefs.SetInt("AuthenticatedBefore", 2);
				Debug.Log("Authentication failed");
			}
		});
	}

	void CreateObjects()
	{
		// create social leaderboard
		ILeaderboard leaderboardCredits = Social.CreateLeaderboard();
		leaderboardCredits.id = SocialIDs.LEADERBOARD_CREDITS;
		leaderboardCredits.LoadScores(result =>
		{
			Debug.Log("Received " + leaderboardCredits.scores.Length + " scores");
			foreach (IScore score in leaderboardCredits.scores)
				Debug.Log(score);
		});
        
        ILeaderboard leaderboardPlanetScore = Social.CreateLeaderboard();
        leaderboardPlanetScore.id = SocialIDs.LEADERBOARD_PLANET;
        leaderboardPlanetScore.LoadScores(result =>
        {
            Debug.Log("Received " + leaderboardPlanetScore.scores.Length + " scores");
            foreach (IScore score in leaderboardPlanetScore.scores)
                Debug.Log(score);
        });
	}

	void ReportPlanetScore(int planetScore)
    {
        Debug.Log($"Report planet score {planetScore}");
		if (_authenticated)
		{
			Social.ReportScore(planetScore, SocialIDs.LEADERBOARD_PLANET,
				success =>
				{
					Debug.Log(success ? $"Reported planetScore {planetScore} successfully" : $"Failed to report planetScore {planetScore}");
				});
		}
		else
		{
			Debug.LogWarning("planetScore not reported, not authenticated");
		}
	}
    
    void ReportCredits(int credits)
    {
        Debug.Log($"Report credits {credits}");
        if (_authenticated)
        {
            Social.ReportScore(credits, SocialIDs.LEADERBOARD_CREDITS,
                success =>
                {
                    Debug.Log(success ? $"Reported credits {credits} successfully" : $"Failed to report credits {credits}");
                });
        }
        else
        {
            Debug.LogWarning("credits not reported, not authenticated");
        }
    }

	void OpenLeaderboard()
	{
        Debug.Log("Open leaderboard");

		Action callback = () => Social.ShowLeaderboardUI();

		if (_authenticated)
		{
			callback();
		}
		else
		{
			Authenticate(callback);
		}
	}

	void ReportAchievement(string achievementId)
	{
		if (_authenticated)
		{
			Social.ReportProgress(achievementId, 100f, success =>
			{
				Debug.Log(success ? $"Reported achievement {achievementId} successfully" : $"Failed to report achievement {achievementId}");
			});
		}
		else
		{
			Debug.LogWarning($"Achievement {achievementId} not reported, not authenticated");
		}
	}

	void ActivateGameCenterPlatform()
	{
#if UNITY_IOS
		UnityEngine.SocialPlatforms.GameCenter.GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif				
	}
	
	void ActivatePlayGamesPlatform()
	{
#if UNITY_ANDROID		
		GooglePlayGames.BasicApi.PlayGamesClientConfiguration config = new GooglePlayGames.BasicApi.PlayGamesClientConfiguration.Builder().Build();
		GooglePlayGames.PlayGamesPlatform.InitializeInstance(config);
		GooglePlayGames.PlayGamesPlatform.DebugLogEnabled = true;
		GooglePlayGames.PlayGamesPlatform.Activate();
#endif
	}
}
