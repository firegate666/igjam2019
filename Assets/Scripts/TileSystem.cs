using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

class PlanetCycle
{
	private readonly List<PlanetRing> _rings = new List<PlanetRing>();

	public PlanetCycle()
	{
		for (int i = GlobalConfig.PlanetLevelHeight; i > 0 ; i--)
		{
			_rings.Add(new PlanetRing(i));
		}
	}

	public PlanetPiece GetFirstFreeAtAngle(float angle)
	{
		PlanetPiece lastPlanetPiece = null; //TODO think!
		for (var index = 0; index < _rings.Count; index++)
		{
			var planetRing = _rings[index];
			PlanetPiece piece = planetRing.GetAtAngle(angle);
			if (piece.element != Elements.NotSet)
			{
				return lastPlanetPiece;
			}
			else if (index == _rings.Count - 1)
			{
				return piece;
			}

			lastPlanetPiece = piece;
		}

		throw new Exception("We did something wrong in the TileSystem list organization....");
	}
}

class PlanetRing
{
	private readonly int _level;

	private readonly List<PlanetPiece> _pieces;

	public PlanetRing(int level)
	{
		_level = level;
		_pieces = new List<PlanetPiece>();
		for (int i = 0; i < GlobalConfig.PlanetBaseLevelSize * Mathf.Pow(2, level - 1); i++)
		{
			_pieces.Add(new PlanetPiece());
		}
	}

	public PlanetPiece GetAtAngle(float angle)
	{
		return _pieces[CalculateIndex(angle)];
	}

	private int CalculateIndex(float angle)
	{
		//total radius / base count / pow2 steps per level (1,2,4,8)->(a third of our cycle) 
		float anglePerPieceOnThisLevel = 360f / GlobalConfig.PlanetBaseLevelSize / Mathf.Pow(2f, _level - 1);
		return Mathf.FloorToInt(angle / anglePerPieceOnThisLevel);
	}
}

class PlanetPiece
{
	public Elements element = Elements.NotSet;
}

public class TileSystem
{
	private readonly PlanetCycle _planetCycle = new PlanetCycle();

	public void DoDrop(float angle, Elements element)
	{
		_planetCycle.GetFirstFreeAtAngle(angle).element = element;
	}
}