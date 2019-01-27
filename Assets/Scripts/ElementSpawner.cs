using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class ElementSpawner : MonoBehaviour
{
    public GameObject ElementToSpawnPrefab;
    public float _distanceToCenter;
    public float _xOffset;
    public Sprite[] _elemntIcons;
    public bool spawnElements = false;

    private List<GameObject> spawnedElements = new List<GameObject>();

    private List<Elements> _allowedElements = new List<Elements>() {Elements.Fire, Elements.Wood, Elements.Water};

    // Start is called before the first frame update
    void Start()
    {
//        SpawnAtPositionAngle(Elements.Fire, 15f);
//        SpawnAtPositionAngle(Elements.NotSet, 150f);
//        SpawnAtPositionAngle(Elements.Water, 180f);
//        SpawnAtPositionAngle(Elements.Stone, 40f);
//        SpawnAtPositionAngle(Elements.Wood, 200f);
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnElements)
        {
            int activeElements = 0;
            foreach (GameObject obj in spawnedElements)
            {
                if (obj.activeSelf)
                {
                    activeElements += 1;
                }
            }

            //Debug.Log("Active elment " + activeElements);
            if (activeElements < 2)
            {
                float angle = 360 / Random.Range(1, 6);
                Elements randElement = _allowedElements[Random.Range(0, _allowedElements.Count)];
                SpawnAtPositionAngle(randElement, angle);
            }
        }
        else
        {
            foreach (GameObject obj in spawnedElements)
            {
                if (obj.activeSelf)
                {
                    obj.SetActive(false);
                }
            } 
        }
    }

    public void SpawnAtPositionAngle(Elements element, float angle)
    {
        GameObject newElement = Instantiate(ElementToSpawnPrefab, transform);
        newElement.GetComponent<ElementContainer>().element = element;

        float positionX = Mathf.Sin(angle / (180 / Mathf.PI)) * _distanceToCenter;
        float positionY = Mathf.Cos(angle / (180 / Mathf.PI)) * _distanceToCenter;

        newElement.transform.position = new Vector3(positionX + _xOffset, positionY, transform.position.z);

        newElement.GetComponentInChildren<SpriteRenderer>().sprite = _elemntIcons[(int) element];

        spawnedElements.Add(newElement);
    }
}