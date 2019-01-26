using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class TileSystemPainter : MonoBehaviour
{
	[SerializeField] private GameObject _tilePrefab;
	[SerializeField] private Sprite[] _elementTextures; 
	[SerializeField] private GameObject _levelContainerPrefab;

	private RectTransform _rectTransform;
	
	private GameObject[] _levelContainers;

	private void Start()
	{
		_rectTransform = GetComponent<RectTransform>();
		_levelContainers = new GameObject[GlobalConfig.PlanetLevelHeight];

		for (int i = GlobalConfig.PlanetLevelHeight; i > 0; i--)
		{
			GameObject levelContainer = Instantiate(_levelContainerPrefab);
			levelContainer.GetComponent<RectTransform>().parent = _rectTransform;
			levelContainer.GetComponent<RectTransform>().localScale = Vector3.one;
			levelContainer.name = "Level " + i;
			_levelContainers[i - 1] = levelContainer;
		}
	}

	public void DrawTile(PlanetPiece planetPiece)
	{
		int level = planetPiece.level;
		int index = planetPiece.indexOnRing;
		float size = planetPiece.angleSize;
		
		GameObject tile = Instantiate(_tilePrefab, _levelContainers[level-1].transform);
		Image img = tile.GetComponent<Image>();
		img.sprite = _elementTextures[(int)planetPiece.element];
		img.fillAmount = size / 360;
		tile.transform.Rotate(0, 0, index * -size);
		tile.transform.localScale *= level;
	}
}