using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    int _player;
    string horizontalAxis;
    string verticalAxis;
    string dropButtonName;

    public float _positionAngle;

    public GameObject tileToDrop;

    public TextMeshPro PlayerNumberLabel;

    // Start is called before the first frame update
    private void OnEnable()
    {
        horizontalAxis = "Xbox" + _player + "Horizontal";
        verticalAxis = "Xbox" + _player + "Vertical";
        dropButtonName = "Xbox" + _player + "Drop";
    }

    public Transform OrbitPivot;   // The transform that this object will orbit around
    public float OrbitSpeed = 100f;

    public void SetPlayer(int player)
    {
        _player = player;
        PlayerNumberLabel.text = player.ToString();
    }
    
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

            if (Mathf.Abs(deg - _positionAngle) > 0.1f * OrbitSpeed)
            {
                if (_positionAngle < deg && deg - _positionAngle < _positionAngle + 360 - deg) // right
                {
                    _positionAngle += Time.deltaTime * OrbitSpeed;
                } else if (_positionAngle < deg && deg - _positionAngle >= _positionAngle + 360 - deg)
                {
                    _positionAngle -= Time.deltaTime * OrbitSpeed;
                } else if (_positionAngle >= deg && _positionAngle - deg < deg + 360 - _positionAngle)
                {
                    _positionAngle -= Time.deltaTime * OrbitSpeed;
                }
                else
                {
                    _positionAngle += Time.deltaTime * OrbitSpeed;
                }
            }

            if (_positionAngle >= 360)
            {
                _positionAngle -= 360;
            } else if (_positionAngle < 0)
            {
                _positionAngle += 360;
            }

            float positionX = Mathf.Sin(_positionAngle / (180 / Mathf.PI)) * (OrbitPivot.localScale.x / 2);
            float positionY = Mathf.Cos(_positionAngle / (180 / Mathf.PI)) * (OrbitPivot.localScale.x / 2);

            transform.position = new Vector3(positionX, positionY, transform.position.z);
        }

        if (Input.GetButtonDown(dropButtonName) || Input.GetButtonDown("Jump"))
        {
            Debug.Log("Drop button down");

            GameObject tile = Instantiate(tileToDrop, transform.position, Quaternion.identity, null);
            Vector3 lookAt = OrbitPivot.position;
            lookAt.z = transform.position.z;
            tile.transform.LookAt(lookAt);

            FlyingTo fly = tile.GetComponent<FlyingTo>();
            fly.TimeToFly = 2;
            fly.FlyTo(lookAt);
        }
    }
}
