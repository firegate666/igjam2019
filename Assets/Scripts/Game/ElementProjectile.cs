using UnityEngine;

namespace DefaultNamespace
{
	public class ElementProjectile : MonoBehaviour
	{
		public float speed;
		public Sprite[] projectileSprites;
		public AnimationCurve sizeCurve;
		private Vector3 _target = Vector3.zero;
		private float _distanceAtStart = 5f;
		private float _startSize;

		public void SetElement(Elements element, Vector3 target)
		{
			_startSize = transform.localScale.x;
			GetComponent<SpriteRenderer>().sprite = projectileSprites[((int) element) - 1];
			_distanceAtStart = (target - transform.position).magnitude;
		}

		private void Update()
		{
			Vector3 moveVector = _target - transform.position;
			transform.position += moveVector * speed * Time.deltaTime;
			float currentDistance = moveVector.magnitude;
			if (currentDistance < 0.01f || GameManager.Instance.gameState != GameState.Planet)
			{
				Destroy(gameObject);
			}
			else
			{
				float currentValue = currentDistance / _distanceAtStart;
				transform.localScale = Vector3.one * sizeCurve.Evaluate(currentValue) * _startSize;
			}
		}
	}
}