using UnityEngine;
using UnityEngine.UI;

public class TileSystemPainter : MonoBehaviour
{
	[SerializeField] private GameObject _tilePrefab;

	public void DrawTile(int index, float size)
	{
		GameObject tile = Instantiate(_tilePrefab);
		tile.GetComponent<Image>().fillAmount = size / 360;
		tile.transform.Rotate(0, 0, index * size);
	}
}