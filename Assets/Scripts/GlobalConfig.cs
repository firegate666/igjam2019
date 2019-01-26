using System.Collections.Generic;

namespace DefaultNamespace
{
	public static class GlobalConfig
	{
		public const int PlanetBaseLevelSize = 3;
		public const int PlanetLevelHeight = 3;
		public const int CycleSegmentOutlineSoftness = 6;

		public static Dictionary<Elements, Elements> PossibleFusions = new Dictionary<Elements, Elements>
		{
			{Elements.Fire, Elements.Wood},
			{Elements.Wood, Elements.Water},
			{Elements.Water, Elements.Fire}
		};
	}
}