using System.Collections.Generic;
using System.Numerics;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class TileSystemPainter : MonoBehaviour
{
	[SerializeField] private GameObject _tilePrefab;
//	[SerializeField] private Transform _tileContainer;
	
	private List<GameObject> gameObjects; 
	private void Start()
	{
		gameObjects = new List<GameObject>();
		for (int i = 1; i <= GlobalConfig.PlanetLevelHeight; i++)
		{
			gameObjects.Add(Instantiate(new GameObject("Level" + i), transform));
		}
	}

	public void DrawTile(int level, int index, float size)
	{
		GameObject tile = Instantiate(_tilePrefab, gameObjects[level].transform);
		tile.GetComponent<Image>().fillAmount = size / 360;
		tile.transform.Rotate(0, 0, index * size);
		tile.transform.localScale *= level;
	}
}