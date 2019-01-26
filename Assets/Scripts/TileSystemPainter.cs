using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class TileSystemPainter : MonoBehaviour
{
	[SerializeField] private GameObject _tilePrefab;
	
	private GameObject[] tileContainer; 
	private void Start()
	{
		tileContainer = new GameObject[GlobalConfig.PlanetLevelHeight];
		
		for (int i = GlobalConfig.PlanetLevelHeight; i > 0 ; i--)
		{
			tileContainer[i -1] = Instantiate(new GameObject("Level" + i), transform);
		}
	}

	public void DrawTile(PlanetPiece planetPiece)
	{
		int level = planetPiece.level;
		int index = planetPiece.indexOnRing;
		float size = planetPiece.angleSize;
		
		GameObject tile = Instantiate(_tilePrefab, tileContainer[level-1].transform);
		tile.GetComponent<Image>().fillAmount = size / 360;
		tile.transform.Rotate(0, 0, index * -size);
		tile.transform.localScale *= level;
	}
}