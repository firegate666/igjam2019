using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public float Speed = 1;
    public float MinX = -40;
    public float MaxX = 40;

    public Transform[] BackgroundTiles;

    // Update is called once per frame
    void Update()
    {
        foreach (var tf in BackgroundTiles)
        {
            tf.Translate(new Vector3(1 * Speed * Time.deltaTime, 0, 0));

            if (tf.position.x < MinX)
            {
                tf.position = new Vector3(MaxX, tf.position.y, tf.position.z);
            }
            else if (tf.position.x > MaxX)
            {
                tf.position = new Vector3(MinX, tf.position.y, tf.position.z);
            }
        }
    }
}
