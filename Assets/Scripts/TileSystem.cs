using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

class PlanetCycle
{
	public readonly List<PlanetRing> _rings = new List<PlanetRing>();

	public PlanetCycle()
	{
		for (int i = GlobalConfig.PlanetLevelHeight; i > 0; i--)
		{
			_rings.Add(new PlanetRing(i));
		}

		for (int i = 0; i < GlobalConfig.PlanetLevelHeight - 1; i++)
		{
			for (int j = 0; j < _rings[i]._pieces.Count; j++)
			{
				_rings[i]._pieces[j].SetUnderlayingPiece(_rings[i + 1]._pieces[Mathf.FloorToInt(j * .5f)]);
			}
		}
	}

	public PlanetPiece GetFirstFreeAtAngle(float angle)
	{
		PlanetPiece lastPlanetPiece = null;
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

	public bool IsPlanetFull()
	{
		return _rings.TrueForAll(r => r.IsRingFull());
	}

	public int CountElement(Elements element)
	{
		int sum = 0;
		foreach (PlanetRing ring in _rings)
		{
			sum += ring.CountElement(element);
		}

		return sum;
	}
}

class PlanetRing
{
	private readonly int _level;
	private readonly float _anglePerPieceOnThisLevel;
	public readonly List<PlanetPiece> _pieces;

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

	public bool IsRingFull()
	{
		return _pieces.TrueForAll(p => p.element != Elements.NotSet);
	}

	public int CountElement(Elements element)
	{
		int sum = 0;
		foreach (PlanetPiece piece in _pieces)
		{
			sum += piece.element == element ? 1 : 0;
//			PlanetPiece underlayingPiece = piece.GetUnderlayingPiece();
//			if (underlayingPiece != null)
//			{
//				sum += underlayingPiece.element == element ? .5f : 0f;
//			}
		}

		return sum;
	}
}

public class PlanetPiece : IDisposable
{
	public Elements element = Elements.NotSet;
	public readonly int level;
	public readonly float angleSize;
	public readonly int indexOnRing;
	private PlanetPiece underlayingPiece = null;
	public GameObject viewObject;

	public PlanetPiece(int level, float angleSize, int indexOnRing)
	{
		this.level = level;
		this.angleSize = angleSize;
		this.indexOnRing = indexOnRing;
	}

	public bool HasLeftNeighbor()
	{
		return false;
	}

	public PlanetPiece GetUnderlayingPiece()
	{
		return underlayingPiece;
	}

	public void SetUnderlayingPiece(PlanetPiece piece)
	{
		underlayingPiece = piece;
	}

	public void Dispose()
	{
		underlayingPiece = null;
		viewObject = null;
	}
}

public class TileSystem : IDisposable
{
	private readonly PlanetCycle _planetCycle = new PlanetCycle();
	private readonly TileSystemPainter _tileSystemPainter;
	private readonly PlanetOutlinePainter _planetOutlinePainter;

	public TileSystem(TileSystemPainter tileSystemPainter, PlanetOutlinePainter planetOutlinePainter)
	{
		_tileSystemPainter = tileSystemPainter;
		_planetOutlinePainter = planetOutlinePainter;

		//initialize base randomly
		for (int i = 0; i < GlobalConfig.PlanetBaseLevelSize; i++)
		{
			Array elements = Enum.GetValues(typeof(Elements));
			this.DoDrop(i * (360 / GlobalConfig.PlanetBaseLevelSize) + 1f, (Elements) Random.Range(1, elements.Length), false);
		}
	}

	public bool DoDrop(float angle, Elements element, bool showScoreIndicator = true)
	{
		
		PlanetPiece planetPiece = _planetCycle.GetFirstFreeAtAngle(angle);
		if (planetPiece == null)
		{
			return false;
		}

		planetPiece.element = element;
		CoroutineProvider.Instance.RunCoroutine(OperateOnElementsAfterSeconds(planetPiece, .5f, showScoreIndicator)); //plus drawing, now 20% off !

		

		return true;
	}

	IEnumerator OperateOnElementsAfterSeconds(PlanetPiece planetPiece, float waitSec, bool showScoreIndicator = true)
	{
		yield return	new WaitForSeconds(waitSec);
		OperateOnElements(planetPiece, true); //plus drawing, now 20% off !
		
		GameManager.Instance.UpdateScoreText(showScoreIndicator);
		
		if (_planetCycle.IsPlanetFull())
		{
			GameManager.Instance.PlanetFull();
		}
		
	}
	
	private bool OperateOnElements(PlanetPiece planetPiece, bool isFirstRecursionStep = false)
	{
		bool thereWasAReaction = false;
		if (planetPiece.GetUnderlayingPiece() != null)
		{
			thereWasAReaction = TryPerformFusion(
				planetPiece.element, planetPiece.GetUnderlayingPiece().element,
				planetPiece.GetUnderlayingPiece()
			);
			 OperateOnElements(planetPiece.GetUnderlayingPiece());
		}
		_tileSystemPainter.DrawTile(planetPiece, isFirstRecursionStep);
		_planetOutlinePainter.DrawOutLineForPiece(planetPiece);

		return thereWasAReaction;
	}

	private bool TryPerformFusion(Elements dropped, Elements receiver, PlanetPiece receivingPiece)
	{
		Elements possibleFusionElement = Elements.NotSet;
		GlobalConfig.PossibleFusions.TryGetValue(dropped, out possibleFusionElement);
		if (possibleFusionElement == receiver)
		{
			receivingPiece.element = dropped;
			return true;
		}

		return false;
	}

	public int CountElement(Elements elementToCount)
	{
		return _planetCycle.CountElement(elementToCount);
	}

	public void Dispose()
	{
		_planetCycle._rings.ForEach(r => r._pieces.ForEach(p => p.Dispose()));
		_planetOutlinePainter.Reset();
		_tileSystemPainter.Reset();
		CoroutineProvider.Instance.StopAll();
	}
}