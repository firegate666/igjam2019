using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    public float Speed;
    public bool Clockwise;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Clockwise ? Vector3.back : Vector3.forward, Speed * Time.deltaTime);

    }
}
