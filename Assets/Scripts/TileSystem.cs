using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlanetCycle
{
	private readonly List<PlanetRing> _rings = new List<PlanetRing>();

	public PlanetCycle()
	{
		for (int i = GlobalConfig.PlanetLevelHeight; i > 0; i--)
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
	private readonly float _anglePerPieceOnThisLevel;
	private readonly List<PlanetPiece> _pieces;

	public PlanetRing(int level)
	{
		_level = level;
		_pieces = new List<PlanetPiece>();

		//total radius / base count / pow2 steps per level (1,2,4,8)->(a third of our cycle) 
		_anglePerPieceOnThisLevel = 360f / GlobalConfig.PlanetBaseLevelSize / Mathf.Pow(2f, _level - 1);

		for (int i = 0; i < GlobalConfig.PlanetBaseLevelSize * Mathf.Pow(2, level - 1); i++)
		{
			_pieces.Add(new PlanetPiece(_level, _anglePerPieceOnThisLevel, i));
		}
	}

	public PlanetPiece GetAtAngle(float angle)
	{
		return _pieces[CalculateIndex(angle)];
	}

	private int CalculateIndex(float angle)
	{
		return Mathf.FloorToInt(angle / _anglePerPieceOnThisLevel);
	}
}

public class PlanetPiece
{
	public Elements element = Elements.NotSet;
	public readonly int level;
	public readonly float angleSize;
	public readonly int indexOnRing;

	public PlanetPiece(int level, float angleSize, int indexOnRing)
	{
		this.level = level;
		this.angleSize = angleSize;
		this.indexOnRing = indexOnRing;
	}

	public bool HasRightNeighbor()
	{
		return true;
	}

	public bool HasLeftNeighbor()
	{
		return false;
	}
}

public class TileSystem
{
	public readonly PlanetCycle planetCycle = new PlanetCycle();
	private readonly TileSystemPainter _tileSystemPainter;
	private readonly PlanetOutlinePainter _planetOutlinePainter;

	public TileSystem(TileSystemPainter tileSystemPainter, PlanetOutlinePainter planetOutlinePainter)
	{
		_tileSystemPainter = tileSystemPainter;
		_planetOutlinePainter = planetOutlinePainter;
	}

	public bool DoDrop(float angle, Elements element)
	{
		Debug.Log("Draw new Tile " + element + " at Angle: " + angle);
		PlanetPiece planetPiece = planetCycle.GetFirstFreeAtAngle(angle);
		if (planetPiece == null)
		{
			return false;
		}

		planetPiece.element = element;

		_tileSystemPainter.DrawTile(planetPiece);
		_planetOutlinePainter.DrawOutLineForPiece(planetPiece);

		return true;
	}

	public int CountElement(Elements elementToCount)
	{
		return (int) elementToCount;
	}
}