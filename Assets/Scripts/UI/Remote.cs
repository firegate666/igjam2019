using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : MonoBehaviour
{
    public float MinX;
    public float MaxX;
    public float MinY;
    public float MaxY;
    
    public float Speed;

	private float remoteXDistance;
	private float remoteYDistance;

	public Vector2 GetDistanceMoved()
	{
		return new Vector2(remoteXDistance, remoteYDistance);
	}

	private void OnEnable()
	{
		remoteXDistance = 0;
		remoteYDistance = 0;
	}

	// Update is called once per frame
	void Update()
    {
        float x = Input.GetAxis("Xbox1Horizontal");
        float y = Input.GetAxis("Xbox1Vertical");

		remoteXDistance += x;
		remoteYDistance += y;

		transform.Translate(x * Time.deltaTime * Speed, y * Time.deltaTime * -Speed, 0);

        Vector3 newPosition = transform.position;
        
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);        
        newPosition.y = Mathf.Clamp(newPosition.y, MinY, MaxY);        
        
        transform.position = newPosition;
    }
}
