using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSystemPainter : MonoBehaviour
{

    [SerializedField] private GameObject _tilePrefab;

    public void DrawTile(int index, float size)
    {
        GameObject tile = Instatiate(_tilePrefab);
        tile.GetComponent<Image>().fillAmount =  size / 360; 
        tile.rotate(index * size);
    }

}
