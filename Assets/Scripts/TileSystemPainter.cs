using System.Collections;
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
            levelContainer.GetComponent<RectTransform>().SetParent(_rectTransform);
            levelContainer.GetComponent<RectTransform>().localScale = Vector3.one;
            levelContainer.name = "Level " + i;
            _levelContainers[i - 1] = levelContainer;
        }
    }

    public void DrawTile(PlanetPiece planetPiece, bool animate)
    {
        if (planetPiece.element == Elements.NotSet)
        {
            if (planetPiece.viewObject != null)
            {
                Destroy(planetPiece.viewObject);
                planetPiece.viewObject = null;
            }

            return;
        }

        int level = planetPiece.level;
        int index = planetPiece.indexOnRing;
        float size = planetPiece.angleSize;

        GameObject tile = planetPiece.viewObject;
        if (planetPiece.viewObject == null)
        {
            tile = Instantiate(_tilePrefab, _levelContainers[level - 1].transform);
            planetPiece.viewObject = tile;
        }
        Image img = tile.transform.GetChild(0).GetComponent<Image>();
        img.sprite = _elementTextures[(int) planetPiece.element];

        Image mask = tile.GetComponent<Image>();
        mask.fillAmount = size / 360;
        tile.transform.rotation = Quaternion.identity;
        tile.transform.Rotate(0, 0, index * -size);
        tile.transform.GetChild(0).transform.rotation = tile.transform.rotation;
        tile.transform.GetChild(0).transform.Rotate(0, 0, index * size); //Rotate the texture back again

        if (animate && GameManager.Instance.gameState == GameState.Planet)
        {
        StartCoroutine(AnimateTile(tile.transform, level, 1f));
        }
        else
        {
        tile.transform.localScale = Vector3.one * level;
        tile.transform.GetChild(0).transform.localScale = Vector3.one / level;
        }
    }

    public void Reset()
    {
        StopAllCoroutines();
        foreach (GameObject levelContainer in _levelContainers)
        {
            foreach (Transform child in levelContainer.transform)
            {
                Destroy(child.gameObject);
            }
        }
    }

    IEnumerator AnimateTile(Transform tile, int level, float time)
    {
        float elapsedTime = 0;

        while (elapsedTime < time)
        {
            float scalar = (Mathf.Clamp(level - 1, 0.1f,1000f) + (elapsedTime/time));
            tile.transform.localScale = Vector3.one * scalar;
            tile.transform.GetChild(0).transform.localScale = Vector3.one / scalar;

            elapsedTime += Time.deltaTime;
            yield return  new WaitForEndOfFrame();
        }
    }
    
}