using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class TileSystemPainter : MonoBehaviour
{
	[SerializeField] private GameObject _tilePrefab;

	public void DrawTile(int level, int index, float size)
	{
		GameObject tile = Instantiate(_tilePrefab);
		tile.GetComponent<Image>().fillAmount = size / 360;
		tile.transform.Rotate(0, 0, index * size);
		tile.transform.localScale *= level;
	}
}