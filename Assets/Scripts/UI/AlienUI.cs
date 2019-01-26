using System;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class AlienUI : MonoBehaviour
{
    private RectTransform _rectTransform;

    public AlienPortraitUI _portrait1;
    public AlienPortraitUI _portrait2;

    private Dictionary<int, AlienPortraitUI> _aliens = new Dictionary<int, AlienPortraitUI>();

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void Reset()
    {
        foreach (var obj in _aliens)
        {
            Destroy(obj.Value.gameObject);
        }
        
        _aliens.Clear();
    }

    public void RemoveAlien(AlienContainer alien)
    {
        Debug.Log("Remove alien " + alien.Id);
        AlienPortraitUI uiAlien;
        _aliens.TryGetValue(alien.Id, out uiAlien);

        if (uiAlien)
        {
            uiAlien.gameObject.SetActive(false);
            _aliens.Remove(alien.Id);
        }
    }

    private AlienPortraitUI GetFreePortrait()
    {
        if (!_portrait1.gameObject.activeSelf)
        {
            return _portrait1;
        } else if (!_portrait2.gameObject.activeSelf)
        {
            return _portrait2;
        }
        
        throw new Exception("only 2 portraits supported");
    }
    
    public void AddAlien(AlienContainer alien)
    {
        AlienPortraitUI alienUI = GetFreePortrait();
        alienUI.AddAlien(alien);
        alienUI.gameObject.SetActive(true);
            
        Debug.Log("Add Alien " + alien.Id);
        _aliens.Add(alien.Id, alienUI);
    }

    public void AddAliens(AlienContainer[] aliens)
    {
        foreach (var alien in aliens)
        {
            AddAlien(alien);
        }
    }
}
