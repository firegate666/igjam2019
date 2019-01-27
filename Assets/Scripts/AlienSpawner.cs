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
    public int Id;
    public Elements Element;
    public Image AlienImage;
    public Image AlienWinnerImage;
    public Image ElementImage;
    public Image UIElementImage;
}

public class AlienSpawner : MonoBehaviour
{
    public Image[] WaterAliens;
    public Image[] FireAliens;
    public Image[] WoodAliens;

    public Image[] WaterWinnerAliens;
    public Image[] FireWinnerAliens;
    public Image[] WoodWinnerAliens;
    
    public Image[] WaterIcons;
    public Image[] FireIcons;
    public Image[] WoodIcon;
    
    public Image[] WaterUIIcons;
    public Image[] FireUIIcons;
    public Image[] WoodUIIcon;

    public static AlienSpawner Instance;

    private List<Elements> _allowedElements = new List<Elements>() { Elements.Fire, Elements.Wood, Elements.Water};
    private Stack<Elements> _availableELements = new Stack<Elements>();

    private int _nextAlienID = 0;

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
        alien.Id = _nextAlienID++;
        alien.Element = GetNextElement();
        alien.AlienImage = GetRandomAlien(alien.Element);
        alien.AlienWinnerImage = GetRandomWinnerAlien(alien.Element);
        alien.ElementImage = GetRandomIcon(alien.Element);
        alien.UIElementImage = GetRandomUIIcon(alien.Element);
        return alien;
    }
    
    public AlienContainer[] SpawnAliens(int playerCount)
    {
        var numberOfAliens = playerCount + 1;
        var aliens = new AlienContainer[numberOfAliens];
        for (int i = 0; i < numberOfAliens; i++)
        {
            var alien = SpawnAlien();
            aliens[alien.Id] = alien;
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
    
    private Image GetRandomUIIcon(Elements element)
    {
        if (element == Elements.Fire)
        {
            return FireUIIcons[Random.Range(0, FireUIIcons.Length)];
        } 
        else if (element == Elements.Wood)
        {
            return WoodUIIcon[Random.Range(0, WoodUIIcon.Length)];
        }
        else if (element == Elements.Water)
        {
            return WaterUIIcons[Random.Range(0, WaterUIIcons.Length)];
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
    
    private Image GetRandomWinnerAlien(Elements element)
    {
        if (element == Elements.Fire)
        {
            return FireWinnerAliens[Random.Range(0, FireWinnerAliens.Length)];
        } 
        else if (element == Elements.Wood)
        {
            return WoodWinnerAliens[Random.Range(0, WoodWinnerAliens.Length)];
        }
        else if (element == Elements.Water)
        {
            return WaterWinnerAliens[Random.Range(0, WaterWinnerAliens.Length)];
        }
        
        throw new Exception("invalid element picked " + element);
    }
}
