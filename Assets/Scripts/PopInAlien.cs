using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopInAlien : MonoBehaviour
{
    public GameObject AlienPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawn(1));
        StartCoroutine(Spawn(2));
        StartCoroutine(Spawn(3));
    }

    // Update is called once per frame
    IEnumerator Spawn(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        GameObject alien = Instantiate(AlienPrefab);
        alien.transform.parent = transform;
    }
}
