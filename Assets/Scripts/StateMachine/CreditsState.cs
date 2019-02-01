using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsState : StateInterface
{
	public void OnLoad()
	{
		SceneManager.LoadSceneAsync("Credits", LoadSceneMode.Additive);
	}

	public void OnPause()
	{
	}

	public void OnStart()
	{
	}

	public void OnStop()
	{
	}

	public void OnUnload()
	{
		SceneManager.UnloadSceneAsync("Credits");
	}

	public void OnUnpause()
	{
	}
}
