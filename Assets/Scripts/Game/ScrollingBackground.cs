using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
	public Vector2 Speed;

	private Renderer _renderer;

	private void Start()
	{
		_renderer = GetComponent<Renderer>();
	}

	void Update()
	{
		_renderer.material.mainTextureOffset += Speed * Time.deltaTime;
	}
}