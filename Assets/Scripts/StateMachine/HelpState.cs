using UnityEngine.SceneManagement;

public class HelpState : StateInterface
{
	public void OnLoad()
	{
		SceneManager.LoadSceneAsync("Help", LoadSceneMode.Additive);
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
		SceneManager.UnloadSceneAsync("Help");
	}

	public void OnUnpause()
	{
	}
}
