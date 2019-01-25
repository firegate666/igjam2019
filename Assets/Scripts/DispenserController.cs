using UnityEngine;

public class DispenserController : MonoBehaviour
{
	[SerializeField] private string InputAxis;
	[SerializeField] private Transform dispenser;
	[SerializeField] private float rotationSpeed;
	[SerializeField] private float dispenserDistance;

	private void Start()
	{
		dispenser.localPosition = transform.position + Vector3.up * dispenserDistance;
	}

	private void Update()
	{
		int moveDirection = Mathf.RoundToInt(Input.GetAxisRaw(InputAxis));
		transform.Rotate(0, 0, rotationSpeed * moveDirection * Time.deltaTime);
	}
}