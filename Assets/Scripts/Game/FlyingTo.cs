using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTo : MonoBehaviour
{

    public float TimeToFly;
    public bool DestroyOnArrive;

    private bool _isActive;
    private Vector3 _targetPosition;

    public void FlyTo(GameObject target)
    {
        if (target.GetComponentInParent<Canvas>() != null)
        {
            // UI element
            _targetPosition = Camera.main.ScreenToWorldPoint(target.transform.position);
        } else
        {
            // no UI element
            _targetPosition = target.transform.position;
        }

        _isActive = true;
    }

    public void FlyTo(Vector3 position)
    {
        _targetPosition = position;
        _isActive = true;
    }

    private void Update()
    {
        if (_isActive)
        {
            iTween.MoveTo(gameObject, iTween.Hash(
                "position", _targetPosition,
                "time", TimeToFly,
                "oncomplete", "OnCompleteMoveTo",
                "easetype", iTween.EaseType.easeOutBounce
            ));
        }
    }

    void OnCompleteMoveTo()
    {
        if (DestroyOnArrive)
        {
            Destroy(gameObject);
        }
    }
}