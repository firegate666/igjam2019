using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	public float Speed;

	private Renderer _renderer;

	private void Start()
	{
		_renderer = GetComponent<Renderer>();
	}

	void Update()
	{
		_renderer.material.mainTextureOffset += Vector2.right * Speed * Time.deltaTime;
	}
}