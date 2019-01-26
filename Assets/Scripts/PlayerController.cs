using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int _player;
    string horizontalAxis;
    string verticalAxis;
    string dropButtonName;
    private float _distanceToCenter;

    public float positionAngle;

	//public GameObject tileToDrop;

	public Elements elementToDrop;

    public TextMeshPro PlayerNumberLabel;

    // Start is called before the first frame update
    private void OnEnable()
    {
        horizontalAxis = "Xbox" + _player + "Horizontal";
        verticalAxis = "Xbox" + _player + "Vertical";
        dropButtonName = "Xbox" + _player + "Drop";

        _distanceToCenter = transform.position.y;
    }

    public Transform OrbitPivot;   // The transform that this object will orbit around
    public float OrbitSpeed = 100f;

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

        transform.position = new Vector3(positionX, positionY, transform.position.z);
    }

	public void SetElementToDrop(Elements element) => elementToDrop = element;

	// Update is called once per frame
	void Update()
    {
        float x = Input.GetAxis(horizontalAxis);
        float y = Input.GetAxis(verticalAxis);

        if (x > 0 || x < 0 || y > 0 || y < 0)
        {
            var rad = Mathf.Atan2(y, x); // In radians
            var deg = rad * (180 / Mathf.PI) + 90;

            if (deg < 0)
            {
                deg = 360 + deg;
            }

            if (Mathf.Abs(deg - positionAngle) > 0.1f * OrbitSpeed)
            {
                if (positionAngle < deg && deg - positionAngle < positionAngle + 360 - deg) // right
                {
                    positionAngle += Time.deltaTime * OrbitSpeed;
                } else if (positionAngle < deg && deg - positionAngle >= positionAngle + 360 - deg)
                {
                    positionAngle -= Time.deltaTime * OrbitSpeed;
                } else if (positionAngle >= deg && positionAngle - deg < deg + 360 - positionAngle)
                {
                    positionAngle -= Time.deltaTime * OrbitSpeed;
                }
                else
                {
                    positionAngle += Time.deltaTime * OrbitSpeed;
                }
            }

            if (positionAngle >= 360)
            {
                positionAngle -= 360;
            } else if (positionAngle < 0)
            {
                positionAngle += 360;
            }

            SetPositionAngle(positionAngle);
        }

        if (Input.GetButtonDown(dropButtonName))
        {
            Debug.Log("Drop button down");

			/*GameObject tile = Instantiate(tileToDrop, transform.position, Quaternion.identity, null);
            Vector3 lookAt = OrbitPivot.position;
            lookAt.z = transform.position.z;
            tile.transform.LookAt(lookAt);

            FlyingTo fly = tile.GetComponent<FlyingTo>();
            fly.FlyTo(lookAt);*/

			GameManager.Instance.doDrop(positionAngle, _player, elementToDrop);
        }
    }
}
