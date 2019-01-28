using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class TrackingManager
{
	public const string TIMER_PLANET = "timer_planet";
	public const string TIMER_GAME = "timer_game";
	public const string TIMER_AD = "timer_ad";
	public const string TIMER_HELP = "timer_help";

	static Dictionary<string, float> _timer = new Dictionary<string, float>();

	public static void StartTimer(string name)
	{
		if (_timer.ContainsKey(name))
		{
			Debug.LogWarning("Tried to start a timer twice: " + name);
			return;
		}

		_timer.Add(name, Time.time);
	}

	public static float StopTimer(string name)
	{
		if (!_timer.ContainsKey(name))
		{
			Debug.LogWarning("Tried to stop a non-existing: " + name);
			return 0f;
		}

		var time = Time.time - _timer[name];
		_timer.Remove(name);

		return time;
	}

	public static void NewGame()
	{
		Track("new_game");
	}

	public static void Credits()
	{
		Track("credits");
	}

	public static void Help(string slideName, float duration)
	{
		Track("help", new Dictionary<string, object>() {{"slideName", slideName}, { "duration", duration } });
	}

	public static void Ad(string adName, float duration, Vector2 remoteDistance)
	{
		Track("ad", new Dictionary<string, object>() { { "adName", adName }, { "duration", duration }, { "remoteDistance", remoteDistance } });
	}

	public static void PlanetFinished(int score, int count, float duration, int totalScore)
	{
		Track("planet_finished", new Dictionary<string, object>() { { "score", score }, { "count", count }, { "duration", duration }, { "totalScore", totalScore } });
	}

	public static void GameFinished(int totalScore, float duration, int highestScore)
	{
		Track("game_finished", new Dictionary<string, object>() { { "totalScore", totalScore } , { "duration", duration }, { "highestScore", highestScore } });
	}

	private static void Track(string customEventName)
	{
		Track(customEventName, new Dictionary<string, object> { });
	}

	private static void Track(string customEventName, Dictionary<string, object> eventData)
	{
		eventData.Add("deviceModel", SystemInfo.deviceModel);
		eventData.Add("deviceName", SystemInfo.deviceName);
		eventData.Add("deviceType", SystemInfo.deviceType);
		eventData.Add("deviceUniqueIdentifier", SystemInfo.deviceUniqueIdentifier);
		eventData.Add("operatingSystem", SystemInfo.operatingSystem);
		eventData.Add("operatingSystemFamily", SystemInfo.operatingSystemFamily);

		Debug.Log("Tracking: " + customEventName);
		Debug.Log(eventData);

		//AnalyticsEvent.Custom(customEventName, eventData);
	}
}
