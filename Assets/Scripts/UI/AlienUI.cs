using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class AlienUI : MonoBehaviour
{
    public AlienUIElement ElementPrefab;
    private RectTransform _rectTransform;

    private Dictionary<Elements, RectTransform> _aliens = new Dictionary<Elements, RectTransform>();

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
        RectTransform uiAlien;
        _aliens.TryGetValue(alien.Element, out uiAlien);

        if (uiAlien)
        {
         
            
            Destroy(uiAlien.gameObject);
            _aliens.Remove(alien.Element);
        }
    }

    public void AddAlien(AlienContainer alien)
    {
        AlienUIElement alienUI = Instantiate(ElementPrefab);
        alienUI.SetAlien(alien);
        RectTransform alientRect = alienUI.GetComponent<RectTransform>();
        alientRect.parent = _rectTransform;
        alientRect.localScale = Vector3.one;
            
        _aliens.Add(alien.Element, alientRect);
    }

    public void AddAliens(AlienContainer[] aliens)
    {
        foreach (var alien in aliens)
        {
            AddAlien(alien);
        }
    }
}
