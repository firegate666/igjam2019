using System;
using System.Collections;
using UnityEngine;

public class CoroutineProvider : MonoBehaviour
{
	private void Start()
	{
		if (Instance != null)
		{
			throw new Exception("only one!");
		}

		Instance = this;
	}

	public void RunCoroutine(IEnumerator coroutine)
	{
		StartCoroutine(coroutine);
	}

	public void StopAll()
	{
		StopAllCoroutines();
	}

	public static CoroutineProvider Instance { get; private set; }
}