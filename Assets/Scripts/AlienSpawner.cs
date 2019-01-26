using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum AlienElement
{
    Water = 0,
    Fire = 1,
    Wood = 2
}

public class AlienContainer
{
    public AlienElement Element;
    public Image AlienImage;
    public Image ElementImage;
}

public class AlienSpawner : MonoBehaviour
{
    public Image[] WaterAliens;
    public Image[] FireAliens;
    public Image[] WoodAliens;

    public Image[] WaterIcons;
    public Image[] FireIcons;
    public Image[] WoodIcon;

    public static AlienSpawner Instance;

    private Stack<AlienElement> _availableELements = new Stack<AlienElement>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }        
    }

    private void RefillElements()
    {
        foreach (AlienElement elem in Enum.GetValues(typeof(AlienElement)))
        {
            _availableELements.Push(elem);
        }
    }
    
    private AlienElement GetNextElement()
    {
        if (_availableELements.Count == 0)
        {
            RefillElements();
        }
        return _availableELements.Pop();
    }

    public AlienContainer[] SpawnAliens(int playerCount)
    {
        var numberOfAliens = playerCount + 1;
        var aliens = new AlienContainer[numberOfAliens];
        for (int i = 0; i < numberOfAliens; i++)
        {
            var alien = new AlienContainer();
            alien.Element = GetNextElement();
            alien.AlienImage = GetRandomAlien(alien.Element);
            alien.ElementImage = GetRandomIcon(alien.Element);

            aliens[i] = alien;
        }

        return aliens;
    }

    private Image GetRandomIcon(AlienElement element)
    {
        if (element == AlienElement.Fire)
        {
            return FireIcons[Random.Range(0, FireIcons.Length)];
        } 
        else if (element == AlienElement.Wood)
        {
            return WoodIcon[Random.Range(0, WoodIcon.Length)];
        }
        else if (element == AlienElement.Water)
        {
            return WaterIcons[Random.Range(0, WaterIcons.Length)];
        }
        
        throw new Exception("invalid element picked " + element);
    }
    
    private Image GetRandomAlien(AlienElement element)
    {
        if (element == AlienElement.Fire)
        {
            return FireAliens[Random.Range(0, FireAliens.Length)];
        } 
        else if (element == AlienElement.Wood)
        {
            return WoodAliens[Random.Range(0, WoodAliens.Length)];
        }
        else if (element == AlienElement.Water)
        {
            return WaterAliens[Random.Range(0, WaterAliens.Length)];
        }
        
        throw new Exception("invalid element picked " + element);
    }
}
