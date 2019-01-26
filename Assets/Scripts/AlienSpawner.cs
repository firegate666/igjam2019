using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class AlienContainer
{
    public Elements Element;
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

    private List<Elements> _allowedElements = new List<Elements>() { Elements.Fire, Elements.Wood, Elements.Water};
    private Stack<Elements> _availableELements = new Stack<Elements>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }        
    }

    private void RefillElements()
    {
        _availableELements.Clear();

        int size = _allowedElements.Count;
        for (int i = 0; i < size; i++)
        {
            int randomIndex = Random.Range(0, _allowedElements.Count);
            _availableELements.Push(_allowedElements[randomIndex]);
            _allowedElements.Remove(_allowedElements[randomIndex]);
        }

        _allowedElements = _availableELements.ToArray().ToList();
    }
    
    private Elements GetNextElement()
    {
        if (_availableELements.Count <= 0)
        {
            RefillElements();
        }
        return _availableELements.Pop();
    }

    public AlienContainer SpawnAlien()
    {
        var alien = new AlienContainer();
        alien.Element = GetNextElement();
        alien.AlienImage = GetRandomAlien(alien.Element);
        alien.ElementImage = GetRandomIcon(alien.Element);
        return alien;
    }
    
    public AlienContainer[] SpawnAliens(int playerCount)
    {
        var numberOfAliens = playerCount + 1;
        var aliens = new AlienContainer[numberOfAliens];
        for (int i = 0; i < numberOfAliens; i++)
        {
            aliens[i] = SpawnAlien();
        }

        return aliens;
    }

    private Image GetRandomIcon(Elements element)
    {
        if (element == Elements.Fire)
        {
            return FireIcons[Random.Range(0, FireIcons.Length)];
        } 
        else if (element == Elements.Wood)
        {
            return WoodIcon[Random.Range(0, WoodIcon.Length)];
        }
        else if (element == Elements.Water)
        {
            return WaterIcons[Random.Range(0, WaterIcons.Length)];
        }
        
        throw new Exception("invalid element picked " + element);
    }
    
    private Image GetRandomAlien(Elements element)
    {
        if (element == Elements.Fire)
        {
            return FireAliens[Random.Range(0, FireAliens.Length)];
        } 
        else if (element == Elements.Wood)
        {
            return WoodAliens[Random.Range(0, WoodAliens.Length)];
        }
        else if (element == Elements.Water)
        {
            return WaterAliens[Random.Range(0, WaterAliens.Length)];
        }
        
        throw new Exception("invalid element picked " + element);
    }
}
