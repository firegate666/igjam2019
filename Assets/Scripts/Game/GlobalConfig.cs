using System.Collections.Generic;

namespace DefaultNamespace
{
	public static class GlobalConfig
	{
		public const int PlanetBaseLevelSize = 3;
		public const int PlanetLevelHeight = 3;
		public const int CycleSegmentOutlineSoftness = 6;
		public const int GameplaySeconds = 120;
		public const float FlySpeedInDegPerSec = 140f;
		public const int ElementSpawnSlots = 6;

		// total random values
		// below first value -> 1 star
		// below second value -> 2 stars
		// else 3 stars
		public static int[] StarBounds = new int[] { 500, 1500, int.MaxValue };

		public static int GetStarsFromScore(int score)
		{
			int stars;
			for (stars = 0; stars < StarBounds.Length; stars++)
			{
				if (score < StarBounds[stars])
					break;
			}

			return stars;
		}

		public static Dictionary<Elements, Elements> PossibleFusions = new Dictionary<Elements, Elements>
		{
			{Elements.Fire, Elements.Wood},
			{Elements.Wood, Elements.Water},
			{Elements.Water, Elements.Fire}
		};
	}
}