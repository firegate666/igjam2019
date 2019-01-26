using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienUI : MonoBehaviour
{
    public AlienUIElement ElementPrefab;
    
    public void AddAliens(AlienContainer[] aliens)
    {
        foreach (var alien in aliens)
        {
            AlienUIElement alienUI = Instantiate(ElementPrefab);
            alienUI.SetAlien(alien);
            alienUI.gameObject.transform.parent = transform;
        }

    }
}
