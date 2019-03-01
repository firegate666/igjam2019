using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

	private List<SpawnedElement> spawnedElements = new List<SpawnedElement>();

	private List<Elements> _allowedElements = new List<Elements> {Elements.Fire, Elements.Wood, Elements.Water};

	int _elementsInSpawning = 0;

	private float elementSeparationAngle;

	private void Start()
	{
		elementSeparationAngle = 360f / GlobalConfig.ElementSpawnSlots;
	}

	void Update()
	{
		spawnedElements.FindAll(e => e.element.activeSelf == false).ForEach(e => Destroy(e.element));
		spawnedElements = spawnedElements.FindAll(e => e.element.activeSelf).ToList();

		if (GameManager.Instance.gameState == GameState.Planet)
		{
			if (_playerController == null)
			{
				_playerController = FindObjectOfType<PlayerController>();
			}

			int activeElements = spawnedElements.FindAll(e => e.element.activeSelf).Count;

			if (activeElements + _elementsInSpawning < 2)
			{
				float angle = elementSeparationAngle * Random.Range(0, GlobalConfig.ElementSpawnSlots);
				Elements randElement = _allowedElements[Random.Range(0, _allowedElements.Count)];

				if (ValidateSpawnPoint(angle, _playerController.positionAngle))
				{
					SpawnAtPositionAngle(randElement, angle);
				}
			}
		}
		else
		{
			foreach (SpawnedElement obj in spawnedElements)
			{
				obj.element.SetActive(false);
			}
		}
	}

	private bool ValidateSpawnPoint(float angle, float playerAngle)
	{
		return
			Mathf.Abs(Mathf.DeltaAngle(angle, playerAngle)) > elementSeparationAngle -1
			&& spawnedElements.TrueForAll(e => Mathf.Abs(Mathf.DeltaAngle(angle, e.angle)) > elementSeparationAngle -1);
	}

	void SpawnAtPositionAngle(Elements element, float angle)
	{
		float positionX = Mathf.Sin(angle / (180 / Mathf.PI)) * _distanceToCenter;
		float positionY = Mathf.Cos(angle / (180 / Mathf.PI)) * _distanceToCenter;
		Vector3 newPosition = new Vector3(positionX + _xOffset, positionY, transform.position.z);

		GameObject newElement = Instantiate(ElementToSpawnPrefab, transform);
		newElement.GetComponent<ElementContainer>().element = element;
		newElement.transform.position = newPosition;

		newElement.GetComponentInChildren<SpriteRenderer>().sprite = _elemntIcons[(int) element];

		spawnedElements.Add(new SpawnedElement(angle, newElement));

		StartCoroutine(SpawnBlocker());
	}

	IEnumerator SpawnBlocker()
	{
		_elementsInSpawning++;

		yield return new WaitForSeconds(0.5f);

		_elementsInSpawning--;
	}

	private struct SpawnedElement
	{
		public readonly float angle;
		public readonly GameObject element;

		public SpawnedElement(float angle, GameObject element)
		{
			this.angle = angle;
			this.element = element;
		}
	}
}