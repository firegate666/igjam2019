public class SocialIDs
{
#if UNITY_IOS
	public static string LEADERBOARD_CREDITS = "highscore";
	public static string LEADERBOARD_PLANET = "best_planet";
#else
	public static string LEADERBOARD_CREDITS = "highscore";
	public static string LEADERBOARD_PLANET = "best_planet";
#endif
}