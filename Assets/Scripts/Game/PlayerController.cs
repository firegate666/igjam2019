using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;



public class PlayerController : MonoBehaviour
{
    int _player;
    string horizontalAxisController;
    string verticalAxisController;
    
    string horizontalAxisKeyboard;
    string verticalAxisKeyboard;

    string dropButtonName;
    private float _distanceToCenter;
    private float _xOffset;

    public GameObject StoneIcon;
    public GameObject WaterIcon;
    public GameObject WoodIcon;
    public GameObject FireIcon;
    public GameObject NotSetIcon;
	public GameObject RocketShip;
	public GameObject ProjectilePrefab;

	private Vector3 lastPosition;

    public float positionAngle;
	private float lastPositionAngle;

	//public GameObject tileToDrop;

	public Elements elementToDrop;
	public Elements lastDroppedElement;

    public TextMeshPro PlayerNumberLabel;
    
    // Start is called before the first frame update
    private void OnEnable()
    {
        horizontalAxisController = "Xbox" + _player + "Horizontal";
        horizontalAxisKeyboard = "Keyboard" + _player + "Horizontal";
        verticalAxisController = "Xbox" + _player + "Vertical";
        verticalAxisKeyboard = "Keyboard" + _player + "Vertical";
        dropButtonName = "Xbox" + _player + "Drop";

		RocketShip.SetActive(true);

        _distanceToCenter = transform.position.y;
        _xOffset = transform.position.x;

		elementToDrop = Elements.Stone;
		lastDroppedElement = Elements.NotSet;

		SetElementToDrop(Elements.NotSet);
    }

    public Transform OrbitPivot;   // The transform that this object will orbit around

    private void DeactivateIcons()
    {
	    StoneIcon.SetActive(false);
	    WaterIcon.SetActive(false);
	    WoodIcon.SetActive(false);
	    FireIcon.SetActive(false);
    }
    
    public void SetPlayer(int player)
    {
        _player = player;
        PlayerNumberLabel.text = player.ToString();
    }

    public void SetPositionAngle(float angle)
    {
		positionAngle = angle;
        
        float positionX = Mathf.Sin(positionAngle / (180 / Mathf.PI)) * _distanceToCenter;
        float positionY = Mathf.Cos(positionAngle / (180 / Mathf.PI)) * _distanceToCenter;

		transform.position = new Vector3(positionX + _xOffset, positionY, transform.position.z);

		}


	public void SetElementToDrop(Elements element)
	{
	lastDroppedElement = elementToDrop;
		elementToDrop = element;

		DeactivateIcons();
		switch (element)
		{
			case Elements.Fire:
				FireIcon.SetActive(true);
				break;
			case Elements.Wood:
				WoodIcon.SetActive(true);
				break;
			case Elements.Stone:
				StoneIcon.SetActive(true);
				break;
			case Elements.Water:
				WaterIcon.SetActive(true);
				break;
			case Elements.NotSet:
				NotSetIcon.SetActive(true);
				break;
				
		}
	}

	private void TranslateIcons()
	{
		Vector3 iconPosition = RocketShip.transform.localPosition + RocketShip.transform.up * 1.9f;

		FireIcon.transform.localPosition = iconPosition;
		WaterIcon.transform.localPosition = iconPosition;
		StoneIcon.transform.localPosition = iconPosition;
		WoodIcon.transform.localPosition = iconPosition;
	}

	void Update()
    {
        float xController = Input.GetAxis(horizontalAxisController);
        float yController = Input.GetAxis(verticalAxisController);

        float xKeyboard = Input.GetAxis(horizontalAxisKeyboard);
        float yKeyboard = Input.GetAxis(verticalAxisKeyboard);

        lastPositionAngle = positionAngle;
		lastPosition = RocketShip.transform.position;

		Quaternion targetRotation;
		bool isMoving;

		if (xController > 0 || xController < 0 || yController > 0 || yController < 0)
        {
            HandleController(xController, yController);
            positionAngle = NormalizeAngle(positionAngle);
            SetPositionAngle(positionAngle);
            isMoving = true;
		}
		else if (xKeyboard > 0 || xKeyboard < 0 || yKeyboard > 0 || yKeyboard < 0)
		{
			HandleKeyboard(xKeyboard, yKeyboard);
			positionAngle = NormalizeAngle(positionAngle);
			SetPositionAngle(positionAngle);
			isMoving = true;
		}
		else
		{
			isMoving = false;
		}

		if (isMoving && Mathf.Abs(lastPositionAngle - positionAngle) > 0.1f)
		{
			targetRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(RocketShip.transform.position.x, RocketShip.transform.position.y, 0) - new Vector3(lastPosition.x, lastPosition.y, 0));
		}
		else
		{
			targetRotation = Quaternion.LookRotation(Vector3.forward, new Vector3(_xOffset, 0, 0) - new Vector3(RocketShip.transform.position.x, RocketShip.transform.position.y, 0));
		}
		RocketShip.transform.rotation = Quaternion.Lerp(RocketShip.transform.rotation, targetRotation, Time.deltaTime * 7);
	    TranslateIcons();

		if (Input.GetButtonDown(dropButtonName) && GameManager.Instance.gameState == GameState.Planet)
        {
	        Elements droppedElement = elementToDrop;
	        if (elementToDrop != Elements.NotSet && GameManager.Instance.doDrop(positionAngle, _player, elementToDrop))
	        {
		        ElementProjectile projectile = Instantiate(ProjectilePrefab).GetComponent<ElementProjectile>();
		        projectile.transform.position = RocketShip.transform.position;
		        projectile.SetElement(droppedElement, Vector3.right * _xOffset);

		        elementToDrop = Elements.NotSet;
	        }
        }
    }

	float NormalizeAngle(float angle)
	{
		if (angle >= 360)
		{
			angle -= 360;
		} else if (angle < 0)
		{
			angle += 360;
		}

		return angle;
	}
	
	void HandleKeyboard(float x, float y)
	{
		positionAngle -= x * Time.deltaTime * GlobalConfig.FlySpeedInDegPerSec;
	}

	void HandleController(float x, float y)
	{
		var rad = Mathf.Atan2(y, x); // In radians
		var deg = rad * (180 / Mathf.PI) + 90;

		if (deg < 0)
		{
			deg = 360 + deg;
		}

		if (Mathf.Abs(deg - positionAngle) > 0.1f * GlobalConfig.FlySpeedInDegPerSec)
		{
			if (positionAngle < deg && deg - positionAngle < positionAngle + 360 - deg) // right
			{
				positionAngle += Time.deltaTime * GlobalConfig.FlySpeedInDegPerSec;
			} else if (positionAngle < deg && deg - positionAngle >= positionAngle + 360 - deg)
			{
				positionAngle -= Time.deltaTime * GlobalConfig.FlySpeedInDegPerSec;
			} else if (positionAngle >= deg && positionAngle - deg < deg + 360 - positionAngle)
			{
				positionAngle -= Time.deltaTime * GlobalConfig.FlySpeedInDegPerSec;
			}
			else
			{
				positionAngle += Time.deltaTime * GlobalConfig.FlySpeedInDegPerSec;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//Debug.Log("Collision Enter!");
		if (elementToDrop == Elements.NotSet)
		{
			elementToDrop = other.transform.parent.GetComponent<ElementContainer>().element;
			SetElementToDrop(elementToDrop);
			other.transform.parent.gameObject.SetActive(false);
		}
	}
}
