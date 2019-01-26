using UnityEngine;

public class AlienUI : MonoBehaviour
{
    public AlienUIElement ElementPrefab;
    private RectTransform _rectTransform;

    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    public void AddAliens(AlienContainer[] aliens)
    {
        foreach (var alien in aliens)
        {
            AlienUIElement alienUI = Instantiate(ElementPrefab);
            alienUI.SetAlien(alien);
            RectTransform alientRect = alienUI.GetComponent<RectTransform>();
            alientRect.parent = _rectTransform;
            alientRect.localScale = Vector3.one;
        }

    }
}
