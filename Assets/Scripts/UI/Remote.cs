using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remote : MonoBehaviour
{
    public float MinX;
    public float MaxX;
    public float Speed;

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Xbox1Horizontal");
        transform.Translate(x * Time.deltaTime * Speed, 0, 0);

        Vector3 newPosition = transform.position;
        newPosition.x = Mathf.Clamp(newPosition.x, MinX, MaxX);        
        transform.position = newPosition;
    }
}
