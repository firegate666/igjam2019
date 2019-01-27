using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using Random = UnityEngine.Random;

public class ElementSpawner : MonoBehaviour
{
    public GameObject ElementToSpawnPrefab;
    private PlayerController _playerController;
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

	int _elementsInSpawning = 0;

    // Update is called once per frame
    void Update()
    {
        if (spawnElements)
        {
            if (_playerController == null)
            {
                _playerController = FindObjectOfType<PlayerController>();
            }

            int activeElements = 0;
            foreach (GameObject obj in spawnedElements)
            {
                if (obj.activeSelf)
                {
                    activeElements += 1;
                }
            }

            //Debug.Log("Active elment " + activeElements);
            if (activeElements + _elementsInSpawning < 2)
            {
                float angle = (360f / 6) * Random.Range(0, 6);
                
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

    private void FixedUpdate()
    {
        List<GameObject> toDelete = new List<GameObject>();
        spawnedElements.ForEach(e =>
        {
            if (e.activeSelf == false)
            {
                toDelete.Add(e);
            }
        });
        for (int i = 0; i < toDelete.Count; i++)
        {
            var e = toDelete[i];
            spawnedElements.Remove(e);
            Destroy(e);
        }
    }

    void SpawnAtPositionAngle(Elements element, float angle)
    {
		float positionX = Mathf.Sin(angle / (180 / Mathf.PI)) * _distanceToCenter;
        float positionY = Mathf.Cos(angle / (180 / Mathf.PI)) * _distanceToCenter;
        Vector3 newPosition = new Vector3(positionX + _xOffset, positionY, transform.position.z);

        if ((_playerController.RocketShip.transform.position -
             new Vector3(positionX, positionY, _playerController.RocketShip.transform.position.z)).magnitude <= 5f)
        {
            return;
        }
        
        foreach (GameObject e in spawnedElements)
        {
            if ((e.transform.position - newPosition).magnitude <= 2f)
            {
                return;
            }
        }

		StartCoroutine(DoSpawn(element, newPosition));
    }

	IEnumerator DoSpawn(Elements element, Vector3 newPosition)
	{
		_elementsInSpawning++;

		yield return new WaitForSeconds(0.5f);

		GameObject newElement = Instantiate(ElementToSpawnPrefab, transform);
		newElement.GetComponent<ElementContainer>().element = element;
		newElement.transform.position = newPosition;

		newElement.GetComponentInChildren<SpriteRenderer>().sprite = _elemntIcons[(int)element];

		spawnedElements.Add(newElement);

		_elementsInSpawning--;
	}
}